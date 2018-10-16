using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakisDotNetAdapter.Legacy.Mapping
{
    /// <summary>
    /// A variable contained in a <see cref="SymbolicTransitionSystem"/>.
    /// </summary>
    public sealed class SymbolicVariable
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
        /// The strongly-typed value.
        /// </summary>
        public object Value { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public SymbolicVariable(string name, object value)
        {
            // Sanity checks.
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name) + ": " + name);

            Name = name;
            Value = value;
        }

        /// <summary><see cref="object.ToString"/></summary>
        public override string ToString()
        {
            return "(" + Name + ": " + Value + ")";
        }

        #endregion
        #region Functionality

        /// <summary>
        /// Type-safe value setter.
        /// </summary>
        public void SetValue<T>(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Type-safe value getter.
        /// </summary>
        public T GetValue<T>()
        {
            return (T)Value;
        }

        #endregion
    }
}
