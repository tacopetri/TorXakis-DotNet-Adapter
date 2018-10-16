using System;
using System.Collections.Generic;
using System.Linq;
using TorXakisDotNetAdapter.Logging;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// A proactive <see cref="Transition"/>, emitting either:
    /// <para><see cref="ModelAction"/> outputs to the tester</para>
    /// <para><see cref="ISystemAction"/> commands to the system</para>
    /// </summary>
    public sealed class ProactiveTransition : Transition
    {
        #region Base

        /// <summary><see cref="Transition.Type"/></summary>
        public override ActionType Type { get { return ActionType.Output; } }

        #endregion
        #region Definitions

        // TODO: Implement!

        #endregion
        #region Variables & Properties

        /// <summary>
        /// The delegate signature of the <see cref="Generate"/> function.
        /// </summary>
        public delegate IAction GenerateDelegate(VariableCollection variables);
        /// <summary>
        /// The generate function: given the variables, which action should be performed?
        /// </summary>
        public GenerateDelegate Generate { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public ProactiveTransition(string name, State from, State to, GenerateDelegate generate, UpdateDelegate update)
            : base(name, from, to, update)
        {
            Generate = generate ?? throw new ArgumentNullException(nameof(generate));
        }

        /// <summary><see cref="object.ToString"/></summary>
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
