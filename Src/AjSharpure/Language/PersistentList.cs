namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    // TODO add IList to interface list
    public class PersistentList : BaseSequence, IPersistentList, IReduce, ICounted
    {
        private object first;
        private IPersistentList rest;
        private int count;

        public PersistentList(object first)
        {
            this.first = first;
            this.rest = null;
            this.count = 1;
        }

        internal PersistentList(object first, IPersistentList rest, int count, IPersistentMap metadata)
            : base(metadata)
        {
            this.first = first;
            this.rest = rest;
            this.count = count;
        }

        public static IPersistentList Create(IList list)
        {
            IPersistentList result = EmptyList.Instance;

            Stack<object> values = new Stack<object>();

            foreach (object obj in list)
                values.Push(obj);

            while (values.Count > 0)
                result = (IPersistentList) result.Cons(values.Pop());

            return result;
        }

        public override IObject WithMetadata(IPersistentMap metadata)
        {
            if (this.Metadata == metadata)
                return this;

            return new PersistentList(this.first, this.rest, this.count, metadata);
        }

        public IPersistentCollection Cons(object obj)
        {
            return new PersistentList(obj, this, this.count + 1, this.Metadata);
        }

        public object Reduce(IFn function)
        {
            Object ret = this.First();

            for (ISequence sequence = this.Next(); sequence != null; sequence = sequence.Next())
                ret = function.Invoke(ret, sequence.First());

            return ret;
        }

        public object Reduce(IFn function, object start)
        {
            Object ret = function.Invoke(start, this.First());

            for (ISequence sequence = this.Next(); sequence != null; sequence = sequence.Next())
                ret = function.Invoke(ret, sequence.First());

            return ret;
        }

        public IPersistentStack Pop()
        {
            if (this.rest == null)
                return EmptyList.Instance;

            return this.rest;
        }

        public object Peek()
        {
            return this.First();
        }

        public override ISequence Next()
        {
            if (this.count == 1)
                return null;

            return (ISequence)this.rest;
        }

        public override object First()
        {
            return this.first;
        }
    }
}
