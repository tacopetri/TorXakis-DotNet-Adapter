using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakisDotNetAdapter.Logging
{
    /// <summary>
    /// All defined <see cref="Log"/> levels.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// The DEBUG level. Highly detailed and usually ignored.
        /// </summary>
        Debug = 1,

        /// <summary>
        /// The INFO level. Information about normal program execution.
        /// </summary>
        Info = 2,

        /// <summary>
        /// The WARN level. Unexpected failures that ARE recoverable.
        /// </summary>
        Warn = 3,

        /// <summary>
        /// The ERROR level. Unexpected failures that ARE NOT recoverable.
        /// </summary>
        Error = 4,
    }
}
