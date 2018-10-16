using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorXakisDotNetAdapter.Logging
{
    /// <summary>
    /// Static class used to route all log messages.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Signals a new <see cref="LogMessage"/>.
        /// </summary>
        public static event Action<LogMessage> Message;

        /// <summary>
        /// Invokes the <see cref="Message"/> event.
        /// </summary>
        private static void OnMessage(object sender, LogLevel level, string message)
        {
            LogMessage log = new LogMessage(sender, level, message);
            Message?.Invoke(log);
        }

        /// <summary>
        /// Sends the given <see cref="LogLevel.Debug"/> message.
        /// </summary>
        public static void Debug(object sender, string message)
        {
            OnMessage(sender, LogLevel.Debug, message);
        }

        /// <summary>
        /// Sends the given <see cref="LogLevel.Info"/> message.
        /// </summary>
        public static void Info(object sender, string message)
        {
            OnMessage(sender, LogLevel.Info, message);
        }

        /// <summary>
        /// Sends the given <see cref="LogLevel.Warn"/> message.
        /// </summary>
        public static void Warn(object sender, string message)
        {
            OnMessage(sender, LogLevel.Warn, message);
        }

        /// <summary>
        /// Sends the given <see cref="LogLevel.Error"/> message.
        /// </summary>
        public static void Error(object sender, string message)
        {
            OnMessage(sender, LogLevel.Error, message);
        }
    }
}
