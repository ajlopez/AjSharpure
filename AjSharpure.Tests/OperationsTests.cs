namespace AjSharpure.Tests
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
    public class OperationsTests
    {
        [TestMethod]
        public void ConvertNullToNullSequence()
        {
            Assert.IsNull(Operations.ToSequence(null));
        }

        [TestMethod]
        public void ConsObjectToNull()
        {
            ISequence sequence = Operations.Cons(1, null);

            Assert.IsNotNull(sequence);
            Assert.AreEqual(1, sequence.First());
            Assert.IsNull(sequence.Next());
        }

        [TestMethod]
        public void NthElementOfNullBeNull()
        {
            Assert.IsNull(Operations.NthElement(null, 0));
        }

        [TestMethod]
        public void NthElementOfStringGetNthCharacter()
        {
            object value;

            value = Operations.NthElement("foo", 0);
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(char));
            Assert.AreEqual('f', value);

            value = Operations.NthElement("foo", 1);
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(char));
            Assert.AreEqual('o', value);

            value = Operations.NthElement("foo", 2);
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(char));
            Assert.AreEqual('o', value);
        }

        [TestMethod]
        public void NthElementOfArrayGetNthElement()
        {
            char[] array = new char[] { 'f', 'o', 'o' };
            object value;

            value = Operations.NthElement(array, 0);
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(char));
            Assert.AreEqual('f', value);

            value = Operations.NthElement(array, 1);
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(char));
            Assert.AreEqual('o', value);

            value = Operations.NthElement(array, 2);
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(char));
            Assert.AreEqual('o', value);
        }

        [TestMethod]
        public void GetFirst()
        {
            Assert.IsNull(Operations.First(null));
            Assert.IsNull(Operations.First(EmptyList.Instance));
            Assert.AreEqual(1, Operations.First(new int[] { 1, 2, 3 }));
            Assert.AreEqual('f', Operations.First("foo"));
        }

        [TestMethod]
        public void GetSecond()
        {
            Assert.IsNull(Operations.Second(null));
            Assert.IsNull(Operations.Second(EmptyList.Instance));
            Assert.AreEqual(2, Operations.Second(new int[] { 1, 2, 3 }));
            Assert.AreEqual('o', Operations.Second("foo"));
        }

        [TestMethod]
        public void GetThird()
        {
            Assert.IsNull(Operations.Third(null));
            Assert.IsNull(Operations.Third(EmptyList.Instance));
            Assert.AreEqual(3, Operations.Third(new int[] { 1, 2, 3 }));
            Assert.AreEqual('o', Operations.Third("foo"));
        }

        [TestMethod]
        public void GetNext()
        {
            Assert.IsNull(Operations.Next(null));
            Assert.IsNull(Operations.Next(EmptyList.Instance));

            ISequence result = Operations.Next(new int[] { 1, 2, 3 });

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result.First());
            Assert.AreEqual(3, result.Next().First());
        }

        [TestMethod]
        public void GetMore()
        {
            Assert.AreEqual(EmptyList.Instance, Operations.More(null));
            Assert.AreEqual(EmptyList.Instance, Operations.More(EmptyList.Instance));

            ISequence result = Operations.More(new int[] { 1, 2, 3 });

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result.First());
            Assert.AreEqual(3, result.Next().First());
        }

        [TestMethod]
        public void ConjOnNullCollection()
        {
            IPersistentCollection result = Operations.Conj(null, 2);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result.ToSequence().First());
        }

        [TestMethod]
        public void ConjToPersistentList()
        {
            IPersistentCollection collection = PersistentList.Create(new object[] { 1, 2, 3 });
            IPersistentCollection result = Operations.Conj(collection, 4);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(4, result.ToSequence().First());
            Assert.AreEqual(3, Operations.NthElement(result, 3));
        }

        [TestMethod]
        public void ConjToPersistentVector()
        {
            IPersistentCollection collection = PersistentVector.Create(new object[] { 1, 2, 3 });
            IPersistentCollection result = Operations.Conj(collection, 4);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(1, result.ToSequence().First());
            Assert.AreEqual(4, Operations.NthElement(result, 3));
        }

        [TestMethod]
        public void AssociateToNullAssociative()
        {
            IAssociative result = Operations.Associate(null, "one", 1);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ContainsKey("one"));
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result.ValueAt("one"));
        }

        [TestMethod]
        public void AssociateToDictionaryObject()
        {
            IDictionary dict = new Hashtable();
            dict["one"] = 0;
            dict["two"] = 2;

            DictionaryObject dictionary = new DictionaryObject(dict);
            IAssociative result = Operations.Associate(dictionary, "one", 1);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ContainsKey("one"));
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result.ValueAt("one"));
            Assert.AreEqual(2, result.ValueAt("two"));
        }
    }
}
