using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;
using System.Reflection;

namespace System.Compilers.Optimizers
{
    public class InlineVariablesOptimizer:Optimizer
    {
        public override void Optimize(NetAstBlock toOptimize)
        {
            InlineVariables(toOptimize);
        }

        void InlineVariables(NetAstBlock method)
        {
            // Analyse the whole method
            Dictionary <NetLocalVariable, int> varAssignments = new Dictionary<NetLocalVariable, int>(new LocalVariableInfoEqComp());
            Dictionary <NetLocalVariable, int> varAccesses = new Dictionary<NetLocalVariable, int>(new LocalVariableInfoEqComp());

            foreach (var expr in method.GetSelfAndChildrenRecursive<NetAstNode>())
            {
                if (expr is NetAstAssignamentStatement && ((expr as NetAstAssignamentStatement).LeftValue is NetAstLocalExpression))
                {
                    var locVar = ((expr as NetAstAssignamentStatement).LeftValue as NetAstLocalExpression).LocalInfo;
                    if (locVar != null)
                        varAssignments[locVar] = varAssignments.GetOrDefault(locVar) + 1;
                }
                else if (expr is NetAstLocalExpression)
                {
                    var locVar = (expr as NetAstLocalExpression).LocalInfo;
                    if (locVar != null)
                        varAccesses[locVar] = varAccesses.GetOrDefault(locVar) + 1;
                }
            }

            // Inline all blocks
            foreach (var block in method.GetSelfAndChildrenRecursive<NetAstBlock>())
            {
                for (int i = 0; i < block.Instructions.Count; i++)
                {
                    var currentInst = block.Instructions[i];
                    var operands = currentInst.GetOperands().ToList();
                    var varOperands = operands.Where(o => o is NetAstLocalExpression).Cast<NetAstLocalExpression>();
                    foreach (var operand in varOperands.Where( e => varAccesses.GetOrDefault(e.LocalInfo) == 1 && varAssignments.GetOrDefault(e.LocalInfo ) == 1))
                    {
                        int assignIndex = block.Instructions.FindIndex(inst => inst is NetAstAssignamentStatement && ((NetAstAssignamentStatement)inst).LeftValue is NetAstLocalExpression &&((inst as NetAstAssignamentStatement).LeftValue as NetAstLocalExpression).LocalInfo.Equals(operand.LocalInfo));
                        int operandIndex = operands.IndexOf(operand);
                        if (assignIndex >= 0 && operandIndex >= 0)
                        {
                            var assign = block.Instructions[assignIndex] as NetAstAssignamentStatement;
                            currentInst.SetOperandAt(operandIndex, assign.Value);
                            block.Instructions.RemoveAt(assignIndex);
                        }
                    }
                }
            }
        }

        private bool TryInline(NetLocalVariable locVar, NetAstExpression expr, NetAstNode nextExpr)
        {
            if (nextExpr is NetAstConditionalGoto)
            {
                var condGoto = nextExpr as NetAstConditionalGoto;
                var condExpr = condGoto.Condition as NetAstLocalExpression;
                if (condExpr != null && condExpr.LocalInfo.Name == locVar.Name)
                {
                    condGoto.Condition = expr;
                    return true;
                }
            }
            else if (nextExpr is NetAstReturn)
            {
                var ret = nextExpr as NetAstReturn;
                var retExpr = ret.Expression as NetAstLocalExpression;
                if (retExpr != null && retExpr.LocalInfo.Name == locVar.Name)
                {
                    ret.Expression = expr;
                    return true;
                }
            }
            return false;
        }
    }

    class LocalVariableInfoEqComp : IEqualityComparer<NetLocalVariable>
    {
        public bool Equals(NetLocalVariable x, NetLocalVariable y)
        {
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(NetLocalVariable obj)
        {
            return obj.Name.GetHashCode();
        }
    }

}
