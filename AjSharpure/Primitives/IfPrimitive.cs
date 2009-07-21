namespace AjSharpure.Primitives
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class IfPrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            if (arguments == null || arguments.Length < 2)
                throw new InvalidOperationException("If should receive a test and a body");
            if (arguments.Length > 3)
                throw new InvalidOperationException("Too many arguments to If");

            object result = machine.Evaluate(arguments[0], environment);

            if (Utilities.IsFalse(result))
            {
                if (arguments.Length == 3)
                    return machine.Evaluate(arguments[2], environment);
                return null;
            }
            else
                return machine.Evaluate(arguments[1], environment);
        }

        public bool IsMacro
        {
            get { return true; }
        }
    }
}