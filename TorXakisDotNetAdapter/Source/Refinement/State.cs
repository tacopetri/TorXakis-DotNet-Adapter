using System;
using System.Collections.Generic;
using System.Linq;
using TorXakisDotNetAdapter.Logging;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// A state contained in a <see cref="TransitionSystem"/>.
    /// </summary>
    public sealed class State
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
        public State(string name)
        {
            // Sanity checks.
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Invalid name: " + name, nameof(name));

            Name = name;
        }

        /// <summary><see cref="object.ToString"/></summary>
        public override string ToString()
        {
            return Name;
        }

        #endregion
        #region Functionality

        // TODO: Implement!

        #endregion
    }
}
