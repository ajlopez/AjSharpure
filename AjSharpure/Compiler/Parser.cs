namespace AjSharpure.Compiler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using AjSharpure.Expressions;
    using AjSharpure.Language;

    public class Parser
    {
        private static Symbol quoteSymbol = Symbol.Create("quote");
        private static Symbol backquoteSymbol = Symbol.Create("backquote");
        private static Symbol metaSymbol = Symbol.Create("meta");
        private static Symbol derefSymbol = Symbol.Create("deref");
        private static Symbol varSymbol = Symbol.Create("var");
        private static Keyword tagKeyword = Keyword.Create("tag");

        private Lexer lexer;

        public Parser(string text)
            : this(new Lexer(text))
        {
        }

        public Parser(TextReader reader)
            : this(new Lexer(reader))
        {
        }

        public Parser(Lexer lexer) 
        {
            this.lexer = lexer;
        }

        //public IExpression ParseExpression()
        //{
        //    object obj = this.ParseForm();

        //    return Utilities.ToExpression(obj);
        //}

        public object ParseForm()
        {
            Token token = lexer.NextToken();

            if (token == null)
                return null;

            if (token.TokenType == TokenType.Macro)
            {
                while (token != null && token.Value == ";")
                {
                    lexer.SkipUpToEndOfLine();
                    token = lexer.NextToken();
                }

                if (token == null)
                    return null;

                if (token.TokenType == TokenType.Macro && token.Value == "'")
                {
                    IList list = new ArrayList();

                    list.Add(quoteSymbol);
                    list.Add(this.ParseForm());

                    return list;
                }

                if (token.TokenType == TokenType.Macro && token.Value == "^")
                {
                    IList list = new ArrayList();

                    list.Add(metaSymbol);
                    list.Add(this.ParseForm());

                    return list;
                }

                if (token.TokenType == TokenType.Macro && token.Value == "@")
                {
                    IList list = new ArrayList();

                    list.Add(derefSymbol);
                    list.Add(this.ParseForm());

                    return list;
                }

                if (token.TokenType == TokenType.Macro && token.Value == "#'")
                {
                    IList list = new ArrayList();

                    list.Add(varSymbol);
                    list.Add(this.ParseForm());

                    return list;
                }

                if (token.TokenType == TokenType.Macro && token.Value == "#^")
                {
                    IDictionary metadata = null;
                    
                    token = this.lexer.NextToken();

                    if (token != null && token.Value == "{")
                        metadata = this.ParseFormMap();
                    else
                    {
                        this.lexer.PushToken(token);
                        object tag = this.ParseForm();
                        IDictionary dict = new Hashtable();
                        dict[tagKeyword] = tag;
                        metadata = new DictionaryObject(dict);
                    }

                    object form = this.ParseForm();

                    return Utilities.ToObject(form).WithMetadata((IPersistentMap) metadata);
                }

                if (token.TokenType == TokenType.Macro && token.Value == "`")
                {
                    IList list = new ArrayList();

                    list.Add(backquoteSymbol);
                    list.Add(this.ParseForm());

                    return list;
                }

                if (token.TokenType == TokenType.Macro)
                    throw new ParserException(string.Format("Unknown macro {0}", token.Value));
            }

            if (token.TokenType == TokenType.Symbol)
            {
                if (token.Value == "true")
                    return true;

                if (token.Value == "false")
                    return false;

                return Symbol.Create(token.Value);
            }

            if (token.TokenType == TokenType.Keyword)
                return Keyword.Create(token.Value);

            if (token.TokenType == TokenType.String)
                return token.Value;

            if (token.TokenType == TokenType.Integer)
                return Int32.Parse(token.Value);

            if (token.TokenType == TokenType.Separator && token.Value == "(")
                return this.ParseFormList();

            if (token.TokenType == TokenType.Separator && token.Value == "[")
                return this.ParseFormArray();

            if (token.TokenType == TokenType.Separator && token.Value == "{")
                return this.ParseFormMap();

            throw new ParserException(string.Format("Unexpected token: {0}", token.Value));
        }

        private IList ParseFormList()
        {
            IList list = new ArrayList();

            Token token = lexer.NextToken();

            while (token != null && !(token.TokenType == TokenType.Separator && token.Value == ")"))
            {
                lexer.PushToken(token);
                list.Add(this.ParseForm());
                token = lexer.NextToken();
            }

            if (token != null && !(token.TokenType == TokenType.Separator && token.Value == ")"))
                lexer.PushToken(token);

            return list;
        }

        private IDictionary ParseFormMap()
        {
            IDictionary dictionary = new Hashtable();

            Token token = lexer.NextToken();

            while (token != null && !(token.TokenType == TokenType.Separator && token.Value == "}"))
            {
                lexer.PushToken(token);
                object key = this.ParseForm();
                object value = this.ParseForm();
                dictionary[key] = value;
                token = lexer.NextToken();
            }

            if (token != null && !(token.TokenType == TokenType.Separator && token.Value == "}"))
                lexer.PushToken(token);

            return new DictionaryObject(dictionary);
        }

        private object[] ParseFormArray()
        {
            IList list = new ArrayList();

            Token token = lexer.NextToken();

            while (token != null && !(token.TokenType == TokenType.Separator && token.Value == "]"))
            {
                lexer.PushToken(token);
                list.Add(this.ParseForm());
                token = lexer.NextToken();
            }

            if (token != null && !(token.TokenType == TokenType.Separator && token.Value == "]"))
                lexer.PushToken(token);

            object[] values = new object[list.Count];

            for (int k = 0; k < list.Count; k++)
                values[k] = list[k];

            return values;
        }
    }
}
