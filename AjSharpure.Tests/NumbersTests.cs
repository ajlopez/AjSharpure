namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NumbersTests
    {
        [TestMethod]
        public void ShouldAddNumbers()
        {
            Assert.AreEqual(0, Numbers.Add(1, -1));
            Assert.AreEqual(3, Numbers.Add(1, 2));
            Assert.AreEqual(3.14, Numbers.Add(1, 2.14));
        }

        [TestMethod]
        public void ShouldSubtractNumbers()
        {
            Assert.AreEqual(2, Numbers.Subtract(1, -1));
            Assert.AreEqual(-1, Numbers.Subtract(1, 2));
            Assert.AreEqual(1-2.14, Numbers.Subtract(1, 2.14));
        }

        [TestMethod]
        public void ShouldMultiplyNumbers()
        {
            Assert.AreEqual(-1, Numbers.Multiply(1, -1));
            Assert.AreEqual(2, Numbers.Multiply(1, 2));
            Assert.AreEqual(2.14, Numbers.Multiply(1, 2.14));
        }

        [TestMethod]
        public void ShouldDivideNumbers()
        {
            Assert.AreEqual(-1.0, Numbers.Divide(1, -1));
            Assert.AreEqual(0.5, Numbers.Divide(1, 2));
            Assert.AreEqual(1/2.14, Numbers.Divide(1, 2.14));
        }

        [TestMethod]
        public void OrNumbers()
        {
            Assert.AreEqual(0, Numbers.Or(0, 0));
            Assert.AreEqual(3, Numbers.Or(1, 2));
            Assert.AreEqual(7, Numbers.Or(2, 5));
            Assert.AreEqual(8, Numbers.Or(8, 8));
        }

        [TestMethod]
        public void AndNumbers()
        {
            Assert.AreEqual(0, Numbers.And(0, 0));
            Assert.AreEqual(0, Numbers.And(1, 2));
            Assert.AreEqual(1, Numbers.And(3, 5));
            Assert.AreEqual(8, Numbers.And(8, 8));
            Assert.AreEqual(3, Numbers.And(3, 3));
        }

        [TestMethod]
        public void XorNumbers()
        {
            Assert.AreEqual(0, Numbers.Xor(0, 0));
            Assert.AreEqual(0, Numbers.Xor(1, 1));
            Assert.AreEqual(6, Numbers.Xor(3, 5));
        }

        [TestMethod]
        public void NotNumbers()
        {
            Assert.AreEqual(-1, Numbers.Not(0));
            Assert.AreEqual(0, Numbers.Not(-1));
            Assert.AreEqual(-2, Numbers.Not(1));
        }

        [TestMethod]
        public void IncrementNumbers()
        {
            Assert.AreEqual(1, Numbers.Increment(0));
            Assert.AreEqual(2, Numbers.Increment(1));
            Assert.AreEqual(3, Numbers.Increment(2));
        }

        [TestMethod]
        public void DecrementNumbers()
        {
            Assert.AreEqual(-1, Numbers.Decrement(0));
            Assert.AreEqual(0, Numbers.Decrement(1));
            Assert.AreEqual(1, Numbers.Decrement(2));
        }
    }
}
