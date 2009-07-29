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
            IExpression formhead = (IExpression) Utilities.ToExpression(elements[0]);

            IFunction function = (IFunction) formhead.Evaluate(machine, environment);

            if (function == null)
            {
                if (elements[0] is INamed)
                    throw new InvalidOperationException(string.Format("Unknown form {0}", ((INamed) elements[0]).FullName));
                else
                    throw new InvalidOperationException(string.Format("Unknown form {0}", elements[0].ToString()));
            }

            object[] arguments = null;

            if (elements.Count > 1)
                arguments = new object[elements.Count - 1];

            if (function.IsSpecialForm)
                for (int k = 1; k < elements.Count; k++)
                    arguments[k - 1] = elements[k];
            else
                for (int k = 1; k < elements.Count; k++)
                    arguments[k - 1] = machine.Evaluate(elements[k], environment);

            return function.Apply(machine, environment, arguments);
        }
    }
}
