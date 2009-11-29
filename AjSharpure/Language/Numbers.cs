namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.VisualBasic.CompilerServices;

    public class Numbers
    {
        public static object Add(object obj1, object obj2)
        {
            return Operators.AddObject(obj1, obj2);
        }

        public static object Subtract(object obj1, object obj2)
        {
            return Operators.SubtractObject(obj1, obj2);
        }

        public static object Multiply(object obj1, object obj2)
        {
            return Operators.MultiplyObject(obj1, obj2);
        }

        public static object Divide(object obj1, object obj2)
        {
            return Operators.DivideObject(obj1, obj2);
        }

        public static object Or(object obj1, object obj2)
        {
            return Operators.OrObject(obj1, obj2);
        }

        public static object And(object obj1, object obj2)
        {
            return Operators.AndObject(obj1, obj2);
        }

        public static object Xor(object obj1, object obj2)
        {
            return Operators.XorObject(obj1, obj2);
        }

        public static object Not(object obj)
        {
            return Operators.NotObject(obj);
        }

        public static object Increment(object obj)
        {
            return Add(obj, 1);
        }

        public static object Decrement(object obj)
        {
            return Add(obj, -1);
        }
    }
}
