namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RecursionDataTests
    {
        [TestMethod]
        public void ShouldCreateWithList()
        {
            List<int> numbers = new List<int>();
            numbers.Add(1);
            numbers.Add(2);
            numbers.Add(3);

            RecursionData data = new RecursionData(numbers);

            Assert.IsNotNull(data.Arguments);
            Assert.AreEqual(3, data.Arguments.Length);
            Assert.AreEqual(1, data.Arguments[0]);
            Assert.AreEqual(2, data.Arguments[1]);
            Assert.AreEqual(3, data.Arguments[2]);
        }

        [TestMethod]
        public void ShouldCreateWithObjectArray()
        {
            RecursionData data = new RecursionData(new int[] { 1, 2, 3 });

            Assert.IsNotNull(data.Arguments);
            Assert.AreEqual(3, data.Arguments.Length);
            Assert.AreEqual(1, data.Arguments[0]);
            Assert.AreEqual(2, data.Arguments[1]);
            Assert.AreEqual(3, data.Arguments[2]);
        }
    }
}
