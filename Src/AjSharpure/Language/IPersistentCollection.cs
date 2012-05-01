namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPersistentCollection : ISequenceable, ICounted
    {
        IPersistentCollection Empty { get; }

        IPersistentCollection Cons(object obj);

        bool Equiv(object obj);
    }
}
