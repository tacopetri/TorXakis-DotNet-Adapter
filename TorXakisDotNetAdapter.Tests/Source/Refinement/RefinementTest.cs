using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TorXakisDotNetAdapter.Logging;
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

        /// <summary><see cref="object.ToString"/></summary>
        public override string ToString()
        {
            return GetType().Name + "(" + GUID + ")";
        }
    }

    /// <summary>
    /// Tests for the <see cref="TorXakisDotNetAdapter.Refinement"/> namespace.
    /// </summary>
    [TestClass]
    public class RefinementTest
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

        private static readonly Guid[] guids = new Guid[] { new Guid("c87354e7-888b-4a7f-a337-d5e24324b4f1"), new Guid("10d0e86d-1d6e-4c4e-80c0-2cea387d98a5") };
        private static readonly string[] systemNames = new string[] { "human_nld_fire_bevelvoerder", "object_firetool_watergun" };
        private static readonly float[][] positions = new float[][] { new float[3] { 0, 0, 0 }, new float[3] { 1, 1, 1 } };
        private static readonly float[][] rotations = new float[][] { new float[4] { 1, 0, 0, 1 }, new float[4] { 0, 1, 0, 1 } };
        private static readonly string[] modelGroups = new string[] { "group_12", "default" };
        private static readonly string[] models = new string[] { "asset_human_nld_ic1_fire_bevelvoerder_12", "asset_object_int_fire_watergun" };
        private static readonly int[] modelVersions = new int[] { 24, 18 };
        private static readonly object[] parents = new object[] { null, null };

        private TransitionSystem CreateTransitionSystem1()
        {
            HashSet<State> states = new HashSet<State>()
            {
                new State("S1"),
                new State("S2"),
            };

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
                        variables.SetValue(nameof(id), id);
                    }
                ),
                new ProactiveTransition("T21", states.ElementAt(1), states.ElementAt(0),
                    (variables) =>
                    {
                        int id = variables.GetValue<int>(nameof(id));
                        return new ItemEventArgsNew()
                        {
                            GUID = guids[id - 1],
                            SystemName = systemNames[id - 1],
                            Position = positions[id - 1],
                            Rotation = rotations[id - 1],
                            ModelGroup = modelGroups[id - 1],
                            Model = models[id - 1],
                            ModelVersion = modelVersions[id - 1],
                            Parent = parents[id - 1],
                        };
                    },
                    (action, variables) =>
                    {
                        variables.ClearValue("id");
                    }
                ),
            };

            TransitionSystem system = new TransitionSystem("IOSTS-1", states, states.ElementAt(0), transitions);

            // Have the properties been initialized correctly?
            CollectionAssert.AreEqual(states.ToList(), system.States.ToList());
            Assert.AreEqual(states.ElementAt(0), system.InitialState);
            Assert.AreEqual(states.ElementAt(0), system.CurrentState);
            CollectionAssert.AreEqual(transitions.ToList(), system.Transitions.ToList());

            return system;
        }

        private TransitionSystem CreateTransitionSystem2()
        {
            HashSet<State> states = new HashSet<State>()
            {
                new State("S1"),
                new State("S2"),
            };

            HashSet<Transition> transitions = new HashSet<Transition>()
            {
                new ReactiveTransition("T12", states.ElementAt(0), states.ElementAt(1),
                    (action) =>
                    {
                        return action is ItemEventArgsNew;
                    },
                    (action, variables) =>
                    {
                        ItemEventArgsNew cast = (ItemEventArgsNew)action;
                        int id = guids.ToList().IndexOf(cast.GUID) + 1;
                        variables.SetValue(nameof(id), id);
                    }
                ),
                new ProactiveTransition("T21", states.ElementAt(1), states.ElementAt(0),
                    (variables) =>
                    {
                        int id = variables.GetValue<int>(nameof(id));
                        return new NewItem()
                        {
                            newItemId = id,
                        };
                    },
                    (action, variables) =>
                    {
                        variables.ClearValue("id");
                    }
                ),
            };

            TransitionSystem system = new TransitionSystem("IOSTS-2", states, states.ElementAt(0), transitions);

            // Have the properties been initialized correctly?
            CollectionAssert.AreEqual(states.ToList(), system.States.ToList());
            Assert.AreEqual(states.ElementAt(0), system.InitialState);
            Assert.AreEqual(states.ElementAt(0), system.CurrentState);
            CollectionAssert.AreEqual(transitions.ToList(), system.Transitions.ToList());

            return system;
        }

        /// <summary>
        /// Test of the <see cref="Refinement.TransitionSystem"/> class.
        /// </summary>
        [TestMethod]
        public void TransitionSystem1()
        {
            TransitionSystem system = CreateTransitionSystem1();
            Console.WriteLine(system);

            // Determine possible reactive transitions.
            NewItem modelInput = new NewItem()
            {
                newItemId = 1,
            };
            Console.WriteLine("Using model input: " + modelInput);
            HashSet<ReactiveTransition> reactives = system.PossibleReactiveTransitions(modelInput);
            Console.WriteLine("Possible reactive transitions: " + string.Join(", ", reactives.Select(x => x.ToString()).ToArray()));
            Assert.AreEqual(1, reactives.Count);
            Assert.AreEqual(system.Transitions.ElementAt(0), reactives.First());

            // Execute and check reactive transition.
            system.ExecuteReactiveTransition(modelInput, reactives.First());
            Console.WriteLine(system);
            Assert.AreEqual(system.States.ElementAt(1), system.CurrentState);

            // Determine possible proactive transitions.
            HashSet<ProactiveTransition> proactives = system.PossibleProactiveTransitions();
            Console.WriteLine("Possible proactive transitions: " + string.Join(", ", proactives.Select(x => x.ToString()).ToArray()));
            Assert.AreEqual(1, proactives.Count);
            Assert.AreEqual(system.Transitions.ElementAt(1), proactives.First());

            // Execute and check proactive transition.
            IAction generatedAction = system.ExecuteProactiveTransition(proactives.First());
            Console.WriteLine("Generated action " + generatedAction);
            Assert.AreEqual(typeof(ItemEventArgsNew), generatedAction.GetType());
            Assert.AreEqual(guids[0], (generatedAction as ItemEventArgsNew).GUID);
            Console.WriteLine(system);
            Assert.AreEqual(system.States.ElementAt(0), system.CurrentState);
        }

        /// <summary>
        /// Test of the <see cref="Refinement.TransitionSystem"/> class.
        /// </summary>
        [TestMethod]
        public void TransitionSystem2()
        {
            TransitionSystem system = CreateTransitionSystem2();
            Console.WriteLine(system);

            // Determine possible reactive transitions.
            ItemEventArgsNew systemEvent = new ItemEventArgsNew()
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
            Console.WriteLine("Using system event: " + systemEvent);
            HashSet<ReactiveTransition> reactives = system.PossibleReactiveTransitions(systemEvent);
            Console.WriteLine("Possible reactive transitions: " + string.Join(", ", reactives.Select(x => x.ToString()).ToArray()));
            Assert.AreEqual(1, reactives.Count);
            Assert.AreEqual(system.Transitions.ElementAt(0), reactives.First());

            // Execute and check reactive transition.
            system.ExecuteReactiveTransition(systemEvent, reactives.First());
            Console.WriteLine(system);
            Assert.AreEqual(system.States.ElementAt(1), system.CurrentState);

            // Determine possible proactive transitions.
            HashSet<ProactiveTransition> proactives = system.PossibleProactiveTransitions();
            Console.WriteLine("Possible proactive transitions: " + string.Join(", ", proactives.Select(x => x.ToString()).ToArray()));
            Assert.AreEqual(1, proactives.Count);
            Assert.AreEqual(system.Transitions.ElementAt(1), proactives.First());

            // Execute and check proactive transition.
            IAction generatedAction = system.ExecuteProactiveTransition(proactives.First());
            Console.WriteLine("Generated action " + generatedAction);
            Assert.AreEqual(typeof(NewItem), generatedAction.GetType());
            Assert.AreEqual(1, (generatedAction as NewItem).newItemId);
            Console.WriteLine(system);
            Assert.AreEqual(system.States.ElementAt(0), system.CurrentState);
        }

        /// <summary>
        /// Test of the <see cref="Refinement.RefinementFramework"/> class.
        /// </summary>
        [TestMethod]
        public void RefinementFramework()
        {
            FileInfo model = new FileInfo(@"..\..\..\TorXakisDotNetAdapter.Models\Models\Reference.txs");
            RefinementFramework framework = new RefinementFramework(model);
            Assert.AreEqual(model, framework.Connector.Model.File);

            TransitionSystem system1 = CreateTransitionSystem1();
            framework.AddSystem(system1);
            Assert.AreEqual(system1, framework.Systems.ElementAt(0));

            TransitionSystem system2 = CreateTransitionSystem2();
            framework.AddSystem(system2);
            Assert.AreEqual(system2, framework.Systems.ElementAt(1));

            Console.WriteLine(framework);

            // Trigger a reactive transition, which should activate a proactive one next.
            if (true)
            {
                NewItem modelInput = new NewItem()
                {
                    newItemId = 1,
                };
                Console.WriteLine("Using model input: " + modelInput);
                HashSet<Tuple<TransitionSystem, ReactiveTransition>> reactives = framework.PossibleReactiveTransitions(modelInput);
                Console.WriteLine("Possible reactives transitions: " + string.Join(", ", reactives.Select(x => x.Item1.Name + ": " + x.Item2).ToArray()));
                Assert.AreEqual(1, reactives.Count);

                framework.HandleModelInput(modelInput);
                Console.WriteLine(framework);
                Assert.AreEqual(system1.States.ElementAt(0), system1.CurrentState);
            }

            // Trigger a reactive transition, which should activate a proactive one next.
            if (true)
            {
                ItemEventArgsNew systemEvent = new ItemEventArgsNew()
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
                Console.WriteLine("Using system event: " + systemEvent);
                HashSet<Tuple<TransitionSystem, ReactiveTransition>> reactives = framework.PossibleReactiveTransitions(systemEvent);
                Console.WriteLine("Possible reactives transitions: " + string.Join(", ", reactives.Select(x => x.Item1.Name + ": " + x.Item2).ToArray()));
                Assert.AreEqual(1, reactives.Count);

                framework.HandleSystemEvent(systemEvent);
                Console.WriteLine(framework);
                Assert.AreEqual(system2.States.ElementAt(0), system2.CurrentState);
            }
        }
    }
}
