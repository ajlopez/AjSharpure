namespace AjSharpure
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IFunction
    {
        object Apply(Machine machine, ValueEnvironment environment, object[] arguments);
        bool IsMacro { get; }
    }
}
