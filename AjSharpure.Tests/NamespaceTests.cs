namespace AjSharpure.Tests
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
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaisIfResetValue()
        {
            ns.SetValue("foo", "bar");
            ns.SetValue("foo", "bar2");
        }
    }
}
