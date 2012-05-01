namespace AjSharpure.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class RecurPrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            return new RecursionData(arguments);
        }

        public bool IsSpecialForm
        {
            get { return false; }
        }
    }
}
