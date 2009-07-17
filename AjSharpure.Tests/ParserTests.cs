namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure.Compiler;
    using AjSharpure.Expressions;
    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ShouldParseBooleans()
        {
            Parser parser = new Parser("true false");

            object value = parser.ParseForm();

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(bool));
            Assert.IsTrue((bool)value);

            value = parser.ParseForm();

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(bool));
            Assert.IsFalse((bool)value);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldParseSymbol()
        {
            Parser parser = new Parser("foo");

            object form = parser.ParseForm();

            Assert.IsNotNull(form);
            Assert.IsInstanceOfType(form, typeof(Symbol));

            Symbol symbol = (Symbol)form;

            Assert.AreEqual("foo", symbol.Name);
            Assert.IsNull(symbol.Namespace);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldParseEmptyString()
        {
            Parser parser = new Parser(string.Empty);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldParseString()
        {
            Parser parser = new Parser("\"foo\"");

            object form = parser.ParseForm();

            Assert.IsNotNull(form);
            Assert.IsInstanceOfType(form, typeof(string));

            Assert.AreEqual("foo", form);
        }

        [TestMethod]
        public void ShouldParseInteger()
        {
            Parser parser = new Parser("123");

            object form = parser.ParseForm();

            Assert.IsNotNull(form);
            Assert.IsInstanceOfType(form, typeof(int));

            Assert.AreEqual(123, (int) form);
        }

        [TestMethod]
        public void ShouldParseList()
        {
            Parser parser = new Parser("(1 2 3)");

            object obj = parser.ParseForm();

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(IList));

            IList list = (IList)obj;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldParseArray()
        {
            Parser parser = new Parser("[1 2 3]");

            object obj = parser.ParseForm();

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(object[]));

            object[] array = (object[])obj;

            Assert.AreEqual(3, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldParseMap()
        {
            Parser parser = new Parser("{:one 1 :two 2 :three 3}");

            object obj = parser.ParseForm();

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(IDictionary));

            IDictionary dictionary = (IDictionary) obj;

            Assert.AreEqual(3, dictionary.Count);
            Assert.AreEqual(1, dictionary[Keyword.Create("one")]);
            Assert.AreEqual(2, dictionary[Keyword.Create("two")]);
            Assert.AreEqual(3, dictionary[Keyword.Create("three")]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldParseSymbolIgnoringComment()
        {
            Parser parser = new Parser("foo ; this is a symbol");

            object form = parser.ParseForm();

            Assert.IsNotNull(form);
            Assert.IsInstanceOfType(form, typeof(Symbol));

            Symbol symbol = (Symbol)form;

            Assert.AreEqual("foo", symbol.Name);
            Assert.IsNull(symbol.Namespace);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldParseSymbolIgnoringPrecedingComment()
        {
            Parser parser = new Parser("; this is a symbol\r\nfoo");

            object form = parser.ParseForm();

            Assert.IsNotNull(form);
            Assert.IsInstanceOfType(form, typeof(Symbol));

            Symbol symbol = (Symbol)form;

            Assert.AreEqual("foo", symbol.Name);
            Assert.IsNull(symbol.Namespace);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldParseQuotedSymbol()
        {
            Parser parser = new Parser("'foo");

            object form = parser.ParseForm();

            Assert.IsNotNull(form);
            Assert.IsInstanceOfType(form, typeof(IList));

            IList list = (IList)form;

            Assert.AreEqual(2, list.Count);

            Assert.IsInstanceOfType(list[0], typeof(Symbol));
            Assert.IsInstanceOfType(list[1], typeof(Symbol));

            Assert.AreEqual("quote", ((Symbol)list[0]).Name);
            Assert.AreEqual("foo", ((Symbol)list[1]).Name);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldParseMetaForm()
        {
            Parser parser = new Parser("^foo");

            object form = parser.ParseForm();

            Assert.IsNotNull(form);
            Assert.IsInstanceOfType(form, typeof(IList));

            IList list = (IList)form;

            Assert.AreEqual(2, list.Count);

            Assert.IsInstanceOfType(list[0], typeof(Symbol));
            Assert.IsInstanceOfType(list[1], typeof(Symbol));

            Assert.AreEqual("meta", ((Symbol)list[0]).Name);
            Assert.AreEqual("foo", ((Symbol)list[1]).Name);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldParseDerefForm()
        {
            Parser parser = new Parser("@foo");

            object form = parser.ParseForm();

            Assert.IsNotNull(form);
            Assert.IsInstanceOfType(form, typeof(IList));

            IList list = (IList)form;

            Assert.AreEqual(2, list.Count);

            Assert.IsInstanceOfType(list[0], typeof(Symbol));
            Assert.IsInstanceOfType(list[1], typeof(Symbol));

            Assert.AreEqual("deref", ((Symbol)list[0]).Name);
            Assert.AreEqual("foo", ((Symbol)list[1]).Name);

            Assert.IsNull(parser.ParseForm());
        }
    }
}
