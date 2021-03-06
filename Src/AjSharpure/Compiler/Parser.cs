﻿namespace AjSharpure.Compiler
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
        private static Symbol unquoteSymbol = Symbol.Create("unquote");
        private static Symbol unquoteSplicingSymbol = Symbol.Create("unquote-splicing");
        private static Symbol metaSymbol = Symbol.Create("meta");
        private static Symbol derefSymbol = Symbol.Create("deref");
        private static Symbol varSymbol = Symbol.Create("var");
        private static Keyword tagKeyword = Keyword.Create("tag");
        private static Symbol vectorSymbol = Symbol.Create("vector");

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

        public object ParseForm()
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                return null;

            if (token.TokenType == TokenType.Macro)
            {
                while (token != null && token.Value == ";")
                {
                    this.lexer.SkipUpToEndOfLine();
                    token = this.lexer.NextToken();
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

                    return Utilities.ToObject(form).WithMetadata((IPersistentMap)metadata);
                }

                if (token.TokenType == TokenType.Macro && token.Value == "`")
                {
                    IList list = new ArrayList();

                    list.Add(backquoteSymbol);
                    list.Add(this.ParseForm());

                    return list;
                }

                if (token.TokenType == TokenType.Macro && token.Value == "~")
                {
                    IList list = new ArrayList();

                    list.Add(unquoteSymbol);
                    list.Add(this.ParseForm());

                    return list;
                }

                if (token.TokenType == TokenType.Macro && token.Value == "~@")
                {
                    IList list = new ArrayList();

                    list.Add(unquoteSplicingSymbol);
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

            if (token.TokenType == TokenType.Character)
                return token.Value[0];

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

            Token token = this.lexer.NextToken();

            while (token != null && !(token.TokenType == TokenType.Separator && token.Value == ")"))
            {
                this.lexer.PushToken(token);
                list.Add(this.ParseForm());
                token = this.lexer.NextToken();
            }

            if (token != null && !(token.TokenType == TokenType.Separator && token.Value == ")"))
                this.lexer.PushToken(token);

            return list;
        }

        private IDictionary ParseFormMap()
        {
            IDictionary dictionary = new Hashtable();

            Token token = this.lexer.NextToken();

            while (token != null && !(token.TokenType == TokenType.Separator && token.Value == "}"))
            {
                this.lexer.PushToken(token);
                object key = this.ParseForm();
                object value = this.ParseForm();
                dictionary[key] = value;
                token = this.lexer.NextToken();
            }

            if (token != null && !(token.TokenType == TokenType.Separator && token.Value == "}"))
                this.lexer.PushToken(token);

            return new DictionaryObject(dictionary);
        }

        private object ParseFormArray()
        {
            ArrayList list = new ArrayList();

            Token token = this.lexer.NextToken();

            while (token != null && !(token.TokenType == TokenType.Separator && token.Value == "]"))
            {
                this.lexer.PushToken(token);
                list.Add(this.ParseForm());
                token = this.lexer.NextToken();
            }

            if (token != null && !(token.TokenType == TokenType.Separator && token.Value == "]"))
                this.lexer.PushToken(token);

            return PersistentVector.Create(list);
        }
    }
}
