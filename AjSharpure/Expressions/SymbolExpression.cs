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
            object result = environment.GetValue(symbol.FullName);

            if (result == null && symbol.Namespace == null)
            {
                result = environment.GetValue(Utilities.GetFullName((string)environment.GetValue(Machine.CurrentNamespaceKey), symbol.Name));

                if (result == null)
                    result = environment.GetValue(Utilities.GetFullName(Machine.AjSharpureCoreKey, symbol.Name));
            }

            return result;
        }

        public object Value
        {
            get { return this.symbol; }
        }
    }
}
