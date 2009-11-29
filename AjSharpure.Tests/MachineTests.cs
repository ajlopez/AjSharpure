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
        public void BeCreatedWithEnvironmentAndCurrentNamespace()
        {
            Machine machine = new Machine();

            Assert.IsNotNull(machine.Environment);
            Assert.AreEqual(Machine.AjSharpureCoreNamespaceName, machine.Environment.GetValue(Machine.CurrentNamespaceKey));
        }

        [TestMethod]
        public void EvaluateSymbolExpression()
        {
            Parser parser = new Parser("foo");
            Machine machine = new Machine();
            machine.Environment.SetValue("foo", "bar");

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.AreEqual("bar", value);
        }

        [TestMethod]
        public void EvaluateQuotedSymbol()
        {
            Parser parser = new Parser("'foo");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(Symbol));
            Assert.AreEqual("foo", ((Symbol)value).Name);
        }

        [TestMethod]
        public void EvaluateQuotedList()
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
        public void EvaluateQuotedArray()
        {
            Parser parser = new Parser("'[1 2 3]");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsInstanceOfType(value, typeof(IPersistentVector));

            IPersistentVector vector = (IPersistentVector)value;

            Assert.AreEqual(3, vector.Count);
            Assert.AreEqual(1, vector[0]);
            Assert.AreEqual(2, vector[1]);
            Assert.AreEqual(3, vector[2]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void EvaluateArray()
        {
            Parser parser = new Parser("[1 2 3]");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsInstanceOfType(value, typeof(IPersistentVector));

            IPersistentVector vector = (IPersistentVector)value;

            Assert.AreEqual(3, vector.Count);
            Assert.AreEqual(1, vector[0]);
            Assert.AreEqual(2, vector[1]);
            Assert.AreEqual(3, vector[2]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void EvaluateArrayWithSymbols()
        {
            Parser parser = new Parser("[one two three]");
            Machine machine = new Machine();

            machine.Environment.SetValue("one", 1);
            machine.Environment.SetValue("two", 2);
            machine.Environment.SetValue("three", 3);

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsInstanceOfType(value, typeof(IPersistentVector));

            IPersistentVector array = (IPersistentVector)value;

            Assert.AreEqual(3, array.Count);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void EvaluateArrayWithKeywords()
        {
            Parser parser = new Parser("[:one :two :three]");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsInstanceOfType(value, typeof(IPersistentVector));

            IPersistentVector vector = (IPersistentVector)value;

            Assert.AreEqual(3, vector.Count);
            Assert.AreEqual(Keyword.Create("one"), vector[0]);
            Assert.AreEqual(Keyword.Create("two"), vector[1]);
            Assert.AreEqual(Keyword.Create("three"), vector[2]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void EvaluateMap()
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
        public void EvaluateMapWithSymbols()
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
        public void EvaluateSetBangExpression()
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
        public void EvaluateSetBangExpressionWithSymbol()
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
        public void EvaluateSimpleListExpression()
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
        public void DefineAVariable()
        {
            Parser parser = new Parser("(def x 1)");
            Machine machine = new Machine();

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(1, result);

            object value = machine.GetVariableValue((string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x");

            Assert.IsNotNull(value);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void RedefineAVariable()
        {
            Parser parser = new Parser("(def x 1)");
            Machine machine = new Machine();

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(1, result);

            object value = machine.GetVariableValue((string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x");

            Assert.IsNotNull(value);
            Assert.AreEqual(1, value);

            parser = new Parser("(def x 2)");

            result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(2, result);
            Assert.AreEqual(2, machine.GetVariableValue((string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x"));
        }

        [TestMethod]
        public void EvaluateSimpleDef()
        {
            Parser parser = new Parser("(def x 1)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());

            object value = machine.GetVariableValue((string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x");

            Assert.IsNotNull(value);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void EvaluateSimpleDefWithSymbol()
        {
            Parser parser = new Parser("(def x one)");
            Machine machine = new Machine();

            machine.Environment.SetValue("one", 1);

            machine.Evaluate(parser.ParseForm());

            object value = machine.GetVariableValue((string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x");

            Assert.IsNotNull(value);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void EvaluateDotInvocation()
        {
            Parser parser = new Parser("(. AjSharpure.Utilities (IsEvaluable 1))");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(bool));
            Assert.IsFalse((bool) value);
        }

        [TestMethod]
        public void EvaluateDotInvocationUsingDirectNameAndArguments()
        {
            Parser parser = new Parser("(. AjSharpure.Utilities IsEvaluable 1)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(bool));
            Assert.IsFalse((bool)value);
        }

        [TestMethod]
        public void EvaluateDotInvocationUsingSymbol()
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
        public void EvaluateDotInvocationUsingSymbolAndDirectNameAndArguments()
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
        public void EvaluateDotInvocationWithTwoParameters()
        {
            Parser parser = new Parser("(. AjSharpure.Utilities (CombineHash 1 2))");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreNotEqual(0, (int) value);
        }

        [TestMethod]
        public void EvaluateDotInvocationWithTwoParametersAndDirectNameAndArguments()
        {
            Parser parser = new Parser("(. AjSharpure.Utilities CombineHash 1 2)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreNotEqual(0, (int)value);
        }

        [TestMethod]
        public void EvaluateToSequenceUsingUtilitiesToSequence()
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
        public void EvaluateDotInvocationOnInstance()
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
        public void EvaluateDotInvocationOnInstanceUsingList()
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
        public void EvaluateDefinedFunction()
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
        public void EvaluateDefinedSpecialForm()
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
        public void DefineAndEvaluateSeq()
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
        public void DefineAndEvaluateAFunctionUsingAmpersand()
        {
            Parser parser = new Parser("(def mylist (fn* [& coll] coll)) (mylist 1 2)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(DefinedFunction));

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList list = (IList)result;

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void DefineAndEvaluateInstancePredicate()
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
        public void DefineAndEvaluateSeqPredicate()
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
        public void EvaluateSimpleDo()
        {
            Parser parser = new Parser("(do 1 2 3)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(3, value);
        }

        [TestMethod]
        public void EvaluateDoWithSymbols()
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
        public void EvaluateDoWithExpression()
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
        public void EvaluateSimpleLet()
        {
            Parser parser = new Parser("(let [x 1] x)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void EvaluateSimpleLetWithTwoBindings()
        {
            Parser parser = new Parser("(let [x 1 y 2] x)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void EvaluateSimpleLetWithRebindings()
        {
            Parser parser = new Parser("(let [x 1 x 2] x)");
            Machine machine = new Machine();

            object value = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(2, value);
        }

        [TestMethod]
        public void EvaluateTrueFalseAndNil()
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
        public void EvaluateSimpleIf()
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
        public void EvaluateSimpleIfElse()
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
        public void EvaluateIf()
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
        public void EvaluateSimpleLoopWithRecur()
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
        public void EvaluateLoopWithRecur()
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
        public void RaiseIfRecurArityDoesNotMatchLoopArity()
        {
            Parser parser = new Parser("(loop [x true y (list 1 2)] (if x (recur (list 1 2 3)) y)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        public void AddNumbers()
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
        public void SubtractNumbers()
        {
            Parser parser = new Parser("(- 1) (- 1 2) (- 1 2 3)");
            Machine machine = new Machine();

            Assert.AreEqual(-1, machine.Evaluate(parser.ParseForm()));
            Assert.AreEqual(-1, machine.Evaluate(parser.ParseForm()));
            Assert.AreEqual(-4, machine.Evaluate(parser.ParseForm()));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void MultiplyNumbers()
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
        public void DivideNumbers()
        {
            Parser parser = new Parser("(/ 2) (/ 1 2) (/ 1 2 3)");
            Machine machine = new Machine();

            Assert.AreEqual(1.0/2, machine.Evaluate(parser.ParseForm()));
            Assert.AreEqual(1.0/2, machine.Evaluate(parser.ParseForm()));
            Assert.AreEqual(1.0/2/3, machine.Evaluate(parser.ParseForm()));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void EvaluateEqualsWithNumbers()
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
        public void EvaluateEqualsWithLists()
        {
            Parser parser = new Parser("(= (list 1 2 3) (list 1 2 3)) (= (list 1 2 3) (list 1 2 3 4))");
            Machine machine = new Machine();

            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsFalse((bool)machine.Evaluate(parser.ParseForm()));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void EvaluateLessWithNumbers()
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
        public void EvaluateLessEqualWithNumbers()
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
        public void EvaluateGreaterWithNumbers()
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
        public void EvaluateGreaterEqualWithNumbers()
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
        public void EvaluateEqualEqualWithNumbers()
        {
            Parser parser = new Parser("(== 1) (== 2 2 ) (== 2 2 2) (== 1 2) (== 1 1 2)");
            Machine machine = new Machine();

            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsTrue((bool)machine.Evaluate(parser.ParseForm()));

            Assert.IsFalse((bool)machine.Evaluate(parser.ParseForm()));
            Assert.IsFalse((bool)machine.Evaluate(parser.ParseForm()));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfSubtractWithoutParameters()
        {
            Parser parser = new Parser("(-)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfDivideWithoutParameters()
        {
            Parser parser = new Parser("(/)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfEqualsWithoutParameters()
        {
            Parser parser = new Parser("(=)");
            Machine machine = new Machine();
 
            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfLessWithoutParameters()
        {
            Parser parser = new Parser("(<)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfGreaterWithoutParameters()
        {
            Parser parser = new Parser("(>)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfLessEqualWithoutParameters()
        {
            Parser parser = new Parser("(<=)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfGreaterEqualWithoutParameters()
        {
            Parser parser = new Parser("(>=)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        public void EvaluateRecursiveAnonymousFactorialFunction()
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
        public void EvaluateAnonymousFactorialFunctionWithTailRecursion()
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
        public void EvaluateNewSystemIOFileInfo()
        {
            Parser parser = new Parser("(new System.IO.FileInfo \"anyfile.txt\")");
            Machine machine = new Machine();

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(System.IO.FileInfo));
            Assert.AreEqual("anyfile.txt", ((System.IO.FileInfo)result).Name);
        }

        [TestMethod]
        public void EvaluateVarExpressionToVar()
        {
            Parser parser = new Parser("(var x)");
            Machine machine = new Machine();
            machine.SetVariableValue(Variable.Intern(machine, (string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x"), 1);

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Variable));

            Variable variable = (Variable)result;

            Assert.AreEqual(machine.Environment.GetValue(Machine.CurrentNamespaceKey), variable.Namespace);
            Assert.AreEqual("x", variable.Name);
        }

        [TestMethod]
        public void EvaluateVarExpressionWithQualifiedSymbolToVar()
        {
            Parser parser = new Parser("(var foo/x)");
            Machine machine = new Machine();
            machine.CreateNamespace("foo");
            machine.SetVariableValue(Variable.Intern(machine, "foo", "x"), 1);

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Variable));

            Variable variable = (Variable)result;

            Assert.AreEqual("foo", variable.Namespace);
            Assert.AreEqual("x", variable.Name);
        }

        [TestMethod]
        public void EvaluateVarMacroExpressionToVar()
        {
            Parser parser = new Parser("#'x");
            Machine machine = new Machine();
            machine.SetVariableValue(Variable.Intern(machine, (string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x"), 1);

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Variable));

            Variable variable = (Variable)result;

            Assert.AreEqual(machine.Environment.GetValue(Machine.CurrentNamespaceKey), variable.Namespace);
            Assert.AreEqual("x", variable.Name);
        }

        [TestMethod]
        public void SetAndGetVariableValue()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");
            Variable variable = Variable.Intern(machine, "foo", "bar");

            machine.SetVariableValue(variable, 3);

            object result = machine.GetVariableValue(variable);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(3, (int)result);
        }

        [TestMethod]
        public void SetAndGetVariableValueUsingAnotherVariable()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");
            Variable variable = Variable.Intern(machine, "foo", "bar");
            Variable variable2 = Variable.Intern(machine, "foo/bar");

            machine.SetVariableValue(variable, 3);

            object result = machine.GetVariableValue(variable2);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(3, (int)result);
        }

        [TestMethod]
        public void SetAndGetVariableValueUsingFullName()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");
            Variable variable = Variable.Intern(machine, "foo", "bar");

            machine.SetVariableValue(variable, 3);

            object result = machine.GetVariableValue("foo", "bar");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(3, (int)result);
        }

        [TestMethod]
        public void GetNullValueIfVariableHasNoValue()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");
            Variable variable = Variable.Intern(machine, "foo", "bar");

            object result = machine.GetVariableValue(variable);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetNullVariableIfVariableIsUndefined()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Variable result = machine.GetVariable("foo", "bar");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetTheSameDefinedVariable()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");
            Variable variable = Variable.Intern(machine, "foo", "bar");

            machine.SetVariableValue(variable, 3);

            Variable result = machine.GetVariable(variable);

            Assert.IsNotNull(result);

            Assert.IsTrue(result == variable);
        }

        [TestMethod]
        public void GetTheSameDefinedVariableUsingFullName()
        {
            Machine machine = new Machine();
            machine.CreateNamespace("foo");
            Variable variable = Variable.Intern(machine, "foo", "bar");

            machine.SetVariableValue(variable, 3);

            Variable result = machine.GetVariable("foo","bar");

            Assert.IsNotNull(result);

            Assert.IsTrue(result == variable);
        }

        [TestMethod]
        public void DefineVariable()
        {
            Machine machine = new Machine();
            Parser parser = new Parser("(def x 1)");

            machine.Evaluate(parser.ParseForm());

            Variable variable = machine.GetVariable(Utilities.ToVariable(machine, machine.Environment, Symbol.Create("x")));

            Assert.IsNotNull(variable);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void DefineVariableWithMetadata()
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
        public void DefineVariableWithMetadataTag()
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
        public void DefineMacroUsingMetadata()
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

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfNamespaceDoesNotExist()
        {
            Machine machine = new Machine();

            machine.GetNamespace("foo");
        }

        [TestMethod]
        public void EvaluateDictionaryObjectAsIAssociative()
        {
            IDictionary dict = new Hashtable();
            dict["one"] = 1;
            dict["two"] = 2;

            DictionaryObject dictionary = new DictionaryObject(dict);

            Machine machine = new Machine();

            object result = machine.Evaluate(dictionary);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IAssociative));

            IAssociative associative = (IAssociative)result;

            Assert.AreEqual(2, associative.Count);
            Assert.IsTrue(associative.ContainsKey("one"));
            Assert.IsTrue(associative.ContainsKey("two"));
            Assert.AreEqual(1, associative.ValueAt("one"));
            Assert.AreEqual(2, associative.ValueAt("two"));
        }

        [TestMethod]
        public void EvaluateDictionaryObjectWithVariables()
        {
            Machine machine = new Machine();
            IDictionary dict = new Hashtable();
            dict["one"] = Utilities.ToVariable(machine, machine.Environment, Symbol.Create("symone"));
            dict["two"] = Utilities.ToVariable(machine, machine.Environment, Symbol.Create("symtwo"));

            DictionaryObject dictionary = new DictionaryObject(dict);

            machine.SetVariableValue(Utilities.ToVariable(machine, machine.Environment, Symbol.Create("symone")), 1);
            machine.SetVariableValue(Utilities.ToVariable(machine, machine.Environment, Symbol.Create("symtwo")), 2);

            object result = machine.Evaluate(dictionary);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IAssociative));

            IAssociative associative = (IAssociative)result;

            Assert.AreEqual(2, associative.Count);
            Assert.IsTrue(associative.ContainsKey("one"));
            Assert.IsTrue(associative.ContainsKey("two"));
            Assert.AreEqual(1, associative.ValueAt("one"));
            Assert.AreEqual(2, associative.ValueAt("two"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void EvaluateThrow()
        {
            Machine machine = new Machine();
            Parser parser = new Parser("(throw (new System.InvalidCastException))");

            machine.Evaluate(parser.ParseForm());
        }

        [TestMethod]
        public void EvaluateClosure()
        {
            Machine machine = new Machine();
            Parser parser = new Parser("(closure +)");

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IFn));

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void EvaluateAndApplyClosure()
        {
            Machine machine = new Machine();
            Parser parser = new Parser("((closure +) 1 2)");

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result);

            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void EvaluateBackquoteWithSimpleObject()
        {
            Machine machine = new Machine();
            Parser parser = new Parser("`1");

            Assert.AreEqual(1, machine.Evaluate(parser.ParseForm()));
            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void EvaluateBackquoteWithNonqualifiedSymbol()
        {
            Machine machine = new Machine();
            Parser parser = new Parser("`x");

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Symbol));

            Symbol symbol = (Symbol) result;

            Assert.AreEqual("x", symbol.Name);
            Assert.AreEqual(machine.Environment.GetValue(Machine.CurrentNamespaceKey), symbol.Namespace);
            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void EvaluateBackquoteWithQualifiedSymbol()
        {
            Machine machine = new Machine();
            Parser parser = new Parser("`foo/x");

            object result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Symbol));

            Symbol symbol = (Symbol)result;

            Assert.AreEqual("x", symbol.Name);
            Assert.AreEqual("foo", symbol.Namespace);
            Assert.IsNull(parser.ParseForm());
        }

        [TestMethod]
        public void EvaluateBackquoteWithSimpleList()
        {
            Machine machine = new Machine();
            Parser parser = new Parser("`(1 2 3)");
            Parser listparser = new Parser("(1 2 3)");

            object result = machine.Evaluate(parser.ParseForm());
            object result2 = listparser.ParseForm();

            Assert.IsTrue(Utilities.Equals(result, result2));
        }

        [TestMethod]
        public void EvaluateBackquoteWithListWithUnquotedSymbol()
        {
            Machine machine = new Machine();
            machine.Environment.SetValue("x", 2);
            Parser parser = new Parser("`(1 ~x 3)");
            Parser listparser = new Parser("(1 2 3)");

            object result = machine.Evaluate(parser.ParseForm());
            object result2 = listparser.ParseForm();

            Assert.IsTrue(Utilities.Equals(result, result2));
        }

        [TestMethod]
        public void EvaluateBackquoteWithListWithUnquotedVariable()
        {
            Machine machine = new Machine();
            Variable variable = Variable.Intern(machine, (string) machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x");
            machine.SetVariableValue(variable, 2);

            Parser parser = new Parser("`(1 ~x 3)");
            Parser listparser = new Parser("(1 2 3)");

            object result = machine.Evaluate(parser.ParseForm());
            object result2 = listparser.ParseForm();

            Assert.IsTrue(Utilities.Equals(result, result2));
        }
    }
}
