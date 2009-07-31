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

        private ValueEnvironment environment;

        internal Namespace(Machine machine, string name)
        {
            this.machine = machine;
            this.name = name;
            // TODO To review, machine initial Environment
            this.environment = new ValueEnvironment(machine.Environment);
        }

        public string Name { get { return this.name; } }
        
        public Machine Machine { get { return this.machine; } }

        public void SetValue(string name, object value)
        {
            this.environment.SetValue(name, value);
        }

        public object GetValue(string name)
        {
            return this.environment.GetValue(name);
        }
    }
}
