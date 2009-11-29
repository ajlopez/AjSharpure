namespace AjSharpure.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class BackquotePrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
                return null;

            if (arguments[0] is Symbol)
            {
                Symbol symbol = (Symbol)arguments[0];

                if (string.IsNullOrEmpty(symbol.Namespace))
                    return Symbol.Create((string) environment.GetValue(Machine.CurrentNamespaceKey), symbol.Name);

                return symbol;
            }

            return MacroUtilities.Expand(arguments[0], machine, environment);
        }

        public bool IsSpecialForm
        {
            get { return true; }
        }
    }
}
