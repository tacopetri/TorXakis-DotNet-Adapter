using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// A proactive <see cref="RefinementTransition"/>, emitting either:
    /// <para><see cref="ModelAction"/> outputs to the tester</para>
    /// <para><see cref="ISystemAction"/> commands to the system</para>
    /// </summary>
    public sealed class ProactiveRefinementTransition : RefinementTransition
    {
        #region Base

        /// <summary><see cref="RefinementTransition.Type"/></summary>
        public override ActionType Type { get { return ActionType.Output; } }

        #endregion
        #region Definitions

        // TODO: Implement!

        #endregion
        #region Variables & Properties

        /// <summary>
        /// The delegate signature of the <see cref="Generate"/>.
        /// </summary>
        public delegate IAction GenerateDelegate(List<RefinementVariable> variables);
        /// <summary>
        /// The generate function: given the variables, which action should be emitted?
        /// </summary>
        public GenerateDelegate Generate { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public ProactiveRefinementTransition(string name, RefinementState from, RefinementState to, GenerateDelegate generate, UpdateDelegate update)
            : base(name, from, to, update)
        {
            Generate = generate ?? throw new ArgumentNullException(nameof(generate));
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
