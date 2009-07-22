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

            if (symbol.Namespace != null)
                throw new InvalidOperationException("Defined name should not have namespace");

            string ns = (string) environment.GetValue(Machine.CurrentNamespaceKey);

            if (symbol.Namespace != null)
                ns = symbol.Namespace;

            object value = machine.Evaluate(arguments[1], environment);

            Variable variable = Variable.Intern(ns, symbol.Name, value, true);

            environment.SetValue(variable.FullName, variable);

            return value;
        }

        public bool IsMacro
        {
            get { return true; }
        }
    }
}
