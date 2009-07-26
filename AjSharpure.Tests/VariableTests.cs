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
        public void ShouldCreateAVariable()
        {
            Variable var = Variable.Create("foo", "bar");

            Assert.IsNotNull(var);
            Assert.AreEqual("foo", var.Namespace);
            Assert.AreEqual("bar", var.Name);
            Assert.AreEqual(Utilities.GetFullName("foo", "bar"), var.FullName);

            Variable var2 = Variable.Create("foo", "bar");

            Assert.IsNotNull(var2);
            Assert.IsTrue(var.Equals(var2));
        }
    }
}
