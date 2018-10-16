using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// An Input Output Symbolic Transition System (IOSTS).
    /// <para>Consists of <see cref="State"/> states and <see cref="Transition"/> transitions.</para>
    /// </summary>
    public sealed class TransitionSystem
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
        /// The collection of contained <see cref="State"/> states.
        /// </summary>
        public HashSet<State> States { get; private set; }

        /// <summary>
        /// The initial <see cref="State"/> state.
        /// </summary>
        public State InitialState { get; private set; }

        /// <summary>
        /// The current <see cref="State"/> state.
        /// </summary>
        public State CurrentState { get; private set; }

        /// <summary>
        /// The collection of contained <see cref="Transition"/> transitions.
        /// </summary>
        public HashSet<Transition> Transitions { get; private set; }

        /// <summary>
        /// The contained <see cref="VariableCollection"/>, holding all variables.
        /// </summary>
        public VariableCollection Variables { get; private set; } = new VariableCollection();

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public TransitionSystem(string name, HashSet<State> states, State initialState, HashSet<Transition> transitions)
        {
            // Sanity checks.
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name) + ": " + name);
            if (states == null) throw new ArgumentNullException(nameof(states));
            if (initialState == null) throw new ArgumentNullException(nameof(initialState));
            if (!states.Contains(initialState)) throw new ArgumentException(nameof(initialState) + ": " + initialState);
            if (transitions == null) throw new ArgumentNullException(nameof(transitions));
            if (transitions.Any(x => !states.Contains(x.From) || !states.Contains(x.To))) throw new ArgumentException(nameof(transitions) + ": " + transitions);

            Name = name;
            States = states;
            InitialState = initialState;
            CurrentState = initialState;
            Transitions = transitions;
        }

        /// <summary><see cref="Object.ToString"/></summary>
        public override string ToString()
        {
            string result = GetType().Name + " (" + Name + ")";

            result += "\n";
            result += "\n" + nameof(States) + ":";
            foreach (State state in States)
                result += "\n\t" + state;

            result += "\n" + nameof(InitialState) + ": " + InitialState;
            result += "\n" + nameof(CurrentState) + ": " + CurrentState;

            result += "\n" + nameof(Transitions) + ":";
            foreach (Transition transition in Transitions)
                result += "\n\t" + transition;

            result += "\n" + nameof(Variables) + ": " + Variables;

            return result;
        }

        #endregion
        #region Functionality

        /// <summary>
        /// Handles the given action, which may result in a synchronized transition.
        /// </summary>
        public bool HandleAction(ActionType type, IAction action)
        {
            Console.WriteLine(nameof(HandleAction) + " Type: " + type + " Action: " + action);

            List<Transition> validTransitions = new List<Transition>();
            foreach (ReactiveTransition transition in Transitions.Where(x => x is ReactiveTransition))
            {
                // Transition must come from the current state.
                if (transition.From != CurrentState) continue;
                // Transition must have the same type (input or output).
                if (transition.Type != type) continue;
                // Transition guard function must evaluate to true.
                if (!transition.Guard(action)) continue;

                // All checks passed!
                validTransitions.Add(transition);
            }

            Console.WriteLine("Valid transitions:\n" + string.Join("\n", validTransitions.Select(x => x.ToString()).ToArray()));

            if (validTransitions.Count == 0) return false;

            // DEBUG: For testing, we pick the first compatible transition, not a random one. (TPE)
            Transition chosenTransition = validTransitions.First();
            Console.WriteLine("Chosen transition:\n" + chosenTransition);

            chosenTransition.Update(action, Variables);

            Console.WriteLine("From: " + CurrentState + " To: " + chosenTransition.To);
            CurrentState = chosenTransition.To;

            return true;
        }

        #endregion
    }
}
