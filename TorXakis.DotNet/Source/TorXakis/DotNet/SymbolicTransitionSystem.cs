using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakis.DotNet
{
    /// <summary>
    /// An Input Output Symbolic Transition System (IOSTS).
    /// <para>Consists of <see cref="SymbolicState"/> states and <see cref="SymbolicTransition"/> transitions.</para>
    /// </summary>
    public sealed class SymbolicTransitionSystem
    {
        #region Definitions

        // TODO: Implement!

        #endregion
        #region Template Variables & Properties

        /// <summary>
        /// The user-friendly name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The collection of contained <see cref="SymbolicState"/> states.
        /// </summary>
        public HashSet<SymbolicState> States { get; private set; }

        /// <summary>
        /// The initial <see cref="SymbolicState"/> state.
        /// </summary>
        public SymbolicState InitialState { get; private set; }

        /// <summary>
        /// The collection of contained <see cref="SymbolicTransition"/> transitions.
        /// </summary>
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

        /// <summary>
        /// Handles the given action, which may result in a synchronized transition.
        /// </summary>
        public bool HandleAction(ActionType type, string channel, Dictionary<string, object> parameters)
        {
            Console.WriteLine(nameof(HandleAction) + " Type: " + type + " Channel: " + channel + " Parameters:\n" + string.Join("\n", parameters.Select(x => x.Key + ": " + x.Value).ToArray()));

            List<SymbolicTransition> validTransitions = new List<SymbolicTransition>();
            foreach (SymbolicTransition transition in Transitions)
            {
                // Transition must come from the current state.
                if (transition.From != CurrentState) continue;
                // Transition must have the same type (input or output).
                if (transition.Type != type) continue;
                // Transition must have the same channel name.
                if (transition.Channel != channel) continue;
                // Transition must have the same parameters (but order does not matter).
                if (!transition.Parameters.SetEquals(new HashSet<string>(parameters.Keys))) continue;
                // Transition guard function must evaluate to true.
                if (!transition.GuardFunction(CurrentVariables, parameters)) continue;

                // All checks passed!
                validTransitions.Add(transition);
            }

            Console.WriteLine("Valid transitions:\n" + string.Join("\n", validTransitions.Select(x => x.ToString()).ToArray()));

            if (validTransitions.Count == 0) return false;

            // DEBUG: For testing, we pick the first compatible transition, not a random one. (TPE)
            SymbolicTransition chosenTransition = validTransitions.First();
            Console.WriteLine("Chosen transition:\n" + chosenTransition);

            Dictionary<string, object> updates = chosenTransition.UpdateFunction(CurrentVariables, parameters);
            foreach (KeyValuePair<string, object> kvp in updates)
            {
                Console.WriteLine("Variable: " + kvp.Key + " Old: " + CurrentVariables[kvp.Key] + " New: " + kvp.Value);
                CurrentVariables[kvp.Key] = kvp.Value;
            }
            Console.WriteLine("From: " + CurrentState + " To: " + chosenTransition.To);
            CurrentState = chosenTransition.To;

            return true;
        }

        #endregion
    }
}
