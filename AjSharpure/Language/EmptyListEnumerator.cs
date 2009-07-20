namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class EmptyListEnumerator : IEnumerator
    {
        private static EmptyListEnumerator instance = new EmptyListEnumerator();

        public static IEnumerator Instance { get { return instance; } }

        private EmptyListEnumerator()
        {
        }

        public object Current
        {
            get { throw new NotSupportedException(); }
        }

        public bool MoveNext()
        {
            return false;
        }

        public void Reset()
        {
        }
    }
}
