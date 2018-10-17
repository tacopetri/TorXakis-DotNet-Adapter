using System;
using System.Collections.Generic;
using System.Linq;
using TorXakisDotNetAdapter.Logging;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// A transition contained in a <see cref="TransitionSystem"/>.
    /// </summary>
    public abstract class Transition
    {
        #region Definitions

        /// <summary>
        /// The associated <see cref="TransitionType"/> value.
        /// </summary>
        public virtual TransitionType Type { get; }

        /// <summary>
        /// The <see cref="System.Type"/> of the associated <see cref="IAction"/> action.
        /// </summary>
        public virtual Type Action { get; }

        #endregion
        #region Variables & Properties

        /// <summary>
        /// The from <see cref="State"/> state.
        /// </summary>
        public State From { get; private set; }

        /// <summary>
        /// The to <see cref="State"/> state.
        /// </summary>
        public State To { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public Transition(State from, State to)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
        }

        /// <summary><see cref="object.ToString"/></summary>
        public override string ToString()
        {
            Type display = null;
            if (typeof(ModelAction).IsAssignableFrom(Action))
                display = typeof(ModelAction);
            else if (typeof(ISystemAction).IsAssignableFrom(Action))
                display = typeof(ISystemAction);

            return Action.Name + " (" + Type + ", " + display.Name + ", " + From + " -> " + To + ")";
        }

        #endregion
        #region Functionality

        /// <summary>
        /// The guard constraint: is this transition valid given the action?
        /// <para>Only valid for <see cref="TransitionType.Reactive"/> transitions!</para>
        /// </summary>
        public abstract bool CheckGuard(IAction action);

        /// <summary>
        /// The generate function: given the variables, which action should be performed?
        /// <para>Only valid for <see cref="TransitionType.Proactive"/> transitions!</para>
        /// </summary>
        public abstract IAction PerformGenerate(VariableCollection variables);

        /// <summary>
        /// The update function: if this transition is taken with the given action, which variables must be updated?
        /// <para>Valid for both <see cref="TransitionType.Reactive"/> and <see cref="TransitionType.Proactive"/> transitions!</para>
        /// </summary>
        public abstract void PerformUpdate(IAction action, VariableCollection variables);

        #endregion
    }
}
