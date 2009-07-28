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
        public void ShouldCreateWithName()
        {
            Variable variable = Variable.Create("foo");

            Assert.IsNotNull(variable);
            Assert.AreEqual("foo", variable.Name);
            Assert.AreEqual("foo", variable.FullName);
            Assert.IsNull(variable.Namespace);
        }

        [TestMethod]
        public void ShouldCreateWithNameAndNamespace()
        {
            Variable variable = Variable.Create("foo", "bar");

            Assert.IsNotNull(variable);
            Assert.AreEqual("foo", variable.Namespace);
            Assert.AreEqual("bar", variable.Name);
            Assert.AreEqual("foo/bar", variable.FullName);
        }

        [TestMethod]
        public void ShouldCreateWithNameAndImplicitNamespace()
        {
            Variable variable = Variable.Create("foo/bar");

            Assert.IsNotNull(variable);
            Assert.AreEqual("foo", variable.Namespace);
            Assert.AreEqual("bar", variable.Name);
            Assert.AreEqual("foo/bar", variable.FullName);
        }

        [TestMethod]
        public void ShouldCreateWithDivideAsName()
        {
            Variable variable = Variable.Create("/");

            Assert.IsNotNull(variable);
            Assert.IsNull(variable.Namespace);
            Assert.AreEqual("/", variable.Name);
            Assert.AreEqual("/", variable.FullName);
        }

        [TestMethod]
        public void ShouldBeEqualToVariableWithSameName()
        {
            Variable variable = Variable.Create("foo");
            Variable variable2 = Variable.Create("foo");

            Assert.AreEqual(variable, variable2);
            Assert.AreEqual(variable.GetHashCode(), variable2.GetHashCode());
        }

        [TestMethod]
        public void ShouldBeNotEqualToVariableWithOtherName()
        {
            Variable variable = Variable.Create("foo");
            Variable variable2 = Variable.Create("bar");

            Assert.AreNotEqual(variable, variable2);
        }

        [TestMethod]
        public void ShouldBeEqualToVariableWithSameNameAndNamespace()
        {
            Variable variable = Variable.Create("foo", "bar");
            Variable variable2 = Variable.Create("foo", "bar");

            Assert.AreEqual(variable, variable2);
            Assert.AreEqual(variable.GetHashCode(), variable2.GetHashCode());
        }

        [TestMethod]
        public void ShouldCompareToOtherVariables()
        {
            Variable variableBar = Variable.Create("bar");
            Variable variableFooBar = Variable.Create("foo", "bar");
            Variable variableBarFoo = Variable.Create("bar", "foo");

            Assert.AreEqual(0, variableBar.CompareTo(variableBar));
            Assert.AreEqual(0, variableFooBar.CompareTo(variableFooBar));

            Assert.AreEqual(-1, variableBar.CompareTo(variableFooBar));
            Assert.AreEqual(1, variableFooBar.CompareTo(variableBar));

            Assert.AreEqual(1, variableFooBar.CompareTo(variableBarFoo));
            Assert.AreEqual(-1, variableBarFoo.CompareTo(variableFooBar));
        }

        [TestMethod]
        public void ShouldCreateVariableWithNullMetadata()
        {
            Variable variable = Variable.Create("bar");
            IObject iobj = variable.WithMetadata(null);

            Assert.IsNotNull(iobj);
            Assert.IsTrue(variable == iobj);
        }

        [TestMethod]
        public void ShouldCreateVariableWithMetadata()
        {
            Variable variable = Variable.Create("bar");
            IObject iobj = variable.WithMetadata(FakePersistentMap.Instance);

            Assert.IsNotNull(iobj);
            Assert.IsTrue(variable != iobj);
            Assert.IsNotNull(iobj.Metadata);
            Assert.IsTrue(iobj.Metadata == FakePersistentMap.Instance);
        }

        [TestMethod]
        public void ShouldResetVariableMetadata()
        {
            Variable variable = Variable.Create("bar");
            variable.ResetMetadata(FakePersistentMap.Instance);

            Assert.IsNotNull(variable.Metadata);
            Assert.IsTrue(variable.Metadata == FakePersistentMap.Instance);
        }

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
