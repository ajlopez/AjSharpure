namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using AjSharpure.Primitives;

    [TestClass]
    public class ExpressionsSimpleTests
    {
        [TestMethod]
        public void NilEvaluateToNull()
        {
            Assert.IsNull(NilExpression.Instance.Value);
            Assert.IsNull(NilExpression.Instance.Evaluate(null, null));
        }

        [TestMethod]
        public void StringConstantEvaluateToItself()
        {
            string value = "foo";
            ConstantExpression expression = new ConstantExpression(value);

            Assert.AreEqual(value, expression.Value);
            Assert.AreEqual(value, expression.Evaluate(null, null));
        }

        [TestMethod]
        public void IntegerConstantEvaluateToItself()
        {
            int value = 123;
            ConstantExpression expression = new ConstantExpression(value);

            Assert.AreEqual(value, expression.Value);
            Assert.AreEqual(value, expression.Evaluate(null, null));
        }

        [TestMethod]
        public void UndefinedSymbolEvaluateToNull()
        {
            Symbol symbol = Symbol.Create("foo");
            Machine machine = new Machine();
            IExpression expression = new SymbolExpression(symbol);

            Assert.IsNull(expression.Evaluate(machine, machine.Environment));
            Assert.AreEqual(symbol, expression.Value);
        }

        [TestMethod]
        public void EvaluateDefinedSymbol()
        {
            Symbol symbol = Symbol.Create("foo");
            Machine machine = new Machine();
            machine.Environment.SetValue(symbol.FullName, "bar");

            IExpression expression = new SymbolExpression(symbol);

            Assert.AreEqual("bar", expression.Evaluate(machine, machine.Environment));
            Assert.AreEqual(symbol, expression.Value);
        }

        [TestMethod]
        public void EvaluateUnqualifiedSymbolAsDefinedVariableInCurrentNamespace()
        {
            Symbol symbol = Symbol.Create("foo");
            Machine machine = new Machine();
            Variable variable = Variable.Intern(machine, (string) machine.Environment.GetValue(Machine.CurrentNamespaceKey), "foo");

            machine.SetVariableValue(variable, "bar");

            IExpression expression = new SymbolExpression(symbol);

            Assert.AreEqual("bar", expression.Evaluate(machine, machine.Environment));
            Assert.AreEqual(symbol, expression.Value);
        }

        [TestMethod]
        public void EvaluateListExpressionWithQuotedValued()
        {
            ListExpression expression = new ListExpression(new object[] { new QuotePrimitive(), 1 });

            Assert.AreEqual(1, expression.Evaluate(null, null));
        }

        [TestMethod]
        public void EvaluateListExpressionWithNullElements()
        {
            ListExpression expression = new ListExpression(null);

            Assert.IsNull(expression.Evaluate(null, null));
        }

        [TestMethod]
        public void EvaluateListExpressionWithNoElements()
        {
            ListExpression expression = new ListExpression(new object[] { });

            Assert.IsNull(expression.Evaluate(null, null));
        }
    }
}
