namespace AjSharpure.Compiler
{
    using System;

    internal class EndOfInputException : Exception
    {
        public EndOfInputException()
            : base("End of Input")
        {
        }
    }
}
