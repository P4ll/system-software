using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TranslatorTesting
{
    class FileWorker
    {
        public List<string> InputData { get; private set; }

        public FileWorker(string filePath)
        {
            InputData = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                string sourceCode = "";
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    if (line[0] == '/')
                    {
                        InputData.Add(sourceCode);
                        sourceCode = "";
                    }
                    else
                    {
                        line += '\n';
                        sourceCode += line;
                    }
                }
                if (sourceCode != "")
                {
                    InputData.Add(sourceCode);
                    sourceCode = "";
                }
            }
        }
    }
}
