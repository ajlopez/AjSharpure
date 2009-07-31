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
        public void ShouldBeCreatedWithEnvironmentAndCurrentNamespace()
        {
            Machine machine = new Machine();

            Assert.IsNotNull(machine.Environment);
            Assert.AreEqual(Machine.AjSharpureCoreKey, machine.Environment.GetValue(Machine.CurrentNamespaceKey));
        }

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
        public void ShouldDefineAVariable()
        {
            Parser parser = new Parser("(def x 1)");
            Machine machine = new Machine();

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(1, result);

            string fullName = Utilities.GetFullName((string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x");

            object value = machine.GetVariableValue(Variable.Create(fullName));

            Assert.IsNotNull(value);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void ShouldRedefineAVariable()
        {
            Parser parser = new Parser("(def x 1)");
            Machine machine = new Machine();

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(1, result);

            string fullName = Utilities.GetFullName((string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x");

            object value = machine.GetVariableValue(Variable.Create(fullName));

            Assert.IsNotNull(value);
            Assert.AreEqual(1, value);

            parser = new Parser("(def x 2)");

            result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(2, result);
            Assert.AreEqual(2, machine.GetVariableValue(Variable.Create(fullName)));
        }

        [TestMethod]
        public void ShouldEvaluateSimpleDef()
        {
            Parser parser = new Parser("(def x 1)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());

            string fullName = Utilities.GetFullName((string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x");

            object value = machine.GetVariableValue(fullName);

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

            string fullName = Utilities.GetFullName((string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x");

            object value = machine.GetVariableValue(fullName);

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
        public void ShouldEvaluateDotInvocationUsingDirectNameAndArguments()
        {
            Parser parser = new Parser("(. AjSharpure.Utilities IsEvaluable 1)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(bool));
            Assert.IsFalse((bool)value);
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
        public void ShouldEvaluateDotInvocationUsingSymbolAndDirectNameAndArguments()
        {
            Parser parser = new Parser("(. AjSharpure.Utilities IsEvaluable one)");
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
        public void ShouldEvaluateDotInvocationWithTwoParametersAndDirectNameAndArguments()
        {
            Parser parser = new Parser("(. AjSharpure.Utilities CombineHash 1 2)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreNotEqual(0, (int)value);
        }

        [TestMethod]
        public void ShouldEvaluateToSequenceUsingUtilitiesToSequence()
        {
            Parser parser = new Parser("(. AjSharpure.Utilities ToSequence [1 2])");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(ISequence));

            ISequence sequence = (ISequence)value;

            Assert.AreEqual(2, sequence.Count);
            Assert.AreEqual(1, sequence.First());
            Assert.AreEqual(2, sequence.Next().First());
            Assert.IsNull(sequence.Next().Next());
        }

        [TestMethod]
        public void ShouldEvaluateDotInvocationOnInstance()
        {
            Parser parser = new Parser("(def x 1) (. x ToString)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(string));
            Assert.AreEqual("1", value);
        }

        [TestMethod]
        public void ShouldEvaluateDotInvocationOnInstanceUsingList()
        {
            Parser parser = new Parser("(def x 123) (. (. x ToString) (Substring 1)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(string));
            Assert.AreEqual("23", value);
        }

        [TestMethod]
        public void ShouldEvaluateDefinedFunction()
        {
            Parser parser = new Parser("(def simple-list (fn* simple-list [x y] (list x y))) (simple-list 1 2)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());
            object result = machine.Evaluate(parser.ParseForm());


            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));
            Assert.AreEqual(2, ((IList) result).Count);
        }

        [TestMethod]
        public void ShouldEvaluateDefinedSpecialForm()
        {
            Parser parser = new Parser("(def myquote (sf* myquote [x] x)) (myquote x)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());
            object result = machine.Evaluate(parser.ParseForm());


            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Symbol));

            Symbol symbol = (Symbol)result;

            Assert.IsNull(symbol.Namespace);
            Assert.AreEqual("x", symbol.Name);
        }

        [TestMethod]
        public void ShouldDefineAndEvaluateSeq()
        {
            Parser parser = new Parser("(def seq (fn* [coll] (. AjSharpure.Utilities ToSequence coll))) (seq [1 2])");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(DefinedFunction));

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ISequence));

            ISequence sequence = (ISequence)result;

            Assert.AreEqual(2, sequence.Count);
            Assert.AreEqual(1, sequence.First());
            Assert.AreEqual(2, sequence.Next().First());
            Assert.IsNull(sequence.Next().Next());

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldDefineAndEvaluateInstancePredicate()
        {
            Parser parser = new Parser("(def instance? (fn* [type obj] (. type IsInstanceOfType obj))) (instance? System.String \"foo\")");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(DefinedFunction));

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsTrue((bool)result);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldDefineAndEvaluateSeqPredicate()
        {
            Parser parser = new Parser("(def instance? (fn* [type obj] (. type IsInstanceOfType obj))) (def seq? (fn* [obj] (instance? AjSharpure.Language.ISequence obj))) (seq? (. AjSharpure.Utilities ToSequence \"foo\")");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(DefinedFunction));
            
            value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(DefinedFunction));

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsTrue((bool)result);

            Assert.IsNull(parser.ParseForm());
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

        [TestMethod]
        public void ShouldEvaluateSimpleLet()
        {
            Parser parser = new Parser("(let [x 1] x)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void ShouldEvaluateSimpleLetWithTwoBindings()
        {
            Parser parser = new Parser("(let [x 1 y 2] x)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void ShouldEvaluateSimpleLetWithRebindings()
        {
            Parser parser = new Parser("(let [x 1 x 2] x)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(2, value);
        }

        [TestMethod]
        public void ShouldEvaluateTrueFalseAndNil()
        {
            Parser parser = new Parser("true false nil");
            Machine machine = new Machine();

            object value;

            value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(bool));
            Assert.IsTrue((bool)value);

            value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(bool));
            Assert.IsFalse((bool)value);

            value = machine.Evaluate(parser.ParseForm());

            Assert.IsNull(value);
        }

        [TestMethod]
        public void ShouldEvaluateSimpleIf()
        {
            Parser parser = new Parser("(if true 1)");
            Machine machine = new Machine();

            object value;

            value = machine.Evaluate(parser.ParseForm());
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void ShouldEvaluateSimpleIfElse()
        {
            Parser parser = new Parser("(if false 1 2)");
            Machine machine = new Machine();

            object value;

            value = machine.Evaluate(parser.ParseForm());
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(2, value);
        }

        [TestMethod]
        public void ShouldEvaluateIf()
        {
            Parser parser = new Parser("(def x true) (if x (list 1 2 3))");
            Machine machine = new Machine();

            object value;

            machine.Evaluate(parser.ParseForm());
            value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(IList));

            IList list = (IList)value;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        public void ShouldEvaluateSimpleLoopWithRecur()
        {
            Parser parser = new Parser("(loop [x true y (list 1 2)] (if x (recur false (list 1 2 3)) y)");
            Machine machine = new Machine();

            object value;

            value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(IList));

            IList list = (IList)value;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        public void ShouldEvaluateLoopWithRecur()
        {
            Parser parser = new Parser("(loop [x true y (list 1 2) z1 1 z2 2 z3 3] (if x (recur false (list z1 z2 z3) 2 3 4) y)");
            Machine machine = new Machine();

            object value;

            value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(IList));

            IList list = (IList)value;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfRecurArityDoesNotMatchLoopArity()
        {
            Parser parser = new Parser("(loop [x true y (list 1 2)] (if x (recur (list 1 2 3)) y)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldAddNumbers()
        {
            Parser parser = new Parser("(+) (+ 1) (+ 1 2) (+ 1 2 3)");
            Machine machine = new Machine();

            Assert.AreEqual(0, machine.Evaluate(parser.ParseForm()));
            Assert.AreEqual(1, machine.Evaluate(parser.ParseForm()));
            Assert.AreEqual(3, machine.Evaluate(parser.ParseForm()));
            Assert.AreEqual(6, machine.Evaluate(parser.ParseForm()));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldSubtractNumbers()
        {
            Parser parser = new Parser("(- 1) (- 1 2) (- 1 2 3)");
            Machine machine = new Machine();

            Assert.AreEqual(-1, machine.Evaluate(parser.ParseForm()));
            Assert.AreEqual(-1, machine.Evaluate(parser.ParseForm()));
            Assert.AreEqual(-4, machine.Evaluate(parser.ParseForm()));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldMultiplyNumbers()
        {
            Parser parser = new Parser("(*) (* 1) (* 1 2) (* 1 2 3)");
            Machine machine = new Machine();

            Assert.AreEqual(1, machine.Evaluate(parser.ParseForm()));
            Assert.AreEqual(1, machine.Evaluate(parser.ParseForm()));
            Assert.AreEqual(2, machine.Evaluate(parser.ParseForm()));
            Assert.AreEqual(6, machine.Evaluate(parser.ParseForm()));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldDivideNumbers()
        {
            Parser parser = new Parser("(/ 2) (/ 1 2) (/ 1 2 3)");
            Machine machine = new Machine();

            Assert.AreEqual(1.0/2, machine.Evaluate(parser.ParseForm()));
            Assert.AreEqual(1.0/2, machine.Evaluate(parser.ParseForm()));
            Assert.AreEqual(1.0/2/3, machine.Evaluate(parser.ParseForm()));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldEvaluateEqualsWithNumbers()
        {
            Parser parser = new Parser("(= 1) (= 1 1) (= 1 1 1) (= 1 2) (= 1 2 1)");
            Machine machine = new Machine();

            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsFalse((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsFalse((bool)machine.Evaluate(parser.ParseForm()));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldEvaluateEqualsWithLists()
        {
            Parser parser = new Parser("(= (list 1 2 3) (list 1 2 3)) (= (list 1 2 3) (list 1 2 3 4))");
            Machine machine = new Machine();

            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsFalse((bool)machine.Evaluate(parser.ParseForm()));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldEvaluateLessWithNumbers()
        {
            Parser parser = new Parser("(< 1) (< 1 2) (< 1 2 3) (< 1 1) (< 1 0 1)");
            Machine machine = new Machine();

            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsFalse((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsFalse((bool)machine.Evaluate(parser.ParseForm()));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldEvaluateLessEqualWithNumbers()
        {
            Parser parser = new Parser("(<= 1) (<= 1 2) (<= 1 2 3) (<= 1 1) (<= 1 0 1)");
            Machine machine = new Machine();

            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsFalse((bool)machine.Evaluate(parser.ParseForm()));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldEvaluateGreaterWithNumbers()
        {
            Parser parser = new Parser("(> 1) (> 2 1 ) (> 2 1 0) (> 1 1) (> 1 0 1)");
            Machine machine = new Machine();

            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsFalse((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsFalse((bool)machine.Evaluate(parser.ParseForm()));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldEvaluateGreaterEqualWithNumbers()
        {
            Parser parser = new Parser("(>= 1) (>= 2 1 ) (>= 2 1 0) (>= 1 1) (>= 1 0 1)");
            Machine machine = new Machine();

            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsFalse((bool)machine.Evaluate(parser.ParseForm()));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfSubtractWithoutParameters()
        {
            Parser parser = new Parser("(-)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfDivideWithoutParameters()
        {
            Parser parser = new Parser("(/)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfEqualsWithoutParameters()
        {
            Parser parser = new Parser("(=)");
            Machine machine = new Machine();
 
            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfLessWithoutParameters()
        {
            Parser parser = new Parser("(<)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfGreaterWithoutParameters()
        {
            Parser parser = new Parser("(>)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfLessEqualWithoutParameters()
        {
            Parser parser = new Parser("(<=)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfGreaterEqualWithoutParameters()
        {
            Parser parser = new Parser("(>=)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldEvaluateRecursiveAnonymousFactorialFunction()
        {
            Parser parser = new Parser("((fn* fact [x] (if (<= x 1) 1 (* x (fact (- x 1))))) 3)");
            Machine machine = new Machine();

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(6, result);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldEvaluateAnonymousFactorialFunctionWithTailRecursion()
        {
            Parser parser = new Parser("((fn* [x y] (if (<= x 1) y (recur (- x 1) (* x y)))) 3 1)");
            Machine machine = new Machine();

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(6, result);

            Assert.IsNull(parser.ParseForm());
        }
        
        [TestMethod]
        public void ShouldEvaluateNewSystemIOFileInfo()
        {
            Parser parser = new Parser("(new System.IO.FileInfo \"anyfile.txt\")");
            Machine machine = new Machine();

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(System.IO.FileInfo));
            Assert.AreEqual("anyfile.txt", ((System.IO.FileInfo)result).Name);
        }

        [TestMethod]
        public void ShouldEvaluateVarExpressionToVar()
        {
            Parser parser = new Parser("(var x)");
            Machine machine = new Machine();
            machine.SetVariableValue(Variable.Create((string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x"), 1);

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Variable));

            Variable variable = (Variable)result;

            Assert.AreEqual(machine.Environment.GetValue(Machine.CurrentNamespaceKey), variable.Namespace);
            Assert.AreEqual("x", variable.Name);
        }

        [TestMethod]
        public void ShouldEvaluateVarExpressionWithQualifiedSymbolToVar()
        {
            Parser parser = new Parser("(var foo/x)");
            Machine machine = new Machine();
            machine.SetVariableValue(Variable.Create("foo", "x"), 1);

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Variable));

            Variable variable = (Variable)result;

            Assert.AreEqual("foo", variable.Namespace);
            Assert.AreEqual("x", variable.Name);
        }

        [TestMethod]
        public void ShouldEvaluateVarMacroExpressionToVar()
        {
            Parser parser = new Parser("#'x");
            Machine machine = new Machine();
            machine.SetVariableValue(Variable.Create((string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x"), 1);

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Variable));

            Variable variable = (Variable)result;

            Assert.AreEqual(machine.Environment.GetValue(Machine.CurrentNamespaceKey), variable.Namespace);
            Assert.AreEqual("x", variable.Name);
        }

        [TestMethod]
        public void ShouldSetAndGetVariableValue()
        {
            Machine machine = new Machine();
            Variable variable = Variable.Create("foo", "bar");

            machine.SetVariableValue(variable, 3);

            object result = machine.GetVariableValue(variable);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(3, (int)result);
        }

        [TestMethod]
        public void ShouldSetAndGetVariableValueUsingAnotherVariable()
        {
            Machine machine = new Machine();
            Variable variable = Variable.Create("foo", "bar");
            Variable variable2 = Variable.Create("foo/bar");

            machine.SetVariableValue(variable, 3);

            object result = machine.GetVariableValue(variable2);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(3, (int)result);
        }

        [TestMethod]
        public void ShouldSetAndGetVariableValueUsingFullName()
        {
            Machine machine = new Machine();
            Variable variable = Variable.Create("foo", "bar");

            machine.SetVariableValue(variable, 3);

            object result = machine.GetVariableValue("foo/bar");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(3, (int)result);
        }

        [TestMethod]
        public void ShouldGetNullValueIfVariableIsUndefined()
        {
            Machine machine = new Machine();
            Variable variable = Variable.Create("foo", "bar");

            object result = machine.GetVariableValue(variable);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldGetNullVariableIfVariableIsUndefined()
        {
            Machine machine = new Machine();
            Variable variable = Variable.Create("foo", "bar");

            Variable result = machine.GetVariable(variable);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldGetTheSameDefinedVariable()
        {
            Machine machine = new Machine();
            Variable variable = Variable.Create("foo", "bar");

            machine.SetVariableValue(variable, 3);

            Variable result = machine.GetVariable(variable);

            Assert.IsNotNull(result);

            Assert.IsTrue(result == variable);
        }

        [TestMethod]
        public void ShouldGetTheSameDefinedVariableUsingFullName()
        {
            Machine machine = new Machine();
            Variable variable = Variable.Create("foo", "bar");

            machine.SetVariableValue(variable, 3);

            Variable result = machine.GetVariable("foo/bar");

            Assert.IsNotNull(result);

            Assert.IsTrue(result == variable);
        }

        [TestMethod]
        public void ShouldDefineVariable()
        {
            Machine machine = new Machine();
            Parser parser = new Parser("(def x 1)");

            machine.Evaluate(parser.ParseForm());

            Variable variable = machine.GetVariable(Utilities.ToVariable(machine, machine.Environment, Symbol.Create("x")));

            Assert.IsNotNull(variable);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldDefineVariableWithMetadata()
        {
            Machine machine = new Machine();
            Parser parser = new Parser("(def #^{:one 1 :two 2} x 1)");

            machine.Evaluate(parser.ParseForm());

            Variable variable = machine.GetVariable(Utilities.ToVariable(machine, machine.Environment, Symbol.Create("x")));

            Assert.IsNotNull(variable);
            Assert.IsNotNull(variable.Metadata);
            Assert.IsInstanceOfType(variable.Metadata, typeof(IDictionary));

            IDictionary dictionary = (IDictionary)variable.Metadata;

            Assert.AreEqual(2, dictionary.Count);
            Assert.IsTrue(dictionary.Contains(Keyword.Create("one")));
            Assert.IsTrue(dictionary.Contains(Keyword.Create("two")));
            Assert.AreEqual(1, dictionary[Keyword.Create("one")]);
            Assert.AreEqual(2, dictionary[Keyword.Create("two")]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldDefineVariableWithMetadataTag()
        {
            Machine machine = new Machine();
            Parser parser = new Parser("(def #^AjSharpure.Language.IObject x 1)");

            machine.Evaluate(parser.ParseForm());

            Variable variable = machine.GetVariable(Utilities.ToVariable(machine, machine.Environment, Symbol.Create("x")));

            Assert.IsNotNull(variable);
            Assert.IsNotNull(variable.Metadata);
            Assert.IsInstanceOfType(variable.Metadata, typeof(IDictionary));

            IDictionary dictionary = (IDictionary)variable.Metadata;

            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.Contains(Keyword.Create("tag")));
            Assert.IsInstanceOfType(dictionary[Keyword.Create("tag")], typeof(Type));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void ShouldDefineMacroUsingMetadata()
        {
            Machine machine = new Machine();
            Parser parser = new Parser("(def #^{:macro true} let (fn* let [& decl] (cons 'let* decl)))");

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedMacro));
        }

        [TestMethod]
        public void CreateNamespace()
        {
            Machine machine = new Machine();

            Namespace ns = machine.CreateNamespace("foo");

            Assert.IsNotNull(ns);
            Assert.AreEqual("foo", ns.Name);
            Assert.IsTrue(machine == ns.Machine);
        }

        [TestMethod]
        public void GetNamespace()
        {
            Machine machine = new Machine();

            machine.CreateNamespace("foo");

            Namespace ns = machine.GetNamespace("foo");

            Assert.IsNotNull(ns);
            Assert.AreEqual("foo", ns.Name);
            Assert.IsTrue(machine == ns.Machine);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfNamespaceAlreadyExists()
        {
            Machine machine = new Machine();

            machine.CreateNamespace("foo");
            machine.CreateNamespace("foo");
        }
    }
}
