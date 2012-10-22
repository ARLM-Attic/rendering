using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;

namespace System.Compilers
{
    class ILReader
    {
        Assembly assembly;

        public ILReader(Assembly assembly)
        {
            this.assembly = assembly;
        }

        static Dictionary<int, FieldInfo> fields = new Dictionary<int, FieldInfo>();

        FieldInfo ResolveField(int token)
        {
            if (fields.ContainsKey(token))
                return fields[token];
            lock (fields)
            {
                FieldInfo field = assembly.ManifestModule.ResolveField(token);
                if (field != null)
                {
                    fields.Add(token, field);
                    return field;
                }
            }

            return null;
        }

        MethodBase ResolveMethod(int token)
        {
            lock (fields)
            {
                MethodBase method = (MethodBase)assembly.ManifestModule.ResolveMethod(token);
                if (method != null)
                    return method;
            }
            return null;
        }

        String ResolveString(int token)
        {
            lock (fields)
            {
                String _string = assembly.ManifestModule.ResolveString(token);
                if (_string != null)
                    return _string;
            }
            return null;
        }

        Type ResolveType(int token)
        {
            lock (fields)
            {
                Type type = assembly.ManifestModule.ResolveType(token);
                if (type != null)
                    return type;
            }
            return null;
        }

        static IEnumerable<OpCode> allOpcodes = new Func<IEnumerable<OpCode>>(() =>
        {
            FieldInfo[] fields = typeof(OpCodes).GetFields();
            List<OpCode> opCodes = new List<OpCode>();
            foreach (var f in fields)
                if (f.FieldType == typeof(OpCode))
                    opCodes.Add((OpCode)f.GetValue(null));
            return opCodes;
        })();

        static IEnumerable<OpCode> AllOpcodes
        {
            get
            {
                return allOpcodes;
            }
        }

        private ILInstruction ReadFrom(BinaryReader binary)
        {
            int position = (int)binary.BaseStream.Position;

            byte opCodeValue = binary.ReadByte();
            OpCode currentOpCode = new OpCode();
            foreach (OpCode opcode in AllOpcodes)
                if (opcode.Value == opCodeValue)
                    currentOpCode = opcode;

            if (currentOpCode.FlowControl == FlowControl.Meta)
            {
                byte opCodeValue2 = binary.ReadByte();

                unchecked
                {
                    short s = (short)((opCodeValue << 8) + (opCodeValue2 << 0));
                    foreach (OpCode opcode in AllOpcodes)
                        if (opcode.Value == s)
                            currentOpCode = opcode;
                }
            }

            object[] args = new object[0];
            int token = 0;
            switch (currentOpCode.OperandType)
            {
                case OperandType.InlineBrTarget:
                    args = new object[] { binary.ReadInt32() };
                    break;
                case OperandType.InlineField:
                    token = binary.ReadInt32();
                    args = new object[] { ResolveField(token) };
                    break;
                case OperandType.InlineI:
                    args = new object[] { binary.ReadInt32() };
                    break;
                case OperandType.InlineI8:
                    args = new object[] { binary.ReadInt64() };
                    break;
                case OperandType.InlineMethod:
                    token = binary.ReadInt32();
                    args = new object[] { ResolveMethod(token) };
                    break;
                case OperandType.InlineNone:
                    args = new object[] { };
                    break;
                case OperandType.InlineR:
                    args = new object[] { binary.ReadDouble() };
                    break;
                case OperandType.InlineSig:
                    args = new object[] { binary.ReadInt32() };
                    break;
                case OperandType.InlineString:
                    token = binary.ReadInt32();
                    args = new object[] { ResolveString(token) };
                    break;
                case OperandType.InlineSwitch:
                    int count = binary.ReadInt32();
                    int[] _switches = new int[count];
                    for (int i = 0; i < _switches.Length; i++)
                        _switches[i] = binary.ReadInt32();
                    args = new object[count];
                    for (int i = 0; i < count; i++)
                        args[i] = _switches[i];
                    break;
                case OperandType.InlineTok:
                    token = binary.ReadInt32();
                    args = new object[] { token };
                    break;
                case OperandType.InlineType:
                    token = binary.ReadInt32();
                    args = new object[] { ResolveType(token) };
                    break;
                case OperandType.InlineVar:
                    short id = binary.ReadInt16();
                    args = new object[] { id };
                    break;
                case OperandType.ShortInlineBrTarget:
                    sbyte _id = (sbyte)(binary.ReadSByte());
                    args = new object[] { _id };
                    break;
                case OperandType.ShortInlineI:
                    _id = binary.ReadSByte();
                    args = new object[] { _id };
                    break;
                case OperandType.ShortInlineR:
                    float v = binary.ReadSingle();
                    args = new object[] { v };
                    break;
                case OperandType.ShortInlineVar:
                    _id = binary.ReadSByte();
                    args = new object[] { _id };
                    break;
            }

            return new ILInstruction(currentOpCode, position, (int)(binary.BaseStream.Position - position), args);
        }

        public IEnumerable<ILInstruction> GetIL(MethodInfo method)
        {
            List<ILInstruction> il = new List<ILInstruction>();
            MethodBody body = method.GetMethodBody();
            BinaryReader br = new BinaryReader(new MemoryStream(body.GetILAsByteArray()));
            while (br.BaseStream.Position < br.BaseStream.Length)
                il.Add(ReadFrom(br));
            return il;
        }

        int NextBlockStart(List<ILInstruction> instructions, int start)
        {
            do
            {
                start = instructions.FindIndex(start, (inst) => { return inst.OpCode == OpCodes.Ldc_I4_M1; });
                if (start == -1)
                    return -1;
                if (instructions[start + 1].OpCode == OpCodes.Stfld && ((FieldInfo)instructions[start + 1].Operand).Name == "<>1__state")
                    return start + 2;
                start++;
            } while (true);
        }
        int NextBlockEnd(List<ILInstruction> instructions, int start)
        {
            do
            {
                start = instructions.FindIndex(start, (inst) => { return inst.OpCode == OpCodes.Stfld; });
                if (start == -1)
                    return -1;
                if (((FieldInfo)instructions[start].Operand).Name == "<>2__current")
                    return start - 1;
                start++;
            } while (true);
        }

        private IEnumerable<ILInstruction> GetILInvolvedOnLeazyMethod(MethodInfo method)
        {
            List<ILInstruction> il = new List<ILInstruction>(GetILForLeazyMethod(method));
            int start = 0, finish = 0;
            do
            {
                start = NextBlockStart(il, start);
                finish = NextBlockEnd(il, start);

                if (start == -1 || finish == -1)
                    yield break;

                for (int i = start; i <= finish; i++)
                    yield return il[i];
                yield return new ILInstruction(OpCodes.Ret, il[finish + 1].Address, 1);

            } while (true);
        }

        private IEnumerable<ILInstruction> GetILForLeazyMethod(MethodInfo method)
        {
            //string innerclassname = method.ReflectedType.FullName + "+<" + method.Name + ">d_0";
            Type type = method.ReflectedType.GetNestedType("<" + method.Name + ">d__0", BindingFlags.NonPublic);

            List<ILInstruction> fullIL = new List<ILInstruction>(GetIL(type.GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)));

            return fullIL;
        }
    }

    public class ILInstruction
    {
        OpCode _operation;
        object[] _params;
        int position;
        int size;

        public OpCode OpCode { get { return _operation; } }

        public bool HasOperand { get { return _params.Length > 0; } }

        public object Operand { get { return HasOperand ? _params[0] : null; } }

        public object[] Args { get { return _params; } }

        public int Address { get { return position; } }

        public ILInstruction Clone()
        {
            return new ILInstruction(this.OpCode, this.position, this.size, this._params);
        }

        internal ILInstruction(OpCode op, int address, int size, params object[] args)
        {
            this._operation = op;
            this._params = args;
            this.position = address;
            this.size = size;
        }

        public int Size { get { return size; } }

        public bool IsBranch
        {
            get { return OpCode.FlowControl == FlowControl.Branch || OpCode.FlowControl == FlowControl.Cond_Branch; }
        }
        public bool IsConditionalBranch
        {
            get { return OpCode.FlowControl == FlowControl.Cond_Branch; }
        }

        public int BrachILAddress
        {
            get
            {
                if (OpCode.OperandType == OperandType.ShortInlineBrTarget)
                {
                    return ((int)((sbyte)Args[0]) + this.Address + this.Size);
                }

                if (OpCode.OperandType == OperandType.InlineBrTarget)
                {
                    return ((int)((int)Args[0]) + this.Address + this.Size);
                }
                return -1;

            }
        }

        public override string ToString()
        {
            string argList = "";
            for (int i = 0; i < Args.Length; i++)
            {
                if (IsBranch)
                {
                    argList += ((sbyte)Args[i]) + ": IL_" + BrachILAddress.ToString("X");
                    continue;
                }

                if (OpCode.OperandType == OperandType.InlineSwitch)
                {
                    argList += ((int)Args[i]).ToString("X");

                    if (i < Args.Length - 1)
                        argList += ", ";
                    continue;
                }

                argList += Args[i];
            }
            //if (Args.Length > 1)
            argList = "(" + argList + ")";
            return "IL_" + Address.ToString("X") + ": " + OpCode.Name + " " + argList;
        }
    }

}
