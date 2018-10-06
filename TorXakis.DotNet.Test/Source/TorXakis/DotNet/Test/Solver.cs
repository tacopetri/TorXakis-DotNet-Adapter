using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakis.DotNet.Test
{
    [TestClass]
    public class Solver
    {
        [TestMethod]
        public void LinearExample()
        {
            // Implementation of example:
            // https://msdn.microsoft.com/en-us/library/ff628587(v=vs.93).aspx

            SolverContext context = SolverContext.GetContext();
            Model model = context.CreateModel();

            Decision vz = new Decision(Domain.RealNonnegative, "barrels_venezuela");
            Decision sa = new Decision(Domain.RealNonnegative, "barrels_saudiarabia");
            model.AddDecisions(vz, sa);

            model.AddConstraints("limits",
                0 <= vz <= 9000,
                0 <= sa <= 6000);

            model.AddConstraints("production",
                0.3 * sa + 0.4 * vz >= 2000,
                0.4 * sa + 0.2 * vz >= 1500,
                0.2 * sa + 0.3 * vz >= 500);

            model.AddGoal("cost", GoalKind.Minimize,
                20 * sa + 15 * vz);

            Solution solution = context.Solve(new SimplexDirective());

            Report report = solution.GetReport();
            Console.WriteLine("vz: {0}, sa: {1}", vz, sa);
            Console.Write("{0}", report);

            Assert.AreEqual("3500", vz.ToString());
            Assert.AreEqual("2000", sa.ToString());
        }

        [TestMethod]
        public void EnumExample()
        {
            // Implementation of example:
            // https://msdn.microsoft.com/en-us/library/ff826355(v=vs.93).aspx

            SolverContext context = SolverContext.GetContext();
            Model model = context.CreateModel();

            Domain colors = Domain.Enum("red", "green", "blue", "yellow");
            Decision be = new Decision(colors, "belgium");
            Decision de = new Decision(colors, "germany");
            Decision fr = new Decision(colors, "france");
            Decision nl = new Decision(colors, "netherlands");
            model.AddDecisions(be, de, fr, nl);

            model.AddConstraints("borders",
                be != de, be != fr, be != nl, de != fr, de != nl);

            DecisionBinding bindBe = be.CreateBinding();
            DecisionBinding bindDe = de.CreateBinding();
            DecisionBinding bindFr = fr.CreateBinding();
            DecisionBinding bindNl = nl.CreateBinding();
            DecisionBinding[] bindings = new DecisionBinding[] { bindBe, bindDe, bindFr, bindNl };

            bindBe.Fix("red");
            bindDe.Fix("blue");

            context.FindAllowedValues(bindings);

            string[] valuesFr = bindFr.StringFeasibleValues.ToArray();
            Console.WriteLine("France: \t{0}", string.Join(", ", valuesFr));

            string[] valuesNl = bindNl.StringFeasibleValues.ToArray();
            Console.WriteLine("Netherlands: \t{0}", string.Join(", ", valuesNl));
        }

        [TestMethod]
        public void SingleChannel()
        {
            SolverContext context = SolverContext.GetContext();
            Model model = context.CreateModel();

            Domain domain = Domain.Enum("CreateItem", "ConnectItem", "DeleteItem");
            Decision channel = new Decision(domain, "channel");
            model.AddDecision(channel);

            Parameter p = new Parameter(Domain.Integer, "p");
            model.AddParameter(p);
            p.SetBinding(3);

            model.AddConstraint("constraint",
                channel == "CreateItem" & p == 3
            );

            Solution solution = context.Solve(new SimplexDirective());

            Report report = solution.GetReport();
            Console.WriteLine("channel: {0}", channel);
            Console.Write("{0}", report);

            Assert.AreEqual("CreateItem", channel.GetString());
        }

        [TestMethod]
        public void MultiChannel()
        {
            SolverContext context = SolverContext.GetContext();
            Model model = context.CreateModel();

            Domain domain = Domain.Enum("CreateItem", "ConnectItem", "DeleteItem");
            Decision channel = new Decision(domain, "channel");
            model.AddDecision(channel);

            model.AddConstraint("constraint",
                channel == "CreateItem" | channel == "ConnectItem"
            );

            // Find all possible values.
            DecisionBinding binding = channel.CreateBinding();
            DecisionBinding[] bindings = new DecisionBinding[] { binding };

            context.FindAllowedValues(bindings);
            string[] values = binding.StringFeasibleValues.ToArray();
            Console.WriteLine("channel: {0}", string.Join(", ", values));

            CollectionAssert.AreEqual(new List<string>() { "CreateItem", "ConnectItem" }, values);
        }
    }
}
