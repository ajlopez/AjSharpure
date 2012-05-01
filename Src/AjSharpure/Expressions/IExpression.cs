namespace AjSharpure.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IExpression
    {
        object Value { get; }

        object Evaluate(Machine machine, ValueEnvironment environment);
    }
}
