namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPersistentStack : IPersistentCollection
    {
        object Peek();

        IPersistentStack Pop();
    }
}
