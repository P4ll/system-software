using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin
{
    class Translator
    {
        public List<Pair<string, string>> LexemTable { get; private set; }
        public List<Pair<string, string>> SyntaxErrorTable { get; private set; }
        public List<Pair<string, string>> SemanticErrorTable { get; private set; }
        public Node BeginNode { get; private set; }
        public string OutputCode { get; private set; }

        private string _sourceCode;
        private Lexer _lexer;
        private SyntaxTree _synTree;
        private Semantic _sem;

        public Translator(string listing)
        {
            _sourceCode = listing;
            interpretateAsLWIQA();
        }

        private void interpretateAsLWIQA()
        {
            if (_sourceCode == "")
                return;
            _lexer = new Lexer();
            LexemTable = _lexer.lex(_sourceCode);
            if (LexemTable[0].val2.Contains("Error"))
                return;
            _synTree = new SyntaxTree(LexemTable, _lexer.tokenInLine);
            _synTree.parse();
            SyntaxErrorTable = _synTree.errorsTable;
            if (SyntaxErrorTable.Count > 0)
                return;
            BeginNode = _synTree.beginNode;
            _sem = new Semantic(BeginNode);
            _sem.analysis();
            SemanticErrorTable = _sem.errorsTable;
            if (SemanticErrorTable.Count > 0)
                return;
            OutputCode = CodeGenerator.generateLWIQA(BeginNode);
        }

    }
}
