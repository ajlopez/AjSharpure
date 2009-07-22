﻿namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class BaseSequence : BaseObject, ISequence, IList
    {
        protected BaseSequence()
        {
        }

        protected BaseSequence(IPersistentMap metadata)
            : base(metadata)
        {
        }

        public IPersistentCollection Empty { get { return EmptyList.Instance; } }

        IPersistentCollection IPersistentCollection.Cons(object obj)
        {
            return new Cons(obj, this);
        }

        public abstract object First();

        public abstract ISequence Next();

        public bool Equiv(object obj)
        {
            if (!(obj is ISequential || obj is IList))
                return false;

            ISequence objseq = Utilities.ToSequence(obj);

            for (ISequence sequence = this.ToSequence(); sequence != null; sequence = sequence.Next(), objseq = objseq.Next())
                if (objseq == null || !Utilities.Equiv(sequence.First(), objseq.First()))
                    return false;

            return objseq == null;
        }

        public ISequence ToSequence()
        {
            return this;
        }

        public virtual ISequence More()
        {
            ISequence sequence = this.Next();

            if (sequence == null)
                return EmptyList.Instance;

            return sequence;
        }

        public ISequence Cons(object obj)
        {
            return new Cons(obj, this);
        }

        #region IList Members

        public int Add(object value)
        {
            throw new NotImplementedException();
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

        public object this[int index]
        {
            get
            {
                return Operations.NthElement(this, index);
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

        public int Count
        {
            get {
                int count = 1;

                for (ISequence sequence = this.Next(); sequence != null; sequence = sequence.Next(), count++)
                    if (sequence is ICounted)
                        return count + ((ICounted)sequence).Count;

                return count;
            }
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
            return new SequenceEnumerator(this);
        }

        #endregion
    }
}
