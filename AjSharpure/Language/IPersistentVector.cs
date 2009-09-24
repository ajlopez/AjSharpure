namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPersistentVector : IAssociative, ISequential, IPersistentStack, IReversible, ICounted
    {
        int Length { get; }

        IPersistentVector AssociateN(int index, object value);

        IPersistentVector Cons(object obj);
    }
}
