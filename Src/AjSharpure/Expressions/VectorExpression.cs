namespace AjSharpure.Expressions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    public class VectorExpression : IExpression
    {
        private IPersistentVector vector;

        public VectorExpression(IPersistentVector vector)
        {
            this.vector = vector;
        }

        public object Value { get { return this.vector; } }

        public object Evaluate(Machine machine, ValueEnvironment environment)
        {
            object[] result = new object[this.vector.Count];

            for (int k = 0; k < this.vector.Length; k++)
                result[k] = machine.Evaluate(this.vector[k], environment);

            return PersistentVector.Create(result);
        }
    }
}
