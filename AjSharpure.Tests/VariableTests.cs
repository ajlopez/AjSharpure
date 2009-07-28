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

        [TestMethod]
        public void ShouldBeEquals()
        {
            Variable var1 = Variable.Create("foo", "bar");
            Variable var2 = Variable.Create("foo", "bar");
            Variable var3 = Variable.Create("foo/bar");

            Assert.IsTrue(var1.Equals(var2));
            Assert.IsTrue(var2.Equals(var1));
            Assert.IsTrue(var1.Equals(var3));
            Assert.IsTrue(var3.Equals(var1));
            Assert.IsTrue(var2.Equals(var3));
            Assert.IsTrue(var3.Equals(var2));
        }

        [TestMethod]
        public void ShouldHaveSameHashCode()
        {
            Variable var1 = Variable.Create("foo", "bar");
            Variable var2 = Variable.Create("foo", "bar");
            Variable var3 = Variable.Create("foo/bar");

            Assert.AreEqual(var1.GetHashCode(), var2.GetHashCode());
            Assert.AreEqual(var1.GetHashCode(), var3.GetHashCode());
        }
    }
}
