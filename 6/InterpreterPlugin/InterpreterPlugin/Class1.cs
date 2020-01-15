using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Plugin
{
    public class Class1
    {
        public string Name = "Translator plugin";
        private RichTextBox textBox;
        private MenuStrip _menu;
        private int _pos;
        private Form form;
        private List<Pair<string, string>> _table;
        private List<int> _tokenInLine;
        private bool _textChanged;
        private Node _beginNode;

        public void run(ref Form form, ref RichTextBox textBox)
        {
            this.textBox = textBox;
            this.form = form;
            _menu = (MenuStrip)form.Controls[1];
            ToolStripMenuItem item = new ToolStripMenuItem("Translator plugin");
            item.DropDown.Items.Add("Get lexical table");
            item.DropDown.Items.Add("Get syntax errors");
            item.DropDown.Items.Add("Get semantic errors");
            item.DropDown.Items.Add("Generate code in LWIQA");
            item.DropDown.Items.Add("Translate into LWIQA");
            item.DropDown.Items[0].Click += getLexTable;
            item.DropDown.Items[1].Click += getSyntaxErrors;
            item.DropDown.Items[2].Click += getSemErrors;
            item.DropDown.Items[3].Click += genCode;
            item.DropDown.Items[4].Click += interpretateAsLWIQA;
            _menu.Items.Add(item);
            _pos = _menu.Items.Count - 1;
            _textChanged = true;
            textBox.TextChanged += textChanged;
        }

        private void textChanged(object sender, EventArgs e)
        {
            _textChanged = true;
        }

        private void getLexTable(object sender, EventArgs e)
        {
            if (!_textChanged)
                return;
            Lexer lex = new Lexer();
            _table = lex.lex(textBox.Text);
            _tokenInLine = lex.tokenInLine;
            InterpreterPlugin.OutputTable ff = new InterpreterPlugin.OutputTable(ref form, _table, "Lexem table", new Pair<string, string>("Lexem", "Discription"));
            ff.Show();
            _textChanged = false;
        }

        private void getSyntaxErrors(object sender, EventArgs e)
        {
            if (_textChanged)
                getLexTable(sender, e);
            SyntaxTree st = new SyntaxTree(_table, _tokenInLine);
            st.parse();
            InterpreterPlugin.TreeForm treeForm = new InterpreterPlugin.TreeForm(st.beginNode, form);
            treeForm.Show();
            _beginNode = st.beginNode;
            if (st.errorsTable.Count != 0)
            {
                InterpreterPlugin.OutputTable syntErr = new InterpreterPlugin.OutputTable(ref form, st.errorsTable, "Syntax errors", new Pair<string, string>("Token", "Message"));
                syntErr.Show();
            }
        }

        private void getSemErrors(object sender, EventArgs e)
        {
            if (_textChanged)
                getSyntaxErrors(sender, e);
            Semantic sem = new Semantic(_beginNode);
            sem.analysis();
            if (sem.errorsTable.Count != 0)
            {
                InterpreterPlugin.OutputTable semErr = new InterpreterPlugin.OutputTable(ref form, sem.errorsTable, "Semantic errors", new Pair<string, string>("Construction", "Message"));
                semErr.Show();
            }
        }

        private void genCode(object sender, EventArgs e)
        {
            if (_textChanged)
                getSemErrors(sender, e);
            string LWIQACode = CodeGenerator.generateLWIQA(_beginNode);
            textBox.Text = LWIQACode;
        }

        private void interpretateAsLWIQA(object sender, EventArgs e)
        {
            if (textBox.Text == "")
                return;
            Translator inter = new Translator(textBox.Text);
            InterpreterPlugin.OutputTable outputTable = new InterpreterPlugin.OutputTable(ref form, inter.LexemTable, "Lexem table", new Pair<string, string>("Lexem", "Discription"));
            outputTable.Show();
            if (inter.LexemTable[0].val2.Contains("Error"))
            {
                return;
            }
            if (inter.SyntaxErrorTable.Count != 0)
            {
                outputTable = new InterpreterPlugin.OutputTable(ref form, inter.SyntaxErrorTable, "Syntax errors", new Pair<string, string>("Token", "Message"));
                outputTable.Show();
                return;
            }
            InterpreterPlugin.TreeForm treeForm = new InterpreterPlugin.TreeForm(inter.BeginNode, form);
            treeForm.Show();
            if (inter.SemanticErrorTable.Count != 0)
            {
                outputTable = new InterpreterPlugin.OutputTable(ref form, inter.SemanticErrorTable, "Semantic errors", new Pair<string, string>("Construction", "Message"));
                outputTable.Show();
                return;
            }
            textBox.Text = inter.OutputCode;
        }

        public void stop()
        {
            _menu.Items.RemoveAt(_pos);
        }
    }
}
