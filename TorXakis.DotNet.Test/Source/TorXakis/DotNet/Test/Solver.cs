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
        public void Example()
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
    }
}
