namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public class Variable : IObject, IComparable, INamed
    {
        private IPersistentMap metadata;
        private string ns;
        private string name;
        private int hash;
        private string fullName;

        protected Variable(string ns, string name)
            : this(ns, name, null)
        {
        }

        protected Variable(string ns, string name, IPersistentMap metadata)
        {
            this.metadata = metadata;
            this.ns = ns;
            this.name = name;
            this.hash = Utilities.CombineHash(name.GetHashCode(), Utilities.Hash(ns));
            this.fullName = Utilities.GetFullName(this.ns, this.name);
        }

        public IPersistentMap Metadata
        {
            get { return this.metadata; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public string Namespace
        {
            get { return this.ns; }
        }

        public string FullName
        {
            get
            {
                return this.fullName;
            }
        }

        public static Variable Intern(Machine machine, string ns, string name)
        {
            if (string.IsNullOrEmpty(ns))
                throw new InvalidOperationException("Variable has no namespace");

            Variable variable = machine.GetVariable(ns, name);

            if (variable != null)
                return variable;

            variable = new Variable(ns, name);

            machine.SetVariable(variable);

            return variable;
        }

        public static Variable Intern(Machine machine, string name)
        {
            if (name.Length == 1 && name.Equals("/"))
                return Intern(machine, null, name);

            int position = name.LastIndexOf('/');

            if (position == -1)
                return Intern(machine, null, name);

            return Intern(machine, name.Substring(0, position), name.Substring(position + 1));
        }

        public IObject WithMetadata(IPersistentMap metadata)
        {
            if (this.Metadata == metadata)
                return this;

            return new Variable(this.ns, this.name, metadata);
        }

        public override int GetHashCode()
        {
            return this.hash;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (!(this is Variable))
                return false;
            
            Variable variable = (Variable)obj;

            return this.name == variable.name && this.ns == variable.ns;
        }

        public int CompareTo(object obj)
        {
            if (this.Equals(obj))
                return 0;

            Variable variable = (Variable)obj;

            if (this.ns == null && variable.ns != null)
                return -1;

            if (this.ns != null)
            {
                if (variable.ns == null)
                    return 1;

                int nsc = this.ns.CompareTo(variable.ns);

                if (nsc != 0)
                    return nsc;
            }

            return this.name.CompareTo(variable.name);
        }

        public void ResetMetadata(IPersistentMap metadata)
        {
            this.metadata = metadata;
        }

        public void SetMacro(Machine machine)
        {
            object obj = machine.GetVariableValue(this);

            if (obj is DefinedFunction)
            {
                obj = ((DefinedFunction)obj).ToMacro();
                machine.SetVariableValue(this, obj);
            }
            else if (obj is DefinedMultiFunction)
            {
                obj = ((DefinedMultiFunction)obj).ToMacro();
                machine.SetVariableValue(this, obj);
            }
            else if (!(obj is DefinedMacro))
                throw new InvalidOperationException();
            
            if (this.metadata != null)
                this.metadata = this.metadata.Associate(Keyword.Create("macro"), true);
            else
            {
                IDictionary dict = new Hashtable();
                dict[Keyword.Create("macro")] = true;
                this.metadata = new DictionaryObject(dict);
            }
        }
    }
}
