namespace AjSharpure.Tests.Language
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure;
    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NamespaceTests
    {
        private Machine machine;
        private Namespace ns;

        [TestInitialize]
        public void SetUp()
        {
            machine = new Machine();
            ns = machine.CreateNamespace("foo");
        }

        [TestMethod]
        public void SetAndGetValue()
        {
            ns.SetValue("foo", "bar");

            Assert.AreEqual("bar", ns.GetValue("foo"));
        }

        [TestMethod]
        public void SetResetAndGetValue()
        {
            this.ns.SetValue("foo", "bar");
            this.ns.SetValue("foo", "bar2");

            Assert.AreEqual("bar2", this.ns.GetValue("foo"));
        }

        [TestMethod]
        public void SetAndGetNewVariable()
        {
            Variable variable = Variable.Intern(this.machine, ns.Name, "bar");
            Variable var = ns.GetVariable("bar");

            Assert.IsNotNull(var);
            Assert.IsTrue(var == variable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfVariableHasAnotherNamespace()
        {
            Variable variable = Variable.Intern(this.machine, "other", "bar");
            ns.SetVariable(variable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfVariableAlredayDefinedInNamespace()
        {
            Variable variable = Variable.Intern(this.machine, this.ns.Name, "bar");
            ns.SetVariable(variable);
            ns.SetVariable(variable);
        }

        [TestMethod]
        public void GetNullIfUndefinedVariable()
        {
            Variable variable = ns.GetVariable("bar");

            Assert.IsNull(variable);
        }
    }
}
