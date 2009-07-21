namespace AjSharpure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class DefinedFunction : IFunction
    {
        private string name;
        private object[] arguments;
        private IExpression expression;

        public DefinedFunction(string name, object[] arguments, IExpression expression)
        {
            this.name = name;
            this.arguments = arguments;
            this.expression = expression;
        }

        public string Name { get { return this.name; } }

        public object Apply(Machine machine, ValueEnvironment environment, object[] argumentValues)
        {
            ValueEnvironment newenv = new ValueEnvironment(environment);

            if (this.name != null)
                newenv.SetLocalValue(this.name, this);

            int k=0;

            foreach (Symbol argname in this.arguments)
                newenv.SetLocalValue(argname.Name, argumentValues[k++]);

            object result = this.expression.Evaluate(machine, newenv);

            while (result != null && result is RecursionData)
            {
                RecursionData data = (RecursionData)result;

                if (Utilities.GetArity(data.Arguments) != Utilities.GetArity(this.arguments))
                    throw new InvalidOperationException("Invalid recursion data");

                newenv = new ValueEnvironment(environment);

                k = 0;

                foreach (Symbol argname in this.arguments)
                    newenv.SetLocalValue(argname.Name, data.Arguments[k++]);

                result = this.expression.Evaluate(machine, newenv);
            }

            return result;
        }

        public bool  IsMacro
        {
        	get { return false; }
        }   
    }
}
