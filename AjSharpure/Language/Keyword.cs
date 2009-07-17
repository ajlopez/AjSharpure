namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Keyword : BaseObject, INamed, IComparable
    {
        private string ns;
        private string name;
        private int hash;

        public static Keyword Create(string ns, string name)
        {
            return new Keyword(ns, name);
        }

        public static Keyword Create(string name)
        {
            int position = name.LastIndexOf('/');

            if (position == -1)
                return Create(null, name);

            return Create(name.Substring(0, position), name.Substring(position + 1));
        }

        private Keyword(string ns, string name)
            : this(ns, name, null)
        {
        }

        private Keyword(string ns, string name, IPersistentMap metadata)
            : base(metadata)
        {
            this.ns = ns;
            this.name = name;
            this.hash = Utilities.CombineHash(name.GetHashCode(), Utilities.Hash(ns));
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
                return (this.ns == null ? this.name : string.Format("{0}/{1}", this.ns, this.name));
            }
        }

        public override IObject WithMetadata(IPersistentMap metadata)
        {
            if (this.Metadata == metadata)
                return this;

            return new Keyword(this.ns, this.name, metadata);
        }

        public override int GetHashCode()
        {
            return this.hash;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (!(this is Keyword))
                return false;
            
            Keyword keyword = (Keyword) obj;

            return this.name == keyword.name && this.ns == keyword.ns;
        }

        public int CompareTo(object obj)
        {
            if (this.Equals(obj))
                return 0;

            Keyword keyword = (Keyword)obj;

            if (this.ns == null && keyword.ns != null)
                return -1;

            if (this.ns != null)
            {
                if (keyword.ns == null)
                    return 1;

                int nsc = this.ns.CompareTo(keyword.ns);

                if (nsc != 0)
                    return nsc;
            }

            return this.name.CompareTo(keyword.name);
        }
    }
}
