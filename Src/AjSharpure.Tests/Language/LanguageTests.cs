namespace AjSharpure.Tests.Language
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure.Language;
    
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LanguageTests
    {
        [TestMethod]
        public void DelayDereference()
        {
            FakeFn fn = new FakeFn();

            Delay delay = new Delay(fn);

            Assert.AreEqual(0, fn.Counter);

            object result = delay.Dereference();

            Assert.AreEqual(1, result);

            object result2 = delay.Dereference();

            Assert.AreEqual(1, result);
            Assert.AreEqual(1, fn.Counter);

            fn.Invoke();

            Assert.AreEqual(2, fn.Counter);
        }

        [TestMethod]
        public void DelayForceObjects()
        {
            Assert.AreEqual(1, Delay.Force(1));
            Assert.AreEqual("foo", Delay.Force("foo"));
            Assert.IsNull(Delay.Force(null));
            ISequence seq = PersistentList.Create(new object[] { 1, 2, 3 }).ToSequence();
            Assert.AreEqual(seq, Delay.Force(seq));
        }

        [TestMethod]
        public void DelayForceDelayedObject()
        {
            FakeFn fn = new FakeFn();
            Delay delay = new Delay(fn);
            Assert.AreEqual(1, Delay.Force(delay));
            Assert.AreEqual(1, Delay.Force(delay));
            Assert.AreEqual(1, fn.Counter);
        }
    }
}
