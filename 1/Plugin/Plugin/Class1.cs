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
        public string Name = "Counter";
        private Label label;
        private RichTextBox textBox;
        private int _symCount = 0;
        private KeyEventArgs _key;
        Form form;

        public void run(ref Form form, ref RichTextBox textBox)
        {
            this.textBox = textBox;
            this.form = form;
            label = new Label();
            label.Text = "Кол-во символов: " + textBox.TextLength.ToString();
            label.Location = new System.Drawing.Point(5, form.Height - 90);
            label.AutoSize = true;
            label.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            textBox.Controls.Add(label);
            textBox.TextChanged += test;
            textBox.KeyDown += keyDown;
            _symCount = textBox.TextLength;
        }
        private void keyDown(object sender, KeyEventArgs e)
        {
            _key = e;
        }
        private void test(object sender, EventArgs e)
        {
            /*
             * 
            int posCur = textBox.SelectionStart;
            if (_key.KeyCode == Keys.Back && posCur != 0)
            {
                _symCount--;
            }
            else if (_key.KeyCode == Keys.Delete && posCur != _symCount - 1)
            {
                _symCount--;
            }
            else
            {
                _symCount++;
            }
            _symCount = textBox.TextLength;
             */
            label.Text = "Кол-во символов: " + textBox.TextLength; //  + " " + posCur + " " + _key.KeyCode
        }
        public void stop()
        {
            textBox.Controls.Remove(label);
            textBox.TextChanged -= test;
        }
    }
}
