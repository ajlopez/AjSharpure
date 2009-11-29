namespace AjSharpure.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Lexer
    {
        private const string NameCharacters = "?_-.";
        private const string MacroCharacters = ";^#'@`~";
        private static string[] MacroBicharacters = { "#'", "#_", "#^", "#(", "#{", "~@" };
        private const string SeparatorCharacters = "()[]{},";

        private TextReader reader;
        private char lastChar;
        private bool hasChar = false;
        private Stack<Token> tokenStack = new Stack<Token>();

        public Lexer(TextReader reader)
        {
            this.reader = reader;
        }

        public Lexer(string text)
            : this(new StringReader(text))
        {
        }

        public void PushToken(Token token)
        {
            if (token != null)
                tokenStack.Push(token);
        }

        public Token NextToken()
        {
            if (tokenStack.Count > 0)
                return tokenStack.Pop();

            char ch;

            try
            {
                this.SkipBlanks();
                ch = this.NextChar();

                if (MacroCharacters.IndexOf(ch) >= 0)
                    return this.NextMacro(ch);

                if (SeparatorCharacters.IndexOf(ch) >= 0)
                    return new Token() { TokenType = TokenType.Separator, Value = ch.ToString() };

                if (ch == '"')
                    return this.NextString();

                if (ch == ':')
                    return this.NextKeyword();

                if (ch == '\\')
                    return this.NextCharacter();

                if (char.IsDigit(ch))
                    return this.NextInteger(ch);

                if (ch == '-')
                {
                    char ch2 = this.NextChar();

                    if (char.IsDigit(ch2))
                    {
                        Token token = this.NextInteger(ch2);
                        token.Value = "-" + token.Value;
                        return token;
                    }

                    this.PushChar(ch2);
                }

                return this.NextSymbol(ch);
            }
            catch (EndOfInputException)
            {
                return null;
            }
        }

        internal char NextChar()
        {
            if (this.hasChar)
            {
                this.hasChar = false;
                return this.lastChar;
            }

            int ch;
            ch = this.reader.Read();

            if (ch < 0)
            {
                throw new EndOfInputException();
            }

            return (char)ch;
        }

        internal void SkipUpToEndOfLine()
        {
            try
            {
                char ch = this.NextChar();

                while (ch != '\n' && ch != '\r')
                    ch = this.NextChar();

                this.PushChar(ch);
            }
            catch (EndOfInputException)
            {
            }
        }

        private Token NextCharacter()
        {
            Token token = new Token() { TokenType = TokenType.Character, Value = this.NextChar().ToString() };

            try
            {
                char ch = this.NextChar();

                if (!char.IsWhiteSpace(ch) && SeparatorCharacters.IndexOf(ch)==-1)
                    throw new LexerException("Invalid character");

                this.PushChar(ch);
            }
            catch (EndOfInputException)
            {
            }

            return token;
        }

        private Token NextMacro(char firstChar)
        {
            string name = firstChar.ToString();

            char ch;

            try
            {
                ch = this.NextChar();

                if (!char.IsWhiteSpace(ch))
                {
                    string newname = name + ch.ToString();
                    if (MacroBicharacters.Contains(newname))
                        name = newname;
                    else
                        this.PushChar(ch);
                }
                else
                {
                    this.PushChar(ch);
                }
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token() { TokenType = TokenType.Macro, Value = name };

            return token;
        }

        private Token NextSymbol(char firstChar)
        {
            string name;

            name = firstChar.ToString();

            char ch;

            try
            {
                ch = this.NextChar();

                while (!char.IsWhiteSpace(ch) && !char.IsControl(ch) && SeparatorCharacters.IndexOf(ch)< 0)
                {
                    name += ch;
                    ch = this.NextChar();
                }

                this.PushChar(ch);
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token() { TokenType = TokenType.Symbol, Value = name };

            return token;
        }

        private Token NextKeyword()
        {
            string name = string.Empty;

            char ch;

            try
            {
                ch = this.NextChar();

                while (!char.IsWhiteSpace(ch) && !char.IsControl(ch) && SeparatorCharacters.IndexOf(ch) < 0 && ch != '.')
                {
                    name += ch;
                    ch = this.NextChar();
                }

                this.PushChar(ch);
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token() { TokenType = TokenType.Keyword, Value = name };

            return token;
        }

        private Token NextInteger(char firstChar)
        {
            string integer;

            integer = new string(firstChar, 1);

            char ch;

            try
            {
                ch = this.NextChar();

                while (char.IsDigit(ch))
                {
                    integer += ch;
                    ch = this.NextChar();
                }

                this.PushChar(ch);
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token() { TokenType = TokenType.Integer, Value = integer };

            return token;
        }

        private Token NextString()
        {
            string text = "";

            char ch;

            try
            {
                ch = this.NextChar();

                while (ch != '"')
                {
                    if (ch == '\\')
                        ch = this.NextChar();

                    text += ch;
                    ch = this.NextChar();
                }
            }
            catch (EndOfInputException)
            {
                throw new LexerException("Unclosed string");
            }

            Token token = new Token() { TokenType = TokenType.String, Value = text };

            return token;
        }

        private void PushChar(char ch)
        {
            this.lastChar = ch;
            this.hasChar = true;
        }

        private void SkipBlanks()
        {
            char ch;

            ch = this.NextChar();

            while (char.IsWhiteSpace(ch))
            {
                ch = this.NextChar();
            }

            this.PushChar(ch);
        }
    }
}
