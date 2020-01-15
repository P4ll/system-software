using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorV2.Utils
{
    class Token
    {
        public enum TokenType {WORD, CONST, SYMBOL, ID};
        public string Value { get; set; }
        public TokenType Type { get; set; }
        public Symbol.SymType ConstType { get; set; }
        public int StringNumber { get; set; }
        public int SpaceCount { get; set; }

        public Token(TokenType type, string val, int stNumber, int spaceCount, Symbol.SymType constType = Symbol.SymType.INT)
        {
            Value = val;
            Type = type;
            StringNumber = stNumber;
            SpaceCount = spaceCount;
            if (type == TokenType.CONST)
            {
                ConstType = constType;
            }
        }
    }
}
