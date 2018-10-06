using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakis.DotNet
{
    /// <summary>
    /// This container class represents an ACTION (either an INPUT or an OUTPUT).
    /// It combines <see cref="Type"/>, <see cref="Channel"/> and <see cref="Data"/> information.
    /// </summary>
    public sealed class TorXakisAction
    {
        #region Definitions

        // TODO: Implement!

        #endregion
        #region Variables & Properties

        /// <summary>
        /// The <see cref="ActionType"/> value.
        /// </summary>
        public ActionType Type { get; private set; }

        /// <summary>
        /// The channel, in user-friendly string format.
        /// </summary>
        public string Channel { get; private set; }

        /// <summary>
        /// The data, in user-friendly string format.
        /// </summary>
        public string Data { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        private TorXakisAction(ActionType type, string channel, string data)
        {
            // Sanity checks.
            if (string.IsNullOrEmpty(channel)) throw new ArgumentException(nameof(channel) + ": " + channel);
            if (string.IsNullOrEmpty(data)) throw new ArgumentException(nameof(data) + ": " + data);

            Type = type;
            Channel = channel;
            Data = data;
        }

        /// <summary>
        /// Returns a new <see cref="ActionType.Input"/> <see cref="TorXakisAction"/> instance, based on the given channel and data.
        /// </summary>
        public static TorXakisAction FromInput(string channel, string data)
        {
            return new TorXakisAction(ActionType.Input, channel, data);
        }

        /// <summary>
        /// Returns a new <see cref="ActionType.Output"/> <see cref="TorXakisAction"/> instance, based on the given channel and data.
        /// </summary>
        public static TorXakisAction FromOutput(string channel, string data)
        {
            return new TorXakisAction(ActionType.Output, channel, data);
        }

        /// <summary>
        /// Returns a new instance, based on the given string. Returns NULL if the string format is invalid.
        /// </summary>
        public static TorXakisAction FromString(string str)
        {
            try
            {
                string[] split = str.Split(' ');
                string channel = split[0];

                ActionType type;
                switch (split[1])
                {
                    case "?":
                        type = ActionType.Input;
                        break;
                    case "!":
                        type = ActionType.Output;
                        break;
                    default:
                        throw new FormatException("Invalid format: " + split[1]);
                }

                string data = string.Join(" ", split.Skip(2).ToArray());
                return new TorXakisAction(type, channel, data);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary><see cref="Object.ToString"/></summary>
        public override string ToString()
        {
            return Channel + " " + (Type == ActionType.Input ? "?" : "!") + " " + Data;
        }

        #endregion
        #region Equality

        /// <summary><see cref="object.Equals(object)"/></summary>
        public override bool Equals(object obj)
        {
            if (obj is TorXakisAction other)
            {
                return this == other;
            }
            else return false;
        }

        /// <summary>
        /// Operator definition for equality. This checks all relevant property values.
        /// </summary>
        public static bool operator ==(TorXakisAction left, TorXakisAction right)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(left, right)) return true;

            // If one is null, but not both, return false.
            if (((object)left == null) || ((object)right == null)) return false;

            // If the types are different, return false.
            if (left.GetType() != right.GetType()) return false;

            // Check that the coordinates are equal.
            return left.Type == right.Type && left.Channel == right.Channel && left.Data == right.Data;
        }

        /// <summary>
        /// Operator definition for inequality. This checks all relevant property values.
        /// </summary>
        public static bool operator !=(TorXakisAction left, TorXakisAction right)
        {
            return !(left == right);
        }

        /// <summary><see cref="object.GetHashCode"/></summary>
        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Channel.GetHashCode() ^ Data.GetHashCode();
        }

        #endregion
    }
}
