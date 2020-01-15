using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin
{
    public class Lexer
    {
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

        public List<int> tokenInLine;

        public Lexer()
        {
            tokenInLine = new List<int>();
        }

        public List<Pair<string, string>> lex(string inpStr)
        {
            List<Pair<string, string>> table = new List<Pair<string, string>>();
            int lineCount = 0;
            for (int i = 0; i < inpStr.Length;)
            {
                if (Char.IsLetter(inpStr[i]) || inpStr[i] == '_')
                {
                    string temp = "";
                    while (i < inpStr.Length && !symbols.ContainsKey(inpStr[i]) && inpStr[i] != ' ')
                    {
                        temp += inpStr[i];
                        ++i;
                    }
                    if (words.ContainsKey(temp))
                    {
                        table.Add(new Pair<string, string>(temp, "Key word " + words[temp].ToString()));
                    }
                    else
                    {
                        table.Add(new Pair<string, string>(temp, "ID"));
                    }
                }
                else if (Char.IsDigit(inpStr[i]))
                {
                    string temp = "";
                    while (i < inpStr.Length && !symbols.ContainsKey(inpStr[i]) && inpStr[i] != ' ')
                    {
                        temp += inpStr[i];
                        ++i;
                    }
                    table.Add(new Pair<string, string>(temp, "Constant"));
                }
                else if (symbols.ContainsKey(inpStr[i]))
                {
                    if (symbols[inpStr[i]] == Symbols.QMARK || symbols[inpStr[i]] == Symbols.QQMARK)
                    {
                        string temp = "";
                        i++;
                        while (i < inpStr.Length && symbols[inpStr[i]] != Symbols.QMARK && symbols[inpStr[i]] != Symbols.QQMARK)
                        {
                            temp += inpStr[i];
                            i++;
                        }
                        if (i >= inpStr.Length)
                        {
                            return new List<Pair<string, string>> { new Pair<string, string>(inpStr[i].ToString(), "Expected \' or \" on " + (lineCount + 1).ToString() + " line") };
                        }
                        table.Add(new Pair<string, string>(temp, "Constant"));
                        i++;
                        continue;
                    }

                    if (symbols[inpStr[i]] == Symbols.SRET)
                    {
                        ++lineCount;
                    }
                    table.Add(new Pair<string, string>(inpStr[i].ToString(), "Symbol " + symbols[inpStr[i]]));
                    ++i;
                }
                else
                {
                    return new List<Pair<string, string>> { new Pair<string, string>(inpStr[i].ToString(), "Error: unexpected symbol: " + inpStr[i] + " on " + (lineCount + 1).ToString() + " line") };
                }
                tokenInLine.Add(lineCount + 1);
            }
            table.Add(new Pair<string, string>("\0", "Symbol " + symbols['\0']));
            tokenInLine.Add(lineCount + 1);
            return table;
        }
    }
}
