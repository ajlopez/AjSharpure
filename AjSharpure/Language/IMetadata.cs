﻿namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IMetadata
    {
        IPersistentMap Metadata { get; }
    }
}
