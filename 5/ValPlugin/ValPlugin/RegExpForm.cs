using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Plugin
{
    public partial class RegExpForm : Form
    {
        Form form;
        RichTextBox textBox;

        public RegExpForm()
        {
            InitializeComponent();
        }

        public RegExpForm(ref Form form, ref RichTextBox textBox)
        {
            InitializeComponent();
            this.form = form;
            this.textBox = textBox;
        }

        private string GetRange(char a, char b) {
            return "[" + a + '-' + b + ']';
        }

        private void button_Click(object sender, EventArgs e)
        {

            string uText = upperTextBox.Text;
            string lText = lowerTextBox.Text;

            if (uText.Length == 1 && lText.Length == 1) {
                var z = textBox.Text.Split('\n');
                textBox.Text = "";
                Regex ex5 = new Regex("^.* " + GetRange(uText[0], lText[0]) + "$");
                foreach (var i in z)
                {
                    if (ex5.IsMatch(i))
                        textBox.Text += i + "\n";
                }
                return;
            }

            string rExp1 = "^.* ";
            for (int i = 0; i < uText.Length - 1; ++i) {
                rExp1 += uText[i];
            }
            rExp1 += GetRange(uText[uText.Length - 1], '9');
            string rExp2 = "^.* ";
            for (int i = 0; i < lText.Length - 1; ++i)
            {
                rExp2 += lText[i];
            }
            rExp2 += GetRange('0', lText[lText.Length - 1]);
            if (uText.Length == 1) {
                rExp1 = "^.* " + GetRange(uText[0], '9');
            }
            rExp1 += '$';
            rExp2 += '$';
            string str3 = "^.* ", str4 = "^.* ";
            str3 += GetRange('0', uText[0]);
            str4 += GetRange(lText[0], '9');
            for (int i = 1; i < uText.Length; ++i)
                str3 += GetRange('0', '9');
            for (int i = 1; i < lText.Length; ++i)
                str4 += GetRange('0', '9');
            str3 += '$';
            str4 += '$';
            //throw new Exception("ALERT");
            Regex ex = new Regex(rExp1);
            Regex ex2 = new Regex(rExp2);
            Regex ex3 = new Regex(str3);
            Regex ex4 = new Regex(str4);
            Regex ex6 = new Regex("^.* [0-9]$");
            var o = textBox.Text.Split('\n');
            textBox.Text = "";
            foreach (var i in o)
            {
                if (ex6.IsMatch(i) && uText.Length > 1)
                    continue;
                if (ex.IsMatch(i) || ex2.IsMatch(i) || (!ex3.IsMatch(i) && !ex4.IsMatch(i)))
                    textBox.Text += i + "\n";
            }
            
        }

        private void RegExpForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
