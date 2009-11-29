namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using AjSharpure.Compiler;
    using System.Collections;
    using AjSharpure.Primitives;

    [TestClass]
    public class VariableTests
    {
        [TestMethod]
        public void CreateWithNameAndNamespace()
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
        public void CreateWithNameAndImplicitNamespace()
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
        public void BeEqualToVariableWithSameName()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable variable = Variable.Intern(machine, "foo/bar");
            Variable variable2 = Variable.Intern(machine, "foo/bar");

            Assert.AreEqual(variable, variable2);
            Assert.AreEqual(variable.GetHashCode(), variable2.GetHashCode());
        }

        [TestMethod]
        public void BeNotEqualToVariableWithOtherName()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable variable = Variable.Intern(machine, "foo/bar");
            Variable variable2 = Variable.Intern(machine, "foo/bar2");

            Assert.AreNotEqual(variable, variable2);
        }

        [TestMethod]
        public void BeEqualToVariableWithSameNameAndNamespace()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable variable = Variable.Intern(machine, "foo", "bar");
            Variable variable2 = Variable.Intern(machine, "foo", "bar");

            Assert.AreEqual(variable, variable2);
            Assert.AreEqual(variable.GetHashCode(), variable2.GetHashCode());
        }

        [TestMethod]
        public void CompareToOtherVariables()
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
        public void CreateVariableWithNullMetadata()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable variable = Variable.Intern(machine, "foo/bar");
            IObject iobj = variable.WithMetadata(null);

            Assert.IsNotNull(iobj);
            Assert.IsTrue(variable == iobj);
        }

        [TestMethod]
        public void CreateVariableWithMetadata()
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
        public void ResetVariableMetadata()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable variable = Variable.Intern(machine, "foo/bar");

            variable.ResetMetadata(FakePersistentMap.Instance);

            Assert.IsNotNull(variable.Metadata);
            Assert.IsTrue(variable.Metadata == FakePersistentMap.Instance);
        }

        [TestMethod]
        public void CreateAVariable()
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
        public void BeEquals()
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
        public void SetMacroOnDefinedFunction()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("ns");

            Variable variable = Variable.Intern(machine, "ns/func");

            Parser parser = new Parser("[x y] (list x y) 1 2");
            object argumentNames = parser.ParseForm();
            object body = parser.ParseForm();

            DefinedFunction func = new DefinedFunction("simple-list", (ICollection)argumentNames, Utilities.ToExpression(body));

            machine.SetVariableValue(variable, func);

            variable.SetMacro(machine);

            object result = machine.GetVariableValue(variable);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedMacro));
        }

        [TestMethod]
        public void SetMacroOnDefinedMultiFunction()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("ns");
            FnStarPrimitive fnprim = new FnStarPrimitive();
            Parser parser = new Parser("([x] (+ x 1)) ([x y] (+ x y 1))");

            object[] parameters = new object[2];
            parameters[0] = parser.ParseForm();
            parameters[1] = parser.ParseForm();

            object result = fnprim.Apply(machine, machine.Environment, parameters);
            DefinedMultiFunction multifn = (DefinedMultiFunction)result;

            Variable variable = Variable.Intern(machine, "ns/func");

            machine.SetVariableValue(variable, multifn);

            variable.SetMacro(machine);

            object mresult = machine.GetVariableValue(variable);

            Assert.IsNotNull(mresult);
            Assert.IsInstanceOfType(mresult, typeof(DefinedMultiMacro));
        }
    }
}
