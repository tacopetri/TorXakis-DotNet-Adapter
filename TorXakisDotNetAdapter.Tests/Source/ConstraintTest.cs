using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

using TorXakisDotNetAdapter.Constraint;

namespace TorXakisDotNetAdapter.Tests
{
    /// <summary>
    /// Tests for the <see cref="TorXakisDotNetAdapter.Constraint"/> namespace.
    /// </summary>
    [TestClass]
    public class ConstraintTest
    {
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

            List<Parameter> variables = new List<Parameter>();
            Parameter item = new Parameter(Domain.IntegerRange(0, 2), "item");
            item.SetBinding(0);
            variables.Add(item);

            // INTERACTION VARIABLES 1 //

            Decision id = new Decision(Domain.IntegerRange(1, 2), "id");

            // INTERACTION VARIABLES 2 //

            string[] guids = new string[] { "c87354e7-888b-4a7f-a337-d5e24324b4f1", "10d0e86d-1d6e-4c4e-80c0-2cea387d98a5" };
            Decision guid = new Decision(Domain.Enum(guids), "Guid");
            string[] systemNames = new string[] { "human_nld_fire_bevelvoerder", "object_firetool_watergun" };
            Decision systemName = new Decision(Domain.Enum(systemNames), "SystemName");
            string[] positions = new string[] { "(0,0,0)", "(1,1,1)" };
            Decision position = new Decision(Domain.Enum(positions), "Position");
            string[] rotations = new string[] { "(0,0,0,1)" };
            Decision rotation = new Decision(Domain.Enum(rotations), "Rotation");
            string[] modelGroups = new string[] { "group_12", "default" };
            Decision modelGroup = new Decision(Domain.Enum(modelGroups), "ModelGroup");
            string[] models = new string[] { "asset_human_nld_ic1_fire_bevelvoerder_12", "asset_object_int_fire_watergun" };
            Decision model = new Decision(Domain.Enum(models), "Model");
            int[] modelVersions = new int[] { 24, 18 };
            Decision modelVersion = new Decision(Domain.Set(modelVersions), "ModelVersion");
            string[] parents = new string[] { "null" };
            Decision parent = new Decision(Domain.Enum(parents), "Parent");

            HashSet<SymbolicTransition> transitions = new HashSet<SymbolicTransition>()
            {
                new SymbolicTransition("T12", states.ElementAt(0), states.ElementAt(1),
                    ActionType.Input, "NewItem",
                    new List<Decision>()
                    {
                        id,
                    },
                    (m, v, p) =>
                    {
                        m.AddConstraint("guard",
                            1 <= id <= 2);
                    },
                    (v, p) =>
                    {
                        Console.WriteLine("Setting item var: " + int.Parse(id.ToString()));
                        item.SetBinding(int.Parse(id.ToString()));
                    }
                ),
                new SymbolicTransition("T21", states.ElementAt(1), states.ElementAt(0),
                    ActionType.Output, "ItemEventArgsNew",
                    new List<Decision>()
                    {
                        guid,
                        systemName,
                        position,
                        rotation,
                        modelGroup,
                        model,
                        modelVersion,
                        parent,
                    },
                    (m, p, v) =>
                    {
                        m.AddParameter(item);

                        m.AddConstraint("guard",
                            Model.If(item == 1,
                                guid == guids[0] &
                                systemName == systemNames[0] &
                                position == positions[0] &
                                rotation == rotations[0] &
                                modelGroup == modelGroups[0] &
                                model == models[0] &
                                modelVersion == modelVersions[0] &
                                parent == parents[0],
                            Model.If(item == 2,
                                guid == guids[1] &
                                systemName == systemNames[1] &
                                position == positions[1] &
                                rotation == rotations[0] &
                                modelGroup == modelGroups[1] &
                                model == models[1] &
                                modelVersion == modelVersions[1] &
                                parent == parents[0],
                            false))
                            );
                    },
                    (v, p) =>
                    {
                        Console.WriteLine("Setting item var: " + 0);
                        item.SetBinding(0);
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
            iosts.HandleAction(ActionType.Input, "NewItem", new Dictionary<string, object>() { { "Param1", 1 }, { "Param2", 2 }, });
            Console.WriteLine(iosts);
            Assert.AreEqual(states.ElementAt(1), iosts.State);
            iosts.HandleAction(ActionType.Output, "ItemEventArgsNew", new Dictionary<string, object>() { { "Param1", 1 }, });
            Console.WriteLine(iosts);
            Assert.AreEqual(states.ElementAt(0), iosts.State);
        }
    }
}
