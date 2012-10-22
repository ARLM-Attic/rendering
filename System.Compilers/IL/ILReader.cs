using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;
using System.Runtime.CompilerServices;

namespace System.Compilers.IL
{
    /// <summary>
    /// Class used to read an il body from an assembly, producing a secuence of ILInstruction objects.
    /// </summary>
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

        RuntimeFieldHandle? ResolveFieldHandle(int token)
        {
            try
            {
                var field = ResolveField(token);
                if (field != null)
                    return field.FieldHandle;
                return null;
            }
            catch
            {
                return null;
            }
            
        }

        RuntimeMethodHandle? ResolveMethodHandle(int token)
        {
            try
            {
                var method = ResolveMethod(token);
                if (method != null)
                    return method.MethodHandle;
                return null;
            }
            catch
            {
                return null;
            }
        }

        RuntimeTypeHandle? ResolveTypeHandle(int token)
        {
            try
            {
                var type = ResolveType(token);
                if (type != null)
                    return type.TypeHandle;
                return null;
            }
            catch
            {
                return null;
            }
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
                    args = new object[] { (object)ResolveFieldHandle(token) ?? (object)ResolveMethodHandle(token) ?? (object)ResolveTypeHandle(token) };
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
            var result = new ILInstruction(currentOpCode, position, (int)(binary.BaseStream.Position - position), args);
            return result;
        }

        public IEnumerable<ILInstruction> GetIL(MethodBase method)
        {
            List<ILInstruction> il = new List<ILInstruction>();
            MethodBody body = method.GetMethodBody();
            BinaryReader br = new BinaryReader(new MemoryStream(body.GetILAsByteArray()));
            while (br.BaseStream.Position < br.BaseStream.Length)
                il.Add(ReadFrom(br));

            AnalizeArrayInitializations(il, method, body);
            return il;
        }

        private void AnalizeArrayInitializations(List<ILInstruction> il, MethodBase method, MethodBody body)
        {
            var ilArray = il.ToArray();
            for (int i = 2; i < il.Count - 1; i++)
            {
                if (il[i].OpCode == OpCodes.Ldelema && il[i + 1].OpCode == OpCodes.Dup)
                {
                    var typeToken = ((Type)il[i].Operand);
                    il[i + 1] = new ILInstruction(OpCodes.Ldelema, il[i + 1].Address, 4, new object[] { typeToken });

                    il.Insert(i + 1, new ILInstruction(il[i - 1].OpCode, -1, 4, il[i - 1].Args));
                    il.Insert(i + 1, new ILInstruction(il[i - 2].OpCode, -1, 4, il[i - 2].Args));
                    i--;
                }
                else if (il[i].OpCode == OpCodes.Ldelema &&  il[i + 1].OpCode == OpCodes.Ldobj)
                {
                    var ldobjIndex = i + 1;
                    var typeToken = ((Type)il[i].Operand);
                    il[ldobjIndex] = new ILInstruction(OpCodes.Ldelem, il[ldobjIndex].Address, 4, new object[] { typeToken });

                    il.RemoveAt(i);

                    i--;
                }
                else if (il[i].OpCode == OpCodes.Ldelema && il[i + 1].OpCode == OpCodes.Ldfld)
                {
                    il[i] = new ILInstruction(OpCodes.Ldelem, il[i].Address, 4, il[i].Args);

                    //il.RemoveAt(i);
                    //i--;
                }
                else if (il[i].OpCode == OpCodes.Ldelema)
                {
                    var stobjIndex = -1;
                    for (int j = i + 1; j < il.Count; j++)
                        if (il[j].OpCode == OpCodes.Stobj)
                        {
                            stobjIndex = j;
                            break;
                        }

                    if (stobjIndex > 0)
                    {
                        var typeToken = ((Type)il[i].Operand).TypeHandle.Value;
                        il[stobjIndex] = new ILInstruction(OpCodes.Stelem, il[stobjIndex].Address, 4, new object[] { typeToken });

                        il.RemoveAt(i);

                        i--;// = stobjIndex - 1;
                    }
                }

            }

        }
    }

    /// <summary>
    /// Represents an IL instruction.
    /// </summary>
    public class ILInstruction
    {
        OpCode _operation;
        object[] _params;
        int position;
        int size;

        /// <summary>
        /// OpCode this instruction will execute.
        /// </summary>
        public OpCode OpCode { get { return _operation; } }

        /// <summary>
        /// Determines if this instruction has at least one operand.
        /// </summary>
        public bool HasOperand { get { return _params.Length > 0; } }

        /// <summary>
        /// Gets the first operand of this instruction, null if it has not an operand.
        /// </summary>
        public object Operand { get { return HasOperand ? _params[0] : null; } }

        /// <summary>
        /// Gets the set of operands of this instruction.
        /// </summary>
        public object[] Args { get { return _params; } }

        /// <summary>
        /// Get the address this instruction have in the body.
        /// </summary>
        public int Address { get { return position; } }

        /// <summary>
        /// Allows to clone this instruction information.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the size (in bytes) of this instruction at the byte-code secuence.
        /// </summary>
        private int Size { get { return size; } }

        /// <summary>
        /// Gets when this instruction is a branch.
        /// </summary>
        public bool IsBranch
        {
            get { return OpCode.FlowControl == FlowControl.Branch || OpCode.FlowControl == FlowControl.Cond_Branch; }
        }

        /// <summary>
        /// Gets when this instruction is a conditional branch.
        /// </summary>
        public bool IsConditionalBranch
        {
            get { return OpCode.FlowControl == FlowControl.Cond_Branch; }
        }

        /// <summary>
        /// Gets the address this branch jump to.
        /// </summary>
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
        
        /// <summary>
        /// Redefines a readable form of this instruction instance.
        /// </summary>
        public override string ToString()
        {
            string argList = "";
            for (int i = 0; i < Args.Length; i++)
            {
                if (IsBranch)
                {
                    int arg = 0;
                    if (OpCode.OperandType == OperandType.ShortInlineBrTarget)
                    {
                        arg = ((int)((sbyte)Args[0]) + this.Address + this.Size);
                    }
                    else if (OpCode.OperandType == OperandType.InlineBrTarget)
                    {
                        arg = ((int)((int)Args[0]) + this.Address + this.Size);
                    }

                    argList += (arg) + ": IL_" + BrachILAddress.ToString("X");
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
