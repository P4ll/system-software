using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterpreterPlugin
{
    public partial class OutputTable : Form
    {
        private Form pForm;
        private string _windowName;
        private Plugin.Pair<string, string> _disc;
        private List<Plugin.Pair<string, string>> _table;

        public OutputTable(ref Form pForm, List<Plugin.Pair<string, string>> table, string windowName, Plugin.Pair<string, string> discRowTable)
        {
            InitializeComponent();
            this.pForm = pForm;
            pForm.FormClosing += clForm;
            this.Text = windowName;
            _windowName = windowName;
            _disc = discRowTable;
            _table = table;
        }

        private void clForm(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OutputTable_Load(object sender, EventArgs e)
        {
            Column1.HeaderText = _disc.val1;
            Column2.HeaderText = _disc.val2;
            for (int i = 0; i < _table.Count; ++i)
            {
                dataGridView1.Rows.Add(new string[] {_table[i].val1, _table[i].val2});
            }
        }
    }
}
