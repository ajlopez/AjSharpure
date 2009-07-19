namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    public class EmptyList : BaseObject, IPersistentList, ISequence, IList, ICounted
    {
        private static EmptyList instance = new EmptyList();

        public static EmptyList Instance { get { return instance; } }

        private EmptyList()
        {
        }

        private EmptyList(IPersistentMap metadata)
            : base(metadata)
        {
        }

        public object First()
        {
            return null;
        }

        public ISequence Next()
        {
            return null;
        }

        public ISequence More()
        {
            return this;
        }

        public ISequence ToSequence()
        {
            return null;
        }

        public IPersistentCollection Cons(object obj)
        {
            return new PersistentList(obj, null, 1, this.Metadata);
        }

        ISequence ISequence.Cons(object obj)
        {
            return (ISequence) this.Cons(obj);
        }

        public object Peek()
        {
            return null;
        }

        public IPersistentStack Pop()
        {
            throw new InvalidOperationException("Can't pop empty list");
        }

        public bool IsEmpty { get { return true; } }

        public int Count { get { return 0; } }

        public IPersistentCollection Empty { get { return this; } }

        public override IObject WithMetadata(IPersistentMap metadata)
        {
            if (this.Metadata == metadata)
                return this;

            return new EmptyList(metadata);
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public override bool Equals(object obj)
        {
            // TODO what if obj == null
            return (obj is ISequential || obj is IList) && Utilities.ToSequence(obj) == null;
        }

        public bool Equiv(object obj)
        {
            return this.Equals(obj);
        }

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
            return false;
        }

        public int IndexOf(object value)
        {
            return -1;
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

        public object this[int index]
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
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
