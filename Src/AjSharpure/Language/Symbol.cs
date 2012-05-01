namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Symbol : BaseObject, INamed, IComparable
    {
        private string ns;
        private string name;
        private int hash;
        private string fullName;

        public static Symbol Create(string ns, string name)
        {
            return new Symbol(ns, name);
        }

        public static Symbol Create(string name)
        {
            if (name.Length == 1 && name.Equals("/"))
                return Create(null, name);

            int position = name.LastIndexOf('/');

            if (position == -1)
                return Create(null, name);

            return Create(name.Substring(0, position), name.Substring(position + 1));
        }

        protected Symbol(string ns, string name)
            : this(ns, name, null)
        {
        }

        protected Symbol(string ns, string name, IPersistentMap metadata)
            : base(metadata)
        {
            this.ns = ns;
            this.name = name;
            this.hash = Utilities.CombineHash(name.GetHashCode(), Utilities.Hash(ns));
            this.fullName = Utilities.GetFullName(this.ns, this.name);
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

        public override IObject WithMetadata(IPersistentMap metadata)
        {
            if (this.Metadata == metadata)
                return this;

            return new Symbol(this.ns, this.name, metadata);
        }

        public override int GetHashCode()
        {
            return this.hash;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (!(this is Symbol))
                return false;
            
            Symbol symbol = (Symbol) obj;

            return this.name == symbol.name && this.ns == symbol.ns;
        }

        public int CompareTo(object obj)
        {
            if (this.Equals(obj))
                return 0;

            Symbol symbol = (Symbol)obj;

            if (this.ns == null && symbol.ns != null)
                return -1;

            if (this.ns != null)
            {
                if (symbol.ns == null)
                    return 1;

                int nsc = this.ns.CompareTo(symbol.ns);

                if (nsc != 0)
                    return nsc;
            }

            return this.name.CompareTo(symbol.name);
        }
    }
}
