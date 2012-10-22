using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;
using System.Compilers.FlowAnalysis;

namespace System.Compilers.Optimizers
{
    public class LoopsOptimizer:Optimizer
    {
        int nextLabelIndex;

        public override void Optimize(NetAstBlock toOptimize)
        {
             //OptBlock result = null;
            foreach (var block in toOptimize.GetSelfAndChildrenRecursive<NetAstBlock>())
            {
                var graph = BuildGraph(block.Instructions, block.EntryGoto.Destination);
                graph.ComputeDominance();
                graph.ComputeDominanceFrontier();
                block.Instructions = FindLoops(new HashSet<ControlFlowNode<NetAstStatement>>(graph.Nodes.Skip(2)), graph.EntryPoint, false).Instructions;
            }
        }

        private OptBlock FindLoops(HashSet<ControlFlowNode<NetAstStatement>> nodes, ControlFlowNode<NetAstStatement> entryPoint, bool excludeEntryPoint)
        {
            List<NetAstStatement> result = new List<NetAstStatement>();

            // Do not modify entry data
            var scope = new HashSet<ControlFlowNode<NetAstStatement>>(nodes);

            var agenda = new Queue<ControlFlowNode<NetAstStatement>>();
            agenda.Enqueue(entryPoint);
            while (agenda.Count > 0)
            {

                ControlFlowNode<NetAstStatement> node = agenda.Dequeue();

                #region If the node is a loop header
                if (scope.Contains(node)                                // The loop has not being analized yet
                    && node.DominanceFrontier.Contains(node)            // And there's an edge from descendent to it self
                    && (node != entryPoint || !excludeEntryPoint))      // and if it's an inner loop it's not the entry of the parent loop
                {

                    //Find the natural loop
                    var loopContents = FindLoopContent(scope, node);

                    #region If the first expression is a loop condition
                    NetAstExpression condExpr = new NetAstConstantExpression(true);
                    OptBlock block = node.InnerData as OptBlock;
                    

                    if (block.Instructions.Count == 1 && block.Instructions[0] is NetAstConditionalGoto)
                    {
                        var condGoto = block.Instructions[0] as NetAstConditionalGoto;
                        var trueLabel = condGoto.Destination;
                        var falseLabel = block.FallthoughGoto.Destination;
                        var trueTarget = labelToCfNode[trueLabel];
                        var falseTarget = labelToCfNode[falseLabel];
                        condExpr = condGoto.Condition;

                        // If one point inside the loop and the other outside
                        if ((!loopContents.Contains(trueTarget) && loopContents.Contains(falseTarget)) ||
                            (loopContents.Contains(trueTarget) && !loopContents.Contains(falseTarget)))
                        {
                            loopContents.Remove(node);
                            scope.Remove(node);

                            // If false means enter the loop
                            if (loopContents.Contains(falseTarget))
                            {
                                // Negate the condition
                                condExpr = new NetAstUnaryOperatorExpression() { Operand = condGoto.Condition, Operator = Operators.Not };
                                var tmp = trueTarget;
                                var tmpLabel = trueLabel;

                                trueTarget = falseTarget;
                                trueLabel = falseLabel;

                                falseLabel = tmpLabel;
                                falseTarget = tmp;
                            }

                            var postLoopTarget = falseTarget;
                            if (postLoopTarget != null)
                            {
                                // Pull more nodes into the loop
                                var postLoopContents = FindDominatedNodes(scope, postLoopTarget);
                                var pullIn = scope.Except(postLoopContents).Where(n => node.Dominates(n));
                                loopContents.UnionWith(pullIn);
                            }

                            // Use loop to implement the condition
                            // Use loop to implement the condition
                            result.Add(new OptBlock()
                            {
                                EntryLabel = block.EntryLabel,
                                Instructions = new List<NetAstStatement>() {
							           		new NetAstWhile() {
							           			Condition = condExpr,
							           			Body = new NetAstBlock() {
							           				EntryGoto = new NetAstUnconditionalGoto(){Destination = trueLabel},
							           				Instructions = FindLoops(loopContents, node, true).Instructions
							           			}
							           		},
							           		new NetAstUnconditionalGoto(){Destination = falseLabel}
                                },
                                FallthoughGoto = null
                            });
                        }
                    }
                    #endregion

                    // Fallback method: while(true)
                    if (scope.Contains(node))
                    {
                        result.Add(new OptBlock()
                        {
                            EntryLabel = new NetAstLabel() { Name = "Loop_" + (nextLabelIndex++) },
                            Instructions = new List<NetAstStatement>() {
						           		new NetAstWhile() {
						           			Body = new NetAstBlock() {
						           				EntryGoto = new NetAstUnconditionalGoto() {Destination = block.EntryLabel},
						           				Instructions = FindLoops(loopContents, node, true).Instructions
						           			}
						           		},
						           	},
                            FallthoughGoto = null
                        });
                    }

                    // Move the content into loop block
                    scope.ExceptWith(loopContents);
                }
                #endregion
                

                // Using the dominator tree should ensure we find the the widest loop first
                foreach (var child in node.DominatorTreeChildren)
                {
                    agenda.Enqueue(child);
                }
            }

            // Add whatever is left
            foreach (var node in scope)
            {
                result.Add(node.InnerData);
            }
            scope.Clear();

            return new OptBlock() { Instructions = result };
        }

        static HashSet<ControlFlowNode<NetAstStatement>> FindLoopContent(HashSet<ControlFlowNode<NetAstStatement>> scope, ControlFlowNode<NetAstStatement> head)
        {
            var viaBackEdges = head.Predecessors.Where(p => head.Dominates(p));
            HashSet<ControlFlowNode<NetAstStatement>> agenda = new HashSet<ControlFlowNode<NetAstStatement>>(viaBackEdges);
            HashSet<ControlFlowNode<NetAstStatement>> result = new HashSet<ControlFlowNode<NetAstStatement>>();

            while (agenda.Count > 0)
            {
                ControlFlowNode<NetAstStatement> addNode = agenda.First();
                agenda.Remove(addNode);

                if (scope.Contains(addNode) && head.Dominates(addNode) && result.Add(addNode))
                {
                    foreach (var predecessor in addNode.Predecessors)
                    {
                        agenda.Add(predecessor);
                    }
                }
            }
            if (scope.Contains(head))
                result.Add(head);

            return result;
        }
    }
}
