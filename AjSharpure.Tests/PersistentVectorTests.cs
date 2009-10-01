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
    public class PersistentVectorTests
    {
        [TestMethod]
        public void CreateVectorFromArray()
        {
            PersistentVector vector = PersistentVector.Create(new object[] { 1, 2, 3 });

            Assert.IsNotNull(vector);
            Assert.AreEqual(3, vector.Count);

            Assert.AreEqual(1, vector[0]);
            Assert.AreEqual(2, vector[1]);
            Assert.AreEqual(3, vector[2]);

            Assert.IsNull(vector.Metadata);
        }

        [TestMethod]
        public void CreateVectorFromSequence()
        {
            PersistentVector vector = PersistentVector.Create(Utilities.ToSequence(new object[] { 1, 2, 3 }));

            Assert.IsNotNull(vector);
            Assert.AreEqual(3, vector.Count);

            Assert.AreEqual(1, vector[0]);
            Assert.AreEqual(2, vector[1]);
            Assert.AreEqual(3, vector[2]);

            Assert.IsNull(vector.Metadata);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RaiseIfTrySetAValue()
        {
            PersistentVector vector = PersistentVector.Create(Utilities.ToSequence(new object[] { 1, 2, 3 }));

            vector[0] = 2;
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void RaiseIfIndexIsOutOfRange()
        {
            PersistentVector vector = PersistentVector.Create(Utilities.ToSequence(new object[] { 1, 2, 3 }));

            Assert.AreEqual(0, vector[3]);
        }

        [TestMethod]
        public void CreateVectorUsingCons()
        {
            PersistentVector vector = PersistentVector.Create((IList)null);

            vector = vector.Cons(1);
            vector = vector.Cons(2);
            vector = vector.Cons(3);

            Assert.AreEqual(3, vector.Count);
            Assert.AreEqual(1, vector[0]);
            Assert.AreEqual(2, vector[1]);
            Assert.AreEqual(3, vector[2]);
        }

        [TestMethod]
        public void CreateVectorWithThousandElements()
        {
            int[] numbers = new int[1000];

            for (int k = 0; k < numbers.Length; k++)
                numbers[k] = (k + 1) * 2;

            PersistentVector vector = PersistentVector.Create(numbers);

            Assert.IsNotNull(vector);
            Assert.AreEqual(numbers.Length, vector.Count);

            for (int k = 0; k < vector.Count; k++)
                Assert.AreEqual((k + 1) * 2, vector[k]);
        }

        [TestMethod]
        public void VectorsWithSameElementsAreEqual()
        {
            PersistentVector vector1 = PersistentVector.Create(new int[] { 1, 2, 3 });
            PersistentVector vector2 = PersistentVector.Create(new int[] { 1, 2, 3 });

            Assert.IsTrue(vector1.Equals(vector2));
            Assert.IsTrue(vector2.Equals(vector1));
        }

        [TestMethod]
        public void VectorsWithDifferentElementsAreNotEqual()
        {
            PersistentVector vector1 = PersistentVector.Create(new int[] { 1, 2, 3 });
            PersistentVector vector2 = PersistentVector.Create(new int[] { 3, 2, 1 });

            Assert.IsFalse(vector1.Equals(vector2));
            Assert.IsFalse(vector2.Equals(vector1));
        }

        [TestMethod]
        public void VectorsWithDifferentCountAreNotEqual()
        {
            PersistentVector vector1 = PersistentVector.Create(new int[] { 1, 2, 3 });
            PersistentVector vector2 = PersistentVector.Create(new int[] { 1, 2, 3, 4 });

            Assert.IsFalse(vector1.Equals(vector2));
            Assert.IsFalse(vector2.Equals(vector1));
        }

        [TestMethod]
        public void VectorsWithSameElementsHaveSameHashCode()
        {
            PersistentVector vector1 = PersistentVector.Create(new int[] { 1, 2, 3 });
            PersistentVector vector2 = PersistentVector.Create(new int[] { 1, 2, 3 });

            Assert.AreEqual(vector1.GetHashCode(), vector2.GetHashCode());
        }

        [TestMethod]
        public void VectorAndPersistentListWithSameElementsAreEqual() 
        {
            PersistentVector vector = PersistentVector.Create(new int[] { 1, 2, 3 });
            IPersistentList list = PersistentList.Create(new int[] { 1, 2, 3 });

            Assert.IsTrue(vector.Equals(list));
            Assert.IsTrue(list.Equals(vector));
        }

        [TestMethod]
        public void VectorAndPersistentListWithSameElementsHaveSameHashCode()
        {
            PersistentVector vector = PersistentVector.Create(new int[] { 1, 2, 3 });
            IPersistentList list = PersistentList.Create(new int[] { 1, 2, 3 });

            Assert.AreEqual(vector.GetHashCode(), list.GetHashCode());
        }

        [TestMethod]
        public void VectorAndPersistentListWithDifferentElementsAreNotEqual()
        {
            PersistentVector vector = PersistentVector.Create(new int[] { 1, 2, 3 });
            IPersistentList list = PersistentList.Create(new int[] { 3, 2, 1 });

            Assert.IsFalse(vector.Equals(list));
            Assert.IsFalse(list.Equals(vector));
        }

        [TestMethod]
        public void VectorAndPersistentListWithDifferentCountAreNotEqual()
        {
            PersistentVector vector = PersistentVector.Create(new int[] { 1, 2, 3 });
            IPersistentList list = PersistentList.Create(new int[] { 1, 2, 3, 4 });

            Assert.IsFalse(vector.Equals(list));
            Assert.IsFalse(list.Equals(vector));
        }
    }
}
