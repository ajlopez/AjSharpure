namespace AjSharpure.Primitives
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class ClosurePrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            if (arguments == null || arguments.Length !=1)
                throw new InvalidOperationException("Closure should have an argument");

            IFunction function = (IFunction)arguments[0];

            return new FnFunction(function, machine, environment);
        }

        public bool IsSpecialForm
        {
            get { return false; }
        }
    }
}