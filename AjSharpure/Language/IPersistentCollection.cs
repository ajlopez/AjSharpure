namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPersistentCollection : ISequenceable, ICounted
    {
        IPersistentCollection Cons(object obj);

        IPersistentCollection Empty { get; }

        bool Equiv(object obj);
    }
}
