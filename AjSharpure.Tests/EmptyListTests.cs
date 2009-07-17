namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EmptyListTests
    {
        [TestMethod]
        public void ShouldGetNullInFirstAndNext()
        {
            Assert.IsNull(EmptyList.Instance.First());
            Assert.IsNull(EmptyList.Instance.Next());
        }

        [TestMethod]
        public void ShouldGetEmptyListInMore()
        {
            Assert.IsTrue(EmptyList.Instance == EmptyList.Instance.More());
        }

        [TestMethod]
        public void ShouldGetEmptyListWithNullMetadata()
        {
            Assert.IsTrue(EmptyList.Instance == EmptyList.Instance.WithMetadata(null));
        }
    }
}
