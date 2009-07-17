namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPersistentMap : IEnumerable, IAssociative, ICounted 
    {
        new IPersistentMap Associate(object key, object value);

        IPersistentMap AssociateWithException(object key, object value);

        IPersistentMap Without(object key);
    }
}
