namespace AjSharpure
{
    using System;
    using System.Collections;

    public class RecursionData
    {
        private object[] arguments;

        public object[] Arguments { get { return this.arguments; } }

        public RecursionData(ICollection arguments)
        {
            if (arguments is object[]) {
                this.arguments = (object[]) arguments;
            }
            else {
                this.arguments = new object[arguments.Count];

                int k = 0;

                foreach (object argument in arguments)
                    this.arguments[k++] = argument;
            }
        }
    }
}