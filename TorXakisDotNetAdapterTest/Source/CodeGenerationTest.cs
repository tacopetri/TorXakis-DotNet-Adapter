using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TorXakisDotNetAdapter
{
    [TestClass]
    public class CodeGenerationTest
    {
        [TestMethod]
        public void ParseTestModel()
        {
            Dictionary<string, Dictionary<string, string>> actions = new Dictionary<string, Dictionary<string, string>>();

            // Find all TXS models.
            DirectoryInfo modelDirectory = new DirectoryInfo(Path.Combine(@"..\..\..\", "TorXakisDotNetAdapter", "Models"));
            foreach (FileInfo modelFile in modelDirectory.GetFiles("*.txs", SearchOption.AllDirectories))
            {
                string modelText = File.ReadAllText(modelFile.FullName);

                // Find all TYPEDEF sections.
                MatchCollection typedefMatches = Regex.Matches(modelText, @"TYPEDEF(.*?)ENDDEF", RegexOptions.Singleline);
                foreach (Match typedefMatch in typedefMatches)
                {
                    string typedefText = typedefMatch.Groups[1].ToString();
                    typedefText = typedefText.Substring(typedefText.IndexOf("::=") + "::=".Length);

                    // Find all ACTION types.
                    foreach (string typeMatch in typedefText.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string typeStripped = typeMatch.Replace("\n", "").Replace("\t", "").Replace(" ", "");

                        string[] typeParts = typeStripped.Split('{');
                        string typeName = typeParts[0];

                        actions.Add(typeName, new Dictionary<string, string>());

                        if (typeParts.Length > 1)
                        {
                            string data = typeParts[1].TrimEnd('}');
                            foreach (string paramMatch in data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                string[] paramParts = paramMatch.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                string[] paramNames = paramParts[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                string paramType = paramParts[1];
                                foreach (string paramName in paramNames)
                                {
                                    actions[typeName].Add(paramName, paramType);
                                }
                            }
                        }
                    }
                }
            }

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
