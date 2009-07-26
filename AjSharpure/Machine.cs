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
        public const string AjSharpureCoreKey = "AjSharpure.Core";
        private const string AjSharpureCurrentNamespaceName = "*ns*";
        private static string currentNamespaceKey = Utilities.GetFullName(AjSharpureCoreKey, AjSharpureCurrentNamespaceName);

        public static string CurrentNamespaceKey { get { return currentNamespaceKey; } }

        private ValueEnvironment environment = new ValueEnvironment();

        private ValueEnvironment variableEnvironment = new ValueEnvironment();

        public ValueEnvironment Environment { get { return this.environment; } }

        public Machine()
        {
            this.environment.SetValue(CurrentNamespaceKey, AjSharpureCoreKey);
            this.environment.SetValue("true", true);
            this.environment.SetValue("false", false);
            this.environment.SetValue("nil", null);
            this.environment.SetValue("quote", new QuotePrimitive());
            this.environment.SetValue("set!", new SetBangPrimitive());
            this.environment.SetValue("list", new ListPrimitive());
            this.environment.SetValue("def", new DefPrimitive());
            this.environment.SetValue("let", new LetPrimitive());
            this.environment.SetValue("if", new IfPrimitive());
            this.environment.SetValue("do", new DoPrimitive());
            this.environment.SetValue("loop", new LoopPrimitive());
            this.environment.SetValue("recur", new RecurPrimitive());
            this.environment.SetValue("fn*", new FnStarPrimitive());
            this.environment.SetValue("new", new NewPrimitive());
            this.environment.SetValue(".", new DotPrimitive());
            this.environment.SetValue("+", new AddPrimitive());
            this.environment.SetValue("*", new MultiplyPrimitive());
            this.environment.SetValue("-", new SubtractPrimitive());
            this.environment.SetValue("/", new DividePrimitive());
            this.environment.SetValue("=", new EqualPrimitive());
            this.environment.SetValue("<", new LessPrimitive());
            this.environment.SetValue(">", new GreaterPrimitive());
            this.environment.SetValue("<=", new LessEqualPrimitive());
            this.environment.SetValue(">=", new GreaterEqualPrimitive());

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

        public void SetVariableValue(Variable variable, object value)
        {
            variableEnvironment.SetValue(variable.FullName, value, true);
        }

        public object GetVariableValue(Variable variable)
        {
            return variableEnvironment.GetValue(variable.FullName);
        }

        public object GetVariableValue(string fullName)
        {
            return variableEnvironment.GetValue(fullName);
        }
    }
}
