namespace AjSharpure
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class DefinedFunction : IFunction
    {
        private string name;
        private ICollection arguments;
        private IExpression expression;
        private int arity = 0;
        private bool variableArity = false;

        public DefinedFunction(string name, ICollection arguments, IExpression expression)
        {
            this.name = name;
            this.arguments = arguments;
            this.expression = expression;

            if (arguments != null)
                foreach (Symbol argname in arguments)
                    if (argname.Name == "&")
                    {
                        this.variableArity = true;
                        break;
                    }
                    else
                        this.arity++;
        }

        public string Name { get { return this.name; } }

        public int Arity { get { return this.arity; } }

        public bool VariableArity { get { return this.variableArity; } }

        public DefinedMacro ToMacro()
        {
            return new DefinedMacro(name, arguments, expression);
        }

        public object Apply(Machine machine, ValueEnvironment environment, object[] argumentValues)
        {
            ValueEnvironment newenv = new ValueEnvironment(environment);

            if (argumentValues == null && this.Arity != 0 && !this.VariableArity)
                throw new InvalidOperationException("Invalid number of parameters");

            if (argumentValues != null && !this.VariableArity && argumentValues.Length != this.Arity)
                throw new InvalidOperationException("Invalid number of parameters");

            if (argumentValues != null && this.VariableArity && argumentValues.Length < this.Arity)
                throw new InvalidOperationException("Invalid number of parameters");

            if (this.name != null)
                newenv.SetValue(this.name, this);

            int k = 0;
            bool islast = false;

            foreach (Symbol argname in this.arguments)
            {
                if (argname.Name == "&")
                    islast = true;
                else
                {
                    if (!islast)
                    {
                        //if (argumentValues == null || argumentValues.Length <= k)
                        //    newenv.SetValue(argname.Name, null);
                        //else
                            newenv.SetValue(argname.Name, argumentValues[k]);

                        k++;
                    }
                    else
                    {
                        IList rest = new ArrayList();

                        while (argumentValues!=null && k < argumentValues.Length)
                            rest.Add(argumentValues[k++]);

                        if (rest.Count > 0)
                            newenv.SetValue(argname.Name, rest);
                        else
                            newenv.SetValue(argname.Name, null);
                    }
                }
            }

            object result = this.expression.Evaluate(machine, newenv);

            while (result != null && result is RecursionData)
            {
                RecursionData data = (RecursionData)result;

                if (Utilities.GetArity(data.Arguments) != Utilities.GetArity(this.arguments))
                    throw new InvalidOperationException("Invalid recursion data");

                newenv = new ValueEnvironment(environment);

                k = 0;

                foreach (Symbol argname in this.arguments)
                    newenv.SetValue(argname.Name, data.Arguments[k++]);

                result = this.expression.Evaluate(machine, newenv);
            }

            return result;
        }

        public virtual bool IsSpecialForm
        {
            get { return false; }
        }
    }
}
