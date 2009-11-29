namespace AjSharpure.Tests
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
        public void CreateWithOneObject()
        {
            Cons cons = new Cons(1);

            Assert.AreEqual(1, cons.First());
            Assert.IsNull(cons.Next());
        }

        [TestMethod]
        public void CreateWithOneObjectAndRest()
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
        public void CreateWithNullMetadata()
        {
            Cons cons = new Cons(1, new Cons(2));
            IObject iobj = cons.WithMetadata(null);

            Assert.IsNotNull(iobj);
            Assert.IsInstanceOfType(iobj, typeof(Cons));
            Assert.IsTrue(iobj == cons);
        }

        [TestMethod]
        public void CreateWithMetadata()
        {
            Cons cons = new Cons(1, new Cons(2));
            IObject iobj = cons.WithMetadata(FakePersistentMap.Instance);

            Assert.IsNotNull(iobj);
            Assert.IsInstanceOfType(iobj, typeof(Cons));
            Assert.IsTrue(iobj != cons);
            Assert.IsTrue(FakePersistentMap.Instance == iobj.Metadata);
            Assert.AreEqual(2, cons.Count);
            Assert.AreEqual(1, cons.First());
            Assert.AreEqual(2, cons.Next().First());
            Assert.IsNull(cons.Next().Next());
        }

        [TestMethod]
        public void GetFirstElementByIndex()
        {
            Cons cons = new Cons(1, new Cons(2));
            Assert.AreEqual(1, cons[0]);
        }

        [TestMethod]
        public void GetSecondElementByIndex()
        {
            Cons cons = new Cons(1, new Cons(2));
            Assert.AreEqual(2, cons[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RaiseWhenTryingToSetElementByIndex()
        {
            Cons cons = new Cons(1, new Cons(2));
            cons[0] = 0;
        }

        [TestMethod]
        public void TwoConsWithSameElementsAreEqual()
        {
            Cons cons1 = new Cons(1, new Cons(2, new Cons(3)));
            Cons cons2 = new Cons(1, new Cons(2, new Cons(3)));

            Assert.IsTrue(cons1.Equals(cons2));
            Assert.IsTrue(cons2.Equals(cons1));
        }

        [TestMethod]
        public void TwoConsWithSameElementsHaveSameHashCode()
        {
            Cons cons1 = new Cons(1, new Cons(2, new Cons(3)));
            Cons cons2 = new Cons(1, new Cons(2, new Cons(3)));

            Assert.AreEqual(cons1.GetHashCode(), cons2.GetHashCode());
        }

        [TestMethod]
        public void TwoConsWithDifferentElementsAreNotEqual()
        {
            Cons cons1 = new Cons(1, new Cons(2, new Cons(3)));
            Cons cons2 = new Cons(3, new Cons(2, new Cons(1)));

            Assert.IsFalse(cons1.Equals(cons2));
            Assert.IsFalse(cons2.Equals(cons1));
        }

        [TestMethod]
        public void TwoConsWithDifferentCountAreNotEqual()
        {
            Cons cons1 = new Cons(1, new Cons(2, new Cons(3)));
            Cons cons2 = new Cons(1, new Cons(2, new Cons(3, new Cons(4))));

            Assert.IsFalse(cons1.Equals(cons2));
            Assert.IsFalse(cons2.Equals(cons1));
        }

        [TestMethod]
        public void CopyToArray()
        {
            Cons cons = new Cons(1, new Cons(2, new Cons(3)));
            object[] array = new object[3];

            cons.CopyTo(array, 0);

            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
        }

        [TestMethod]
        public void CopyToSubarray()
        {
            Cons cons = new Cons(1, new Cons(2, new Cons(3)));
            object[] array = new object[4];

            cons.CopyTo(array, 1);

            Assert.AreEqual(1, array[1]);
            Assert.AreEqual(2, array[2]);
            Assert.AreEqual(3, array[3]);
        }
    }
}
