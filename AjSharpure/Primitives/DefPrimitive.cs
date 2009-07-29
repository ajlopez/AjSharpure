namespace AjSharpure.Primitives
{
    using System;
    using System.Collections;
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

            Variable variable = Utilities.ToVariable(machine, environment, symbol);

            object value = machine.Evaluate(arguments[1], environment);

            machine.SetVariableValue(variable, value);

            if (symbol.Metadata != null)
            {
                IDictionary dictionary = (IDictionary)symbol.Metadata;
                IDictionary evaluated = new Hashtable();

                foreach (object key in dictionary.Keys)
                {
                    object val = machine.Evaluate(dictionary[key], environment);
                    evaluated[key] = val;
                }

                variable.ResetMetadata(new DictionaryObject(evaluated));
            }
            else
                variable.ResetMetadata(null);

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
