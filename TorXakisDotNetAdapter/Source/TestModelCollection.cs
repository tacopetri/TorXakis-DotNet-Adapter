using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TorXakisDotNetAdapter
{
    /// <summary>
    /// A collection of <see cref="TestModel"/> objects.
    /// </summary>
    public sealed class TestModelCollection
    {
        #region Definitions

        // TODO: Implement!

        #endregion
        #region Variables & Properties

        /// <summary>
        /// The test model directory.
        /// </summary>
        public DirectoryInfo Directory { get; private set; }

        /// <summary>
        /// The contained <see cref="TestModel"/> instances.
        /// </summary>
        public List<TestModel> Models { get; private set; } = new List<TestModel>();

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public TestModelCollection(DirectoryInfo directory)
        {
            // Sanity checks.
            if (directory == null || !directory.Exists)
                throw new ArgumentException("Invalid directory: " + directory, nameof(directory));

            Directory = directory;

            // Find model files inside directory (recursive).
            foreach (FileInfo file in Directory.GetFiles("*" + TestModel.FileExtension, SearchOption.AllDirectories))
            {
                TestModel model = new TestModel(file);
                Models.Add(model);
            }
        }

        /// <summary><see cref="object.ToString"/></summary>
        public override string ToString()
        {
            string result = GetType().Name + " " + nameof(Directory) + " (" + Directory.Name + ")";
            foreach (TestModel model in Models)
                result += "\n" + model;
            return result;
        }

        #endregion
        #region Functionality

        /// <summary>
        /// Aggregates <see cref="TestModel.ParseActions"/>.
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> ParseActions()
        {
            Dictionary<string, Dictionary<string, string>> actions = new Dictionary<string, Dictionary<string, string>>();

            foreach (TestModel model in Models)
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
