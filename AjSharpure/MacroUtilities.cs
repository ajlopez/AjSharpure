namespace AjSharpure
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    public class MacroUtilities
    {
        public static object Expand(object obj, Machine machine, ValueEnvironment environment)
        {
            if (obj == null)
                return null;

            if (obj is string)
                return obj;

            if (!(obj is IList))
                return obj;

            IList list = (IList)obj;

            if (IsMacro(list))
                return machine.Evaluate(list[1], environment);

            ArrayList newlist = new ArrayList();

            foreach (object element in list)
            {
                if (element is IList)
                {
                    IList elementList = (IList)element;

                    if (IsListMacro(elementList))
                    {
                        newlist.AddRange((ICollection)machine.Evaluate(elementList[1], environment));
                        continue;
                    }
                }

                newlist.Add(Expand(element, machine, environment));
            }

            if (obj is System.Array)
                return newlist.ToArray();

            if (obj is IPersistentVector)
                return PersistentVector.Create(newlist);

            return newlist;
        }

        private static bool IsMacro(IList list)
        {
            if (list == null)
                return false;

            if (list.Count != 2)
                return false;

            if (list[0] is Symbol)
            {
                Symbol symbol = (Symbol) list[0];

                if (symbol.Name.Equals("unquote"))
                    return true;

                //if (symbol.Name.Equals("backquote"))
                //    return true;
            }

            return false;
        }

        private static bool IsListMacro(IList list)
        {
            if (list == null)
                return false;

            if (list.Count != 2)
                return false;

            if (list[0] is Symbol)
            {
                Symbol symbol = (Symbol)list[0];

                if (symbol.Name.Equals("unquote-splicing"))
                    return true;
            }

            return false;
        }
    }
}
