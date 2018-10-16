using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// A transition contained in a <see cref="RefinementSystem"/>.
    /// </summary>
    public abstract class RefinementTransition
    {
        #region Definitions

        /// <summary>
        /// The <see cref="ActionType"/> value: input or output?
        /// </summary>
        public abstract ActionType Type { get; }

        #endregion
        #region Variables & Properties

        /// <summary>
        /// The user-friendly name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The from <see cref="RefinementState"/> state.
        /// </summary>
        public RefinementState From { get; private set; }

        /// <summary>
        /// The to <see cref="RefinementState"/> state.
        /// </summary>
        public RefinementState To { get; private set; }

        /// <summary>
        /// The delegate signature of the <see cref="Update"/>.
        /// </summary>
        public delegate void UpdateDelegate(IAction action, List<RefinementVariable> variables);
        /// <summary>
        /// The update function: if this transition is taken, which variables must be updated?
        /// </summary>
        public UpdateDelegate Update { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public RefinementTransition(string name, RefinementState from, RefinementState to, UpdateDelegate update)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
            Update = update ?? throw new ArgumentNullException(nameof(update));
        }

        /// <summary><see cref="Object.ToString"/></summary>
        public override string ToString()
        {
            return GetType().Name + " (" + Name + ") " + From + " -> " + To;
        }

        #endregion
        #region Functionality

        // TODO: Implement!

        #endregion
    }
}
