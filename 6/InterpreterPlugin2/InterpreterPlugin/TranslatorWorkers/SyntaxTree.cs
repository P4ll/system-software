using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin
{
    public class SyntaxTree
    {
        public enum TypeNode {VAR, CONST, ADD, SUB, MULT, DIV, SET, FOR1, FOR2, EXPR, STARTPROG, EOF, IN, RANGE, EXTRA};

        public Node beginNode;
        public List<Pair<string, string>> errorsTable;

        private bool _isEnd = false;
        private List<Pair<string, string>> _table;
        private int _pos = 0;
        private Pair<string, string> _curTok;
        private List<int> _tokenInLine;

        public SyntaxTree(List<Pair<string, string>> table, List<int> tokenInLine)
        {
            _table = table;
            prepAns();
            errorsTable = new List<Pair<string, string>>();
            _tokenInLine = tokenInLine;
        }

        public void parse()
        {
            beginNode.childs = new List<Node>();
            state(beginNode);
        }

        private void state(Node par)
        {
            if (_isEnd)
                return;
            int sCountCur = 0;
            int sCountPrev = par.childs.Count == 0 ? 0 : sCountPrev = par.childs[par.childs.Count - 1].spaceCount;

            while (equalLexSym(Lexer.Symbols.SPACE))
            {
                nextTok();
                sCountCur++;
            }

            if (sCountCur > sCountPrev)
            {
                par = par.childs[par.childs.Count - 1];
            }
            else if (sCountCur < sCountPrev)
            {
                Node newPar = par;
                while (newPar != beginNode && newPar.spaceCount != sCountCur)
                {
                    newPar = newPar.parent;
                }
                if (newPar.spaceCount != sCountCur)
                {
                    errorAns("IndentationError: unindent does not match");
                }
                else
                {
                    par = newPar.parent;
                }
            }

            if (_curTok.val2[0] == 'K')
            {
                if (Lexer.words[_curTok.val1] == Lexer.Words.FOR)
                {
                    Node node = new Node(TypeNode.FOR1, spaceCount: sCountCur, position: _tokenInLine[_pos]);
                    par.childs.Add(node);
                    node.parent = par;
                    nextTok();
                    shiftSpace();
                    if (_curTok.val2[0] == 'I')
                    {
                        Node tempSet = new Node(TypeNode.SET, parent: node, position: _tokenInLine[_pos], spaceCount: sCountCur);
                        node.childs.Add(tempSet);
                        Node tempVar = new Node(TypeNode.VAR, _curTok.val1, parent: tempSet, position: _tokenInLine[_pos], spaceCount: sCountCur);
                        tempSet.childs.Add(tempVar);
                    }
                    else
                    {
                        errorAns("Not a variable in for statement");
                    }
                    nextTok();
                    shiftSpace();
                    if (Lexer.words[_curTok.val1] != Lexer.Words.IN)
                    {
                        errorAns("No in operator in for statement");
                    }
                    nextTok();
                    shiftSpace();
                    if (Lexer.words.ContainsKey(_curTok.val1) && Lexer.words[_curTok.val1] == Lexer.Words.RANGE)
                    {
                        node.tn = TypeNode.FOR2;
                        range(node);
                    }
                    else if (_curTok.val2[0] == 'I')
                    {
                        Node temp = new Node(TypeNode.VAR, _curTok.val1, position: _tokenInLine[_pos], spaceCount: sCountCur);
                        node.childs.Add(temp);
                        temp.parent = node;
                        nextTok();
                    }
                    else
                    {
                        errorAns("Expected range() or container");
                    }
                    shiftSpace();
                    if (nEqualLexSym(Lexer.Symbols.DDOT))
                    {
                        errorAns("Expected :");
                    }
                    nextTok();
                    shiftSpace();
                    if (nEqualLexSym(Lexer.Symbols.SRET))
                    {
                        errorAns("Expected loop's body");
                    }
                    nextTok();
                    state(par); // par.spaceCount
                }
                else
                {
                    errorAns("This key word does't exist");
                }
            }
            else if (_curTok.val2[0] == 'I')
            {
                Node node = new Node(TypeNode.SET, spaceCount: sCountCur, position: _tokenInLine[_pos]);
                par.childs.Add(node);
                node.parent = par;
                Node vInS = new Node(TypeNode.VAR, _curTok.val1, parent: node, position: _tokenInLine[_pos], spaceCount: sCountCur);
                node.childs.Add(vInS);
                nextTok();
                shiftSpace();
                if (nEqualLexSym(Lexer.Symbols.EQUAL))
                {
                    errorAns("Expected =");
                }
                nextTok();
                shiftSpace();
                expression(node);

                nextTok();
                state(par); // par.spaceCount
            }
            else
            {
                errorAns("Bad statement");

                nextTok();
                state(par); // par.spaceCount
            }
        }

        private void shiftSpace()
        {
            if (_isEnd)
                return;
            while (Lexer.symbols.ContainsKey(_curTok.val1[0]) && Lexer.symbols[_curTok.val1[0]] == Lexer.Symbols.SPACE)
            {
                nextTok();
            }
        }

        private void expression(Node par)
        {
            if (_isEnd)
                return;
            int testerBr = 0;
            Node exprNode = new Node(TypeNode.EXPR, parent: par, position: _tokenInLine[_pos]);
            par.childs.Add(exprNode);
            while (nEqualLexSym(Lexer.Symbols.SRET) && nEqualLexSym(Lexer.Symbols.EOF))
            {
                if (_isEnd)
                    return;
                shiftSpace();
                if (_curTok.val2[0] == 'C')
                {
                    Node cNode = new Node(TypeNode.CONST, _curTok.val1, parent: exprNode, position: _tokenInLine[_pos]);
                    exprNode.childs.Add(cNode);
                }
                else if (_curTok.val2[0] == 'I')
                {
                    Node vNode = new Node(TypeNode.VAR, _curTok.val1, parent: exprNode, position: _tokenInLine[_pos]);
                    exprNode.childs.Add(vNode);
                }
                else if (equalLexSym(Lexer.Symbols.PLUS))
                {
                    Node oNode = new Node(TypeNode.ADD, parent: exprNode, position: _tokenInLine[_pos]);
                    exprNode.childs.Add(oNode);
                }
                else if (equalLexSym(Lexer.Symbols.MINUS))
                {
                    Node oNode = new Node(TypeNode.SUB, parent: exprNode, position: _tokenInLine[_pos]);
                    exprNode.childs.Add(oNode);
                }
                else if (equalLexSym(Lexer.Symbols.MULT))
                {
                    Node oNode = new Node(TypeNode.MULT, parent: exprNode, position: _tokenInLine[_pos]);
                    exprNode.childs.Add(oNode);
                }
                else if (equalLexSym(Lexer.Symbols.DIV))
                {
                    Node oNode = new Node(TypeNode.DIV, parent: exprNode, position: _tokenInLine[_pos]);
                    exprNode.childs.Add(oNode);
                }
                else
                {
                    Node oNode = new Node(TypeNode.EXTRA, _curTok.val1, parent: exprNode, position: _tokenInLine[_pos]);
                    exprNode.childs.Add(oNode);
                    if (equalLexSym(Lexer.Symbols.LBRS))
                        testerBr++;
                    else if (equalLexSym(Lexer.Symbols.RBRS))
                        testerBr--;
                    else
                        errorAns("unexpected symbol in expression");
                }
                nextTok();
            }
            if (testerBr != 0)
            {
                errorAns("Expected ( or )");
            }
        }

        private void range(Node par)
        {
            if (_isEnd)
                return;
            Node rangeNode = new Node(TypeNode.RANGE, position: _tokenInLine[_pos]);
            rangeNode.parent = par;
            par.childs.Add(rangeNode);
            nextTok();
            shiftSpace();
            if (nEqualLexSym(Lexer.Symbols.LBRS))
            {
                errorAns("Expected (");
            }
            for (int i = 0; i < 3; ++i)
            {
                nextTok();
                shiftSpace();
                if (_curTok.val2[0] == 'C')
                {
                    Node temp = new Node(TypeNode.CONST, _curTok.val1, position: _tokenInLine[_pos]);
                    rangeNode.childs.Add(temp);
                    temp.parent = rangeNode;
                }
                else if (_curTok.val2[0] == 'I')
                {
                    Node temp = new Node(TypeNode.VAR, _curTok.val1, position: _tokenInLine[_pos]);
                    rangeNode.childs.Add(temp);
                    temp.parent = rangeNode;
                }
                else
                {
                    errorAns("Unexpected value in range()");
                }
                nextTok();
                shiftSpace();
                if (nEqualLexSym(Lexer.Symbols.COMMA))
                {
                    if (nEqualLexSym(Lexer.Symbols.RBRS))
                    {
                        errorAns("Expected )");
                    }
                    nextTok();
                    return;
                }
            }
        }

        private bool equalLexSym(Lexer.Symbols s)
        {
            if (!Lexer.symbols.ContainsKey(_curTok.val1[0]))
                return false;
            if (Lexer.symbols[_curTok.val1[0]] == s)
                return true;
            else
                return false;
        }

        private bool nEqualLexSym(Lexer.Symbols s)
        {
            if (!Lexer.symbols.ContainsKey(_curTok.val1[0]))
                return true;
            if (Lexer.symbols[_curTok.val1[0]] != s)
                return true;
            else
                return false;
        }

        private void nextTok()
        {
            if (_isEnd)
                return;
            _pos++;
            _curTok = _table[_pos];
            if (!Lexer.symbols.ContainsKey(_curTok.val1[0]))
                return;
            if (Lexer.symbols[_curTok.val1[0]] == Lexer.Symbols.EOF)
                _isEnd = true;
        }

        private void errorAns(string mes)
        {
            errorsTable.Add(new Pair<string, string>(_curTok.val1, mes + " on " + _tokenInLine[_pos] + " line"));
            nextTok();
            _isEnd = true;
        }

        private void prepAns()
        {
            if (_table.Count == 0)
            {
                beginNode = new Node(TypeNode.EOF);
            }
            else
            {
                beginNode = new Node(TypeNode.STARTPROG);
                _curTok = _table[0];
            }

        }
    }
}