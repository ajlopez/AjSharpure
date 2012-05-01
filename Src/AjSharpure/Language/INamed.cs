namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface INamed
    {
        string Name { get; }

        string Namespace { get; }

        string FullName { get; }
    }
}
