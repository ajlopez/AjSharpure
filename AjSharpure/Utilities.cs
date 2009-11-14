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

        public static int CombineHash(IEnumerable elements)
        {
            int value = 0;

            if (elements != null)
                foreach (object element in elements)
                    value = CombineHash(value, Hash(element));

            return value;
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

            if (obj is IPersistentVector)
                return true;

            if (obj is IPersistentMap)
                return true;

            if (obj is Variable)
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

            if (obj is IPersistentVector)
                return new VectorExpression((IPersistentVector)obj);

            if (obj is IPersistentMap)
                return new MapExpression((IPersistentMap)obj);

            if (obj is IDictionary)
                return new DictionaryExpression((IDictionary)obj);

            if (obj is Variable)
                return new VariableExpression((Variable)obj);

            if (obj is IList && !(obj is System.Array) && !(obj is String) && !(obj is System.ValueType))
                return new ListExpression((IList)obj);

            if (obj is Symbol)
                return new SymbolExpression((Symbol)obj);

            if (obj is IFunction)
                return new ConstantExpression(obj);

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
            if (obj == null)
            {
                writer.Write("nil");
                return;
            }

            if (obj is String)
            {
                writer.Write('"');
                writer.Write((string)obj);
                writer.Write('"');
                return;
            }

            if (obj is Symbol)
            {
                writer.Write(((Symbol)obj).FullName);
                return;
            }

            if (obj is System.Array)
            {
                writer.Write("[");
                System.Array array = (System.Array)obj;

                for (int k = 0; k < array.Length; k++)
                {
                    if (k > 0)
                        writer.Write(" ");

                    Print(array.GetValue(k), writer);
                }

                writer.Write("]");

                return;
            }

            if (obj is IEnumerable)
            {
                writer.Write("(");
                int count = 0;

                foreach (object element in (IEnumerable)obj)
                {
                    if (count > 0)
                        writer.Write(" ");

                    Print(element, writer);

                    count++;
                }

                writer.Write(")");

                return;
            }

            writer.Write(obj.ToString());
        }

        public static IObject ToObject(object obj)
        {
            if (obj is IObject)
                return (IObject)obj;

            if (obj is IList)
                return (IObject)ListObject.Create((IList)obj);

            if (obj is IDictionary)
                return new DictionaryObject((IDictionary)obj);

            // TODO implements IWrapper
            throw new NotImplementedException();
        }

        public static ISequence ToSequence(object obj)
        {
            if (obj is BaseSequence)
                return (BaseSequence)obj;

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

        public static bool Equals(object obj1, object obj2)
        {
            if (obj1 == obj2)
                return true;

            if (obj1 != null)
            {
                if (obj1.Equals(obj2))
                    return true;

                if (obj1 is IEnumerable && obj2 is IEnumerable)
                {
                    IEnumerator enum1 = ((IEnumerable)obj1).GetEnumerator();
                    IEnumerator enum2 = ((IEnumerable)obj2).GetEnumerator();

                    while (enum1.MoveNext())
                    {
                        if (!enum2.MoveNext())
                            return false;
                        if (!Equals(enum1.Current, enum2.Current))
                            return false;
                    }

                    return !enum2.MoveNext();
                }

                // TODO implements obj1 is Number
                if (obj1 is IPersistentCollection)
                    return ((IPersistentCollection)obj1).Equiv(obj2);

                return obj1.Equals(obj2);
            }

            return false;
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

            if (obj is bool && ((bool)obj) == false)
                return true;

            return false;
        }

        public static string[] EvaluateBindings(Machine machine, ValueEnvironment newenv, ICollection bindings)
        {
            if ((bindings.Count % 2) != 0)
                throw new InvalidOperationException("Let should receive a collection as first argument with even length");

            int k = 0;
            string name = null;
            string[] names = new string[bindings.Count / 2];

            foreach (object obj in bindings)
            {
                if ((k % 2) == 0)
                {
                    // TODO review if Name or FullName
                    if (obj is INamed)
                        name = ((INamed)obj).FullName;
                    else if (obj is string)
                        name = (string)obj;
                    else
                        throw new InvalidOperationException("Let expect a symbol or a string to name a value");

                    names[k / 2] = name;
                }
                else
                    newenv.SetValue(name, machine.Evaluate(obj, newenv), true);

                k++;
            }

            return names;
        }

        public static int GetArity(object[] array)
        {
            if (array == null)
                return 0;

            return array.Length;
        }

        public static int GetArity(ICollection array)
        {
            if (array == null)
                return 0;

            return array.Count;
        }

        public static int Compare(object obj1, object obj2)
        {
            if (obj1 == null && obj2 == null)
                return 0;

            if (obj1 is IComparable)
                return ((IComparable)obj1).CompareTo(obj2);

            throw new InvalidOperationException("It can't compare object of class: " + obj1.GetType().FullName);
        }

        public static string GetFullName(string ns, string name)
        {
            return (ns == null ? name : string.Format("{0}/{1}", ns, name));
        }

        public static Type GetType(object typename)
        {
            if (typename == null)
                return null;

            if (typename is Symbol)
                return Type.GetType(((Symbol)typename).Name);

            if (typename is String)
                return Type.GetType((string)typename);

            return Type.GetType(typename.ToString());
        }

        public static Variable ToVariable(Machine machine, ValueEnvironment environment, Symbol symbol)
        {
            string ns = symbol.Namespace;

            if (String.IsNullOrEmpty(ns))
                ns = (string)environment.GetValue(Machine.CurrentNamespaceKey);

            string name = symbol.Name;

            Variable variable = machine.GetVariable(ns, name);

            if (variable == null)
                variable = Variable.Intern(machine, ns, name);

            return variable;
        }
    }
}
