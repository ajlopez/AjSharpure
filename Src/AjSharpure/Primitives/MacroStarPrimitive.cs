namespace AjSharpure.Primitives
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class MacroStarPrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            object[] argumentNames;
            IList body;

            if (arguments[0] is Symbol)
            {
                Symbol symbol = (Symbol)arguments[0];
                argumentNames = (object[])arguments[1];
                body = (IList) arguments[2];

                return new DefinedMacro(symbol.Name, argumentNames, body);
            }

            argumentNames = (object[])arguments[0];
            body = (IList) arguments[1];

            return new DefinedMacro(null, argumentNames, body);
        }

        public bool IsSpecialForm
        {
            get { return true; }
        }
    }
}
