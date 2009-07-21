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
    }
}
