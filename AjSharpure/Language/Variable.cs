namespace AjSharpure.Language
{
    using System;
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

        public static Variable Create(string ns, string name)
        {
            return new Variable(ns, name);
        }

        public static Variable Create(string name)
        {
            if (name.Length == 1 && name.Equals("/"))
                return Create(null, name);

            int position = name.LastIndexOf('/');

            if (position == -1)
                return Create(null, name);

            return Create(name.Substring(0, position), name.Substring(position + 1));
        }

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
            
            Variable variable = (Variable) obj;

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
    }
}
