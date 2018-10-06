using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakis.DotNet
{
    /// <summary>
    /// An Input Output Symbolic Transition System (IOSTS).
    /// <para>Consists of <see cref="SymbolicState"/> states and <see cref="SymbolicTransition"/> transitions.</para>
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, IsReference = true)]
    public sealed class SymbolicTransitionSystem
    {
        #region Definitions

        // TODO: Implement!

        #endregion
        #region Template Variables & Properties

        /// <summary>
        /// The user-friendly name.
        /// </summary>
        [JsonProperty]
        public string Name { get; private set; }

        /// <summary>
        /// The collection of contained <see cref="SymbolicState"/> states.
        /// </summary>
        [JsonProperty]
        public HashSet<SymbolicState> States { get; private set; }

        /// <summary>
        /// The initial <see cref="SymbolicState"/> state.
        /// </summary>
        [JsonProperty]
        public SymbolicState InitialState { get; private set; }

        /// <summary>
        /// The collection of contained <see cref="SymbolicTransition"/> transitions.
        /// </summary>
        [JsonProperty]
        public HashSet<SymbolicTransition> Transitions { get; private set; }

        /// <summary>
        /// The initial variables: expressed as named key-value pairs.
        /// </summary>
        public Dictionary<string, object> InitialVariables { get; private set; }

        #endregion
        #region Instance Variables & Properties

        /// <summary>
        /// The current <see cref="SymbolicState"/> state.
        /// </summary>
        [JsonProperty]
        public SymbolicState CurrentState { get; private set; }

        /// <summary>
        /// The current variables: expressed as named key-value pairs.
        /// </summary>
        public Dictionary<string, object> CurrentVariables { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        [JsonConstructor]
        public SymbolicTransitionSystem(string name, HashSet<SymbolicState> states, SymbolicState initialState, HashSet<SymbolicTransition> transitions, Dictionary<string, object> initialVariables)
        {
            // Sanity checks.
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name) + ": " + name);
            if (states == null) throw new ArgumentNullException(nameof(states));
            if (initialState == null) throw new ArgumentNullException(nameof(initialState));
            if (!states.Contains(initialState)) throw new ArgumentException(nameof(initialState) + ": " + initialState);
            if (transitions == null) throw new ArgumentNullException(nameof(transitions));
            if (transitions.Any(x => !states.Contains(x.From) || !states.Contains(x.To))) throw new ArgumentException(nameof(transitions) + ": " + transitions);
            if (initialVariables == null) throw new ArgumentNullException(nameof(initialVariables));

            Name = name;
            States = new HashSet<SymbolicState>(states);
            InitialState = initialState;
            Transitions = new HashSet<SymbolicTransition>(transitions);
            InitialVariables = new Dictionary<string, object>(initialVariables);

            CurrentState = InitialState;
            CurrentVariables = new Dictionary<string, object>(InitialVariables);
        }

        /// <summary><see cref="Object.ToString"/></summary>
        public override string ToString()
        {
            string result = "IOSTS (" + Name + ")";

            result += "\n";
            result += "\n" + nameof(States) + ":";
            foreach (SymbolicState state in States)
                result += "\n\t" + state;

            result += "\n" + nameof(InitialState) + ": " + InitialState;

            result += "\n" + nameof(Transitions) + ":";
            foreach (SymbolicTransition transition in Transitions)
                result += "\n\t" + transition;

            result += "\n" + nameof(InitialVariables) + ":";
            foreach (KeyValuePair<string, object> variable in InitialVariables)
                result += "\n\t" + variable.Key + ": " + variable.Value;

            result += "\n";
            result += "\n" + nameof(CurrentState) + ": " + CurrentState;

            result += "\n" + nameof(CurrentVariables) + ":";
            foreach (KeyValuePair<string, object> variable in CurrentVariables)
                result += "\n\t" + variable.Key + ": " + variable.Value;

            return result;
        }

        #endregion
        #region Functionality

        // TODO: Implement!

        #endregion
    }
}
