namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PersistentVector : BasePersistentVector
    {
        private object[] root;
        private object[] tail;
        private int count;
        private int shift;

        private static object[] emptyArray = new object[0];

        private const int NodeSize = 32;
        private const int Shift = 5;

        private static PersistentVector emptyVector = new PersistentVector(0, Shift, emptyArray, emptyArray);

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

            foreach (object element in elements)
                vector = vector.Cons(element);

            return vector;
        }

        private PersistentVector(int count, int shift, object[] root, object[] tool)
            : this(null, count, shift, root, tool)
        {
        }

        private PersistentVector(IPersistentMap metadata, int count, int shift, object[] root, object[] tail)
            : base(metadata)
        {
            this.count = count;
            this.shift = shift;
            this.root = root;
            this.tail = tail;
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

                return this.tail[index];
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

                return new PersistentVector(this.Metadata, this.count + 1, this.shift, this.root, newtail);
            }

            throw new NotImplementedException();
        }
    }
}
