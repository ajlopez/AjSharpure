namespace AjSharpure.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    public class LessEqualPrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
                throw new InvalidOperationException("Less or Equal should have an argument at least");

            if (arguments.Length == 1)
                return true;

            bool result = true;
            object argument = null;

            for (int k = 0; k < arguments.Length; k++)
            {
                if (k>0)
                    result = result && (Utilities.Compare(argument, arguments[k])<=0);

                if (!result)
                    return false;

                argument = arguments[k];
            }

            return true;
        }

        public bool IsMacro
        {
            get { return false; }
        }
    }
}
