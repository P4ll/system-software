using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslatorV2.Utils;

namespace TranslatorV2.Workers
{
    class Syntax
    {
        public Node StartNode { get; private set; }
        public List<ErrorMessage> ErrorsTable { get; private set; }

        private List<Token> _tokensTable;
        private Token _currentToken;
        private bool _isEnd = false;
        private int _posInTokenTable = 0;

        public Syntax(List<Token> tokensTable)
        {
            _tokensTable = tokensTable;
            StartNode = new Node(Node.NodeType.BEGIN);
            if (_tokensTable.Count == 0)
            {
                _isEnd = true;
                return;
            }
            _currentToken = _tokensTable[_posInTokenTable];
        }

        public void Analysis()
        {
            state(StartNode);
        }

        private void nextToken()
        {
            _posInTokenTable++;
            if (_posInTokenTable >= _tokensTable.Count)
            {
                _isEnd = true;
                return;
            }
            _currentToken = _tokensTable[_posInTokenTable];
        }

        private void state(Node par)
        {
            if (_isEnd)
                return;
            if (_currentToken.Type == Token.TokenType.WORD)
            {
                if (Lexer.words[_currentToken.Value] == Lexer.Words.FOR)
                {
                    Node forNode = new Node(Node.NodeType.FOR, parentNode: par);
                    nextToken();
                    if (_currentToken.Type != Token.TokenType.ID)
                        raiseError("Expected iterable variable");
                    forNode.Childs.Add(new Node(Node.NodeType.VAR, _currentToken.Value, parentNode: forNode));
                    nextToken();
                    if (Lexer.words[_currentToken.Value] != Lexer.Words.IN)
                        raiseError("Expected IN in FOR statement");
                    nextToken();
                    if (Lexer.words[_currentToken.Value] != Lexer.Words.RANGE)
                        raiseError("Expected RANGE in FOR statement");
                    nextToken();
                    range(forNode);
                    if (_currentToken.Type != Token.TokenType.SYMBOL || Lexer.symbols[_currentToken.Value[0]] != Lexer.Symbols.DDOT)
                        raiseError("Expected <:>");
                    nextToken();
                    if (_currentToken.Type != Token.TokenType.SYMBOL || Lexer.symbols[_currentToken.Value[0]] != Lexer.Symbols.SRET)
                        raiseError("Expected body of loop");
                    nextToken();
                    state(forNode);
                }
                else
                {
                    raiseError("Unexpected key word");
                }
            }
            else if (_currentToken.Type == Token.TokenType.ID)
            {

            }
            else
            {
                raiseError("Bad statement");
            }
        }

        private void expression(Node par)
        {
            if (_isEnd)
                return;
        }

        private void range(Node par)
        {
            if (_isEnd)
                return;
        }

        private void raiseError(string message)
        {
            ErrorsTable.Add(new ErrorMessage(message, _currentToken.StringNumber));
            _isEnd = true;
        }
    }
}
