namespace AjSharpure.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    class DeferredSequenceFunction : IFn
    {
        private ISequence sequence;

        internal DeferredSequenceFunction(ISequence sequence)
        {
            this.sequence = sequence;
        }

        public object Invoke(params object[] parameters)
        {
            return this.sequence;
        }
    }
}
