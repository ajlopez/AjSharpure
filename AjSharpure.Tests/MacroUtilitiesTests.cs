namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections;
    using System.Linq;

    using AjSharpure;
    using AjSharpure.Compiler;
    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MacroUtilitiesTests
    {
        [TestMethod]
        public void ShouldExpandConstants()
        {
            Assert.AreEqual(1, MacroUtilities.Expand(1, null, null));
            Assert.AreEqual("foo", MacroUtilities.Expand("foo", null, null));
        }
        
        [TestMethod]
        public void ShouldExpandSymbol()
        {
            Symbol symbol = Symbol.Create("foo");
            Assert.AreEqual(symbol, MacroUtilities.Expand(symbol, null, null));
        }

        [TestMethod]
        public void ShouldExpandSimpleArray()
        {
            Parser parser = new Parser("[1 2 3]");
            object array = parser.ParseForm();
            object result = MacroUtilities.Expand(array, null, null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IPersistentVector));

            IPersistentVector resultVector = (IPersistentVector)result;

            Assert.AreEqual(3, resultVector.Count);
            Assert.AreEqual(1, resultVector[0]);
            Assert.AreEqual(2, resultVector[1]);
            Assert.AreEqual(3, resultVector[2]);
        }

        [TestMethod]
        public void ShouldExpandSimpleList()
        {
            Parser parser = new Parser("(1 2 3)");
            object array = parser.ParseForm();
            object result = MacroUtilities.Expand(array, null, null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList resultList = (IList)result;

            Assert.AreEqual(3, resultList.Count);
            Assert.AreEqual(1, resultList[0]);
            Assert.AreEqual(2, resultList[1]);
            Assert.AreEqual(3, resultList[2]);
        }

        [TestMethod]
        public void ShouldExpandBackquotedSymbol()
        {
            Parser parser = new Parser("(backquote x)");
            object list = parser.ParseForm();
            Machine machine = new Machine();

            machine.Environment.SetValue("x", "y");

            object result = MacroUtilities.Expand(list, machine, machine.Environment);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("y", result);
        }

        [TestMethod]
        public void ShouldExpandBackquotedSymbolInList()
        {
            Parser parser = new Parser("(1 (backquote x) 3)");
            object list = parser.ParseForm();
            Machine machine = new Machine();

            machine.Environment.SetValue("x", 2);

            object result = MacroUtilities.Expand(list, machine, machine.Environment);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList resultList = (IList)result;

            Assert.AreEqual(3, resultList.Count);
            Assert.AreEqual(1, resultList[0]);
            Assert.AreEqual(2, resultList[1]);
            Assert.AreEqual(3, resultList[2]);
        }

        [TestMethod]
        public void ShouldExpandBacklistedSymbolInList()
        {
            Parser parser = new Parser("(def x (list 2 3)) (1 (backlist x) 4)");

            Machine machine = new Machine();
            machine.Evaluate(parser.ParseForm());

            object list = parser.ParseForm();
            object result = MacroUtilities.Expand(list, machine, machine.Environment);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList resultList = (IList)result;

            Assert.AreEqual(4, resultList.Count);
            Assert.AreEqual(1, resultList[0]);
            Assert.AreEqual(2, resultList[1]);
            Assert.AreEqual(3, resultList[2]);
            Assert.AreEqual(4, resultList[3]);
        }
    }
}
