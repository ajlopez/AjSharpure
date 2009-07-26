namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjSharpure.Compiler;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void ShouldParseSymbol()
        {
            Lexer lexer = new Lexer("foo");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Symbol, token.TokenType);
            Assert.AreEqual("foo", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ShouldParseSymbolWithQuestionMark()
        {
            Lexer lexer = new Lexer("foo?");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Symbol, token.TokenType);
            Assert.AreEqual("foo?", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ShouldParseSimbolWithHyphen()
        {
            Lexer lexer = new Lexer("foo-bar");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Symbol, token.TokenType);
            Assert.AreEqual("foo-bar", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ShouldParseSymbolWithDots()
        {
            Lexer lexer = new Lexer("System.foo.bar");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Symbol, token.TokenType);
            Assert.AreEqual("System.foo.bar", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ShouldParseSymbolWithSurroundingSpaces()
        {
            Lexer lexer = new Lexer("  foo  ");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Symbol, token.TokenType);
            Assert.AreEqual("foo", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ShouldParseKeyword()
        {
            Lexer lexer = new Lexer(":foo");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Keyword, token.TokenType);
            Assert.AreEqual("foo", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ShouldParseParenthesis()
        {
            Lexer lexer = new Lexer("()");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Separator, token.TokenType);
            Assert.AreEqual("(", token.Value);

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Separator, token.TokenType);
            Assert.AreEqual(")", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ShouldParseBrackets()
        {
            Lexer lexer = new Lexer("[]");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Separator, token.TokenType);
            Assert.AreEqual("[", token.Value);

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Separator, token.TokenType);
            Assert.AreEqual("]", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ShouldParseString()
        {
            Lexer lexer = new Lexer("\"foo bar\"");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.String, token.TokenType);
            Assert.AreEqual("foo bar", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ShouldParseStringWithEmbeddedDoubleQuotes()
        {
            Lexer lexer = new Lexer("\"foo \\\"bar\\\"\"");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.String, token.TokenType);
            Assert.AreEqual("foo \"bar\"", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ShouldParseMultipleLineString()
        {
            Lexer lexer = new Lexer("\"foo\r\nbar\"");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.String, token.TokenType);
            Assert.AreEqual("foo\r\nbar", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        [ExpectedException(typeof(LexerException), "Unclosed string")]
        public void ShouldRaiseIfStringIsUnclosed()
        {
            Lexer lexer = new Lexer("\"Unclosed");

            lexer.NextToken();
        }

        [TestMethod]
        public void ShouldParseInteger()
        {
            Lexer lexer = new Lexer("123");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Integer, token.TokenType);
            Assert.AreEqual("123", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ShouldPushToken()
        {
            Lexer lexer = new Lexer(string.Empty);

            Token token = new Token { Value = "foo", TokenType = TokenType.Symbol };
            
            lexer.PushToken(token);

            Token retrieved = lexer.NextToken();

            Assert.IsNotNull(retrieved);
            Assert.AreEqual("foo", retrieved.Value);
            Assert.AreEqual(TokenType.Symbol, retrieved.TokenType);
        }

        [TestMethod]
        public void ShouldParseMacroCharacters()
        {
            string chars = ";^#\\'@`~";
            Lexer lexer = new Lexer(chars);

            Token token;

            foreach (char ch in chars)
            {
                token = lexer.NextToken();
                Assert.IsNotNull(token);
                Assert.AreEqual(TokenType.Macro, token.TokenType);
                Assert.AreEqual(ch.ToString(), token.Value);
            }

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ShouldParseDispatchMacros()
        {
            string macros = "#' #( #{ #^ #_";
            Lexer lexer = new Lexer(macros);

            Token token;

            foreach (string macro in macros.Split(' '))
            {
                token = lexer.NextToken();
                Assert.IsNotNull(token);
                Assert.AreEqual(TokenType.Macro, token.TokenType);
                Assert.AreEqual(macro, token.Value);
            }

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ShouldParseSeparatorCharacters()
        {
            string chars = ",";
            Lexer lexer = new Lexer(chars);

            Token token;

            foreach (char ch in chars)
            {
                token = lexer.NextToken();
                Assert.IsNotNull(token);
                Assert.AreEqual(TokenType.Separator, token.TokenType);
                Assert.AreEqual(ch.ToString(), token.Value);
            }

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ShouldParseSpecialNames()
        {
            string names = "+ - * / > < = == >= <=";
            Lexer lexer = new Lexer(names);
            Token token;

            foreach (string name in names.Split(' '))
            {
                token = lexer.NextToken();

                Assert.IsNotNull(token);
                Assert.AreEqual(TokenType.Symbol, token.TokenType);
                Assert.AreEqual(name, token.Value);
            }

            Assert.IsNull(lexer.NextToken());
        }
    }
}
