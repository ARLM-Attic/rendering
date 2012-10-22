using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.IL;
using System.Reflection;
using System.Compilers.AST;
using System.Reflection.Emit;
using System.Compilers.Optimizers;
using System.Runtime.CompilerServices;

namespace System.Compilers.IL
{

    public static class ILDecompiler
    {
        static ExecutionState[] EmptyExecStateArray = new ExecutionState[] { };

        static Stack<NetAstExpression> addressCalls = new Stack<NetAstExpression>();
        static Dictionary<NetAstExpression, NetAstExpression> adressExpressions = new Dictionary<NetAstExpression, NetAstExpression>();

        static Dictionary<MethodBase, NetAstBlock> cache = new Dictionary<MethodBase, NetAstBlock>();

        public static NetAstBlock GetMethodBody(MethodBase methodInfo)
        {
            if (!cache.ContainsKey(methodInfo))
            {

                var e = ILDecompiler.GetBasicNetAstFrom(methodInfo);

                new TrivialGotoRemoverOptimizer().Optimize(e);
                new InlineVariablesOptimizer().Optimize(e);
                new SplitToBlocksOptimizer().Optimize(e);
                new ShortCircuitAndTernaryOperatorOptimizer().Optimize(e);
                new LoopsOptimizer().Optimize(e);
                new ConditionalsOptimizer().Optimize(e);
                new FlattenBlocksOptimizer().Optimize(e);
                new TrivialGotoRemoverOptimizer().Optimize(e);
                new ArrayInitializerOptimizer().Optimize(e);
                new MultidimensionalArraysOptimizer().Optimize(e);
                new InlineVariablesOptimizer().Optimize(e);
                new GotoRemovalOptimizer().Optimize(e);
                new LoopsFormatterOptimizer().Optimize(e);

                NormalizeExpressions(e);
                AddVariableDeclarations(e);

                if (cache.Count > 500)
                    cache.Clear();

                cache.Add(methodInfo, e);
            }

            return cache[methodInfo];
        }

        private static void NormalizeExpressions(NetAstBlock e)
        {
            foreach (var expr in e.GetSelfAndChildrenRecursive<NetAstExpression>())
            {
                expr.Normalize();
                expr.SetPrecedences(0);
            }

            foreach (var item in e.GetSelfAndChildrenRecursive<NetAstNode>())
            {
                var operands = item.GetOperands();
                for (int i = 0; i < operands.Length; i++)
                {
                    var unaryOperation = operands[i] as NetAstUnaryOperatorExpression;
                    if (unaryOperation != null && unaryOperation.Operator == Operators.None)
                        item.SetOperandAt(i, unaryOperation.Operand);
                }
            }
        }

        private static void AddVariableDeclarations(NetAstBlock ilMethod)
        {
            List<NetLocalVariable> variables = new List<NetLocalVariable>();
            var assigned = ilMethod.GetSelfAndChildrenRecursive<NetAstAssignamentStatement>().Where(a=>a.LeftValue is NetAstLocalExpression).Select(e => ((NetAstLocalExpression)e.LeftValue).LocalInfo).Distinct();
            var accessed = ilMethod.GetSelfAndChildrenRecursive<NetAstLocalExpression>().Select(e => e.LocalInfo).Distinct();
            variables.AddRange(assigned.Union(accessed).Distinct());
            foreach (var item in variables)
            {
                ilMethod.Instructions.Insert(0, new NetAstLocalDeclaration() { LocalInfo = item });
            }
        }

        private static NetAstBlock GetBasicNetAstFrom(MethodBase methodInfo)
        {
            ILReader reader = new ILReader(methodInfo.Module.Assembly);

            var il = reader.GetIL(methodInfo).ToArray();

            var execStates = GetExecutionStatesFromIL(il, methodInfo, methodInfo.GetMethodBody());

            var expressions = ConvertToILExpressions(execStates);

            InlineVariables(expressions);

            TypeAnalysis.Run(methodInfo, expressions);

            var block = new NetAstBlock() { Instructions = ConvertToNetAstNode(methodInfo, expressions) };

            //Console.BufferHeight = 100;
            foreach (var i in block.Instructions.ToArray())
                if (i.ToString().Contains("_CAP"))
                    block.Instructions.Remove(i);
            //foreach (var i in block.Instructions.ToArray())
            //    Console.WriteLine(i);

            return block;
        }

        static List<ExecutionState> GetExecutionStatesFromIL(ILInstruction[] instructions, MethodBase methodInfo, MethodBody methodBody)
        {
            var instrToExecState = new Dictionary<ILInstruction, ExecutionState>();
            var body = new List<ExecutionState>(instructions.Length);
            List<ILInstruction> prefixes = null;

            #region Create temporary structure for the stack analysis
            foreach (var inst in instructions)
            {
                if (inst.OpCode.OpCodeType == OpCodeType.Prefix)
                {
                    if (prefixes == null)
                        prefixes = new List<ILInstruction>(1);
                    prefixes.Add(inst);
                    continue;
                }
                OpCodeCodes code = (OpCodeCodes)inst.OpCode.Value;
                
                object operand = inst.Operand;
                ILTools.Expand(inst, instructions, ref code, ref operand, methodInfo, methodBody);
                ExecutionState execState = new ExecutionState()
                {
                    Offset = inst.Address,
                    Code = code,
                    Operand = operand,
                    PopCount = inst.CountOfPops(),
                    PushCount = inst.CountOfPushes()
                };
                if (prefixes != null)
                {
                    instrToExecState[prefixes[0]] = execState;
                    execState.Offset = prefixes[0].Address;
                    execState.Prefixes = prefixes.ToArray();
                    prefixes = null;
                }
                else
                {
                    instrToExecState[inst] = execState;
                }
                body.Add(execState);
            }
            for (int i = 0; i < body.Count - 1; i++)
            {
                body[i].Next = body[i + 1];
            }
            #endregion

            #region Assign stack and variable states to instructions according to code flow.
            int varCount = methodBody.LocalVariables.Count;

            body[0].StackBefore = new List<StackSlot>();
            body[0].VariablesBefore = VariableSlot.MakeEmptyState(varCount);
            
            Stack<ExecutionState> agenda = new Stack<ExecutionState>();
            agenda.Push(body[0]);

            while (agenda.Count > 0)
            {
                var currentState = agenda.Pop();

                // Calculate new stack
                List<StackSlot> newStack = StackSlot.CloneStack(currentState.StackBefore, currentState.PopCount);
                for (int i = 0; i < currentState.PushCount; i++)
                    newStack.Add(new StackSlot(currentState));
                
                // Calculate new variable state
                VariableSlot[] newVariableState = VariableSlot.CloneVariableState(currentState.VariablesBefore);
                if (currentState.Code == OpCodeCodes.Stloc)
                {
                    int varIndex = ((LocalVariableInfo)currentState.Operand).LocalIndex;
                    newVariableState[varIndex] = new VariableSlot(currentState);
                }

                // Find all successors
                var branchTargets = new List<ExecutionState>();
                if (currentState.Code.CanFallThough())
                {
                    branchTargets.Add(currentState.Next);
                }
                if (currentState.Operand is ILInstruction[])
                {
                    foreach (var inst in (ILInstruction[])currentState.Operand)
                    {
                        ExecutionState target = instrToExecState[inst];
                        branchTargets.Add(target);
                        // The target of a branch must have label
                        if (target.Label == null)
                        {
                            target.Label = new ILLabel() { Name = target.Name };
                        }
                    }
                }
                else if (currentState.Operand is ILInstruction)
                {
                    ExecutionState target = instrToExecState[(ILInstruction)currentState.Operand];
                    branchTargets.Add(target);
                    // The target of a branch must have label
                    if (target.Label == null)
                    {
                        target.Label = new ILLabel() { Name = target.Name };
                    }
                }

                // Apply the state to successors
                foreach (ExecutionState branchTarget in branchTargets)
                {
                    if (branchTarget.StackBefore == null && branchTarget.VariablesBefore == null)
                    {
                        if (branchTargets.Count == 1)
                        {
                            branchTarget.StackBefore = newStack;
                            branchTarget.VariablesBefore = newVariableState;
                        }
                        else
                        {
                            // Do not share data for several ExecutionStates
                            branchTarget.StackBefore = StackSlot.CloneStack(newStack, 0);
                            branchTarget.VariablesBefore = VariableSlot.CloneVariableState(newVariableState);
                        }
                        agenda.Push(branchTarget);
                    }
                    else
                    {
                        if (branchTarget.StackBefore.Count != newStack.Count)
                        {
                            throw new Exception("Inconsistent stack size at " + currentState.Name);
                        }

                        // Be careful not to change our new data - it might be reused for several branch targets.
                        // In general, be careful that two ExecutionStates never share data structures.

                        bool modified = false;

                        // Merge stacks - modify the target
                        for (int i = 0; i < newStack.Count; i++)
                        {
                            ExecutionState[] oldPushedBy = branchTarget.StackBefore[i].PushedBy;
                            ExecutionState[] newPushedBy = oldPushedBy.Union(newStack[i].PushedBy).ToArray();
                            if (newPushedBy.Length > oldPushedBy.Length)
                            {
                                branchTarget.StackBefore[i] = new StackSlot(newPushedBy, null);
                                modified = true;
                            }
                        }

                        // Merge variables - modify the target
                        for (int i = 0; i < newVariableState.Length; i++)
                        {
                            VariableSlot oldSlot = branchTarget.VariablesBefore[i];
                            VariableSlot newSlot = newVariableState[i];
                            // All can not be unioned further
                            if (!oldSlot.StoredByAll)
                            {
                                if (newSlot.StoredByAll)
                                {
                                    branchTarget.VariablesBefore[i] = newSlot;
                                    modified = true;
                                }
                                else
                                {
                                    ExecutionState[] oldStoredBy = oldSlot.StoredBy;
                                    ExecutionState[] newStoredBy = oldStoredBy.Union(newSlot.StoredBy).ToArray();
                                    if (newStoredBy.Length > oldStoredBy.Length)
                                    {
                                        branchTarget.VariablesBefore[i] = new VariableSlot(newStoredBy, false);
                                        modified = true;
                                    }
                                }
                            }
                        }

                        if (modified)
                        {
                            agenda.Push(branchTarget);
                        }
                    }
                }
            }
            #endregion

            #region Genertate temporary variables to replace stack
            var unreacheble = new List<ExecutionState>();
            foreach (ExecutionState execState in body)
            {
                if (execState.StackBefore == null)
                {
                    unreacheble.Add(execState);
                    continue;
                }
                int argIdx = 0;
                int popCount = execState.PopCount ?? execState.StackBefore.Count;
                for (int i = execState.StackBefore.Count - popCount; i < execState.StackBefore.Count; i++)
                {
                    var tmpVar = new NetLocalVariable() { Name = string.Format("arg_{0:X2}_{1}", execState.Offset, argIdx), IsGenerated = true };
                    execState.StackBefore[i] = new StackSlot(execState.StackBefore[i].PushedBy, tmpVar);
                    foreach (ExecutionState pushedBy in execState.StackBefore[i].PushedBy)
                    {
                        if (pushedBy.StoreTo == null)
                        {
                            pushedBy.StoreTo = new List<NetLocalVariable>(1);
                        }
                        pushedBy.StoreTo.Add(tmpVar);
                    }
                    argIdx++;
                }
            }

            foreach (var item in unreacheble)
            {
                body.Remove(item);
            }
            #endregion

            #region Convert the local variables
            ConvertLocalVariables(body, methodInfo, methodBody);
            #endregion

            #region Convert branch targets to labels
            foreach (var executionState in body)
            {
                if (executionState.Operand is ILInstruction[])
                {
                    List<ILLabel> newOperand = new List<ILLabel>();
                    foreach (var target in (ILInstruction[])executionState.Operand)
                    {
                        newOperand.Add(instrToExecState[target].Label);
                    }
                    executionState.Operand = newOperand.ToArray();
                }
                else if (executionState.Operand is ILInstruction)
                {
                    executionState.Operand = instrToExecState[(ILInstruction)executionState.Operand].Label;
                }
            }
            #endregion

            return body;
        }

        static List<ILNode> ConvertToILExpressions(List<ExecutionState> body)
        {
            List<ILNode> ast = new List<ILNode>();

            // Convert stack-based IL code to ILAst tree
            foreach (ExecutionState execState in body)
            {
                ILExpression expr = new ILExpression(execState.Code, execState.Operand);
                expr.Prefixes = execState.Prefixes;

                // Label for this instruction
                if (execState.Label != null)
                {
                    ast.Add(execState.Label);
                }

                // Reference arguments using temporary variables
                int popCount = execState.PopCount ?? execState.StackBefore.Count;
                for (int i = execState.StackBefore.Count - popCount; i < execState.StackBefore.Count; i++)
                {
                    StackSlot slot = execState.StackBefore[i];
                    expr.Arguments.Add(new ILExpression(OpCodeCodes.Ldloc, slot.LoadFrom));
                }

                // Store the result to temporary variable(s) if needed
                if (execState.StoreTo == null || execState.StoreTo.Count == 0)
                {
                    ast.Add(expr);
                }
                else if (execState.StoreTo.Count == 1)
                {
                    ast.Add(new ILExpression(OpCodeCodes.Stloc, execState.StoreTo[0], expr));
                }
                else
                {
                    NetLocalVariable tmpVar = new NetLocalVariable() { Name = "expr" + execState.Offset.ToString("X2"), IsGenerated = true };
                    ast.Add(new ILExpression(OpCodeCodes.Stloc, tmpVar, expr));
                    foreach (NetLocalVariable storeTo in execState.StoreTo.AsEnumerable().Reverse())
                    {
                        ast.Add(new ILExpression(OpCodeCodes.Stloc, storeTo, new ILExpression(OpCodeCodes.Ldloc, tmpVar)));
                    }
                }
            }

            return ast;
        }

        private static List<NetAstStatement> ConvertToNetAstNode(MethodBase methodInfo, List<ILNode> body)
        {
            List<NetAstStatement> result = new List<NetAstStatement>(body.Count);

            for (int i = 0; i < body.Count; i++)
            {
                var statement = ConvertToNetStatement(body[i], methodInfo);
                if (statement != null)
                    result.Add(statement);
            }
            return result;
        }

        private static NetAstStatement ConvertToNetStatement(ILNode node, MethodBase methodInfo)
        {
            if (node is ILLabel)
                return new NetAstLabel() { Name = ((ILLabel)node).Name };
            ILExpression iLExpression = (ILExpression)node;
            NetAstExpression[] ops = new NetAstExpression[0] ;
            if (iLExpression.Arguments != null)
            {
                var argsList = iLExpression.Arguments.Select((a) => ConvertToNetExpression(a, methodInfo, null)).ToArray();
                var allOps = new List<NetAstExpression>();
                foreach (var item in argsList)
                    allOps.AddRange(item);
                ops = allOps.ToArray();
            }
            return CreateStatementNodeFor(iLExpression, ops, methodInfo);
        }

        private static IEnumerable<NetAstExpression> ConvertToNetExpression(ILExpression iLExpression, MethodBase methodInfo, Type expectedType)
        {
            NetAstExpression[] ops = new NetAstExpression[0];
            if (iLExpression.Arguments != null)
            {
                var argsList = iLExpression.Arguments.Select((a) => ConvertToNetExpression(a, methodInfo, expectedType)).ToArray();
                var allOps = new List<NetAstExpression>();
                foreach (var item in argsList)
                    allOps.AddRange(item);
                ops = allOps.ToArray();
            }
            return CreateExpressionNodeFor(iLExpression, ops, methodInfo, expectedType);
        }

        private static void ConvertLocalVariables(List<ExecutionState> body, MethodBase methodInfo, MethodBody methodBody)
        {
            #region Optimization
            int varCount = methodBody.LocalVariables.Count;

            for (int variableIndex = 0; variableIndex < varCount; variableIndex++)
            {
                // Find all stores and loads for this variable
                List<ExecutionState> stores = body.Where(b => b.Code == OpCodeCodes.Stloc && b.Operand is LocalVariableInfo && b.OperandAsVariable.LocalIndex == variableIndex).ToList();
                List<ExecutionState> loads = body.Where(b => (b.Code == OpCodeCodes.Ldloc || b.Code == OpCodeCodes.Ldloca) && b.Operand is LocalVariableInfo && b.OperandAsVariable.LocalIndex == variableIndex).ToList();
                Type varType = methodBody.LocalVariables[variableIndex].LocalType;

                List<VariableInfo> newVars;

                // If any of the loads is from "all", use single variable
                // If any of the loads is ldloca, fallback to single variable as well
                if (loads.Any(b => b.VariablesBefore[variableIndex].StoredByAll || b.Code == OpCodeCodes.Ldloca))
                {
                    newVars = new List<VariableInfo>(1) { new VariableInfo() {
							Variable = new NetLocalVariable() {
								Name = "var" + variableIndex,
						    		Type = varType,
						    		OriginalVariable = methodBody.LocalVariables[variableIndex]
							},
							Stores = stores,
							Loads  = loads
						}};
                }
                else
                {
                    // Create a new variable for each store
                    newVars = stores.Select(st => new VariableInfo()
                    {
                        Variable = new NetLocalVariable()
                        {
                            Name = "var" + variableIndex + "" + st.Offset.ToString("X2"),
                            Type = varType,
                            OriginalVariable = methodBody.LocalVariables[variableIndex]
                        },
                        Stores = new List<ExecutionState>() { st },
                        Loads = new List<ExecutionState>()
                    }).ToList();

                    // Add loads to the data structure; merge variables if necessary
                    foreach (ExecutionState load in loads)
                    {
                        ExecutionState[] storedBy = load.VariablesBefore[variableIndex].StoredBy;
                        if (storedBy.Length == 0)
                        {
                            throw new Exception("Load of uninitialized variable");
                        }
                        else if (storedBy.Length == 1)
                        {
                            VariableInfo newVar = newVars.Where(v => v.Stores.Contains(storedBy[0])).Single();
                            newVar.Loads.Add(load);
                        }
                        else
                        {
                            List<VariableInfo> mergeVars = newVars.Where(v => v.Stores.Union(storedBy).Any()).ToList();
                            VariableInfo mergedVar = new VariableInfo()
                            {
                                Variable = mergeVars[0].Variable,
                                Stores = mergeVars.SelectMany(v => v.Stores).ToList(),
                                Loads = mergeVars.SelectMany(v => v.Loads).ToList()
                            };
                            mergedVar.Loads.Add(load);
                            newVars = newVars.Except(mergeVars).ToList();
                            newVars.Add(mergedVar);
                        }
                    }
                }

                // Set bytecode operands
                foreach (VariableInfo newVar in newVars)
                {
                    foreach (ExecutionState store in newVar.Stores)
                    {
                        store.Operand = newVar.Variable;
                    }
                    foreach (ExecutionState load in newVar.Loads)
                    {
                        load.Operand = newVar.Variable;
                    }
                }
            }
            #endregion

            #region No optimization
            //var variables = methodBody.LocalVariables.Select(v => new NetLocalVariable() { Name = "var_" + v.LocalIndex , Type = v.LocalType, OriginalVariable = v }).ToList();
            //foreach (var execState in body)
            //{
            //    if (execState.Code == OpCodeCodes.Ldloc || execState.Code == OpCodeCodes.Stloc || execState.Code == OpCodeCodes.Ldloca)
            //    {
            //        int index = ((LocalVariableInfo)execState.Operand).LocalIndex;
            //        execState.Operand = variables[index];
            //    }
            //}
            #endregion
        }

        private static IEnumerable<NetAstExpression> CreateExpressionNodeFor(ILExpression instruction, NetAstExpression[] ops, MethodBase methodInfo, Type expetedType)
        {
            OpCodeCodes opCode = instruction.Code;

            switch (opCode)
            {
                #region Operations

                #region Binary Operations

                case OpCodeCodes.Add:
                case OpCodeCodes.Add_Ovf:
                case OpCodeCodes.Add_Ovf_Un:
                case OpCodeCodes.Sub:
                case OpCodeCodes.Sub_Ovf:
                case OpCodeCodes.Sub_Ovf_Un:
                case OpCodeCodes.Mul:
                case OpCodeCodes.Mul_Ovf:
                case OpCodeCodes.Mul_Ovf_Un:
                case OpCodeCodes.Div:
                case OpCodeCodes.Div_Un:
                case OpCodeCodes.Rem:
                case OpCodeCodes.Rem_Un:
                case OpCodeCodes.And:
                case OpCodeCodes.Or:
                case OpCodeCodes.Xor:
                case OpCodeCodes.Ceq:
                case OpCodeCodes.Cgt:
                case OpCodeCodes.Cgt_Un:
                case OpCodeCodes.Clt:
                case OpCodeCodes.Clt_Un:
                    {
                        Operators op = Operators.None;
                        switch (opCode)
                        {
                            case OpCodeCodes.Add:
                            case OpCodeCodes.Add_Ovf:
                            case OpCodeCodes.Add_Ovf_Un:
                                op = Operators.Addition;
                                break;
                            case OpCodeCodes.Sub:
                            case OpCodeCodes.Sub_Ovf:
                            case OpCodeCodes.Sub_Ovf_Un:
                                op = Operators.Subtraction;
                                break;
                            case OpCodeCodes.Mul:
                            case OpCodeCodes.Mul_Ovf:
                            case OpCodeCodes.Mul_Ovf_Un:
                                op = Operators.Multiply;
                                break;
                            case OpCodeCodes.Div:
                            case OpCodeCodes.Div_Un:
                                op = Operators.Division;
                                break;
                            case OpCodeCodes.Rem:
                            case OpCodeCodes.Rem_Un:
                                op = Operators.Modulus;
                                break;
                            case OpCodeCodes.And:
                                op = Operators.LogicAnd;
                                break;
                            case OpCodeCodes.Or:
                                op = Operators.LogicOr;
                                break;
                            case OpCodeCodes.Xor:
                                op = Operators.LogicXor;
                                break;
                            case OpCodeCodes.Ceq:
                                op = Operators.Equality;
                                break;
                            case OpCodeCodes.Cgt:
                            case OpCodeCodes.Cgt_Un:
                                op = Operators.GreaterThan;
                                break;
                            case OpCodeCodes.Clt:
                            case OpCodeCodes.Clt_Un:
                                op = Operators.LessThan;
                                break;
                        }
                        if ((op == Operators.Equality || op == Operators.Inequality) &&
                            ops[0] == null || ops[1] == null)
                        {
                            yield return new NetAstNullComparitionExpression() { Operator = op, Operand = ops[0] ?? ops[1] };
                            yield break;
                        }

                        if ((op == Operators.Equality || op == Operators.Inequality) &&
                            (ops[0].StaticType != ops[1].StaticType))
                        {
                            yield return ResolveBoolComparison(op, ops[0], ops[1]);
                            yield break;
                        }
                        yield return new NetAstBinaryOperatorExpression() { Operator = op, RightOperand = ops[1], LeftOperand = ops[0] };
                        yield break;
                    }
                #endregion

                #region Unary operations

                case OpCodeCodes.Neg:
                case OpCodeCodes.Not:
                    {
                        Operators op = Operators.None;
                        switch ((OpCodeCodes)opCode)
                        {
                            case OpCodeCodes.Neg:
                                op = Operators.UnaryNegation;
                                break;
                            case OpCodeCodes.Not:
                                op = Operators.Not;
                                break;
                        }
                        yield return new NetAstUnaryOperatorExpression() { Operator = op, Operand = ops[0] };
                        yield break;
                    }

                #region Cast

                case OpCodeCodes.Castclass:
                case OpCodeCodes.Conv_I:
                case OpCodeCodes.Conv_I1:
                case OpCodeCodes.Conv_I2:
                case OpCodeCodes.Conv_I4:
                case OpCodeCodes.Conv_I8:
                case OpCodeCodes.Conv_Ovf_I:
                case OpCodeCodes.Conv_Ovf_I_Un:
                case OpCodeCodes.Conv_Ovf_I1:
                case OpCodeCodes.Conv_Ovf_I1_Un:
                case OpCodeCodes.Conv_Ovf_I2:
                case OpCodeCodes.Conv_Ovf_I2_Un:
                case OpCodeCodes.Conv_Ovf_I4:
                case OpCodeCodes.Conv_Ovf_I4_Un:
                case OpCodeCodes.Conv_Ovf_I8:
                case OpCodeCodes.Conv_Ovf_I8_Un:
                case OpCodeCodes.Conv_Ovf_U:
                case OpCodeCodes.Conv_Ovf_U_Un:
                case OpCodeCodes.Conv_Ovf_U1:
                case OpCodeCodes.Conv_Ovf_U1_Un:
                case OpCodeCodes.Conv_Ovf_U2:
                case OpCodeCodes.Conv_Ovf_U2_Un:
                case OpCodeCodes.Conv_Ovf_U4:
                case OpCodeCodes.Conv_Ovf_U4_Un:
                case OpCodeCodes.Conv_Ovf_U8:
                case OpCodeCodes.Conv_Ovf_U8_Un:
                case OpCodeCodes.Conv_R_Un:
                case OpCodeCodes.Conv_R4:
                case OpCodeCodes.Conv_R8:
                case OpCodeCodes.Conv_U:
                case OpCodeCodes.Conv_U1:
                case OpCodeCodes.Conv_U2:
                case OpCodeCodes.Conv_U4:
                case OpCodeCodes.Conv_U8:

                case OpCodeCodes.Unbox:
                case OpCodeCodes.Unbox_Any:
                    {
                        Type targetType = null;

                        switch (opCode)
                        {
                            case OpCodeCodes.Unbox_Any:
                            case OpCodeCodes.Unbox:
                            case OpCodeCodes.Castclass:
                                targetType = instruction.Operand as Type;
                                break;
                            case OpCodeCodes.Conv_I1:
                            case OpCodeCodes.Conv_Ovf_I1:
                            case OpCodeCodes.Conv_Ovf_I1_Un:
                                targetType = typeof(sbyte);
                                break;
                            case OpCodeCodes.Conv_I2:
                            case OpCodeCodes.Conv_Ovf_I2:
                            case OpCodeCodes.Conv_Ovf_I2_Un:
                                targetType = typeof(short);
                                break;
                            case OpCodeCodes.Conv_I4:
                            case OpCodeCodes.Conv_I:
                            case OpCodeCodes.Conv_Ovf_I:
                            case OpCodeCodes.Conv_Ovf_I4:
                            case OpCodeCodes.Conv_Ovf_I4_Un:
                                targetType = typeof(int);
                                break;
                            case OpCodeCodes.Conv_I8:
                            case OpCodeCodes.Conv_Ovf_I8:
                            case OpCodeCodes.Conv_Ovf_I8_Un:
                                targetType = typeof(long);
                                break;
                            case OpCodeCodes.Conv_U1:
                            case OpCodeCodes.Conv_Ovf_U1:
                            case OpCodeCodes.Conv_Ovf_U1_Un:
                                targetType = typeof(byte);
                                break;
                            case OpCodeCodes.Conv_U2:
                            case OpCodeCodes.Conv_Ovf_U2:
                            case OpCodeCodes.Conv_Ovf_U2_Un:
                                targetType = typeof(ushort);
                                break;
                            case OpCodeCodes.Conv_U:
                            case OpCodeCodes.Conv_U4:
                            case OpCodeCodes.Conv_Ovf_U:
                            case OpCodeCodes.Conv_Ovf_U_Un:
                            case OpCodeCodes.Conv_Ovf_U4:
                            case OpCodeCodes.Conv_Ovf_U4_Un:
                                targetType = typeof(uint);
                                break;
                            case OpCodeCodes.Conv_U8:
                            case OpCodeCodes.Conv_Ovf_U8:
                            case OpCodeCodes.Conv_Ovf_U8_Un:
                                targetType = typeof(ulong);
                                break;
                            case OpCodeCodes.Conv_R_Un:
                            case OpCodeCodes.Conv_R4:
                                targetType = typeof(float);
                                break;
                            case OpCodeCodes.Conv_R8:
                                targetType = typeof(double);
                                break;
                            default: throw new NotImplementedException("Its missing " + opCode);
                        }

                        yield return new NetAstConvertOperatorExpression() { TargetType = targetType, Operand = ops[0] };
                        yield break;
                    }

                #endregion

                #endregion

                #endregion

                #region Constants

                case OpCodeCodes.Ldc_I4: yield return new NetAstConstantExpression((expetedType != null && expetedType == typeof(bool)) ? (object)Convert.ToBoolean(instruction.Operand) : (int)instruction.Operand); yield break;
                case OpCodeCodes.Ldc_I4_0: yield return new NetAstConstantExpression((expetedType != null && expetedType == typeof(bool)) ? (object)false : 0); yield break;
                case OpCodeCodes.Ldc_I4_1: yield return new NetAstConstantExpression((expetedType != null && expetedType == typeof(bool)) ? (object)false : 1); yield break;
                case OpCodeCodes.Ldc_I4_2: yield return new NetAstConstantExpression(2); yield break;
                case OpCodeCodes.Ldc_I4_3: yield return new NetAstConstantExpression(3); yield break;
                case OpCodeCodes.Ldc_I4_4: yield return new NetAstConstantExpression(4); yield break;
                case OpCodeCodes.Ldc_I4_5: yield return new NetAstConstantExpression(5); yield break;
                case OpCodeCodes.Ldc_I4_6: yield return new NetAstConstantExpression(6); yield break;
                case OpCodeCodes.Ldc_I4_7: yield return new NetAstConstantExpression(7); yield break;
                case OpCodeCodes.Ldc_I4_8: yield return new NetAstConstantExpression(8); yield break;
                case OpCodeCodes.Ldc_I4_M1: yield return new NetAstConstantExpression(-1); yield break;
                case OpCodeCodes.Ldc_I4_S: yield return new NetAstConstantExpression((sbyte)instruction.Operand); yield break;
                case OpCodeCodes.Ldc_I8: yield return new NetAstConstantExpression((long)instruction.Operand); yield break;
                case OpCodeCodes.Ldc_R4: yield return new NetAstConstantExpression((float)instruction.Operand); yield break;
                case OpCodeCodes.Ldc_R8: yield return new NetAstConstantExpression((double)instruction.Operand); yield break;
                case OpCodeCodes.Ldstr: yield return new NetAstConstantExpression((string)instruction.Operand); yield break;
                #endregion

                #region Load Arguments
                case OpCodeCodes.Ldarg_0:
                    {
                        yield return new NetAstThisExpression();
                        yield break;
                    }

                case OpCodeCodes.Ldarg:
                case OpCodeCodes.Ldarga:
                case OpCodeCodes.Ldarga_S:
                case OpCodeCodes.Ldarg_S:
                    //case OpCodeCodes.Ldarg_1:
                    //case OpCodeCodes.Ldarg_2:
                    //case OpCodeCodes.Ldarg_3:
                    {
                        yield return new NetAstArgumentExpression() { ParameterInfo = (ParameterInfo)instruction.Operand };
                        yield break;
                    }
                #endregion

                #region Load Locals

                case OpCodeCodes.Ldloc:
                case OpCodeCodes.Ldloc_S:
                case OpCodeCodes.Ldloca:
                case OpCodeCodes.Ldloca_S:
                case OpCodeCodes.Ldloc_0:
                case OpCodeCodes.Ldloc_1:
                case OpCodeCodes.Ldloc_2:
                case OpCodeCodes.Ldloc_3:
                    {
                        yield return new NetAstLocalExpression() { LocalInfo = (NetLocalVariable)instruction.Operand };
                        yield break;
                    }

                #endregion

                #region Load Field

                case OpCodeCodes.Ldfld:
                case OpCodeCodes.Ldflda:
                    FieldInfo field = (FieldInfo)instruction.Operand;
                    if (ops.Length == 0)
                        yield break;
                    yield return new NetAstFieldExpression() { LeftSide = ops[0], Member = field };
                    yield break;

                case OpCodeCodes.Ldsfld:
                case OpCodeCodes.Ldsflda:
                    FieldInfo field2 = (FieldInfo)instruction.Operand;
                    yield return new NetAstFieldExpression() { LeftSide = null, Member = field2 };
                    yield break;

                #endregion

                #region Calls

                case OpCodeCodes.Call:
                case OpCodeCodes.Callvirt:
                case OpCodeCodes.Calli:
                    {
                        if (instruction.Operand is MethodInfo)
                        {
                            MethodInfo method = (MethodInfo)instruction.Operand;
                            if (IsAddressCall(method))
                            {
                                var accessExpression = new NetMDArrayAccessExpression() { ArrayExpression = ops[0], IndicesExpressions = ops.Skip(1).ToArray() };
                                addressCalls.Push(accessExpression);
                                yield break;
                                //yield return accessExpression;                               
                            }

                            if (method.IsStatic && method.Name.StartsWith("op_"))
                            { // es un operador
                                var op = OperatorsExtensor.Parse(method.Name.Substring(3));
                                switch (ops.Length)
                                {
                                    case 1:
                                        switch (op)
                                        {
                                            case Operators.Cast:
                                            case Operators.Implicit:
                                                yield return new NetAstConvertOperatorExpression { Operand = ops[0], TargetType = method.ReturnType };
                                                yield break;
                                            default:
                                                yield return new NetAstUnaryOperatorExpression { Operand = ops[0], Operator = op };
                                                yield break;
                                        }
                                    case 2:
                                        yield return new NetAstMethodCallExpression { Member = method, Arguments = ops };
                                        //yield return new NetAstBinaryOperatorExpression{ LeftOperand = ops[0], RightOperand = ops[1], Operator = op, ReturnType = method.ReturnType };
                                        yield break;
                                    case 3:
                                        yield return new NetAstTernaryOperatorExpressionv { Conditional = ops[0], WhenTrue = ops[1], WhenFalse = ops[2] };
                                        yield break;
                                }
                            }

                            yield return new NetAstMethodCallExpression() { Arguments = ops, Member = method };
                            yield break;
                        }
                        if (instruction.Operand is ConstructorInfo)
                        {
                            ConstructorInfo constructor = (ConstructorInfo)instruction.Operand;
                            yield return new NetAstConstructorCallExpression() { Arguments = ops, Member = constructor };
                            yield break;
                        }
                        throw new NotSupportedException();
                    }
                #endregion

                #region Initialization

                case OpCodeCodes.Initobj:
                    {
                        Type type = (Type)instruction.Operand;
                        yield return new NetAstInitObjectExpression() { TargetType = type };
                        yield break;

                    }
                case OpCodeCodes.Newobj:
                    {
                        ConstructorInfo constructor = (ConstructorInfo)instruction.Operand;
                        yield return new NetAstConstructorCallExpression() { Arguments = ops, Member = constructor };
                        yield break;
                    }
                #endregion

                #region Arrays
                case OpCodeCodes.Newarr:
                    yield return new NetInitArrayExpression() { ArrayType = (Type)instruction.Operand, ArraySize = ops[0] };
                    yield break;
                case OpCodeCodes.Ldelem:
                case OpCodeCodes.Ldelema:
                case OpCodeCodes.Ldelem_I:
                case OpCodeCodes.Ldelem_I1:
                case OpCodeCodes.Ldelem_I2:
                case OpCodeCodes.Ldelem_I4:
                case OpCodeCodes.Ldelem_I8:
                case OpCodeCodes.Ldelem_R4:
                case OpCodeCodes.Ldelem_R8:
                case OpCodeCodes.Ldelem_Ref:
                    yield return new NetArrayAccessExpression() { ArrayExpression = ops[0], IndexExpression = ops[1] };
                    yield break;
                case OpCodeCodes.Ldlen:
                    yield return new NetAstPropertyExpression() { LeftSide = ops[0], Member = ops[0].StaticType.GetProperty("Length") };
                    yield break;
                #endregion

                #region Dup
                case OpCodeCodes.Dup:
                    {
                        if (ops.Length == 0)
                        {
                            addressCalls.Push(addressCalls.Peek());
                            yield break;
                        }
                        yield return ops[0]; yield return ops[0]; yield break;
                    }
                #endregion

                case OpCodeCodes.Ldobj:
                    yield return addressCalls.Pop();
                    yield break;

                case OpCodeCodes.Ldtoken:
                    yield return new NetAstConstantExpression(instruction.Operand);
                    yield break;

                default: throw new NotSupportedException();
            }
        }

        private static NetAstStatement CreateStatementNodeFor(ILExpression instruction, NetAstExpression[] ops, MethodBase methodInfo)
        {
            switch (instruction.Code)
            {
                case OpCodeCodes.Initobj:
                    return new NetAstAssignamentStatement { LeftValue = ops[0], Value = new NetAstInitObjectExpression { TargetType = instruction.Operand as Type } };

                #region Explicity Pop

                case OpCodeCodes.Pop: // expression explicity popped.
                    NetAstExpression e = ops[0];
                    if (e is NetAstMethodCallExpression)
                        return new NetAstExpressionStatement() { Expression = e };
                    return null;

                #endregion

                #region Return

                case OpCodeCodes.Ret:
                    return new NetAstReturn()
                    {
                        Expression = (methodInfo is ConstructorInfo) ? null : (((MethodInfo)methodInfo).ReturnType != typeof(void) ? ops[0] : null)
                    };

                #endregion

                #region Assignaments

                #region Set Field ::= <LeftSide>.<Field> = <ops[0]>

                
                case OpCodeCodes.Stfld:
                    var field = (FieldInfo)instruction.Operand;
                    NetAstExpression leftSide = ops[0];
                    NetAstExpression value = ops[1];

                    return new NetAstAssignamentStatement()
                    {
                        LeftValue = new NetAstFieldExpression { LeftSide = leftSide, Member = field },
                        Value = value
                    };

                case OpCodeCodes.Stsfld:
                    var staticField = (FieldInfo)instruction.Operand;

                    return new NetAstAssignamentStatement()
                    {
                        LeftValue = new NetAstFieldExpression { Member = staticField, LeftSide = null },
                        Value = ops[0]
                    };

                #endregion

                #region Set Arguments ::= <param i> = <ops[0]>

                case OpCodeCodes.Starg:
                case OpCodeCodes.Starg_S:
                    {
                        return new NetAstAssignamentStatement()
                        {
                            LeftValue = new NetAstArgumentExpression { ParameterInfo = (ParameterInfo)instruction.Operand },
                            Value = ops[0]
                        };
                    }

                #endregion

                #region Set Locals ::= <local i> = <ops[0]>
                case OpCodeCodes.Stloc:
                case OpCodeCodes.Stloc_S:
                case OpCodeCodes.Stloc_0:
                case OpCodeCodes.Stloc_1:
                case OpCodeCodes.Stloc_2:
                case OpCodeCodes.Stloc_3:
                    {
                        if (ops.Length == 0)
                            return null;

                        return new NetAstAssignamentStatement()
                        {
                            LeftValue = new NetAstLocalExpression { LocalInfo = (NetLocalVariable)instruction.Operand },
                            Value = ops[0]
                        };
                    }

                #endregion

                #endregion

                #region Jumps

                case OpCodeCodes.Beq:
                case OpCodeCodes.Beq_S:
                case OpCodeCodes.Bge:
                case OpCodeCodes.Bge_S:
                case OpCodeCodes.Bge_Un:
                case OpCodeCodes.Bge_Un_S:
                case OpCodeCodes.Bgt:
                case OpCodeCodes.Bgt_S:
                case OpCodeCodes.Bgt_Un:
                case OpCodeCodes.Bgt_Un_S:
                case OpCodeCodes.Ble:
                case OpCodeCodes.Ble_S:
                case OpCodeCodes.Ble_Un:
                case OpCodeCodes.Ble_Un_S:
                case OpCodeCodes.Blt:
                case OpCodeCodes.Blt_S:
                case OpCodeCodes.Blt_Un:
                case OpCodeCodes.Blt_Un_S:
                case OpCodeCodes.Bne_Un:
                case OpCodeCodes.Bne_Un_S:
                    {
                        Operators op = Operators.None;
                        switch (instruction.Code)
                        {
                            case OpCodeCodes.Beq:
                            case OpCodeCodes.Beq_S:
                                op = Operators.Equality;
                                break;
                            case OpCodeCodes.Bge:
                            case OpCodeCodes.Bge_S:
                            case OpCodeCodes.Bge_Un:
                            case OpCodeCodes.Bge_Un_S:
                                op = Operators.GreaterThanOrEquals;
                                break;
                            case OpCodeCodes.Bgt:
                            case OpCodeCodes.Bgt_S:
                            case OpCodeCodes.Bgt_Un:
                            case OpCodeCodes.Bgt_Un_S:
                                op = Operators.GreaterThan;
                                break;
                            case OpCodeCodes.Ble:
                            case OpCodeCodes.Ble_S:
                            case OpCodeCodes.Ble_Un:
                            case OpCodeCodes.Ble_Un_S:
                                op = Operators.LessThanOrEquals;
                                break;
                            case OpCodeCodes.Blt:
                            case OpCodeCodes.Blt_S:
                            case OpCodeCodes.Blt_Un:
                            case OpCodeCodes.Blt_Un_S:
                                op = Operators.LessThan;
                                break;
                            case OpCodeCodes.Bne_Un:
                            case OpCodeCodes.Bne_Un_S:
                                op = Operators.Inequality;
                                break;
                        }

                        return new NetAstConditionalGoto()
                        {
                            Condition = new NetAstBinaryOperatorExpression()
                            {
                                Operator = op,
                                RightOperand = ops[1],
                                LeftOperand = ops[0]
                            },
                            Destination = new NetAstLabel() { Name = ((ILLabel)instruction.Operand).Name }
                        };
                    }
                case OpCodeCodes.Brfalse:
                case OpCodeCodes.Brfalse_S:
                    return new NetAstConditionalGoto()
                    {
                        Condition = new NetAstUnaryOperatorExpression()
                        {
                            Operator = Operators.Not,
                            Operand = ops[0]
                        },
                        Destination = new NetAstLabel() { Name = ((ILLabel)instruction.Operand).Name }
                    };
                case OpCodeCodes.Brtrue:
                case OpCodeCodes.Brtrue_S:
                    return new NetAstConditionalGoto()
                    {
                        Condition = ops[0],
                        Destination = new NetAstLabel() { Name = ((ILLabel)instruction.Operand).Name }
                    };
                case OpCodeCodes.Br:
                case OpCodeCodes.Br_S:
                    return new NetAstUnconditionalGoto()
                    {
                        Destination = new NetAstLabel() { Name = ((ILLabel)instruction.Operand).Name }
                    };
                #endregion

                #region Arrays
                case OpCodeCodes.Stelem :
                case OpCodeCodes.Stelem_I:
                case OpCodeCodes.Stelem_I1:
                case OpCodeCodes.Stelem_I2:
                case OpCodeCodes.Stelem_I4:
                case OpCodeCodes.Stelem_I8:
                case OpCodeCodes.Stelem_R4:
                case OpCodeCodes.Stelem_R8:
                case OpCodeCodes.Stelem_Ref:
                    return new NetArrayElementAssignament() { ArrayExpression = ops[0], ArrayIndex = ops[1], Value = ops[2] };
                #endregion

                #region Calls
                case OpCodeCodes.Call:
                case OpCodeCodes.Callvirt:
                case OpCodeCodes.Calli:
                    {
                        if (instruction.Operand is MethodInfo)
                        {
                            MethodInfo method = (MethodInfo)instruction.Operand;
                            return new NetAstExpressionStatement() { Expression = new NetAstMethodCallExpression() { Arguments = ops, Member = method } };
                        }
                        if (instruction.Operand is ConstructorInfo)
                        {
                            ConstructorInfo constructor = (ConstructorInfo)instruction.Operand;
                            return new NetAstAssignamentStatement() { LeftValue = ops[0], Value = new NetAstConstructorCallExpression() { Arguments = ops.Skip(1).ToArray(), Member = constructor } };
                        }
                        throw new NotSupportedException();
                    }
                #endregion

                case OpCodeCodes.Stobj:
                    return new NetAstAssignamentStatement { LeftValue = ops[0], Value = ops[1] };

                    //var left = addressCalls.Pop() as NetMDArrayAccessExpression;
                    //if (left != null)
                    //    return new NetMDArrayElementAssignament() { ArrayExpression = left.ArrayExpression, Value = ops[1], ArrayIndices = left.IndicesExpressions };
                    break;
            }
            return null;
        }

        private static NetAstExpression ResolveBoolComparison(Operators op, NetAstExpression o1, NetAstExpression o2)
        {
            if (o1.StaticType != typeof(int))
            {
                var temp = o2;
                o2 = o1;
                o1 = temp;
            }

            switch (op)
            {
                case Operators.Equality:
                    if (Convert.ToInt32(((NetAstConstantExpression)o1).Value) == 0)
                        return new NetAstUnaryOperatorExpression
                        {
                            Operator = Operators.Not,
                            Operand = o2
                        };
                    else
                        return o2;
                case Operators.Inequality:
                    if (Convert.ToInt32(((NetAstConstantExpression)o1).Value) == 0)
                        return o2;
                    else
                        return new NetAstUnaryOperatorExpression
                        {
                            Operator = Operators.Not,
                            Operand = o2
                        };
            }

            throw new NotImplementedException();
        }

        private static void InlineVariables(List<ILNode> method)
        {
            // Analyse the whole method
            Dictionary<NetLocalVariable, int> numStloc = new Dictionary<NetLocalVariable, int>();
            Dictionary<NetLocalVariable, int> numLdloc = new Dictionary<NetLocalVariable, int>();
            Dictionary<NetLocalVariable, int> numLdloca = new Dictionary<NetLocalVariable, int>();

            foreach (ILExpression expr in method.GetSelfAndChildrenRecursive())
            {
                NetLocalVariable locVar = expr.Operand as NetLocalVariable;
                if (locVar != null)
                {
                    if (!numStloc.ContainsKey(locVar)) numStloc.Add(locVar, 0);
                    if (!numLdloc.ContainsKey(locVar)) numLdloc.Add(locVar, 0);
                    if (!numLdloca.ContainsKey(locVar)) numLdloca.Add(locVar, 0);

                    if (expr.Code == OpCodeCodes.Stloc)
                        numStloc[locVar] += 1;
                    else if (expr.Code == OpCodeCodes.Ldloc)
                        numLdloc[locVar] += 1;
                    else if (expr.Code == OpCodeCodes.Ldloca)
                        numLdloca[locVar] += 1;
                    else
                        throw new NotSupportedException(expr.Code.ToString());
                }
            }

            // Inline all blocks
            List<ILNode> body = method;
            for (int i = 0; i < body.Count - 1; )
            {
                ILExpression nextExpr = body[i + 1] as ILExpression;
                NetLocalVariable locVar;
                ILExpression expr;
                ILExpression ldParent;
                int ldPos;
                if (body[i].Match(OpCodeCodes.Stloc, out locVar, out expr) &&
                    numStloc[locVar] == 1 &&
                    numLdloc[locVar] == 1 &&
                    numLdloca[locVar] == 0 &&
                    nextExpr != null &&
                    FindLdloc(nextExpr, locVar, out ldParent, out ldPos) == true &&
                    ldParent != null)
                {
                    // We are moving the expression evaluation past the other aguments.
                    // It is ok to pass ldloc because the expression can not contain stloc and thus the ldloc will still return the same value
                    body.RemoveAt(i);
                    ldParent.Arguments[ldPos] = expr; // Inline the stloc body
                    i = Math.Max(0, i - 1); // Go back one step
                }
                else
                {
                    i++;
                }
            }
        }
        
        static bool? FindLdloc(ILExpression expr, NetLocalVariable v, out ILExpression parent, out int pos)
        {
            parent = null;
            pos = 0;
            for (int i = 0; i < expr.Arguments.Count; i++)
            {
                ILExpression arg = expr.Arguments[i];
                if (arg.Code == OpCodeCodes.Ldloc && arg.Operand == v)
                {
                    parent = expr;
                    pos = i;
                    return true;
                }
                bool? r = FindLdloc(arg, v, out parent, out pos);
                if (r != null)
                    return r;
            }
            return expr.Code == OpCodeCodes.Ldloc ? (bool?)null : false;
        }

        static bool IsAddressCall(MethodInfo method)
        {
            return method.ReturnType != null && method.ReturnType.IsByRef && method.Equals(method.DeclaringType.GetMethod("Address"));
        }

        class VariableInfo
        {
            public NetLocalVariable Variable { get; set; }
            public List<ExecutionState> Stores { get; set; }
            public List<ExecutionState> Loads { get; set; }
        }

        class StackSlot
        {
            public readonly ExecutionState[] PushedBy;  // One of those
            public readonly NetLocalVariable LoadFrom;  // Where can we get the value from in AST

            public StackSlot(ExecutionState[] pushedBy, NetLocalVariable loadFrom)
            {
                this.PushedBy = pushedBy;
                this.LoadFrom = loadFrom;
            }

            public StackSlot(ExecutionState pushedBy)
            {
                this.PushedBy = new[] { pushedBy };
                this.LoadFrom = null;
            }

            public static List<StackSlot> CloneStack(List<StackSlot> stack, int? popCount)
            {
                if (popCount.HasValue)
                {
                    return stack.GetRange(0, stack.Count - popCount.Value);
                }
                else
                {
                    return new List<StackSlot>(0);
                }
            }
        }

        /// <summary> Immutable </summary>
        class VariableSlot
        {
            public readonly ExecutionState[] StoredBy;    // One of those
            public readonly bool StoredByAll; // Overestimate which is useful for exceptional control flow.

            public VariableSlot(ExecutionState[] storedBy, bool storedByAll)
            {
                this.StoredBy = storedBy;
                this.StoredByAll = storedByAll;
            }

            public VariableSlot(ExecutionState storedBy)
            {
                this.StoredBy = new[] { storedBy };
                this.StoredByAll = false;
            }

            public static VariableSlot[] CloneVariableState(VariableSlot[] state)
            {
                VariableSlot[] clone = new VariableSlot[state.Length];
                for (int i = 0; i < clone.Length; i++)
                {
                    clone[i] = state[i];
                }
                return clone;
            }

            public static VariableSlot[] MakeEmptyState(int varCount)
            {
                VariableSlot[] emptyVariableState = new VariableSlot[varCount];
                for (int i = 0; i < emptyVariableState.Length; i++)
                {
                    emptyVariableState[i] = new VariableSlot(EmptyExecStateArray, false);
                }
                return emptyVariableState;
            }

            public static VariableSlot[] MakeFullState(int varCount)
            {
                VariableSlot[] unknownVariableState = new VariableSlot[varCount];
                for (int i = 0; i < unknownVariableState.Length; i++)
                {
                    unknownVariableState[i] = new VariableSlot(EmptyExecStateArray, true);
                }
                return unknownVariableState;
            }
        }

        class ExecutionState
        {
            public ILLabel Label { get; set; }      // Non-null only if needed
            public int Offset { get; set; }
            public int EndOffset { get; set; }
            public OpCodeCodes Code { get; set; }
            public object Operand { get; set; }
            public int? PopCount { get; set; }   // Null means pop all
            public int PushCount { get; set; }
            public string Name { get { return "IL_" + this.Offset.ToString("X2"); } }
            public ExecutionState Next { get; set; }
            public ILInstruction[] Prefixes { get; set; }        // Non-null only if needed
            public List<StackSlot> StackBefore { get; set; }     // Unique per ExecutionState; not shared
            public List<NetLocalVariable> StoreTo { get; set; }         // Store result of instruction to those AST variables
            public VariableSlot[] VariablesBefore { get; set; } // Unique per ExecutionState; not shared

            public LocalVariableInfo OperandAsVariable { get { return (LocalVariableInfo)this.Operand; } }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                // Label
                sb.Append(this.Name);
                sb.Append(':');
                if (this.Label != null)
                    sb.Append('*');

                // Name
                sb.Append(' ');
                if (this.Prefixes != null)
                {
                    foreach (var prefix in this.Prefixes)
                    {
                        sb.Append(prefix.OpCode.Name);
                        sb.Append(' ');
                    }
                }
                sb.Append(this.Code.ToString().ToLower());

                if (this.Operand != null)
                {
                    sb.Append(' ');
                    if (this.Operand is ILInstruction)
                    {
                        sb.Append("IL_" + ((ILInstruction)this.Operand).Address.ToString("X2"));
                    }
                    else if (this.Operand is ILInstruction[])
                    {
                        foreach (ILInstruction inst in (ILInstruction[])this.Operand)
                        {
                            sb.Append("IL_" + inst.Address.ToString("X2"));
                            sb.Append(" ");
                        }
                    }
                    else if (this.Operand is NetAstLabel)
                    {
                        sb.Append(((NetAstLabel)this.Operand).Name);
                    }
                    else if (this.Operand is NetAstLabel[])
                    {
                        foreach (var label in (NetAstLabel[])this.Operand)
                        {
                            sb.Append(label);
                            sb.Append(" ");
                        }
                    }
                    else
                    {
                        sb.Append(this.Operand.ToString());
                    }
                }

                if (this.StackBefore != null)
                {
                    sb.Append(" StackBefore={");
                    bool first = true;
                    foreach (StackSlot slot in this.StackBefore)
                    {
                        if (!first) sb.Append(",");
                        bool first2 = true;
                        foreach (var pushedBy in slot.PushedBy)
                        {
                            if (!first2) sb.Append("|");
                            sb.AppendFormat("IL_{0:X2}", pushedBy.Offset);
                            first2 = false;
                        }
                        first = false;
                    }
                    sb.Append("}");
                }

                if (this.StoreTo != null && this.StoreTo.Count > 0)
                {
                    sb.Append(" StoreTo={");
                    bool first = true;
                    foreach (var stackVar in this.StoreTo)
                    {
                        if (!first) sb.Append(",");
                        sb.Append(stackVar.Name);
                        first = false;
                    }
                    sb.Append("}");
                }

                if (this.VariablesBefore != null)
                {
                    sb.Append(" VarsBefore={");
                    bool first = true;
                    foreach (VariableSlot varSlot in this.VariablesBefore)
                    {
                        if (!first) sb.Append(",");
                        if (varSlot.StoredByAll)
                        {
                            sb.Append("*");
                        }
                        else if (varSlot.StoredBy.Length == 0)
                        {
                            sb.Append("_");
                        }
                        else
                        {
                            bool first2 = true;
                            foreach (var storedBy in varSlot.StoredBy)
                            {
                                if (!first2) sb.Append("|");
                                sb.AppendFormat("IL_{0:X2}", storedBy.Offset);
                                first2 = false;
                            }
                        }
                        first = false;
                    }
                    sb.Append("}");
                }

                return sb.ToString();
            }
        }        
    }
}
