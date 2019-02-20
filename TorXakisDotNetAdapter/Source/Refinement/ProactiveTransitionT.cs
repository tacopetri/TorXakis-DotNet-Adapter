using System;
using System.Collections.Generic;
using System.Linq;
using TorXakisDotNetAdapter.Logging;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// Type-safe implementation of <see cref="ProactiveTransition"/>.
    /// </summary>
    public sealed class ProactiveTransition<T> : ProactiveTransition where T : IAction
    {
        #region Definitions

        /// <summary><see cref="Transition.Action"/></summary>
        public override Type Action { get { return typeof(T); } }

        #endregion
        #region Variables & Properties

        /// <summary><see cref="Transition.ReactiveGuard"/></summary>
        public Func<VariableCollection, bool> Guard { get; private set; }

        /// <summary><see cref="Transition.GenerateAction"/></summary>
        public Func<VariableCollection, T> Generate { get; private set; }

        /// <summary><see cref="Transition.UpdateVariables"/></summary>
        public Action<VariableCollection, T> Update { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public ProactiveTransition(State from, State to, Func<VariableCollection, bool> guard, Func<VariableCollection, T> generate, Action<VariableCollection, T> update)
            : base(from, to)
        {
            Guard = guard ?? throw new ArgumentNullException(nameof(guard));
            Generate = generate ?? throw new ArgumentNullException(nameof(generate));
            Update = update ?? throw new ArgumentNullException(nameof(update));
        }

        #endregion
        #region Functionality

        /// <summary><see cref="Transition.ReactiveGuard"/></summary>
        public override bool ReactiveGuard(VariableCollection variables, IAction action)
        {
            throw new InvalidOperationException("Operation not defined for transition type: " + Type);
        }

        /// <summary><see cref="Transition.ProactiveGuard"/></summary>
        public override bool ProactiveGuard(VariableCollection variables)
        {
            return Guard.Invoke(variables);
        }

        /// <summary><see cref="Transition.GenerateAction"/></summary>
        public override IAction GenerateAction(VariableCollection variables)
        {
            return Generate.Invoke(variables);
        }

        /// <summary><see cref="Transition.UpdateVariables"/></summary>
        public override void UpdateVariables(VariableCollection variables, IAction action)
        {
            Update.Invoke(variables, (T)action);
        }

        #endregion
    }
}
