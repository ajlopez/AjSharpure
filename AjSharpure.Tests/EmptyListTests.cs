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
        public void ShouldBeFixedSizeAndReadOnlye()
        {
            Assert.IsTrue(EmptyList.Instance.IsFixedSize);
            Assert.IsTrue(EmptyList.Instance.IsReadOnly);
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

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenAdd()
        {
            EmptyList.Instance.Add(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenRemove()
        {
            EmptyList.Instance.Remove(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenRemoveAt()
        {
            EmptyList.Instance.RemoveAt(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenClear()
        {
            EmptyList.Instance.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenInsert()
        {
            EmptyList.Instance.Insert(0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldRaiseWhenSetValue()
        {
            EmptyList.Instance[0] = 1;
        }
    }
}
