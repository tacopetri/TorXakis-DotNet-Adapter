using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UppaalTron.DotNet
{
    /// <summary>
    /// A test adapter for UPPAAL TRON.
    /// <para>http://people.cs.aau.dk/~marius/tron/manual.pdf</para>
    /// </summary>
    public class UppaalTronAdapter : IDisposable
    {
        #region Log

        /// <summary>
        /// The <see cref="ILog"/> for this class.
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
        #region Definitions

        /// <summary>
        /// The spacing used between log parts.
        /// </summary>
        private const string Space = " ";

        /// <summary>
        /// The different commands as defined by UPPAAL TRON.
        /// </summary>
        private enum Commands : byte
        {
            SA_InpEnc = 1,

            SA_OutEnc = 2,

            SA_VarToInp = 3,

            SA_VarToOut = 4,

            SA_TimeUnit = 5,

            SA_Timeout = 6,

            SA_TestExec = 64,

            SA_GetError = 127,
        }

        /// <summary>
        /// The special bit used to signal ACK messages by UPPAAL TRON.
        /// </summary>
        private const int Ack = (1 << 31);

        #endregion
        #region Variables & Properties

        /// <summary>
        /// The TCP port to listen on.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// The UPPAAL model to test.
        /// </summary>
        public FileInfo Model { get; private set; }

        /// <summary>
        /// The command line arguments passed to the TRON executable.
        /// </summary>
        public string Arguments { get; private set; }

        /// <summary>
        /// The managed <see cref="Thread"/> instance.
        /// </summary>
        private Thread thread;

        /// <summary>
        /// The managed <see cref="TcpListener"/> instance.
        /// </summary>
        private TcpListener tcpListener;

        /// <summary>
        /// The managed <see cref="TcpClient"/> instance.
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// The managed <see cref="BinaryReader"/> instance.
        /// Note that UPPAAL TRON uses the Big-Endian format, so we use <see cref="IPAddress.NetworkToHostOrder(int)"/> and its variants.
        /// </summary>
        private BinaryReader reader;

        /// <summary>
        /// The managed <see cref="BinaryWriter"/> instance.
        /// Note that UPPAAL TRON uses the Big-Endian format, so we use <see cref="IPAddress.HostToNetworkOrder(int)"/> and its variants.
        /// </summary>
        private BinaryWriter writer;

        /// <summary>
        /// The managed <see cref="Process"/> instance.
        /// </summary>
        private Process process;

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public UppaalTronAdapter(int port, FileInfo model, string arguments)
        {
            // Sanity checks.
            if (port < 0) throw new ArgumentException(nameof(port));
            if (model == null || !model.Exists) throw new ArgumentException(nameof(model));

            Port = port;
            Model = model;
            Arguments = arguments;

            log.Info("Creating: " + this);
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~UppaalTronAdapter()
        {
            // Make sure to always shut down cleanly!
            Stop();
        }

        /// <summary>
        /// Start the adapter.
        /// </summary>
        public void Start()
        {
            if (thread == null)
            {
                log.Info("Starting: " + this);

                thread = new Thread(new ThreadStart(ThreadLoop))
                {
                    Name = GetType().FullName,
                    IsBackground = true,
                    Priority = ThreadPriority.Highest,

                    CurrentCulture = CultureInfo.InvariantCulture,
                    CurrentUICulture = CultureInfo.InvariantCulture,
                };
                thread.Start();
            }
        }

        /// <summary>
        /// Stop the adapter.
        /// </summary>
        public void Stop()
        {
            if (thread != null)
            {
                log.Info("Stopping: " + this);

                thread.Abort();
                thread = null;
            }
            Dispose();
        }

        /// <summary><see cref="IDisposable.Dispose"/></summary>
        public void Dispose()
        {
            try
            {
                if (tcpListener != null)
                {
                    tcpListener.Stop();
                    tcpListener = null;
                }
                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }
                if (writer != null)
                {
                    writer.Dispose();
                    writer = null;
                }
                if (tcpClient != null)
                {
                    tcpClient.Close();
                    tcpClient.Dispose();
                    tcpClient = null;
                }
                if (process != null)
                {
                    if (!process.HasExited) process.CloseMainWindow();
                    process = null;
                }
            }
            catch (Exception e)
            {
                log.Warn(nameof(Dispose) + " Exception!", e);
            }
        }

        #endregion
        #region Thread Loop

        /// <summary>
        /// The main thread loop: first accepting a connection, then listening for incoming messages.
        /// </summary>
        private void ThreadLoop()
        {
            try
            {
                StartTron();

                tcpListener = new TcpListener(IPAddress.Any, Port);
                tcpListener.Start();
                log.Info("Waiting for TCP connection: " + tcpListener.LocalEndpoint);

                tcpClient = tcpListener.AcceptTcpClient();
                tcpClient.NoDelay = true;
                log.Info("Connected TCP client: " + tcpListener.LocalEndpoint);

                NetworkStream stream = tcpClient.GetStream();
                reader = new BinaryReader(stream);
                writer = new BinaryWriter(stream);

                TronConnected?.Invoke();

                while (true)
                {
                    KeyValuePair<int, int[]> kvp = ReadInput();
                    InputReceived?.Invoke(kvp.Key, kvp.Value);
                }
            }
            catch (ThreadAbortException)
            {
                // This exception is normal when shutting down.
            }
            catch (Exception e)
            {
                Trace.WriteLine(GetType().Name + " " + nameof(ThreadLoop) + " Exception!" + "\n" + e);
                log.Warn(nameof(ThreadLoop) + " Exception!", e);
            }
            finally
            {
                Dispose();
            }
        }

        #endregion
        #region Functionality

        /// <summary><see cref="object.ToString"/></summary>
        public override string ToString()
        {
            return GetType().Name + " Port (" + Port + ") Model (" + Model.FullName + ") Arguments (" + Arguments + ")";
        }

        /// <summary>
        /// Runs the external TRON executable.
        /// </summary>
        private void StartTron()
        {
            // Prepare the process start.
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "tron.exe",
                WorkingDirectory = Model.Directory.FullName,
                Arguments = Arguments,

                UseShellExecute = false,
                RedirectStandardInput = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Maximized,
            };

            log.Info("Starting process: " + startInfo.FileName + "\n" + "Working directory: " + startInfo.WorkingDirectory + "\n" + "Arguments: " + startInfo.Arguments);

            process = new Process()
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true,
            };

            process.OutputDataReceived += OnTronOutputReceived;
            process.ErrorDataReceived += OnTronErrorReceived;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

        }

        /// <summary>
        /// Signals that an output has been received from the TRON process.
        /// </summary>
        public event Action<string> TronOutputReceived;

        /// <summary>
        /// Signals that an error has been received from the TRON process.
        /// </summary>
        public event Action<string> TronErrorReceived;

        /// <summary>
        /// Callback for all output coming from the TRON process.
        /// </summary>
        private void OnTronOutputReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;

            log.Info("Received TRON output: " + e.Data);
            TronOutputReceived?.Invoke(e.Data);
        }

        /// <summary>
        /// Callback for all errors coming from the TRON process.
        /// </summary>
        private void OnTronErrorReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;

            log.Info("Received TRON error: " + e.Data);
            TronErrorReceived?.Invoke(e.Data);
        }

        /// <summary>
        /// Signals that the TRON executable has been connected to the adapter.
        /// </summary>
        public event Action TronConnected;

        /// <summary>
        /// Signals that test input has been received from TRON.
        /// </summary>
        public event Action<int, int[]> InputReceived;

        #endregion
        #region Message Format

        private string ReadString()
        {
            int length = reader.ReadByte();
            if (length > 0)
            {
                byte[] buffer = new byte[length];
                reader.Read(buffer, 0, length);
                return Encoding.UTF8.GetString(buffer);
            }
            else return null;
        }

        private void WriteString(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            writer.Write((byte)bytes.Length);
            writer.Write(bytes);
            writer.Flush();
        }

        private string GetErrorMessage(int errorCode)
        {
            log.Info("Getting error message for error code: " + errorCode);
            writer.Write((byte)Commands.SA_GetError);
            writer.Write(IPAddress.HostToNetworkOrder(errorCode));
            writer.Flush();

            string result = ReadString();
            log.Info("Result: " + result);
            return result;
        }

        /// <summary>
        /// Sets the time unit (in micro-seconds).
        /// </summary>
        public void SetTimeUnit(long microSeconds)
        {
            log.Info("Setting time unit: " + microSeconds + " microseconds");
            writer.Write((byte)Commands.SA_TimeUnit);
            writer.Write(IPAddress.HostToNetworkOrder(microSeconds));
            writer.Flush();

            int result = IPAddress.NetworkToHostOrder(reader.ReadInt32());
            log.Info("Result: " + result);
            if (result < 0)
            {
                log.Info("Error! Code: " + result + " Message: " + GetErrorMessage(result));
            }
        }

        /// <summary>
        /// Set the time-out (in time units).
        /// </summary>
        public void SetTimeOut(int timeUnits)
        {
            log.Info("Setting time out: " + timeUnits + " time units");
            writer.Write((byte)Commands.SA_Timeout);
            writer.Write(IPAddress.HostToNetworkOrder(timeUnits));
            writer.Flush();

            int result = IPAddress.NetworkToHostOrder(reader.ReadInt32());
            log.Info("Result: " + result);
            if (result < 0)
            {
                log.Info("Error! Code: " + result + " Message: " + GetErrorMessage(result));
            }
        }

        /// <summary>
        /// Registers an input channel (by name).
        /// </summary>
        public int AddInput(string channel)
        {
            log.Info("Adding input channel: " + channel);
            writer.Write((byte)Commands.SA_InpEnc);
            WriteString(channel);
            writer.Flush();

            int result = IPAddress.NetworkToHostOrder(reader.ReadInt32());
            log.Info("Result: " + result);
            if (result < 0)
            {
                log.Info("Error! Code: " + result + " Message: " + GetErrorMessage(result));
            }
            return result;
        }

        /// <summary>
        /// Registers an output channel (by name).
        /// </summary>
        public int AddOutput(string channel)
        {
            log.Info("Adding output channel: " + channel);
            writer.Write((byte)Commands.SA_OutEnc);
            WriteString(channel);
            writer.Flush();

            int result = IPAddress.NetworkToHostOrder(reader.ReadInt32());
            log.Info("Result: " + result);
            if (result < 0)
            {
                log.Info("Error! Code: " + result + " Message: " + GetErrorMessage(result));
            }
            return result;
        }

        /// <summary>
        /// Registers a variable (by name) for an input channel (by id).
        /// </summary>
        public int AddVariableToInput(int channel, string variable)
        {
            log.Info("Adding variable to input: " + channel + " " + variable);
            writer.Write((byte)Commands.SA_VarToInp);
            writer.Write(IPAddress.HostToNetworkOrder(channel));
            WriteString(variable);
            writer.Flush();

            int result = IPAddress.NetworkToHostOrder(reader.ReadInt32());
            log.Info("Result: " + result);
            if (result < 0)
            {
                log.Info("Error! Code: " + result + " Message: " + GetErrorMessage(result));
            }
            return result;
        }

        /// <summary>
        /// Registers a variable (by name) for an output channel (by id).
        /// </summary>
        public int AddVariableToOutput(int channel, string variable)
        {
            log.Info("Adding variable to output: " + channel + " " + variable);
            writer.Write((byte)Commands.SA_VarToOut);
            writer.Write(IPAddress.HostToNetworkOrder(channel));
            WriteString(variable);
            writer.Flush();

            int result = IPAddress.NetworkToHostOrder(reader.ReadInt32());
            log.Info("Result: " + result);
            if (result < 0)
            {
                log.Info("Error! Code: " + result + " Message: " + GetErrorMessage(result));
            }
            return result;
        }

        /// <summary>
        /// Starts the test run!
        /// </summary>
        public string StartTest()
        {
            log.Info("Starting test!");
            writer.Write((byte)Commands.SA_TestExec);
            writer.Flush();

            string result = ReadString();
            log.Info("Result: " + result);
            return result;
        }

        private KeyValuePair<int, int[]> ReadInput()
        {
            int channel = IPAddress.NetworkToHostOrder(reader.ReadInt32());
            if ((channel & Ack) != 0)
            {
                channel &= ~Ack;

                log.Info("Received ACK: " + channel);
                return new KeyValuePair<int, int[]>(channel, null);
            }
            else
            {
                short n = IPAddress.NetworkToHostOrder(reader.ReadInt16());
                int[] variables = new int[n];
                for (int i = 0; i < n; i++)
                    variables[i] = IPAddress.NetworkToHostOrder(reader.ReadInt32());

                string message = "Received input: " + channel;
                if (variables != null && variables.Length > 0)
                    message += " (" + string.Join(", ", variables.Select(x => x.ToString()).ToArray()) + ")";
                log.Info(message);

                SendAck(1);
                return new KeyValuePair<int, int[]>(channel, variables);
            }
        }

        /// <summary>
        /// Sends the given SUT output (by channel id), with parameters (by variable ids).
        /// </summary>
        public bool SendOutput(int channel, params int[] variables)
        {
            if (writer != null)
            {
                string message = "Sending output: " + channel;
                if (variables != null && variables.Length > 0)
                    message += " (" + string.Join(", ", variables.Select(x => x.ToString()).ToArray()) + ")";
                log.Info(message);

                writer.Write(IPAddress.HostToNetworkOrder(channel));
                writer.Write(IPAddress.HostToNetworkOrder((short)variables.Length));
                foreach (int variable in variables)
                    writer.Write(IPAddress.HostToNetworkOrder(variable));
                writer.Flush();

                return true;
            }
            else return false;
        }

        private bool SendAck(int count)
        {
            if (writer != null)
            {
                log.Info("Sending ACK: x" + count);

                writer.Write(IPAddress.HostToNetworkOrder(Ack | count));
                writer.Flush();

                return true;
            }
            else return false;
        }

        #endregion
    }
}
