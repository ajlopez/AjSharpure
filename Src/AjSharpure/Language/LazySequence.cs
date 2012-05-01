namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    // TODO LazySequence implements IList
    public sealed class LazySequence : BaseObject, ISequence, IList
    {
        private IFn function;
        private ISequence sequence;

        public LazySequence(IFn function)
        {
            this.function = function;
        }

        private LazySequence(ISequence sequence, IPersistentMap metadata)
            : base(metadata)
        {
            this.function = null;
            this.sequence = sequence;
        }

        public ISequence Empty { get { return EmptyList.Instance; } }

        IPersistentCollection IPersistentCollection.Empty { get { return EmptyList.Instance; } }

        public int Count
        {
            get
            {
                int count = 0;

                for (ISequence sequence = this.ToSequence(); sequence != null; sequence = sequence.Next())
                    count++;

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

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
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

        public override IObject WithMetadata(IPersistentMap metadata)
        {
            return new LazySequence(this.ToSequence(), metadata);
        }

        public ISequence ToSequence()
        {
            lock (this) {
                if (this.function != null)
                {
                    this.sequence = Utilities.ToSequence(this.function.Invoke());
                    this.function = null;
                }

                return this.sequence;
            }
        }

        public bool Equiv(object obj)
        {
            return this.Equals(obj);
        }

        public object First()
        {
            this.ToSequence();

            if (this.sequence == null)
                return null;

            return this.sequence.First();
        }

        public ISequence Next()
        {
            this.ToSequence();

            if (this.sequence == null)
                return null;

            return this.sequence.Next();
        }

        public ISequence More()
        {
            this.ToSequence();

            if (this.sequence == null)
                return EmptyList.Instance;

            return this.sequence.Next();
        }

        public ISequence Cons(object obj)
        {
            return Operations.Cons(obj, this);
        }

        IPersistentCollection IPersistentCollection.Cons(object obj)
        {
            return this.Cons(obj);
        }

        public bool Contains(object obj)
        {
            for (ISequence sequence = this.ToSequence(); sequence != null; sequence = sequence.Next())
                if (Utilities.Equiv(sequence.First(), obj))
                    return true;

            return false;
        }

        public int IndexOf(object obj)
        {
            ISequence sequence = this.ToSequence();

            for (int k = 0; sequence != null; sequence = sequence.Next(), k++)
                if (Utilities.Equiv(sequence.First(), obj))
                    return k;

            return -1;
        }

        public IEnumerator GetEnumerator()
        {
            return new SequenceEnumerator(this.ToSequence());
        }

        public override int GetHashCode()
        {
            return Utilities.Hash(this.ToSequence());
        }

        public override bool Equals(object obj)
        {
            ISequence sequence = this.ToSequence();

            if (sequence != null)
                return sequence.Equiv(obj);

            // TODO to review, Java implementation doesn't have this condition
            if (obj == null)
                return true;

            return (obj is ISequential || obj is IList) && Utilities.ToSequence(obj) == null;
        }

        public int Add(object value)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
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

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
    }
}
