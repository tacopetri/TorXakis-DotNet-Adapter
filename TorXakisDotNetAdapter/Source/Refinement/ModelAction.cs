using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TorXakisDotNetAdapter.Refinement
{
    /// <summary>
    /// The abstract class for all model actions.
    /// The derived classes are parsed from TorXakis models and their code is generated.
    /// </summary>
    public abstract class ModelAction : IAction
    {
        #region Definitions

        /// <summary>
        /// The name of the assembly (and the namespace) that contains the generated model actions.
        /// </summary>
        public const string ModelsAssemblyName = "TorXakisDotNetAdapter.Models";

        #endregion
        #region Variables & Properties

        // TODO: Implement!

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public ModelAction()
        {

        }

        /// <summary><see cref="object.ToString"/></summary>
        public override string ToString()
        {
            return Serialize();
        }

        #endregion
        #region Equality

        /// <summary><see cref="object.Equals(object)"/></summary>
        public override bool Equals(object obj)
        {
            if (obj is ModelAction other)
            {
                return this == other;
            }
            else return false;
        }

        /// <summary>
        /// Operator definition for equality. This checks all relevant property values.
        /// </summary>
        public static bool operator ==(ModelAction left, ModelAction right)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(left, right)) return true;

            // If one is null, but not both, return false.
            if (left is null || right is null) return false;

            // If the types are different, return false.
            if (left.GetType() != right.GetType()) return false;

            // Check that the properties are equal.
            return left.Serialize() == right.Serialize();
        }

        /// <summary>
        /// Operator definition for inequality. This checks all relevant property values.
        /// </summary>
        public static bool operator !=(ModelAction left, ModelAction right)
        {
            return !(left == right);
        }

        /// <summary><see cref="object.GetHashCode"/></summary>
        public override int GetHashCode()
        {
            return Serialize().GetHashCode();
        }

        #endregion
        #region Functionality

        /// <summary>
        /// The cached <see cref="Serialize"/> value.
        /// </summary>
        private string serialized;

        /// <summary>
        /// Serializes this object into a string.
        /// </summary>
        public string Serialize()
        {
            // If this object was never serialized before, use reflection to convert its properties into a string.
            if (serialized == null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(GetType().Name);

                PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                if (properties.Length > 0)
                {
                    sb.Append("(");
                    for (int i = 0; i < properties.Length; i++)
                    {
                        if (i > 0) sb.Append(",");
                        PropertyInfo property = properties[i];
                        sb.Append(property.GetValue(this).ToString());
                    }
                    sb.Append(")");
                }

                serialized = sb.ToString();
            }

            return serialized;
        }

        /// <summary>
        /// Deserializes the given string into an object instance.
        /// </summary>
        public static ModelAction Deserialize(string str)
        {
            string[] parts = str.Split('(');

            // Map the type name back to a class defined in the models assembly.
            string typeName = parts[0];
            Type type = null;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith(ModelsAssemblyName))
                {
                    type = assembly.GetType(ModelsAssemblyName + "." + typeName);
                }
            }

            // Instantiate an object of this type.
            ModelAction action = (ModelAction)Activator.CreateInstance(type);

            // Assign the serialized property values on the new instance.
            if (parts.Length > 1)
            {
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                string[] values = parts[1].TrimEnd(')').Split(',');
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo property = properties[i];
                    string value = values[i];

                    if (property.PropertyType == typeof(bool))
                        property.SetValue(action, bool.Parse(value));
                    else if (property.PropertyType == typeof(int))
                        property.SetValue(action, int.Parse(value));
                    else if (property.PropertyType == typeof(string))
                        property.SetValue(action, value);
                }
            }

            return action;
        }

        #endregion
    }
}
