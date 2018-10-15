using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TorXakisDotNetAdapter.Models;
using TorXakisDotNetAdapter.Refinement;

namespace TorXakisDotNetAdapter.Tests
{
    /// <summary>
    /// Test class definition for a SUT message.
    /// </summary>
    public class ItemEventArgsNew : ISystemAction
    {
        /// <summary>
        /// The globally unique identifer.
        /// </summary>
        public Guid GUID { get; set; }
        /// <summary>
        /// The system name.
        /// </summary>
        public string SystemName { get; set; }
        /// <summary>
        /// The position (3 vector).
        /// </summary>
        public float[] Position { get; set; } = new float[3];
        /// <summary>
        /// The rotation (4 vector).
        /// </summary>
        public float[] Rotation { get; set; } = new float[4];
        /// <summary>
        /// The model group.
        /// </summary>
        public string ModelGroup { get; set; }
        /// <summary>
        /// The model.
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// The model version.
        /// </summary>
        public int ModelVersion { get; set; }
        /// <summary>
        /// The parent item.
        /// </summary>
        public object Parent { get; set; }

        /// <summary>
        /// Constructor, with parameters.
        /// </summary>
        public ItemEventArgsNew() { }
    }

    /// <summary>
    /// Tests for the <see cref="TorXakisDotNetAdapter.Refinement"/> namespace.
    /// </summary>
    [TestClass]
    public class RefinementTest
    {
        /// <summary>
        /// Creates a small test IOSTS.
        /// </summary>
        [TestMethod]
        public void Create()
        {
            HashSet<RefinementState> states = new HashSet<RefinementState>()
            {
                new RefinementState("S1"),
                new RefinementState("S2"),
            };

            // LOCATION VARIABLES //

            List<RefinementVariable> variables = new List<RefinementVariable>()
            {
                new RefinementVariable("item", 0),
            };

            // INTERACTION VARIABLES //

            Guid[] guids = new Guid[] { new Guid("c87354e7-888b-4a7f-a337-d5e24324b4f1"), new Guid("10d0e86d-1d6e-4c4e-80c0-2cea387d98a5") };
            string[] systemNames = new string[] { "human_nld_fire_bevelvoerder", "object_firetool_watergun" };
            float[][] positions = new float[][] { new float[3] { 0, 0, 0 }, new float[3] { 1, 1, 1 } };
            float[][] rotations = new float[][] { new float[4] { 1, 0, 0, 1 }, new float[4] { 0, 1, 0, 1 } };
            string[] modelGroups = new string[] { "group_12", "default" };
            string[] models = new string[] { "asset_human_nld_ic1_fire_bevelvoerder_12", "asset_object_int_fire_watergun" };
            int[] modelVersions = new int[] { 24, 18 };
            object[] parents = new object[] { null, null };

            HashSet<RefinementTransition> transitions = new HashSet<RefinementTransition>()
            {
                new RefinementTransition("T12", states.ElementAt(0), states.ElementAt(1),
                    ActionType.Input,
                    (action) =>
                    {
                        if (action is NewItem cast)
                        {
                            return cast.newItemId == 1;
                        }
                        return false;
                    },
                    (action, vars) =>
                    {
                        NewItem cast = (NewItem)action;
                        int id = cast.newItemId;
                        Console.WriteLine("Setting item var: " + id);
                        vars.First(x=>x.Name == "item").SetValue(id);
                    }
                ),
                new RefinementTransition("T21", states.ElementAt(1), states.ElementAt(0),
                    ActionType.Output,
                    (action) =>
                    {
                        if (action is ItemEventArgsNew cast)
                        {
                            return true;
                        }
                        return false;
                    },
                    (action, vars) =>
                    {
                        int id = 0;
                        Console.WriteLine("Setting item var: " + id);
                        vars.First(x=>x.Name == "item").SetValue(id);
                    }
                ),
            };

            RefinementSystem iosts = new RefinementSystem("IOSTS", states, states.ElementAt(0), transitions, variables);
            Console.WriteLine(iosts);

            // Have the properties been initialized correctly?
            CollectionAssert.AreEqual(states.ToList(), iosts.States.ToList());
            Assert.AreEqual(states.ElementAt(0), iosts.State);
            CollectionAssert.AreEqual(transitions.ToList(), iosts.Transitions.ToList());
            CollectionAssert.AreEqual(variables, iosts.Variables);
            CollectionAssert.AreEqual(variables.Select(x => x.Name).ToList(), iosts.Variables.Select(x => x.Name).ToList());

            // Test some transitions!
            NewItem modelAction = new NewItem()
            {
                newItemId = 1,
            };
            iosts.HandleAction(ActionType.Input, modelAction);
            Console.WriteLine(iosts);
            Assert.AreEqual(states.ElementAt(1), iosts.State);

            ItemEventArgsNew systemAction = new ItemEventArgsNew()
            {
                GUID = guids[0],
                SystemName = systemNames[0],
                Position = positions[0],
                Rotation = rotations[0],
                ModelGroup = modelGroups[0],
                Model = models[0],
                ModelVersion = modelVersions[0],
                Parent = parents[0],
            };
            iosts.HandleAction(ActionType.Output, systemAction);
            Console.WriteLine(iosts);
            Assert.AreEqual(states.ElementAt(0), iosts.State);
        }
    }
}
