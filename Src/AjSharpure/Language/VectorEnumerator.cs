namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    public class VectorEnumerator : IEnumerator
    {
        private IPersistentVector vector;
        private int index;
        private object current;

        public VectorEnumerator(IPersistentVector vector)
        {
            this.vector = vector;
            this.index = 0;
        }

        public object Current
        {
            get { return this.current; }
        }

        public bool MoveNext()
        {
            if (this.vector == null || this.vector.Count <= this.index)
                return false;

            this.current = this.vector[this.index];
            this.index++;

            return true;
        }

        public void Reset()
        {
            this.index = 0;
        }
    }
}
