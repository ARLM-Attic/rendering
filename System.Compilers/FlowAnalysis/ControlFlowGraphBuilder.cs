using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;

namespace System.Compilers.FlowAnalysis
{
    class ControlFlowGraphBuilder
    {
        ControlFlowNode<NetAstNode> entryPoint;
        ControlFlowNode<NetAstNode> regularExit;
        List<NetAstNode> methodBody;
        List<ControlFlowNode<NetAstNode>> nodes;
        Dictionary<NetAstNode, bool> hasIncomingJumps;



        private ControlFlowGraphBuilder(IEnumerable<NetAstNode> methodBody)
        {
            this.methodBody = new List<NetAstNode>(methodBody);
            hasIncomingJumps = Enumerable.ToDictionary(methodBody, (inst) => inst, (inst) => false);
            nodes = new List<ControlFlowNode<NetAstNode>>();
            entryPoint = new ControlFlowNode<NetAstNode>(ControlFlowNodeType.EntryPoint);
            
            nodes.Add(entryPoint);
            regularExit = new ControlFlowNode<NetAstNode>(ControlFlowNodeType.RegularExit);
            nodes.Add(regularExit);            
        }

        public static ControlFlowGraph<NetAstNode> BuildGraph(IEnumerable<NetAstNode> methodBody)
        {
            ControlFlowGraphBuilder builder = new ControlFlowGraphBuilder(methodBody);
            return builder.Build();
        }

        public ControlFlowGraph<NetAstNode> Build()
        {
            CalculateHasIncomingJumps();
            CreateNodes();
            CreateRegularControlFlow();
            return new ControlFlowGraph<NetAstNode>(nodes.ToArray());
        }

        void CalculateHasIncomingJumps()
        {
            foreach (var inst in methodBody)
            {
                if (inst is NetGoto)
                    hasIncomingJumps.Add((inst as NetGoto).Destination, true);
                
                else if (inst is NetSwitch)
                {
                    foreach (var i in ((NetSwitch)inst).Cases)
                        hasIncomingJumps[i] = true;
                }
            }
        }

        void CreateNodes()
        {
            // Step 2a: find basic blocks and create nodes for them
            for (int i = 0; i < methodBody.Count; i++)
            {
                var inst = methodBody[i];
                if (IsBranch(inst))
                    nodes.Add(new ControlFlowNode<NetAstNode>(ControlFlowNodeType.Normal) { InnerData = inst });
                else
                {
                    NetAstBlock instructionsBlock = new NetAstBlock();
                    instructionsBlock.Instructions.Add(inst);
                    
                    i++;
                    for(; i < methodBody.Count && !IsStartOfBlock(methodBody[i]); i++)
                        instructionsBlock.Instructions.Add(methodBody[i]);
                    nodes.Add(new ControlFlowNode<NetAstNode>(ControlFlowNodeType.Normal) { InnerData = instructionsBlock });
                    //The outter for will increase the variable, so we need to dicrease it here to not miss instructions.
                    i--;
                }
            }
        }

        private void CreateRegularControlFlow()
        {
            CreateEdge(entryPoint, methodBody[0]);
            for(int i = 0; i<nodes.Count; i++)
            {
                var node = nodes[i];
                var end = GetEnd(node);
                if (end != null)
                {
                    // create normal edges from one instruction to the next
                    if (!IsUnconditionalBranch(end) && i + 1 < nodes.Count)
                        CreateEdge(node, nodes[i + 1]);

                    // create edges for branch instructions
                    else if (IsUnconditionalBranch(end))
                        CreateEdge(node, (end as NetGoto).Destination);


                    else if (end is NetSwitch)
                        foreach (var switchCase in ((NetSwitch)end).Cases)
                            CreateEdge(node, switchCase);


                    else if (end is NetAstReturn)
                        CreateEdge(node, regularExit);
                }
            }
        }

        private bool IsUnconditionalBranch(NetAstNode end)
        {
            return end is NetGoto;
        }

        private NetAstNode GetEnd(ControlFlowNode<NetAstNode> node)
        {
            if (node.InnerData is NetAstBlock)
            {
                return ((NetAstBlock)node.InnerData).End;
            }
            return node.InnerData;
        }

        private bool IsStartOfBlock(NetAstNode netAstNode)
        {
            return IsBranch(netAstNode) || hasIncomingJumps[netAstNode];
        }

        private bool IsBranch(NetAstNode inst)
        {
            return inst is NetGoto || inst is NetAstReturn || inst is NetSwitch;
        }

        #region Create edges
        void CreateEdge(ControlFlowNode<NetAstNode> fromNode, NetAstNode toInstruction)
        {
            CreateEdge(fromNode, nodes.Single( (n) => n.InnerData == toInstruction ||
                                                     (n.InnerData is NetAstBlock && (n.InnerData as NetAstBlock).Start == toInstruction)));
        }

        void CreateEdge(ControlFlowNode<NetAstNode> fromNode, ControlFlowNode<NetAstNode> toNode)
        {
            ControlFlowEdge<NetAstNode> edge = new ControlFlowEdge<NetAstNode>(fromNode, toNode);
            fromNode.Outgoing.Add(edge);
            toNode.Incoming.Add(edge);
        }
        #endregion
    }
}
