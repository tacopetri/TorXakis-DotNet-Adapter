﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TorXakisDotNetAdapter
{
    /// <summary>
    /// Provides an object-oriented parser for TorXakis models (.txs files).
    /// </summary>
    public sealed class TorXakisModel
    {
        #region Definitions

        /// <summary>
        /// The file extension for model files.
        /// </summary>
        public const string FileExtension = ".txs";

        #endregion
        #region Variables & Properties

        /// <summary>
        /// The model file.
        /// </summary>
        public FileInfo File { get; private set; }

        #endregion
        #region Create & Destroy

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public TorXakisModel(FileInfo file)
        {
            // Sanity checks.
            if (file == null || !file.Exists || file.Extension.ToLowerInvariant() != FileExtension)
                throw new ArgumentException("Invalid file: " + file, nameof(file));

            File = file;
        }

        /// <summary><see cref="Object.ToString"/></summary>
        public override string ToString()
        {
            return GetType().Name + " " + nameof(File) + " (" + File.FullName + ")";
        }

        #endregion
        #region Functionality

        /// <summary>
        /// Parses the defined <see cref="TorXakisAction"/> types from the model.
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> ParseActions()
        {
            Dictionary<string, Dictionary<string, string>> actions = new Dictionary<string, Dictionary<string, string>>();

            string modelText = System.IO.File.ReadAllText(File.FullName);

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

                    // Parse NAME.
                    string[] typeParts = typeStripped.Split('{');
                    string typeName = typeParts[0];
                    actions.Add(typeName, new Dictionary<string, string>());

                    // Parse PARAMETER NAMES, and their TYPES.
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

            return actions;
        }

        #endregion
    }
}
