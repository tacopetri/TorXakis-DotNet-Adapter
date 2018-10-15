using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TorXakisDotNetAdapter.Tests
{
    [TestClass]
    public class SolverTest
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

        [TestMethod]
        public void NewItem()
        {
            SolverContext solverContext = SolverContext.GetContext();
            Model solverModel = solverContext.CreateModel();

            Parameter id = new Parameter(Domain.IntegerRange(1, 2), nameof(id));
            id.SetBinding(2);
            solverModel.AddParameter(id);

            string[] guids = new string[] { "c87354e7-888b-4a7f-a337-d5e24324b4f1", "10d0e86d-1d6e-4c4e-80c0-2cea387d98a5" };
            Decision guid = new Decision(Domain.Enum(guids), nameof(guid));
            string[] systemNames = new string[] { "human_nld_fire_bevelvoerder", "object_firetool_watergun" };
            Decision systemName = new Decision(Domain.Enum(systemNames), nameof(systemName));
            string[] positions = new string[] { "(0,0,0)", "(1,1,1)" };
            Decision position = new Decision(Domain.Enum(positions), nameof(position));
            string[] rotations = new string[] { "(0,0,0,1)" };
            Decision rotation = new Decision(Domain.Enum(rotations), nameof(rotation));
            string[] modelGroups = new string[] { "group_12", "default" };
            Decision modelGroup = new Decision(Domain.Enum(modelGroups), nameof(modelGroup));
            string[] models = new string[] { "asset_human_nld_ic1_fire_bevelvoerder_12", "asset_object_int_fire_watergun" };
            Decision model = new Decision(Domain.Enum(models), nameof(model));
            int[] modelVersions = new int[] { 24, 18 };
            Decision modelVersion = new Decision(Domain.Set(modelVersions), nameof(modelVersion));
            string[] parents = new string[] { "null" };
            Decision parent = new Decision(Domain.Enum(parents), nameof(parent));

            solverModel.AddDecisions(guid, systemName, position, rotation, modelGroup, model, modelVersion, parent);

            Term guard =
                Model.If(id == 1,
                        guid == guids[0] &
                        systemName == systemNames[0] &
                        position == positions[0] &
                        rotation == rotations[0] &
                        modelGroup == modelGroups[0] &
                        model == models[0] &
                        modelVersion == modelVersions[0] &
                        parent == parents[0],
                Model.If(id == 2,
                        guid == guids[1] &
                        systemName == systemNames[1] &
                        position == positions[1] &
                        rotation == rotations[0] &
                        modelGroup == modelGroups[1] &
                        model == models[1] &
                        modelVersion == modelVersions[1] &
                        parent == parents[0],
                false));

            solverModel.AddConstraint(nameof(guard), guard);

            Solution solution = solverContext.Solve(new HybridLocalSearchDirective());

            Report report = solution.GetReport();
            Console.WriteLine("{0}: {1}", nameof(id), id);
            Console.WriteLine("{0}: {1}", nameof(guid), guid);
            Console.WriteLine("{0}: {1}", nameof(systemName), systemName);
            Console.WriteLine("{0}: {1}", nameof(position), position);
            Console.WriteLine("{0}: {1}", nameof(rotation), rotation);
            Console.WriteLine("{0}: {1}", nameof(modelGroup), modelGroup);
            Console.WriteLine("{0}: {1}", nameof(model), model);
            Console.WriteLine("{0}: {1}", nameof(modelVersion), modelVersion);
            Console.WriteLine("{0}: {1}", nameof(parent), parent);
            Console.Write("{0}", report);
        }
    }
}
