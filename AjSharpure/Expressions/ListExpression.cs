namespace AjSharpure.Expressions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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

            object[] arguments = null;

            if (elements.Count > 1)
                arguments = new object[elements.Count - 1];

            if (function.IsMacro)
                for (int k = 1; k < elements.Count; k++)
                    arguments[k - 1] = elements[k];
            else
                for (int k = 1; k < elements.Count; k++)
                    arguments[k - 1] = machine.Evaluate(elements[k], environment);

            return function.Apply(machine, environment, arguments);
        }
    }
}
