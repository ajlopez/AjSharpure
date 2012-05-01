namespace AjSharpure.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class SetBangPrimitive : IFunction
    {
        public bool IsSpecialForm
        {
            get { return true; }
        }

        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            Symbol symbol = (Symbol)arguments[0];

            object value = arguments[1];

            if (Utilities.IsEvaluable(value))
            {
                IExpression expression = Utilities.ToExpression(arguments[1]);

                value = expression.Evaluate(machine, environment);
            }

            environment.SetValue(symbol.FullName, value);

            return value;
        }
    }
}
