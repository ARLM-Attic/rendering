using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Compilers.AST;

namespace System.Compilers.FlowAnalysis
{
    public class ControlFlowGraph<T>
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

        public void ResetVisited()
        {
            foreach (ControlFlowNode<T> node in nodes)
            {
                node.Visited = false;
            }
        }

        public ReadOnlyCollection<ControlFlowNode<T>> Nodes { get { return nodes; } }

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
