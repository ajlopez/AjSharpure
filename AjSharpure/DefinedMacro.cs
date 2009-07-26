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
        private object[] arguments;
        private IList body;

        public DefinedMacro(string name, object[] arguments, IList body)
        {
            this.name = name;
            this.arguments = arguments;
            this.body = body;
        }

        public string Name { get { return this.name; } }

        public object Apply(Machine machine, ValueEnvironment environment, object[] argumentValues)
        {
            ValueEnvironment newenv = new ValueEnvironment(environment);

            if (this.name != null)
                newenv.SetValue(this.name, this);

            int k=0;

            foreach (Symbol argname in this.arguments)
                newenv.SetValue(argname.Name, argumentValues[k++]);

            object result = machine.Evaluate(MacroUtilities.Expand(this.body, machine, newenv), environment);

            result = machine.Evaluate(result, environment);

            return result;
        }

        public bool  IsSpecialForm
        {
        	get { return true; }
        }   
    }
}
