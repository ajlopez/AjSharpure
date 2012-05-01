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


        public DefinedMultiMacro ToMacro()
        {
            List<DefinedMacro> macros = new List<DefinedMacro>();

            foreach (DefinedFunction fn in this.functions)
                macros.Add(fn.ToMacro());

            return new DefinedMultiMacro(this.name, macros);
        }

        public object Apply(Machine machine, ValueEnvironment environment, object[] argumentValues)
        {
            foreach (DefinedFunction func in this.functions)
                if ((func.Arity == 0 && argumentValues == null) || func.Arity == argumentValues.Length || (func.Arity <= argumentValues.Length && func.VariableArity))
                    return func.Apply(machine, environment, argumentValues);

            throw new InvalidOperationException("Invalid number of parameters");
        }

        public virtual bool IsSpecialForm
        {
            get { return false; }
        }
    }
}
