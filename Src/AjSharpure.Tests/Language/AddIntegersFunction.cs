namespace AjSharpure.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    class AddIntegersFunction : IFn
    {
        public object Invoke(params object[] parameters)
        {
            return ((int)parameters[0]) + ((int)parameters[1]);
        }
    }
}
