namespace AjSharpure.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class FnFunction : IFn, IFunction
    {
        private IFunction function;
        private Machine machine;
        private ValueEnvironment environment;

        public FnFunction(IFunction function, Machine machine, ValueEnvironment environment)
        {
            this.function = function;
            this.machine = machine;
            this.environment = environment;
        }

        public object Invoke(params object[] arguments)
        {
            return this.function.Apply(this.machine, this.environment, arguments);
        }

        public object Apply(Machine machine, ValueEnvironment environment, object[] arguments)
        {
            return this.Invoke(arguments);
        }

        public bool IsSpecialForm
        {
            get { return this.function.IsSpecialForm; }
        }
    }
}
