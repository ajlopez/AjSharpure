namespace AjSharpure
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    public sealed class Operations
    {
        public static ISequence ToSequence(object obj)
        {
            if (obj == null)
                return null;

            if (obj is ISequence)
                return (ISequence)obj;

            if (obj is ISequenceable)
                return ((ISequenceable)obj).ToSequence();

            if (obj is IEnumerable)
                return EnumeratorSequence.Create(((IEnumerable)obj).GetEnumerator());

            throw new ArgumentException("Don't know how to create ISequence from " + obj.GetType().FullName);
        }

        public static ISequence Cons(object obj, object coll)
        {
            if (coll == null)
                return new PersistentList(obj);
            if (coll is ISequence)
                return new Cons(obj, (ISequence)coll);
            else
                return new Cons(obj, ToSequence(coll));
        }

        public static object First(object obj)
        {
            if (obj is ISequence)
                return ((ISequence)obj).First();

            if (obj is IEnumerable)
            {
                IEnumerator enumerator = ((IEnumerable)obj).GetEnumerator();

                if (enumerator.MoveNext())
                    return enumerator.Current;

                return null;
            }

            ISequence sequence = ToSequence(obj);

            if (sequence == null)
                return null;

            return sequence.First();
        }

        public static ISequence Next(object obj)
        {
            if (obj is ISequence)
                return ((ISequence)obj).Next();

            ISequence sequence = ToSequence(obj);

            if (sequence == null)
                return null;

            return sequence.Next();
        }

        public static ISequence More(object obj)
        {
            if (obj is ISequence)
                return ((ISequence)obj).More();

            ISequence sequence = ToSequence(obj);

            if (sequence == null)
                return EmptyList.Instance;

            return sequence.More();
        }

        public static object Second(object obj)
        {
            return First(Next(obj));
        }

        public static object Third(object obj)
        {
            return First(Next(Next(obj)));
        }

        public static object NthElement(object obj, int index)
        {
            if (obj == null)
                return null;

            if (obj is string)
                return ((string)obj)[index];

            if (obj is System.Array)
                return ((System.Array)obj).GetValue(index);

            // Testing BaseSequence, LazySequence to avoid recursive call, they use NthElement to resolve [index]
            if (obj is IList && !(obj is BaseSequence) && !(obj is LazySequence))
                return ((IList)obj)[index];

            if (obj is ISequential)
            {
                ISequence sequence = Utilities.ToSequence(obj);

                for (int i = 0; i <= index && sequence != null; i++, sequence = sequence.Next())
                    if (i == index)
                        return sequence.First();

                throw new IndexOutOfRangeException();
            }

            throw new NotImplementedException();
        }

        public static IPersistentCollection Conj(IPersistentCollection collection, object obj)
        {
            if (collection == null)
                return new PersistentList(obj);

            return collection.Cons(obj);
        }

        public static IAssociative Associate(object collection, object key, object value)
        {
            if (collection == null)
            {
                IDictionary dictionary = new Hashtable();
                dictionary[key] = value;
                return new DictionaryObject(dictionary);
            }

            return ((IAssociative)collection).Associate(key, value);
        }
    }
}
