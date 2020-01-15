using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin
{
    public class Semantic
    {
        public enum VarType {FLOAT, INT, STRING};
        public List<Pair<string, string>> errorsTable;

        private Node _beginNode;

        public Semantic(Node begNode)
        {
            _beginNode = begNode;
            errorsTable = new List<Pair<string, string>>();
        }

        public void analysis()
        {
            checkForBody(_beginNode);
            checkUseWithoutInit(_beginNode, new Dictionary<string, bool>());
            checkIterableVar(_beginNode, new Dictionary<string, bool>());
            if (errorsTable.Count == 0)
                checkRange(_beginNode, new Dictionary<string, bool>());
        }

        private void checkRange2(Node node, Dictionary<string, bool> used)
        {

        }

        private void checkRange(Node node, Dictionary<string, bool> used)
        {
            foreach (var z in node.childs)
            {
                if (z.tn == SyntaxTree.TypeNode.SET)
                {
                    string vv = z.childs[0].val;
                    bool canUse = true;
                    if (z.childs.Count < 2)
                        continue;
                    Node expr = z.childs[1];
                    foreach (var expZ in expr.childs)
                    {
                        if (expZ.tn != SyntaxTree.TypeNode.CONST && expZ.tn != SyntaxTree.TypeNode.VAR)
                            continue;
                        bool isConstFloat = expZ.tn == SyntaxTree.TypeNode.CONST && expZ.val.Contains('.');
                        if (isConstFloat)
                        {
                            canUse = false;
                            break;
                        }
                        else if (expZ.tn == SyntaxTree.TypeNode.CONST)
                        {
                            continue;
                        }
                        if (!used.ContainsKey(expZ.val))
                        {
                            continue;
                        }
                        bool isVarFloat = expZ.tn == SyntaxTree.TypeNode.VAR && !used[expZ.val];
                        if (isConstFloat || isVarFloat)
                        {
                            canUse = false;
                            break;
                        }
                    }
                    if (used.ContainsKey(vv))
                        used[vv] = canUse;
                    else
                        used.Add(vv, canUse);
                }
                else if (z.tn == SyntaxTree.TypeNode.RANGE)
                {
                    foreach (var rangeZ in z.childs)
                    {
                        bool isConstFloat = rangeZ.tn == SyntaxTree.TypeNode.CONST && rangeZ.val.Contains('.');
                        bool isVarFloat = rangeZ.tn == SyntaxTree.TypeNode.VAR && !used[rangeZ.val];
                        if (isConstFloat || isVarFloat)
                        {
                            errorAns(rangeZ, "using float in range()");
                        }
                    }
                }
                else
                {
                    checkRange(z, new Dictionary<string, bool>(used));
                }
            }
        }

        private void checkUseWithoutInit(Node node, Dictionary<string, bool> used)
        {
            foreach (var z in node.childs)
            {
                if (z.tn == SyntaxTree.TypeNode.SET)
                {
                    if (z.childs.Count > 1)
                    {
                        foreach (var kk in z.childs[1].childs)
                        {
                            if (kk.tn == SyntaxTree.TypeNode.VAR && !used.ContainsKey(kk.val))
                            {
                                errorAns(kk, "using variable without init");
                            }
                        }
                    }
                    if (!used.ContainsKey(z.childs[0].val))
                        used.Add(z.childs[0].val, true);
                }
                else if (z.tn == SyntaxTree.TypeNode.VAR)
                {
                    if (!used.ContainsKey(z.val))
                    {
                        errorAns(z, "using variable without init");
                    }
                }
                else
                {
                    checkUseWithoutInit(z, new Dictionary<string, bool>(used));
                }
            }
        }

        private void checkForBody(Node node)
        {
            if (node.tn == SyntaxTree.TypeNode.FOR1 || node.tn == SyntaxTree.TypeNode.FOR2)
            {
                if (node.childs.Count < 3)
                {
                    errorAns(node, "expected body of loop");
                }
            }
            foreach (var z in node.childs)
            {
                checkForBody(z);
            }
        }

        private void checkIterableVar(Node node, Dictionary<string, bool> used)
        {
            int beg = 0;
            int end = node.childs.Count;

            if (node.tn == SyntaxTree.TypeNode.FOR1 || node.tn == SyntaxTree.TypeNode.FOR2)
            {
                used.Add(node.childs[0].childs[0].val, true);
                beg = 2;
            }
            else if (node.tn == SyntaxTree.TypeNode.SET)
            {
                if (used.ContainsKey(node.childs[0].val))
                {
                    errorAns(node, "assignment the value of an iterable variable");
                }
            }

            for (int i = beg; i < end; ++i)
            {
                checkIterableVar(node.childs[i], new Dictionary<string, bool>(used));
            }
        }

        private void errorAns(Node z, string mess)
        {
            string vv = z.val == null ? "" : z.val;
            errorsTable.Add(new Pair<string, string>($"{z.tn.ToString()} {vv}", $"Error on {z.position} line: {mess}"));
        }
    }
}
