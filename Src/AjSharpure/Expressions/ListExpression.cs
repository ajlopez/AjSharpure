namespace AjSharpure.Expressions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    public class ListExpression : IExpression
    {
        private IList elements;

        public ListExpression(IList elements)
        {
            this.elements = elements;
        }

        public object Value { get { return this.elements; } }

        public object Evaluate(Machine machine, ValueEnvironment environment)
        {
            if (this.elements == null || this.elements.Count == 0)
                return null;

            IExpression formhead = (IExpression)Utilities.ToExpression(this.elements[0]);

            IFunction function = (IFunction)formhead.Evaluate(machine, environment);

            if (function == null)
            {
                if (this.elements[0] is INamed)
                    throw new InvalidOperationException(string.Format("Unknown form {0}", ((INamed)this.elements[0]).FullName));
                else
                    throw new InvalidOperationException(string.Format("Unknown form {0}", this.elements[0].ToString()));
            }

            object[] arguments = null;

            if (this.elements.Count > 1)
                arguments = new object[this.elements.Count - 1];

            if (function.IsSpecialForm)
                for (int k = 1; k < this.elements.Count; k++)
                    arguments[k - 1] = this.elements[k];
            else
                for (int k = 1; k < this.elements.Count; k++)
                    arguments[k - 1] = machine.Evaluate(this.elements[k], environment);

            return function.Apply(machine, environment, arguments);
        }
    }
}
