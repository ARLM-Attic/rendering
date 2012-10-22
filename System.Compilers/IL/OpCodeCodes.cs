using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Compilers.IL
{
    #region OpCode Codes
    /// <summary>
    /// This enum allows to access to the opcodes as numbers.
    /// </summary>
    internal enum OpCodeCodes
    {
        /// <summary>
        /// nop
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Nop = 0,
        /// <summary>
        /// break
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Break = 1,
        /// <summary>
        /// ldarg.0
        /// Pop: Pop0
        /// Push: Push1
        /// </summary>
        Ldarg_0 = 2,
        /// <summary>
        /// ldarg.1
        /// Pop: Pop0
        /// Push: Push1
        /// </summary>
        Ldarg_1 = 3,
        /// <summary>
        /// ldarg.2
        /// Pop: Pop0
        /// Push: Push1
        /// </summary>
        Ldarg_2 = 4,
        /// <summary>
        /// ldarg.3
        /// Pop: Pop0
        /// Push: Push1
        /// </summary>
        Ldarg_3 = 5,
        /// <summary>
        /// ldloc.0
        /// Pop: Pop0
        /// Push: Push1
        /// </summary>
        Ldloc_0 = 6,
        /// <summary>
        /// ldloc.1
        /// Pop: Pop0
        /// Push: Push1
        /// </summary>
        Ldloc_1 = 7,
        /// <summary>
        /// ldloc.2
        /// Pop: Pop0
        /// Push: Push1
        /// </summary>
        Ldloc_2 = 8,
        /// <summary>
        /// ldloc.3
        /// Pop: Pop0
        /// Push: Push1
        /// </summary>
        Ldloc_3 = 9,
        /// <summary>
        /// stloc.0
        /// Pop: Pop1
        /// Push: Push0
        /// </summary>
        Stloc_0 = 10,
        /// <summary>
        /// stloc.1
        /// Pop: Pop1
        /// Push: Push0
        /// </summary>
        Stloc_1 = 11,
        /// <summary>
        /// stloc.2
        /// Pop: Pop1
        /// Push: Push0
        /// </summary>
        Stloc_2 = 12,
        /// <summary>
        /// stloc.3
        /// Pop: Pop1
        /// Push: Push0
        /// </summary>
        Stloc_3 = 13,
        /// <summary>
        /// ldarg.s
        /// Pop: Pop0
        /// Push: Push1
        /// </summary>
        Ldarg_S = 14,
        /// <summary>
        /// ldarga.s
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldarga_S = 15,
        /// <summary>
        /// starg.s
        /// Pop: Pop1
        /// Push: Push0
        /// </summary>
        Starg_S = 16,
        /// <summary>
        /// ldloc.s
        /// Pop: Pop0
        /// Push: Push1
        /// </summary>
        Ldloc_S = 17,
        /// <summary>
        /// ldloca.s
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldloca_S = 18,
        /// <summary>
        /// stloc.s
        /// Pop: Pop1
        /// Push: Push0
        /// </summary>
        Stloc_S = 19,
        /// <summary>
        /// ldnull
        /// Pop: Pop0
        /// Push: Pushref
        /// </summary>
        Ldnull = 20,
        /// <summary>
        /// ldc.i4.m1
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldc_I4_M1 = 21,
        /// <summary>
        /// ldc.i4.0
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldc_I4_0 = 22,
        /// <summary>
        /// ldc.i4.1
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldc_I4_1 = 23,
        /// <summary>
        /// ldc.i4.2
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldc_I4_2 = 24,
        /// <summary>
        /// ldc.i4.3
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldc_I4_3 = 25,
        /// <summary>
        /// ldc.i4.4
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldc_I4_4 = 26,
        /// <summary>
        /// ldc.i4.5
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldc_I4_5 = 27,
        /// <summary>
        /// ldc.i4.6
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldc_I4_6 = 28,
        /// <summary>
        /// ldc.i4.7
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldc_I4_7 = 29,
        /// <summary>
        /// ldc.i4.8
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldc_I4_8 = 30,
        /// <summary>
        /// ldc.i4.s
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldc_I4_S = 31,
        /// <summary>
        /// ldc.i4
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldc_I4 = 32,
        /// <summary>
        /// ldc.i8
        /// Pop: Pop0
        /// Push: Pushi8
        /// </summary>
        Ldc_I8 = 33,
        /// <summary>
        /// ldc.r4
        /// Pop: Pop0
        /// Push: Pushr4
        /// </summary>
        Ldc_R4 = 34,
        /// <summary>
        /// ldc.r8
        /// Pop: Pop0
        /// Push: Pushr8
        /// </summary>
        Ldc_R8 = 35,
        /// <summary>
        /// dup
        /// Pop: Pop1
        /// Push: Push1_push1
        /// </summary>
        Dup = 37,
        /// <summary>
        /// pop
        /// Pop: Pop1
        /// Push: Push0
        /// </summary>
        Pop = 38,
        /// <summary>
        /// jmp
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Jmp = 39,
        /// <summary>
        /// call
        /// Pop: Varpop
        /// Push: Varpush
        /// </summary>
        Call = 40,
        /// <summary>
        /// calli
        /// Pop: Varpop
        /// Push: Varpush
        /// </summary>
        Calli = 41,
        /// <summary>
        /// ret
        /// Pop: Varpop
        /// Push: Push0
        /// </summary>
        Ret = 42,
        /// <summary>
        /// br.s
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Br_S = 43,
        /// <summary>
        /// brfalse.s
        /// Pop: Popi
        /// Push: Push0
        /// </summary>
        Brfalse_S = 44,
        /// <summary>
        /// brtrue.s
        /// Pop: Popi
        /// Push: Push0
        /// </summary>
        Brtrue_S = 45,
        /// <summary>
        /// beq.s
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Beq_S = 46,
        /// <summary>
        /// bge.s
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Bge_S = 47,
        /// <summary>
        /// bgt.s
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Bgt_S = 48,
        /// <summary>
        /// ble.s
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Ble_S = 49,
        /// <summary>
        /// blt.s
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Blt_S = 50,
        /// <summary>
        /// bne.un.s
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Bne_Un_S = 51,
        /// <summary>
        /// bge.un.s
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Bge_Un_S = 52,
        /// <summary>
        /// bgt.un.s
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Bgt_Un_S = 53,
        /// <summary>
        /// ble.un.s
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Ble_Un_S = 54,
        /// <summary>
        /// blt.un.s
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Blt_Un_S = 55,
        /// <summary>
        /// br
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Br = 56,
        /// <summary>
        /// brfalse
        /// Pop: Popi
        /// Push: Push0
        /// </summary>
        Brfalse = 57,
        /// <summary>
        /// brtrue
        /// Pop: Popi
        /// Push: Push0
        /// </summary>
        Brtrue = 58,
        /// <summary>
        /// beq
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Beq = 59,
        /// <summary>
        /// bge
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Bge = 60,
        /// <summary>
        /// bgt
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Bgt = 61,
        /// <summary>
        /// ble
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Ble = 62,
        /// <summary>
        /// blt
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Blt = 63,
        /// <summary>
        /// bne.un
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Bne_Un = 64,
        /// <summary>
        /// bge.un
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Bge_Un = 65,
        /// <summary>
        /// bgt.un
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Bgt_Un = 66,
        /// <summary>
        /// ble.un
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Ble_Un = 67,
        /// <summary>
        /// blt.un
        /// Pop: Pop1_pop1
        /// Push: Push0
        /// </summary>
        Blt_Un = 68,
        /// <summary>
        /// switch
        /// Pop: Popi
        /// Push: Push0
        /// </summary>
        Switch = 69,
        /// <summary>
        /// ldind.i1
        /// Pop: Popi
        /// Push: Pushi
        /// </summary>
        Ldind_I1 = 70,
        /// <summary>
        /// ldind.u1
        /// Pop: Popi
        /// Push: Pushi
        /// </summary>
        Ldind_U1 = 71,
        /// <summary>
        /// ldind.i2
        /// Pop: Popi
        /// Push: Pushi
        /// </summary>
        Ldind_I2 = 72,
        /// <summary>
        /// ldind.u2
        /// Pop: Popi
        /// Push: Pushi
        /// </summary>
        Ldind_U2 = 73,
        /// <summary>
        /// ldind.i4
        /// Pop: Popi
        /// Push: Pushi
        /// </summary>
        Ldind_I4 = 74,
        /// <summary>
        /// ldind.u4
        /// Pop: Popi
        /// Push: Pushi
        /// </summary>
        Ldind_U4 = 75,
        /// <summary>
        /// ldind.i8
        /// Pop: Popi
        /// Push: Pushi8
        /// </summary>
        Ldind_I8 = 76,
        /// <summary>
        /// ldind.i
        /// Pop: Popi
        /// Push: Pushi
        /// </summary>
        Ldind_I = 77,
        /// <summary>
        /// ldind.r4
        /// Pop: Popi
        /// Push: Pushr4
        /// </summary>
        Ldind_R4 = 78,
        /// <summary>
        /// ldind.r8
        /// Pop: Popi
        /// Push: Pushr8
        /// </summary>
        Ldind_R8 = 79,
        /// <summary>
        /// ldind.ref
        /// Pop: Popi
        /// Push: Pushref
        /// </summary>
        Ldind_Ref = 80,
        /// <summary>
        /// stind.ref
        /// Pop: Popi_popi
        /// Push: Push0
        /// </summary>
        Stind_Ref = 81,
        /// <summary>
        /// stind.i1
        /// Pop: Popi_popi
        /// Push: Push0
        /// </summary>
        Stind_I1 = 82,
        /// <summary>
        /// stind.i2
        /// Pop: Popi_popi
        /// Push: Push0
        /// </summary>
        Stind_I2 = 83,
        /// <summary>
        /// stind.i4
        /// Pop: Popi_popi
        /// Push: Push0
        /// </summary>
        Stind_I4 = 84,
        /// <summary>
        /// stind.i8
        /// Pop: Popi_popi8
        /// Push: Push0
        /// </summary>
        Stind_I8 = 85,
        /// <summary>
        /// stind.r4
        /// Pop: Popi_popr4
        /// Push: Push0
        /// </summary>
        Stind_R4 = 86,
        /// <summary>
        /// stind.r8
        /// Pop: Popi_popr8
        /// Push: Push0
        /// </summary>
        Stind_R8 = 87,
        /// <summary>
        /// add
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Add = 88,
        /// <summary>
        /// sub
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Sub = 89,
        /// <summary>
        /// mul
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Mul = 90,
        /// <summary>
        /// div
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Div = 91,
        /// <summary>
        /// div.un
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Div_Un = 92,
        /// <summary>
        /// rem
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Rem = 93,
        /// <summary>
        /// rem.un
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Rem_Un = 94,
        /// <summary>
        /// and
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        And = 95,
        /// <summary>
        /// or
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Or = 96,
        /// <summary>
        /// xor
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Xor = 97,
        /// <summary>
        /// shl
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Shl = 98,
        /// <summary>
        /// shr
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Shr = 99,
        /// <summary>
        /// shr.un
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Shr_Un = 100,
        /// <summary>
        /// neg
        /// Pop: Pop1
        /// Push: Push1
        /// </summary>
        Neg = 101,
        /// <summary>
        /// not
        /// Pop: Pop1
        /// Push: Push1
        /// </summary>
        Not = 102,
        /// <summary>
        /// conv.i1
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_I1 = 103,
        /// <summary>
        /// conv.i2
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_I2 = 104,
        /// <summary>
        /// conv.i4
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_I4 = 105,
        /// <summary>
        /// conv.i8
        /// Pop: Pop1
        /// Push: Pushi8
        /// </summary>
        Conv_I8 = 106,
        /// <summary>
        /// conv.r4
        /// Pop: Pop1
        /// Push: Pushr4
        /// </summary>
        Conv_R4 = 107,
        /// <summary>
        /// conv.r8
        /// Pop: Pop1
        /// Push: Pushr8
        /// </summary>
        Conv_R8 = 108,
        /// <summary>
        /// conv.u4
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_U4 = 109,
        /// <summary>
        /// conv.u8
        /// Pop: Pop1
        /// Push: Pushi8
        /// </summary>
        Conv_U8 = 110,
        /// <summary>
        /// callvirt
        /// Pop: Varpop
        /// Push: Varpush
        /// </summary>
        Callvirt = 111,
        /// <summary>
        /// cpobj
        /// Pop: Popi_popi
        /// Push: Push0
        /// </summary>
        Cpobj = 112,
        /// <summary>
        /// ldobj
        /// Pop: Popi
        /// Push: Push1
        /// </summary>
        Ldobj = 113,
        /// <summary>
        /// ldstr
        /// Pop: Pop0
        /// Push: Pushref
        /// </summary>
        Ldstr = 114,
        /// <summary>
        /// newobj
        /// Pop: Varpop
        /// Push: Pushref
        /// </summary>
        Newobj = 115,
        /// <summary>
        /// castclass
        /// Pop: Popref
        /// Push: Pushref
        /// </summary>
        Castclass = 116,
        /// <summary>
        /// isinst
        /// Pop: Popref
        /// Push: Pushi
        /// </summary>
        Isinst = 117,
        /// <summary>
        /// conv.r.un
        /// Pop: Pop1
        /// Push: Pushr8
        /// </summary>
        Conv_R_Un = 118,
        /// <summary>
        /// unbox
        /// Pop: Popref
        /// Push: Pushi
        /// </summary>
        Unbox = 121,
        /// <summary>
        /// throw
        /// Pop: Popref
        /// Push: Push0
        /// </summary>
        Throw = 122,
        /// <summary>
        /// ldfld
        /// Pop: Popref
        /// Push: Push1
        /// </summary>
        Ldfld = 123,
        /// <summary>
        /// ldflda
        /// Pop: Popref
        /// Push: Pushi
        /// </summary>
        Ldflda = 124,
        /// <summary>
        /// stfld
        /// Pop: Popref_pop1
        /// Push: Push0
        /// </summary>
        Stfld = 125,
        /// <summary>
        /// ldsfld
        /// Pop: Pop0
        /// Push: Push1
        /// </summary>
        Ldsfld = 126,
        /// <summary>
        /// ldsflda
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldsflda = 127,
        /// <summary>
        /// stsfld
        /// Pop: Pop1
        /// Push: Push0
        /// </summary>
        Stsfld = 128,
        /// <summary>
        /// stobj
        /// Pop: Popi_pop1
        /// Push: Push0
        /// </summary>
        Stobj = 129,
        /// <summary>
        /// conv.ovf.i1.un
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_I1_Un = 130,
        /// <summary>
        /// conv.ovf.i2.un
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_I2_Un = 131,
        /// <summary>
        /// conv.ovf.i4.un
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_I4_Un = 132,
        /// <summary>
        /// conv.ovf.i8.un
        /// Pop: Pop1
        /// Push: Pushi8
        /// </summary>
        Conv_Ovf_I8_Un = 133,
        /// <summary>
        /// conv.ovf.u1.un
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_U1_Un = 134,
        /// <summary>
        /// conv.ovf.u2.un
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_U2_Un = 135,
        /// <summary>
        /// conv.ovf.u4.un
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_U4_Un = 136,
        /// <summary>
        /// conv.ovf.u8.un
        /// Pop: Pop1
        /// Push: Pushi8
        /// </summary>
        Conv_Ovf_U8_Un = 137,
        /// <summary>
        /// conv.ovf.i.un
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_I_Un = 138,
        /// <summary>
        /// conv.ovf.u.un
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_U_Un = 139,
        /// <summary>
        /// box
        /// Pop: Pop1
        /// Push: Pushref
        /// </summary>
        Box = 140,
        /// <summary>
        /// newarr
        /// Pop: Popi
        /// Push: Pushref
        /// </summary>
        Newarr = 141,
        /// <summary>
        /// ldlen
        /// Pop: Popref
        /// Push: Pushi
        /// </summary>
        Ldlen = 142,
        /// <summary>
        /// ldelema
        /// Pop: Popref_popi
        /// Push: Pushi
        /// </summary>
        Ldelema = 143,
        /// <summary>
        /// ldelem.i1
        /// Pop: Popref_popi
        /// Push: Pushi
        /// </summary>
        Ldelem_I1 = 144,
        /// <summary>
        /// ldelem.u1
        /// Pop: Popref_popi
        /// Push: Pushi
        /// </summary>
        Ldelem_U1 = 145,
        /// <summary>
        /// ldelem.i2
        /// Pop: Popref_popi
        /// Push: Pushi
        /// </summary>
        Ldelem_I2 = 146,
        /// <summary>
        /// ldelem.u2
        /// Pop: Popref_popi
        /// Push: Pushi
        /// </summary>
        Ldelem_U2 = 147,
        /// <summary>
        /// ldelem.i4
        /// Pop: Popref_popi
        /// Push: Pushi
        /// </summary>
        Ldelem_I4 = 148,
        /// <summary>
        /// ldelem.u4
        /// Pop: Popref_popi
        /// Push: Pushi
        /// </summary>
        Ldelem_U4 = 149,
        /// <summary>
        /// ldelem.i8
        /// Pop: Popref_popi
        /// Push: Pushi8
        /// </summary>
        Ldelem_I8 = 150,
        /// <summary>
        /// ldelem.i
        /// Pop: Popref_popi
        /// Push: Pushi
        /// </summary>
        Ldelem_I = 151,
        /// <summary>
        /// ldelem.r4
        /// Pop: Popref_popi
        /// Push: Pushr4
        /// </summary>
        Ldelem_R4 = 152,
        /// <summary>
        /// ldelem.r8
        /// Pop: Popref_popi
        /// Push: Pushr8
        /// </summary>
        Ldelem_R8 = 153,
        /// <summary>
        /// ldelem.ref
        /// Pop: Popref_popi
        /// Push: Pushref
        /// </summary>
        Ldelem_Ref = 154,
        /// <summary>
        /// stelem.i
        /// Pop: Popref_popi_popi
        /// Push: Push0
        /// </summary>
        Stelem_I = 155,
        /// <summary>
        /// stelem.i1
        /// Pop: Popref_popi_popi
        /// Push: Push0
        /// </summary>
        Stelem_I1 = 156,
        /// <summary>
        /// stelem.i2
        /// Pop: Popref_popi_popi
        /// Push: Push0
        /// </summary>
        Stelem_I2 = 157,
        /// <summary>
        /// stelem.i4
        /// Pop: Popref_popi_popi
        /// Push: Push0
        /// </summary>
        Stelem_I4 = 158,
        /// <summary>
        /// stelem.i8
        /// Pop: Popref_popi_popi8
        /// Push: Push0
        /// </summary>
        Stelem_I8 = 159,
        /// <summary>
        /// stelem.r4
        /// Pop: Popref_popi_popr4
        /// Push: Push0
        /// </summary>
        Stelem_R4 = 160,
        /// <summary>
        /// stelem.r8
        /// Pop: Popref_popi_popr8
        /// Push: Push0
        /// </summary>
        Stelem_R8 = 161,
        /// <summary>
        /// stelem.ref
        /// Pop: Popref_popi_popref
        /// Push: Push0
        /// </summary>
        Stelem_Ref = 162,
        /// <summary>
        /// ldelem
        /// Pop: Popref_popi
        /// Push: Push1
        /// </summary>
        Ldelem = 163,
        /// <summary>
        /// stelem
        /// Pop: Popref_popi_pop1
        /// Push: Push0
        /// </summary>
        Stelem = 164,
        /// <summary>
        /// unbox.any
        /// Pop: Popref
        /// Push: Push1
        /// </summary>
        Unbox_Any = 165,
        /// <summary>
        /// conv.ovf.i1
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_I1 = 179,
        /// <summary>
        /// conv.ovf.u1
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_U1 = 180,
        /// <summary>
        /// conv.ovf.i2
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_I2 = 181,
        /// <summary>
        /// conv.ovf.u2
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_U2 = 182,
        /// <summary>
        /// conv.ovf.i4
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_I4 = 183,
        /// <summary>
        /// conv.ovf.u4
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_U4 = 184,
        /// <summary>
        /// conv.ovf.i8
        /// Pop: Pop1
        /// Push: Pushi8
        /// </summary>
        Conv_Ovf_I8 = 185,
        /// <summary>
        /// conv.ovf.u8
        /// Pop: Pop1
        /// Push: Pushi8
        /// </summary>
        Conv_Ovf_U8 = 186,
        /// <summary>
        /// refanyval
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Refanyval = 194,
        /// <summary>
        /// ckfinite
        /// Pop: Pop1
        /// Push: Pushr8
        /// </summary>
        Ckfinite = 195,
        /// <summary>
        /// mkrefany
        /// Pop: Popi
        /// Push: Push1
        /// </summary>
        Mkrefany = 198,
        /// <summary>
        /// ldtoken
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldtoken = 208,
        /// <summary>
        /// conv.u2
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_U2 = 209,
        /// <summary>
        /// conv.u1
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_U1 = 210,
        /// <summary>
        /// conv.i
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_I = 211,
        /// <summary>
        /// conv.ovf.i
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_I = 212,
        /// <summary>
        /// conv.ovf.u
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_Ovf_U = 213,
        /// <summary>
        /// add.ovf
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Add_Ovf = 214,
        /// <summary>
        /// add.ovf.un
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Add_Ovf_Un = 215,
        /// <summary>
        /// mul.ovf
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Mul_Ovf = 216,
        /// <summary>
        /// mul.ovf.un
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Mul_Ovf_Un = 217,
        /// <summary>
        /// sub.ovf
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Sub_Ovf = 218,
        /// <summary>
        /// sub.ovf.un
        /// Pop: Pop1_pop1
        /// Push: Push1
        /// </summary>
        Sub_Ovf_Un = 219,
        /// <summary>
        /// endfinally
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Endfinally = 220,
        /// <summary>
        /// leave
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Leave = 221,
        /// <summary>
        /// leave.s
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Leave_S = 222,
        /// <summary>
        /// stind.i
        /// Pop: Popi_popi
        /// Push: Push0
        /// </summary>
        Stind_I = 223,
        /// <summary>
        /// conv.u
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Conv_U = 224,
        /// <summary>
        /// prefix7
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Prefix7 = 248,
        /// <summary>
        /// prefix6
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Prefix6 = 249,
        /// <summary>
        /// prefix5
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Prefix5 = 250,
        /// <summary>
        /// prefix4
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Prefix4 = 251,
        /// <summary>
        /// prefix3
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Prefix3 = 252,
        /// <summary>
        /// prefix2
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Prefix2 = 253,
        /// <summary>
        /// prefix1
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Prefix1 = 254,
        /// <summary>
        /// prefixref
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Prefixref = 255,
        /// <summary>
        /// arglist
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Arglist = -512,
        /// <summary>
        /// ceq
        /// Pop: Pop1_pop1
        /// Push: Pushi
        /// </summary>
        Ceq = -511,
        /// <summary>
        /// cgt
        /// Pop: Pop1_pop1
        /// Push: Pushi
        /// </summary>
        Cgt = -510,
        /// <summary>
        /// cgt.un
        /// Pop: Pop1_pop1
        /// Push: Pushi
        /// </summary>
        Cgt_Un = -509,
        /// <summary>
        /// clt
        /// Pop: Pop1_pop1
        /// Push: Pushi
        /// </summary>
        Clt = -508,
        /// <summary>
        /// clt.un
        /// Pop: Pop1_pop1
        /// Push: Pushi
        /// </summary>
        Clt_Un = -507,
        /// <summary>
        /// ldftn
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldftn = -506,
        /// <summary>
        /// ldvirtftn
        /// Pop: Popref
        /// Push: Pushi
        /// </summary>
        Ldvirtftn = -505,
        /// <summary>
        /// ldarg
        /// Pop: Pop0
        /// Push: Push1
        /// </summary>
        Ldarg = -503,
        /// <summary>
        /// ldarga
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldarga = -502,
        /// <summary>
        /// starg
        /// Pop: Pop1
        /// Push: Push0
        /// </summary>
        Starg = -501,
        /// <summary>
        /// ldloc
        /// Pop: Pop0
        /// Push: Push1
        /// </summary>
        Ldloc = -500,
        /// <summary>
        /// ldloca
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Ldloca = -499,
        /// <summary>
        /// stloc
        /// Pop: Pop1
        /// Push: Push0
        /// </summary>
        Stloc = -498,
        /// <summary>
        /// localloc
        /// Pop: Popi
        /// Push: Pushi
        /// </summary>
        Localloc = -497,
        /// <summary>
        /// endfilter
        /// Pop: Popi
        /// Push: Push0
        /// </summary>
        Endfilter = -495,
        /// <summary>
        /// unaligned.
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Unaligned = -494,
        /// <summary>
        /// volatile.
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Volatile = -493,
        /// <summary>
        /// tail.
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Tailcall = -492,
        /// <summary>
        /// initobj
        /// Pop: Popi
        /// Push: Push0
        /// </summary>
        Initobj = -491,
        /// <summary>
        /// constrained.
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Constrained = -490,
        /// <summary>
        /// cpblk
        /// Pop: Popi_popi_popi
        /// Push: Push0
        /// </summary>
        Cpblk = -489,
        /// <summary>
        /// initblk
        /// Pop: Popi_popi_popi
        /// Push: Push0
        /// </summary>
        Initblk = -488,
        /// <summary>
        /// rethrow
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Rethrow = -486,
        /// <summary>
        /// sizeof
        /// Pop: Pop0
        /// Push: Pushi
        /// </summary>
        Sizeof = -484,
        /// <summary>
        /// refanytype
        /// Pop: Pop1
        /// Push: Pushi
        /// </summary>
        Refanytype = -483,
        /// <summary>
        /// readonly.
        /// Pop: Pop0
        /// Push: Push0
        /// </summary>
        Readonly = -482,
    }

    #endregion
}
