namespace AjSharpure.Primitives
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class NewPrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            object name = arguments[0];

            object result = machine.Evaluate(name, environment);

            Type type = null;

            if (!(result is Type))
            {
                type = Utilities.GetType(name);

                if (type == null)
                    throw new ArgumentException("New should receive a type name");
            }
            else
                type = (Type)result;

            object[] parameters = new object[arguments.Length - 1];

            for (int k = 1; k < arguments.Length; k++)
                parameters[k - 1] = machine.Evaluate(arguments[k], environment);

            return Activator.CreateInstance(type, parameters);
        }

        public bool IsMacro
        {
            get { return true; }
        }
    }
}