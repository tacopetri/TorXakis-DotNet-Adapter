﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakisDotNetAdapter.Legacy.Mapping
{
    /// <summary>
    /// A transition contained in a <see cref="SymbolicTransitionSystem"/>.
    /// </summary>
    public sealed class SymbolicTransition
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
        /// The from <see cref="SymbolicState"/> state.
        /// </summary>
        public SymbolicState From { get; private set; }

        /// <summary>
        /// The to <see cref="SymbolicState"/> state.
        /// </summary>
        public SymbolicState To { get; private set; }

        /// <summary>
        /// The <see cref="ActionType"/> value: input or output?
        /// </summary>
        public ActionType Type { get; private set; }

        /// <summary>
        /// The channel name to communicate on.
        /// </summary>
        public string Channel { get; private set; }

        /// <summary>
        /// The set of variables (values to be decided during transitions).
        /// </summary>
        public List<SymbolicVariable> Variables { get; private set; }

        /// <summary>
        /// The delegate signature of the <see cref="Guard"/>.
        /// </summary>
        public delegate List<SymbolicVariable> GuardDelegate(List<SymbolicVariable> variables, List<SymbolicVariable> parameters);
        /// <summary>
        /// The guard constraint: is this transition valid given the parameter values?
        /// </summary>
        public GuardDelegate Guard { get; private set; }

        /// <summary>
        /// The delegate signature of the <see cref="Update"/>.
        /// </summary>
        public delegate void UpdateDelegate(List<SymbolicVariable> variables, List<SymbolicVariable> parameters);
        /// <summary>
        /// The update function: if this transition is taken, which variables must be updated?
        /// </summary>
        public UpdateDelegate Update { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public SymbolicTransition(string name, SymbolicState from, SymbolicState to, ActionType type, string channel,
            List<SymbolicVariable> variables,
            GuardDelegate guard,
            UpdateDelegate update)
        {
            // Sanity checks.
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name) + ": " + name);
            if (string.IsNullOrEmpty(channel)) throw new ArgumentException(nameof(channel) + ": " + channel);

            Name = name;
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));

            Type = type;
            Channel = channel;
            Variables = variables ?? throw new ArgumentNullException(nameof(variables));
            Guard = guard;
            Update = update ?? throw new ArgumentNullException(nameof(update));
        }

        /// <summary><see cref="object.ToString"/></summary>
        public override string ToString()
        {
            string result = "Transition (" + Name + ") " + From + " -> " + To;
            result += "\n\t\t" + nameof(Type) + ": " + Type;
            result += "\n\t\t" + nameof(Channel) + ": " + Channel;

            result += "\n\t\t" + nameof(Variables) + ": " + string.Join(", ", Variables.Select(x => x.Name).ToArray());

            result += "\n\t\t" + nameof(Guard) + ": " + Guard.ToString();
            result += "\n\t\t" + nameof(Update) + ": " + Update;

            return result;
        }

        #endregion
        #region Functionality

        // TODO: Implement!

        #endregion
    }
}