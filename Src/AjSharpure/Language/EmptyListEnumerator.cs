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

        private EmptyListEnumerator()
        {
        }

        public static IEnumerator Instance { get { return instance; } }

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
