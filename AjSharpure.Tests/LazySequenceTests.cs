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
    public class LazySequenceTests
    {
        [TestMethod]
        public void ShouldCreateWithNumberSequence() 
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));

            Assert.IsNotNull(sequence);

            object value = sequence.First();

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(0, (int)value);
        }

        [TestMethod]
        public void ShouldContainNumbers()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));

            for (int k=0; k<100; k++)
                Assert.IsTrue(sequence.Contains(k));
        }

        [TestMethod]
        public void ShouldGetIndexOf()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));

            for (int k = 0; k < 100; k++)
                Assert.AreEqual(k, sequence.IndexOf(k));
        }

        [TestMethod]
        public void ShouldGetAndProcessEnumerator()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));
            IEnumerator enumerator = sequence.GetEnumerator();

            for (int k = 0; k < 100; k++)
            {
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(k, enumerator.Current);
            }
        }

        [TestMethod]
        public void ShouldCreateWithMeta()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));

            IObject iobj = sequence.WithMetadata(null);

            Assert.IsNotNull(iobj);
            Assert.IsInstanceOfType(iobj, typeof(LazySequence));
        }

        [TestMethod]
        public void ShouldCreateWithConsAndNumberSequence()
        {
            ISequence seq = new LazySequence(new NumberSequenceFunction(1));
            seq = seq.Cons(0);

            Assert.IsNotNull(seq);
            Assert.IsInstanceOfType(seq, typeof(ISequence));

            ISequence sequence = (ISequence) seq;

            object value = sequence.First();

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(0, (int)value);

            value = sequence.Next().First();

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(1, (int)value);
        }

        [TestMethod]
        public void ShouldCreateWithConsSequenceAndNumberSequence()
        {
            ISequence sequence = new LazySequence(new NumberSequenceFunction(1));
            sequence = sequence.Cons(0);

            Assert.IsNotNull(sequence);

            object value = sequence.First();

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(0, (int)value);

            value = sequence.Next().First();

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(1, (int)value);
        }

        [TestMethod]
        public void ShouldGetNext()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));

            ISequence value = sequence.Next();

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(LazySequence));
            Assert.AreEqual(1, value.First());
        }

        [TestMethod]
        public void ShouldGetHashCode()
        {
            LazySequence sequence = new LazySequence(new DeferredSequenceFunction(EmptyList.Instance));
            Assert.AreEqual(sequence.GetHashCode(), EmptyList.Instance.GetHashCode());
        }

        [TestMethod]
        public void ShouldGetMore()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));

            ISequence value = sequence.More();

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(LazySequence));
            Assert.AreEqual(1, value.First());
        }

        // TODO review if there are equals
        //[TestMethod]
        //public void ShouldBeEqualToEmptyList()
        //{
        //    LazySequence sequence = new LazySequence(new NullSequenceFunction());

        //    Assert.IsTrue(sequence.Equals(EmptyList.Instance));
        //}

        [TestMethod]
        public void ShouldBeEqualToNull()
        {
            LazySequence sequence = new LazySequence(new NullSequenceFunction());

            Assert.IsTrue(sequence.Equals(null));
        }

        [TestMethod]
        public void ShouldBeEquivToNull()
        {
            LazySequence sequence = new LazySequence(new NullSequenceFunction());

            Assert.IsTrue(sequence.Equiv(null));
        }

        [TestMethod]
        public void ShouldGetZeroCountFromNull()
        {
            LazySequence sequence = new LazySequence(new NullSequenceFunction());

            Assert.AreEqual(0, sequence.Count);
        }

        [TestMethod]
        public void ShouldGetEmptyList()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(10));

            ISequence coll = sequence.Empty;

            Assert.IsNotNull(coll);
            Assert.IsInstanceOfType(coll, typeof(EmptyList));
        }

        [TestMethod]
        public void ShouldGetNextFromNull()
        {
            LazySequence sequence = new LazySequence(new NullSequenceFunction());

            ISequence value = sequence.Next();

            Assert.IsNull(value);
        }

        [TestMethod]
        public void ShouldGetMoreFromNull()
        {
            LazySequence sequence = new LazySequence(new NullSequenceFunction());

            ISequence value = sequence.More();

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(EmptyList));
        }

        [TestMethod]
        public void ShouldGetSecondNumber()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));

            Assert.IsNotNull(sequence);

            object value = sequence.Next().First();

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(1, (int)value);
        }

        [TestMethod]
        public void ShouldGetOneHundredNumbers()
        {
            ISequence sequence = new LazySequence(new NumberSequenceFunction(0));

            Assert.IsNotNull(sequence);

            for (int k = 0; k < 100; k++)
            {
                object value = sequence.First();

                Assert.IsNotNull(value);
                Assert.IsInstanceOfType(value, typeof(int));
                Assert.AreEqual(k, (int)value);

                sequence = sequence.Next();
            }
        }

        [TestMethod]
        public void ShouldGetOneHundredValues()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));

            Assert.IsNotNull(sequence);

            for (int k = 0; k < 100; k++)
            {
                object value = sequence[k];

                Assert.IsNotNull(value);
                Assert.IsInstanceOfType(value, typeof(int));
                Assert.AreEqual(k, (int)value);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenAdd()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));
            sequence.Add(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenRemove()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));
            sequence.Remove(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenRemoveAt()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));
            sequence.RemoveAt(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenClear()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));
            sequence.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenInsert()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));
            sequence.Insert(0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenSetValue()
        {
            LazySequence sequence = new LazySequence(new NumberSequenceFunction(0));
            sequence[0] = 1;
        }
    }
}
