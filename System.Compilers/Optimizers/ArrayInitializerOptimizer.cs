using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Compilers.Optimizers
{
    class ArrayInitializerOptimizer:Optimizer
    {
        MethodInfo methodInfo;

        public override void Optimize(NetAstBlock toOptimize)
        {
            methodInfo = typeof(RuntimeHelpers).GetMethod("InitializeArray");

            foreach (var block in toOptimize.GetSelfAndChildrenRecursive<NetAstBlock>())
            {
                InlineArrayInitializer(block);
                InlineMDArrayInitializer(block);
            }
        }

        private void InlineMDArrayInitializer(NetAstBlock block)
        {
            for (int i = 1; i < block.Instructions.Count - 1; i++)
            {
                var initCall = (block.Instructions[i] as NetAstExpressionStatement != null) ? (block.Instructions[i] as NetAstExpressionStatement).Expression as NetAstMethodCallExpression : null;
                var arrayAssign = block.Instructions[i - 1] as NetAstAssignamentStatement;
                //var realArray = block.Instructions[i - 1] as NetAstAssignament;
                if (initCall != null && arrayAssign != null && initCall.Arguments.Length>0 && initCall.Arguments[0] is NetAstLocalExpression &&
                    ((NetAstLocalExpression)initCall.Arguments[0]).LocalInfo.Equals(((NetAstLocalExpression)arrayAssign.LeftValue).LocalInfo) &&
                    arrayAssign.Value is NetAstConstructorCallExpression
                    )
                {
                    var arrayCreation = arrayAssign.Value as NetAstConstructorCallExpression;
                    var arrayDimensions = arrayCreation.Arguments.Select( a => (int)((NetAstConstantExpression)a).Value).ToArray();
                    var array = Array.CreateInstance(arrayCreation.ConstructorInfo.DeclaringType.GetElementType(), arrayDimensions);

                    var fieldHandle = (RuntimeFieldHandle)((NetAstConstantExpression)initCall.Arguments[1]).Value;

                    // Initialize
                    RuntimeHelpers.InitializeArray(array, fieldHandle);

                    NetInitMDArrayExpression arrayInitializer = new NetInitMDArrayExpression() { ArraySizes = arrayDimensions.Select(e => new NetAstConstantExpression( e )).ToArray(), ArrayType = array.GetType() };
                    arrayInitializer.InitValues = Array.CreateInstance(typeof(NetAstConstantExpression), arrayDimensions);
                    
                    int totalPositions = 1;
                    for (int j = 0; j < arrayDimensions.Length; j++)
                        totalPositions *= arrayDimensions[j];

                    int[] indices = new int[arrayDimensions.Length];
                    for (int j = 0; j < totalPositions; j++)
                    {
                        int divisor = totalPositions / arrayDimensions[0];
                        int reminder = j;
                        for (int k = 0; k < arrayDimensions.Length; k++)
                        {
                            indices[k] = reminder / divisor;
                            reminder = reminder % divisor;

                            if (k < arrayDimensions.Length - 1)
                                divisor /= arrayDimensions[k + 1];
                        }

                        arrayInitializer.InitValues.SetValue(new NetAstConstantExpression(array.GetValue(indices)), indices);
                    }

                    block.Instructions.RemoveAt(i);
                    arrayAssign.Value = arrayInitializer;
                }
            }
        }

        public void InlineArrayInitializer(NetAstBlock block)
        {
            for (int i = 1; i < block.Instructions.Count - 1; i++)
            {
                var initCall = (block.Instructions[i] as NetAstExpressionStatement != null) ? (block.Instructions[i] as NetAstExpressionStatement).Expression as NetAstMethodCallExpression : null;
                var arrayAssign = block.Instructions[i - 1] as NetAstAssignamentStatement;
                //var realArray = block.Instructions[i - 1] as NetAstAssignament;
                if (initCall != null && arrayAssign != null && initCall.Arguments.Length > 0 && initCall.Arguments[0] is NetAstLocalExpression &&
                    ((NetAstLocalExpression)initCall.Arguments[0]).LocalInfo.Equals(((NetAstLocalExpression)arrayAssign.LeftValue).LocalInfo) &&
                    arrayAssign.Value is NetInitArrayExpression
                    )
                {
                    var arrayInitializer = arrayAssign.Value as NetInitArrayExpression;
                    var arraySize = arrayInitializer.ArraySizeAsInt;
                    var array = Array.CreateInstance(arrayInitializer.ArrayType, arraySize);

                    var fieldHandle = (RuntimeFieldHandle)((NetAstConstantExpression)initCall.Arguments[1]).Value;
                    
                    //Initialize 
                    RuntimeHelpers.InitializeArray(array, fieldHandle);

                    arrayInitializer.InitValues = new NetAstConstantExpression[arraySize];
                    for (int j = 0; j < arraySize; j++)
                        arrayInitializer.InitValues[j] = new NetAstConstantExpression(array.GetValue(j) );
                    //arrayAssign.Value = arrayInitializer;
                    
                    block.Instructions.RemoveAt(i);
                    //block.Instructions.RemoveAt(i - 2);
                }
            }
        }
    }
}
