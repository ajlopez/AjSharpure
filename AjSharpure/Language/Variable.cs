namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    public class Variable : Symbol
    {
        public static Variable Create(string ns, string name)
        {
            return new Variable(ns, name);
        }

        public static Variable Create(string name)
        {
            int position = name.LastIndexOf('/');

            if (position == -1)
                return Create(null, name);

            return Create(name.Substring(0, position), name.Substring(position + 1));
        }

        private Variable(string ns, string name)
            : this(ns, name, null)
        {
        }

        private Variable(string ns, string name, IPersistentMap metadata)
            : base(ns,name,metadata)
        {
        }
    }
}
