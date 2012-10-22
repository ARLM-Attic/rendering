using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;

namespace System.Compilers.Optimizers
{
    public class GotoRemovalOptimizer:Optimizer
    {
        Dictionary<NetAstNode, NetAstNode> parent = new Dictionary<NetAstNode, NetAstNode>();
        Dictionary<NetAstNode, NetAstNode> nextSibling = new Dictionary<NetAstNode, NetAstNode>();
        HashSet<NetAstNode> remove = new HashSet<NetAstNode>();
        HashSet<NetAstNode> toContinue = new HashSet<NetAstNode>();
        HashSet<NetAstNode> toBreak = new HashSet<NetAstNode>();

		public override void Optimize(NetAstBlock toOptimize)
        {
            RemoveGotos(toOptimize);            
        }

        private void RemoveGotos(NetAstBlock method)
        {
            // Build the navigation data
            parent[method] = null;
            foreach (NetAstNode node in method.GetSelfAndChildrenRecursive<NetAstNode>())
            {
                NetAstNode previousChild = null;
                foreach (NetAstNode child in node.GetChildren())
                {
                    parent[child] = node;
                    if (previousChild != null)
                        nextSibling[previousChild] = child;
                    previousChild = child;
                }
                if (previousChild != null)
                    nextSibling[previousChild] = null;
            }

            // Simplify gotos
            bool modified;
            do
            {
                modified = false;
                foreach (var gotoExpr in method.GetSelfAndChildrenRecursive<NetAstNode>().Where(e => e is NetAstUnconditionalGoto))
                {
                    modified |= TrySimplifyGoto(gotoExpr);
                }
            } while (modified);

            RemoveRedundantCode(method);
            RemoveDeadLabels(method);
        }

        bool TrySimplifyGoto(NetAstNode gotoExpr)
        {
            if (remove.Contains(gotoExpr) || toContinue.Contains(gotoExpr) || toBreak.Contains(gotoExpr))
                return false;

            var target = Enter(gotoExpr, new HashSet<NetAstNode>());
            if (target == null)
                return false;

            // The gotoExper is marked as visited because we do not want to
            // walk over node which we plan to modify

            // The simulated path always has to start in the same try-block
            // in other for the same finally blocks to be executed.

            if (target == Exit(gotoExpr, new HashSet<NetAstNode>() { gotoExpr }))
            {
                remove.Add(gotoExpr);
                //gotoExpr = new NetAstEmptyNode();
                return true;
            }

            NetAstNode breakBlock = GetParents(gotoExpr).Where(n => n is NetAstWhile || n is NetAstSwitch).FirstOrDefault();
            if (breakBlock != null && target == Exit(breakBlock, new HashSet<NetAstNode>() { gotoExpr }))
            {
                toBreak.Add(gotoExpr);
                //gotoExpr = new NetAstBreak();
                return true;
            }

            NetAstNode continueBlock = GetParents(gotoExpr).Where(n => n is NetAstWhile).FirstOrDefault();
            if (continueBlock != null && target == Enter(continueBlock, new HashSet<NetAstNode>() { gotoExpr }))
            {
                toContinue.Add(gotoExpr);
                //gotoExpr = new NetAstContinue();
                return true;
            }
            return false;
        }

        private void RemoveRedundantCode(NetAstBlock method)
        {
            // Remove dead lables and nops
            foreach (NetAstBlock block in method.GetSelfAndChildrenRecursive<NetAstBlock>().ToList())
            {
                //Remove redundant goto
                block.Instructions = block.Instructions.Where(n => !remove.Contains(n)).ToList();
                
                //Replace continue statements
                block.Instructions = block.Instructions.Select(n => toContinue.Contains(n) ? new NetAstContinue() : n).ToList();

                //Replace break statements
                block.Instructions = block.Instructions.Select(n => toBreak.Contains(n) ? new NetAstBreak() : n).ToList();
            }

            // Remove redundant continue
            foreach (var loop in method.GetSelfAndChildrenRecursive<NetAstWhile>())
            {
                var body = loop.Body as NetAstBlock;
                var instructions = body.Instructions;

                if (instructions.Count > 0 && instructions.Last() is NetAstContinue)
                {
                    instructions.RemoveAt(instructions.Count - 1);
                }
            }

            // Remove redundant return
            if (method.Instructions.Count > 0 && method.Instructions.Last() is NetAstReturn && ((NetAstReturn)method.Instructions.Last()).Expression == null)
            {
                method.Instructions.RemoveAt(method.Instructions.Count - 1);
            }

        }

        private void RemoveDeadLabels(NetAstBlock method)
        {
            // Remove dead lables and nops
            HashSet<NetAstLabel> liveLabels = new HashSet<NetAstLabel>(method.GetSelfAndChildrenRecursive<NetAstNode>().SelectMany(e => GetBranchTargets(e)));
            foreach (NetAstBlock block in method.GetSelfAndChildrenRecursive<NetAstBlock>().ToList())
            {
                //Remove redundant goto
                block.Instructions = block.Instructions.Where(n => !(n is NetAstLabel) || liveLabels.Contains((NetAstLabel)n)).ToList();
            }
        }
        
        // Get the first expression to be excecuted if the instruction pointer is at the start of the given node.
        NetAstNode Enter(NetAstNode node, HashSet<NetAstNode> visitedNodes)
        {
            if (node == null)
                throw new ArgumentNullException();

            if (!visitedNodes.Add(node))
                return null;  // Infinite loop

            NetAstNode label = node as NetAstLabel;
            if (label != null)            
                return Exit(label, visitedNodes);
        
            //ILExpression expr = node as ILExpression;
            if (node is NetAstUnconditionalGoto)
            {
                var expr = node as NetAstUnconditionalGoto;
                var target = expr.Destination;
                return Enter(target, visitedNodes);
            }
            
            if (node is NetAstBreak)
            {
                var breakBlock = GetParents(node).Where(n => n is NetAstWhile || n is NetAstSwitch).First();
                return Exit(breakBlock, new HashSet<NetAstNode>() { node });
            }
            
            if (node is NetAstContinue)
            {
                var continueBlock = GetParents(node).Where(n => n is NetAstWhile).First();
                return Enter(continueBlock, new HashSet<NetAstNode>() { node });
            }
            if (node is NetAstBlock)
            {
                var block = node as NetAstBlock;
                if (block.EntryGoto != null)
                {
                    return Enter(block.EntryGoto, visitedNodes);
                }
                else if (block.Instructions.Count > 0)
                {
                    return Enter(block.Instructions[0], visitedNodes);
                }
                else
                {
                    return Exit(block, visitedNodes);
                }
            }

            if (node is NetAstIf)
                return (node as NetAstIf).Condition;

            if (node is NetAstWhile)
            {
                var loop = node as NetAstWhile;
                if (loop.Condition != null)
                    return loop.Condition;
                else
                    return Enter(loop.Body, visitedNodes);
            }

            if (node is NetAstSwitch)
                return (node as NetAstSwitch).Condition;
            
            return node;
        }

        // Get the first expression to be excecuted if the instruction pointer is at the end of the given node
        NetAstNode Exit(NetAstNode node, HashSet<NetAstNode> visitedNodes)
        {
            if (node == null)
                throw new ArgumentNullException();

            NetAstNode nodeParent = parent[node];
            if (nodeParent == null)
                return null;  // Exited main body

            if (nodeParent is NetAstBlock)
            {
                NetAstNode nextNode = nextSibling[node];
                if (nextNode != null)
                {
                    return Enter(nextNode, visitedNodes);
                }
                else
                {
                    return Exit(nodeParent, visitedNodes);
                }
            }

            if (nodeParent is NetAstIf)
            {
                return Exit(nodeParent, visitedNodes);
            }

            if (nodeParent is NetAstWhile)
            {
                return Enter(nodeParent, visitedNodes);
            }

            throw new NotSupportedException(nodeParent.GetType().ToString());
        }

        IEnumerable<NetAstNode> GetParents(NetAstNode node)
        {
            NetAstNode current = node;
            while (true)
            {
                current = parent[current];
                if (current == null)
                    yield break;
                yield return current;
            }
        }
    }
}
