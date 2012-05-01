namespace AjSharpure.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjSharpure.Language;

    class FakeFn : IFn
    {
        public int Counter { get; set; }

        public object Invoke(params object[] arguments)
        {
            this.Counter++;

            return this.Counter;
        }
    }
}
