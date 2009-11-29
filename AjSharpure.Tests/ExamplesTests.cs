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
        public void ParseDefConsExample()
        {
            Parser parser = new Parser(File.OpenText("DefCons.ajshp"));
            object result = parser.ParseForm();

            Assert.IsNotNull(result);
            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        [DeploymentItem("Examples/DefCons.ajshp")]
        public void EvaluateDefConsExample()
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
        public void EvaluateConsDefinedInDefConsExample()
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
        public void ParseDefFirstExample()
        {
            Parser parser = new Parser(File.OpenText("DefFirst.ajshp"));
            object result = parser.ParseForm();

            Assert.IsNotNull(result);
            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        [DeploymentItem("Examples/DefFirst.ajshp")]
        public void EvaluateDefFirstExample()
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
        public void EvaluateFirstDefinedInDefFirstExample()
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
        public void ParseDefNextExample()
        {
            Parser parser = new Parser(File.OpenText("DefNext.ajshp"));
            object result = parser.ParseForm();

            Assert.IsNotNull(result);
            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        [DeploymentItem("Examples/DefNext.ajshp")]
        public void EvaluateDefNextExample()
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
        public void EvaluateNextDefinedInDefNextExample()
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
        public void ParseDefRestExample()
        {
            Parser parser = new Parser(File.OpenText("DefRest.ajshp"));
            object result = parser.ParseForm();

            Assert.IsNotNull(result);
            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        [DeploymentItem("Examples/DefRest.ajshp")]
        public void EvaluateDefRestExample()
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
        public void EvaluateRestDefinedInDefRestExample()
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
        public void ParseAndEvaluateDefListOpsExample()
        {
            Parser parser = new Parser(File.OpenText("DefListOps.ajshp"));
            Machine machine = new Machine();
            object result = parser.ParseForm();

            while (result != null)
            {
                machine.Evaluate(result);
                result = parser.ParseForm();
            }

            Assert.IsNotNull(machine.GetVariableValue(Machine.AjSharpureCoreNamespaceName, "cons"));
            Assert.IsNotNull(machine.GetVariableValue(Machine.AjSharpureCoreNamespaceName, "first"));
            Assert.IsNotNull(machine.GetVariableValue(Machine.AjSharpureCoreNamespaceName, "next"));
            Assert.IsNotNull(machine.GetVariableValue(Machine.AjSharpureCoreNamespaceName, "rest"));
            Assert.IsNotNull(machine.GetVariableValue(Machine.AjSharpureCoreNamespaceName, "second"));
            Assert.IsNotNull(machine.GetVariableValue(Machine.AjSharpureCoreNamespaceName, "ffirst"));
            Assert.IsNotNull(machine.GetVariableValue(Machine.AjSharpureCoreNamespaceName, "nfirst"));
            Assert.IsNotNull(machine.GetVariableValue(Machine.AjSharpureCoreNamespaceName, "fnext"));
            Assert.IsNotNull(machine.GetVariableValue(Machine.AjSharpureCoreNamespaceName, "nnext"));
        }

        [TestMethod]
        [DeploymentItem("Examples/DefListOps.ajshp")]
        [DeploymentItem("Examples/ListOpsTests.ajshp")]
        public void TestListOpsFromExample()
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

        [TestMethod]
        [DeploymentItem("Examples/DefListOpsWithTests.ajshp")]
        public void DefAndTestListOpsFromExample()
        {
            Parser parser = new Parser(File.OpenText("DefListOpsWithTests.ajshp"));
            Machine machine = new Machine();

            object result = parser.ParseForm();

            int ntest = 0;

            while (result != null)
            {
                object value = machine.Evaluate(result);

                Assert.IsNotNull(value);

                if (value is bool)
                {
                    ntest++;
                    Assert.IsTrue((bool)value, string.Format("Test {0} failed", ntest));
                }
                else
                    Assert.IsInstanceOfType(value, typeof(DefinedFunction));

                result = parser.ParseForm();
            }
        }

        // TODO: Review, the code is not right, it doesn't run in Clojure
        //[TestMethod]
        //[DeploymentItem("Examples/DefMyListAsMacroWithTests.ajshp")]
        //public void DefAndTestMyListAsMacroFromExample()
        //{
        //    this.LoadAndEvaluateDefsWithTests("DefMyListAsMacroWithTests.ajshp");
        //}

        [TestMethod]
        [DeploymentItem("Examples/DefTypePredicatesWithTests.ajshp")]
        public void DefAndTestTypePredicatesFromExample()
        {
            Assert.AreEqual(4, this.LoadAndEvaluateDefsWithTests("DefTypePredicatesWithTests.ajshp"));
        }

        [TestMethod]
        [DeploymentItem("Examples/DefConsWithTests.ajshp")]
        public void DefAndTestConsFromExample()
        {
            Assert.AreEqual(1, this.LoadAndEvaluateDefsWithTests("DefConsWithTests.ajshp"));
        }

        [TestMethod]
        [DeploymentItem("Examples/DefFnWithTests.ajshp")]
        public void DefAndTestFnFromExample()
        {
            Assert.AreEqual(1, this.LoadAndEvaluateDefsWithTests("DefFnWithTests.ajshp"));
        }

        [TestMethod]
        [DeploymentItem("Examples/DefAssocWithTests.ajshp")]
        public void DefAndTestAssocFromExample()
        {
            Assert.AreEqual(1, this.LoadAndEvaluateDefsWithTests("DefAssocWithTests.ajshp"));
        }

        [TestMethod]
        [DeploymentItem("Examples/DefDefnWithTests.ajshp")]
        public void DefAndTestDefnFromExample()
        {
            Assert.IsTrue(this.LoadAndEvaluateDefsWithTests("DefDefnWithTests.ajshp") > 0);
        }

        [TestMethod]
        [DeploymentItem("Examples/DefCore.ajshp")]
        public void ParseDefCoreExample()
        {
            Parser parser = new Parser(File.OpenText("DefCore.ajshp"));

            for (object result = parser.ParseForm(); result != null; result = parser.ParseForm())
                Assert.IsInstanceOfType(result, typeof(IList));
        }

        [TestMethod]
        [DeploymentItem("Examples/DefCoreWithTests.ajshp")]
        public void DefAndTestCoreFromExample()
        {
            Assert.IsTrue(this.LoadAndEvaluateDefsWithTests("DefCoreWithTests.ajshp") > 20);
        }

        private int LoadAndEvaluateDefsWithTests(string filename)
        {
            Parser parser = new Parser(File.OpenText(filename));
            Machine machine = new Machine();

            object result = parser.ParseForm();

            int ntest = 0;

            while (result != null)
            {
                object value = machine.Evaluate(result);

                Assert.IsNotNull(value);

                if (value is bool)
                {
                    ntest++;
                    Assert.IsTrue((bool)value, string.Format("Test {0} failed", ntest));
                }
                //else
                //    Assert.IsInstanceOfType(value, typeof(IFunction));

                result = parser.ParseForm();
            }

            return ntest;
        }
    }
}
