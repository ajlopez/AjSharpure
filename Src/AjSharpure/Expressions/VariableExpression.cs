namespace AjSharpure.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    public class VariableExpression : IExpression
    {
        private Variable variable;

        public VariableExpression(Variable variable)
        {
            this.variable = variable;
        }

        public object Value
        {
            get { return this.variable; }
        }

        public object Evaluate(Machine machine, ValueEnvironment environment)
        {
            return machine.GetVariableValue(this.variable);
        }
    }
}
