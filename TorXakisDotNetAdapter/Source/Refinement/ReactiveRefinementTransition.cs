using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// A reactive <see cref="RefinementTransition"/>, responding to either:
    /// <para><see cref="ModelAction"/> inputs from the tester</para>
    /// <para><see cref="ISystemAction"/> events from the system</para>
    /// </summary>
    public sealed class ReactiveRefinementTransition : RefinementTransition
    {
        #region Base

        /// <summary><see cref="RefinementTransition.Type"/></summary>
        public override ActionType Type { get { return ActionType.Input; } }

        #endregion
        #region Definitions

        // TODO: Implement!

        #endregion
        #region Variables & Properties

        /// <summary>
        /// The delegate signature of the <see cref="Guard"/>.
        /// </summary>
        public delegate bool GuardDelegate(IAction action);
        /// <summary>
        /// The guard constraint: is this transition valid given the parameter values?
        /// </summary>
        public GuardDelegate Guard { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public ReactiveRefinementTransition(string name, RefinementState from, RefinementState to, GuardDelegate guard, UpdateDelegate update)
            : base(name, from, to, update)
        {
            Guard = guard ?? throw new ArgumentNullException(nameof(guard));
        }

        /// <summary><see cref="Object.ToString"/></summary>
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
        #region Functionality

        // TODO: Implement!

        #endregion

    }
}
