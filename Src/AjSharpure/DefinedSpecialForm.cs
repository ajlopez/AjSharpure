namespace AjSharpure
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class DefinedSpecialForm : DefinedFunction
    {
        public DefinedSpecialForm(string name, ICollection arguments, IExpression expression)
            : base(name, arguments, expression)
        {
        }

        public override bool IsSpecialForm
        {
            get { return true; }
        }
    }
}
