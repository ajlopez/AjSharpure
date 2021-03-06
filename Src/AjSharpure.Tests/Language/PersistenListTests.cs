﻿namespace AjSharpure.Tests.Language
{
    using System;
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure;
    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PersistenListTests
    {
        [TestMethod]
        public void CreateListWithOneObject()
        {
            PersistentList list = new PersistentList(1);

            Assert.IsNotNull(list);
            Assert.AreEqual(1, list.Count);
            Assert.IsInstanceOfType(list.First(), typeof(int));
            Assert.AreEqual(1, (int)list.First());
            Assert.IsNull(list.Next());
        }

        [TestMethod]
        public void CreateFromArray()
        {
            int[] numbers = new int[] { 1, 2, 3 };
            PersistentList list = (PersistentList) PersistentList.Create(numbers);

            Assert.IsNotNull(list);
            Assert.AreEqual(3, list.Count);

            int nc = 0;

            foreach (object element in list)
            {
                nc++;
                Assert.IsInstanceOfType(element, typeof(int));
                Assert.AreEqual(nc, (int)element);
            }
        }

        [TestMethod]
        public void DoPop()
        {
            int[] numbers = new int[] { 0, 1, 2, 3 };
            PersistentList list = (PersistentList)PersistentList.Create(numbers);
            IPersistentStack popValue = list.Pop();

            Assert.IsNotNull(popValue);
            Assert.IsInstanceOfType(popValue, typeof(PersistentList));

            list = (PersistentList)popValue;

            Assert.AreEqual(3, list.Count);
            Assert.IsInstanceOfType(list, typeof(PersistentList));

            int nc = 0;

            foreach (object element in list)
            {
                nc++;
                Assert.IsInstanceOfType(element, typeof(int));
                Assert.AreEqual(nc, (int)element);
            }
        }

        [TestMethod]
        public void DoPopAsEmptyList()
        {
            IPersistentStack list = new PersistentList(1);
            list = list.Pop();

            Assert.IsNotNull(list);
            Assert.AreEqual(0, list.Count);
            Assert.IsInstanceOfType(list, typeof(EmptyList));
        }

        [TestMethod]
        public void DoPeek()
        {
            int[] numbers = new int[] { 0, 1, 2, 3 };
            IPersistentStack list = (PersistentList)PersistentList.Create(numbers);
            object obj = list.Peek();

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(int));
            Assert.AreEqual(0, (int)obj);
        }

        [TestMethod]
        public void GetEmpty()
        {
            int[] numbers = new int[] { 0, 1, 2, 3 };
            IPersistentStack list = (PersistentList)PersistentList.Create(numbers);
            IPersistentCollection coll = list.Empty;

            Assert.IsNotNull(coll);
            Assert.IsInstanceOfType(coll, typeof(EmptyList));
            Assert.AreEqual(0, coll.Count);
        }

        [TestMethod]
        public void CreateFromList()
        {
            List<int> numbers = new List<int>();
            numbers.Add(1);
            numbers.Add(2);
            numbers.Add(3);

            PersistentList list = (PersistentList)PersistentList.Create(numbers);

            Assert.IsNotNull(list);
            Assert.AreEqual(3, list.Count);

            int nc = 0;

            foreach (object element in list)
            {
                nc++;
                Assert.IsInstanceOfType(element, typeof(int));
                Assert.AreEqual(nc, (int)element);
            }
        }

        [TestMethod]
        public void CreateWithMeta()
        {
            List<int> numbers = new List<int>();
            numbers.Add(1);
            numbers.Add(2);
            numbers.Add(3);

            PersistentList original = (PersistentList)PersistentList.Create(numbers);
            IObject obj = original.WithMetadata(null);

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(PersistentList));

            PersistentList list = (PersistentList)obj;

            Assert.IsNotNull(list);
            Assert.AreEqual(3, list.Count);

            int nc = 0;

            foreach (object element in list)
            {
                nc++;
                Assert.IsInstanceOfType(element, typeof(int));
                Assert.AreEqual(nc, (int)element);
            }
        }

        [TestMethod]
        public void CreateFromPersistentList()
        {
            List<int> numbers = new List<int>();
            numbers.Add(1);
            numbers.Add(2);
            numbers.Add(3);

            PersistentList originalList = (PersistentList)PersistentList.Create(numbers);
            PersistentList list = (PersistentList)PersistentList.Create(originalList);

            Assert.IsNotNull(list);
            Assert.AreEqual(3, list.Count);

            int nc = 0;

            foreach (object element in list)
            {
                nc++;
                Assert.IsInstanceOfType(element, typeof(int));
                Assert.AreEqual(nc, (int)element);
            }
        }

        [TestMethod]
        public void GetAndProcessEnumerator()
        {
            List<int> numbers = new List<int>();
            numbers.Add(1);
            numbers.Add(2);
            numbers.Add(3);

            PersistentList originalList = (PersistentList)PersistentList.Create(numbers);
            PersistentList list = (PersistentList)PersistentList.Create(originalList);

            Assert.IsNotNull(list);
            Assert.AreEqual(3, list.Count);

            int nc = 0;

            IEnumerator enumerator = list.GetEnumerator();

            Assert.IsNotNull(enumerator);

            while (enumerator.MoveNext())
            {
                nc++;
                object element = enumerator.Current;
                Assert.IsInstanceOfType(element, typeof(int));
                Assert.AreEqual(nc, (int)element);
            }

            Assert.AreEqual(3, nc);

            nc = 0;

            enumerator.Reset();

            while (enumerator.MoveNext())
            {
                nc++;
                object element = enumerator.Current;
                Assert.IsInstanceOfType(element, typeof(int));
                Assert.AreEqual(nc, (int)element);
            }

            Assert.AreEqual(3, nc);
        }

        [TestMethod]
        public void ConvertToSequenceAsItself()
        {
            int[] numbers = new int[] { 1, 2, 3 };
            PersistentList list = (PersistentList)PersistentList.Create(numbers);
            ISequence sequence = list.ToSequence();
            Assert.IsNotNull(sequence);
            Assert.IsTrue(sequence == list);
        }

        [TestMethod]
        public void ConsToProduceAPersistentList()
        {
            int[] numbers = new int[] { 1, 2, 3 };
            PersistentList original = (PersistentList)PersistentList.Create(numbers);
            IPersistentCollection coll = original.Cons(0);

            Assert.IsNotNull(coll);
            Assert.IsInstanceOfType(coll, typeof(PersistentList));

            PersistentList list = (PersistentList) coll;

            Assert.AreEqual(4, list.Count);

            int nc = 0;

            foreach (object element in list)
            {
                Assert.IsInstanceOfType(element, typeof(int));
                Assert.AreEqual(nc, (int)element);
                nc++;
            }
        }

        [TestMethod]
        public void ReduceAddingIntegers()
        {
            int[] numbers = new int[] { 1, 2, 3 };
            PersistentList list = (PersistentList)PersistentList.Create(numbers);
            object value = list.Reduce(new AddIntegersFunction());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(6, value);
        }

        [TestMethod]
        public void ReduceAddingIntegersUsingSeed()
        {
            int[] numbers = new int[] { 1, 2, 3 };
            PersistentList list = (PersistentList)PersistentList.Create(numbers);
            object value = list.Reduce(new AddIntegersFunction(), 7);

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(13, value);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RaiseWhenAdd()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));
            sequence.Add(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RaiseWhenRemove()
        {
            int[] numbers = new int[] { 1, 2, 3 };
            PersistentList list = (PersistentList)PersistentList.Create(numbers);
            list.Remove(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RaiseWhenRemoveAt()
        {
            int[] numbers = new int[] { 1, 2, 3 };
            PersistentList list = (PersistentList)PersistentList.Create(numbers);
            list.RemoveAt(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RaiseWhenClear()
        {
            int[] numbers = new int[] { 1, 2, 3 };
            PersistentList list = (PersistentList)PersistentList.Create(numbers);
            list.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RaiseWhenInsert()
        {
            int[] numbers = new int[] { 1, 2, 3 };
            PersistentList list = (PersistentList)PersistentList.Create(numbers);
            list.Insert(0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RaiseWhenSetValue()
        {
            int[] numbers = new int[] { 1, 2, 3 };
            PersistentList list = (PersistentList)PersistentList.Create(numbers);
            list[0] = 1;
        }

        [TestMethod]
        public void GetEmptyListIfNoElements()
        {
            IPersistentList list = PersistentList.Create(new ArrayList());

            Assert.IsNotNull(list);
            Assert.IsInstanceOfType(list, typeof(EmptyList));
        }

        [TestMethod]
        public void TwoPersistentListsWithSameElementsAreEqual()
        {
            IPersistentList list1 = PersistentList.Create(new int[] { 1, 2, 3 });
            IPersistentList list2 = PersistentList.Create(new int[] { 1, 2, 3 });

            Assert.IsTrue(list1.Equals(list2));
        }

        [TestMethod]
        public void TwoPersistentListsWithSameElementsHaveSameHashCode()
        {
            IPersistentList list1 = PersistentList.Create(new int[] { 1, 2, 3 });
            IPersistentList list2 = PersistentList.Create(new int[] { 1, 2, 3 });

            Assert.AreEqual(list1.GetHashCode(), list2.GetHashCode());
        }

        [TestMethod]
        public void TwoPersistentListsWithDifferentElementsAreNotEqual()
        {
            IPersistentList list1 = PersistentList.Create(new int[] { 1, 2, 3 });
            IPersistentList list2 = PersistentList.Create(new int[] { 3, 2, 1 });

            Assert.IsFalse(list1.Equals(list2));
        }

        [TestMethod]
        public void TwoPersistentListsWithDifferentCountAreNotEqual()
        {
            IPersistentList list1 = PersistentList.Create(new int[] { 1, 2, 3 });
            IPersistentList list2 = PersistentList.Create(new int[] { 1, 2, 3, 4 });

            Assert.IsFalse(list1.Equals(list2));
            Assert.IsFalse(list2.Equals(list1));
        }
    }
}
