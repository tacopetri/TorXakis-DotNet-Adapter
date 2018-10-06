using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakis.DotNet.Mapping
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
        #region Variables & Properties

        /// <summary>
        /// The user-friendly name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The collection of contained <see cref="SymbolicState"/> states.
        /// </summary>
        public HashSet<SymbolicState> States { get; private set; }

        /// <summary>
        /// The current <see cref="SymbolicState"/> state.
        /// </summary>
        public SymbolicState State { get; private set; }

        /// <summary>
        /// The collection of contained <see cref="SymbolicTransition"/> transitions.
        /// </summary>
        public HashSet<SymbolicTransition> Transitions { get; private set; }

        /// <summary>
        /// The current variables: expressed as named keys bound to values.
        /// </summary>
        public List<SymbolicVariable> Variables { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public SymbolicTransitionSystem(string name, HashSet<SymbolicState> states, SymbolicState state, HashSet<SymbolicTransition> transitions, List<SymbolicVariable> variables)
        {
            // Sanity checks.
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name) + ": " + name);
            if (states == null) throw new ArgumentNullException(nameof(states));
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (!states.Contains(state)) throw new ArgumentException(nameof(state) + ": " + state);
            if (transitions == null) throw new ArgumentNullException(nameof(transitions));
            if (transitions.Any(x => !states.Contains(x.From) || !states.Contains(x.To))) throw new ArgumentException(nameof(transitions) + ": " + transitions);
            if (variables == null) throw new ArgumentNullException(nameof(variables));

            Name = name;
            States = states;
            State = state;
            Transitions = transitions;
            Variables = variables;
        }

        /// <summary><see cref="Object.ToString"/></summary>
        public override string ToString()
        {
            string result = "IOSTS (" + Name + ")";

            result += "\n";
            result += "\n" + nameof(States) + ":";
            foreach (SymbolicState state in States)
                result += "\n\t" + state;

            result += "\n" + nameof(State) + ": " + State;

            result += "\n" + nameof(Transitions) + ":";
            foreach (SymbolicTransition transition in Transitions)
                result += "\n\t" + transition;

            result += "\n" + nameof(Variables) + ":";
            foreach (SymbolicVariable variable in Variables)
                result += "\n\t" + variable;

            return result;
        }

        #endregion
        #region Functionality

        /// <summary>
        /// Handles the given action, which may result in a synchronized transition.
        /// </summary>
        public bool HandleAction(ActionType type, string channel, List<SymbolicVariable> parameters)
        {
            Console.WriteLine(nameof(HandleAction) + " Type: " + type + " Channel: " + channel + " Parameters:\n" + string.Join("\n", parameters.Select(x => x.ToString()).ToArray()));

            List<SymbolicTransition> validTransitions = new List<SymbolicTransition>();
            Dictionary<SymbolicTransition, List<SymbolicVariable>> validAssignments = new Dictionary<SymbolicTransition, List<SymbolicVariable>>();
            foreach (SymbolicTransition transition in Transitions)
            {
                // Transition must come from the current state.
                if (transition.From != State) continue;
                // Transition must have the same type (input or output).
                if (transition.Type != type) continue;
                // Transition must have the same channel name.
                if (transition.Channel != channel) continue;
                // Transition must have the same parameters (but order does not matter).
                //if (!new HashSet<string>(transition.Variables.Select(x => x.Name)).SetEquals(new HashSet<string>(parameters.Select(x => x.Name)))) continue;
                // Transition guard function must evaluate to true.
                List<SymbolicVariable> assignments = transition.Guard(Variables, parameters);
                if (assignments == null) continue;
                Console.WriteLine("Parameter assignments:\n" + string.Join("\n", assignments.Select(x => x.ToString()).ToArray()));

                // All checks passed!
                validTransitions.Add(transition);
                validAssignments[transition] = assignments;
            }

            Console.WriteLine("Valid transitions:\n" + string.Join("\n", validTransitions.Select(x => x.ToString()).ToArray()));

            if (validTransitions.Count == 0) return false;

            // DEBUG: For testing, we pick the first compatible transition, not a random one. (TPE)
            SymbolicTransition chosenTransition = validTransitions.First();
            Console.WriteLine("Chosen transition:\n" + chosenTransition);

            chosenTransition.Update(Variables, validAssignments[chosenTransition]);

            Console.WriteLine("From: " + State + " To: " + chosenTransition.To);
            State = chosenTransition.To;

            return true;
        }

        #endregion
    }
}
