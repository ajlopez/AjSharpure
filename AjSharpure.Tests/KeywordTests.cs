namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure.Compiler;
    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class KeywordTests
    {
        [TestMethod]
        public void ShouldCreateWithName()
        {
            Keyword keyword = Keyword.Create("foo");

            Assert.IsNotNull(keyword);
            Assert.AreEqual("foo", keyword.Name);
            Assert.AreEqual("foo", keyword.FullName);
            Assert.IsNull(keyword.Namespace);
        }

        [TestMethod]
        public void ShouldCreateWithNameAndNamespace()
        {
            Keyword keyword = Keyword.Create("foo", "bar");

            Assert.IsNotNull(keyword);
            Assert.AreEqual("foo", keyword.Namespace);
            Assert.AreEqual("bar", keyword.Name);
            Assert.AreEqual("foo/bar", keyword.FullName);
        }

        [TestMethod]
        public void ShouldCreateWithNameAndImplicitNamespace()
        {
            Keyword keyword = Keyword.Create("foo/bar");

            Assert.IsNotNull(keyword);
            Assert.AreEqual("foo", keyword.Namespace);
            Assert.AreEqual("bar", keyword.Name);
            Assert.AreEqual("foo/bar", keyword.FullName);
        }

        [TestMethod]
        public void ShouldBeEqualToKeywordWithSameName()
        {
            Keyword keyword = Keyword.Create("foo");
            Keyword keyword2 = Keyword.Create("foo");

            Assert.AreEqual(keyword, keyword2);
            Assert.AreEqual(keyword.GetHashCode(), keyword2.GetHashCode());
        }

        [TestMethod]
        public void ShouldBeNotEqualToKeywordWithOtherName()
        {
            Keyword keyword = Keyword.Create("foo");
            Keyword keyword2 = Keyword.Create("bar");

            Assert.AreNotEqual(keyword, keyword2);
        }

        [TestMethod]
        public void ShouldBeEqualToKeywordWithSameNameAndNamespace()
        {
            Keyword keyword = Keyword.Create("foo", "bar");
            Keyword keyword2 = Keyword.Create("foo", "bar");

            Assert.AreEqual(keyword, keyword2);
            Assert.AreEqual(keyword.GetHashCode(), keyword2.GetHashCode());
        }

        [TestMethod]
        public void ShouldCompareToOtherKeywords()
        {
            Keyword keywordBar = Keyword.Create("bar");
            Keyword keywordFooBar = Keyword.Create("foo", "bar");
            Keyword keywordBarFoo = Keyword.Create("bar", "foo");

            Assert.AreEqual(0, keywordBar.CompareTo(keywordBar));
            Assert.AreEqual(0, keywordFooBar.CompareTo(keywordFooBar));

            Assert.AreEqual(-1, keywordBar.CompareTo(keywordFooBar));
            Assert.AreEqual(1, keywordFooBar.CompareTo(keywordBar));

            Assert.AreEqual(1, keywordFooBar.CompareTo(keywordBarFoo));
            Assert.AreEqual(-1, keywordBarFoo.CompareTo(keywordFooBar));
        }
    }
}
