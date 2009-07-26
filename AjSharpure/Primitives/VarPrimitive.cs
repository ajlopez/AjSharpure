namespace AjSharpure.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class VarPrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            Symbol symbol = (Symbol)arguments[0];

            string ns = symbol.Namespace;

            if (string.IsNullOrEmpty(ns))
                ns = (string) environment.GetValue(Machine.CurrentNamespaceKey);

            return Variable.Create(ns, symbol.Name);
        }

        public bool IsSpecialForm
        {
            get { return true; }
        }
    }
}
