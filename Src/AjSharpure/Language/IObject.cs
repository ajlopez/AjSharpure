﻿namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IObject : IMetadata
    {
        IObject WithMetadata(IPersistentMap metadata);
    }
}
