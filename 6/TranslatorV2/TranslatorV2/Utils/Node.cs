using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorV2.Utils
{
    class Node
    {
        public enum NodeType { FOR, SET, VAR, BEGIN, END, CONST, EXPR, ADD, MULT, DIV, SUB, RANGE };
        public NodeType Type { get; set; }
        public string Value { get; set; }
        public Node ParentNode { get; set; }
        public List<Node> Childs { get; set; }

        public Node(NodeType type, string value = null, List<Node> childs = null, Node parentNode = null)
        {
            Type = type;
            if (childs == null)
                Childs = new List<Node>();
            else
                Childs = childs;
            if (value != null)
                Value = value;
            if (parentNode != null)
                ParentNode = parentNode;
        }
    }
}
