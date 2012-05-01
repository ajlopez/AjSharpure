namespace AjSharpure.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    public class MultiplyPrimitive : IFunction
    {
        public bool IsSpecialForm
        {
            get { return false; }
        }

        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            object result = 1;

            if (arguments == null)
                return result;

            foreach (object argument in arguments)
                result = Numbers.Multiply(result, argument);

            return result;
        }
    }
}
