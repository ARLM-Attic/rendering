using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Unsafe;

namespace System.Maths
{
    #region Vector1

    public struct Vector1
    {
        public float X;

        public Vector1(float x)
        {
            this.X = x;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return X;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: X = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public static Vector1 operator +(Vector1 v1, Vector1 v2)
        {
            return new Vector1(v1.X+(v2.X));
        }

        public static Vector1 operator *(Vector1 v1, Vector1 v2)
        {
            return new Vector1(v1.X * (v2.X));
        }
        public static Vector1 operator /(Vector1 v1, Vector1 v2)
        {
            return new Vector1(v1.X/(v2.X));
        }

        public static Vector1 operator *(Vector1 v, float factor)
        {
            return new Vector1(v.X * factor);
        }
        public static Vector1 operator /(Vector1 v, float factor)
        {
            return new Vector1(v.X / factor);
        }

        public static Vector1 operator *(float factor, Vector1 v)
        {
            return v * factor;
        }

        public static Vector1 operator &(Vector1 v1, Vector1 v2)
        {
            return new Vector1((int)v1.X & (int)v2.X);
        }

        public static Vector1 operator |(Vector1 v1, Vector1 v2)
        {
            return new Vector1((int)v1.X | (int)v2.X);
        }

        public static Vector1 operator ^(Vector1 v1, Vector1 v2)
        {
            return new Vector1((int)v1.X ^ (int)v2.X);
        }

        public static Vector1 operator ~(Vector1 v)
        {
            return new Vector1(~(int)v.X);
        }

        public static Vector1 operator %(Vector1 v1, Vector1 v2)
        {
            return new Vector1(v1.X % v2.X);
        }




        public static explicit operator float(Vector1 v)
        {
            return v.X;
        }

        public static implicit operator Vector1(float x)
        {
            return new Vector1(x);
        }

        public static Vector1 operator -(Vector1 v1, Vector1 v2)
        {
            return new Vector1(v1.X-(v2.X));
        }

        public override string ToString()
        {
            return this.X.ToString();
        }
    }

    #endregion

    #region Vector2

    public struct Vector2
    {
        public float X;

        public float Y;

        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2(float x)
            : this(x, x)
        {
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X+(v2.X), v1.Y+(v2.Y));
        }

        public static Vector2 operator *(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X * (v2.X), v1.Y * (v2.Y));
        }
        public static Vector2 operator /(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X/(v2.X), v1.Y/(v2.Y));
        }

        public static Vector2 operator *(Vector2 v, float factor)
        {
            return new Vector2(v.X * factor, v.Y * factor);
        }
        public static Vector2 operator /(Vector2 v, float factor)
        {
            return new Vector2(v.X / factor, v.Y / factor);
        }

        public static Vector2 operator *(float factor, Vector2 v)
        {
            return v * factor;
        }

        public static Vector2 operator &(Vector2 v1, Vector2 v2)
        {
            return new Vector2((int)v1.X & (int)v2.X, (int)v1.Y & (int)v2.Y);
        }

        public static Vector2 operator |(Vector2 v1, Vector2 v2)
        {
            return new Vector2((int)v1.X | (int)v2.X, (int)v1.Y | (int)v2.Y);
        }

        public static Vector2 operator ^(Vector2 v1, Vector2 v2)
        {
            return new Vector2((int)v1.X ^ (int)v2.X, (int)v1.Y ^ (int)v2.Y);
        }

        public static Vector2 operator ~(Vector2 v)
        {
            return new Vector2(~(int)v.X, ~(int)v.Y);
        }

        public static Vector2 operator %(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X % v2.X, v1.Y % v2.Y);
        }

        public static explicit operator Vector1(Vector2 v)
        {
            return new Vector1(v.X);
        }

        public static explicit operator float(Vector2 v)
        {
            return v.X;
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X-(v2.X), v1.Y-(v2.Y));
        }

        public override string ToString()
        {
            return this.X.ToString() + "; " + this.Y.ToString();
        }
    }

    #endregion

    #region Vector3

    public struct Vector3
    {
        public float X;

        public float Y;

        public float Z;

        public Vector3(float x, float y, float z)
        {

            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3(float x)
            : this(x, x, x)
        {
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X+(v2.X), v1.Y+(v2.Y), v1.Z+(v2.Z));
        }

        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X * (v2.X), v1.Y * (v2.Y), v1.Z * (v2.Z));
        }
        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X/(v2.X), v1.Y/(v2.Y), v1.Z/(v2.Z));
        }

        public static Vector3 operator *(Vector3 v, float factor)
        {
            return new Vector3(v.X * factor, v.Y * factor, v.Z * factor);
        }
        public static Vector3 operator /(Vector3 v, float factor)
        {
            return new Vector3(v.X / factor, v.Y / factor, v.Z / factor);
        }

        public static Vector3 operator *(float factor, Vector3 v)
        {
            return v * factor;
        }

        public static Vector3 operator &(Vector3 v1, Vector3 v2)
        {
            return new Vector3((int)v1.X & (int)v2.X, (int)v1.Y & (int)v2.Y, (int)v1.Z & (int)v2.Z);
        }

        public static Vector3 operator |(Vector3 v1, Vector3 v2)
        {
            return new Vector3((int)v1.X | (int)v2.X, (int)v1.Y | (int)v2.Y, (int)v1.Z | (int)v2.Z);
        }

        public static Vector3 operator ^(Vector3 v1, Vector3 v2)
        {
            return new Vector3((int)v1.X ^ (int)v2.X, (int)v1.Y ^ (int)v2.Y, (int)v1.Z ^ (int)v2.Z);
        }

        public static Vector3 operator ~(Vector3 v)
        {
            return new Vector3(~(int)v.X, ~(int)v.Y, ~(int)v.Z);
        }

        public static Vector3 operator %(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X % v2.X, v1.Y % v2.Y, v1.Z % v2.Z);
        }

        public static explicit operator Vector2(Vector3 v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static explicit operator float(Vector3 v)
        {
            return v.X;
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X-(v2.X), v1.Y-(v2.Y), v1.Z-(v2.Z));
        }

        public override string ToString()
        {
            return this.X.ToString() + "; " + this.Y.ToString() + "; " + this.Z.ToString();
        }
    }

    #endregion

    #region Vector4

    public struct Vector4
    {
        public float X;

        public float Y;

        public float Z;

        public float W;

        public Vector4(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public Vector4(Vector3 v, float w)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
            this.W = w;
        }

        public Vector4(float x)
            : this(x, x, x, x)
        {
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    case 3: return W;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    case 3: W = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public static Vector4 operator +(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.X+(v2.X), v1.Y+(v2.Y), v1.Z+(v2.Z), v1.W+(v2.Z));
        }

        public static Vector4 operator *(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.X * (v2.X), v1.Y * (v2.Y), v1.Z * (v2.Z), v1.W * (v2.W));
        }

        public static Vector4 operator /(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.X/(v2.X), v1.Y/(v2.Y), v1.Z/(v2.Z), v1.W/(v2.W));
        }

        public static Vector4 operator *(Vector4 v, float factor)
        {
            return new Vector4(v.X * factor, v.Y * factor, v.Z * factor, v.W * factor);
        }
        public static Vector4 operator /(Vector4 v, float factor)
        {
            return new Vector4(v.X / factor, v.Y / factor, v.Z / factor, v.W / factor);
        }

        public static Vector4 operator *(float factor, Vector4 v)
        {
            return v * factor;
        }

        public static Vector4 operator &(Vector4 v1, Vector4 v2)
        {
            return new Vector4((int)v1.X & (int)v2.X, (int)v1.Y & (int)v2.Y, (int)v1.Z & (int)v2.Z, (int)v1.W & (int)v2.W);
        }

        public static Vector4 operator |(Vector4 v1, Vector4 v2)
        {
            return new Vector4((int)v1.X | (int)v2.X, (int)v1.Y | (int)v2.Y, (int)v1.Z | (int)v2.Z, (int)v1.W | (int)v2.W);
        }

        public static Vector4 operator ^(Vector4 v1, Vector4 v2)
        {
            return new Vector4((int)v1.X ^ (int)v2.X, (int)v1.Y ^ (int)v2.Y, (int)v1.Z ^ (int)v2.Z, (int)v1.W ^ (int)v2.W);
        }

        public static Vector4 operator ~(Vector4 v)
        {
            return new Vector4(~(int)v.X, ~(int)v.Y, ~(int)v.Z, ~(int)v.W);
        }

        public static Vector4 operator %(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.X % v2.X, v1.Y % v2.Y, v1.Z % v2.Z, v1.W % v2.W);
        }

        public static explicit operator Vector3(Vector4 v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static explicit operator Vector2(Vector4 v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static explicit operator Vector1(Vector4 v)
        {
            return new Vector1(v.X);
        }

        public static Vector4 operator -(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.X-(v2.X), v1.Y-(v2.Y), v1.Z-(v2.Z), v1.W-(v2.W));
        }

        public override string ToString()
        {
            return this.X.ToString() + "; " + this.Y.ToString() + "; " + this.Z.ToString() + "; " + this.W.ToString();
        }

        public Vector3 Homogeneous
        {
            get { return (Vector3)(this / this.W); }
        }
    }

    #endregion
}
