using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// A state contained in a <see cref="RefinementSystem"/>.
    /// </summary>
    public sealed class RefinementState
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
        public RefinementState(string name)
        {
            // Sanity checks.
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Invalid name: " + name, nameof(name));

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
