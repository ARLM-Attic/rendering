using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Compilers.FlowAnalysis
{
    public enum ControlFlowNodeType
    {
        Normal,
        EntryPoint,
        RegularExit
    }

    public class ControlFlowNode<T>
    {
        public ControlFlowNodeType NodeType { get; private set; }

        public bool Visited { get; set; }

        public T InnerData { get; set; }
        
        public ControlFlowNode<T> ImmediateDominator { get; internal set; }

        public readonly List<ControlFlowNode<T>> DominatorTreeChildren = new List<ControlFlowNode<T>>();

        public int BlockIndex { get; private set; }
        
        // The dominance frontier of this node.
        // This is the set of nodes for which this node dominates a predecessor, but which are not strictly dominated by this node.
        // b.DominanceFrontier = { y in CFG; (exists p in predecessors(y): b dominates p) and not (b strictly dominates y)}
        public HashSet<ControlFlowNode<T>> DominanceFrontier;

        public readonly List<ControlFlowEdge<T>> Incoming = new List<ControlFlowEdge<T>>();

        public readonly List<ControlFlowEdge<T>> Outgoing = new List<ControlFlowEdge<T>>();

        public ControlFlowNode(ControlFlowNodeType type)
        {
            NodeType = type;
        }

        public ControlFlowNode(int blockIndex, ControlFlowNodeType type)
        {
            NodeType = type;
            BlockIndex = blockIndex;
        }

        public IEnumerable<ControlFlowNode<T>> Predecessors
        {
            get
            {
                return Incoming.Select(e => e.Source);
            }
        }

        public IEnumerable<ControlFlowNode<T>> Successors
        {
            get
            {
                return Outgoing.Select(e => e.Target);
            }
        }

        public bool IsReachable
        {
            get { return ImmediateDominator != null; }
        }

        public void TraversePreOrder(Func<ControlFlowNode<T>, IEnumerable<ControlFlowNode<T>>> children, Action<ControlFlowNode<T>> visitAction)
        {
            if (Visited)
                return;
            Visited = true;
            visitAction(this);
            foreach (ControlFlowNode<T> t in children(this))
                t.TraversePreOrder(children, visitAction);
        }

        public void TraversePostOrder(Func<ControlFlowNode<T>, IEnumerable<ControlFlowNode<T>>> children, Action<ControlFlowNode<T>> visitAction)
        {
            if (Visited)
                return;
            Visited = true;
            foreach (ControlFlowNode<T> t in children(this))
                t.TraversePostOrder(children, visitAction);
            visitAction(this);
        }

        public bool Dominates(ControlFlowNode<T> node)
        {
            ControlFlowNode<T> tmp = node;
            while (tmp != null)
            {
                if (tmp == this)
                    return true;
                tmp = tmp.ImmediateDominator;
            }
            return false;
        }
    }
}
