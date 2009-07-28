namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DictionaryObject : BaseObject, IDictionaryObject
    {
        private static DictionaryObject empty = new DictionaryObject(new Hashtable());

        private IDictionary values;

        public DictionaryObject(IDictionary values)
            : this(values, null)
        {
        }

        public DictionaryObject(IDictionary values, IPersistentMap metadata)
            : base(metadata)
        {
            this.values = values;
        }

        public override IObject WithMetadata(IPersistentMap metadata)
        {
            if (this.Metadata == metadata)
                return this;

            return new DictionaryObject(this.values, metadata);
        }

        #region IDictionary Members

        public void Add(object key, object value)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(object key)
        {
            return this.values.Contains(key);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        public bool IsFixedSize
        {
            get { return true; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public ICollection Keys
        {
            get { return this.values.Keys; }
        }

        public void Remove(object key)
        {
            throw new NotSupportedException();
        }

        public ICollection Values
        {
            get { return this.values.Values; }
        }

        public object this[object key]
        {
            get
            {
                return this.values[key];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            this.values.CopyTo(array,index);
        }

        public int Count
        {
            get { return this.values.Count; }
        }

        public bool IsSynchronized
        {
            get { return true; }
        }

        public object SyncRoot
        {
            get { return this.values; }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        #endregion

        #region IAssociative Members

        public bool ContainsKey(object key)
        {
            return this.Contains(key);
        }

        public DictionaryEntry EntryAt(object key)
        {
            IDictionaryEnumerator enumerator = this.GetEnumerator();

            while (enumerator.MoveNext())
                if (enumerator.Key.Equals(key))
                    return enumerator.Entry;

            throw new InvalidOperationException(string.Format("Unexistent key {0}", key.ToString()));
        }

        public IAssociative Associate(object key, object value)
        {
            if (this.Contains(key) && Utilities.Equals(this[key], value))
                return this;

            // TODO Reimplement sharing values and keys
            IDictionary dict = new Hashtable(this.values);
            dict[key] = value;
            return new DictionaryObject(dict);
        }

        public object ValueAt(object key)
        {
            return this[key];
        }

        public object ValueAt(object key, object notFound)
        {
            if (!this.Contains(key))
                return notFound;

            return this[key];
        }

        #endregion

        #region IPersistentCollection Members

        public IPersistentCollection Cons(object obj)
        {
            if (obj is DictionaryEntry)
            {
                return this.Associate(((DictionaryEntry)obj).Key, ((DictionaryEntry)obj).Value);
            }

            if (obj is System.Array)
            {
                System.Array array = (System.Array)obj;

                if (array.Length != 2)
                    throw new InvalidOperationException("Array should be a pair");

                return this.Associate(array.GetValue(0), array.GetValue(1));
            }

            // TODO Implements on IPersistentVector or alike, as VectorObject

            IAssociative result = this;

            for (ISequence sequence = Utilities.ToSequence(obj); sequence != null; sequence = sequence.Next())
            {
                object first = sequence.First();

                result = result.Associate(((DictionaryEntry)first).Key, ((DictionaryEntry)first).Value);
            }

            return result;
        }

        public IPersistentCollection Empty
        {
            get { return DictionaryObject.empty; }
        }

        public bool Equiv(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is IDictionary))
                return false;

            if (obj == this)
                return true;

            IDictionary dictionary = (IDictionary)obj;

            if (dictionary.Count != this.Count)
                return false;

            foreach (object key in this.Keys)
                if (!dictionary.Contains(key) || !Utilities.Equiv(this[key], dictionary[key]))
                    return false;

            return true;
        }

        #endregion

        #region ISequenceable Members

        public ISequence ToSequence()
        {
            return EnumeratorSequence.Create(this.GetEnumerator());
        }

        #endregion
    }
}
