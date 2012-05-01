namespace AjSharpure.Tests.Language
{
    using System;
    using System.Text;
    using System.Collections;
    using System.Linq;

    using AjSharpure;
    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ListObjectTests
    {
        private ListObject list;

        [TestInitialize]
        public void SetUp()
        {
            IList original = new ArrayList();

            original.Add(1);
            original.Add(2);
            original.Add(3);

            this.list = (ListObject) ListObject.Create(original);
        }

        [TestMethod]
        public void ShouldBeReadOnly()
        {
            Assert.IsTrue(this.list.IsReadOnly);
        }

        [TestMethod]
        public void ShouldBeFixedSize()
        {
            Assert.IsTrue(this.list.IsFixedSize);
        }

        [TestMethod]
        public void ShouldHaveOriginalValues()
        {
            Assert.AreEqual(3, this.list.Count);
            Assert.AreEqual(1, this.list[0]);
            Assert.AreEqual(2, this.list[1]);
            Assert.AreEqual(3, this.list[2]);
        }

        [TestMethod]
        public void ShouldRetrieveOriginalValuesUsingFirstAndNext()
        {
            Assert.AreEqual(1, this.list.First());
            Assert.AreEqual(2, this.list.Next().First());
            Assert.AreEqual(3, this.list.Next().Next().First());
            Assert.IsNull(this.list.Next().Next().Next());
        }

        [TestMethod]
        public void ShouldRetrieveOriginalValuesFromNext()
        {
            ISequence sequence = this.list.Next();
            
            Assert.IsInstanceOfType(sequence, typeof(IList));

            IList list = (IList)sequence;

            Assert.AreEqual(2, list[0]);
            Assert.AreEqual(3, list[1]);
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void ShouldGetEmptyListIfOriginalListIsNull()
        {
            IPersistentList result = ListObject.Create(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(EmptyList));
        }

        [TestMethod]
        public void FirstShouldBeNullIfEmptyList()
        {
            IPersistentList result = ListObject.Create(new ArrayList());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(EmptyList));
        }

        [TestMethod]
        public void ShouldGetPop()
        {
            IPersistentStack stack = this.list.Pop();

            Assert.IsNotNull(stack);
            Assert.AreEqual(2, stack.Count);
            Assert.AreEqual(2, stack.Peek());

            stack = stack.Pop();

            Assert.IsNotNull(stack);
            Assert.AreEqual(1, stack.Count);
            Assert.AreEqual(3, stack.Peek());

            stack = stack.Pop();

            Assert.IsInstanceOfType(stack, typeof(EmptyList));
        }

        [TestMethod]
        public void ShouldRetrieveValuesUsingForEach()
        {
            int k = 0;

            foreach (object element in this.list)
                Assert.AreEqual(++k, element);
        }

        [TestMethod]
        public void ShouldRetrieveValuesFromNextSublistUsingForEach()
        {
            int k = 1;

            foreach (object element in (IList) this.list.Next())
                Assert.AreEqual(++k, element);
        }

        [TestMethod]
        public void ShouldGetContains()
        {
            Assert.IsTrue(this.list.Contains(1));
            Assert.IsTrue(this.list.Contains(2));
            Assert.IsTrue(this.list.Contains(3));
            Assert.IsFalse(this.list.Contains(4));
        }

        [TestMethod]
        public void ShouldGetContainsFromSublist()
        {
            IList sublist = (IList) this.list.Next();

            Assert.IsFalse(sublist.Contains(1));
            Assert.IsTrue(sublist.Contains(2));
            Assert.IsTrue(sublist.Contains(3));
            Assert.IsFalse(sublist.Contains(4));
        }

        [TestMethod]
        public void ShouldGetIndexOf()
        {
            Assert.AreEqual(0, this.list.IndexOf(1));
            Assert.AreEqual(1, this.list.IndexOf(2));
            Assert.AreEqual(2, this.list.IndexOf(3));
            Assert.AreEqual(-1, this.list.IndexOf(4));
        }

        [TestMethod]
        public void ShouldGetIndexOfFromSublist()
        {
            IList sublist = (IList)this.list.Next();

            Assert.AreEqual(-1, sublist.IndexOf(1));
            Assert.AreEqual(0, sublist.IndexOf(2));
            Assert.AreEqual(1, sublist.IndexOf(3));
            Assert.AreEqual(-1, sublist.IndexOf(4));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseIfSetValue()
        {
            this.list[0] = 10;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseIfClear()
        {
            this.list.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseIfRemove()
        {
            this.list.Remove(1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseIfRemoveAt()
        {
            this.list.RemoveAt(1);
        }

        [TestMethod]
        public void ShouldGetSameObjectWithNullMetadata()
        {
            IObject iobj = this.list.WithMetadata(null);

            Assert.IsNotNull(iobj);
            Assert.IsTrue(iobj == this.list);
        }

        [TestMethod]
        public void ShouldGetAnotherListWithMetadata()
        {
            IObject iobj = this.list.WithMetadata(FakePersistentMap.Instance);

            Assert.IsNotNull(iobj);
            Assert.IsTrue(iobj != this.list);
            Assert.IsInstanceOfType(iobj, typeof(ListObject));
        }
    }
}
