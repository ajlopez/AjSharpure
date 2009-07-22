namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class VariableTests
    {
        [TestMethod]
        public void ShouldInternAVariable()
        {
            Variable var = Variable.Intern("foo", "bar", 1);

            Assert.IsNotNull(var);
            Assert.AreEqual(1, var.Value);
            Assert.AreEqual(1, var.Root);
            Assert.AreEqual("foo", var.Namespace);
            Assert.AreEqual("bar", var.Name);
            Assert.AreEqual(Utilities.GetFullName("foo", "bar"), var.FullName);

            Variable var2 = Variable.GetVariable("foo", "bar");

            Assert.IsNotNull(var2);
            Assert.IsTrue(var == var2);
        }

        [TestMethod]
        public void ShouldInternTwiceAVariableWithoutChangingRoot()
        {
            Variable var = Variable.Intern("foo", "bar", 1);
            Variable var2 = Variable.Intern("foo", "bar", 2);

            Assert.IsNotNull(var);
            Assert.IsNotNull(var2);
            Assert.IsTrue(var == var2);
            Assert.AreEqual(1, var.Root);
            Assert.AreEqual(1, var2.Value);
        }
    }
}
