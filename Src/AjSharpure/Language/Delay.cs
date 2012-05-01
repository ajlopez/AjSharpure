namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Delay : IDereference
    {
        private object value;
        private IFn function;

        public Delay(IFn function)
        {
            this.function = function;
        }

        public object Dereference()
        {
            if (this.function != null)
            {
                lock (this)
                {
                    if (this.function != null)
                    {
                        this.value = this.function.Invoke();
                        this.function = null;
                    }
                }
            }

            return this.value;
        }

        public static object Force(object obj)
        {
            return (obj is IDereference) ? ((IDereference)obj).Dereference() : obj;
        }
    }
}
