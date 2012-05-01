namespace AjSharpure.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Language;

    class NumberSequenceFunction : IFn
    {
        private int number;

        internal NumberSequenceFunction(int number)
        {
            this.number = number;
        }

        public object Invoke(params object[] parameters)
        {
            return new Cons(this.number, new LazySequence(new NumberSequenceFunction(this.number + 1)));
        }
    }
}
