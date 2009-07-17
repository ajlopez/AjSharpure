﻿namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConsTests
    {
        [TestMethod]
        public void ShouldCreateWithOneObject()
        {
            Cons cons = new Cons(1);

            Assert.AreEqual(1, cons.First());
            Assert.IsNull(cons.Next());
        }

        [TestMethod]
        public void ShouldCreateWithOneObjectAndRest()
        {
            Cons cons = new Cons(1, new Cons(2));

            Assert.AreEqual(1, cons.First());

            ISequence rest = cons.Next();

            Assert.IsNotNull(rest);
            Assert.IsInstanceOfType(rest, typeof(Cons));

            Assert.AreEqual(2, rest.First());
            Assert.IsNull(rest.Next());
        }

        [TestMethod]
        public void ShouldCreateWithNullMetadata()
        {
            Cons cons = new Cons(1, new Cons(2));
            IObject iobj = cons.WithMetadata(null);

            Assert.IsNotNull(iobj);
            Assert.IsInstanceOfType(iobj, typeof(Cons));
            Assert.IsTrue(iobj == cons);
        }
    }
}
