namespace AjSharpure.Tests
{
    using System;
    using System.Collections;

    using AjSharpure;
    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ValueEnvironmentTests
    {
        [TestMethod]
        public void ShouldSetValue()
        {
            ValueEnvironment environment = new ValueEnvironment();

            environment.SetValue("foo", "bar");
            Assert.AreEqual("bar", environment.GetValue("foo"));
        }

        [TestMethod]
        public void ShouldSetVariableValue()
        {
            ValueEnvironment environment = new ValueEnvironment();
            Variable var = Variable.Intern("ns", "foo", null);
            var.Value = "bar";

            environment.SetValue(var.FullName, var);
            Assert.AreEqual("bar", environment.GetValue(var.FullName));
        }

        [TestMethod]
        public void ShouldSetVariableValueUsingRoot()
        {
            ValueEnvironment environment = new ValueEnvironment();
            Variable var = Variable.Intern("ns","foo","bar");

            environment.SetValue(var.FullName, var);
            Assert.AreEqual("bar", environment.GetValue(var.FullName));
        }

        [TestMethod]
        public void ShouldGetNullIfNoValue()
        {
            ValueEnvironment environment = new ValueEnvironment();
           
            Assert.IsNull(environment.GetValue("foo"));
        }

        [TestMethod]
        public void ShouldGetValueFromParent()
        {
            ValueEnvironment parent = new ValueEnvironment();
            ValueEnvironment environment = new ValueEnvironment(parent);

            parent.SetValue("foo", "bar");

            Assert.AreEqual("bar", environment.GetValue("foo"));
        }

        [TestMethod]
        public void ShouldSetAndGetLocalValue()
        {
            ValueEnvironment parent = new ValueEnvironment();
            ValueEnvironment environment = new ValueEnvironment(parent);

            environment.SetValue("foo", "bar");

            Assert.AreEqual("bar", environment.GetValue("foo"));
            Assert.IsNull(parent.GetValue("foo"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfTryToSetAnAlreadyDefinedValue()
        {
            ValueEnvironment environment = new ValueEnvironment();

            environment.SetValue("foo", "bar");
            environment.SetValue("foo", "newbar");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfTryToSetAnAlreadyDefinedValueWithResetInFalse()
        {
            ValueEnvironment environment = new ValueEnvironment();

            environment.SetValue("foo", "bar");
            environment.SetValue("foo", "newbar", false);
        }

        [TestMethod]
        public void ShouldResetValueUsingResetInTrue()
        {
            ValueEnvironment environment = new ValueEnvironment();

            environment.SetValue("foo", "bar");
            environment.SetValue("foo", "rebar", true);
        }
    }
}
