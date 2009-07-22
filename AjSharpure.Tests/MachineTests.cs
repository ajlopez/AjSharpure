﻿namespace AjSharpure.Tests
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

            Variable variable = machine.Environment.GetVariable(fullName);

            Assert.IsNotNull(variable);
            Assert.AreEqual(1, variable.Value);
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

            Variable variable = machine.Environment.GetVariable(fullName);

            Assert.IsNotNull(variable);
            Assert.AreEqual(1, variable.Value);

            parser = new Parser("(def x 2)");

            result = machine.Evaluate(parser.ParseForm());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(2, result);
            Assert.AreEqual(2, variable.Value);
        }

        [TestMethod]
        public void ShouldEvaluateSimpleDef()
        {
            Parser parser = new Parser("(def x 1)");
            Machine machine = new Machine();

            machine.Evaluate(parser.ParseForm());

            string fullName = Utilities.GetFullName((string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x");

            object value = machine.Environment.GetValue(fullName);

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

            object value = machine.Environment.GetValue(fullName);

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
    }
}
