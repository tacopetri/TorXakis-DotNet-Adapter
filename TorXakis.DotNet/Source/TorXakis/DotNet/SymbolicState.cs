using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakis.DotNet
{
    /// <summary>
    /// A state contained in a <see cref="SymbolicTransitionSystem"/>.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, IsReference = true)]
    public sealed class SymbolicState
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

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        [JsonConstructor]
        public SymbolicState(string name)
        {
            // Sanity checks.
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name) + ": " + name);

            Name = name;
        }

        /// <summary><see cref="Object.ToString"/></summary>
        public override string ToString()
        {
            return "State (" + Name + ")";
        }

        #endregion
        #region Functionality

        // TODO: Implement!

        #endregion
    }
}
