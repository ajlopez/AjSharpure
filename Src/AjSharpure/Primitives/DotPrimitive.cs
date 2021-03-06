﻿namespace AjSharpure.Primitives
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
        public bool IsSpecialForm
        {
            get { return true; }
        }

        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            object name = arguments[0];

            object result = machine.Evaluate(name, environment);

            Type type = null;

            if (name is INamed || name is string)
                type = Utilities.GetType(name);

            if (type != null)
                return this.ApplyToType(type, machine, environment, arguments);

            return this.ApplyToObject(result, machine, environment, arguments);
        }

        public object ApplyToType(Type type, Machine machine, ValueEnvironment environment, object[] arguments)
        {
            INamed named = null;
            object[] pars = null;

            if (arguments[1] is IList)
            {
                IList parameters = (IList)arguments[1];
                named = (INamed)parameters[0];
                pars = new object[parameters.Count - 1];
                for (int k = 1; k < parameters.Count; k++)
                    pars[k - 1] = machine.Evaluate(parameters[k], environment);
            }
            else
            {
                named = (INamed)arguments[1];
                pars = new object[arguments.Length - 2];

                for (int k = 2; k < arguments.Length; k++)
                    pars[k - 2] = machine.Evaluate(arguments[k], environment);
            }

            return type.InvokeMember(named.Name, System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Static, null, null, pars);
        }

        public object ApplyToObject(object obj, Machine machine, ValueEnvironment environment, object[] arguments)
        {
            Type type = obj.GetType();
            INamed named = null;
            object[] pars = null;

            if (arguments[1] is IList)
            {
                IList parameters = (IList)arguments[1];
                named = (INamed)parameters[0];
                pars = new object[parameters.Count - 1];
                for (int k = 1; k < parameters.Count; k++)
                    pars[k - 1] = machine.Evaluate(parameters[k], environment);
            }
            else
            {
                named = (INamed)arguments[1];
                pars = new object[arguments.Length - 2];

                for (int k = 2; k < arguments.Length; k++)
                    pars[k - 2] = machine.Evaluate(arguments[k], environment);
            }

            return type.InvokeMember(named.Name, System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Instance, null, obj, pars);
        }
    }
}