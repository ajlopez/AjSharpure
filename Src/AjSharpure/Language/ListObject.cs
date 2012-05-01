namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    public class ListObject : BaseSequence, IListObject
    {
        private IList list;
        private int offset;

        private ListObject(IList list)
            : this(list, 0, null)
        {
        }

        private ListObject(IList list, int offset, IPersistentMap metadata)
            : base(metadata)
        {
            this.list = list;
            this.offset = offset;
        }

        public override int Count
        {
            get { return this.list.Count - this.offset; }
        }

        public override object SyncRoot
        {
            get { return this.list; }
        }

        public override object this[int index]
        {
            get
            {
                return this.list[index + this.offset];
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        public static IPersistentList Create(IList list)
        {
            if (list == null || list.Count == 0)
                return EmptyList.Instance;

            return new ListObject(list);
        }

        public static IPersistentList Create(IList list, IPersistentMap metadata)
        {
            if (list == null || list.Count == 0)
                return EmptyList.Instance;

            return new ListObject(list, 0, metadata);
        }

        public object Peek()
        {
            return this.First();
        }

        public IPersistentStack Pop()
        {
            ISequence result = this.Next();

            if (result == null)
                return EmptyList.Instance;

            if (this.Count <= 1)
                return EmptyList.Instance;

            return new ListObject(this.list, this.offset + 1, null);
        }

        public override IObject WithMetadata(IPersistentMap metadata)
        {
            if (this.Metadata == metadata)
                return this;

            return new ListObject(this.list, this.offset, metadata);
        }

        public override ISequence Next()
        {
            if (this.Count <= 1)
                return null;

            return new ListObject(this.list, this.offset + 1, null);
        }

        public override object First()
        {
            if (this.Count == 0)
                return null;

            return this.list[this.offset];
        }

        public override bool Contains(object value)
        {
            if (this.offset == 0)
                return this.list.Contains(value);

            for (int k = this.offset; k < this.list.Count; k++)
                if (Utilities.Equals(value, this.list[k]))
                    return true;

            return false;
        }

        public override int IndexOf(object value)
        {
            if (this.offset == 0)
                return this.list.IndexOf(value);

            for (int k = this.offset; k < this.list.Count; k++)
                if (Utilities.Equals(value, this.list[k]))
                    return k - this.offset;

            return -1;
        }

        public override void CopyTo(Array array, int index)
        {
            if (this.offset == 0)
                this.list.CopyTo(array, index);

            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            if (this.offset == 0)
                return this.list.GetEnumerator();

            return new ListEnumerator(this.list, this.offset);
        }
    }
}
