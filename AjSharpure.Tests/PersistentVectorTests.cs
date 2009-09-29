namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

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
    }
}
