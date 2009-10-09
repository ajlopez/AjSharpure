namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPersistentSet : IPersistentCollection, ICounted
    {
        IPersistentSet Disjoin(object obj);
        bool Contains(object obj);
        object GetObject(object obj);
    }
}
