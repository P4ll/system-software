using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Plugin
{
    public class Pair<T1, T2>
    {
        public T1 val1;
        public T2 val2;

        public Pair()
        {
        }

        public Pair(T1 val1, T2 val2)
        {
            this.val1 = val1;
            this.val2 = val2;
        }
    }
    public class Lexer
    {
        enum Words {FOR, IN};
        enum Symbols {LBRS, RBRS, PLUS, MINUS, MULT, DIV, DDOT}
        Dictionary<string, Words> words = new Dictionary<string, Words>
        {
            {"for", Words.FOR},
            {"in", Words.IN}
        };
        Dictionary<char, Symbols> symbols = new Dictionary<char, Symbols>
        {
            {'(', Symbols.LBRS},
            {')', Symbols.RBRS},
            {'+', Symbols.PLUS},
            {'-', Symbols.MINUS},
            {'*', Symbols.MULT},
            {'/', Symbols.DIV},
            {':', Symbols.DDOT}
        };

        public Lexer()
        {
        }

        public List<Pair<string, string>> lex(string inpStr)
        {
            List<Pair<string, string>> table = new List<Pair<string, string>>();
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
                        table.Add(new Pair<string, string>(temp, "Key word"));
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
                    table.Add(new Pair<string, string>(inpStr[i].ToString(), "Symbol"));
                    ++i;
                }
                else
                {
                    return new List<Pair<string, string>> { new Pair<string, string>(inpStr[i].ToString(), "Error: unexpected symbol") };
                }
            }
            return table;
        }
    }

    public class Class1
    {
        public string Name = "Lexer plugin";
        private RichTextBox textBox;
        Form form;

        public void run(ref Form form, ref RichTextBox textBox)
        {
            this.textBox = textBox;
            this.form = form;
        }

        private void lexicalAnalysis()
        {
            Lexer lex = new Lexer();
            List<Pair<string, string>> lexTable = lex.lex(textBox.Text);

        }

        private void test(object sender, EventArgs e)
        {
        }

        public void stop()
        {
        }
    }
}
