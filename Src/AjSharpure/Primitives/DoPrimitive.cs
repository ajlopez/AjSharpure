namespace AjSharpure.Primitives
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class DoPrimitive : IFunction
    {
        public bool IsSpecialForm
        {
            get { return true; }
        }

        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            object result = null;

            foreach (object argument in arguments)
                result = machine.Evaluate(argument, environment);

            return result;
        }
    }
}