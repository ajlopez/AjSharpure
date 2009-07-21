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
    public class OperationsTests
    {
        [TestMethod]
        public void ShouldConvertNullToNullSequence()
        {
            Assert.IsNull(Operations.ToSequence(null));
        }

        [TestMethod]
        public void ShouldConsObjectToNull()
        {
            ISequence sequence = Operations.Cons(1, null);

            Assert.IsNotNull(sequence);
            Assert.AreEqual(1, sequence.First());
            Assert.IsNull(sequence.Next());
        }

        [TestMethod]
        public void NthElementOfNullShouldBeNull()
        {
            Assert.IsNull(Operations.NthElement(null, 0));
        }

        [TestMethod]
        public void NthElementOfStringShouldGetNthCharacter()
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
        public void NthElementOfArrayShouldGetNthElement()
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
        public void ShouldGetFirst()
        {
            Assert.IsNull(Operations.First(null));
            Assert.IsNull(Operations.First(EmptyList.Instance));
            Assert.AreEqual(1, Operations.First(new int[] { 1, 2, 3 }));
            Assert.AreEqual('f', Operations.First("foo"));
        }

        [TestMethod]
        public void ShouldGetSecond()
        {
            Assert.IsNull(Operations.Second(null));
            Assert.IsNull(Operations.Second(EmptyList.Instance));
            Assert.AreEqual(2, Operations.Second(new int[] { 1, 2, 3 }));
            Assert.AreEqual('o', Operations.Second("foo"));
        }

        [TestMethod]
        public void ShouldGetThird()
        {
            Assert.IsNull(Operations.Third(null));
            Assert.IsNull(Operations.Third(EmptyList.Instance));
            Assert.AreEqual(3, Operations.Third(new int[] { 1, 2, 3 }));
            Assert.AreEqual('o', Operations.Third("foo"));
        }

        [TestMethod]
        public void ShouldGetNext()
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
        public void ShouldGetMore()
        {
            Assert.AreEqual(EmptyList.Instance, Operations.More(null));
            Assert.AreEqual(EmptyList.Instance, Operations.More(EmptyList.Instance));

            ISequence result = Operations.More(new int[] { 1, 2, 3 });

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result.First());
            Assert.AreEqual(3, result.Next().First());
        }
    }
}
