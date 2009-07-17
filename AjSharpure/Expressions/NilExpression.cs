namespace AjSharpure.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NilExpression : ConstantExpression
    {
        private static NilExpression instance = new NilExpression();

        public static NilExpression Instance { get { return instance; } }

        private NilExpression()
            : base(null)
        {
        }
    }
}
