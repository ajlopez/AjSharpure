namespace AjSharpure.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    public class DividePrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
                throw new InvalidOperationException("Divide should have an argument at least");

            if (arguments.Length == 1)
                return Numbers.Divide(1, arguments[0]);

            object result = arguments[0];

            for (int k = 1; k < arguments.Length; k++)
                result = Numbers.Divide(result, arguments[k]);

            return result;
        }

        public bool IsSpecialForm
        {
            get { return false; }
        }
    }
}
