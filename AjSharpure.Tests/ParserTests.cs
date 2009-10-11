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
        public void ParseBooleans()
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
        public void ParseSymbol()
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
        public void ParseEmptyString()
        {
            Parser parser = new Parser(string.Empty);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ParseString()
        {
            Parser parser = new Parser("\"foo\"");

            object form = parser.ParseForm();

            Assert.IsNotNull(form);
            Assert.IsInstanceOfType(form, typeof(string));

            Assert.AreEqual("foo", form);
        }

        [TestMethod]
        public void ParseInteger()
        {
            Parser parser = new Parser("123");

            object form = parser.ParseForm();

            Assert.IsNotNull(form);
            Assert.IsInstanceOfType(form, typeof(int));

            Assert.AreEqual(123, (int) form);
        }

        [TestMethod]
        public void ParseList()
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
        public void ParseArray()
        {
            Parser parser = new Parser("[1 2 3]");

            object obj = parser.ParseForm();

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(IPersistentVector));

            IPersistentVector vector = (IPersistentVector)obj;

            Assert.AreEqual(3, vector.Count);
            Assert.AreEqual(1, vector[0]);
            Assert.AreEqual(2, vector[1]);
            Assert.AreEqual(3, vector[2]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ParseMap()
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
        public void ParseMapAsAssociative()
        {
            Parser parser = new Parser("{:one 1 :two 2 :three 3}");

            object obj = parser.ParseForm();

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(IAssociative));

            IAssociative associative = (IAssociative)obj;

            Assert.AreEqual(3, associative.Count);
            Assert.AreEqual(1, associative.ValueAt(Keyword.Create("one")));
            Assert.AreEqual(2, associative.ValueAt(Keyword.Create("two")));
            Assert.AreEqual(3, associative.ValueAt(Keyword.Create("three")));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ParseSymbolIgnoringComment()
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
        public void ParseSymbolIgnoringPrecedingComment()
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
        public void ParseQuotedSymbol()
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
        public void ParseBackQuotedSymbol()
        {
            Parser parser = new Parser("`foo");

            object form = parser.ParseForm();

            Assert.IsNotNull(form);
            Assert.IsInstanceOfType(form, typeof(IList));

            IList list = (IList)form;

            Assert.AreEqual(2, list.Count);

            Assert.IsInstanceOfType(list[0], typeof(Symbol));
            Assert.IsInstanceOfType(list[1], typeof(Symbol));

            Assert.AreEqual("backquote", ((Symbol)list[0]).Name);
            Assert.AreEqual("foo", ((Symbol)list[1]).Name);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ParseMetaForm()
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
        public void ParseDerefForm()
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

        [TestMethod]
        public void ParseVarForm()
        {
            Parser parser = new Parser("#'foo");

            object form = parser.ParseForm();

            Assert.IsNotNull(form);
            Assert.IsInstanceOfType(form, typeof(IList));

            IList list = (IList)form;

            Assert.AreEqual(2, list.Count);

            Assert.IsInstanceOfType(list[0], typeof(Symbol));
            Assert.IsInstanceOfType(list[1], typeof(Symbol));

            Assert.AreEqual("var", ((Symbol)list[0]).Name);
            Assert.AreEqual("foo", ((Symbol)list[1]).Name);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ParseSymbolWithMetadata()
        {
            Parser parser = new Parser("#^{:one 1 :two 2} foo");

            object form = parser.ParseForm();

            Assert.IsNotNull(form);
            Assert.IsInstanceOfType(form, typeof(Symbol));

            Symbol symbol = (Symbol)form;

            Assert.AreEqual("foo", symbol.Name);
            Assert.AreEqual("foo", symbol.FullName);
            Assert.IsNotNull(symbol.Metadata);
            Assert.IsInstanceOfType(symbol.Metadata, typeof(IDictionary));

            IDictionary dict = (IDictionary)symbol.Metadata;

            Assert.IsTrue(dict.Contains(Keyword.Create("one")));
            Assert.IsTrue(dict.Contains(Keyword.Create("two")));

            Assert.AreEqual(1, dict[Keyword.Create("one")]);
            Assert.AreEqual(2, dict[Keyword.Create("two")]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ParseListWithMetadata()
        {
            Parser parser = new Parser("#^{:one 1 :two 2} (1 2)");

            object form = parser.ParseForm();

            Assert.IsNotNull(form);
            Assert.IsInstanceOfType(form, typeof(IList));
            Assert.IsInstanceOfType(form, typeof(IObject));

            IList list = (IList)form;

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);

            IObject iobj = (IObject)form;

            Assert.IsNotNull(iobj.Metadata);

            IDictionary dict = (IDictionary)iobj.Metadata;

            Assert.IsTrue(dict.Contains(Keyword.Create("one")));
            Assert.IsTrue(dict.Contains(Keyword.Create("two")));

            Assert.AreEqual(1, dict[Keyword.Create("one")]);
            Assert.AreEqual(2, dict[Keyword.Create("two")]);

            Assert.IsNull(parser.ParseForm());
        }
    }
}
