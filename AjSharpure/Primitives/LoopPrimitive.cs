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
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            object result = null;
            ValueEnvironment newenv = null;

            while (true)
            {
                if (arguments != null)
                    foreach (object argument in arguments)
                    {
                        if (newenv == null) // first argument
                        {
                            newenv = new ValueEnvironment(environment);

                            if (argument != null)
                            {
                                if (!(argument is ICollection))
                                    throw new InvalidOperationException("Let must receive a list as first argument");

                                Utilities.EvaluateBindings(machine, newenv, (ICollection)argument);
                            }
                        }
                        else
                            result = machine.Evaluate(argument, newenv);
                    }

                if (result != null && result is RecursionData)
                {
                    RecursionData data = (RecursionData)result;

                    if (Utilities.GetArity(data.Arguments) != Utilities.GetArity(arguments))
                        throw new InvalidOperationException("Invalid recursion data");

                    arguments = data.Arguments;

                    result = null;

                    continue;
                }

                return result;
            }
        }

        public bool IsMacro
        {
            get { return true; }
        }
    }
}