namespace AjSharpure.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class DefPrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            Symbol symbol = (Symbol)arguments[0];

            object value = machine.Evaluate(arguments[1], environment);

            environment.SetValue(symbol.FullName, value);

            return value;
        }

        public bool IsMacro
        {
            get { return true; }
        }
    }
}
