namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure;
    using AjSharpure.Compiler;
    using AjSharpure.Expressions;
    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MachineTests
    {
        [TestMethod]
        public void ShouldEvaluateSymbolExpression()
        {
            Parser parser = new Parser("foo");
            Machine machine = new Machine();
            machine.Environment.SetValue("foo", "bar");

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.AreEqual("bar", value);
        }

        [TestMethod]
        public void ShouldEvaluateQuotedSymbol()
        {
            Parser parser = new Parser("'foo");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(Symbol));
            Assert.AreEqual("foo", ((Symbol)value).Name);
        }

        [TestMethod]
        public void ShouldEvaluateQuotedList()
        {
            Parser parser = new Parser("'(1 2 3)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(IList));

            IList list = (IList)value;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        public void ShouldEvaluateQuotedArray()
        {
            Parser parser = new Parser("'[1 2 3]");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsInstanceOfType(value, typeof(object[]));

            object[] array = (object[])value;

            Assert.AreEqual(3, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldEvaluateArray()
        {
            Parser parser = new Parser("[1 2 3]");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsInstanceOfType(value, typeof(object[]));

            object[] array = (object[])value;

            Assert.AreEqual(3, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldEvaluateArrayWithSymbols()
        {
            Parser parser = new Parser("[one two three]");
            Machine machine = new Machine();

            machine.Environment.SetValue("one", 1);
            machine.Environment.SetValue("two", 2);
            machine.Environment.SetValue("three", 3);

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsInstanceOfType(value, typeof(object[]));

            object[] array = (object[])value;

            Assert.AreEqual(3, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldEvaluateArrayWithKeywords()
        {
            Parser parser = new Parser("[:one :two :three]");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsInstanceOfType(value, typeof(object[]));

            object[] array = (object[])value;

            Assert.AreEqual(3, array.Length);
            Assert.AreEqual(Keyword.Create("one"), array[0]);
            Assert.AreEqual(Keyword.Create("two"), array[1]);
            Assert.AreEqual(Keyword.Create("three"), array[2]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldEvaluateMap()
        {
            Parser parser = new Parser("{:one 1 :two 2 :three 3}");

            Machine machine = new Machine();

            object obj = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(IDictionary));

            IDictionary dictionary = (IDictionary)obj;

            Assert.AreEqual(3, dictionary.Count);
            Assert.AreEqual(1, dictionary[Keyword.Create("one")]);
            Assert.AreEqual(2, dictionary[Keyword.Create("two")]);
            Assert.AreEqual(3, dictionary[Keyword.Create("three")]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldEvaluateMapWithSymbols()
        {
            Parser parser = new Parser("{:one one :two two :three three}");

            Machine machine = new Machine();

            machine.Environment.SetValue("one", 1);
            machine.Environment.SetValue("two", 2);
            machine.Environment.SetValue("three", 3);

            object obj = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(IDictionary));

            IDictionary dictionary = (IDictionary)obj;

            Assert.AreEqual(3, dictionary.Count);
            Assert.AreEqual(1, dictionary[Keyword.Create("one")]);
            Assert.AreEqual(2, dictionary[Keyword.Create("two")]);
            Assert.AreEqual(3, dictionary[Keyword.Create("three")]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldEvaluateSetBangExpression()
        {
            Parser parser = new Parser("(set! foo \"bar\")");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(string));
            Assert.AreEqual("bar", value);

            object var = machine.Environment.GetValue("foo");

            Assert.IsNotNull(var);
            Assert.IsInstanceOfType(var, typeof(string));
            Assert.AreEqual("bar", var);
        }

        [TestMethod]
        public void ShouldEvaluateSetBangExpressionWithSymbol()
        {
            Parser parser = new Parser("(set! foo mybar)");
            Machine machine = new Machine();
            machine.Environment.SetValue("mybar", "bar");

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(string));
            Assert.AreEqual("bar", value);

            object var = machine.Environment.GetValue("foo");

            Assert.IsNotNull(var);
            Assert.IsInstanceOfType(var, typeof(string));
            Assert.AreEqual("bar", var);
        }

        [TestMethod]
        public void ShouldEvaluateSimpleListExpression()
        {
            Parser parser = new Parser("(list 1 2 3)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(IList));

            IList list = (IList)value;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        public void ShouldEvaluateSimpleDef()
        {
            Parser parser = new Parser("(def x 1)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());

            object value = machine.Environment.GetValue("x");

            Assert.IsNotNull(value);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void ShouldEvaluateSimpleDefWithSymbol()
        {
            Parser parser = new Parser("(def x one)");
            Machine machine = new Machine();

            machine.Environment.SetValue("one", 1);

            machine.Evaluate(parser.ParseForm());

            object value = machine.Environment.GetValue("x");

            Assert.IsNotNull(value);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void ShouldEvaluateDotInvocation()
        {
            Parser parser = new Parser("(. AjSharpure.Utilities (IsEvaluable 1))");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(bool));
            Assert.IsFalse((bool) value);
        }

        [TestMethod]
        public void ShouldEvaluateDotInvocationUsingSymbol()
        {
            Parser parser = new Parser("(. AjSharpure.Utilities (IsEvaluable one))");
            Machine machine = new Machine();
            machine.Environment.SetValue("one", 1);

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(bool));
            Assert.IsFalse((bool)value);
        }

        [TestMethod]
        public void ShouldEvaluateDotInvocationWithTwoParameters()
        {
            Parser parser = new Parser("(. AjSharpure.Utilities (CombineHash 1 2))");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreNotEqual(0, (int) value);
        }

        [TestMethod]
        public void ShouldEvaluateDefinedFunction()
        {
            Parser parser = new Parser("(def simple-list (fn simple-list [x y] (list x y))) (simple-list 1 2)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());
            object result = machine.Evaluate(parser.ParseForm());


            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));
            Assert.AreEqual(2, ((IList) result).Count);
        }

        [TestMethod]
        public void ShouldEvaluateSimpleDo()
        {
            Parser parser = new Parser("(do 1 2 3)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(3, value);
        }

        [TestMethod]
        public void ShouldEvaluateDoWithSymbols()
        {
            Parser parser = new Parser("(def x 1) (def y 2) (def z 3) (do x y z)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
            machine.Evaluate(parser.ParseForm());
            machine.Evaluate(parser.ParseForm());
            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(3, value);
        }

        [TestMethod]
        public void ShouldEvaluateDoWithExpression()
        {
            Parser parser = new Parser("(do 1 2 (list 1 2 3))");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(IList));

            IList list = (IList)value;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }
    }
}
