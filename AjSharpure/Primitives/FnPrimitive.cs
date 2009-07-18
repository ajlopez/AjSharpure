namespace AjSharpure.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class FnPrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            Symbol symbol = (Symbol)arguments[0];
            object[] argumentNames = (object[])arguments[1];
            IExpression body = Utilities.ToExpression(arguments[2]);

            return new DefinedFunction(symbol.Name, argumentNames, body);
        }

        public bool IsMacro
        {
            get { return true; }
        }
    }
}
