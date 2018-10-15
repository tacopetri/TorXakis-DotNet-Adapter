using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TorXakisDotNetAdapter.Tests
{
    [TestClass]
    public class CodeGenerationTest
    {
        [TestMethod]
        public void ParseTestModel()
        {
            // Find all model files.
            DirectoryInfo modelDirectory = new DirectoryInfo(Path.Combine(@"..\..\..\", "TorXakisDotNetAdapter.Models", "Models"));
            foreach (FileInfo modelFile in modelDirectory.GetFiles("*.txs", SearchOption.AllDirectories))
            {
                // Create model from file.
                TorXakisModel model = new TorXakisModel(modelFile);
                Console.WriteLine(model);

                // Parse actions from model.
                Dictionary<string, Dictionary<string, string>> actions = model.ParseActions();
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
}
