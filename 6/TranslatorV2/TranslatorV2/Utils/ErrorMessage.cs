using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorV2.Utils
{
    class ErrorMessage
    {
        public string Text { get; set; }
        public int NumberOfString { get; set; }

        public ErrorMessage(string text, int lineNumber = 0)
        {
            Text = text;
            NumberOfString = lineNumber;
        }
    }
}
