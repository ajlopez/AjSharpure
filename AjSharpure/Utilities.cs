namespace AjSharpure
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public sealed class Utilities
    {
        public static int Hash(object obj)
        {
            if (obj == null)
                return 0;

            return obj.GetHashCode();
        }

        public static int CombineHash(int seed, int hash)
        {
            //a la boost
            unchecked
            {
                seed ^= hash + (int)0x9e3779b9 + (seed << 6) + (seed >> 2);
            }

            return seed;
        }

        public static bool IsEvaluable(object obj)
        {
            if (obj == null)
                return false;

            if (obj is object[])
                return true;

            if (obj is System.Array)
                return false;

            if (obj is IDictionary)
                return true;

            if (obj is IList || obj is Symbol || obj is IExpression)
                return true;

            return false;
        }

        public static IExpression ToExpression(object obj)
        {
            if (obj == null)
                return null;

            if (obj is IExpression)
                return (IExpression)obj;

            if (obj is object[])
                return new VectorExpression((object[])obj);

            if (obj is IDictionary)
                return new DictionaryExpression((IDictionary)obj);

            if (obj is IList && !(obj is System.Array) && !(obj is String) && !(obj is System.ValueType))
                return new ListExpression((IList)obj);

            if (obj is Symbol)
                return new SymbolExpression((Symbol)obj);

            throw new InvalidOperationException(string.Format("Type {0} can't be converted to Expression", obj.GetType().FullName));
        }

        public static string PrintString(object obj)
        {
            StringWriter writer = new StringWriter();
            Print(obj, writer);
            return writer.ToString();
        }

        public static void Print(object obj, TextWriter writer)
        {
            writer.Write(obj.ToString());
        }

        public static ISequence ToSequence(object obj)
        {
            if (obj is BaseSequence)
                return (BaseSequence) obj;

            if (obj is LazySequence)
                return ((LazySequence)obj).ToSequence();

            return ToSequenceFrom(obj);
        }

        public static ISequence ToSequenceFrom(object obj)
        {
            if (obj is ISequenceable)
                return ((ISequenceable)obj).ToSequence();
            if (obj == null)
                return null;
            if (obj is IEnumerable)
                return EnumeratorSequence.Create(((IEnumerable)obj).GetEnumerator());

            throw new ArgumentException("Don't know how to create ISequence from: " + obj.GetType().Name);
        }

        public static bool Equiv(object obj1, object obj2)
        {
            if (obj1 == obj2)
                return true;

            if (obj1 != null)
            {
                // TODO implements obj1 is Number
                if (obj1 is IPersistentCollection)
                    return ((IPersistentCollection)obj1).Equiv(obj2);

                return obj1.Equals(obj2);
            }

            return false;
        }

        public static bool IsFalse(object obj)
        {
            if (obj == null)
                return true;

            if (obj is bool && ((bool)obj)==false)
                return true;

            return false;
        }
    }
}
