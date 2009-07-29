namespace AjSharpure.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    public class SymbolExpression : IExpression
    {
        private Symbol symbol;

        public SymbolExpression(Symbol symbol)
        {
            this.symbol = symbol;
        }

        public object Evaluate(Machine machine, ValueEnvironment environment)
        {
            string fullName = null;

            if (string.IsNullOrEmpty(symbol.Namespace))
            {
                // TODO this lookup should be for special forms
                if (machine.Environment.IsDefined(symbol.Name))
                    return machine.Environment.GetValue(symbol.Name);

                // Test if it is a Type
                // TODO import treatment
                if (symbol.Name.IndexOf('.')>0) {
                    Type type = Utilities.GetType(symbol.Name);

                    if (type!=null)
                        return type;
                }

                if (environment.IsDefined(symbol.Name))
                    return environment.GetValue(symbol.Name);

                fullName = Utilities.GetFullName((string) environment.GetValue(Machine.CurrentNamespaceKey), symbol.Name);
            }
            else
                fullName = symbol.FullName;

            return machine.GetVariableValue(fullName);
        }

        public object Value
        {
            get { return this.symbol; }
        }
    }
}
