using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace Plugin
{
    public class Class1
    {
        public string Name = "Word plugin";
        private RichTextBox textBox;
        private MenuStrip _menu;
        private int _pos;
        Form form;
        
        private Object _missingObj = Missing.Value;
        private Object _trueObj = true;
        private Object _falseObj = false;
        private Word._Application _application;
        private Word._Document _document;
        private Word.Range _currentRange = null;
        

        public void run(ref Form form, ref RichTextBox textBox)
        {
            this.textBox = textBox;
            this.form = form;
            _menu = (MenuStrip)form.Controls[1];
            ToolStripMenuItem item = new ToolStripMenuItem("Word plugin");
            item.DropDown.Items.Add("Save as .doc");
            item.DropDown.Items.Add("Save as .docx");
            item.DropDown.Items[0].Click += saveAsWordDoc;
            item.DropDown.Items[1].Click += saveAsWordDocX;
            _menu.Items.Add(item);
            _pos = _menu.Items.Count - 1;
        }

        private void saveAsWordDoc(object sender, EventArgs e)
        {
            saveDoc(".doc", "doc", "Microsoft Word 2003", Word.WdSaveFormat.wdFormatDocument);
        }

        private void saveAsWordDocX(object sender, EventArgs e)
        {
            saveDoc(".docx", "docx", "Microsoft Word", Word.WdSaveFormat.wdFormatDocumentDefault);
        }

        private void saveDoc(string ext, string extName, string fullExtName, Word.WdSaveFormat saveFormat)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = fullExtName + "|*" + ext;
            sfd.Title = "Save an " + ext + " file";
            sfd.AddExtension = true;
            sfd.DefaultExt = extName;

            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            Object templatePathObj = sfd.FileName;
            Object fileForm = ext;
            _application = new Word.Application();
            try
            {
                _document = _application.Documents.Add(ref _missingObj, ref _missingObj, ref _missingObj, ref _missingObj);
            }
            catch (Exception error)
            {
                this.closeDoc();
                throw error;
            }
            object start = 0;
            object end = 0;
            _currentRange = _document.Range(ref start, ref end);
            _currentRange.Text = textBox.Text;
            _application.Visible = false;

            _document.SaveAs(ref templatePathObj, saveFormat, ref _missingObj, ref _missingObj, ref _missingObj, ref _missingObj, ref _missingObj, ref _missingObj, ref _missingObj, ref _missingObj, ref _missingObj, ref _missingObj, ref _missingObj, ref _missingObj, ref _missingObj, ref _missingObj);
            closeDoc();
        }
        
        private void closeDoc()
        {
            if (_document != null)
            {
                _document.Close(ref _falseObj, ref _missingObj, ref _missingObj);
            }
            _application.Quit(ref _missingObj, ref _missingObj, ref _missingObj);
            _document = null;
            _application = null;
        }
        
        public void stop()
        {
            _menu.Items.RemoveAt(_pos);
        }
    }
}
