namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    public class VectorEnumerator : IEnumerator
    {
        IPersistentVector vector;
        int index;
        object current;

        public VectorEnumerator(IPersistentVector vector)
        {
            this.vector = vector;
            this.index = 0;
        }

        public object Current
        {
            get { return current; }
        }

        public bool MoveNext()
        {
            if (this.vector == null || this.vector.Count <= index)
                return false;

            current = this.vector[index];
            index++;

            return true;
        }

        public void Reset()
        {
            index = 0;
        }
    }
}
