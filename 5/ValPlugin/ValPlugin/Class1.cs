using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Plugin
{
    public class Class1
    {
        public string Name = "Validator";
        private RichTextBox textBox;
        private MenuStrip _menu;
        private int _pos;
        Form form;

        private RegExpForm regForm;

        public void run(ref Form form, ref RichTextBox textBox)
        {
            this.textBox = textBox;
            this.form = form;
            regForm = new RegExpForm(ref form, ref textBox);
            regForm.Show();
        }

        private void getData(object sender, EventArgs e)
        {
        }

        public void stop()
        {
            regForm.Dispose();
        }
    }
}
