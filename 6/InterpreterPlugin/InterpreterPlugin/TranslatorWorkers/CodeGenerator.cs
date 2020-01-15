using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin
{
    public static class CodeGenerator
    {
        public static string generateLWIQA(Node stNode)
        {
            StringBuilder sb = new StringBuilder();
            int labelCount = 0;

            foreach (var z in stNode.childs)
            {
                addState(z, sb, labelCount);
            }

            return sb.ToString();
        }

        private static void addState(Node node, StringBuilder sb, int labelCount)
        {
            if (node.tn == SyntaxTree.TypeNode.SET)
            {
                addSpaces(sb, node.spaceCount);
                sb.Append($"&{node.childs[0].val}& :=");
                addExpr(node.childs[1], sb);
                sb.Append('\n');
            }
            else if (node.tn == SyntaxTree.TypeNode.FOR2)
            {
                addForBeg(node, sb, labelCount);
                addState(node.childs[2], sb, labelCount + 1);
                addForEnd(node, sb, labelCount);
            }
        }

        private static void addForBeg(Node forNode, StringBuilder sb, int labelCount)
        {
            string begVal = "";
            string endVal = "";
            if (forNode.childs[1].tn == SyntaxTree.TypeNode.RANGE)
            {
                Node rangeNode = forNode.childs[1];
                if (rangeNode.childs.Count == 1)
                {
                    begVal = "0";
                    if (rangeNode.childs[0].tn == SyntaxTree.TypeNode.CONST)
                        endVal = rangeNode.childs[0].val;
                    else
                        endVal = $"&{rangeNode.childs[0].val}&";
                }
                else if (rangeNode.childs.Count >= 2)
                {
                    if (rangeNode.childs[0].tn == SyntaxTree.TypeNode.CONST)
                    {
                        begVal = rangeNode.childs[0].val;
                    }
                    else
                    {
                        begVal = $"&{rangeNode.childs[0].val}&";
                    }

                    if (rangeNode.childs[1].tn == SyntaxTree.TypeNode.CONST)
                        endVal = rangeNode.childs[1].val;
                    else
                        endVal = $"&{rangeNode.childs[1].val}&";
                }
            }
            addSpaces(sb, forNode.spaceCount);
            sb.Append($"&{forNode.childs[0].childs[0].val}& := {begVal}");
            sb.Append('\n');
            addSpaces(sb, forNode.spaceCount);
            sb.Append($"LABEL &labelIn{labelCount}&");
            sb.Append('\n');
            addSpaces(sb, forNode.spaceCount);
            sb.Append($"IF &{forNode.childs[0].childs[0].val}& >= {endVal} THEN GOTO &lablOut{labelCount}&");
            sb.Append('\n');
        }

        private static void addForEnd(Node forNode, StringBuilder sb, int labelCount)
        {
            string iter = "1";
            if (forNode.childs[1].tn == SyntaxTree.TypeNode.RANGE)
            {
                Node rangeNode = forNode.childs[1];
                if (rangeNode.childs.Count == 3)
                    iter = rangeNode.childs[2].val;
            }
            addSpaces(sb, forNode.spaceCount + 2);
            sb.Append($"&{forNode.childs[0].childs[0].val}& := &{forNode.childs[0].childs[0].val}& + {iter}");
            sb.Append('\n');
            addSpaces(sb, forNode.spaceCount);
            sb.Append($"GOTO &labelIn{labelCount}&");
            sb.Append('\n');
            addSpaces(sb, forNode.spaceCount);
            sb.Append($"LABEL &labelOut{labelCount}&");
            sb.Append('\n');
        }

        private static void addExpr(Node expr, StringBuilder sb)
        {
            foreach (var z in expr.childs)
            {
                if (z.tn == SyntaxTree.TypeNode.VAR)
                {
                    sb.Append($" &{z.val}&");
                }
                else if (z.tn == SyntaxTree.TypeNode.CONST)
                {
                    sb.Append($" {z.val}");
                }
                else if (z.tn == SyntaxTree.TypeNode.ADD)
                {
                    sb.Append(" +");
                }
                else if (z.tn == SyntaxTree.TypeNode.SUB)
                {
                    sb.Append(" -");
                }
                else if (z.tn == SyntaxTree.TypeNode.MULT)
                {
                    sb.Append(" *");
                }
                else if (z.tn == SyntaxTree.TypeNode.DIV)
                {
                    sb.Append(" /");
                }
                else
                {
                    if (z == expr.childs[0])
                        sb.Append($" {z.val}");
                    else
                        sb.Append($"{z.val}");
                }
            }
        }

        private static void addSpaces(StringBuilder sb, int spaceCount)
        {
            for (int i = 0; i < spaceCount; ++i)
                sb.Append(' ');
        }
    }
}
