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

        public int Length
        {
            get { return this.Count; }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public IPersistentCollection Empty
        {
            get { throw new NotImplementedException(); }
        }

        public abstract int Count { get; }

        public bool IsFixedSize
        {
            get { return true; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public abstract object this[int index] { get; set; }

        public IPersistentVector AssociateN(int index, object value)
        {
            throw new NotImplementedException();
        }

        public abstract IPersistentVector Cons(object obj);

        public override string ToString()
        {
            return Utilities.PrintString(this);
        }

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

        IPersistentCollection IPersistentCollection.Cons(object obj)
        {
            return this.Cons(obj);
        }

        public bool Equiv(object obj)
        {
            throw new NotImplementedException();
        }

        public ISequence ToSequence()
        {
            return EnumeratorSequence.Create(this.GetEnumerator());
        }

        public object Peek()
        {
            throw new NotImplementedException();
        }

        public IPersistentStack Pop()
        {
            throw new NotImplementedException();
        }

        public ISequence ReverseSequence()
        {
            throw new NotImplementedException();
        }

        public int Add(object value)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public abstract bool Contains(object value);

        public abstract int IndexOf(object value);

        public void Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        public void Remove(object value)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public void CopyTo(Array array, int index)
        {
            foreach (object element in this)
                array.SetValue(element, index++);
        }

        public IEnumerator GetEnumerator()
        {
            return new VectorEnumerator(this);
        }
    }
}
