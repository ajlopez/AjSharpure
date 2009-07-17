namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExpressionsSimpleTests
    {
        [TestMethod]
        public void NilShouldEvaluateToNull()
        {
            Assert.IsNull(NilExpression.Instance.Value);
            Assert.IsNull(NilExpression.Instance.Evaluate(null, null));
        }

        [TestMethod]
        public void StringConstantShouldEvaluateToItself()
        {
            string value = "foo";
            ConstantExpression expression = new ConstantExpression(value);

            Assert.AreEqual(value, expression.Value);
            Assert.AreEqual(value, expression.Evaluate(null, null));
        }

        [TestMethod]
        public void IntegerConstantShouldEvaluateToItself()
        {
            int value = 123;
            ConstantExpression expression = new ConstantExpression(value);

            Assert.AreEqual(value, expression.Value);
            Assert.AreEqual(value, expression.Evaluate(null, null));
        }

        [TestMethod]
        public void UndefinedSymbolShouldEvaluateToNull()
        {
            Symbol symbol = Symbol.Create("foo");
            ValueEnvironment environment = new ValueEnvironment();
            IExpression expression = new SymbolExpression(symbol);

            Assert.IsNull(expression.Evaluate(null, environment));
            Assert.AreEqual(symbol, expression.Value);
        }

        [TestMethod]
        public void ShouldEvaluateDefinedSymbol()
        {
            Symbol symbol = Symbol.Create("foo");
            ValueEnvironment environment = new ValueEnvironment();
            environment.SetValue(symbol.FullName, "bar");

            IExpression expression = new SymbolExpression(symbol);

            Assert.AreEqual("bar", expression.Evaluate(null, environment));
            Assert.AreEqual(symbol, expression.Value);
        }
    }
}
