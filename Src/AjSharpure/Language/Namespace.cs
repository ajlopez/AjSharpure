namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Namespace
    {
        private string name;
        private Machine machine;

        private Dictionary<string, object> environment = new Dictionary<string, object>();
        private Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

        internal Namespace(Machine machine, string name)
        {
            this.machine = machine;
            this.name = name;
        }

        public string Name { get { return this.name; } }
        
        public Machine Machine { get { return this.machine; } }

        public void SetValue(string name, object value)
        {
            this.environment[name] = value;
        }

        public object GetValue(string name)
        {
            if (!this.environment.ContainsKey(name))
                return null;

            return this.environment[name];
        }

        public Variable GetVariable(string name)
        {
            if (!this.variables.ContainsKey(name))
                return null;

            return this.variables[name];
        }

        public void SetVariable(Variable variable)
        {
            if (!Utilities.Equals(variable.Namespace, this.name))
                throw new InvalidOperationException("Variable belongs to another namespace");

            if (this.variables.ContainsKey(variable.Name))
                throw new InvalidOperationException(string.Format("Variable {0} already exists in namespace", variable.FullName));

            this.variables[variable.Name] = variable;
        }
    }
}
