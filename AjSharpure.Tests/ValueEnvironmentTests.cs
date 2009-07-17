namespace AjSharpure.Tests
{
    using System;
    using System.Collections;

    using AjSharpure;

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

            environment.SetLocalValue("foo", "bar");

            Assert.AreEqual("bar", environment.GetValue("foo"));
            Assert.IsNull(parent.GetValue("foo"));
        }

        [TestMethod]
        public void ShouldSetAndGetGlobalValue()
        {
            ValueEnvironment parent = new ValueEnvironment();
            ValueEnvironment environment = new ValueEnvironment(parent);

            environment.SetGlobalValue("foo", "bar");

            Assert.AreEqual("bar", environment.GetValue("foo"));
            Assert.AreEqual("bar", parent.GetValue("foo"));
        }
    }
}
