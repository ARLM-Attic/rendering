using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;
using System.Diagnostics;
using System.Reflection;

namespace System.Compilers.Optimizers
{
    public class ShortCircuitAndTernaryOperatorOptimizer : Optimizer
    {

        Dictionary<NetAstLabel, int> labelGlobalRefCount;
        Dictionary<NetAstLabel, OptBlock> labelToBasicBlock;
		
        public override void Optimize(NetAstBlock toOptimize)
        {
            //AnalyseLabels(toOptimize);
            foreach (var block in toOptimize.GetSelfAndChildrenRecursive<NetAstBlock>().ToList())
                MakeOptimizations(block);
        }

        void AnalyseLabels(NetAstBlock method)
        {
            labelGlobalRefCount = new Dictionary<NetAstLabel, int>();
            foreach (NetAstLabel target in method.GetSelfAndChildrenRecursive<NetAstNode>().SelectMany(e => GetBranchTargets(e)))
            {
                if (!labelGlobalRefCount.ContainsKey(target))
                    labelGlobalRefCount[target] = 0;
                labelGlobalRefCount[target]++;
            }

            labelToBasicBlock = new Dictionary<NetAstLabel, OptBlock>();
            foreach (OptBlock bb in method.GetSelfAndChildrenRecursive<OptBlock>())
            {
                foreach (NetAstLabel label in bb.GetChildren().OfType<NetAstLabel>())
                {
                    labelToBasicBlock[label] = bb;
                }
            }
        }

        void MakeOptimizations(NetAstBlock block)
        {
            bool modified;
            do
            {
                modified = false;
                AnalyseLabels(block);
                for (int i = 0; i < block.Instructions.Count; )
                {
                    OptBlock bb = (OptBlock)block.Instructions[i];
                    if (TrySimplifyShortCircuit(block.Instructions, bb))
                    {
                        modified = true;
                        continue;
                    }
                    if (TrySimplifyTernaryOperator(block.Instructions, bb))
                    {
                        modified = true;
                        continue;
                    }
                    i++;
                }
            } while (modified);
        }

        bool TrySimplifyTernaryOperator(List<NetAstStatement> scope, OptBlock head)
        {
            //Debug.Assert(scope.Contains(head));

            if (head.Instructions.Count == 1 && head.Instructions[0] is NetAstConditionalGoto)
            {
                var condGoto = head.Instructions[0] as NetAstConditionalGoto;
                NetAstExpression condExpr = condGoto.Condition;
                NetAstLabel trueLabel = condGoto.Destination;
                NetAstLabel falseLabel = head.FallthoughGoto.Destination;
                OptBlock trueBlock = labelToBasicBlock[trueLabel];
                OptBlock falseBlock = labelToBasicBlock[falseLabel];
                var trueLocalAssignment = trueBlock.Instructions.Count > 0 ? trueBlock.Instructions[0] as NetAstAssignamentStatement : null;
                var falseLocalAssignment = falseBlock.Instructions.Count > 0 ? falseBlock.Instructions[0] as NetAstAssignamentStatement : null;
                NetAstLabel trueFall = trueBlock.FallthoughGoto!= null ? trueBlock.FallthoughGoto.Destination : null;
                NetAstLabel falseFall = falseBlock.FallthoughGoto != null ? falseBlock.FallthoughGoto.Destination : null;


                if (labelGlobalRefCount[trueLabel] == 1 && labelGlobalRefCount[falseLabel] == 1 &&
                    trueBlock.Instructions.Count == 1 && falseBlock.Instructions.Count == 1 &&
                    trueLocalAssignment != null && falseLocalAssignment != null &&
                    trueLocalAssignment.LeftValue is NetAstLocalExpression && falseLocalAssignment.LeftValue is NetAstLocalExpression &&
                    ((NetAstLocalExpression) trueLocalAssignment.LeftValue).LocalInfo.Name == ((NetAstLocalExpression) falseLocalAssignment.LeftValue).LocalInfo.Name &&
                    trueFall != null && falseFall != null &&
                    trueFall.Equals(falseFall)
                   )
                {
                    var trueLocVar = ((NetAstLocalExpression)trueLocalAssignment.LeftValue).LocalInfo;
                    NetAstExpression trueExpr = trueLocalAssignment.Value;

                    NetAstExpression falseExpr = falseLocalAssignment.Value;

                    if ((trueExpr.StaticType == typeof(bool) || falseExpr.StaticType == typeof(bool)) &&
                        (trueExpr is NetAstConstantExpression  || falseExpr is NetAstConstantExpression ))
                    {
                        var condOperator = Operators.None;
                        var leftOperand = condExpr;
                        var rightOperand = trueExpr;

                        if (falseExpr is NetAstConstantExpression)
                        {
                            if((int)((NetAstConstantExpression)trueExpr).Value == 0)
                                condOperator = Operators.ConditionalAnd;
                            else
                            {
                                condOperator = Operators.ConditionalOr;
                                leftOperand = new NetAstUnaryOperatorExpression(){ Operator = Operators.Not, Operand = condExpr};
                            }
                        }
                        else
                        {
                            if ((int)((NetAstConstantExpression)trueExpr).Value == 1)
                                condOperator = Operators.ConditionalOr;
                            else
                            {
                                condOperator = Operators.ConditionalAnd;
                                leftOperand = new NetAstUnaryOperatorExpression() { Operator = Operators.Not, Operand = condExpr };
                            }

                            rightOperand = falseExpr;
                        }

                        // Create the short-circuit expression
                        head.Instructions = new List<NetAstStatement>() { new NetAstAssignamentStatement() 
                        {
                            LeftValue = new NetAstLocalExpression { LocalInfo = trueLocVar },

                            Value = new NetAstBinaryOperatorExpression(){ LeftOperand = leftOperand, RightOperand = rightOperand, Operator = condOperator }
                        }
                        };
                    }

                    else
                    {
                        // Create the ternary expression
                        head.Instructions = new List<NetAstStatement>() { new NetAstAssignamentStatement() 
                        {
                            LeftValue = new NetAstLocalExpression { LocalInfo = trueLocVar },
                            Value = new NetAstTernaryOperatorExpressionv(){ Conditional = condExpr, WhenTrue = trueExpr, WhenFalse = falseExpr}
                        }
                        };
                    }
                    head.FallthoughGoto = new NetAstUnconditionalGoto() { Destination = trueFall };

                    

                    // Remove the old blocks
                    scope.Remove(labelToBasicBlock[trueLabel]);
                    scope.Remove(labelToBasicBlock[falseLabel]);
                    labelToBasicBlock.Remove(trueLabel);
                    labelToBasicBlock.Remove(falseLabel);
                    labelGlobalRefCount.Remove(trueLabel);
                    labelGlobalRefCount.Remove(falseLabel);

                    return true;
                }
            }
            return false;
        }

        bool TrySimplifyShortCircuit(List<NetAstStatement> scope, OptBlock head)
        {
            Debug.Assert(scope.Contains(head));

            if (head.Instructions.Count == 1 && head.Instructions[0] is NetAstConditionalGoto)
            {
                var condGoto = head.Instructions[0] as NetAstConditionalGoto;
                var condExpr = condGoto.Condition;
                var trueLabel = condGoto.Destination;
                var falseLabel = head.FallthoughGoto.Destination;
            
                for (int pass = 0; pass < 2; pass++)
                {

                    // On the second pass, swap labels and negate expression of the first branch
                    NetAstLabel nextLabel = (pass == 0) ? trueLabel : falseLabel;
                    NetAstLabel otherLablel = (pass == 0) ? falseLabel : trueLabel;
                    bool negate = (pass == 1);

                    OptBlock nextBasicBlock = labelToBasicBlock[nextLabel];
                    if (nextBasicBlock.Instructions.Count == 0)
                        continue;

                    var nextCondGoto = nextBasicBlock.Instructions[0] as NetAstConditionalGoto;
                    
                    
                    
                    if (scope.Contains(nextBasicBlock) &&
                        nextBasicBlock != head &&
                        labelGlobalRefCount[nextBasicBlock.EntryLabel] == 1 &&
                        nextBasicBlock.Instructions.Count == 1 && nextCondGoto != null
                        )
                    {
                        var nextCondExpr = nextCondGoto.Condition;
                        NetAstLabel nextTrueLabel = nextCondGoto.Destination;
                        NetAstLabel nextFalseLabel = nextBasicBlock.FallthoughGoto.Destination;

                        if((otherLablel.Equals(nextFalseLabel) || otherLablel.Equals(nextTrueLabel)))
                        {
                        // Create short cicuit branch
                        if (otherLablel == nextFalseLabel)
                        {
                            var andLeftOperator = negate ? new NetAstUnaryOperatorExpression() { Operator = Operators.Not, Operand = condExpr } : condExpr;
                            var andRightOperator = condExpr;
                            head.Instructions[0] = new NetAstConditionalGoto() { Destination = nextTrueLabel, Condition = new NetAstBinaryOperatorExpression() { Operator = Operators.ConditionalAnd, LeftOperand = andLeftOperator, RightOperand = andRightOperator } };
                        }
                        else
                        {
                            var orLeftOperator = negate ? condExpr : new NetAstUnaryOperatorExpression() { Operator = Operators.Not, Operand = condExpr };
                            var orRightOperator = nextCondExpr;
                            head.Instructions[0] = new NetAstConditionalGoto() { Destination = nextTrueLabel, Condition = new NetAstBinaryOperatorExpression() { Operator = Operators.ConditionalOr, LeftOperand = orLeftOperator, RightOperand = orRightOperator } };
                        }
                        head.FallthoughGoto = new NetAstUnconditionalGoto() { Destination = nextFalseLabel };

                        // Remove the inlined branch from scope
                        labelGlobalRefCount.Remove(nextBasicBlock.EntryLabel);
                        labelToBasicBlock.Remove(nextBasicBlock.EntryLabel);
                        scope.Remove(nextBasicBlock);

                        return true;
                        }
                    }
                }
            }
            return false;
        }
		
    }
}
