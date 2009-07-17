namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IReduce
    {
        object Reduce(IFn function);

        object Reduce(IFn function, object start);
    }
}
