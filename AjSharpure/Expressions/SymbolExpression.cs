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
            return environment.GetValue(symbol.FullName);
        }

        public object Value
        {
            get { return this.symbol; }
        }
    }
}
