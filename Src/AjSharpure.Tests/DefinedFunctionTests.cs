﻿namespace AjSharpure.Tests
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
    public class DefinedFunctionTests
    {
        [TestMethod]
        public void DefineAndEvaluateSimpleList()
        {
            Parser parser = new Parser("[x y] (list x y) 1 2");
            object argumentNames = parser.ParseForm();
            object body = parser.ParseForm();

            DefinedFunction func = new DefinedFunction("simple-list", (ICollection)argumentNames, Utilities.ToExpression(body));

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
