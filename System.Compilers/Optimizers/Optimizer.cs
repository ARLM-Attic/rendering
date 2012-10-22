using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;
using System.Compilers.FlowAnalysis;

namespace System.Compilers.Optimizers
{
    public abstract class Optimizer
    {

        protected Dictionary<NetAstStatement, ControlFlowNode<NetAstStatement>> astNodeToCfNode;
        protected Dictionary<NetAstLabel, ControlFlowNode<NetAstStatement>> labelToCfNode;

        protected Dictionary<NetAstNode, int> indexes;
        protected List<ControlFlowNode<NetAstStatement>> flowNodes;

        public abstract void Optimize(NetAstBlock toOptimize);

        protected ControlFlowGraph<NetAstStatement> BuildGraph(List<NetAstStatement> nodes, NetAstLabel entryLabel)
        {
            int index = 0;
            flowNodes = new List<ControlFlowNode<NetAstStatement>>();
            EntryPoint = new ControlFlowNode<NetAstStatement>(index++, ControlFlowNodeType.EntryPoint);
            flowNodes.Add(EntryPoint);
            RegularExit = new ControlFlowNode<NetAstStatement>(index++, ControlFlowNodeType.RegularExit);
            flowNodes.Add(RegularExit);

            // Create graph nodes
            astNodeToCfNode = new Dictionary<NetAstStatement, ControlFlowNode<NetAstStatement>>();
            labelToCfNode = new Dictionary<NetAstLabel, ControlFlowNode<NetAstStatement>>();
            indexes = new Dictionary<NetAstNode, int>();

            foreach (var node in nodes)
            {
                ControlFlowNode<NetAstStatement> cfNode = new ControlFlowNode<NetAstStatement>(index++, ControlFlowNodeType.Normal);
                flowNodes.Add(cfNode);
                astNodeToCfNode[node] = cfNode;
                indexes[node] = cfNode.BlockIndex;
                cfNode.InnerData = node;

                // Find all contained labels
                foreach (NetAstLabel label in node.GetSelfAndChildrenRecursive<NetAstLabel>())
                    labelToCfNode[label] = cfNode;
            }

            // Entry endge
			var entryNode = labelToCfNode[entryLabel];
            var entryEdge = new ControlFlowEdge<NetAstStatement>(EntryPoint, entryNode);
			EntryPoint.Outgoing.Add(entryEdge);
			entryNode.Incoming.Add(entryEdge);

            // Create jump edges
            foreach (var node in nodes)
            {
                ControlFlowNode<NetAstStatement> source = astNodeToCfNode[node];

                // Find all branches
                foreach (var child in node.GetSelfAndChildrenRecursive<NetAstNode>())
                {
                    IEnumerable<NetAstLabel> targets = GetBranchTargets(child);
                    if (targets != null)
                    {
                        foreach (var target in targets)
                        {
                            ControlFlowNode<NetAstStatement> destination;
                            // Labels which are out of out scope will not be int the collection
                            if (labelToCfNode.TryGetValue(target, out destination) && destination != source)
                            {
                                ControlFlowEdge<NetAstStatement> edge = new ControlFlowEdge<NetAstStatement>(source, destination);
                                source.Outgoing.Add(edge);
                                destination.Incoming.Add(edge);
                            }
                        }
                    }
                }
            }

            return new ControlFlowGraph<NetAstStatement>(flowNodes.ToArray());
        }

        protected HashSet<ControlFlowNode<NetAstStatement>> FindDominatedNodes(HashSet<ControlFlowNode<NetAstStatement>> scope, ControlFlowNode<NetAstStatement> head)
        {
            var agenda = new HashSet<ControlFlowNode<NetAstStatement>>();
            var result = new HashSet<ControlFlowNode<NetAstStatement>>();
            agenda.Add(head);

            while (agenda.Count > 0)
            {
                var addNode = agenda.First();
                agenda.Remove(addNode);

                if (scope.Contains(addNode) && head.Dominates(addNode) && result.Add(addNode))
                {
                    foreach (var successor in addNode.Successors)
                    {
                        agenda.Add(successor);
                    }
                }
            }

            return result;
        }

        protected IEnumerable<NetAstLabel> GetBranchTargets(NetAstNode source)
        {
            if (source is NetAstUnconditionalGoto)
                return new NetAstLabel[] { (source as NetAstUnconditionalGoto).Destination };

            else if (source is NetAstConditionalGoto)
            {
                var condGoto = source as NetAstConditionalGoto;
                return new NetAstLabel[] { condGoto.Destination };
            }
            return Enumerable.Empty<NetAstLabel>();
        }

        protected bool IsBranch(NetAstNode lastNode)
        {
            return lastNode is NetAstGoto
                || lastNode is NetAstReturn;
        }


        protected ControlFlowNode<NetAstStatement> NextOf(NetAstConditionalGoto condGoto)
        {
            var instIndex = indexes[condGoto];
            return flowNodes[instIndex + 1];
        }

        protected IEnumerable<T> Get<T>(IEnumerable<NetAstStatement> block) where T : class
        {
            foreach (var item in block)
            {
                if (item is T)
                    yield return item as T;
            }
        }

        public ControlFlowNode<NetAstStatement> RegularExit { get; set; }
        public ControlFlowNode<NetAstStatement> EntryPoint { get; set; }

       
    }

    public static class Extensions
    {
        public static V GetOrDefault<K, V>(this Dictionary<K, V> dict, K key)
        {
            V ret;
            dict.TryGetValue(key, out ret);
            return ret;
        }
    }
}
