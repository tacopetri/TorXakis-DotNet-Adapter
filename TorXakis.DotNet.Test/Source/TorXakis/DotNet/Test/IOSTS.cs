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
                new SymbolicState("S3"),
            };

            HashSet<SymbolicTransition> transitions = new HashSet<SymbolicTransition>()
            {
                new SymbolicTransition("T12", states.ElementAt(0), states.ElementAt(1),
                    ActionType.Input, "input",
                    new HashSet<string>() { "Param1", "Param2" },
                    (Dictionary<string, object> v, Dictionary<string, object> p) =>
                    {
                        return (bool)v["Var1"] == true;
                    },
                    (Dictionary<string, object> v, Dictionary<string, object> p) =>
                    {
                        return new Dictionary<string, object>() { { "Var1", false } };
                    }
                ),
                new SymbolicTransition("T23", states.ElementAt(1), states.ElementAt(2),
                    ActionType.Output, "command",
                    new HashSet<string>() { "Param1" },
                    (Dictionary<string, object> v, Dictionary<string, object> p) =>
                    {
                        return true;
                    },
                    (Dictionary<string, object> v, Dictionary<string, object> p) =>
                    {
                        return new Dictionary<string, object>() { { "Var2", 15 }, { "Var3", 7.5 } };
                    }
                ),
                new SymbolicTransition("T31", states.ElementAt(2), states.ElementAt(0),
                    ActionType.Input, "simple",
                    new HashSet<string>(),
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

            Dictionary<string, object> variables = new Dictionary<string, object>()
            {
                { "Var1", true },
                { "Var2", 10 },
                { "Var3", 2.5 },
                { "Var4", "text" },
            };

            SymbolicTransitionSystem iosts = new SymbolicTransitionSystem("IOSTS", states, states.ElementAt(0), transitions, variables);
            Console.WriteLine(iosts);

            // Have the static properties been initialized correctly?
            CollectionAssert.AreEqual(states.ToList(), iosts.States.ToList());
            Assert.AreEqual(states.ElementAt(0), iosts.InitialState);
            CollectionAssert.AreEqual(transitions.ToList(), iosts.Transitions.ToList());
            CollectionAssert.AreEqual(variables.Select(x => x.Key).ToList(), iosts.InitialVariables.Select(x => x.Key).ToList());
            CollectionAssert.AreEqual(variables.Select(x => x.Value).ToList(), iosts.InitialVariables.Select(x => x.Value).ToList());

            // Have the dynamic properties been initialized correctly?
            Assert.AreEqual(iosts.InitialState, iosts.CurrentState);
            CollectionAssert.AreEqual(iosts.InitialVariables.Select(x => x.Key).ToList(), iosts.CurrentVariables.Select(x => x.Key).ToList());
            CollectionAssert.AreEqual(iosts.InitialVariables.Select(x => x.Value).ToList(), iosts.CurrentVariables.Select(x => x.Value).ToList());

            // Test some transitions!
            iosts.HandleAction(ActionType.Input, "input", new Dictionary<string, object>() { { "Param1", 1 }, { "Param2", 2 }, });
            Console.WriteLine(iosts);
            Assert.AreEqual(states.ElementAt(1), iosts.CurrentState);
            iosts.HandleAction(ActionType.Output, "command", new Dictionary<string, object>() { { "Param1", 1 }, });
            Console.WriteLine(iosts);
            Assert.AreEqual(states.ElementAt(2), iosts.CurrentState);
        }
    }
}
