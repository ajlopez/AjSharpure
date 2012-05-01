namespace AjSharpure.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    internal class FakePersistentMap : IPersistentMap
    {
        public static IPersistentMap Instance = new FakePersistentMap();

        #region IPersistentMap Members

        public IPersistentMap Associate(object key, object value)
        {
            throw new NotImplementedException();
        }

        public IPersistentMap AssociateWithException(object key, object value)
        {
            throw new NotImplementedException();
        }

        public IPersistentMap Without(object key)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAssociative Members

        public bool ContainsKey(object key)
        {
            throw new NotImplementedException();
        }

        public System.Collections.DictionaryEntry EntryAt(object key)
        {
            throw new NotImplementedException();
        }

        IAssociative IAssociative.Associate(object key, object value)
        {
            throw new NotImplementedException();
        }

        public object ValueAt(object key)
        {
            throw new NotImplementedException();
        }

        public object ValueAt(object key, object notFound)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPersistentCollection Members

        public IPersistentCollection Cons(object obj)
        {
            throw new NotImplementedException();
        }

        public IPersistentCollection Empty
        {
            get { throw new NotImplementedException(); }
        }

        public bool Equiv(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISequenceable Members

        public ISequence ToSequence()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICounted Members

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
