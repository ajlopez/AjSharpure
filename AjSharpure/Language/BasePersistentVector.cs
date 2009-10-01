namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    // TODO implement RandomAccess, Comparable, Streamable (Java names)
    // TODO implements evaluation as a function, taking one parameter as index
    public abstract class BasePersistentVector : BaseFunction, IPersistentVector, IList
    {
        public BasePersistentVector(IPersistentMap metadata)
            : base(metadata)
        {
        }

        protected BasePersistentVector()
        {
        }

        public override string ToString()
        {
            return Utilities.PrintString(this);
        }

        #region IPersistentVector Members

        public int Length
        {
            get { throw new NotImplementedException(); }
        }

        public IPersistentVector AssociateN(int index, object value)
        {
            throw new NotImplementedException();
        }

        public IPersistentVector Cons(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAssociative Members

        public bool ContainsKey(object key)
        {
            throw new NotImplementedException();
        }

        public DictionaryEntry EntryAt(object key)
        {
            throw new NotImplementedException();
        }

        public IAssociative Associate(object key, object value)
        {
            throw new NotImplementedException();
        }

        public object ValueAt(object key)
        {
            throw new NotImplementedException();
        }

        public object ValueAt(object key, object notFound)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPersistentCollection Members

        IPersistentCollection IPersistentCollection.Cons(object obj)
        {
            throw new NotImplementedException();
        }

        public IPersistentCollection Empty
        {
            get { throw new NotImplementedException(); }
        }

        public bool Equiv(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISequenceable Members

        public ISequence ToSequence()
        {
            return EnumeratorSequence.Create(this.GetEnumerator());
        }

        #endregion

        #region ICounted Members

        public abstract int Count { get; }

        #endregion

        #region IPersistentStack Members

        public object Peek()
        {
            throw new NotImplementedException();
        }

        public IPersistentStack Pop()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IReversible Members

        public ISequence ReverseSequence()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IList Members

        public int Add(object value)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        public bool IsFixedSize
        {
            get { return true; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public void Remove(object value)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public abstract object this[int index] { get; set; }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            foreach (object element in this)
                array.SetValue(element, index++);
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
            return new VectorEnumerator(this);
        }

        #endregion
    }
}
