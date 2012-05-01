namespace AjSharpure.Language
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

        public virtual bool IsSynchronized { get { return true; } }

        public virtual object SyncRoot { get { return this; } }

        public bool IsFixedSize
        {
            get { return true; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public virtual int Count
        {
            get
            {
                int count = 1;

                for (ISequence sequence = this.Next(); sequence != null; sequence = sequence.Next(), count++)
                    if (sequence is ICounted)
                        return count + ((ICounted)sequence).Count;

                return count;
            }
        }

        public virtual object this[int index]
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

        public override bool Equals(object obj)
        {
            if (!(obj is ISequence) && !(obj is IEnumerable))
                return false;

            ISequence seq = Utilities.ToSequence(obj);

            for (ISequence myseq = this.ToSequence(); myseq != null; myseq = myseq.Next(), seq = seq.Next())
                if (seq == null || !Utilities.Equals(myseq.First(), seq.First()))
                    return false;

            return seq == null;
        }

        public override int GetHashCode()
        {
            return Utilities.CombineHash(this);
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

        public int Add(object value)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public virtual bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public virtual int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

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

        public virtual void CopyTo(Array array, int index)
        {
            foreach (object element in this)
                array.SetValue(element, index++);
        }

        public virtual IEnumerator GetEnumerator()
        {
            return new SequenceEnumerator(this);
        }
    }
}
