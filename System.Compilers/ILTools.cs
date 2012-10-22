using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace System.Compilers
{
    public static class ILTools
    {
        public static int CountOfPops(this ILInstruction instruction)
        {
            switch (instruction.OpCode.StackBehaviourPop)
            {
                case StackBehaviour.Varpop:
                    if (instruction.Operand is MethodBase)
                    {
                        MethodBase method = instruction.Operand as MethodBase;
                        return method.GetParameters().Length + (method.IsStatic || method.IsConstructor ? 0 : 1);
                    }
                    if (instruction.OpCode == OpCodes.Ret)
                    {
                        return instruction.Size;
                    }
                    return 0;
                case StackBehaviour.Pop0:
                    return 0;
                case StackBehaviour.Popi:
                case StackBehaviour.Popref:
                case StackBehaviour.Pop1:
                    return 1;
                case StackBehaviour.Popi_pop1:
                case StackBehaviour.Popi_popi:
                case StackBehaviour.Popi_popi8:
                case StackBehaviour.Popi_popr4:
                case StackBehaviour.Popi_popr8:
                case StackBehaviour.Popref_pop1:
                case StackBehaviour.Popref_popi:
                case StackBehaviour.Pop1_pop1:
                    return 2;
                case StackBehaviour.Popref_popi_pop1:
                case StackBehaviour.Popref_popi_popi:
                case StackBehaviour.Popref_popi_popi8:
                case StackBehaviour.Popref_popi_popr4:
                case StackBehaviour.Popref_popi_popr8:
                case StackBehaviour.Popref_popi_popref:
                case StackBehaviour.Popi_popi_popi:
                    return 3;
            }

            return 0;
        }

        public static int CountOfPushes(this ILInstruction instruction)
        {
            OpCode opCode = instruction.OpCode;
            switch (opCode.StackBehaviourPush)
            {
                case StackBehaviour.Pushi:
                case StackBehaviour.Pushi8:
                case StackBehaviour.Pushr4:
                case StackBehaviour.Pushr8:
                case StackBehaviour.Pushref:
                case StackBehaviour.Varpush:
                case StackBehaviour.Push1: return 1;
            }
            return 0;
        }

        public static bool IsStatement(this ILInstruction instruction)
        {
            var opCode = instruction.OpCode;
            return opCode.OperandType == OperandType.InlineBrTarget || opCode.OperandType == OperandType.InlineMethod || opCode.OperandType == OperandType.InlineSwitch || opCode.OperandType == OperandType.ShortInlineBrTarget
                || opCode == OpCodes.Starg || opCode == OpCodes.Stfld;
        }
    }


}
