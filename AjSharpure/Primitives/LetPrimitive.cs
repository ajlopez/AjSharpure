namespace AjSharpure.Primitives
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class LetPrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            object result = null;
            ValueEnvironment newenv = null;

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

            return result;
        }

        public bool IsSpecialForm
        {
            get { return true; }
        }
    }
}