using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TorXakisDotNetAdapter.Logging;

namespace TorXakisDotNetAdapter.Tests
{
    /// <summary>
    /// Tests for the <see cref="TorXakisModel"/> and <see cref="TorXakisModelCollection"/> classes.
    /// </summary>
    [TestClass]
    public class TorXakisModelTest
    {
        /// <summary><see cref="ClassInitializeAttribute"/></summary>
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // Forward log messages to console.
            Log.Message += (message) =>
            {
                Console.WriteLine(message);
            };
        }

        /// <summary>
        /// Test for the <see cref="TorXakisModel.ParseConnections"/> method.
        /// </summary>
        [TestMethod]
        public void ParseConnections()
        {
            // Find all model files.
            DirectoryInfo directory = new DirectoryInfo(Path.Combine(@"..\..\..\", "TorXakisDotNetAdapter.Models", "Models"));
            TorXakisModelCollection collection = new TorXakisModelCollection(directory);
            Console.WriteLine(collection);

            // Parse connections from models.
            foreach (TorXakisModel model in collection.Models)
            {
                Dictionary<int, List<string>> connections = model.ParseConnections();
                foreach (KeyValuePair<int, List<string>> kvp in connections)
                {
                    string info = "Port: " + kvp.Key + " Input: " + kvp.Value[0] + " Output: " + kvp.Value[1];
                    Console.WriteLine(info);
                }
            }
        }

        /// <summary>
        /// Test for the <see cref="TorXakisModel.ParseActions"/> method.
        /// </summary>
        [TestMethod]
        public void ParseActions()
        {
            // Find all model files.
            DirectoryInfo directory = new DirectoryInfo(Path.Combine(@"..\..\..\", "TorXakisDotNetAdapter.Models", "Models"));
            TorXakisModelCollection collection = new TorXakisModelCollection(directory);
            Console.WriteLine(collection);

            // Parse actions from models.
            Dictionary<string, Dictionary<string, string>> actions = collection.ParseActions();
            foreach (KeyValuePair<string, Dictionary<string, string>> kvp1 in actions)
            {
                string info = kvp1.Key;
                foreach (KeyValuePair<string, string> kvp2 in kvp1.Value)
                {
                    info += "\n    " + kvp2.Key + " " + kvp2.Value;
                }
                Console.WriteLine(info);
            }
        }
    }
}
