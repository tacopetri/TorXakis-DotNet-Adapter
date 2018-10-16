using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakisDotNetAdapter.Logging
{
    /// <summary>
    /// Represents a simple <see cref="Log"/> message.
    /// Contains <see cref="UTC"/>, <see cref="Level"/> and <see cref="Message"/>.
    /// </summary>
    public sealed class LogMessage
    {
        #region Definitions

        // TODO: Implement!

        #endregion
        #region Variables & Properties

        /// <summary>
        /// The sender object.
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// The Coordinated Universal Time (UTC).
        /// </summary>
        public DateTime UTC { get; private set; }

        /// <summary>
        /// The <see cref="LogLevel"/> value.
        /// </summary>
        public LogLevel Level { get; private set; }

        /// <summary>
        /// The user-friendly message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// The <see cref="System.Exception"/> that is associated with this event: used for debugging/tracing applications.
        /// </summary>
        public Exception Exception { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public LogMessage(object sender, LogLevel level, string message, Exception exception = null)
        {
            Sender = sender;
            UTC = DateTime.UtcNow;
            Level = level;
            Message = message;
            Exception = exception;
        }

        /// <summary><see cref="Object.ToString"/></summary>
        public override string ToString()
        {
            return "[" + Level.ToString().ToUpperInvariant() + "] " + Message;
        }

        /// <summary>
        /// Verbose variant of <see cref="ToString"/> that includes ALL properties.
        /// </summary>
        public string ToStringFull()
        {
            return "[" + UTC.ToLocalTime().ToString("HH:mm:ss.fff") + "] "
               + "[" + Sender.GetType().Name + "] "
               + "[" + Level.ToString().ToUpperInvariant() + "] "
               + Message
               + (Exception == null ? "" : "\n" + Exception);
        }

        #endregion
    }
}
