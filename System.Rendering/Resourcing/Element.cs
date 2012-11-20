using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Unsafe;
using System.Rendering.Resourcing;

namespace System.Rendering
{
    public static class DataComponentExtensors
    {
        static readonly Dictionary<int, DataDescription> descriptors = new Dictionary<int, DataDescription>();
        public static DataDescription GetDescriptor<T>() where T : struct
        {
            return GetDescriptor(typeof(T));
        }

        public static bool IsValid(Type elementType)
        {
            if (descriptors.ContainsKey(elementType.MetadataToken))
                return true;

            if (elementType.IsPrimitive)
                return true;

            if (!elementType.IsValueType)
                return false;

            foreach (var f in elementType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic))
                if (!IsValid(f.FieldType))
                    return false;

            return true;
        }

        public static DataDescription GetDescriptor(Type type)
        {
            if (!IsValid(type))
                throw new ArgumentException("Type " + type.Name + " can not be treated as a buffer element.");

            return GetDescriptorAndAddIfNoPresent(type);
        }

        internal static DataDescription GetDescriptorAndAddIfNoPresent(Type type)
        {
            if (!descriptors.ContainsKey(type.MetadataToken))
                descriptors.Add(type.MetadataToken, new DataDescription(type));

            return descriptors[type.MetadataToken];
        }

        public static unsafe void Copy<T, R>(Array src, Array dst, Func<T, R> transform)
        {
            Type srcElementType = src.GetType().GetElementType();
            Type dstElementType = dst.GetType().GetElementType();

            if (srcElementType != typeof(T) || dstElementType != typeof(R))
                throw new ArgumentException("transform");

            if (!srcElementType.IsValueType || !dstElementType.IsValueType)
                throw new ArgumentException("Types must be value types");

            PointerManager.Copy(src.Cast<T>().Select(e => transform(e)).ToArray(), dst);
        }

        static Func<IntPtr, T> GetReader<T>() where T : struct
        {
            if (typeof(T) == typeof(byte))
                return (Func<IntPtr, T>)(object)(Func<IntPtr, byte>)PointerManager.ReadByte;
            if (typeof(T) == typeof(bool))
                return (Func<IntPtr, T>)(object)(Func<IntPtr, bool>)PointerManager.ReadBool;
            if (typeof(T) == typeof(short))
                return (Func<IntPtr, T>)(object)(Func<IntPtr, short>)PointerManager.ReadInt16;
            if (typeof(T) == typeof(ushort))
                return (Func<IntPtr, T>)(object)(Func<IntPtr, ushort>)PointerManager.ReadUInt16;
            if (typeof(T) == typeof(int))
                return (Func<IntPtr, T>)(object)(Func<IntPtr, int>)PointerManager.ReadInt;
            if (typeof(T) == typeof(uint))
                return (Func<IntPtr, T>)(object)(Func<IntPtr, uint>)PointerManager.ReadUInt32;
            if (typeof(T) == typeof(long))
                return (Func<IntPtr, T>)(object)(Func<IntPtr, long>)PointerManager.ReadInt64;
            if (typeof(T) == typeof(float))
                return (Func<IntPtr, T>)(object)(Func<IntPtr, float>)PointerManager.ReadFloat;
            if (typeof(T) == typeof(double))
                return (Func<IntPtr, T>)(object)(Func<IntPtr, double>)PointerManager.ReadDouble;
            if (typeof(T) == typeof(IntPtr))
                return (Func<IntPtr, T>)(object)(Func<IntPtr, IntPtr>)PointerManager.ReadIntPtr;
            throw new NotSupportedException();
        }

        static Action<IntPtr, T> GetWriter<T>() where T : struct
        {
            if (typeof(T) == typeof(byte))
                return (Action<IntPtr, T>)(object)(Action<IntPtr, byte>)PointerManager.Write;
            if (typeof(T) == typeof(bool))
                return (Action<IntPtr, T>)(object)(Action<IntPtr, bool>)PointerManager.Write;
            if (typeof(T) == typeof(short))
                return (Action<IntPtr, T>)(object)(Action<IntPtr, short>)PointerManager.Write;
            if (typeof(T) == typeof(ushort))
                return (Action<IntPtr, T>)(object)(Action<IntPtr, ushort>)PointerManager.Write;
            if (typeof(T) == typeof(int))
                return (Action<IntPtr, T>)(object)(Action<IntPtr, int>)PointerManager.Write;
            if (typeof(T) == typeof(uint))
                return (Action<IntPtr, T>)(object)(Action<IntPtr, uint>)PointerManager.Write;
            if (typeof(T) == typeof(long))
                return (Action<IntPtr, T>)(object)(Action<IntPtr, long>)PointerManager.Write;
            if (typeof(T) == typeof(float))
                return (Action<IntPtr, T>)(object)(Action<IntPtr, float>)PointerManager.Write;
            if (typeof(T) == typeof(double))
                return (Action<IntPtr, T>)(object)(Action<IntPtr, double>)PointerManager.Write;
            if (typeof(T) == typeof(IntPtr))
                return (Action<IntPtr, T>)(object)(Action<IntPtr, IntPtr>)PointerManager.Write;
            throw new NotSupportedException();
        }

        static Converter<T, R> GetConverter<T, R>()
            where T : struct
            where R : struct
        {
            return t => (R)Convert.ChangeType(t, typeof(R));
        }

        private static unsafe void CopyConverting<T, R>(IntPtr src, IntPtr dst, int count)
            where T : struct
            where R : struct
        {
            int srcElementSize = Marshal.SizeOf(typeof(T));
            int dstElementSize = Marshal.SizeOf(typeof(R));

            var readerT = GetReader<T>();
            var writerR = GetWriter<R>();
            var converter = GetConverter<T, R>();

            for (int i = 0; i < count; i++)
            {
                writerR(dst, converter(readerT(src)));
                src = (IntPtr)((int)src + srcElementSize);
                dst = (IntPtr)((int)dst + dstElementSize);
            }
        }

        public static unsafe void Copy(IntPtr src, Type srcElementType, IntPtr dst, Type dstElementType, int count)
        {
            if (!srcElementType.IsValueType || !dstElementType.IsValueType)
                throw new ArgumentException("Types must be value types");

            if (srcElementType == dstElementType)
            {
                PointerManager.Copy(src, dst, count * Marshal.SizeOf(srcElementType));
                return;
            }

            if (srcElementType.IsPrimitive && dstElementType.IsPrimitive)
            {
                var method = typeof(DataComponentExtensors).GetMethod("CopyConverting", BindingFlags.NonPublic | BindingFlags.Static);
                method.MakeGenericMethod(srcElementType, dstElementType).Invoke(
                    null, new object[] { src, dst, count }
                    );
                return;
            }

            if (dstElementType.IsPrimitive || srcElementType.IsPrimitive)
            {
                if (Marshal.SizeOf(dstElementType) != Marshal.SizeOf(srcElementType))
                    throw new ArgumentException("Can not copy from/to a basic type to/from a struct type with different sizes");

                PointerManager.Copy(src, dst, count * Marshal.SizeOf(srcElementType));
                return;
            }

            DataDescription srcDescriptor = GetDescriptorAndAddIfNoPresent(srcElementType);
            DataDescription dstDescriptor = GetDescriptorAndAddIfNoPresent(dstElementType);

            int srcElementSize = Marshal.SizeOf(srcElementType);
            int dstElementSize = Marshal.SizeOf(dstElementType);

            foreach (var srcField in srcDescriptor.Declaration)
            {
                DataComponentDescription dstField;
                if (dstDescriptor.Match(srcField.Semantic, out dstField))
                {
                    if (srcDescriptor.IsByteOrder && dstDescriptor.IsByteOrder)
                    {
                        int fieldSize = Math.Min(srcField.Size, dstField.Size);

                        dst = (IntPtr)((int)dst + dstField.Offset);
                        src = (IntPtr)((int)src + srcField.Offset);

                        PointerManager.Copy(src, dst, count, srcElementSize, dstElementSize, fieldSize);
                    }
                    else
                    {
                        #region Retrieve Data
                        float[] values;
                        if (srcDescriptor.IsBitOrder)
                        {
                            values = GetData(src, srcField.CapsuleSize, srcElementSize, srcField.Offset, srcField.SizeInBits, count, srcField.Map);
                        }
                        else
                        {
                            if (srcField.BasicType != TypeCode.Single)
                                throw new NotSupportedException();

                            values = new float[count];
                            IntPtr valuesPtr = Marshal.UnsafeAddrOfPinnedArrayElement(values, 0);
                            PointerManager.Copy((IntPtr)((int)src+srcField.Offset), valuesPtr, count, srcElementSize, 4, 4);
                        }
                        #endregion

                        #region Set Data
                        if (dstDescriptor.IsBitOrder)
                        {
                            SetData(dst, values, dstField.CapsuleSize, dstElementSize, dstField.Offset, dstField.SizeInBits, dstField.Map);
                        }
                        else
                        {
                            if (dstField.BasicType != TypeCode.Single)
                                throw new NotSupportedException();

                            IntPtr valuesPtr = Marshal.UnsafeAddrOfPinnedArrayElement(values, 0);
                            PointerManager.Copy(valuesPtr, (IntPtr)((int)dst+dstField.Offset), count, 4, dstElementSize, 4);
                        }
                        #endregion
                    }
                }
            }
        }

        private static unsafe float[] GetData(IntPtr source, int capsuleSize, int elementSize, int startBitOffset, int bits, int count, ComponentMap map)
        {
            float[] values = new float[count];

            int capsuleOffset = startBitOffset / (capsuleSize*8);
            source = (IntPtr)((int)source + capsuleOffset * capsuleSize);
            startBitOffset = startBitOffset % (capsuleSize*8);

            switch (capsuleSize)
            {
                case 1:
                    {
                        byte* d = (byte*)source;
                        for (int i = 0; i < count; i++)
                        {
                            byte dataValue = *d;
                            int intVal = (dataValue >> startBitOffset) & ((1 << bits) - 1);
                            switch (map)
                            {
                                case ComponentMap.None: values[i] = intVal; break;
                                case ComponentMap.Signed: values[i] = (intVal / (float)((1 << bits) - 1)) * 2 - 1; break;
                                case ComponentMap.Unsigned: values[i] = (intVal / (float)((1 << bits) - 1)); break;
                            }
                            d = (byte*)((byte*)d + elementSize);
                        }
                    }
                    break;
                case 2:
                    {
                        short* d = (short*)source;
                        for (int i = 0; i < count; i++)
                        {
                            short dataValue = *d;
                            int intVal = (dataValue >> startBitOffset) & ((1 << bits) - 1);
                            switch (map)
                            {
                                case ComponentMap.None: values[i] = intVal; break;
                                case ComponentMap.Signed: values[i] = (intVal / (float)((1 << bits) - 1)) * 2 - 1; break;
                                case ComponentMap.Unsigned: values[i] = (intVal / (float)((1 << bits) - 1)); break;
                            }
                            d = (short*)((byte*)d + elementSize);
                        }
                    }
                    break;
                case 4:
                    {
                        int* d = (int*)source;
                        for (int i = 0; i < count; i++)
                        {
                            int dataValue = *d;
                            int intVal = (dataValue >> startBitOffset) & ((1 << bits) - 1);
                            switch (map)
                            {
                                case ComponentMap.None: values[i] = intVal; break;
                                case ComponentMap.Signed: values[i] = (intVal / (float)((1 << bits) - 1)) * 2 - 1; break;
                                case ComponentMap.Unsigned: values[i] = (intVal / (float)((1 << bits) - 1)); break;
                            }
                            d = (int*)((byte*)d + elementSize);
                        }
                    }
                    break;
                case 8:
                    {
                        long* d = (long*)source;
                        for (int i = 0; i < count; i++)
                        {
                            long dataValue = *d;
                            long intVal = (dataValue >> startBitOffset) & ((1 << bits) - 1);
                            switch (map)
                            {
                                case ComponentMap.None: values[i] = intVal; break;
                                case ComponentMap.Signed: values[i] = (intVal / (float)((1 << bits) - 1)) * 2 - 1; break;
                                case ComponentMap.Unsigned: values[i] = (intVal / (float)((1 << bits) - 1)); break;
                            }
                            d = (long*)((byte*)d + elementSize);
                        }
                    }
                    break;
                default: throw new NotSupportedException();
            }

            return values;
        }

        private static unsafe void SetData(IntPtr destination, float[] values, int capsuleSize, int elementSize, int startBitOffset, int bits, ComponentMap map)
        {
            int capsuleOffset = startBitOffset / (1 << capsuleSize);
            destination = (IntPtr)((int)destination + capsuleOffset * capsuleSize);
            startBitOffset = startBitOffset % (1 << capsuleSize);

            int count = values.Length;

            switch (capsuleSize)
            {
                case 1:
                    {
                        byte* d = (byte*)destination;
                        for (int i = 0; i < count; i++)
                        {
                            int intVal = 0;
                            switch (map)
                            {
                                case ComponentMap.None: intVal = (int)values[i]; break;
                                case ComponentMap.Signed: intVal = (int)((values[i] + 1) * ((1 << bits) - 1) / 2); break;
                                case ComponentMap.Unsigned: intVal = (int)(values[i] * ((1 << bits) - 1)); break;
                            }

                            byte msk = (byte)~(((1 << bits) - 1) << startBitOffset);
                            *d = (byte)(*d & msk | intVal);

                            d = (byte*)((byte*)d + elementSize);
                        }
                    }
                    break;
                case 2:
                    {
                        ushort* d = (ushort*)destination;
                        for (int i = 0; i < count; i++)
                        {
                            int intVal = 0;
                            switch (map)
                            {
                                case ComponentMap.None: intVal = (int)values[i]; break;
                                case ComponentMap.Signed: intVal = (int)((values[i] + 1) * ((1 << bits) - 1) / 2); break;
                                case ComponentMap.Unsigned: intVal = (int)(values[i] * ((1 << bits) - 1)); break;
                            }

                            ushort msk = (ushort)~(((1 << bits) - 1) << startBitOffset);
                            *d = (ushort)(*d & msk | intVal);

                            d = (ushort*)((byte*)d + elementSize);
                        }
                    }
                    break;
                case 4:
                    {
                        uint* d = (uint*)destination;
                        for (int i = 0; i < count; i++)
                        {
                            uint intVal = 0;
                            switch (map)
                            {
                                case ComponentMap.None: intVal = (uint)values[i]; break;
                                case ComponentMap.Signed: intVal = (uint)((values[i] + 1) * ((1 << bits) - 1) / 2); break;
                                case ComponentMap.Unsigned: intVal = (uint)(values[i] * ((1 << bits) - 1)); break;
                            }

                            uint msk = ~(uint)(((1 << bits) - 1) << startBitOffset);
                            *d = (uint)(*d & msk | intVal);

                            d = (uint*)((byte*)d + elementSize);
                        }
                    }
                    break;
                case 8:
                    {
                        ulong* d = (ulong*)destination;
                        for (int i = 0; i < count; i++)
                        {
                            ulong intVal = 0;
                            switch (map)
                            {
                                case ComponentMap.None: intVal = (ulong)values[i]; break;
                                case ComponentMap.Signed: intVal = (ulong)((values[i] + 1) * ((1 << bits) - 1) / 2); break;
                                case ComponentMap.Unsigned: intVal = (ulong)(values[i] * ((1 << bits) - 1)); break;
                            }

                            ulong msk = ~(ulong)(((1 << bits) - 1) << startBitOffset);
                            *d = (ulong)(*d & msk | intVal);

                            d = (ulong*)((byte*)d + elementSize);
                        }
                    }
                    break;

                default: throw new NotSupportedException();
            }
        }

        public static unsafe void Copy(Array src, Array dst)
        {
            GC.Collect(); // This will prevent of GC to move the pinned arrays.
            Copy(Marshal.UnsafeAddrOfPinnedArrayElement(src, 0), src.GetType().GetElementType(), Marshal.UnsafeAddrOfPinnedArrayElement(dst, 0), dst.GetType().GetElementType(), src.Length);
        }
    }

    public struct DataComponentDescription
    {
        /// <summary>
        /// Gets the basic type this component is formed by.
        /// </summary>
        public TypeCode BasicType { get; internal set; }

        public bool IsOnBits { get { return SizeInBits != 0; } }

        public bool IsOnBytes { get { return Size != 0; } }
        /// <summary>
        /// Gets the map mode this component should use to convert to other values.
        /// </summary>
        public ComponentMap Map { get; internal set; }
        /// <summary>
        /// Gets the offset in bits of this component into the Data.
        /// </summary>
        public int Offset { get; internal set; }
        /// <summary>
        /// Gets the size in bytes of this component into the Data.
        /// </summary>
        public int Size
        {
            get; internal set;
        }
        /// <summary>
        /// Gets the size in bits of this component into the Data.
        /// </summary>
        public int SizeInBits
        {
            get;
            internal set;
        }

        public int CapsuleSize
        {
            get
            {
                int size = 1;
                if (SizeInBits > 8)
                    size = 2;
                if (SizeInBits > 16)
                    size = 4;
                if (SizeInBits > 32)
                    size = 8;
                if (Offset % (1 << size) != 0)
                    size *= 2;
                return size;
            }
        }

        /// <summary>
        /// Gets the number of components this data component should have.
        /// </summary>
        public int Components
        {
            get;
            internal set;
        }

        public DataComponentAttribute Semantic { get; set; }
    }

    public class DataDescription
    {
        readonly Dictionary<DataComponentAttribute, DataComponentDescription> declarationMap = new Dictionary<DataComponentAttribute, DataComponentDescription>();

        static int GetSizeInBitsOf(TypeCode type)
        {
            int typeBits = 8;

            switch (type)
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                    typeBits = 8;
                    break;
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    typeBits = 16;
                    break;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Single:
                    typeBits = 32;
                    break;
                default: throw new NotSupportedException("Component type is not supported " + type);
            }

            return typeBits;
        }

        internal DataDescription(Type dataType)
        {
            var typeSemantics = (DataComponentAttribute[])dataType.GetCustomAttributes(typeof(DataComponentAttribute), true);
            Array.Sort(typeSemantics, (s1, s2) => s1.StartBit - s2.StartBit);

            if (typeSemantics.Any(d => d.MatchField) && typeSemantics.Any(d => !d.MatchField))
                throw new InvalidDescriptionException(dataType, "Semantic should either be specified entirely to a type or to its fields.");

            int typeSize = Marshal.SizeOf(dataType) * 8;

            this.TypeSize = typeSize;

            FieldInfo[] fields = dataType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            Array.Sort(fields, (f1, f2) => (int)Marshal.OffsetOf(dataType, f1.Name) - (int)Marshal.OffsetOf(dataType, f2.Name));

            if (typeSemantics.Length > 0)
            {
                int offset = 0;

                if (typeSemantics[0].MatchField)
                { // Description was made at struct and refers to each field.
                    if (fields.Length != typeSemantics.Length)
                        throw new InvalidDescriptionException(dataType, "Not all fields have a semantic, or there are more semantics than fields.");
                    int currentField = 0;

                    foreach (var semantic in typeSemantics)
                    {
                        int components;
                        TypeCode typeCode;
                        if (GetFieldTypeInfo(fields[currentField].FieldType, out components, out typeCode))
                        {
                            var dataComponentDescription = new DataComponentDescription
                            {
                                BasicType = typeCode,
                                Components = components,
                                Map = semantic.Mapping,
                                Offset = offset,
                                SizeInBits = 0,
                                Size = GetSizeInBitsOf(typeCode) * components / 8,
                                Semantic = semantic
                            };

                            declarationMap[semantic] = dataComponentDescription;

                            declaration.Add(dataComponentDescription);

                            offset += dataComponentDescription.Size;

                            currentField++;
                        }
                        else
                            throw new InvalidDescriptionException(dataType, "Field " + fields[currentField].Name + " can not be treated as primitive value or vector");
                    }
                }
                else
                { // Description was made at struct type and refers to each bit descomposition.
                    int currentSemIndex = 0;
                    foreach (var semantic in typeSemantics)
                    {
                        int components = 1;
                        TypeCode typeCode = TypeCode.Int32;

                        var dataComponentDescription = new DataComponentDescription
                        {
                            BasicType = typeCode,
                            Components = components,
                            Map = semantic.Mapping,
                            Offset = offset,
                            SizeInBits = currentSemIndex == typeSemantics.Length - 1 ? typeSize - offset : typeSemantics[currentSemIndex + 1].StartBit - offset,
                            Size = 0,
                            Semantic = semantic
                        };

                        declarationMap[semantic] = dataComponentDescription;

                        declaration.Add(dataComponentDescription);

                        offset += dataComponentDescription.SizeInBits;

                        currentSemIndex++;
                    }

                    if (offset != Marshal.SizeOf(dataType) * 8)
                        throw new InvalidDescriptionException(dataType, "Description number of bits differs from struct number of bits.");
                }
            }
            else
                if (dataType.IsPrimitive)
                {
                    declaration.Add(new DataComponentDescription
                    {
                        BasicType = Type.GetTypeCode(dataType),
                        Components = 1,
                        Map = ComponentMap.None,
                        Offset = 0,
                        Size = Marshal.SizeOf(dataType)
                    });
                }
                else
                {
                    int offset = 0;
                    foreach (var field in fields)
                    {
                        var sem = (DataComponentAttribute[])field.GetCustomAttributes(typeof(DataComponentAttribute), true);
                        if (sem.Length != 1)
                            throw new InvalidDescriptionException(dataType, "Field " + field.Name + " has not one semantic.");

                        var semantic = sem[0];

                        if (!semantic.MatchField)
                            throw new InvalidDescriptionException(dataType, "Field " + field.Name + " can not be a bit decomposition. Use semantic at struct type instead.");

                        int components;
                        TypeCode typeCode;

                        if (!GetFieldTypeInfo(field.FieldType, out components, out typeCode))
                            throw new InvalidDescriptionException(dataType, "Field " + field.Name + " can not be treated as primitive value or vector");

                        var dataComponentDescription = new DataComponentDescription
                        {
                            BasicType = typeCode,
                            Components = components,
                            Map = semantic.Mapping,
                            Offset = offset,
                            SizeInBits = 0,
                            Size = GetSizeInBitsOf(typeCode) * components / 8,
                            Semantic = semantic
                        };

                        declarationMap[semantic] = dataComponentDescription;

                        declaration.Add(dataComponentDescription);

                        offset += dataComponentDescription.Size;
                    }
                }
        }
        
        List<DataComponentDescription> declaration = new List<DataComponentDescription> ();
        public IEnumerable<DataComponentDescription> Declaration
        {
            get
            {
                return declaration;
            }
        }

        public bool IsBitOrder { get { return declaration[0].IsOnBits; } }

        public bool IsByteOrder { get { return declaration[0].IsOnBytes; } }

        public int TypeSize { get; private set; }

        struct FieldTypeInfo
        {
            public bool IsOk;
            public int components;
            public TypeCode typeCode;
        }

        static Dictionary<Type, FieldTypeInfo> __cachedFieldTypeInfos = new Dictionary<Type, FieldTypeInfo>();

        private static bool GetFieldTypeInfo(Type fieldType, out int components, out TypeCode typeCode)
        {
            if (!__cachedFieldTypeInfos.ContainsKey(fieldType))
            {
                if (fieldType.IsPrimitive)
                {
                    components = 1;
                    typeCode = Type.GetTypeCode(fieldType);
                    __cachedFieldTypeInfos.Add(fieldType, new FieldTypeInfo { components = components, typeCode = typeCode, IsOk = true });
                }
                else
                {   // componentizable types...
                    components = 0;
                    typeCode = TypeCode.Single;
                    FieldInfo[] fields = fieldType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (fields.Length != 0)
                    {
                        if (fields.All(f => f.FieldType.Equals(fields[0].FieldType)))
                        {
                            components = fields.Length;
                            typeCode = Type.GetTypeCode(fields[0].FieldType);
                            __cachedFieldTypeInfos.Add(fieldType, new FieldTypeInfo { components = components, typeCode = typeCode, IsOk = true });
                        }
                    }
                }
            }

            var fti = __cachedFieldTypeInfos[fieldType];
            components = fti.components;
            typeCode = fti.typeCode;
            return fti.IsOk;
        }

        public bool Match(DataComponentAttribute semantic, out DataComponentDescription field)
        {
            if (declarationMap.ContainsKey(semantic))
            {
                field = declarationMap[semantic];
                return true;
            }
            field = new DataComponentDescription();
            return false;
        }

        public bool Contains(DataComponentAttribute semantic)
        {
            return declarationMap.ContainsKey(semantic);
        }

        public override bool Equals(object obj)
        {
            if (obj is DataDescription)
                return declaration.SequenceEqual(((DataDescription)obj).declaration);
            return false;
        }

        public override int GetHashCode()
        {
            return declaration.Count == 0 ? 0 : declaration[0].Semantic.GetHashCode() + declaration.Count == 1 ? 0 : declaration[1].Semantic.GetHashCode();
        }
    }


}
