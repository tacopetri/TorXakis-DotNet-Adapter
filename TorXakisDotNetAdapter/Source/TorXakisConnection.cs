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

namespace TorXakisDotNetAdapter
{
    /// <summary>
    /// A single TorXakis TCP connection. The set of connections are managed by the <see cref="TorXakisAdapter"/>.
    /// <para>https://github.com/TorXakis/TorXakis</para>
    /// </summary>
    internal sealed class TorXakisConnection : IDisposable
    {
        #region Definitions

        // TODO: Implement!

        #endregion
        #region Variables & Properties

        /// <summary>
        /// A lock object to make this class thread-safe.
        /// </summary>
        private readonly object locker = new object();

        /// <summary>
        /// The assigned TCP port.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// The user-friendly input channel name.
        /// </summary>
        public string InputChannel { get; private set; }

        /// <summary>
        /// The user-friendly output channel name.
        /// </summary>
        public string OutputChannel { get; private set; }

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
        /// The managed <see cref="StreamReader"/> instance.
        /// </summary>
        private StreamReader streamReader;

        /// <summary>
        /// The managed <see cref="StreamWriter"/> instance.
        /// </summary>
        private StreamWriter streamWriter;

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public TorXakisConnection(int port, string inputChannel, string outputChannel)
        {
            // Sanity checks.
            if (port < 0) throw new ArgumentException(nameof(port) + ": " + port);

            Port = port;
            InputChannel = inputChannel;
            OutputChannel = outputChannel;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~TorXakisConnection()
        {
            // Make sure to always shut down cleanly!
            Stop();
        }

        /// <summary>
        /// Start the instance.
        /// </summary>
        public void Start()
        {
            lock (locker)
            {
                if (State != States.Created) return;

                if (thread == null)
                {
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
        }

        /// <summary>
        /// Stop the instance.
        /// </summary>
        public void Stop()
        {
            lock (locker)
            {
                if (thread != null)
                {
                    thread.Abort();
                    thread = null;

                    Dispose();
                }
            }
        }

        /// <summary><see cref="IDisposable.Dispose"/></summary>
        public void Dispose()
        {
            lock (locker)
            {
                if (tcpListener == null && tcpClient == null && streamReader == null && streamWriter == null)
                    return;

                if (tcpListener != null)
                {
                    tcpListener.Stop();
                    tcpListener = null;
                }
                if (streamReader != null)
                {
                    streamReader.Dispose();
                    streamReader = null;
                }
                if (streamWriter != null)
                {
                    streamWriter.Dispose();
                    streamWriter = null;
                }
                if (tcpClient != null)
                {
                    tcpClient.Close();
                    tcpClient.Dispose();
                    tcpClient = null;
                }

                GoToState(States.Disconnected);
                GoToState(States.Stopped);
                GoToState(States.Disposed);
            }
        }

        #endregion
        #region Events

        /// <summary>
        /// Signals that input has been received from the test runner.
        /// </summary>
        public event Action<TorXakisAction> InputReceived;

        /// <summary>Fires the event:<see cref="InputReceived"/></summary>
        private void OnInputReceived(TorXakisAction action)
        {
            InputReceived?.Invoke(action);
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
                GoToState(States.Started);

                tcpListener = new TcpListener(IPAddress.Any, Port);
                tcpListener.Start();
                Log("Waiting for TCP connection: " + tcpListener.LocalEndpoint);

                tcpClient = tcpListener.AcceptTcpClient();
                tcpClient.NoDelay = true;
                Log("Connected TCP client: " + tcpListener.LocalEndpoint);

                NetworkStream stream = tcpClient.GetStream();
                // TorXakis expects UTF-8 encoding without BOM!
                streamReader = new StreamReader(stream, new UTF8Encoding(false));
                streamWriter = new StreamWriter(stream, new UTF8Encoding(false));

                GoToState(States.Connected);

                while (true)
                {
                    string data = ReceiveInput();
                    TorXakisAction action = TorXakisAction.FromInput(InputChannel, data);
                    OnInputReceived(action);
                }
            }
            catch (ThreadAbortException)
            {
                // This exception is normal when shutting down.
            }
            catch (Exception e)
            {
                Log(nameof(ThreadLoop) + " exception: " + this, e);
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
            return GetType().Name + " " + nameof(Port) + ": " + Port + " " + nameof(InputChannel) + ": " + InputChannel + " " + nameof(OutputChannel) + ": " + OutputChannel;
        }

        /// <summary>
        /// Logs the given message, which automatically appends accurate timing information.
        /// </summary>
        private void Log(string message, Exception e = null)
        {
            string line = "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "] " + message;
            Trace.WriteLine(line + "\n" + e);
        }

        #endregion
        #region Message Format

        /// <summary>
        /// Reads the next test input, in a blocking fashion.
        /// </summary>
        private string ReceiveInput()
        {
            if (streamReader != null)
            {
                return streamReader.ReadLine();
            }
            else return null;
        }

        /// <summary>
        /// Sends the given output, in a blocking fashion.
        /// </summary>
        public bool SendOutput(TorXakisAction action)
        {
            lock (locker)
            {
                if (streamWriter != null)
                {
                    try
                    {
                        // TorXakis expects \n terminated lines (Linux), not \r\n terminated lines (Windows)!
                        streamWriter.Write(action.Data + "\n");
                        streamWriter.Flush();
                        return true;
                    }
                    catch (Exception e)
                    {
                        Log(nameof(SendOutput) + " exception: " + this, e);
                        return false;
                    }
                }
                else return false;
            }
        }

        #endregion
        #region State Machine

        /// <summary>
        /// The possible states for this object.
        /// </summary>
        public enum States
        {
            /// <summary>
            /// The link has been created, but not yet started.
            /// </summary>
            Created,
            /// <summary>
            /// The link has been started, but not yet connected.
            /// </summary>
            Started,
            /// <summary>
            /// The link has been connected, and is ready for use.
            /// </summary>
            Connected,
            /// <summary>
            /// The link has been disconnected: it can no longer be used.
            /// </summary>
            Disconnected,
            /// <summary>
            /// The link has been stopped: it can no longer be used.
            /// </summary>
            Stopped,
            /// <summary>
            /// The link has been disposed: it can no longer be used.
            /// </summary>
            Disposed,
        }

        /// <summary>
        /// The current <see cref="States"/> value.
        /// </summary>
        public States State { get; private set; } = States.Created;

        /// <summary>
        /// Signals that the <see cref="States"/> value has chaged.
        /// </summary>
        public event Action<TorXakisConnection> StateChanged;

        /// <summary>
        /// Fires the <see cref="StateChanged"/>event.
        /// </summary>
        private void OnStateChanged()
        {
            Log(State + ": " + this);
            StateChanged?.Invoke(this);
        }

        /// <summary>
        /// Go to the given state, if possible.
        /// </summary>
        private void GoToState(States destination)
        {
            // Start?
            if (State == States.Created && destination == States.Started)
            {
                State = destination;
                OnStateChanged();
            }
            // Connect?
            else if (State == States.Started && destination == States.Connected)
            {
                State = destination;
                OnStateChanged();
            }
            // Disconnect?
            else if (State == States.Connected && destination == States.Disconnected)
            {
                State = destination;
                OnStateChanged();
            }
            // Stop?
            else if (State == States.Disconnected && destination == States.Stopped)
            {
                State = destination;
                OnStateChanged();
            }
            // Dispose?
            else if (State == States.Stopped && destination == States.Disposed)
            {
                State = destination;
                OnStateChanged();
            }
        }

        #endregion
    }
}
