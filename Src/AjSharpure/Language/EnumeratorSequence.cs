namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class EnumeratorSequence : BaseSequence
    {
        private IEnumerator enumerator;
        private object first;
        private ISequence rest;
        private bool restWasCalculated;

        public static EnumeratorSequence Create(IEnumerator enumerator)
        {
            if (enumerator == null)
                return null;

            if (!enumerator.MoveNext())
                return null;

            return new EnumeratorSequence(enumerator, enumerator.Current);
        }

        private EnumeratorSequence(IEnumerator enumerator, object first)
            : this(enumerator, first, null)
        {
        }

        private EnumeratorSequence(IEnumerator enumerator, object first, IPersistentMap metadata)
            : base(metadata)
        {
            this.enumerator = enumerator;
            this.first = first;
        }

        private EnumeratorSequence(IEnumerator enumerator, object first, ISequence rest, IPersistentMap metadata)
            : base(metadata)
        {
            this.enumerator = enumerator;
            this.first = first;
            this.rest = rest;
            this.restWasCalculated = true;
        }

        public override IObject WithMetadata(IPersistentMap metadata)
        {
            if (this.Metadata == metadata)
                return this;

            lock (this)
            {
                if (this.restWasCalculated)
                    return new EnumeratorSequence(this.enumerator, this.first, metadata);
                else
                    return new EnumeratorSequence(this.enumerator, this.first, this.rest, metadata);
            }
        }

        public override object First()
        {
            return this.first;
        }

        public override ISequence Next()
        {
            if (!this.restWasCalculated)
            {
                lock (this)
                {
                    if (!this.restWasCalculated)
                    {
                        this.rest = Create(this.enumerator);
                        this.restWasCalculated = true;
                    }
                }
            }

            return this.rest;
        }
    }
}
