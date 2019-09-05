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
        /// The lowest <see cref="LogLevel"/> to pass along.
        /// </summary>
        public static LogLevel Level { get; set; } = LogLevel.Info;

        /// <summary>
        /// Invokes the <see cref="Message"/> event.
        /// </summary>
        private static void OnMessage(object sender, LogLevel level, string message, Exception exception)
        {
            // Ignore messages with a lower level than the current level.
            if (level < Level) return;

            LogMessage log = new LogMessage(sender, level, message, exception);
            Message?.Invoke(log);
        }

        /// <summary>
        /// Sends the given <see cref="LogLevel.Debug"/> message.
        /// </summary>
        public static void Debug(object sender, string message, Exception exception = null)
        {
            OnMessage(sender, LogLevel.Debug, message, exception);
        }

        /// <summary>
        /// Sends the given <see cref="LogLevel.Info"/> message.
        /// </summary>
        public static void Info(object sender, string message, Exception exception = null)
        {
            OnMessage(sender, LogLevel.Info, message, exception);
        }

        /// <summary>
        /// Sends the given <see cref="LogLevel.Warn"/> message.
        /// </summary>
        public static void Warn(object sender, string message, Exception exception = null)
        {
            OnMessage(sender, LogLevel.Warn, message, exception);
        }

        /// <summary>
        /// Sends the given <see cref="LogLevel.Error"/> message.
        /// </summary>
        public static void Error(object sender, string message, Exception exception = null)
        {
            OnMessage(sender, LogLevel.Error, message, exception);
        }
    }
}
