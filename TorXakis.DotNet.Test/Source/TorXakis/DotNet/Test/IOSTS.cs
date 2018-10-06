using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakis.DotNet.Test
{
    [TestClass]
    public class IOSTS
    {
        [TestMethod]
        public void Create()
        {
            HashSet<SymbolicState> states = new HashSet<SymbolicState>()
            {
                new SymbolicState("S1"),
                new SymbolicState("S2"),
            };

            HashSet<SymbolicTransition> transitions = new HashSet<SymbolicTransition>()
            {
                new SymbolicTransition("T12", states.ElementAt(0), states.ElementAt(1),
                    ActionType.Input, "NewItem",
                    new List<Decision>()
                    {
                        new Decision(Domain.IntegerRange(1, 2), "id"),
                    },
                    (Dictionary<string, object> v, Dictionary<string, object> p) =>
                    {
                        return true;
                    },
                    (Dictionary<string, object> v, Dictionary<string, object> p) =>
                    {
                        return new Dictionary<string, object>();
                    }
                ),
                new SymbolicTransition("T21", states.ElementAt(1), states.ElementAt(0),
                    ActionType.Output, "ItemEventArgsNew",
                    new List<Decision>()
                    {
                        new Decision(Domain.Enum("c87354e7-888b-4a7f-a337-d5e24324b4f1", "10d0e86d-1d6e-4c4e-80c0-2cea387d98a5"), "Guid"),
                        new Decision(Domain.Enum("human_nld_fire_bevelvoerder", "object_firetool_watergun"), "SystemName"),
                        new Decision(Domain.Enum("(0,0,0)", "(1,1,1)"), "Position"),
                        new Decision(Domain.Enum("(0,0,0,1)"), "Rotation"),
                        new Decision(Domain.Enum("group_12", "default"), "ModelGroup"),
                        new Decision(Domain.Enum("asset_human_nld_ic1_fire_bevelvoerder_12", "asset_object_int_fire_watergun"), "Model"),
                        new Decision(Domain.Set(24, 18), "ModelVersion"),
                        new Decision(Domain.Enum("null"), "Parent"),
                    },
                    (Dictionary<string, object> v, Dictionary<string, object> p) =>
                    {
                        return true;
                    },
                    (Dictionary<string, object> v, Dictionary<string, object> p) =>
                    {
                        return new Dictionary<string, object>();
                    }
                ),
            };

            List<Parameter> variables = new List<Parameter>();
            Parameter var1 = new Parameter(Domain.Boolean, "Bool");
            var1.SetBinding(true);
            variables.Add(var1);
            Parameter var2 = new Parameter(Domain.Integer, "Int");
            var2.SetBinding(10);
            variables.Add(var2);
            Parameter var3 = new Parameter(Domain.Real, "Float");
            var3.SetBinding(2.5);
            variables.Add(var3);
            Parameter var4 = new Parameter(Domain.Enum("text", "book"), "String");
            var4.SetBinding("text");
            variables.Add(var4);

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
