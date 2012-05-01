namespace AjSharpure.Language
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;

    public interface IAssociative : IPersistentCollection
    {
        bool ContainsKey(object key);

        DictionaryEntry EntryAt(object key);

        IAssociative Associate(object key, object value);

        object ValueAt(object key);

        object ValueAt(object key, object notFound);
    }
}
