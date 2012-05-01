namespace AjSharpure.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure;
    using AjSharpure.Compiler;

    public class Program
    {
        public static void Main(string[] args)
        {
            Machine machine = new Machine();
            Parser parser = new Parser(System.Console.In);

            Console.WriteLine("AjSharpure 0.0.1");
            Console.WriteLine("Clojure-like interpreter written in C#");

            while (true)
            {
                object value = machine.Evaluate(parser.ParseForm());
                Console.WriteLine(Utilities.PrintString(value));
            }
        }
    }
}
