namespace AjSharpure.Expressions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class VectorExpression : IExpression
    {
        private object[] vector;

        public VectorExpression(object[] vector)
        {
            this.vector = vector;
        }

        public object Value { get { return this.vector; } }

        public object Evaluate(Machine machine, ValueEnvironment environment)
        {
            object[] result = new object[vector.Length];

            for (int k = 0; k < vector.Length; k++)
                result[k] = machine.Evaluate(vector[k], environment);

            return result;
        }
    }
}
