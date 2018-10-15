using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using TorXakisDotNetAdapter.Models;

namespace TorXakisDotNetAdapter.Tests
{
    /// <summary>
    /// Tests for the <see cref="ModelAction"/> class.
    /// </summary>
    [TestClass]
    public class ModelActionTest
    {
        // Define action types with 0, 1 and 2 properties.
        private readonly List<Tuple<ModelAction, string>> actions = new List<Tuple<ModelAction, string>>()
        {
            new Tuple<ModelAction, string>(
                new DeleteBegin(),
                "DeleteBegin"
            ),
            new Tuple<ModelAction, string>(
                new NewItem() { newItemId = 1 },
                "NewItem(1)"
            ),
            new Tuple<ModelAction, string>(
                new ConnectItem() { connectItemId1 = 1, connectItemId2 = 2 },
                "ConnectItem(1,2)"
            ),
        };

        /// <summary>
        /// Test for the <see cref="ModelAction.Serialize"/> method.
        /// </summary>
        [TestMethod]
        public void Serialize()
        {
            // Check serialization of actions.
            foreach (Tuple<ModelAction, string> tuple in actions)
            {
                string serialize = tuple.Item1.Serialize();
                Console.WriteLine(serialize);
                Assert.AreEqual(tuple.Item2, serialize);
            }
        }

        /// <summary>
        /// Test for the <see cref="ModelAction.Deserialize"/> method.
        /// </summary>
        [TestMethod]
        public void Deserialize()
        {
            // Check deserialization of actions.
            foreach (Tuple<ModelAction, string> tuple in actions)
            {
                ModelAction action = ModelAction.Deserialize(tuple.Item2);
                Console.WriteLine(action);
                Assert.AreEqual(tuple.Item1, action);
            }
        }
    }
}
