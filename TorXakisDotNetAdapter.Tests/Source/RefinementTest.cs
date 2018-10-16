using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// Test of the <see cref="Refinement.Framework"/> class.
        /// </summary>
        [TestMethod]
        public void Framework()
        {
            FileInfo model = new FileInfo(@"..\..\..\TorXakisDotNetAdapter.Models\Models\Reference.txs");
            Framework framework = new Framework(model);
            Console.WriteLine(framework);

            HashSet<State> states = new HashSet<State>()
            {
                new State("S1"),
                new State("S2"),
            };

            // STATIC LOOKUP //

            Guid[] guids = new Guid[] { new Guid("c87354e7-888b-4a7f-a337-d5e24324b4f1"), new Guid("10d0e86d-1d6e-4c4e-80c0-2cea387d98a5") };
            string[] systemNames = new string[] { "human_nld_fire_bevelvoerder", "object_firetool_watergun" };
            float[][] positions = new float[][] { new float[3] { 0, 0, 0 }, new float[3] { 1, 1, 1 } };
            float[][] rotations = new float[][] { new float[4] { 1, 0, 0, 1 }, new float[4] { 0, 1, 0, 1 } };
            string[] modelGroups = new string[] { "group_12", "default" };
            string[] models = new string[] { "asset_human_nld_ic1_fire_bevelvoerder_12", "asset_object_int_fire_watergun" };
            int[] modelVersions = new int[] { 24, 18 };
            object[] parents = new object[] { null, null };

            HashSet<Transition> transitions = new HashSet<Transition>()
            {
                new ReactiveTransition("T12", states.ElementAt(0), states.ElementAt(1),
                    (action) =>
                    {
                        return action is NewItem;
                    },
                    (action, variables) =>
                    {
                        NewItem cast = (NewItem)action;
                        int id = cast.newItemId;
                        Console.WriteLine("Setting item var: " + id);
                        variables.SetValue("item", id);
                    }
                ),
                new ProactiveTransition("T21", states.ElementAt(1), states.ElementAt(0),
                    (variables) =>
                    {
                        int id = variables.GetValue<int>("item");
                        return new ItemEventArgsNew()
                        {
                            GUID = guids[id-1],
                            SystemName = systemNames[id-1],
                            Position = positions[id-1],
                            Rotation = rotations[id-1],
                            ModelGroup = modelGroups[id-1],
                            Model = models[id-1],
                            ModelVersion = modelVersions[id-1],
                            Parent = parents[id-1],
                        };
                    },
                    (action, variables) =>
                    {
                        int id = 0;
                        Console.WriteLine("Setting item var: " + id);
                        variables.SetValue("item", id);
                    }
                ),
            };

            TransitionSystem system = new TransitionSystem("IOSTS", states, states.ElementAt(0), transitions);
            Console.WriteLine(system);

            framework.AddSystem(system);
            Console.WriteLine(framework);

            // Have the properties been initialized correctly?
            CollectionAssert.AreEqual(states.ToList(), system.States.ToList());
            Assert.AreEqual(states.ElementAt(0), system.InitialState);
            Assert.AreEqual(states.ElementAt(0), system.CurrentState);
            CollectionAssert.AreEqual(transitions.ToList(), system.Transitions.ToList());

            // Test some transitions!
            NewItem modelAction = new NewItem()
            {
                newItemId = 1,
            };
            system.HandleAction(ActionType.Input, modelAction);
            Console.WriteLine(system);
            Assert.AreEqual(states.ElementAt(1), system.CurrentState);

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
            system.HandleAction(ActionType.Output, systemAction);
            Console.WriteLine(system);
            Assert.AreEqual(states.ElementAt(0), system.CurrentState);
        }
    }
}
