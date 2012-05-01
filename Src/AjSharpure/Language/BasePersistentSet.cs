namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    // TODO ISet is not in .NET, equivalence?
    // TODO java version implements java.util.Set
    public class BasePersistentSet : BaseFunction, IPersistentSet, ICollection
    {
        private IPersistentMap implementation;

        protected BasePersistentSet(IPersistentMap metadata, IPersistentMap implementation)
            : base(metadata)
        {
            this.implementation = implementation;
        }

        public IPersistentCollection Empty
        {
            get { throw new NotImplementedException(); }
        }

        public int Count
        {
            get { return this.implementation.Count; }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public override string ToString()
        {
            return Utilities.PrintString(this);
        }

        public bool Contains(object obj)
        {
            return this.implementation.ContainsKey(obj);
        }

        public ISequence ToSequence()
        {
            throw new NotImplementedException();
        }

        public IPersistentSet Disjoin(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public IPersistentCollection Cons(object obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public object GetObject(object obj)
        {
            throw new NotImplementedException();
        }

        public bool Equiv(object obj)
        {
            return this.Equals(obj);
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
    }
}
