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

        /// <summary><see cref="Transition.CheckGuard"/></summary>
        public Func<T, bool> Guard { get; private set; }

        /// <summary><see cref="Transition.PerformUpdate"/></summary>
        public Action<T, VariableCollection> Update { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public ReactiveTransition(State from, State to, Func<T, bool> guard, Action<T, VariableCollection> update)
            : base(from, to)
        {
            Guard = guard ?? throw new ArgumentNullException(nameof(guard));
            Update = update ?? throw new ArgumentNullException(nameof(update));
        }

        #endregion
        #region Functionality

        /// <summary><see cref="Transition.CheckGuard"/></summary>
        public override bool CheckGuard(IAction action)
        {
            return Guard.Invoke((T)action);
        }

        /// <summary><see cref="Transition.PerformGenerate"/></summary>
        public override IAction PerformGenerate(VariableCollection variables)
        {
            throw new InvalidOperationException("Operation not defined for transition type: " + Type);
        }

        /// <summary><see cref="Transition.PerformUpdate"/></summary>
        public override void PerformUpdate(IAction action, VariableCollection variables)
        {
            Update.Invoke((T)action, variables);
        }

        #endregion
    }
}
