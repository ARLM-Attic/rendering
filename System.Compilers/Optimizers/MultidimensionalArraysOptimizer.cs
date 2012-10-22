using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;

namespace System.Compilers.Optimizers
{
    class MultidimensionalArraysOptimizer:Optimizer
    {
        public override void Optimize(NetAstBlock toOptimize)
        {
            var result  = toOptimize.GetSelfAndChildrenRecursive<NetAstNode>().ToList();
            foreach (var node in result)
            {
                AnalyzeOperands(node);
            }

            foreach (var block in toOptimize.GetSelfAndChildrenRecursive<NetAstBlock>())
            {
                AnalyseStatements(block.Instructions);
            }
        }

        void AnalyzeOperands(NetAstNode node)
        {
            var operands = node.GetOperands();

            for (int i = 0; i < operands.Length; i++)
            {
                var current = operands[i];
                if (current is NetAstMethodBaseCallExpression )
                {
                    var method = current as NetAstMethodCallExpression;
                    if (method != null && !method.MethodInfo.IsStatic && 
                        method.MethodInfo.DeclaringType.IsArray && 
                        method.MethodInfo.DeclaringType.GetArrayRank() > 1)
                    {
                        var getMethodInfo = method.MethodInfo.DeclaringType.GetMethod("Get");
                        var indices = new NetAstExpression[method.MethodInfo.GetParameters().Length];
                        Array.Copy(method.Arguments, 1, indices, 0, indices.Length);
                        if (method.MethodInfo.Equals(getMethodInfo))
                            node.SetOperandAt(i, new NetMDArrayAccessExpression() { ArrayExpression = method.Arguments[0], IndicesExpressions = indices });
                    }

                    var ctor = current as NetAstConstructorCallExpression;
                    if (ctor != null && ctor.ConstructorInfo.DeclaringType.IsArray && ctor.ConstructorInfo.DeclaringType.GetArrayRank() > 1)
                        node.SetOperandAt(i, new NetInitMDArrayExpression() { ArraySizes = ctor.Arguments, ArrayType = ctor.ConstructorInfo.DeclaringType });
                }
            }
        }

        void AnalyseStatements(List<NetAstStatement> instructions)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                var statement = instructions[i] as NetAstExpressionStatement;
                if (statement != null)
                {
                    var current = statement.Expression as NetAstMethodCallExpression;
                    if (current != null)
                    {
                        var methodInfo = current.MethodInfo;
                        if (!methodInfo.IsStatic &&
                            methodInfo.DeclaringType.IsArray &&
                            methodInfo.DeclaringType.GetArrayRank() > 1)
                        {
                            var setMethodInfo = methodInfo.DeclaringType.GetMethod("Set");
                            var indices = new NetAstExpression[methodInfo.GetParameters().Length - 1];
                            Array.Copy(current.Arguments, 1, indices, 0, indices.Length);
                            if (current.MethodInfo.Equals(setMethodInfo))
                                instructions[i] = new NetMDArrayElementAssignament() { ArrayExpression = current.Arguments[0], ArrayIndices = indices, Value = current.Arguments[current.Arguments.Length - 1] };
                        }
                    }
                }
            }
        }
    }
}
