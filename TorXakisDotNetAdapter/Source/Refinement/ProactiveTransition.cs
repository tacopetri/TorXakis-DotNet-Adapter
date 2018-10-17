using System;
using System.Collections.Generic;
using System.Linq;
using TorXakisDotNetAdapter.Logging;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// A proactive <see cref="Transition"/>, emitting either:
    /// <para><see cref="ModelAction"/> outputs to the tester.</para>
    /// <para><see cref="ISystemAction"/> commands to the system.</para>
    /// </summary>
    public abstract class ProactiveTransition : Transition
    {
        #region Definitions

        /// <summary><see cref="Transition.Type"/></summary>
        public override TransitionType Type { get { return TransitionType.Proactive; } }

        #endregion
        #region Variables & Properties

        // TODO: Implement!

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public ProactiveTransition(State from, State to)
            : base(from, to)
        {

        }

        #endregion
        #region Functionality

        // TODO: Implement!

        #endregion
    }
}
