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
    public class SymbolTests
    {
        [TestMethod]
        public void ShouldCreateWithName()
        {
            Symbol symbol = Symbol.Create("foo");

            Assert.IsNotNull(symbol);
            Assert.AreEqual("foo", symbol.Name);
            Assert.AreEqual("foo", symbol.FullName);
            Assert.IsNull(symbol.Namespace);
        }

        [TestMethod]
        public void ShouldCreateWithNameAndNamespace()
        {
            Symbol symbol = Symbol.Create("foo", "bar");

            Assert.IsNotNull(symbol);
            Assert.AreEqual("foo", symbol.Namespace);
            Assert.AreEqual("bar", symbol.Name);
            Assert.AreEqual("foo/bar", symbol.FullName);
        }

        [TestMethod]
        public void ShouldCreateWithNameAndImplicitNamespace()
        {
            Symbol symbol = Symbol.Create("foo/bar");

            Assert.IsNotNull(symbol);
            Assert.AreEqual("foo", symbol.Namespace);
            Assert.AreEqual("bar", symbol.Name);
            Assert.AreEqual("foo/bar", symbol.FullName);
        }

        [TestMethod]
        public void ShouldCreateWithDivideAsName()
        {
            Symbol symbol = Symbol.Create("/");

            Assert.IsNotNull(symbol);
            Assert.IsNull(symbol.Namespace);
            Assert.AreEqual("/", symbol.Name);
            Assert.AreEqual("/", symbol.FullName);
        }

        [TestMethod]
        public void ShouldBeEqualToSymbolWithSameName()
        {
            Symbol symbol = Symbol.Create("foo");
            Symbol symbol2 = Symbol.Create("foo");

            Assert.AreEqual(symbol, symbol2);
            Assert.AreEqual(symbol.GetHashCode(), symbol2.GetHashCode());
        }

        [TestMethod]
        public void ShouldBeNotEqualToSymbolWithOtherName()
        {
            Symbol symbol = Symbol.Create("foo");
            Symbol symbol2 = Symbol.Create("bar");

            Assert.AreNotEqual(symbol, symbol2);
        }

        [TestMethod]
        public void ShouldBeEqualToSymbolWithSameNameAndNamespace()
        {
            Symbol symbol = Symbol.Create("foo", "bar");
            Symbol symbol2 = Symbol.Create("foo", "bar");

            Assert.AreEqual(symbol, symbol2);
            Assert.AreEqual(symbol.GetHashCode(), symbol2.GetHashCode());
        }

        [TestMethod]
        public void ShouldCompareToOtherSymbols()
        {
            Symbol symbolBar = Symbol.Create("bar");
            Symbol symbolFooBar = Symbol.Create("foo", "bar");
            Symbol symbolBarFoo = Symbol.Create("bar", "foo");

            Assert.AreEqual(0, symbolBar.CompareTo(symbolBar));
            Assert.AreEqual(0, symbolFooBar.CompareTo(symbolFooBar));

            Assert.AreEqual(-1, symbolBar.CompareTo(symbolFooBar));
            Assert.AreEqual(1, symbolFooBar.CompareTo(symbolBar));

            Assert.AreEqual(1, symbolFooBar.CompareTo(symbolBarFoo));
            Assert.AreEqual(-1, symbolBarFoo.CompareTo(symbolFooBar));
        }

        [TestMethod]
        public void ShouldCreateSymbolWithNullMetadata()
        {
            Symbol symbol = Symbol.Create("bar");
            IObject iobj = symbol.WithMetadata(null);

            Assert.IsNotNull(iobj);
            Assert.IsTrue(symbol == iobj);
        }

        [TestMethod]
        public void ShouldCreateSymbolWithMetadata()
        {
            Symbol symbol = Symbol.Create("bar");
            IObject iobj = symbol.WithMetadata(FakePersistentMap.Instance);

            Assert.IsNotNull(iobj);
            Assert.IsTrue(symbol != iobj);
            Assert.IsNotNull(iobj.Metadata);
            Assert.IsTrue(iobj.Metadata == FakePersistentMap.Instance);
        }
    }
}
