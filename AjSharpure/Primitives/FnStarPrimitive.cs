namespace AjSharpure.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class FnStarPrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            object[] argumentNames;
            IExpression body;

            if (arguments[0] is Symbol)
            {
                Symbol symbol = (Symbol)arguments[0];
                argumentNames = (object[])arguments[1];
                body = Utilities.ToExpression(arguments[2]);

                return new DefinedFunction(symbol.Name, argumentNames, body);
            }

            argumentNames = (object[])arguments[0];
            body = Utilities.ToExpression(arguments[1]);

            return new DefinedFunction(null, argumentNames, body);
        }

        public bool IsMacro
        {
            get { return true; }
        }
    }
}
