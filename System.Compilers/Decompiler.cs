using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Compilers.AST;
using System.Compilers.IL;

namespace System.Compilers
{
    public static class Decompiler
    {
        private static int IndexOf(this IEnumerable<ILInstruction> e, int Address)
        {
            int index = 0;
            foreach (var i in e)
            {
                if (i.Address == Address)
                    return index;
                index++;
            }

            return -1;
        }

        #region Execution State 

        public class ExecutionState
        {
            /// <summary>
            /// Stack state before executing current instruction
            /// </summary>
            public Stack<NetAstExpression> StackBefore { get; internal set; }

            public Stack<NetAstExpression> StackAfter { get; internal set; }

            public ILInstruction Instruction { get { return IL[Index]; } }

            private IEnumerable<ExecutionState> GetNext(ExecutionStateFactory factory, MethodInfo methodInfo, Dictionary<int, NetAstLabel> labels)
            {
                this.Prev = new ExecutionState[0];

                List<ExecutionState> result = new List<ExecutionState>();

                if (!Instruction.IsBranch)
                {
                    if (Instruction.OpCode != OpCodes.Ret)
                        result.Add(factory.CreateFrom(IL, Index + 1, StackAfter, methodInfo, labels));
                }
                else
                {
                    if (Instruction.IsConditionalBranch)
                        result.Add(factory.CreateFrom(IL, Index + 1, StackAfter, methodInfo, labels));

                    result.Add(factory.CreateFrom(IL, IndexOf(IL, Instruction.BrachILAddress), StackAfter, methodInfo, labels));
                }

                foreach (var n in result)
                {
                    if (n.Prev.Length == 1)
                        n.Prev = n.Prev.Union(new ExecutionState[] { this }).ToArray();
                    else
                        n.Prev = n.Prev.Union(new ExecutionState[] { this }).ToArray();
                }

                return result;
            }

            private ExecutionState()
            {
            }

            public void Initialize(ExecutionStateFactory factory, ILInstruction[] il, int index, Stack<NetAstExpression> stack, MethodInfo methodInfo, Dictionary<int, NetAstLabel> labels)
            {
                this.StackBefore = stack;
                this.IL = il;
                this.Index = index;

                Execute(methodInfo, labels);

                this.Next = GetNext(factory, methodInfo, labels).ToArray();
            }

            #region Decompiling

            private NetAstExpression CreateExpressionNodeFor(ILInstruction instruction, NetAstExpression[] ops, MethodInfo methodInfo)
            {
                OpCode opCode = instruction.OpCode;

                switch ((OpCodeCodes)opCode.Value)
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
                        switch ((OpCodeCodes)opCode.Value)
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
                            (ops[0].StaticType != ops[1].StaticType))
                            return ResolveBoolComparison(op, ops[0], ops[1]);
                        return new NetAstBinaryOperatorExpression() { Operator = op, RightOperand = ops[0], LeftOperand = ops[1] };
                        }
                    #endregion

                    #region Unary operations

                    case OpCodeCodes.Neg:
                    case OpCodeCodes.Not:
                        {
                            Operators op = Operators.None;
                            switch ((OpCodeCodes)opCode.Value)
                            {
                                case OpCodeCodes.Neg:
                                    op = Operators.UnaryNegation;
                                    break;
                                case OpCodeCodes.Not:
                                    op = Operators.Not;
                                    break;
                            }
                            return new NetAstUnaryOperatorExpression() { Operator = op, Operand = ops[0] };
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
                        {
                            Type targetType = null;

                            switch ((OpCodeCodes)opCode.Value)
                            {
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
                                default: throw new NotImplementedException("Its missing " + ((OpCodeCodes)opCode.Value));
                            }

                            return new NetAstConvertOperatorExpression() { TargetType = targetType, Operand = ops[0] };
                        }

                    #endregion

                    #endregion

                    #endregion

                    #region Constants

                    case OpCodeCodes.Ldc_I4: return new NetAstConstantExpression() { Value = (int)instruction.Operand };
                    case OpCodeCodes.Ldc_I4_0: return new NetAstConstantExpression() { Value = 0 };
                    case OpCodeCodes.Ldc_I4_1: return new NetAstConstantExpression() { Value = 1 };
                    case OpCodeCodes.Ldc_I4_2: return new NetAstConstantExpression() { Value = 2 };
                    case OpCodeCodes.Ldc_I4_3: return new NetAstConstantExpression() { Value = 3 };
                    case OpCodeCodes.Ldc_I4_4: return new NetAstConstantExpression() { Value = 4 };
                    case OpCodeCodes.Ldc_I4_5: return new NetAstConstantExpression() { Value = 5 };
                    case OpCodeCodes.Ldc_I4_6: return new NetAstConstantExpression() { Value = 6 };
                    case OpCodeCodes.Ldc_I4_7: return new NetAstConstantExpression() { Value = 7 };
                    case OpCodeCodes.Ldc_I4_8: return new NetAstConstantExpression() { Value = 8 };
                    case OpCodeCodes.Ldc_I4_M1: return new NetAstConstantExpression() { Value = -1 };
                    case OpCodeCodes.Ldc_I4_S: return new NetAstConstantExpression { Value = (sbyte)instruction.Operand };
                    case OpCodeCodes.Ldc_I8: return new NetAstConstantExpression { Value = (long)instruction.Operand };
                    case OpCodeCodes.Ldc_R4: return new NetAstConstantExpression { Value = (float)instruction.Operand };
                    case OpCodeCodes.Ldc_R8: return new NetAstConstantExpression { Value = (double)instruction.Operand };

                    #endregion

                    #region Load Arguments

                    case OpCodeCodes.Ldarg:
                    case OpCodeCodes.Ldarga:
                    case OpCodeCodes.Ldarga_S:
                    case OpCodeCodes.Ldarg_S:
                    case OpCodeCodes.Ldarg_0:
                    case OpCodeCodes.Ldarg_1:
                    case OpCodeCodes.Ldarg_2:
                    case OpCodeCodes.Ldarg_3:
                        {
                            int index;

                            switch ((OpCodeCodes)instruction.OpCode.Value)
                            {
                                case OpCodeCodes.Ldarg_0: index = 0; break;
                                case OpCodeCodes.Ldarg_1: index = 1; break;
                                case OpCodeCodes.Ldarg_2: index = 2; break;
                                case OpCodeCodes.Ldarg_3: index = 3; break;
                                default:
                                    index = (int)Convert.ChangeType(instruction.Operand, typeof(int));
                                    break;
                            }
                            if (index == 0 && !methodInfo.IsStatic) return new NetAstThisExpression() { DeclaringType = methodInfo.DeclaringType };
                            return new NetAstArgumentExpression() { ParameterInfo = methodInfo.GetParameters()[index + (methodInfo.IsStatic ? 0 : -1)] };
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
                            int index;

                            switch ((OpCodeCodes)instruction.OpCode.Value)
                            {
                                case OpCodeCodes.Ldloc_0: index = 0; break;
                                case OpCodeCodes.Ldloc_1: index = 1; break;
                                case OpCodeCodes.Ldloc_2: index = 2; break;
                                case OpCodeCodes.Ldloc_3: index = 3; break;
                                default:
                                    index = (int)Convert.ChangeType(instruction.Operand, typeof(int));
                                    break;
                            }

                            return new NetAstLocalExpression() { LocalInfo = methodInfo.GetMethodBody().LocalVariables[index] };
                        }

                    #endregion

                    #region Load Field

                    case OpCodeCodes.Ldfld:
                    case OpCodeCodes.Ldflda:
                    case OpCodeCodes.Ldsfld:
                    case OpCodeCodes.Ldsflda:

                        FieldInfo field = (FieldInfo)instruction.Operand;

                        return new NetAstFieldExpression() { LeftSide = ops[0], FieldInfo = field };

                    #endregion

                    #region Calls

                    case OpCodeCodes.Call:
                    case OpCodeCodes.Callvirt:
                    case OpCodeCodes.Calli:
                        {
                            if (instruction.Operand is MethodInfo)
                            {
                                MethodInfo method = (MethodInfo)instruction.Operand;
                                return new NetAstMethodCallExpression() { Arguments = ops, MethodBase = method };
                            }
                            if (instruction.Operand is ConstructorInfo)
                            {
                                ConstructorInfo constructor = (ConstructorInfo)instruction.Operand;
                                return new NetAstConstructorCallExpression() { Arguments = ops, MethodBase = constructor };
                            }
                            throw new NotSupportedException();
                        }
                    #endregion

                    #region Initialization

                    case OpCodeCodes.Initobj:
                        {
                            Type type = (Type)instruction.Operand;
                            return new NetAstInitObjectExpression() { TargetType = type };
                        }
                    case OpCodeCodes.Newobj:
                        {
                            ConstructorInfo constructor = (ConstructorInfo)instruction.Operand;
                            return new NetAstConstructorCallExpression() { Arguments = ops, MethodBase = constructor };
                        }
                    #endregion
                }

                return null;
            }

            private NetAstExpression ResolveBoolComparison(Operators op, NetAstExpression o1, NetAstExpression o2)
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

            private NetAstNode CreateStatementNodeFor(ILInstruction instruction, NetAstExpression[] ops, MethodInfo methodInfo, Dictionary<int, NetAstLabel> labels)
            {
                switch ((OpCodeCodes)instruction.OpCode.Value)
                {
                    #region Explicity Pop

                    case OpCodeCodes.Pop: // expression explicity popped.
                            NetAstExpression e = ops[0];
                            if (e is NetAstMethodCallExpression)
                                return e;
                            return null;

                    #endregion

                    #region Return

                    case OpCodeCodes.Ret:
                            return new NetAstReturn() { Expression = methodInfo.ReturnType != typeof(void) ? ops[0] : null };

                    #endregion

                    #region Assignaments

                    #region Set Field ::= <LeftSide>.<Field> = <ops[0]>

                    case OpCodeCodes.Stsfld:
                    case OpCodeCodes.Stfld:
                            var field = (FieldInfo)instruction.Operand;
                            NetAstExpression leftSide = ops[1];
                            NetAstExpression value = ops[0];

                            return new NetAstFieldAssignament()
                            {
                                FieldInfo = field, 
                                LeftSide = leftSide,
                                Value = value
                            };

                    #endregion

                    #region Set Arguments ::= <param i> = <ops[0]>

                    case OpCodeCodes.Starg:
                    case OpCodeCodes.Starg_S:
                        {
                            int index = Convert.ToInt32(instruction.Operand);
                            return new NetAstArgumentAssignament(){
                                ParameterInfo = methodInfo.GetParameters()[index],
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
                            int local = 0;
                            switch ((OpCodeCodes)instruction.OpCode.Value)
                            {
                                case OpCodeCodes.Stloc_S:
                                case OpCodeCodes.Stloc:
                                    local = Convert.ToInt32(instruction.Operand);
                                    break;
                                case OpCodeCodes.Stloc_0:
                                    local = 0;
                                    break;
                                case OpCodeCodes.Stloc_1:
                                    local = 1;
                                    break;
                                case OpCodeCodes.Stloc_2:
                                    local = 2;
                                    break;
                                case OpCodeCodes.Stloc_3:
                                    local = 3;
                                    break;
                            }

                            return new NetAstLocalAssignament()
                            {
                                LocalInfo = methodInfo.GetMethodBody().LocalVariables[local],
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
                            switch ((OpCodeCodes)instruction.OpCode.Value)
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
                                    RightOperand = ops[0],
                                    LeftOperand = ops[1]
                                },
                                TrueDestination = GetOrCreateLabel (instruction.BrachILAddress, labels)
                            };
                        }
                    case OpCodeCodes.Brfalse:
                    case OpCodeCodes.Brfalse_S:
                        return new NetAstConditionalGoto()
                        {
                            Condition = new NetAstUnaryOperatorExpression(){
                                Operator = Operators.Not,
                                Operand = ops[0]
                            },
                            TrueDestination = GetOrCreateLabel(instruction.BrachILAddress, labels)
                        };
                    case OpCodeCodes.Brtrue:
                    case OpCodeCodes.Brtrue_S:
                        return new NetAstConditionalGoto()
                        {
                            Condition = ops[0],
                            TrueDestination = GetOrCreateLabel(instruction.BrachILAddress, labels)
                        };
                    case OpCodeCodes.Br:
                    case OpCodeCodes.Br_S:
                        return new NetAstUnconditionalGoto()
                        {
                            Destination = GetOrCreateLabel (instruction.BrachILAddress, labels)
                        };
                    #endregion
                }

                return null;
            }

            private NetAstLabel GetOrCreateLabel(int address, Dictionary<int, NetAstLabel> labels)
            {
                if (!labels.ContainsKey(address))
                    labels.Add(address, new NetAstLabel() { ILAddress = address });

                return labels[address];
            }

            #endregion

            private void Execute(MethodInfo methodInfo, Dictionary<int, NetAstLabel> labels)
            {
                this.StackAfter = new Stack<NetAstExpression>(StackBefore);

                int countOfPops = this.Instruction.CountOfPops();

                NetAstExpression[] ops = new NetAstExpression[countOfPops];
                for (int i = ops.Length - 1; i >= 0; i--)
                    ops[i] = StackAfter.Pop();

                this.Expression = CreateExpressionNodeFor(Instruction, ops, methodInfo);

                int countOfPushes = this.Instruction.CountOfPushes();
                if (countOfPushes > 0)
                {
                    while (countOfPushes > 0)
                    {
                        StackAfter.Push(this.Expression);
                        countOfPushes--;
                    }
                }
                else
                {
                    this.Statement = CreateStatementNodeFor(Instruction, ops, methodInfo, labels);
                    if (this.Statement == null)
                        this.Statement = this.Expression;
                    if (this.Statement != null)
                        this.Statement.ILAddress = Instruction.Address;
                }

            }

            public ExecutionState[] Next { get; internal set; }

            public ExecutionState[] Prev { get; internal set; }

            public int Index { get; private set; }

            public ILInstruction[] IL { get; private set; }

            public NetAstExpression Expression { get; private set; }

            public NetAstNode Statement { get; private set; }

            private static bool SameStackState(Stack<NetAstExpression> s1, Stack<NetAstExpression> s2)
            {
                return s1.Count == s2.Count && (s1.SequenceEqual(s2));
            }

            public static ExecutionStateFactory GetFactory()
            {
                return new ExecutionStateFactory();
            }

            public class ExecutionStateFactory {
                Dictionary<int, Dictionary<Stack<NetAstExpression>, ExecutionState>> instances = new Dictionary<int,Dictionary<Stack<NetAstExpression>,ExecutionState>>();

                class StackStateEqCmpr : IEqualityComparer<Stack<NetAstExpression>>
                {
                    public bool Equals(Stack<NetAstExpression> x, Stack<NetAstExpression> y)
                    {
                        return SameStackState(x, y);
                    }

                    public int GetHashCode(Stack<NetAstExpression> obj)
                    {
                        return obj.Count;
                    }
                }

                public ExecutionState CreateFrom(ILInstruction[] il, int index, Stack<NetAstExpression> stack, MethodInfo methodInfo, Dictionary<int, NetAstLabel> labels)
                {
                    if (!instances.ContainsKey(index))
                        instances.Add(index, new Dictionary<Stack<NetAstExpression>, ExecutionState>(new StackStateEqCmpr()));

                    if (!instances[index].ContainsKey(stack))
                    {
                        var es = new ExecutionState();
                        instances[index].Add(stack, es);
                        es.Initialize (this, il, index, stack, methodInfo, labels);
                    }
                    
                    return instances[index][stack];
                }
            }
        }

        #endregion

        static List<NetAstNode> GetInstructions(ExecutionState s0)
        {
            HashSet<ExecutionState> nodes = new HashSet<ExecutionState>();

            Queue<ExecutionState> q = new Queue<ExecutionState>();

            q.Enqueue(s0);

            while (q.Count > 0)
            {
                var n = q.Dequeue();

                if (!nodes.Contains(n))
                {
                    foreach (var node in n.Next)
                        q.Enqueue(node);

                    nodes.Add(n);
                }
            }

            List<NetAstNode> nodeList = new List<NetAstNode>(nodes.Where(n => n.Statement != null).Select(n => n.Statement));
            nodeList.Sort((n1, n2) => n1.ILAddress.CompareTo(n2.ILAddress));

            return nodeList;
        }

        public static NetAstBlock GetNetAstFrom(MethodInfo method)
        {
            ILReader reader = new ILReader(method.Module.Assembly);

            var il = reader.GetIL(method).ToArray();

            var body = new List<NetAstNode>();

            List<ExecutionState>[] states = new List<ExecutionState>[il.Length];
            for (int i = 0; i < states.Length; i++)
                states[i] = new List<ExecutionState>();

            var factory = ExecutionState.GetFactory();

            Dictionary<int, NetAstLabel> labels = new Dictionary<int, NetAstLabel>();

            var s0 = factory.CreateFrom(il, 0, new Stack<NetAstExpression>(), method, labels);

            var instructions = GetInstructions(s0);

            foreach (var label in labels.Values)
            {
                int address = label.ILAddress;

                instructions.Insert(instructions.IndexOf(instructions.First(n => n.ILAddress >= address)), label);
            }

            var declarations = method.GetMethodBody().LocalVariables.Select(loc => new NetAstLocalDeclaration() { LocalInfo = loc });

            instructions.InsertRange(0, declarations.Cast<NetAstNode>());

            var block = new NetAstBlock();

            block.Instructions.AddRange(instructions);

            return block;
        }
    }
}
