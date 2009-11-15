namespace AjSharpure.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    public class ThrowPrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            throw (Exception)arguments[0];
        }

        public bool IsSpecialForm
        {
            get { return false; }
        }
    }
}
