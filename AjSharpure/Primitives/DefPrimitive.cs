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

            if (!string.IsNullOrEmpty(symbol.Namespace))
                throw new InvalidOperationException("Defined name should not have namespace");

            string ns = (string) environment.GetValue(Machine.CurrentNamespaceKey);

            if (!string.IsNullOrEmpty(symbol.Namespace))
                ns = symbol.Namespace;

            object value = machine.Evaluate(arguments[1], environment);

            Variable variable = Variable.Create(ns, symbol.Name);

            machine.SetVariableValue(variable, value);

            if (value is IFunction && ((IFunction) value).IsSpecialForm)
                machine.Environment.SetValue(variable.Name, value, true);

            return value;
        }

        public bool IsSpecialForm
        {
            get { return true; }
        }
    }
}
