namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    // TODO inherit from BaseFunction
    public abstract class BasePersistentMap : BaseObject, IPersistentMap, IDictionary, IEnumerable
    {
        protected BasePersistentMap()
        {
        }

        protected BasePersistentMap(IPersistentMap metadata)
            : base(metadata)
        {
        }

        public IPersistentCollection Empty
        {
            get { throw new NotImplementedException(); }
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public ICollection Keys
        {
            get { throw new NotImplementedException(); }
        }

        public ICollection Values
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public object this[object key]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        public override string ToString()
        {
            return Utilities.PrintString(this);
        }

        public IPersistentCollection Cons(object obj)
        {
            if (obj is DictionaryEntry)
            {
                return this.Associate(((DictionaryEntry)obj).Key, ((DictionaryEntry)obj).Value);
            }

            throw new NotImplementedException();
        }

        public IPersistentMap Associate(object key, object value)
        {
            throw new NotImplementedException();
        }

        public IPersistentMap AssociateWithException(object key, object value)
        {
            throw new NotImplementedException();
        }

        public IPersistentMap Without(object key)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(object key)
        {
            throw new NotImplementedException();
        }

        public DictionaryEntry EntryAt(object key)
        {
            throw new NotImplementedException();
        }

        IAssociative IAssociative.Associate(object key, object value)
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

        public bool Equiv(object obj)
        {
            throw new NotImplementedException();
        }

        public ISequence ToSequence()
        {
            throw new NotImplementedException();
        }

        public void Add(object key, object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(object key)
        {
            throw new NotImplementedException();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
