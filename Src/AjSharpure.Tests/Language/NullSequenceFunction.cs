namespace AjSharpure.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    class NullSequenceFunction : IFn
    {
        internal NullSequenceFunction()
        {
        }

        public object Invoke(params object[] parameters)
        {
            return null;
        }
    }
}
