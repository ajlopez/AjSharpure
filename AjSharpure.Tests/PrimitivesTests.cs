namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure;
    using AjSharpure.Compiler;
    using AjSharpure.Language;
    using AjSharpure.Primitives;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PrimitivesTests
    {
        [TestMethod]
        public void ShouldEvaluateDoWithOneSimpleArgument()
        {
            DoPrimitive doprim = new DoPrimitive();
            Machine machine = new Machine();

            object result = doprim.Apply(machine, machine.Environment, new object[] { 1 });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void ShouldEvaluateDoWithoutArguments()
        {
            DoPrimitive doprim = new DoPrimitive();
            Machine machine = new Machine();

            object result = doprim.Apply(machine, machine.Environment, new object[] {});

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldEvaluateDoWithManyArguments()
        {
            DoPrimitive doprim = new DoPrimitive();
            Machine machine = new Machine();

            object result = doprim.Apply(machine, machine.Environment, new object[] { 1, 2, 3 });


            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfOddNumberOfLetBindingArguments()
        {
            LetPrimitive letprim = new LetPrimitive();
            letprim.Apply(null, null, new object[] { new object[] { "x", 1, "y" } });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfInvalidNameInLetArguments()
        {
            LetPrimitive letprim = new LetPrimitive();
            letprim.Apply(new Machine(), null, new object[] { new object[] { "x", 1, "y", 2, 3, 4 } });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfLetBindingArgumentIsNotACollection()
        {
            LetPrimitive letprim = new LetPrimitive();
            letprim.Apply(null, null, new object[] { 123 });
        }

        [TestMethod]
        public void ShouldEvaluateLetWithSimpleBinding()
        {
            LetPrimitive letprim = new LetPrimitive();
            Machine machine = new Machine();

            object result = letprim.Apply(machine, machine.Environment, new object[] { new object[] { Symbol.Create("foo"), "bar" }, Symbol.Create("foo") });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void ShouldEvaluateLetWithTwoBindings()
        {
            LetPrimitive letprim = new LetPrimitive();
            Machine machine = new Machine();

            object result = letprim.Apply(machine, machine.Environment, new object[] { new object[] { Symbol.Create("x"), 1, Symbol.Create("y"), Symbol.Create("x") }, Symbol.Create("y") });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfIfReceivesNullAsArguments()
        {
            IfPrimitive ifprim = new IfPrimitive();
            ifprim.Apply(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfIfReceivesTooFewArguments()
        {
            IfPrimitive ifprim = new IfPrimitive();
            ifprim.Apply(null, null, new object[] { false });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfIfReceivesTooManyArguments()
        {
            IfPrimitive ifprim = new IfPrimitive();
            ifprim.Apply(null, null, new object[] { false, 1, 2, 3 });
        }

        [TestMethod]
        public void ShouldEvaluateSimpleIf()
        {
            IfPrimitive ifprim = new IfPrimitive();

            object result = ifprim.Apply(new Machine(), null, new object[] { true, 1 });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void ShouldEvaluateSimpleIfWithElse()
        {
            IfPrimitive ifprim = new IfPrimitive();

            object result = ifprim.Apply(new Machine(), null, new object[] { false, 1, 2 });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void ShouldEvaluateToNullIfSimpleIfHasNoElse()
        {
            IfPrimitive ifprim = new IfPrimitive();

            object result = ifprim.Apply(new Machine(), null, new object[] { false, 1 });

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ShouldEvaluateSimpleIfWithFalseSymbol()
        {
            IfPrimitive ifprim = new IfPrimitive();
            Machine machine = new Machine();

            object result = ifprim.Apply(machine, machine.Environment, new object[] { Symbol.Create("false"), 1, 2 });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void ShouldEvaluateSimpleRecur()
        {
            RecurPrimitive recurprim = new RecurPrimitive();

            object result = recurprim.Apply(null, null, new object[] { 1, 2, 3 });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RecursionData));

            RecursionData data = (RecursionData)result;

            Assert.IsNotNull(data.Arguments);
            Assert.AreEqual(3, data.Arguments.Length);
            Assert.AreEqual(1, data.Arguments[0]);
            Assert.AreEqual(2, data.Arguments[1]);
            Assert.AreEqual(3, data.Arguments[2]);
        }

        [TestMethod]
        public void ShouldEvaluateLoopWithTwoBindingsAndWithoutRecur()
        {
            LoopPrimitive loopprim = new LoopPrimitive();
            Machine machine = new Machine();

            object result = loopprim.Apply(machine, machine.Environment, new object[] { new object[] { Symbol.Create("x"), 1, Symbol.Create("y"), Symbol.Create("x") }, Symbol.Create("y") });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void ShouldCreateAnObjectWithNew()
        {
            NewPrimitive newprim = new NewPrimitive();
            Machine machine = new Machine();

            object result = newprim.Apply(machine, machine.Environment, new object[] { Symbol.Create("System.IO.FileInfo"), "aFileName.txt"});
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(System.IO.FileInfo));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldRaiseWhenUnknownType()
        {
            NewPrimitive newprim = new NewPrimitive();
            Machine machine = new Machine();

            newprim.Apply(machine, machine.Environment, new object[] { Symbol.Create("NonExistentType"), "aFileName.txt" });
        }

        [TestMethod]
        public void ShouldGetVariableWithCurrentNamespace()
        {
            VarPrimitive varprim = new VarPrimitive();
            Machine machine = new Machine();
            machine.SetVariableValue(Variable.Create((string) machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x"), "bar");

            object result= varprim.Apply(machine, machine.Environment, new object[] { Symbol.Create("x") });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Variable));

            Variable variable = (Variable)result;

            Assert.AreEqual(machine.Environment.GetValue(Machine.CurrentNamespaceKey), variable.Namespace);
            Assert.AreEqual("x", variable.Name);
        }

        [TestMethod]
        public void ShouldGetVariableFromQualifiedSymbol()
        {
            VarPrimitive varprim = new VarPrimitive();
            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            machine.SetVariableValue(Variable.Create("foo", "x"), "bar");

            object result = varprim.Apply(machine, machine.Environment, new object[] { Symbol.Create("foo/x") });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Variable));

            Variable variable = (Variable)result;

            Assert.AreEqual("foo", variable.Namespace);
            Assert.AreEqual("x", variable.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ShouldRaiseIfVarPrimitiveDoesNotReceiveASymbol()
        {
            VarPrimitive varprim = new VarPrimitive();
            Machine machine = new Machine();

            varprim.Apply(machine, machine.Environment, new object[] { "foo" });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfVarPrimitiveReceivesUndefinedSymbol()
        {
            VarPrimitive varprim = new VarPrimitive();
            Machine machine = new Machine();

            varprim.Apply(machine, machine.Environment, new object[] { Symbol.Create("x") });
        }

        [TestMethod]
        public void ShouldDefineASimpleSymbol()
        {
            DefPrimitive defprim = new DefPrimitive();
            Machine machine = new Machine();

            object result = defprim.Apply(machine, machine.Environment, new object[] { Symbol.Create("x"), 1 });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(1, result);

            object value = machine.GetVariableValue(Utilities.GetFullName((string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x"));

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(int));
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfQualifiedSymbol()
        {
            DefPrimitive defprim = new DefPrimitive();
            Machine machine = new Machine();

            defprim.Apply(machine, machine.Environment, new object[] { Symbol.Create("foo/x"), 1 });
        }

        [TestMethod]
        public void ShouldDefineAnSpecialForm()
        {
            DefPrimitive defprim = new DefPrimitive();
            Machine machine = new Machine();
            DefinedSpecialForm sf = new DefinedSpecialForm("x", null, null);

            object result = defprim.Apply(machine, machine.Environment, new object[] { Symbol.Create("x"), sf });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedSpecialForm));
            Assert.IsTrue(sf == result);

            object value = machine.GetVariableValue(Utilities.GetFullName((string)machine.Environment.GetValue(Machine.CurrentNamespaceKey), "x"));

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(DefinedSpecialForm));
            Assert.IsTrue(sf == value);

            object defsf = machine.Environment.GetValue("x");

            Assert.IsNotNull(defsf);
            Assert.IsInstanceOfType(defsf, typeof(DefinedSpecialForm));
            Assert.IsTrue(defsf == value);
        }

        [TestMethod]
        public void ShouldDefineAFunction()
        {
            FnStarPrimitive fnprim = new FnStarPrimitive();
            Machine machine = new Machine();
            Parser parser = new Parser("[x] (+ x 1)");

            object arguments = parser.ParseForm();
            object body = parser.ParseForm();

            object result = fnprim.Apply(machine, machine.Environment, new object[] { arguments, body });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedFunction));

            DefinedFunction func = (DefinedFunction)result;
            Assert.AreEqual(1, func.Arity);
            Assert.IsFalse(func.VariableArity);
        }

        [TestMethod]
        public void ShouldDefineAMultiFunction()
        {
            FnStarPrimitive fnprim = new FnStarPrimitive();
            Machine machine = new Machine();
            Parser parser = new Parser("([x] (+ x 1)) ([x y] (+ x y 1))");

            object[] parameters = new object[2];
            parameters[0] = parser.ParseForm();
            parameters[1] = parser.ParseForm();

            object result = fnprim.Apply(machine, machine.Environment, parameters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedMultiFunction));
        }

        [TestMethod]
        public void ShouldDefineAndInvokeAMultiFunction()
        {
            FnStarPrimitive fnprim = new FnStarPrimitive();
            Machine machine = new Machine();
            Parser parser = new Parser("([x] (+ x 1)) ([x y] (+ x y 1))");

            object[] parameters = new object[2];
            parameters[0] = parser.ParseForm();
            parameters[1] = parser.ParseForm();

            object result = fnprim.Apply(machine, machine.Environment, parameters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedMultiFunction));

            DefinedMultiFunction func = (DefinedMultiFunction)result;

            object result1 = func.Apply(machine, machine.Environment, new object[] { 1 });

            Assert.IsNotNull(result1);
            Assert.IsInstanceOfType(result1, typeof(int));
            Assert.AreEqual(2, result1);

            object result2 = func.Apply(machine, machine.Environment, new object[] { 1, 2 });

            Assert.IsNotNull(result2);
            Assert.IsInstanceOfType(result2, typeof(int));
            Assert.AreEqual(4, result2);
        }

        [TestMethod]
        public void ShouldDefineAFunctionWithVariableArguments()
        {
            FnStarPrimitive fnprim = new FnStarPrimitive();
            Machine machine = new Machine();
            Parser parser = new Parser("[x & xs] (+ x xs)");

            object arguments = parser.ParseForm();
            object body = parser.ParseForm();

            object result = fnprim.Apply(machine, machine.Environment, new object[] { arguments, body });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedFunction));

            DefinedFunction func = (DefinedFunction) result;
            Assert.AreEqual(1, func.Arity);
            Assert.IsTrue(func.VariableArity);
        }

        [TestMethod]
        public void ShouldDefineAndInvokeAFunctionWithVariableArguments()
        {
            FnStarPrimitive fnprim = new FnStarPrimitive();
            Machine machine = new Machine();
            Parser parser = new Parser("[& coll] coll");

            object arguments = parser.ParseForm();
            object body = parser.ParseForm();

            object result = fnprim.Apply(machine, machine.Environment, new object[] { arguments, body });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedFunction));

            DefinedFunction func = (DefinedFunction)result;
            Assert.AreEqual(0, func.Arity);
            Assert.IsTrue(func.VariableArity);

            object value = func.Apply(machine, machine.Environment, new object[] { 1, 2 });

            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(value, typeof(IList));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfInvalidArgumentNameInFunction()
        {
            FnStarPrimitive fnprim = new FnStarPrimitive();
            Machine machine = new Machine();
            Parser parser = new Parser("[1] (+ x xs)");

            object arguments = parser.ParseForm();
            object body = parser.ParseForm();

            fnprim.Apply(machine, machine.Environment, new object[] { arguments, body });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfQualifiedArgumentNameInFunction()
        {
            FnStarPrimitive fnprim = new FnStarPrimitive();
            Machine machine = new Machine();
            Parser parser = new Parser("[foo/bar] (+ x xs)");

            object arguments = parser.ParseForm();
            object body = parser.ParseForm();

            fnprim.Apply(machine, machine.Environment, new object[] { arguments, body });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfNoVarsArguments()
        {
            FnStarPrimitive fnprim = new FnStarPrimitive();
            Machine machine = new Machine();
            Parser parser = new Parser("[&] (+ x xs)");

            object arguments = parser.ParseForm();
            object body = parser.ParseForm();

            fnprim.Apply(machine, machine.Environment, new object[] { arguments, body });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfTooManyVarsArguments()
        {
            FnStarPrimitive fnprim = new FnStarPrimitive();
            Machine machine = new Machine();
            Parser parser = new Parser("[& x y] (+ x xs)");

            object arguments = parser.ParseForm();
            object body = parser.ParseForm();

            fnprim.Apply(machine, machine.Environment, new object[] { arguments, body });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfTooManyVarsMarkers()
        {
            FnStarPrimitive fnprim = new FnStarPrimitive();
            Machine machine = new Machine();
            Parser parser = new Parser("[& x & y] (+ x xs)");

            object arguments = parser.ParseForm();
            object body = parser.ParseForm();

            fnprim.Apply(machine, machine.Environment, new object[] { arguments, body });
        }
    }
}
