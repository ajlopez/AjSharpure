﻿namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    public class ListEnumerator : IEnumerator
    {
        private IList original;
        private IEnumerator enumerator;
        private int offset;

        public ListEnumerator(IList list, int offset)
        {
            this.original = list;
            this.enumerator = list.GetEnumerator();
            this.offset = offset;
            this.AdjustOffset();
        }

        public object Current
        {
            get { return this.enumerator.Current; }
        }

        public bool MoveNext()
        {
            return this.enumerator.MoveNext();
        }

        public void Reset()
        {
            this.enumerator = this.original.GetEnumerator();
            this.AdjustOffset();
        }

        private void AdjustOffset()
        {
            for (int k = 0; k < this.offset; k++)
                this.enumerator.MoveNext();
        }
    }
}
