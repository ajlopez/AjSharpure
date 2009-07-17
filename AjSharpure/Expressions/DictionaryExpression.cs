namespace AjSharpure.Expressions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DictionaryExpression : IExpression
    {
        private IDictionary dictionary;

        public DictionaryExpression(IDictionary dictionary)
        {
            this.dictionary = dictionary;
        }

        public object Value { get { return this.dictionary; } }

        public object Evaluate(Machine machine, ValueEnvironment environment)
        {
            IDictionary result = new Hashtable();

            foreach (object key in this.dictionary.Keys)
            {
                object value = this.dictionary[key];

                object newkey = machine.Evaluate(key, environment);

                object newvalue = machine.Evaluate(value, environment);

                result[newkey] = newvalue;
            }

            return result;
        }
    }
}
