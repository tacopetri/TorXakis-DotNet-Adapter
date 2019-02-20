using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakisDotNetAdapter
{
    /// <summary>
    /// Static class containing extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Re-used <see cref="System.Random"/> instance, to avoid seeding problems.
        /// </summary>
        private static readonly Random random = new Random();

        /// <summary>
        /// Returns a random element from the given <see cref="IEnumerable{T}"/>.
        /// </summary>
        public static T Random<T>(this IEnumerable<T> input)
        {
            return input.ElementAt(random.Next(input.Count()));
        }
    }
}
