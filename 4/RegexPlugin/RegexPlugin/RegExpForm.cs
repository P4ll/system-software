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

namespace RegexPlugin
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

        private void button_Click(object sender, EventArgs e)
        {
            Regex ex = new Regex(richTextBox.Text);
            var o = textBox.Text.Split('\n');
            textBox.Text = "";
            foreach (var i in o)
            {
                if (ex.IsMatch(i))
                    textBox.Text += i + "\n";
            }
        }

        private void RegExpForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
