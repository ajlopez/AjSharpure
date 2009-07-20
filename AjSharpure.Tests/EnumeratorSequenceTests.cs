namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EnumeratorSequenceTests
    {
        [TestMethod]
        public void ShouldGetNullIfNullSequence()
        {
            Assert.IsNull(EnumeratorSequence.Create(null));
        }

        [TestMethod]
        public void ShouldGetNullIfEmptySequence()
        {
            Assert.IsNull(EnumeratorSequence.Create("".GetEnumerator()));
        }

        [TestMethod]
        public void ShouldCreateSequenceFromStringEnumerator()
        {
            ISequence sequence = EnumeratorSequence.Create("foo".GetEnumerator());

            Assert.AreEqual('f', sequence.First());
            Assert.AreEqual('o', sequence.Next().First());
            Assert.AreEqual('o', sequence.Next().Next().First());

            Assert.IsNull(sequence.Next().Next().Next());
        }

        [TestMethod]
        public void ShouldCreateSequenceFromArrayEnumerator()
        {
            ISequence sequence = EnumeratorSequence.Create((new int[] { 1, 2, 3 }).GetEnumerator());

            for (int k = 0; k < 3; k++, sequence = sequence.Next())
                Assert.AreEqual(k + 1, sequence.First());

            Assert.IsNull(sequence);
        }

        [TestMethod]
        public void ShouldCreateSequenceWithNullMetadata()
        {
            EnumeratorSequence sequence = EnumeratorSequence.Create((new int[] { 1, 2, 3 }).GetEnumerator());
            IObject iobj = sequence.WithMetadata(null);

            Assert.IsNotNull(iobj);
            Assert.IsInstanceOfType(iobj, typeof(EnumeratorSequence));
            Assert.IsTrue(sequence == iobj);
        }

        [TestMethod]
        public void ShouldCreateSequenceWithMetadata()
        {
            EnumeratorSequence sequence = EnumeratorSequence.Create((new int[] { 1, 2, 3 }).GetEnumerator());
            IObject iobj = sequence.WithMetadata(FakePersistentMap.Instance);

            Assert.IsNotNull(iobj);
            Assert.IsInstanceOfType(iobj, typeof(EnumeratorSequence));
            Assert.IsTrue(sequence != iobj);
        }
    }
}
