using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using TorXakisDotNetAdapter.Logging;

namespace TorXakisDotNetAdapter
{
    /// <summary>
    /// Provides the TCP connection to the TorXakis test tool.
    /// Parses the <see cref="TorXakisModel"/> for defined <see cref="TorXakisConnection"/> channels.
    /// </summary>
    public sealed class TorXakisConnector : IDisposable
    {
        #region Variables & Properties

        /// <summary>
        /// A lock object to make this class thread-safe.
        /// </summary>
        private readonly object locker = new object();

        /// <summary>
        /// Log the TorXakis console output to the <see cref="Trace"/> channel?
        /// Can be disabled if the <see cref="ConsoleOutputReceived"/> and <see cref="ConsoleErrorReceived"/> are hooked.
        /// </summary>
        public bool LogConsoleToTrace { get; set; } = true;

        /// <summary>
        /// The <see cref="TorXakisModel"/> to test.
        /// </summary>
        public TorXakisModel Model { get; private set; }

        /// <summary>
        /// The managed <see cref="TorXakisConnection"/> instances.
        /// </summary>
        private readonly Dictionary<int, TorXakisConnection> connections = new Dictionary<int, TorXakisConnection>();

        /// <summary>
        /// The set of user-friendly input channel names.
        /// </summary>
        public IEnumerable<string> InputChannels
        {
            get
            {
                foreach (TorXakisConnection connection in connections.Values)
                    if (!string.IsNullOrEmpty(connection.InputChannel))
                        yield return connection.InputChannel;
            }
        }

        /// <summary>
        /// The set of user-friendly output channel names.
        /// </summary>
        public IEnumerable<string> OutputChannels
        {
            get
            {
                foreach (TorXakisConnection connection in connections.Values)
                    if (!string.IsNullOrEmpty(connection.OutputChannel))
                        yield return connection.OutputChannel;
            }
        }

        /// <summary>
        /// The managed <see cref="Process"/> instance.
        /// </summary>
        private Process process;

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public TorXakisConnector(FileInfo model)
        {
            // Sanity checks.
            if (model == null || !model.Exists)
                throw new ArgumentException(nameof(model) + ": " + model);

            Model = new TorXakisModel(model);
            ParseModel();

            Log.Info(this, "Created: " + this);
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~TorXakisConnector()
        {
            // Make sure to always shut down cleanly!
            Stop();
        }

        /// <summary>
        /// Start the adapter.
        /// </summary>
        public void Start()
        {
            lock (locker)
            {
                foreach (TorXakisConnection connection in connections.Values)
                {
                    connection.Start();
                }

                StartTorXakis();
            }
        }

        /// <summary>
        /// Stop the adapter.
        /// </summary>
        public void Stop()
        {
            lock (locker)
            {
                foreach (TorXakisConnection connection in connections.Values)
                {
                    connection.Stop();
                }

                if (process != null)
                {
                    if (!process.HasExited) process.CloseMainWindow();
                    process = null;
                }
            }
        }

        /// <summary><see cref="IDisposable.Dispose"/></summary>
        public void Dispose()
        {
            // We use Start and Stop to setup and cleanup.
            Stop();
        }

        #endregion
        #region Events

        /// <summary>
        /// Signals that an output line has been received from the underlying <see cref="process"/>.
        /// </summary>
        public event Action<string> ConsoleOutputReceived;

        /// <summary>
        /// Callback for all output lines coming from the underlying <see cref="process"/>.
        /// </summary>
        private void OnConsoleOutputReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;

            if (LogConsoleToTrace) Log.Info(this, "Info: " + e.Data);
            ConsoleOutputReceived?.Invoke(e.Data);
        }

        /// <summary>
        /// Signals that an error line has been received from the underlying <see cref="process"/>.
        /// </summary>
        public event Action<string> ConsoleErrorReceived;

        /// <summary>
        /// Callback for all errors coming from the underlying <see cref="process"/>.
        /// </summary>
        private void OnConsoleErrorReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;

            if (LogConsoleToTrace) Log.Error(this, "Error: " + e.Data);
            ConsoleErrorReceived?.Invoke(e.Data);
        }

        /// <summary>
        /// Signals that the test runner executable has been started and is waiting for commands.
        /// </summary>
        public event Action Started;

        /// <summary>Fires the event: <see cref="Started"/></summary>
        private void OnStarted()
        {
            Log.Info(this, "Started: " + this);
            Started?.Invoke();
        }

        /// <summary>
        /// Signals that input has been received from the test runner.
        /// </summary>
        public event Action<TorXakisAction> InputReceived;

        /// <summary>Fires the event: <see cref="InputReceived"/></summary>
        private void OnInputReceived(TorXakisAction action)
        {
            // Sanity checks.
            if (action.Type != ActionType.Input)
                throw new ArgumentException("Not input: " + action);

            Log.Info(this, nameof(InputReceived) + ": " + action);
            InputReceived?.Invoke(action);
        }

        #endregion
        #region Functionality

        /// <summary><see cref="object.ToString"/></summary>
        public override string ToString()
        {
            return GetType().Name
                + "\n\t" + nameof(Model) + ": " + Model.File.Name
                + "\n\t" + nameof(InputChannels) + ": " + string.Join(", ", InputChannels.ToArray())
                + "\n\t" + nameof(OutputChannels) + ": " + string.Join(", ", OutputChannels.ToArray());
        }

        /// <summary>
        /// Parses the <see cref="Model"/> file for all required information (such as the defined TCP channels).
        /// </summary>
        private void ParseModel()
        {
            Dictionary<int, List<string>> parsed = Model.ParseConnections();

            // Create the parsed connections.
            foreach (KeyValuePair<int, List<string>> kvp in parsed)
            {
                TorXakisConnection connection = new TorXakisConnection(kvp.Key, kvp.Value[0], kvp.Value[1]);
                AddConnection(connection);
            }
        }

        /// <summary>
        /// Launches the external test runner executable.
        /// </summary>
        private void StartTorXakis()
        {
            // Prepare the process start.
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "torxakis.exe",
                WorkingDirectory = Model.File.Directory.FullName,
                Arguments = "\"" + Model.File.Name + "\"",

                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Maximized,
            };

            Log.Info(this, "Starting process: " + startInfo.FileName + "\n" + "Working directory: " + startInfo.WorkingDirectory + "\n" + "Arguments: " + startInfo.Arguments);

            process = new Process()
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true,
            };

            process.OutputDataReceived += OnConsoleOutputReceived;
            process.ErrorDataReceived += OnConsoleErrorReceived;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            OnStarted();
        }

        #endregion
        #region Connections

        /// <summary>
        /// Adds the given <see cref="TorXakisConnection"/> to <see cref="connections"/>.
        /// </summary>
        private void AddConnection(TorXakisConnection connection)
        {
            if (connections.TryGetValue(connection.Port, out TorXakisConnection existing))
                throw new ArgumentException("Port is already taken by: " + existing);

            connections.Add(connection.Port, connection);
            connection.StateChanged += Connection_StateChanged;
            connection.InputReceived += Connection_InputReceived;
        }

        /// <summary>
        /// Adds the given <see cref="TorXakisConnection"/> to <see cref="connections"/>.
        /// </summary>
        private void RemoveConnection(TorXakisConnection connection)
        {
            if (!connections.ContainsKey(connection.Port))
                throw new ArgumentException("Port is not registered: " + connection);

            connections.Remove(connection.Port);
            connection.StateChanged -= Connection_StateChanged;
            connection.InputReceived -= Connection_InputReceived;
        }

        /// <summary>
        /// Callback for <see cref="TorXakisConnection.StateChanged"/>.
        /// </summary>
        private void Connection_StateChanged(TorXakisConnection connection)
        {
            // Once all connections have been established, we are ready to begin the test run.
            if (connections.All(x => x.Value.State == TorXakisConnection.States.Connected))
            {
                // TODO!
            }
        }

        /// <summary>
        /// Callback for <see cref="TorXakisConnection.InputReceived"/>.
        /// </summary>
        private void Connection_InputReceived(TorXakisAction action)
        {
            OnInputReceived(action);
        }

        #endregion
        #region Message Format

        /// <summary>
        /// Sends the given output, in a blocking fashion.
        /// </summary>
        public bool SendOutput(TorXakisAction action)
        {
            lock (locker)
            {
                // Sanity checks.
                if (action.Type != ActionType.Output)
                    throw new ArgumentException("Not output: " + action);

                TorXakisConnection connection = connections.Values.FirstOrDefault(x => x.OutputChannel == action.Channel);
                if (connection == null) throw new Exception("Connection not found for: " + action);
                Log.Info(this, nameof(SendOutput) + ": " + action);
                return connection.SendOutput(action);
            }
        }

        #endregion
        #region Console Commands

        /// <summary>
        /// Sends the given console command, in a blocking fashion.
        /// </summary>
        public bool SendConsoleCommand(string command)
        {
            lock (locker)
            {
                if (process != null)
                {
                    try
                    {
                        process.StandardInput.WriteLine(command);
                        Log.Info(this, nameof(SendConsoleCommand) + ": " + command);
                        return true;
                    }
                    catch (Exception e)
                    {
                        Log.Error(this, nameof(SendConsoleCommand) + " Exception!", e);
                        return false;
                    }
                }
                else return false;
            }
        }

        #endregion
    }
}
