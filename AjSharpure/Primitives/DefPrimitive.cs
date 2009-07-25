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

            Variable variable = environment.GetVariable(Utilities.GetFullName(ns, symbol.Name));

            if (variable == null)
            {
                variable = Variable.GetVariable(ns, symbol.Name);

                if (variable == null)
                    variable = Variable.Intern(ns, symbol.Name, value, true);
                else
                    variable.Value = value;

                environment.SetValue(variable.FullName, variable);
            }
            else
                variable.Value = value;

            return value;
        }

        public bool IsSpecialForm
        {
            get { return true; }
        }
    }
}
