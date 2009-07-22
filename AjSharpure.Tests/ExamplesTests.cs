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
        public void ShouldEvaluateFirstDefinedInDefConsExample()
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
        public void ShouldEvaluateNextDefinedInDefConsExample()
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
    }
}
