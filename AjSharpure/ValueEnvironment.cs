namespace AjSharpure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    public class ValueEnvironment
    {
        private ValueEnvironment parent;
        private Dictionary<string, object> values = new Dictionary<string, object>();

        public ValueEnvironment()
        {
        }

        public ValueEnvironment(ValueEnvironment parent)
        {
            this.parent = parent;
        }

        public void SetValue(string key, object value)
        {
            if (this.values.ContainsKey(key))
                throw new InvalidOperationException("A named value cannot be redefined");

            this.values[key] = value;
        }

        public void SetValue(string key, object value, bool reset)
        {
            if (!reset && this.values.ContainsKey(key))
                throw new InvalidOperationException("A named value cannot be redefined");

            this.values[key] = value;
        }

        public object GetValue(string key)
        {
            if (!this.values.ContainsKey(key))
            {
                if (this.parent != null)
                    return this.parent.GetValue(key);

                return null;
            }

            object value = this.values[key];

            return value;
        }

        public bool IsDefined(string key)
        {
            if (this.values.ContainsKey(key))
                return true;

            if (this.parent != null)
                return this.parent.IsDefined(key);

            return false;
        }
    }
}
