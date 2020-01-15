using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorV2.Utils
{
    class Symbol
    {
        public enum SymType {INT, FLOAT, STRING};
        public string Name { get; private set; }
        public SymType Type { get; private set; }

        public Symbol(string name, SymType type = SymType.INT)
        {
            Name = name;
            Type = type;
        }
    }
}
