using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plugin;

namespace TranslatorTesting
{
    [TestClass]
    public class Test
    {
        private static string _commonPath = @"C:\Users\P4L\Dropbox\СПО\TranslatorTesting\TranslatorTesting\TestsTxt\";
        private string[] _filePath = new string[] { _commonPath + @"Lexer.txt",
                                                    _commonPath + @"Syntax.txt",
                                                    _commonPath + @"Semantic.txt",
                                                    _commonPath + @"CodeGen.txt",
                                                    _commonPath + @"CodeGenAns.txt"};
        [TestMethod]
        public void TestLexer()
        {
            FileWorker fw = new FileWorker(_filePath[0]);
            foreach (var i in fw.InputData)
            {
                Translator tr = new Translator(i);
                Assert.AreEqual(tr.LexemTable[0].val2.Contains("Error"), true);
                if (!tr.LexemTable[0].val2.Contains("Error"))
                {
                    Console.WriteLine("Error in Lexer");
                    Console.WriteLine(i);
                }
            }
        }

        [TestMethod]
        public void TestSyntax()
        {
            FileWorker fw = new FileWorker(_filePath[1]);
            foreach (var i in fw.InputData)
            {
                Translator tr = new Translator(i);
                Assert.AreEqual(tr.SyntaxErrorTable.Count != 0, true);
                if (tr.SyntaxErrorTable.Count != 0)
                {
                    Console.WriteLine("Error in Syntax");
                    Console.WriteLine(i);
                }
            }
        }

        [TestMethod]
        public void TestSemantic()
        {
            FileWorker fw = new FileWorker(_filePath[2]);
            foreach (var i in fw.InputData)
            {
                Translator tr = new Translator(i);
                Assert.AreEqual(tr.SemanticErrorTable.Count != 0, true);
                if (tr.SemanticErrorTable.Count != 0)
                {
                    Console.WriteLine("Error in Semantic");
                    Console.WriteLine(i);
                }
            }
        }

        [TestMethod]
        public void TestCodeGen()
        {
            FileWorker inputFile = new FileWorker(_filePath[3]);
            FileWorker ansFile = new FileWorker(_filePath[4]);

            for (int i = 0; i < inputFile.InputData.Count; ++i)
            {
                Translator tr = new Translator(inputFile.InputData[i]);
                Assert.AreEqual(tr.OutputCode, ansFile.InputData[i]);
                if (tr.OutputCode != ansFile.InputData[i])
                {
                    Console.WriteLine("Error in CodeGen");
                    Console.WriteLine(i);
                }
            }
        }
    }
}
