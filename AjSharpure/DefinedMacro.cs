namespace AjSharpure
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class DefinedMacro : IFunction
    {
        private string name;
        private ICollection arguments;
        private IList body;
        private int arity = 0;
        private bool variableArity = false;

        public DefinedMacro(string name, ICollection arguments, IList body)
        {
            this.name = name;
            this.arguments = arguments;
            this.body = body;

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

        public object Apply(Machine machine, ValueEnvironment environment, object[] argumentValues)
        {
            ValueEnvironment newenv = new ValueEnvironment(environment);

            if (this.name != null)
                newenv.SetValue(this.name, this);

            int k = 0;
            bool islast = false;

            // TODO refactor see DefinedFunction
            foreach (Symbol argname in this.arguments)
            {
                if (argname.Name == "&")
                    islast = true;
                else
                {
                    if (!islast)
                        newenv.SetValue(argname.Name, argumentValues[k++]);
                    else
                    {
                        IList rest = new ArrayList();

                        while (k < argumentValues.Length)
                            rest.Add(argumentValues[k++]);

                        if (rest.Count > 0)
                            newenv.SetValue(argname.Name, rest);
                        else
                            newenv.SetValue(argname.Name, null);
                    }
                }
            }

            object result = machine.Evaluate(MacroUtilities.Expand(this.body, machine, newenv), newenv);

            result = machine.Evaluate(result, environment);

            return result;
        }

        public bool IsSpecialForm
        {
            get { return true; }
        }
    }
}
