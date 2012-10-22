using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;

namespace System.Compilers.Optimizers
{
    public class LoopsFormatterOptimizer :Optimizer
    {
        public override void Optimize(NetAstBlock toOptimize)
        {
            foreach (var block in toOptimize.GetSelfAndChildrenRecursive<NetAstBlock>())
            {
                TryFormatWhileAsFor(block);
                TryFormatAsDoWhile(block);
            }
        }

        private void TryFormatAsDoWhile(NetAstBlock block)
        {
            for (int i = 0; i < block.Instructions.Count; i++)
            {
                var whileStatement = block.Instructions[i] as NetAstWhile;
                if (whileStatement != null && 
                    whileStatement.Condition == null &&
                    whileStatement.Body is NetAstBlock)
                {
                    var whileBlock = whileStatement.Body as NetAstBlock;
                    var breakCondition = whileBlock.Instructions.Last() as NetAstIf;
                    if (breakCondition != null && breakCondition.WhenFalse == null)
                    {
                        var ifBlock = breakCondition.WhenTrue as NetAstBlock;
                        if (ifBlock != null && ifBlock.Instructions.Count == 1 && ifBlock.Instructions[0] is NetAstBreak)
                        {
                            whileBlock.Instructions.RemoveAt(whileBlock.Instructions.Count - 1);
                            block.Instructions[i] = new NetAstDoWhile() { Condition = new NetAstUnaryOperatorExpression() 
                                                                                    {Operand = breakCondition.Condition, Operator = Operators.Not},
                                                                          Body = whileBlock };
                        }
                    }
                }
            }
        }

        private void TryFormatWhileAsFor(NetAstBlock block)
        {
            for (int i = 0; i < block.Instructions.Count - 1; i++)
            {
                var initStatement = block.Instructions[i] as NetAstAssignamentStatement;
                var whileStatement = block.Instructions[i+1] as NetAstWhile;
                if (initStatement != null && whileStatement != null && whileStatement.Body is NetAstBlock)
                {
                    var whileBody = (NetAstBlock)whileStatement.Body;
                    var iteration = whileBody.Instructions.Last() as NetAstAssignamentStatement;
                    if (iteration != null)
                    {
                        block.Instructions.Insert(i, 
                            new NetAstFor(){ Condition = whileStatement.Condition,
                                             InitAssign = initStatement, 
                                             IterationStatement = iteration, 
                                             Body = whileBody
                            });

                        block.Instructions.RemoveAt(i + 2);
                        block.Instructions.RemoveAt(i+1);
                        whileBody.Instructions.RemoveAt(whileBody.Instructions.Count - 1);
                    }
                }
            }
        }
    }
}
