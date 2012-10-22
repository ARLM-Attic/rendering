#define FLOAT

#if DECIMAL
using FLOATINGTYPE = System.Decimal;
#endif
#if DOUBLE
using FLOATINGTYPE = System.Double;
#endif
#if FLOAT
using FLOATINGTYPE = System.Single;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Maths
{
    /// <summary>
    /// Class with a group of vectors and extensors.
    /// </summary>
    public static class Vectors
    {
        public static Vector3 Black { get { return new Vector3(0, 0, 0); } }
        public static Vector4 Ow { get { return new Vector4(0, 0, 0, 1); } }
        public static Vector4 Transparent { get { return new Vector4(0, 0, 0, 0); } }
        public static Vector3 White { get { return new Vector3(1, 1, 1); } }
        public static Vector3 Red { get { return new Vector3(1, 0, 0); } }
        public static Vector3 Green { get { return new Vector3(0, 1, 0); } }
        public static Vector3 Blue { get { return new Vector3(0, 0, 1); } }
        public static Vector3 Yellow { get { return new Vector3(1, 1, 0); } }
        public static Vector3 Magenta { get { return new Vector3(1, 0, 1); } }
        public static Vector3 Cyan { get { return new Vector3(0, 1, 1); } }
        public static Vector4 One { get { return new Vector4(1, 1, 1, 1); } }
        public static Vector4 Zero { get { return new Vector4(0, 0, 0, 0); } }

        public static Vector3 O { get { return new Vector3(0, 0, 0); } }
        public static Vector3 Front { get { return new Vector3(0, 0, 1); } }
        public static Vector3 Back { get { return new Vector3(0, 0, -1); } }
        public static Vector3 Up { get { return new Vector3(0, 1, 0); } }
        public static Vector3 Down { get { return new Vector3(0, -1, 0); } }
        public static Vector3 Right { get { return new Vector3(1, 0, 0); } }
        public static Vector3 Left { get { return new Vector3(-1, 0, 0); } }
        public static Vector3 XAxis { get { return new Vector3(1, 0, 0); } }
        public static Vector3 YAxis { get { return new Vector3(0, 1, 0); } }
        public static Vector3 ZAxis { get { return new Vector3(0, 0, 1); } }
        public static Vector3 Middle { get { return new Vector3((FLOATINGTYPE)0.5, (FLOATINGTYPE)0.5, (FLOATINGTYPE)0.5); } }
        

        private static FLOATINGTYPE Clamp(FLOATINGTYPE x)
        {
            return Math.Max(0, Math.Min(1, x));
        }

        public static int ToARGB(this Vector4 v)
        {
            uint r = (byte)(255 * Clamp(v.X));
            uint g = (byte)(255 * Clamp(v.Y));
            uint b = (byte)(255 * Clamp(v.Z));
            uint a = (byte)(255 * Clamp(v.W));
            return (int)(a << 24 | r << 16 | g << 8 | b << 0);
        }

        public static Vector4 FromARGB(int _argb)
        {
            uint argb = (uint)_argb;
            byte a = (byte)(argb >> 24);
            byte r = (byte)((argb >> 16) % 256);
            byte g = (byte)((argb >> 8) % 256);
            byte b = (byte)((argb >> 0) % 256);
            return new Vector4(r / 255f, g / 255f, b / 255f, a / 255f);
        }
    }
}
