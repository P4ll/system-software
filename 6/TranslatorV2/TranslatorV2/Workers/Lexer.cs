using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslatorV2.Utils;

namespace TranslatorV2.Workers
{
    class Lexer
    {
        public List<Token> TokensTable { get; private set; }
        public List<Symbol> SymbolsTable { get; private set; }
        public List<ErrorMessage> ErrorsTable { get; private set; }
        public enum Words { FOR, IN, RANGE };
        public enum Symbols { LBRS, RBRS, PLUS, MINUS, MULT, DIV, DDOT, SPACE, SRET, EQUAL, LESS, GREATER, LBRR, RBRR, COMMA, EOF, QMARK, QQMARK }
        public static Dictionary<string, Words> words = new Dictionary<string, Words>
        {
            {"for", Words.FOR},
            {"in", Words.IN},
            {"range", Words.RANGE}
        };
        public static Dictionary<char, Symbols> symbols = new Dictionary<char, Symbols>
        {
            {'(', Symbols.LBRS},
            {')', Symbols.RBRS},
            {'+', Symbols.PLUS},
            {'-', Symbols.MINUS},
            {'*', Symbols.MULT},
            {'/', Symbols.DIV},
            {':', Symbols.DDOT},
            {' ', Symbols.SPACE},
            {'\n', Symbols.SRET},
            {'=', Symbols.EQUAL},
            {'<', Symbols.LESS},
            {'>', Symbols.GREATER},
            {'[', Symbols.LBRR},
            {']', Symbols.RBRR},
            {',', Symbols.COMMA},
            {'\'', Symbols.QMARK},
            {'\"', Symbols.QQMARK},
            {'\0', Symbols.EOF}
        };

        public Lexer()
        {
            TokensTable = new List<Token>();
            SymbolsTable = new List<Symbol>();
            ErrorsTable = new List<ErrorMessage>();
        }

        public void Analysis(string inputText)
        {
            inputText = deleteReturnCorret(inputText);
            int lineCount = 1;
            int spaceCount = 0;
            for (int i = 0; i < inputText.Length;)
            {
                while (i < inputText.Length && inputText[i] == ' ' && inputText[i] == '\t')
                {
                    spaceCount++;
                    if (inputText[i] == '\t')
                        spaceCount += 3;
                    i++;
                }

                if (Char.IsLetter(inputText[i]) || inputText[i] == '_')
                {
                    string temp = "";
                    while (i < inputText.Length && !symbols.ContainsKey(inputText[i]))
                    {
                        temp += inputText[i];
                        ++i;
                    }
                    if (words.ContainsKey(temp))
                    {
                        TokensTable.Add(new Token(Token.TokenType.WORD, temp, lineCount, spaceCount));
                    }
                    else
                    {
                        SymbolsTable.Add(new Symbol(temp));
                        TokensTable.Add(new Token(Token.TokenType.ID, (SymbolsTable.Count - 1).ToString(), lineCount, spaceCount));
                    }
                }
                else if (Char.IsDigit(inputText[i]))
                {
                    string temp = "";
                    while (i < inputText.Length && !symbols.ContainsKey(inputText[i]))
                    {
                        temp += inputText[i];
                        ++i;
                    }
                    TokensTable.Add(new Token(Token.TokenType.CONST, temp, lineCount, spaceCount));
                    if (temp.Contains('.'))
                    {
                        TokensTable[TokensTable.Count - 1].ConstType = Symbol.SymType.FLOAT;
                    }
                }
                else if (symbols.ContainsKey(inputText[i]))
                {
                    if (symbols[inputText[i]] == Symbols.QMARK || symbols[inputText[i]] == Symbols.QQMARK)
                    {
                        i++;
                        string temp = "";
                        while (i < inputText.Length && symbols[inputText[i]] != Symbols.QMARK && symbols[inputText[i]] != Symbols.QQMARK)
                        {
                            temp += inputText[i];
                            i++;
                        }
                        TokensTable.Add(new Token(Token.TokenType.CONST, temp, lineCount, spaceCount, Symbol.SymType.STRING));
                        i++;
                        spaceCount = 0;
                        continue;
                    }

                    if (symbols[inputText[i]] == Symbols.SRET)
                    {
                        lineCount++;
                    }
                    TokensTable.Add(new Token(Token.TokenType.SYMBOL, inputText[i].ToString(), lineCount, spaceCount));
                    i++;
                }
                else
                {
                    ErrorsTable.Add(new ErrorMessage($"Unexpected symbol: {inputText[i]}", lineCount));
                    return;
                }
                spaceCount = 0;
            }
        }

        private string deleteReturnCorret(string str)
        {
            string temp = "";
            foreach (var i in str)
            {
                if (i != '\r')
                    temp += i;
            }
            return temp;
        }
    }
}
