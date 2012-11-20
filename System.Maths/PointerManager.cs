using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Unsafe
{
    public class PointerManager
    {
        #region Instances
        private static Dictionary<Type, object> managersCache = new Dictionary<Type,object> ();
        private static Dictionary<Type, int> sizesOf = new Dictionary<Type, int>();

        private static IPointerManager<int> IntPtrManager = GetManager<int>();
        private static IPointerManager<uint> UIntPtrManager = GetManager<uint>();
        private static IPointerManager<short> Int16PtrManager = GetManager<short>();
        private static IPointerManager<ushort> UInt16PtrManager = GetManager<ushort>();
        private static IPointerManager<float> FloatPtrManager = GetManager<float>();
        private static IPointerManager<byte> BytePtrManager = GetManager<byte>();
        private static IPointerManager<char> CharPtrManager = GetManager<char>();
        private static IPointerManager<bool> BoolPtrManager = GetManager<bool>();
        private static IPointerManager<IntPtr> IntPtrPtrManager = GetManager<IntPtr>();

        private static object lastManagerUsed = null;
        private static Type lastTypeRequest = null;

        #endregion

        #region Private

        private static IPointerManager<S> CreateManager<S>() where S : struct
        {
            #region Dynamic Module Builder
            AppDomain domain = AppDomain.CurrentDomain;
            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = "PointingTemp";
            AssemblyBuilder assembly = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assembly.DefineDynamicModule("PointingTemp", assemblyName + ".dll");
            #endregion

            #region Type Builder
            TypeBuilder typeBuilder = moduleBuilder.DefineType("PointerManagerOf" + typeof(S).Name, TypeAttributes.Public | TypeAttributes.SequentialLayout | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit | TypeAttributes.Serializable, typeof(object), new Type[] { typeof(IPointerManager<S>) });
            #endregion

            #region GetPtr method

            Type byRefS = typeof(S).MakeByRefType();

            MethodInfo methodCast = typeof(IntPtr).GetMethod("op_Explicit", new Type[] { typeof(int) }); 

            MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetPtr",
                MethodAttributes.Public | MethodAttributes.ReuseSlot |
                MethodAttributes.Virtual | MethodAttributes.HideBySig,
                typeof(IntPtr),
                //null, null, 
                new Type[] { byRefS });//, new Type[][] { new Type[] { typeof(System.Runtime.CompilerServices.StrongBox<int>) } }, null);

            ILGenerator methodIL = methodBuilder.GetILGenerator();
            methodIL.Emit(OpCodes.Ldarg_1);
            methodIL.EmitCall(OpCodes.Call, methodCast, null);
            methodIL.Emit(OpCodes.Ret);

            MethodInfo methodInfo = typeof(IPointerManager<S>).GetMethod("GetPtr");

            typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);

            #endregion

            #region GetArray method


            #endregion

            Type type = typeBuilder.CreateType();

            IPointerManager<S> pm = (IPointerManager<S>)Activator.CreateInstance(type);

            return pm;
        }

        #endregion

        public static IPointerManager<T> GetManager<T>() where T:struct
        {
            Type type = typeof(T);

            if (lastTypeRequest == type)
                return (IPointerManager<T>)lastManagerUsed;

            lastTypeRequest = type;

            if (!managersCache.ContainsKey(type))
            {
                managersCache.Add(type, CreateManager<T>());
                sizesOf.Add(type, Marshal.SizeOf(type));
            }
            return (IPointerManager<T>)(lastManagerUsed = managersCache[type]);
        }

        /// <summary>
        /// Copies a secuence of values from one array to another.
        /// </summary>
        /// <param name="offsetSrc">Start item to copy.</param>
        /// <param name="offsetDst">Start item to be replaced.</param>
        /// <param name="count">Number of elements of the source array copied to destination array.</param>
        public static void Copy(Array src, int offsetSrc, Array dst, int offsetDst, int count)
        {
            Copy(Marshal.UnsafeAddrOfPinnedArrayElement(src, offsetSrc), Marshal.UnsafeAddrOfPinnedArrayElement(dst, offsetDst), count * Marshal.SizeOf(src.GetType().GetElementType()));
        }

        public static void Copy(Array src, Array dst)
        {
            Copy(src, 0, dst, 0, src.Length);
        }

        public static void CopyBy4(IntPtr source, IntPtr destination, int numberOfCopies, int sizeOfSourceSlots, int sizeOfDestinationSlots)
        {
            unsafe
            {
                int* s = (int*)source;
                int* d = (int*)destination;
                int ds = sizeOfSourceSlots;
                int dd = sizeOfDestinationSlots;
                while (numberOfCopies > 0)
                {
                    *d = *s;
                    s = (int*)((byte*)s + ds);
                    d = (int*)((byte*)d + dd);
                    numberOfCopies--;
                }
            }
        }

        public static void Clear(IntPtr dst, int numberOfClears, int sizeOfDstSlots, int countOfBytes)
        {
            unsafe
            {
                byte* d = (byte*)dst;
                while (numberOfClears > 0)
                {
                    int c = countOfBytes;
                    while (c > 0)
                    {
                        *d = 0;
                        d++;
                        c--;
                    }
                    d += sizeOfDstSlots - countOfBytes;
                    numberOfClears--;
                }
            }
        }

        public static void Copy(IntPtr source, IntPtr destination, int numberOfCopies, int sizeOfSourceSlots, int sizeOfDestinationSlots, int countOfBytes)
        {
            unsafe
            {
                byte* s = (byte*)source;
                byte* d = (byte*)destination;
                int stepS = sizeOfSourceSlots - countOfBytes;
                int stepD = sizeOfDestinationSlots - countOfBytes;

                while (numberOfCopies > 0)
                {
                    int c = countOfBytes;
                    while (c > 0)
                    {
                        *d = *s;
                        s++;
                        d++;
                        c--;
                    }
                    s += stepS;
                    d += stepD;
                    numberOfCopies--;
                }
            }
        }
        
        public static void Copy(IntPtr source, IntPtr destination, int countOfBytes)
        {
            unsafe
            {
                byte* s = (byte*)source;
                byte* d = (byte*)destination;
                while (countOfBytes > 0)
                {
                    *d = *s;
                    s++;
                    d++;
                    countOfBytes--;
                }
            }
        }

        public unsafe static int ReadInt(IntPtr ptr)
        {
            return *((int*)ptr);
        }

        public unsafe static short ReadInt16(IntPtr ptr)
        {
            return *((short*)ptr);
        }

        public unsafe static ushort ReadUInt16(IntPtr ptr)
        {
            return *((ushort*)ptr);
        }

        public unsafe static long ReadInt64(IntPtr ptr)
        {
            return *((long*)ptr);
        }

        public unsafe static float ReadFloat(IntPtr ptr)
        {
            return *((float*)ptr);
        }
        public unsafe static double ReadDouble(IntPtr ptr)
        {
            return *((double*)ptr);
        }
        
        public unsafe static bool ReadBool(IntPtr ptr)
        {
            return *((bool*)ptr);
        }
        
        public unsafe static char ReadChar(IntPtr ptr)
        {
            return *((char*)ptr);
        }
        
        public unsafe static IntPtr ReadIntPtr(IntPtr ptr)
        {
            return *((IntPtr*)ptr);
        }
        
        public unsafe static byte ReadByte(IntPtr ptr)
        {
            return *((byte*)ptr);
        }

        public unsafe static uint ReadUInt32(IntPtr ptr)
        {
            return *((uint*)ptr);
        }

        public static D Read<D>(IntPtr ptr) where D : struct
        {
            D data = new D();
            IntPtr dataPtr = GetPtr<D>(ref data);
            Copy(ptr, dataPtr, Marshal.SizeOf(typeof (D)));
            return data;
        }

        public unsafe static void Write(IntPtr ptr, int data)
        {
            *((int*)ptr) = data;
        }

        public unsafe static void Write(IntPtr ptr, uint data)
        {
            *((uint*)ptr) = data;
        }

        public unsafe static void Write(IntPtr ptr, float data)
        {
            *((float*)ptr) = data;
        }
        
        public unsafe static void Write(IntPtr ptr, bool data)
        {
            *((bool*)ptr) = data;
        }
        
        public unsafe static void Write(IntPtr ptr, char data)
        {
            *((char*)ptr) = data;
        }
        
        public unsafe static void Write(IntPtr ptr, IntPtr data)
        {
            *((IntPtr*)ptr) = data;
        }

        public unsafe static void Write(IntPtr ptr, byte data)
        {
            *((byte*)ptr) = data;
        }

        public unsafe static void Write(IntPtr ptr, double data)
        {
            *((double*)ptr) = data;
        }
        public unsafe static void Write(IntPtr ptr, long data)
        {
            *((long*)ptr) = data;
        }
        public unsafe static void Write(IntPtr ptr, short data)
        {
            *((short*)ptr) = data;
        }
        public unsafe static void Write(IntPtr ptr, ushort data)
        {
            *((ushort*)ptr) = data;
        }
        public unsafe static void Write<D>(IntPtr _ptr, D data) where D:struct
        {
            float* ptr = (float*)_ptr;
            float* dataPtr = (float*)GetPtr<D>(ref data);
            int sizeOf = sizesOf[typeof(D)];
            switch (sizeOf)
            {
                case 4://float
                    *ptr = *dataPtr;
                    break;
                case 8://2 float
                    *ptr = *dataPtr;
                    ptr += 4;
                    dataPtr += 4;
                    *ptr = *dataPtr;
                    break;
                case 12://3 float
                    *ptr = *dataPtr;
                    ptr += 4;
                    dataPtr += 4;
                    *ptr = *dataPtr;
                    ptr += 4;
                    dataPtr += 4;
                    *ptr = *dataPtr;
                    break;
                case 16://4 float
                    *ptr = *dataPtr;
                    ptr += 4;
                    dataPtr += 4;
                    *ptr = *dataPtr;
                    ptr += 4;
                    dataPtr += 4;
                    *ptr = *dataPtr;
                    ptr += 4;
                    dataPtr += 4;
                    *ptr = *dataPtr;
                    break;
                default:
                    Copy((IntPtr)dataPtr, _ptr, sizesOf[typeof(D)]);
                    break;
            }
        }

        public static IntPtr GetPtr(ref int var)
        {
            return IntPtrManager.GetPtr(ref var);
        }
        
        public static IntPtr GetPtr(ref float var)
        {
            return FloatPtrManager.GetPtr(ref var);
        }
        
        public static IntPtr GetPtr(ref bool var)
        {
            return BoolPtrManager.GetPtr(ref var);
        }
        
        public static IntPtr GetPtr(ref char var)
        {
            return CharPtrManager.GetPtr(ref var);
        }
        
        public static IntPtr GetPtr(ref IntPtr var)
        {
            return IntPtrPtrManager.GetPtr(ref var);
        }
        
        public static IntPtr GetPtr(ref byte var)
        {
            return BytePtrManager.GetPtr(ref var);
        }

        public static int GetSize<T>(ref T var) where T : struct
        {
            if (var is IPointable)
                return ((IPointable)var).Size;
            return Marshal.SizeOf(typeof(T));
        }

        public static IntPtr GetPtr<T>(ref T var) where T:struct
        {
            if (var is IPointable)
                return ((IPointable)var).Ptr;

            IPointerManager<T> manager = GetManager<T>();
            IntPtr result = manager.GetPtr(ref var);
            return result;
        }

    }

    public interface IPointerManager<T>
    {
        IntPtr GetPtr(ref T var);
    }

    public interface IPointable
    {
        /// <summary>
        /// When implemented gets the pointer of this struct.
        /// </summary>
        IntPtr Ptr { get; }
        /// <summary>
        /// When implemented gets the size of this struct in bytes.
        /// </summary>
        int Size { get; }
    }
}
