﻿namespace AjSharpure.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    public class EqualPrimitive : IFunction
    {
        public bool IsSpecialForm
        {
            get { return false; }
        }

        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
                throw new InvalidOperationException("Equal should have an argument at least");

            if (arguments.Length == 1)
                return true;

            bool result = true;
            object argument = null;

            for (int k = 0; k < arguments.Length; k++)
            {
                if (k > 0)
                    result = result && Utilities.Equals(argument, arguments[k]);

                if (!result)
                    return false;

                argument = arguments[k];
            }

            return true;
        }
    }
}
