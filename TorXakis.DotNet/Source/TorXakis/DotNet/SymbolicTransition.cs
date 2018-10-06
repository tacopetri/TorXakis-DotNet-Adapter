﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakis.DotNet
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
        /// The set of parameters.
        /// </summary>
        public HashSet<string> Parameters { get; private set; }

        /// <summary>
        /// The delegate signature of the <see cref="GuardFunction"/>.
        /// </summary>
        public delegate bool GuardDelegate(Dictionary<string, object> variables, Dictionary<string, object> parameters);
        /// <summary>
        /// The guard function: is this transition valid given the parameter values?
        /// </summary>
        public GuardDelegate GuardFunction { get; private set; }

        /// <summary>
        /// The delegate signature of the <see cref="UpdateFunction"/>.
        /// </summary>
        public delegate Dictionary<string, object> UpdateDelegate(Dictionary<string, object> variables, Dictionary<string, object> parameters);
        /// <summary>
        /// The update function: if this transition is taken, which variables must be updated?
        /// </summary>
        public UpdateDelegate UpdateFunction { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public SymbolicTransition(string name, SymbolicState from, SymbolicState to, ActionType type, string channel,
            HashSet<string> parameters,
            GuardDelegate guardFunction,
            UpdateDelegate updateFunction)
        {
            // Sanity checks.
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name) + ": " + name);
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(from));
            if (string.IsNullOrEmpty(channel)) throw new ArgumentException(nameof(channel) + ": " + channel);
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (guardFunction == null) throw new ArgumentNullException(nameof(guardFunction));
            if (updateFunction == null) throw new ArgumentNullException(nameof(updateFunction));

            Name = name;
            From = from;
            To = to;

            Type = type;
            Channel = channel;
            Parameters = new HashSet<string>(parameters);
            GuardFunction = guardFunction;
            UpdateFunction = updateFunction;
        }

        /// <summary><see cref="Object.ToString"/></summary>
        public override string ToString()
        {
            string result = "Transition (" + Name + ") " + From + " => " + To;
            result += "\n\t\t" + nameof(Type) + ": " + Type;
            result += "\n\t\t" + nameof(Channel) + ": " + Channel;

            result += "\n\t\t" + nameof(Parameters) + ": " + string.Join(", ", Parameters.ToArray());

            result += "\n\t\t" + nameof(GuardFunction) + ": " + GuardFunction;
            result += "\n\t\t" + nameof(UpdateFunction) + ": " + UpdateFunction;

            return result;
        }

        #endregion
        #region Functionality

        // TODO: Implement!

        #endregion
    }
}