namespace AjSharpure
{
    using System;
    using System.Collections.Generic;
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

        public static object NthElement(object obj, int index)
        {
            if (obj == null)
                return null;

            if (obj is string)
                return ((string)obj)[index];

            if (obj is System.Array)
                return ((System.Array)obj).GetValue(index);

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
    }
}
