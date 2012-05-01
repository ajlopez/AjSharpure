namespace AjSharpure.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EmptyListTests
    {
        [TestMethod]
        public void ShouldGetNullInFirstAndNext()
        {
            Assert.IsNull(EmptyList.Instance.First());
            Assert.IsNull(EmptyList.Instance.Next());
        }

        [TestMethod]
        public void ShouldBeFixedSizeAndReadOnly()
        {
            Assert.IsTrue(EmptyList.Instance.IsFixedSize);
            Assert.IsTrue(EmptyList.Instance.IsReadOnly);
        }

        [TestMethod]
        public void ShouldGetItselfInMore()
        {
            Assert.IsTrue(EmptyList.Instance.More() == EmptyList.Instance);
        }

        [TestMethod]
        public void ShouldBeEqualToItself()
        {
            Assert.IsTrue(EmptyList.Instance.Equals(EmptyList.Instance));
        }

        [TestMethod]
        public void ShouldBeNotEqualToObjects()
        {
            Assert.IsFalse(EmptyList.Instance.Equals(1));
            Assert.IsFalse(EmptyList.Instance.Equals("foo"));
        }

        [TestMethod]
        public void ShouldBeNotEqualToNotEmptyList()
        {
            Assert.IsFalse(EmptyList.Instance.Equals(PersistentList.Create(new int[] {1,2,3})));
        }

        [TestMethod]
        public void ShouldBeEquivalentToItself()
        {
            Assert.IsTrue(EmptyList.Instance.Equiv(EmptyList.Instance));
        }

        [TestMethod]
        public void ShouldBeNotEquivalentToObjects()
        {
            Assert.IsFalse(EmptyList.Instance.Equiv(1));
            Assert.IsFalse(EmptyList.Instance.Equiv("foo"));
        }

        [TestMethod]
        public void ShouldBeNotEquivalentToNotEmptyList()
        {
            Assert.IsFalse(EmptyList.Instance.Equiv(PersistentList.Create(new int[] { 1, 2, 3 })));
        }

        [TestMethod]
        public void ShouldGetEmptyListInMore()
        {
            Assert.IsTrue(EmptyList.Instance == EmptyList.Instance.More());
        }

        [TestMethod]
        public void ShouldNotContainAnyElement()
        {
            Assert.IsFalse(EmptyList.Instance.Contains(1));
            Assert.AreEqual(-1, EmptyList.Instance.IndexOf(1));
        }

        [TestMethod]
        public void ShouldGetEmptyListWithNullMetadata()
        {
            Assert.IsTrue(EmptyList.Instance == EmptyList.Instance.WithMetadata(null));
        }

        [TestMethod]
        public void ShouldGetEmptyListWithEmpty()
        {
            IPersistentCollection coll = EmptyList.Instance.Empty;

            Assert.IsNotNull(coll);
            Assert.IsInstanceOfType(coll, typeof(EmptyList));
            Assert.IsTrue(coll == EmptyList.Instance);
        }

        [TestMethod]
        public void ShouldConsWithObject()
        {
            IPersistentCollection coll = EmptyList.Instance.Cons(1);

            Assert.IsNotNull(coll);
            Assert.AreEqual(1, coll.Count);
            Assert.AreEqual(1, coll.ToSequence().First());
        }

        [TestMethod]
        public void ShouldConsAsISequenceWithObject()
        {
            ISequence sequence = ((ISequence) EmptyList.Instance).Cons(1);

            Assert.IsNotNull(sequence);
            Assert.AreEqual(1, sequence.Count);
            Assert.AreEqual(1, sequence.First());
        }

        [TestMethod]
        public void ShouldGetEmptyListWithMetadata()
        {
            IObject iobj = EmptyList.Instance.WithMetadata(FakePersistentMap.Instance);
            Assert.IsNotNull(iobj);
            Assert.IsInstanceOfType(iobj, typeof(EmptyList));
            Assert.IsTrue(EmptyList.Instance != iobj);
        }

        [TestMethod]
        public void ShouldGetNullInPeek()
        {
            Assert.IsNull(EmptyList.Instance.Peek());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseWhenPop()
        {
            EmptyList.Instance.Pop();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenAdd()
        {
            EmptyList.Instance.Add(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenRemove()
        {
            EmptyList.Instance.Remove(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenRemoveAt()
        {
            EmptyList.Instance.RemoveAt(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenClear()
        {
            EmptyList.Instance.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenInsert()
        {
            EmptyList.Instance.Insert(0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenSetValue()
        {
            EmptyList.Instance[0] = 1;
        }

        [TestMethod]
        public void ShouldGetHashCode()
        {
            Assert.AreEqual(1, EmptyList.Instance.GetHashCode());
        }

        [TestMethod]
        public void ShouldGetEnumerator()
        {
            IEnumerator enumerator = EmptyList.Instance.GetEnumerator();

            Assert.IsNotNull(enumerator);
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Reset();
            Assert.IsFalse(enumerator.MoveNext());
        }
    }
}
