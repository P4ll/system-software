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

namespace Plugin
{
    public class Class1
    {
        public string Name = "Data reciver";
        private RichTextBox textBox;
        private MenuStrip _menu;
        private int _pos;
        Form form;

        private const string IP_ADR = "127.0.0.1";
        private const int PORT = 8005;

        public void run(ref Form form, ref RichTextBox textBox)
        {
            this.textBox = textBox;
            this.form = form;
            _menu = (MenuStrip)form.Controls[1];
            ToolStripMenuItem item = new ToolStripMenuItem(Name);
            item.DropDown.Items.Add("Add info");
            item.DropDown.Items[0].Click += getData;
            _menu.Items.Add(item);
            _pos = _menu.Items.Count - 1;
        }

        private void getData(object sender, EventArgs e)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(IP_ADR), PORT);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                Byte[] buff = new Byte[256];

                int countBytes = socket.Receive(buff, buff.Length, 0);
                textBox.AppendText(" " + Encoding.Unicode.GetString(buff, 0, countBytes));

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void stop()
        {
            _menu.Items.RemoveAt(_pos);
        }
    }
}
