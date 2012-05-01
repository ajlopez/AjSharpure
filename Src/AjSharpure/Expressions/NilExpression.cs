namespace AjSharpure.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NilExpression : ConstantExpression
    {
        private static NilExpression instance = new NilExpression();

        private NilExpression()
            : base(null)
        {
        }

        public static NilExpression Instance { get { return instance; } }
    }
}
