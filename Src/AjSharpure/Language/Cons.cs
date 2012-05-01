namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Cons : BaseSequence
    {
        private object first;
        private ISequence rest;

        public Cons(object first)
            : this(first, null, null)
        {
        }

        public Cons(object first, ISequence rest)
            : this(first, rest, null)
        {
        }

        public Cons(object first, ISequence rest, IPersistentMap metadata)
            : base(metadata)
        {
            this.first = first;
            this.rest = rest;
        }

        public override bool IsSynchronized
        {
            get { return true; }
        }

        public override object SyncRoot
        {
            get { return this; }
        }

        public override object First()
        {
            return this.first;
        }

        public override ISequence Next()
        {
            return this.rest;
        }

        public override IObject WithMetadata(IPersistentMap metadata)
        {
            if (this.Metadata == metadata)
                return this;

            return new Cons(this.first, this.rest, metadata);
        }
    }
}
