using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TorXakisDotNetAdapter.Logging;
using TorXakisDotNetAdapter.Mapping;

namespace TorXakisDotNetAdapter.Tests
{
    /// <summary>
    /// Tests for the <see cref="TorXakisDotNetAdapter.Mapping"/> namespace.
    /// </summary>
    [TestClass]
    public class MappingTest
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
        /// Creates a small test IOSTS.
        /// </summary>
        [TestMethod]
        public void Create()
        {
            HashSet<SymbolicState> states = new HashSet<SymbolicState>()
            {
                new SymbolicState("S1"),
                new SymbolicState("S2"),
            };

            // LOCATION VARIABLES //

            List<SymbolicVariable> variables = new List<SymbolicVariable>()
            {
                new SymbolicVariable("item", 0),
            };

            // INTERACTION VARIABLES //

            Guid[] guids = new Guid[] { new Guid("c87354e7-888b-4a7f-a337-d5e24324b4f1"), new Guid("10d0e86d-1d6e-4c4e-80c0-2cea387d98a5") };
            string[] systemNames = new string[] { "human_nld_fire_bevelvoerder", "object_firetool_watergun" };
            string[] positions = new string[] { "(0,0,0)", "(1,1,1)" };
            string[] rotations = new string[] { "(0,0,0,1)" };
            string[] modelGroups = new string[] { "group_12", "default" };
            string[] models = new string[] { "asset_human_nld_ic1_fire_bevelvoerder_12", "asset_object_int_fire_watergun" };
            int[] modelVersions = new int[] { 24, 18 };
            object[] parents = new object[] { null };

            HashSet<SymbolicTransition> transitions = new HashSet<SymbolicTransition>()
            {
                new SymbolicTransition("T12", states.ElementAt(0), states.ElementAt(1),
                    ActionType.Input, "NewItem",
                    new List<SymbolicVariable>()
                    {
                        new SymbolicVariable("id", 0),
                    },
                    (vs, ps) =>
                    {
                        Console.WriteLine("Received item var: " + ps.First(x => x.Name == "id").GetValue<int>());
                        return ps;
                    },
                    (vs, ps) =>
                    {
                        int id = ps.First(x => x.Name == "id").GetValue<int>();
                        Console.WriteLine("Setting item var: " + id);
                        vs.First(x=>x.Name == "item").SetValue(id);
                    }
                ),
                new SymbolicTransition("T21", states.ElementAt(1), states.ElementAt(0),
                    ActionType.Output, "ItemEventArgsNew",
                    new List<SymbolicVariable>()
                    {
                        new SymbolicVariable("Guid", null),
                        new SymbolicVariable("SystemName", null),
                        new SymbolicVariable("Position", null),
                        new SymbolicVariable("Rotation", null),
                        new SymbolicVariable("ModelGroup", null),
                        new SymbolicVariable("Model", null),
                        new SymbolicVariable("ModelVersion", null),
                        new SymbolicVariable("Parent", null),
                    },
                    (vs, ps) =>
                    {
                        int item = vs.First(x => x.Name == "item").GetValue<int>();

                        List<SymbolicVariable> result = new List<SymbolicVariable>()
                        {
                            new SymbolicVariable("Guid", guids[item-1] ),
                            new SymbolicVariable("SystemName", systemNames[item-1] ),
                            new SymbolicVariable("Position", positions[item-1] ),
                            new SymbolicVariable("Rotation", rotations[0] ),
                            new SymbolicVariable("ModelGroup", modelGroups[item-1] ),
                            new SymbolicVariable("Model", models[item-1] ),
                            new SymbolicVariable("ModelVersion", modelVersions[item-1] ),
                            new SymbolicVariable("Parent", parents[0] ),
                        };
                        return result;
                    },
                    (vs, ps) =>
                    {
                        int id = 0;
                        Console.WriteLine("Setting item var: " + id);
                        vs.First(x=>x.Name == "item").SetValue(id);
                    }
                ),
            };

            SymbolicTransitionSystem iosts = new SymbolicTransitionSystem("IOSTS", states, states.ElementAt(0), transitions, variables);
            Console.WriteLine(iosts);

            // Have the properties been initialized correctly?
            CollectionAssert.AreEqual(states.ToList(), iosts.States.ToList());
            Assert.AreEqual(states.ElementAt(0), iosts.State);
            CollectionAssert.AreEqual(transitions.ToList(), iosts.Transitions.ToList());
            CollectionAssert.AreEqual(variables, iosts.Variables);
            CollectionAssert.AreEqual(variables.Select(x => x.Name).ToList(), iosts.Variables.Select(x => x.Name).ToList());

            // Test some transitions!
            iosts.HandleAction(ActionType.Input, "NewItem", new List<SymbolicVariable>() { new SymbolicVariable("id", 1) });
            Console.WriteLine(iosts);
            Assert.AreEqual(states.ElementAt(1), iosts.State);
            iosts.HandleAction(ActionType.Output, "ItemEventArgsNew", new List<SymbolicVariable>());
            Console.WriteLine(iosts);
            Assert.AreEqual(states.ElementAt(0), iosts.State);
        }
    }
}
