namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public class Variable
    {
        private static Dictionary<string, Variable> variables = new Dictionary<string,Variable>();
        [ThreadStatic]
        private static Dictionary<string, object> values;
        private string ns;
        private string name;
        private string fullName;
        private object root;

        public static Variable Intern(string ns, string name, object root)
        {
            return Intern(ns, name, root, true);
        }

        public static Variable GetVariable(string ns, string name)
        {
            string fullName = Utilities.GetFullName(ns, name);

            lock (variables)
            {
                if (variables.ContainsKey(fullName))
                    return variables[fullName];

                return null;
            }
        }

        public static Variable Intern(string ns, string name, object root, bool replaceRoot)
        {
            string fullName = Utilities.GetFullName(ns, name);

            Variable var;

            lock (variables) {
                if (variables.ContainsKey(fullName))
                {
                    var = variables[fullName];
                    if (!var.HasRoot() && replaceRoot)
                        var.BindRoot(root);
                }
                else
                {
                    var = new Variable(ns, name, root);
                    variables[fullName] = var;
                }
            }

            return var;
        }

        private Variable(string ns, string name, object root)
        {
            this.ns = ns;
            this.name = name;
            this.root = root;
            this.fullName = Utilities.GetFullName(this.ns, this.name);
        }

        public object Value { 
            get 
            {
                if (values == null || !values.ContainsKey(this.fullName))
                {
                    if (this.HasRoot())
                        return this.root;
                    else
                        return null;
                }

                return values[this.fullName];
            }
            set 
            {
                if (values == null)
                    values = new Dictionary<string, object>();

                values[this.fullName] = value; 
            }
        }

        public object Root { get { return this.root; } }

        public string Namespace { get { return this.ns; } }

        public string Name { get { return this.name; } }

        public string FullName { get { return this.fullName; } }

        private bool HasRoot() 
        {
            return this.root != null;
        }

        private void BindRoot(object root)
        {
            this.root = root;
        }
    }
}
