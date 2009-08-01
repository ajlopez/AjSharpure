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
        public void ShouldCreateWithNameAndNamespace()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable variable = Variable.Intern(machine, "foo", "bar");

            Assert.IsNotNull(variable);
            Assert.AreEqual("foo", variable.Namespace);
            Assert.AreEqual("bar", variable.Name);
            Assert.AreEqual("foo/bar", variable.FullName);
        }

        [TestMethod]
        public void ShouldCreateWithNameAndImplicitNamespace()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable variable = Variable.Intern(machine, "foo/bar");

            Assert.IsNotNull(variable);
            Assert.AreEqual("foo", variable.Namespace);
            Assert.AreEqual("bar", variable.Name);
            Assert.AreEqual("foo/bar", variable.FullName);
        }

        [TestMethod]
        public void ShouldBeEqualToVariableWithSameName()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable variable = Variable.Intern(machine, "foo/bar");
            Variable variable2 = Variable.Intern(machine, "foo/bar");

            Assert.AreEqual(variable, variable2);
            Assert.AreEqual(variable.GetHashCode(), variable2.GetHashCode());
        }

        [TestMethod]
        public void ShouldBeNotEqualToVariableWithOtherName()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable variable = Variable.Intern(machine, "foo/bar");
            Variable variable2 = Variable.Intern(machine, "foo/bar2");

            Assert.AreNotEqual(variable, variable2);
        }

        [TestMethod]
        public void ShouldBeEqualToVariableWithSameNameAndNamespace()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable variable = Variable.Intern(machine, "foo", "bar");
            Variable variable2 = Variable.Intern(machine, "foo", "bar");

            Assert.AreEqual(variable, variable2);
            Assert.AreEqual(variable.GetHashCode(), variable2.GetHashCode());
        }

        [TestMethod]
        public void ShouldCompareToOtherVariables()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");
            machine.CreateNamespace("bar");

            Variable variableBar = Variable.Intern(machine, "bar", "bar");
            Variable variableFooBar = Variable.Intern(machine, "foo", "bar");
            Variable variableBarFoo = Variable.Intern(machine, "bar", "foo");

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
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable variable = Variable.Intern(machine, "foo/bar");
            IObject iobj = variable.WithMetadata(null);

            Assert.IsNotNull(iobj);
            Assert.IsTrue(variable == iobj);
        }

        [TestMethod]
        public void ShouldCreateVariableWithMetadata()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable variable = Variable.Intern(machine, "foo/bar");

            IObject iobj = variable.WithMetadata(FakePersistentMap.Instance);

            Assert.IsNotNull(iobj);
            Assert.IsTrue(variable != iobj);
            Assert.IsNotNull(iobj.Metadata);
            Assert.IsTrue(iobj.Metadata == FakePersistentMap.Instance);
        }

        [TestMethod]
        public void ShouldResetVariableMetadata()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable variable = Variable.Intern(machine, "foo/bar");

            variable.ResetMetadata(FakePersistentMap.Instance);

            Assert.IsNotNull(variable.Metadata);
            Assert.IsTrue(variable.Metadata == FakePersistentMap.Instance);
        }

        [TestMethod]
        public void ShouldCreateAVariable()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable var = Variable.Intern(machine, "foo", "bar");

            Assert.IsNotNull(var);
            Assert.AreEqual("foo", var.Namespace);
            Assert.AreEqual("bar", var.Name);
            Assert.AreEqual(Utilities.GetFullName("foo", "bar"), var.FullName);

            Variable var2 = Variable.Intern(machine, "foo", "bar");

            Assert.IsNotNull(var2);
            Assert.IsTrue(var.Equals(var2));
        }

        [TestMethod]
        public void ShouldBeEquals()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable var1 = Variable.Intern(machine, "foo", "bar");
            Variable var2 = Variable.Intern(machine, "foo", "bar");
            Variable var3 = Variable.Intern(machine, "foo/bar");

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
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable variable = Variable.Intern(machine, "foo/bar");
            Variable var1 = Variable.Intern(machine, "foo", "bar");
            Variable var2 = Variable.Intern(machine, "foo", "bar");
            Variable var3 = Variable.Intern(machine, "foo/bar");

            Assert.AreEqual(var1.GetHashCode(), var2.GetHashCode());
            Assert.AreEqual(var1.GetHashCode(), var3.GetHashCode());
        }
    }
}
