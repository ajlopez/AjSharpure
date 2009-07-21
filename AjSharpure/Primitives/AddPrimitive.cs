namespace AjSharpure.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    public class AddPrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            object result = 0;

            if (arguments == null)
                return result;

            foreach (object argument in arguments)
                result = Numbers.Add(result, argument);

            return result;
        }

        public bool IsMacro
        {
            get { return false; }
        }
    }
}
