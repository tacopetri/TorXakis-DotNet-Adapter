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

        // TODO: Implement!

        #endregion
        #region Variables & Properties

        /// <summary>
        /// The <see cref="System.Type"/> of the associated <see cref="IAction"/> action.
        /// </summary>
        public Type Action { get; private set; }

        /// <summary>
        /// The from <see cref="State"/> state.
        /// </summary>
        public State From { get; private set; }

        /// <summary>
        /// The to <see cref="State"/> state.
        /// </summary>
        public State To { get; private set; }

        /// <summary>
        /// The delegate signature of the <see cref="Update"/>.
        /// </summary>
        public delegate void UpdateDelegate(IAction action, VariableCollection variables);
        /// <summary>
        /// The update function: if this transition is taken, which variables must be updated?
        /// </summary>
        public UpdateDelegate Update { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public Transition(Type action, State from, State to, UpdateDelegate update)
        {
            if (action == null || !typeof(IAction).IsAssignableFrom(action))
                throw new ArgumentException("Invalid action type: " + action, nameof(action));
            Action = action;
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
            Update = update ?? throw new ArgumentNullException(nameof(update));
        }

        /// <summary><see cref="object.ToString"/></summary>
        public override string ToString()
        {
            Type display = null;
            if (typeof(ModelAction).IsAssignableFrom(Action))
                display = typeof(ModelAction);
            else if (typeof(ISystemAction).IsAssignableFrom(Action))
                display = typeof(ISystemAction);

            return Action.Name + " (" + display.Name + ", " + From + " -> " + To + ")";
        }

        #endregion
        #region Functionality

        // TODO: Implement!

        #endregion
    }
}
