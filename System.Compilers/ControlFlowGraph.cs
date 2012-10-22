using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Compilers.AST;

namespace System.Compilers
{
    class ControlFlowGraph<T>
    {
        readonly ReadOnlyCollection<ControlFlowNode<T>> nodes;

        public ControlFlowNode<T> EntryPoint
        {
            get { return nodes[0]; }
        }

        public ControlFlowNode<T> RegularExit
        {
            get { return nodes[1]; }
        }

        internal ControlFlowGraph(ControlFlowNode<T>[] nodes)
        {
            this.nodes = new ReadOnlyCollection<ControlFlowNode<T>>(nodes);
        }

        //Just for wrapping another control flow graph
        public static ControlFlowGraph<T> BuildGraph(T firstNode, Func<T, IEnumerable<T>> sucessors, Func<T, IEnumerable<T>> predecessors)
        {
            Dictionary<T, ControlFlowNode<T>> nodes = new Dictionary<T, ControlFlowNode<T>>();

            ControlFlowNode<T> entryNode = new ControlFlowNode<T>(ControlFlowNodeType.EntryPoint);

            ControlFlowNode<T> regularExitNode = new ControlFlowNode<T>(ControlFlowNodeType.RegularExit);

            var result = BuildNode(firstNode, sucessors, predecessors, nodes, regularExitNode);
            
            entryNode.Outgoing.Add(new ControlFlowEdge<T>(entryNode,result));

            result.Incoming.Add(new ControlFlowEdge<T>(entryNode,result));

            return new ControlFlowGraph<T>(new ControlFlowNode<T>[]{entryNode, regularExitNode});
        }

        private static ControlFlowNode<T> BuildNode(T node, Func<T, IEnumerable<T>> sucessors, Func<T, IEnumerable<T>> predecessors, Dictionary<T, ControlFlowNode<T>> builtNodes, ControlFlowNode<T> regularExitNode)
        {
            if (builtNodes.ContainsKey(node))
                return builtNodes[node];

            var result = new ControlFlowNode<T>(ControlFlowNodeType.Normal);

            foreach (var predecessor in predecessors(node))
            {
                var predecessorControlNode = BuildNode(predecessor, sucessors, predecessors, builtNodes, regularExitNode);
                var newEdge = new ControlFlowEdge<T>(predecessorControlNode, result);
                result.Incoming.Add(newEdge);
            }

            int count = 0;

            foreach (var sucessor in sucessors(node))
            {
                var sucessorControlNode = BuildNode(sucessor, sucessors, predecessors, builtNodes, regularExitNode);
                var newEdge = new ControlFlowEdge<T>(result, sucessorControlNode);
                result.Outgoing.Add(newEdge);
                count++;
            }

            if (count == 0)
                result.Outgoing.Add(new ControlFlowEdge<T>(result, regularExitNode));

            return result;
        }

        public ControlFlowGraph<NetAstNode> BuildGraphFromAst(NetAstBlock methodBody)
        {

            #region Split blocks such that no goto destiny is in the middle of any block
            var splittenBlocks = Split(methodBody);
            #endregion

            #region Create nodes for splitten blocks
            #endregion

            #region Add flow edges between nodes created
            #endregion

            return null;
        }

        private IEnumerable<NetAstNode> Split(NetAstBlock methodBody)
        {
            Dictionary<NetAstNode, bool> isStartOfBlock = new Dictionary<NetAstNode, bool>();
            List<NetAstNode> result = new List<NetAstNode>();

            //Mark instructions that are jumps or jump destination.
            foreach (var item in methodBody.Instructions)
            {
                if (item is NetGoto)
                {
                    isStartOfBlock[item] = true;
                    isStartOfBlock[((NetGoto)item).Destination] = true;
                }
                else if(item is NetAstReturn)
                    isStartOfBlock[item] = true;
            }

            for (int i = 0; i < methodBody.Instructions.Length; i++)
            {
                var currentInstruction = methodBody.Instructions[i];

                if (currentInstruction is NetGoto || currentInstruction is NetAstReturn)
                    result.Add(currentInstruction);

                else
                {
                    List<NetAstNode> instructionsBlock = new List<NetAstNode>();
                    instructionsBlock.Add(currentInstruction);
                    i++;
                    for (; i < methodBody.Instructions.Length; i++)
                    {
                        currentInstruction = methodBody.Instructions[i];
                        if (!isStartOfBlock.ContainsKey(currentInstruction) || !isStartOfBlock[currentInstruction])
                            instructionsBlock.Add(currentInstruction);
                    }
                    i--;

                    result.Add(new NetAstBlock() { Instructions = instructionsBlock.ToArray() });
                }

            }

            return result;
        }

        public void ResetVisited()
        {
            foreach (ControlFlowNode<T> node in nodes)
            {
                node.Visited = false;
            }
        }

        public void ComputeDominance()
        {
            EntryPoint.ImmediateDominator = EntryPoint;
            bool changed = true;
            while (changed)
            {
                changed = false;
                ResetVisited();

                // for all nodes b except the entry point
                EntryPoint.TraversePreOrder(
                    b => b.Successors,
                    b =>
                    {
                        if (b != EntryPoint)
                        {
                            ControlFlowNode<T> newIdom = b.Predecessors.First(block => block.Visited);
                            // for all other predecessors p of b
                            foreach (ControlFlowNode<T> p in b.Predecessors)
                            {
                                if (p != b && p.ImmediateDominator != null)
                                {
                                    newIdom = FindCommonDominator(p, newIdom);
                                }
                            }
                            if (b.ImmediateDominator != newIdom)
                            {
                                b.ImmediateDominator = newIdom;
                                changed = true;
                            }
                        }
                    });
            }

            EntryPoint.ImmediateDominator = null;
            foreach (ControlFlowNode<T> node in nodes)
            {
                if (node.ImmediateDominator != null)
                    node.ImmediateDominator.DominatorTreeChildren.Add(node);
            }
        }

        static ControlFlowNode<T> FindCommonDominator(ControlFlowNode<T> b1, ControlFlowNode<T> b2)
        {
            HashSet<ControlFlowNode<T>> path1 = new HashSet<ControlFlowNode<T>>();
            while (b1 != null && path1.Add(b1))
                b1 = b1.ImmediateDominator;
            while (b2 != null)
            {
                if (path1.Contains(b2))
                    return b2;
                else
                    b2 = b2.ImmediateDominator;
            }
            throw new Exception("No common dominator found!");
        }

        /// <summary>
        /// Computes dominance frontiers.
        /// This method requires that the dominator tree is already computed!
        /// </summary>
        public void ComputeDominanceFrontier()
        {
            ResetVisited();

            EntryPoint.TraversePostOrder(
                b => b.DominatorTreeChildren,
                n =>
                {
                    n.DominanceFrontier = new HashSet<ControlFlowNode<T>>();
                    
                    // DF_local computation
                    foreach (ControlFlowNode<T> succ in n.Successors)
                        if (succ.ImmediateDominator != n)
                            n.DominanceFrontier.Add(succ);

                    // DF_up computation
                    foreach (ControlFlowNode<T> child in n.DominatorTreeChildren)                    
                        foreach (ControlFlowNode<T> p in child.DominanceFrontier)                        
                            if (p.ImmediateDominator != n)                            
                                n.DominanceFrontier.Add(p);                        
                    
                });
        }
    }
}
