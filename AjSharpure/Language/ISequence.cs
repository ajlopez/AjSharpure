namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ISequence : IPersistentCollection, ISequential
    {
        object First();
        ISequence Next();
        ISequence More();
        new ISequence Cons(object obj);
    }
}
