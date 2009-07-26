namespace AjSharpure.Tests
{
    using System;
    using System.IO;
    using System.Collections;
    using System.Linq;
    using System.Text;

    using AjSharpure;
    using AjSharpure.Compiler;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExamplesTests
    {
        [TestMethod]
        [DeploymentItem("Examples/DefCons.ajshp")]
        public void ShouldParseDefConsExample()
        {
            Parser parser = new Parser(File.OpenText("DefCons.ajshp"));
            object result = parser.ParseForm();

            Assert.IsNotNull(result);
            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        [DeploymentItem("Examples/DefCons.ajshp")]
        public void ShouldEvaluateDefConsExample()
        {
            Parser parser = new Parser(File.OpenText("DefCons.ajshp"));
            Machine machine = new Machine();

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedFunction));

            DefinedFunction deffun = (DefinedFunction)result;

            Assert.AreEqual("cons", deffun.Name);
        }

        [TestMethod]
        [DeploymentItem("Examples/DefCons.ajshp")]
        public void ShouldEvaluateConsDefinedInDefConsExample()
        {
            Parser parser = new Parser(File.OpenText("DefCons.ajshp"));
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());

            parser = new Parser("(cons 1 (list 2 3))");

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(IList));

            IList list = (IList)value;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        [DeploymentItem("Examples/DefFirst.ajshp")]
        public void ShouldParseDefFirstExample()
        {
            Parser parser = new Parser(File.OpenText("DefFirst.ajshp"));
            object result = parser.ParseForm();

            Assert.IsNotNull(result);
            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        [DeploymentItem("Examples/DefFirst.ajshp")]
        public void ShouldEvaluateDefFirstExample()
        {
            Parser parser = new Parser(File.OpenText("DefFirst.ajshp"));
            Machine machine = new Machine();

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedFunction));

            DefinedFunction deffun = (DefinedFunction)result;

            Assert.AreEqual("first", deffun.Name);
        }

        [TestMethod]
        [DeploymentItem("Examples/DefFirst.ajshp")]
        public void ShouldEvaluateFirstDefinedInDefFirstExample()
        {
            Parser parser = new Parser(File.OpenText("DefFirst.ajshp"));
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());

            parser = new Parser("(first (list 1 2 3))");

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        [DeploymentItem("Examples/DefNext.ajshp")]
        public void ShouldParseDefNextExample()
        {
            Parser parser = new Parser(File.OpenText("DefNext.ajshp"));
            object result = parser.ParseForm();

            Assert.IsNotNull(result);
            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        [DeploymentItem("Examples/DefNext.ajshp")]
        public void ShouldEvaluateDefNextExample()
        {
            Parser parser = new Parser(File.OpenText("DefNext.ajshp"));
            Machine machine = new Machine();

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedFunction));

            DefinedFunction deffun = (DefinedFunction)result;

            Assert.AreEqual("next", deffun.Name);
        }

        [TestMethod]
        [DeploymentItem("Examples/DefNext.ajshp")]
        public void ShouldEvaluateNextDefinedInDefNextExample()
        {
            Parser parser = new Parser(File.OpenText("DefNext.ajshp"));
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());

            parser = new Parser("(next (list 0 1 2 3))");

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(IList));

            IList list = (IList)value;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        [DeploymentItem("Examples/DefRest.ajshp")]
        public void ShouldParseDefRestExample()
        {
            Parser parser = new Parser(File.OpenText("DefRest.ajshp"));
            object result = parser.ParseForm();

            Assert.IsNotNull(result);
            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        [DeploymentItem("Examples/DefRest.ajshp")]
        public void ShouldEvaluateDefRestExample()
        {
            Parser parser = new Parser(File.OpenText("DefRest.ajshp"));
            Machine machine = new Machine();

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedFunction));

            DefinedFunction deffun = (DefinedFunction)result;

            Assert.AreEqual("rest", deffun.Name);
        }

        [TestMethod]
        [DeploymentItem("Examples/DefRest.ajshp")]
        public void ShouldEvaluateRestDefinedInDefRestExample()
        {
            Parser parser = new Parser(File.OpenText("DefRest.ajshp"));
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());

            parser = new Parser("(rest (list 0 1 2 3))");

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(IList));

            IList list = (IList)value;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        [DeploymentItem("Examples/DefListOps.ajshp")]
        public void ShouldParseAndEvaluateDefListOpsExample()
        {
            Parser parser = new Parser(File.OpenText("DefListOps.ajshp"));
            Machine machine = new Machine();
            object result = parser.ParseForm();

            while (result != null)
            {
                machine.Evaluate(result);
                result = parser.ParseForm();
            }

            Assert.IsNotNull(machine.GetVariableValue(Utilities.GetFullName(Machine.AjSharpureCoreKey, "cons")));
            Assert.IsNotNull(machine.GetVariableValue(Utilities.GetFullName(Machine.AjSharpureCoreKey, "first")));
            Assert.IsNotNull(machine.GetVariableValue(Utilities.GetFullName(Machine.AjSharpureCoreKey, "next")));
            Assert.IsNotNull(machine.GetVariableValue(Utilities.GetFullName(Machine.AjSharpureCoreKey, "rest")));
            Assert.IsNotNull(machine.GetVariableValue(Utilities.GetFullName(Machine.AjSharpureCoreKey, "second")));
            Assert.IsNotNull(machine.GetVariableValue(Utilities.GetFullName(Machine.AjSharpureCoreKey, "ffirst")));
            Assert.IsNotNull(machine.GetVariableValue(Utilities.GetFullName(Machine.AjSharpureCoreKey, "nfirst")));
            Assert.IsNotNull(machine.GetVariableValue(Utilities.GetFullName(Machine.AjSharpureCoreKey, "fnext")));
            Assert.IsNotNull(machine.GetVariableValue(Utilities.GetFullName(Machine.AjSharpureCoreKey, "nnext")));
        }

        [TestMethod]
        [DeploymentItem("Examples/DefListOps.ajshp")]
        [DeploymentItem("Examples/ListOpsTests.ajshp")]
        public void ShouldTestListOpsFromExample()
        {
            Parser parser = new Parser(File.OpenText("DefListOps.ajshp"));
            Machine machine = new Machine();

            object result = parser.ParseForm();

            while (result != null)
            {
                machine.Evaluate(result);
                result = parser.ParseForm();
            }

            parser = new Parser(File.OpenText("ListOpsTests.ajshp"));

            result = parser.ParseForm();

            while (result != null)
            {
                object value = machine.Evaluate(result);
                
                Assert.IsNotNull(value);
                Assert.IsInstanceOfType(value, typeof(bool));
                Assert.IsTrue((bool)value);

                result = parser.ParseForm();
            }
        }
    }
}
