namespace AjSharpure
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class DefinedMultiFunction : IFunction
    {
        private string name;
        private List<DefinedFunction> functions;

        public DefinedMultiFunction(string name, List<DefinedFunction> functions)
        {
            this.name = name;
            this.functions = functions;
        }

        public string Name { get { return this.name; } }


        public DefinedMacro ToMacro()
        {
            throw new NotImplementedException();
        }

        public object Apply(Machine machine, ValueEnvironment environment, object[] argumentValues)
        {
            foreach (DefinedFunction func in this.functions)
                if (func.Arity == argumentValues.Length || (func.Arity <= argumentValues.Length && func.VariableArity))
                    return func.Apply(machine, environment, argumentValues);

            throw new InvalidOperationException("Invalid number of parameters");
        }

        public virtual bool IsSpecialForm
        {
            get { return false; }
        }
    }
}
