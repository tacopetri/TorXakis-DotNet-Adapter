using System;
using System.Collections.Generic;
using System.Linq;
using TorXakisDotNetAdapter.Logging;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// Type-safe implementation of <see cref="ReactiveTransition"/>.
    /// </summary>
    public sealed class ReactiveTransition<T> : ReactiveTransition where T : IAction
    {
        #region Definitions

        /// <summary><see cref="Transition.Action"/></summary>
        public override Type Action { get { return typeof(T); } }

        #endregion
        #region Variables & Properties

        /// <summary><see cref="Transition.ReactiveGuard"/></summary>
        public Func<VariableCollection, T, bool> Guard { get; private set; }

        /// <summary><see cref="Transition.UpdateVariables"/></summary>
        public Action<VariableCollection, T> Update { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public ReactiveTransition(State from, State to, Func<VariableCollection, T, bool> guard, Action<VariableCollection, T> update)
            : base(from, to)
        {
            Guard = guard ?? throw new ArgumentNullException(nameof(guard));
            Update = update ?? throw new ArgumentNullException(nameof(update));
        }

        #endregion
        #region Functionality

        /// <summary><see cref="Transition.ProactiveGuard"/></summary>
        public override bool ProactiveGuard(VariableCollection variables)
        {
            throw new InvalidOperationException("Operation not defined for transition type: " + Type);
        }

        /// <summary><see cref="Transition.ReactiveGuard"/></summary>
        public override bool ReactiveGuard(VariableCollection variables, IAction action)
        {
            return Guard.Invoke(variables, (T)action);
        }

        /// <summary><see cref="Transition.GenerateAction"/></summary>
        public override IAction GenerateAction(VariableCollection variables)
        {
            throw new InvalidOperationException("Operation not defined for transition type: " + Type);
        }

        /// <summary><see cref="Transition.UpdateVariables"/></summary>
        public override void UpdateVariables(VariableCollection variables, IAction action)
        {
            Update.Invoke(variables, (T)action);
        }

        #endregion
    }
}
