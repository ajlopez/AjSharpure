namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class BaseObject : IObject
    {
        private IPersistentMap metadata;

        public BaseObject()
        {
        }

        public BaseObject(IPersistentMap metadata)
        {
            this.metadata = metadata;
        }

        public IPersistentMap Metadata { get { return this.metadata; } }

        public abstract IObject WithMetadata(IPersistentMap metadata);
    }
}
