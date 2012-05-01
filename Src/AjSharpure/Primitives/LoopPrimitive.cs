namespace AjSharpure.Primitives
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class LoopPrimitive : IFunction
    {
        public bool IsSpecialForm
        {
            get { return true; }
        }

        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
                return null;

            object result = null;
            ValueEnvironment newenv = null;
            string[] names = null;

            newenv = new ValueEnvironment(environment);

            object argument = arguments[0];

            if (argument != null)
            {
                if (!(argument is ICollection))
                    throw new InvalidOperationException("Let must receive a list as first argument");

                names = Utilities.EvaluateBindings(machine, newenv, (ICollection)argument);
            }

            for (int k = 1; k < arguments.Length; k++)
                result = machine.Evaluate(arguments[k], newenv);

            while (result != null && result is RecursionData)
            {
                RecursionData data = (RecursionData)result;

                if (Utilities.GetArity(data.Arguments) != Utilities.GetArity(names))
                    throw new InvalidOperationException("Invalid recursion data");

                newenv = new ValueEnvironment(environment);
                result = null;

                for (int k = 0; k < names.Length; k++)
                    newenv.SetValue(names[k], data.Arguments[k]);

                for (int k = 1; k < arguments.Length; k++)
                    result = machine.Evaluate(arguments[k], newenv);
            }

            return result;
        }
    }
}