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
    }
}
