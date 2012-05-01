namespace AjSharpure.Tests.Compiler
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
        public void ParseSymbol()
        {
            Lexer lexer = new Lexer("foo");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Symbol, token.TokenType);
            Assert.AreEqual("foo", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ParseSymbolWithQuestionMark()
        {
            Lexer lexer = new Lexer("foo?");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Symbol, token.TokenType);
            Assert.AreEqual("foo?", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ParseSimbolWithHyphen()
        {
            Lexer lexer = new Lexer("foo-bar");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Symbol, token.TokenType);
            Assert.AreEqual("foo-bar", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ParseSymbolWithDots()
        {
            Lexer lexer = new Lexer("System.foo.bar");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Symbol, token.TokenType);
            Assert.AreEqual("System.foo.bar", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ParseSymbolWithSurroundingSpaces()
        {
            Lexer lexer = new Lexer("  foo  ");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Symbol, token.TokenType);
            Assert.AreEqual("foo", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ParseKeyword()
        {
            Lexer lexer = new Lexer(":foo");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Keyword, token.TokenType);
            Assert.AreEqual("foo", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ParseParenthesis()
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
        public void ParseBrackets()
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
        public void ParseString()
        {
            Lexer lexer = new Lexer("\"foo bar\"");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.String, token.TokenType);
            Assert.AreEqual("foo bar", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ParseStringWithEmbeddedDoubleQuotes()
        {
            Lexer lexer = new Lexer("\"foo \\\"bar\\\"\"");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.String, token.TokenType);
            Assert.AreEqual("foo \"bar\"", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ParseMultipleLineString()
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
        public void RaiseIfStringIsUnclosed()
        {
            Lexer lexer = new Lexer("\"Unclosed");

            lexer.NextToken();
        }

        [TestMethod]
        public void ParseInteger()
        {
            Lexer lexer = new Lexer("123");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Integer, token.TokenType);
            Assert.AreEqual("123", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ParseNegativeInteger()
        {
            Lexer lexer = new Lexer("-123");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Integer, token.TokenType);
            Assert.AreEqual("-123", token.Value);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void PushToken()
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
        public void ParseMacroCharacters()
        {
            string chars = ";^#@'`~";
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
        public void ParseDispatchMacros()
        {
            string macros = "#' #( #{ #^ #_ ~@";
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
        public void ParseSeparatorCharacters()
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
        public void ParseSpecialNames()
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

        [TestMethod]
        public void ParseACharacter()
        {
            Lexer lexer = new Lexer("\\a");
            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual("a", token.Value);
            Assert.AreEqual(TokenType.Character, token.TokenType);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ParseTwoCharacters()
        {
            Lexer lexer = new Lexer("\\a \\b");
            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual("a", token.Value);
            Assert.AreEqual(TokenType.Character, token.TokenType);

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual("b", token.Value);
            Assert.AreEqual(TokenType.Character, token.TokenType);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ParseACharacterAndSeparator()
        {
            Lexer lexer = new Lexer("\\a)");
            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual("a", token.Value);
            Assert.AreEqual(TokenType.Character, token.TokenType);

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(")", token.Value);
            Assert.AreEqual(TokenType.Separator, token.TokenType);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        [ExpectedException(typeof(LexerException),"Invalid character")]
        public void RaiseIfInvalidCharacter()
        {
            Lexer lexer = new Lexer("\\foo");
            Token token;

            token = lexer.NextToken();
        }
    }
}
