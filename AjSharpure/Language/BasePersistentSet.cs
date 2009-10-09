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
        private int hash = -1;
        private IPersistentMap implementation;

        protected BasePersistentSet(IPersistentMap metadata, IPersistentMap implementation)
            : base(metadata)
        {
            this.implementation = implementation;
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IPersistentSet Members

        public IPersistentSet Disjoin(object obj)
        {
            throw new NotImplementedException();
        }

        public object GetObject(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPersistentCollection Members

        public IPersistentCollection Cons(object obj)
        {
            throw new NotImplementedException();
        }

        public IPersistentCollection Empty
        {
            get { throw new NotImplementedException(); }
        }

        public bool Equiv(object obj)
        {
            return this.Equals(obj);
        }

        #endregion

        #region ICounted Members

        public int Count
        {
            get { return this.implementation.Count; }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
