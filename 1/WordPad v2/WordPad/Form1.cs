using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace WordPad
{
    public partial class Form1 : Form
    {
        private const string DIR_PATH = "\\..\\..\\Files\\";
        private const string STAND_FILE_NAME = "Untitled";
        private string _currentFileName = STAND_FILE_NAME + ".txt";
        private bool _textChanged = false;
        private string[] _pluginsFileName;
        private string[] _pluginNames;
        private Assembly[] asms;
        private Type[] types;
        private bool _cans = false;
        private bool[] _isPluginStart;
        private object[] objs;


        public Form1()
        {
            InitializeComponent();
            _pluginsFileName = Directory.GetFiles("Plugins");
            for (int i = 0; i < _pluginsFileName.Length; ++i)
            {
                _pluginsFileName[i] = Path.GetFullPath(_pluginsFileName[i]);
            }
            int n = _pluginsFileName.Length;
            _pluginNames = new string[n];
            _isPluginStart = new bool[n];
            asms = new Assembly[n];
            types = new Type[n];
            objs = new object[n];
            for (int i = 0; i < n; ++i)
            {
                asms[i] = Assembly.LoadFile(_pluginsFileName[i]);
                types[i] = asms[i].GetType("Plugin.Class1", true, true);
                objs[i] = Activator.CreateInstance(types[i]);
                _pluginNames[i] = types[i].GetField("Name").GetValue(objs[i]) as string;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.Text = "";
            _currentFileName = getNewFileName();
            setFrameName(_currentFileName);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDialog.FileName = "";
            openDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(openDialog.FileName, Encoding.Default))
                {
                    textBox.Text = sr.ReadToEnd();
                    _currentFileName = openDialog.FileName;
                    setFrameName(_currentFileName);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(_currentFileName))
            {
                sw.Write(textBox.Text);
            }
            _textChanged = false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveAsDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(_currentFileName = saveAsDialog.FileName))
                {
                    sw.Write(textBox.Text);
                    setFrameName(_currentFileName);
                }
            }
            else
            {
                _cans = true;
                return;
            }
            _textChanged = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_textChanged)
            {
                //saveAsToolStripMenuItem_Click(sender, e);
                Form1.ActiveForm.Close();
            }
            else
            {
                Form1.ActiveForm.Close();
            }
        }

        private string getNewFileName()
        {
            int i = 0;
            while (File.Exists(STAND_FILE_NAME + i + ".txt"))
            {
                i++;
            }
            return STAND_FILE_NAME + i + ".txt";
        }

        private void setFrameName(string str)
        {
            Form1.ActiveForm.Text = str;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            setFrameName(_currentFileName = getNewFileName());
            foreach (var i in _pluginNames)
                pluginsToolStripMenuItem.DropDownItems.Add(i);
            for (int i = 0; i < _pluginNames.Length; ++i)
                pluginsToolStripMenuItem.DropDownItems[i].Click += invo;
        }

        private void invo(object sender, EventArgs e)
        {
            var sen = sender as ToolStripMenuItem;
            int index = (sen.OwnerItem as ToolStripMenuItem).DropDownItems.IndexOf(sen);
            if (_isPluginStart[index])
            {
                sen.Checked = false;
                stopPlugin(index);
                _isPluginStart[index] = false;
            }
            else
            {
                sen.Checked = true;
                startPlugin(index);
                _isPluginStart[index] = true;
            }
        }

        private void startPlugin(int index)
        {
            MethodInfo method = types[index].GetMethod("run");
            object result = method.Invoke(objs[index], new object[] { Form1.ActiveForm, textBox });
        }

        private void stopPlugin(int index)
        {
            MethodInfo method = types[index].GetMethod("stop");
            object result = method.Invoke(objs[index], new object[] { });
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            _textChanged = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_textChanged)
            {
                string mess = "Сохранить изменения?";
                string capt = "Сохранение";
                MessageBoxButtons bt = MessageBoxButtons.YesNo;
                DialogResult = MessageBox.Show(mess, capt, bt);
                if (DialogResult == DialogResult.Yes)
                {
                    saveAsToolStripMenuItem_Click(sender, e);
                }
                if (_cans)
                {
                    e.Cancel = true;
                    _cans = false;
                }
            }
        }
    }
}