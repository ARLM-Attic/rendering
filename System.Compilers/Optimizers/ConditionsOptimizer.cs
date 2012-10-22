using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;
using System.Compilers.FlowAnalysis;

namespace System.Compilers.Optimizers
{
    public class ConditionalsOptimizer : Optimizer
    {
        public override void Optimize(NetAstBlock toOptimize)
        {
            //OptBlock result = null;
            foreach (var block in toOptimize.GetSelfAndChildrenRecursive<NetAstBlock>())
            {
                var graph = BuildGraph(block.Instructions, block.EntryGoto.Destination);
                graph.ComputeDominance();
                graph.ComputeDominanceFrontier();
                block.Instructions = FindConditions(new HashSet<ControlFlowNode<NetAstStatement>>(graph.Nodes.Skip(2)), graph.EntryPoint).Instructions;
            }
        }

        OptBlock FindConditions(HashSet<ControlFlowNode<NetAstStatement>> nodes, ControlFlowNode<NetAstStatement> entryNode)
        {
            List<NetAstStatement> result = new List<NetAstStatement>();

            // Do not modify entry data
            var scope = new HashSet<ControlFlowNode<NetAstStatement>>(nodes);

            HashSet<ControlFlowNode<NetAstStatement>> agenda = new HashSet<ControlFlowNode<NetAstStatement>>();
            agenda.Add(entryNode);
            while (agenda.Any())
            {
                ControlFlowNode<NetAstStatement> node = agenda.First();
                // Attempt for a good order
                while (agenda.Contains(node.ImmediateDominator))
                    node = node.ImmediateDominator;
                agenda.Remove(node);

                // Find a block that represents a simple condition
                if (scope.Contains(node))
                {
                    NetAstExpression condExpr = new NetAstConstantExpression(true);
                    OptBlock block = node.InnerData as OptBlock;


                    if (block.Instructions.Count == 1 && block.Instructions[0] is NetAstConditionalGoto)
                    {
                        var condGoto = block.Instructions[0] as NetAstConditionalGoto;
                        var trueLabel = condGoto.Destination;
                        var falseLabel = block.FallthoughGoto.Destination;
                        
                        // Swap bodies since that seems to be the usual C# order
                        var temp = trueLabel;
                        trueLabel = falseLabel;
                        falseLabel = temp;

                        ControlFlowNode<NetAstStatement> trueTarget = null;
                        labelToCfNode.TryGetValue(trueLabel, out trueTarget);

                        ControlFlowNode<NetAstStatement> falseTarget = null;
                        labelToCfNode.TryGetValue(falseLabel, out falseTarget); 
                        
                        condExpr = condGoto.Condition;
                        var trueBody = new NetAstBlock() { EntryGoto = new NetAstUnconditionalGoto() { Destination = trueLabel } };
                        var falseBody = new NetAstBlock() { EntryGoto = new NetAstUnconditionalGoto() { Destination = falseLabel } };

                        condExpr = new NetAstUnaryOperatorExpression() { Operand = condExpr, Operator = Operators.Not };

                        // Convert the basic block to ILCondition
                        NetAstIf ilCond = new NetAstIf()
                        {
                            Condition = condExpr,
                            WhenTrue = trueBody ,
                            WhenFalse = falseBody 
                        };
                        result.Add(new OptBlock()
                        {
                            EntryLabel = block.EntryLabel,  // Keep the entry label
                            Instructions = { ilCond }
                        });

                        // Remove the item immediately so that it is not picked up as content
                        scope.Remove(node);

                        // Pull in the conditional code
                        var frontiers = new HashSet<ControlFlowNode<NetAstStatement>>();
                        if (trueTarget != null)
                            frontiers.UnionWith(trueTarget.DominanceFrontier);
                        if (falseTarget != null)
                            frontiers.UnionWith(falseTarget.DominanceFrontier);

                        if (trueTarget != null && !frontiers.Contains(trueTarget))
                        {
                            var content = FindDominatedNodes(scope, trueTarget);
                            scope.ExceptWith(content);
                            trueBody.Instructions.AddRange(FindConditions(content, trueTarget).Instructions);
                        }
                        if (falseTarget != null && !frontiers.Contains(falseTarget))
                        {
                            var content = FindDominatedNodes(scope, falseTarget);
                            scope.ExceptWith(content);
                            falseBody.Instructions.AddRange(FindConditions(content, falseTarget).Instructions);
                        }

                        if (falseBody.Instructions.Count == 0)
                            ilCond.WhenFalse = null;
                    }

                    // Add the node now so that we have good ordering
                    if (scope.Contains(node))
                    {
                        result.Add(node.InnerData);
                        scope.Remove(node);
                    }
                }
                // Using the dominator tree should ensure we find the the widest loop first
                foreach (var child in node.DominatorTreeChildren)
                    agenda.Add(child);
            }

            // Add whatever is left
            foreach (var node in scope)
                result.Add(node.InnerData);

            return new OptBlock() { Instructions = result };
        }
    }
}
