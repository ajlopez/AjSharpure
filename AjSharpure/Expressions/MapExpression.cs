namespace AjSharpure.Expressions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    public class MapExpression : IExpression
    {
        private IPersistentMap map;

        public MapExpression(IPersistentMap map)
        {
            this.map = map;
        }

        public object Value { get { return this.map; } }

        public object Evaluate(Machine machine, ValueEnvironment environment)
        {
            // TODO get a predefined empty dictionary object
            IPersistentMap map = new DictionaryObject(new Hashtable());

            foreach (DictionaryEntry entry in this.map)
            {
                object key = machine.Evaluate(entry.Key, environment);
                object value = machine.Evaluate(entry.Value, environment);

                map = map.Associate(key, value);
            }

            return map;
        }
    }
}
