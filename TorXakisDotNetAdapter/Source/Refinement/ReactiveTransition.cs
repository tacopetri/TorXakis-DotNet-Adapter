using System;
using System.Collections.Generic;
using System.Linq;
using TorXakisDotNetAdapter.Logging;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// A reactive <see cref="Transition"/>, responding to either:
    /// <para><see cref="ModelAction"/> inputs from the tester.</para>
    /// <para><see cref="ISystemAction"/> events from the system.</para>
    /// </summary>
    public abstract class ReactiveTransition : Transition
    {
        #region Definitions

        /// <summary><see cref="Transition.Type"/></summary>
        public override TransitionType Type { get { return TransitionType.Reactive; } }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public ReactiveTransition(State from, State to)
            : base(from, to)
        {

        }

        #endregion
    }
}
