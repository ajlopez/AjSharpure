namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PersistentVector : BasePersistentVector
    {
        private List<object[]> root;
        private object[] tail;
        private int count;

        private static object[] emptyArray = new object[0];

        private const int NodeSize = 32;

        private static PersistentVector emptyVector = new PersistentVector(null, emptyArray);

        public static PersistentVector Create(ISequence sequence)
        {
            PersistentVector vector = emptyVector;

            for (; sequence != null; sequence = sequence.Next())
                vector = vector.Cons(sequence.First());

            return vector;
        }

        public static PersistentVector Create(IEnumerable elements)
        {
            PersistentVector vector = emptyVector;

            if (elements != null)
                foreach (object element in elements)
                    vector = vector.Cons(element);

            return vector;
        }

        private PersistentVector(List<object[]> root, object[] tool)
            : this(null, root, tool)
        {
        }

        private PersistentVector(IPersistentMap metadata, List<object[]> root, object[] tail)
            : base(metadata)
        {
            this.root = root;
            this.tail = tail;

            this.count = (tail == null ? 0 : tail.Length) + (root == null ? 0 : root.Count * NodeSize);
        }

        public override int Count
        {
            get
            {
                return this.count;
            }
        }

        public override object this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                    throw new IndexOutOfRangeException();

                if (this.root == null)
                    return this.tail[index];

                if (index < this.root.Count * NodeSize)
                {
                    int node = index / NodeSize;
                    index = index % NodeSize;

                    return this.root[node][index];
                }

                return this.tail[index - this.root.Count * NodeSize];
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        public PersistentVector Cons(object obj)
        {
            if (this.tail.Length < NodeSize)
            {
                object[] newtail = new object[this.tail.Length + 1];

                Array.Copy(this.tail, newtail, this.tail.Length);

                newtail[this.tail.Length] = obj;

                return new PersistentVector(this.Metadata, this.root, newtail);
            }

            List<object[]> newroot;

            if (this.root != null)
                newroot = new List<object[]>(this.root);
            else
                newroot = new List<object[]>();

            newroot.Add(this.tail);

            object[] newtail2 = new object[] { obj };

            return new PersistentVector(this.Metadata, newroot, newtail2);
        }

        public override IObject WithMetadata(IPersistentMap metadata)
        {
            if (this.Metadata == metadata)
                return this;

            return new PersistentVector(metadata, this.root, this.tail);
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
    }
}
