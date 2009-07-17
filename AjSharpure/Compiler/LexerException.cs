using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AjSharpure.Compiler
{
    public class LexerException : Exception
    {
        public LexerException(string message)
            : base(message)
        {
        }
    }
}
