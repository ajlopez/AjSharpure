namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPersistentVector : IAssociative, ISequential, IPersistentStack, IReversible, ICounted
    {
        int Length { get; }

        object this[int index] { get; }

        IPersistentVector AssociateN(int index, object value);

        new IPersistentVector Cons(object obj);
    }
}
