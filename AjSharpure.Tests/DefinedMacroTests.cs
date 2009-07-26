namespace AjSharpure.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure;
    using AjSharpure.Compiler;
    using AjSharpure.Expressions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DefinedMacroTests
    {
        [TestMethod]
        public void ShouldDefineAndEvaluateSimpleList()
        {
            Parser parser = new Parser("[x y] (list 'list (backquote x) (backquote y)) 1 2");
            object argumentNames = parser.ParseForm();
            object body = parser.ParseForm();

            DefinedMacro func = new DefinedMacro("simple-list", (object[])argumentNames, (IList) body);

            Assert.AreEqual("simple-list", func.Name);

            object[] arguments = new object[2];
            arguments[0] = parser.ParseForm();
            arguments[1] = parser.ParseForm();

            Machine machine = new Machine();
            
            object result = func.Apply(machine, machine.Environment, arguments);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));
            Assert.AreEqual(2, ((IList)result).Count);
        }
    }
}
