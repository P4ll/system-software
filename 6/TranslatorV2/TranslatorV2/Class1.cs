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

        public void run(ref Form form, ref RichTextBox textBox)
        {
            this.textBox = textBox;
            this.form = form;
            _menu = (MenuStrip)form.Controls[1];
            ToolStripMenuItem item = new ToolStripMenuItem("Translator plugin");
            item.DropDown.Items.Add("DOIT");
            item.DropDown.Items[0].Click += foo;
            _menu.Items.Add(item);
            _pos = _menu.Items.Count - 1;
        }

        private void foo(object sender, EventArgs e)
        {

        }

        public void stop()
        {
            _menu.Items.RemoveAt(_pos);
        }
    }
}
