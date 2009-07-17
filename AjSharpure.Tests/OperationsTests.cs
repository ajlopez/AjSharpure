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
    }
}
