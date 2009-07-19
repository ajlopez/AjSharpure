namespace AjSharpure
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;
    using AjSharpure.Primitives;

    public class Machine
    {
        private ValueEnvironment environment = new ValueEnvironment();

        public ValueEnvironment Environment { get { return this.environment; } }

        public Machine()
        {
            this.environment.SetValue("quote", new QuotePrimitive());
            this.environment.SetValue("set!", new SetBangPrimitive());
            this.environment.SetValue("list", new ListPrimitive());
            this.environment.SetValue("def", new DefPrimitive());
            this.environment.SetValue("fn", new FnPrimitive());
            this.environment.SetValue(".", new DotPrimitive());

            this.environment.SetValue("AjSharpure.Utilities", typeof(Utilities));
        }

        public object Evaluate(object obj)
        {
            return this.Evaluate(obj, this.environment);
        }

        public object Evaluate(object obj, ValueEnvironment environment)
        {
            if (obj == null)
                return null;

            if (Utilities.IsEvaluable(obj))
                return (Utilities.ToExpression(obj)).Evaluate(this, environment);

            return obj;
        }
    }
}
