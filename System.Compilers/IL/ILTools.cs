using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace System.Compilers.IL
{
    public static class ILTools
    {
        public static int? CountOfPops(this ILInstruction inst)
        {
            if (inst.OpCode == OpCodes.Ret)
                return null;

            switch (inst.OpCode.StackBehaviourPop)
            {
                case StackBehaviour.Pop0: return 0;
                case StackBehaviour.Pop1: return 1;
                case StackBehaviour.Popi: return 1;
                case StackBehaviour.Popref: return 1;
                case StackBehaviour.Pop1_pop1: return 2;
                case StackBehaviour.Popi_pop1: return 2;
                case StackBehaviour.Popi_popi: return 2;
                case StackBehaviour.Popi_popi8: return 2;
                case StackBehaviour.Popi_popr4: return 2;
                case StackBehaviour.Popi_popr8: return 2;
                case StackBehaviour.Popref_pop1: return 2;
                case StackBehaviour.Popref_popi: return 2;
                case StackBehaviour.Popi_popi_popi: return 3;
                case StackBehaviour.Popref_popi_pop1: return 3;
                case StackBehaviour.Popref_popi_popi: return 3;
                case StackBehaviour.Popref_popi_popi8: return 3;
                case StackBehaviour.Popref_popi_popr4: return 3;
                case StackBehaviour.Popref_popi_popr8: return 3;
                case StackBehaviour.Popref_popi_popref: return 3;
                case StackBehaviour.Varpop:
                    switch ((OpCodeCodes)inst.OpCode.Value)
                    {
                        case OpCodeCodes.Call:
                        case OpCodeCodes.Callvirt:
                                MethodBase cecilMethod = ((MethodBase)inst.Operand);
                                if (cecilMethod.IsStatic)
                                {
                                    return cecilMethod.GetParameters().Length;
                                }
                                else
                                {
                                    return cecilMethod.GetParameters().Length + 1 /* this */;
                                }
                            
                        case OpCodeCodes.Calli: throw new NotImplementedException();
                        case OpCodeCodes.Ret: return null;
                        case OpCodeCodes.Newobj:
                            ConstructorInfo ctorMethod = ((ConstructorInfo)inst.Operand);
                            return ctorMethod.GetParameters().Length;
                        default: throw new Exception("Unknown Varpop opcode");
                    }
                default: throw new Exception("Unknown pop behaviour: " + inst.OpCode.StackBehaviourPop);
            }
        }

        public static int CountOfPushes(this ILInstruction inst)
        {
            switch (inst.OpCode.StackBehaviourPush)
            {
                case StackBehaviour.Push0: return 0;
                case StackBehaviour.Push1: return 1;
                case StackBehaviour.Push1_push1: return 2;
                case StackBehaviour.Pushi: return 1;
                case StackBehaviour.Pushi8: return 1;
                case StackBehaviour.Pushr4: return 1;
                case StackBehaviour.Pushr8: return 1;
                case StackBehaviour.Pushref: return 1;
                case StackBehaviour.Varpush:     // Happens only for calls
                    switch ((OpCodeCodes)inst.OpCode.Value)
                    {
                        case OpCodeCodes.Call:
                        case OpCodeCodes.Callvirt:
                            MethodBase cecilMethod = ((MethodBase)inst.Operand);
                            if (cecilMethod.IsConstructor || ((MethodInfo)cecilMethod).ReturnType == typeof(void))
                            {
                                return 0;
                            }
                            else
                            {
                                return 1;
                            }
                        case OpCodeCodes.Calli: throw new NotImplementedException();
                        default: throw new Exception("Unknown Varpush opcode");
                    }
                default: throw new Exception("Unknown push behaviour: " + inst.OpCode.StackBehaviourPush);
            }
        }

        public static bool IsStatement(this ILInstruction instruction)
        {
            var opCode = instruction.OpCode;
            return opCode.OperandType == OperandType.InlineBrTarget || opCode.OperandType == OperandType.InlineMethod || opCode.OperandType == OperandType.InlineSwitch || opCode.OperandType == OperandType.ShortInlineBrTarget
                || opCode == OpCodes.Starg || opCode == OpCodes.Stfld;
        }

        internal static bool CanFallThough(this OpCodeCodes code)
        {
            switch (code)
            {
                case OpCodeCodes.Br:
                case OpCodeCodes.Br_S:
                case OpCodeCodes.Leave:
                case OpCodeCodes.Leave_S:
                case OpCodeCodes.Ret:
                case OpCodeCodes.Endfilter:
                case OpCodeCodes.Endfinally:
                case OpCodeCodes.Throw:
                case OpCodeCodes.Rethrow:
                    return false;
                default:
                    return true;
            }
        }

        internal static void Expand(ILInstruction inst, ILInstruction[] instructions, ref OpCodeCodes code, ref object operand, MethodBase method, MethodBody methodBody)
        {
            switch (code)
            {
                case OpCodeCodes.Ldarg_0:
                    if (method.IsStatic)
                    {
                        code = OpCodeCodes.Ldarg;
                        operand = method.GetParameters()[0];
                    }
                    else
                    {
                        code = OpCodeCodes.Ldarg_0;
                        operand = null;
                    }
                    break;
                case OpCodeCodes.Ldarg_1: code = OpCodeCodes.Ldarg; operand = method.GetParameters()[!method.IsStatic ? 0 : 1]; break;
                case OpCodeCodes.Ldarg_2: code = OpCodeCodes.Ldarg; operand = method.GetParameters()[!method.IsStatic ? 1 : 2]; break;
                case OpCodeCodes.Ldarg_3: code = OpCodeCodes.Ldarg; operand = method.GetParameters()[!method.IsStatic ? 2 : 3]; break;
                case OpCodeCodes.Ldloc_0: code = OpCodeCodes.Ldloc; operand = methodBody.LocalVariables[0]; break;
                case OpCodeCodes.Ldloc_1: code = OpCodeCodes.Ldloc; operand = methodBody.LocalVariables[1]; break;
                case OpCodeCodes.Ldloc_2: code = OpCodeCodes.Ldloc; operand = methodBody.LocalVariables[2]; break;
                case OpCodeCodes.Ldloc_3: code = OpCodeCodes.Ldloc; operand = methodBody.LocalVariables[3]; break;
                case OpCodeCodes.Stloc_0: code = OpCodeCodes.Stloc; operand = methodBody.LocalVariables[0]; break;
                case OpCodeCodes.Stloc_1: code = OpCodeCodes.Stloc; operand = methodBody.LocalVariables[1]; break;
                case OpCodeCodes.Stloc_2: code = OpCodeCodes.Stloc; operand = methodBody.LocalVariables[2]; break;
                case OpCodeCodes.Stloc_3: code = OpCodeCodes.Stloc; operand = methodBody.LocalVariables[3]; break;

                case OpCodeCodes.Ldarg_S: code = OpCodeCodes.Ldarg;
                    {
                        int index = (int)Convert.ChangeType(operand, typeof(int));
                        operand = method.GetParameters()[index + (method.IsStatic ? 0 : -1)] ;
                    }
                    break;

                case OpCodeCodes.Ldarga_S: code = OpCodeCodes.Ldarga;
                    {
                        int index = (int)Convert.ChangeType(operand, typeof(int));
                        operand = method.GetParameters()[index + (method.IsStatic ? 0 : -1)];
                    } 
                    break;

                case OpCodeCodes.Starg_S: code = OpCodeCodes.Starg; 
                    {
                        int index = (int)Convert.ChangeType(operand, typeof(int));
                        operand = method.GetParameters()[index + (method.IsStatic ? 0 : -1)];
                    }
                    break;

                case OpCodeCodes.Ldloc_S: code = OpCodeCodes.Ldloc;
                    {
                        int index = (int)Convert.ChangeType(operand, typeof(int));
                        operand = methodBody.LocalVariables[index];
                    }
                    break;
                
                case OpCodeCodes.Ldloca_S: code = OpCodeCodes.Ldloca;
                    {
                        int index = (int)Convert.ChangeType(operand, typeof(int));
                        operand = methodBody.LocalVariables[index];
                    }
                    break;
                
                case OpCodeCodes.Stloc_S: code = OpCodeCodes.Stloc;
                    {
                        int index = (int)Convert.ChangeType(operand, typeof(int));
                        operand = methodBody.LocalVariables[index];
                    }
                    break;

                case OpCodeCodes.Ldc_I4_M1: code = OpCodeCodes.Ldc_I4; operand = -1; break;
                case OpCodeCodes.Ldc_I4_0: code = OpCodeCodes.Ldc_I4; operand = 0; break;
                case OpCodeCodes.Ldc_I4_1: code = OpCodeCodes.Ldc_I4; operand = 1; break;
                case OpCodeCodes.Ldc_I4_2: code = OpCodeCodes.Ldc_I4; operand = 2; break;
                case OpCodeCodes.Ldc_I4_3: code = OpCodeCodes.Ldc_I4; operand = 3; break;
                case OpCodeCodes.Ldc_I4_4: code = OpCodeCodes.Ldc_I4; operand = 4; break;
                case OpCodeCodes.Ldc_I4_5: code = OpCodeCodes.Ldc_I4; operand = 5; break;
                case OpCodeCodes.Ldc_I4_6: code = OpCodeCodes.Ldc_I4; operand = 6; break;
                case OpCodeCodes.Ldc_I4_7: code = OpCodeCodes.Ldc_I4; operand = 7; break;
                case OpCodeCodes.Ldc_I4_8: code = OpCodeCodes.Ldc_I4; operand = 8; break;
                case OpCodeCodes.Ldc_I4_S: code = OpCodeCodes.Ldc_I4; operand = (int)(sbyte)operand; break;

                case OpCodeCodes.Br: 
                case OpCodeCodes.Br_S: code = OpCodeCodes.Br; operand = instructions.Single(i => i.Address == inst.BrachILAddress); break;
                
                case OpCodeCodes.Brfalse: 
                case OpCodeCodes.Brfalse_S: code = OpCodeCodes.Brfalse; operand = instructions.Single(i => i.Address == inst.BrachILAddress); break;

                case OpCodeCodes.Brtrue:
                case OpCodeCodes.Brtrue_S: code = OpCodeCodes.Brtrue; operand = instructions.Single(i => i.Address == inst.BrachILAddress); break;
                
                case OpCodeCodes.Beq:
                case OpCodeCodes.Beq_S: code = OpCodeCodes.Beq; operand = instructions.Single(i => i.Address == inst.BrachILAddress); break;
                
                case OpCodeCodes.Bge:
                case OpCodeCodes.Bge_S: code = OpCodeCodes.Bge; operand = instructions.Single(i => i.Address == inst.BrachILAddress); break;
                
                case OpCodeCodes.Bgt:
                case OpCodeCodes.Bgt_S: code = OpCodeCodes.Bgt; operand = instructions.Single(i => i.Address == inst.BrachILAddress); break;
                
                case OpCodeCodes.Ble:
                case OpCodeCodes.Ble_S: code = OpCodeCodes.Ble; operand = instructions.Single(i => i.Address == inst.BrachILAddress); break;
                
                case OpCodeCodes.Blt:
                case OpCodeCodes.Blt_S: code = OpCodeCodes.Blt; operand = instructions.Single(i => i.Address == inst.BrachILAddress); break;
                
                case OpCodeCodes.Bne_Un:
                case OpCodeCodes.Bne_Un_S: code = OpCodeCodes.Bne_Un; operand = instructions.Single(i => i.Address == inst.BrachILAddress); break;
                
                case OpCodeCodes.Bge_Un:
                case OpCodeCodes.Bge_Un_S: code = OpCodeCodes.Bge_Un; operand = instructions.Single(i => i.Address == inst.BrachILAddress); break;
                
                case OpCodeCodes.Bgt_Un:
                case OpCodeCodes.Bgt_Un_S: code = OpCodeCodes.Bgt_Un; operand = instructions.Single(i => i.Address == inst.BrachILAddress); break;
                
                case OpCodeCodes.Ble_Un:
                case OpCodeCodes.Ble_Un_S: code = OpCodeCodes.Ble_Un; operand = instructions.Single(i => i.Address == inst.BrachILAddress); break;
                
                case OpCodeCodes.Blt_Un:
                case OpCodeCodes.Blt_Un_S: code = OpCodeCodes.Blt_Un; operand = instructions.Single(i => i.Address == inst.BrachILAddress); break;
                
                case OpCodeCodes.Leave:
                case OpCodeCodes.Leave_S: code = OpCodeCodes.Leave; operand = instructions.Single(i => i.Address == inst.BrachILAddress); break;
            }
        }   
    }
}
