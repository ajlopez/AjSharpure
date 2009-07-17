namespace AjSharpure.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure;
    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UtilitiesTests
    {
        [TestMethod]
        public void ShouldGetZeroHashToNull()
        {
            Assert.AreEqual(0, Utilities.Hash(null));
        }

        [TestMethod]
        public void ShouldRecognizeSymbolAsEvaluable()
        {
            Symbol symbol = Symbol.Create("foo");

            Assert.IsTrue(Utilities.IsEvaluable(symbol));
        }

        [TestMethod]
        public void ShouldRecognizeConstantsAsEvaluable()
        {
            Assert.IsFalse(Utilities.IsEvaluable("foo"));
            Assert.IsFalse(Utilities.IsEvaluable(123));
        }

        [TestMethod]
        public void ShouldRecognizeIListAsEvaluable()
        {
            IList list = new ArrayList();

            list.Add(1);
            list.Add(2);
            list.Add(3);

            Assert.IsTrue(Utilities.IsEvaluable(list));
        }

        [TestMethod]
        public void ShouldRecognizeArrayAsNotEvaluable()
        {
            Assert.IsFalse(Utilities.IsEvaluable(new int[] { 1, 2, 3 }));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfTryToConvertArrayToExpression()
        {
            Utilities.ToExpression(new int[] { 1, 2, 3 });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfTryToConvertStringToExpression()
        {
            Utilities.ToExpression("foo");
        }

        [TestMethod]
        public void ShouldPrintInteger()
        {
            Assert.AreEqual("1", Utilities.PrintString(1));
            Assert.AreEqual("123", Utilities.PrintString(123));
        }

        [TestMethod]
        public void ShouldPrintString()
        {
            Assert.AreEqual("foo", Utilities.PrintString("foo"));
            Assert.AreEqual("bar", Utilities.PrintString("bar"));
        }

        [TestMethod]
        public void NullShouldBeEquivalentToNull()
        {
            Assert.IsTrue(Utilities.Equiv(null, null));
        }

        [TestMethod]
        public void NullShouldBeNotEquivalentToNotNull()
        {
            Assert.IsFalse(Utilities.Equiv(null, 1));
            Assert.IsFalse(Utilities.Equiv(1, null));
        }

        [TestMethod]
        public void ShouldConvertNullSequenceToNull()
        {
            Assert.IsNull(Utilities.ToSequence(null));
        }

        [TestMethod]
        public void ShouldConvertEmptySequenceToEmptySequence()
        {
            Assert.IsTrue(EmptyList.Instance == Utilities.ToSequence(EmptyList.Instance));
        }

        [TestMethod]
        public void ShouldConvertEnumeratorSequenceToItself()
        {
            ISequence sequence = EnumeratorSequence.Create((new int[] { 1, 2, 3 }).GetEnumerator());

            Assert.IsTrue(sequence == Utilities.ToSequence(sequence));
        }
    }
}
