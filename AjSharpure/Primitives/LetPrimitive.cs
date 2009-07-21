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

                        EvaluateBindings(machine, newenv, (ICollection)argument);
                    }
                }
                else
                    result = machine.Evaluate(argument, newenv);
            }

            return result;
        }

        public bool IsMacro
        {
            get { return true; }
        }

        private static void EvaluateBindings(Machine machine, ValueEnvironment newenv, ICollection bindings)
        {
            if ((bindings.Count % 2) != 0)
                throw new InvalidOperationException("Let should receive a collection as first argument with even length");

            int k = 0;
            string name = null;

            foreach (object obj in bindings) 
            {
                if ((k % 2) == 0)
                {
                    // TODO review if Name or FullName
                    if (obj is INamed)
                        name = ((INamed)obj).FullName;
                    else if (obj is string)
                        name = (string)obj;
                    else
                        throw new InvalidOperationException("Let expect a symbol or a string to name a value");
                }
                else
                    newenv.SetLocalValue(name, machine.Evaluate(obj, newenv));

                k++;
            }
        }
    }
}