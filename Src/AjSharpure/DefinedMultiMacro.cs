namespace AjSharpure
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class DefinedMultiMacro : IFunction
    {
        private string name;
        private List<DefinedMacro> macros;

        public DefinedMultiMacro(string name, List<DefinedMacro> macros)
        {
            this.name = name;
            this.macros = macros;
        }

        public string Name { get { return this.name; } }

        public virtual bool IsSpecialForm
        {
            get { return true; }
        }

        public object Apply(Machine machine, ValueEnvironment environment, object[] argumentValues)
        {
            foreach (DefinedMacro macro in this.macros)
                if ((macro.Arity == 0 && argumentValues == null) || macro.Arity == argumentValues.Length || (macro.Arity <= argumentValues.Length && macro.VariableArity))
                    return macro.Apply(machine, environment, argumentValues);

            throw new InvalidOperationException("Invalid number of parameters");
        }
    }
}
