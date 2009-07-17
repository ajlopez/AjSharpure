namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    public class SequenceEnumerator : IEnumerator
    {
        ISequence sequence;
        ISequence original;
        object current;

        public SequenceEnumerator(ISequence sequence)
        {
            this.sequence = sequence;
            this.original = sequence;
        }

        public object Current
        {
            get { return current; }
        }

        public bool MoveNext()
        {
            if (this.sequence == null || this.sequence is EmptyList)
                return false;

            current = this.sequence.First();
            this.sequence = this.sequence.Next();

            return true;
        }

        public void Reset()
        {
            this.sequence = this.original;
        }
    }
}
