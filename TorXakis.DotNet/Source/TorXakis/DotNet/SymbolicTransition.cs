using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakis.DotNet
{
    /// <summary>
    /// A transition contained in a <see cref="SymbolicTransitionSystem"/>.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, IsReference = true)]
    public sealed class SymbolicTransition
    {
        #region Definitions

        // TODO: Implement!

        #endregion
        #region Variables & Properties

        /// <summary>
        /// The user-friendly name.
        /// </summary>
        [JsonProperty]
        public string Name { get; private set; }

        /// <summary>
        /// The from <see cref="SymbolicState"/> state.
        /// </summary>
        [JsonProperty]
        public SymbolicState From { get; private set; }

        /// <summary>
        /// The to <see cref="SymbolicState"/> state.
        /// </summary>
        [JsonProperty]
        public SymbolicState To { get; private set; }

        /// <summary>
        /// The <see cref="ActionType"/> value: input or output?
        /// </summary>
        [JsonProperty]
        public ActionType Type { get; private set; }

        /// <summary>
        /// The channel name to communicate on.
        /// </summary>
        [JsonProperty]
        public string Channel { get; private set; }

        /// <summary>
        /// The set of parameters.
        /// </summary>
        public HashSet<string> Parameters { get; private set; }

        /// <summary>
        /// The delegate signature of the <see cref="GuardExpression"/>.
        /// </summary>
        public delegate bool GuardDelegate(Dictionary<string, object> variables, Dictionary<string, object> parameters);
        /// <summary>
        /// The guard expression: is this transition valid given the parameter values?
        /// </summary>
        public GuardDelegate GuardExpression { get; private set; }

        /// <summary>
        /// The delegate signature of the <see cref="UpdateExpression"/>.
        /// </summary>
        public delegate Dictionary<string, object> UpdateDelegate(Dictionary<string, object> variables, Dictionary<string, object> parameters);
        /// <summary>
        /// The update expression: if this transition is taken, which variables must be updated?
        /// </summary>
        public UpdateDelegate UpdateExpression { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        [JsonConstructor]
        public SymbolicTransition(string name, SymbolicState from, SymbolicState to, ActionType type, string channel,
            HashSet<string> parameters,
            GuardDelegate guardExpression,
            UpdateDelegate updateExpression)
        {
            // Sanity checks.
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name) + ": " + name);
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(from));
            if (string.IsNullOrEmpty(channel)) throw new ArgumentException(nameof(channel) + ": " + channel);
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (guardExpression == null) throw new ArgumentNullException(nameof(guardExpression));
            if (updateExpression == null) throw new ArgumentNullException(nameof(updateExpression));

            Name = name;
            From = from;
            To = to;

            Type = type;
            Channel = channel;
            Parameters = new HashSet<string>(parameters);
            GuardExpression = guardExpression;
            UpdateExpression = updateExpression;
        }

        /// <summary><see cref="Object.ToString"/></summary>
        public override string ToString()
        {
            string result = "Transition (" + Name + ") " + From + " => " + To;
            result += "\n\t\t" + nameof(Type) + ": " + Type;
            result += "\n\t\t" + nameof(Channel) + ": " + Channel;

            result += "\n\t\t" + nameof(Parameters) + ": " + string.Join(", ", Parameters.ToArray());

            result += "\n\t\t" + nameof(GuardExpression) + ": " + GuardExpression;
            result += "\n\t\t" + nameof(UpdateExpression) + ": " + UpdateExpression;

            return result;
        }

        #endregion
        #region Functionality

        // TODO: Implement!

        #endregion
    }
}
