﻿namespace AjSharpure.Primitives
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class FnStarPrimitive : IFunction
    {
        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            object[] argumentNames;
            IExpression body;

            if (arguments[0] is Symbol)
            {
                if (!(arguments[1] is object[]))
                    return ApplyMultiFunction(machine, environment, arguments);

                Symbol symbol = (Symbol)arguments[0];
                argumentNames = (object[])arguments[1];
                this.CheckArgumentNames(argumentNames);
                body = Utilities.ToExpression(arguments[2]);

                return new DefinedFunction(symbol.Name, argumentNames, body);
            }

            if (!(arguments[0] is object[]))
                return ApplyMultiFunction(machine, environment, arguments);

            argumentNames = (object[])arguments[0];
            this.CheckArgumentNames(argumentNames);
            body = Utilities.ToExpression(arguments[1]);

            return new DefinedFunction(null, argumentNames, body);
        }

        public object ApplyMultiFunction(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            Symbol symbol = null;
            List<DefinedFunction> functions = new List<DefinedFunction>();

            if (arguments[0] is Symbol)
                symbol = (Symbol)arguments[0];

            for (int k = ((symbol == null) ? 0 : 1); k < arguments.Length; k++)
            {
                object[] funarguments = (new ArrayList((IList)arguments[k])).ToArray();
                functions.Add((DefinedFunction)this.Apply(machine, environment, funarguments));
            }

            return new DefinedMultiFunction((symbol == null) ? null : symbol.Name, functions);
        }

        public bool IsSpecialForm
        {
            get { return true; }
        }

        private void CheckArgumentNames(object[] argumentNames)
        {
            int nvars = -1;

            foreach (object name in argumentNames)
            {
                if (name == null || !(name is Symbol))
                    throw new InvalidOperationException("Invalid argument in function");

                Symbol symbol = (Symbol)name;

                if (!string.IsNullOrEmpty(symbol.Namespace))
                    throw new InvalidOperationException("Qualified symbol is not a valid argument");

                if (nvars >= 0)
                    nvars++;

                if (symbol.Name == "&")
                {
                    if (nvars == -1)
                        nvars++;
                    else
                        throw new InvalidOperationException("Repeated '&' in argument list");
                }
            }

            if (nvars != -1 && nvars != 1)
                throw new InvalidOperationException("Invalid arguments");
        }
    }
}
