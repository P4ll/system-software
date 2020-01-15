using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin
{
    public class Node
    {
        public SyntaxTree.TypeNode tn;
        public string val;
        public List<Node> childs;
        public int spaceCount;
        public Node parent;
        public int position;

        public Node(SyntaxTree.TypeNode tn, string val = null, int spaceCount = 0, Node parent = null, int position = 0)
        {
            this.tn = tn;
            this.val = val;
            this.childs = new List<Node>();
            if (spaceCount == 0)
                this.spaceCount = parent == null ? 0 : parent.spaceCount;
            else
                this.spaceCount = spaceCount;
            this.parent = parent;
            this.position = position;
        }
    }
}
