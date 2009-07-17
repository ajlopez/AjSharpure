namespace AjSharpure.Primitives
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class DotPrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            object name = arguments[0];

            if (Utilities.IsEvaluable(name))
            {
                IExpression expression = Utilities.ToExpression(name);

                name = expression.Evaluate(machine, environment);
            }

            if (!(name is Type))
                throw new ArgumentException("It's not a type");

            Type type = (Type)name;

            IList parameters = (IList) arguments[1];
            INamed named = (INamed) parameters[0];

            object[] pars = new object[parameters.Count-1];

            for (int k=1; k<parameters.Count; k++)
                pars[k-1] = machine.Evaluate(parameters[k]);

            return type.InvokeMember(named.Name, System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Static, null, null, pars);
        }

        public bool IsMacro
        {
            get { return true; }
        }
    }
}