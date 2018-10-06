using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakis.DotNet
{
    /// <summary>
    /// A state contained in a <see cref="SymbolicTransitionSystem"/>.
    /// </summary>
    public sealed class SymbolicState
    {
        #region Definitions

        // TODO: Implement!

        #endregion
        #region Variables & Properties

        /// <summary>
        /// The user-friendly name.
        /// </summary>
        public string Name { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
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
