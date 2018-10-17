using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// Transitions are categorized into REACTIVE and PROACTIVE transitions.
    /// </summary>
    public enum TransitionType
    {
        /// <summary>
        /// A reactive transition,
        /// responding to a <see cref="ModelAction"/> input or <see cref="ISystemAction"/> event.
        /// </summary>
        Reactive,

        /// <summary>
        /// A proactive transition,
        /// emitting a new <see cref="ModelAction"/> output or <see cref="ISystemAction"/> command.
        /// </summary>
        Proactive,
    }
}
