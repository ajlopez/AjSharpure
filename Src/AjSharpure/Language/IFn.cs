namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    // TODO IFn in Java implements Callable and Runnable
    public interface IFn
    {
        object Invoke(params object[] arguments);
    }
}
