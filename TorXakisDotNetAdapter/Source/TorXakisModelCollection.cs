using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TorXakisDotNetAdapter
{
    /// <summary>
    /// A collection of <see cref="TorXakisModel"/> objects.
    /// </summary>
    public sealed class TorXakisModelCollection
    {
        #region Definitions

        // TODO: Implement!

        #endregion
        #region Variables & Properties

        /// <summary>
        /// The model directory.
        /// </summary>
        public DirectoryInfo Directory { get; private set; }

        /// <summary>
        /// The contained <see cref="TorXakisModel"/> instances.
        /// </summary>
        public List<TorXakisModel> Models { get; private set; } = new List<TorXakisModel>();

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public TorXakisModelCollection(DirectoryInfo directory)
        {
            // Sanity checks.
            if (directory == null || !directory.Exists)
                throw new ArgumentException("Invalid directory: " + directory, nameof(directory));

            Directory = directory;

            // Find model files inside directory (recursive).
            foreach (FileInfo file in Directory.GetFiles("*" + TorXakisModel.FileExtension, SearchOption.AllDirectories))
            {
                TorXakisModel model = new TorXakisModel(file);
                Models.Add(model);
            }
        }

        /// <summary><see cref="Object.ToString"/></summary>
        public override string ToString()
        {
            string result = GetType().Name + " " + nameof(Directory) + " (" + Directory.FullName + ")";
            foreach (TorXakisModel model in Models)
                result += "\n" + model;
            return result;
        }

        #endregion
        #region Functionality

        /// <summary>
        /// Aggregates <see cref="TorXakisModel.ParseActions"/>.
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> ParseActions()
        {
            Dictionary<string, Dictionary<string, string>> actions = new Dictionary<string, Dictionary<string, string>>();

            foreach (TorXakisModel model in Models)
            {
                foreach (KeyValuePair<string, Dictionary<string, string>> kvp in model.ParseActions())
                {
                    actions.Add(kvp.Key, kvp.Value);
                }
            }

            return actions;
        }

        #endregion
    }
}
