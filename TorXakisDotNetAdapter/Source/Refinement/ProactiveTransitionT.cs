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

        /// <summary><see cref="Transition.PerformGenerate"/></summary>
        public Func<VariableCollection, T> Generate { get; private set; }

        /// <summary><see cref="Transition.PerformUpdate"/></summary>
        public Action<T, VariableCollection> Update { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public ProactiveTransition(State from, State to, Func<VariableCollection, T> generate, Action<T, VariableCollection> update)
            : base(from, to)
        {
            Generate = generate ?? throw new ArgumentNullException(nameof(generate));
            Update = update ?? throw new ArgumentNullException(nameof(update));
        }

        #endregion
        #region Functionality

        /// <summary><see cref="Transition.CheckGuard"/></summary>
        public override bool CheckGuard(IAction action)
        {
            throw new InvalidOperationException("Operation not defined for transition type: " + Type);
        }

        /// <summary><see cref="Transition.PerformGenerate"/></summary>
        public override IAction PerformGenerate(VariableCollection variables)
        {
            return Generate.Invoke(variables);
        }

        /// <summary><see cref="Transition.PerformUpdate"/></summary>
        public override void PerformUpdate(IAction action, VariableCollection variables)
        {
            Update.Invoke((T)action, variables);
        }

        #endregion
    }
}
