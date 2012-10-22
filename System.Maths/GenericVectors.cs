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
    using System.Maths.Scalaring;
    using System.Unsafe;
    using System.Collections;

    #region Vector1<T>

    public struct Vector1<T> : IPointable where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        public T X;

        public Vector1(T x)
        {
            this.X = x;
        }

        public T this[int index]
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

        public static implicit operator Vector1(Vector1<T> v)
        {
            return new Vector1(v.X.AsFloating());
        }

        public static implicit operator Vector1<T>(Vector1 v)
        {
            return new Vector1<T>(v.X.As<T>());
        }

        public static explicit operator T(Vector1<T> v)
        {
            return v.X;
        }

        public static implicit operator Vector1<T>(T x)
        {
            return new Vector1<T>(x);
        }

        public static Vector1<T> operator +(Vector1<T> v1, Vector1<T> v2)
        {
            return ((Vector1)v1 + (Vector1)v2);
        }

        public static Vector1<T> operator -(Vector1<T> v1, Vector1<T> v2)
        {
            return ((Vector1)v1 - (Vector1)v2);
        }

        public static Vector1<T> operator *(Vector1<T> v1, Vector1<T> v2)
        {
            return ((Vector1)v1 * (Vector1)v2);
        }

        public static Vector1<T> operator /(Vector1<T> v1, Vector1<T> v2)
        {
            return ((Vector1)v1 / (Vector1)v2);
        }

        public static Vector1<T> operator &(Vector1<T> v1, Vector1<T> v2)
        {
            return ((Vector1)v1 & (Vector1)v2);
        }

        public static Vector1<T> operator %(Vector1<T> v1, Vector1<T> v2)
        {
            return ((Vector1)v1 % (Vector1)v2);
        }




        public override string ToString()
        {
            return this.X.ToString();
        }

        IntPtr IPointable.Ptr
        {
            get { return PointerManager.GetPtr(ref X); }
        }

        int IPointable.Size
        {
            get { return PointerManager.GetSize(ref X); }
        }
    }

    #endregion
    
    #region Vector2<T>

    public struct Vector2<T> : IPointable where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        public T X;

        public T Y;

        public Vector2(T x, T y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2(T x)
            : this(x, x)
        {
        }

        public T this[int index]
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

        public static implicit operator Vector2 (Vector2<T> v)
        {
            return new Vector2(v.X.AsFloating(), v.Y.AsFloating());
        }

        public static implicit operator Vector2<T>(Vector2 v)
        {
            return new Vector2<T>(v.X.As<T>(), v.Y.As<T>());
        }

        public static explicit operator T(Vector2<T> v)
        {
            return v.X;
        }

        public static Vector2<T> operator +(Vector2<T> v1, Vector2<T> v2)
        {
            return ((Vector2)v1 + (Vector2)v2);
        }

        public static Vector2<T> operator -(Vector2<T> v1, Vector2<T> v2)
        {
            return ((Vector2)v1 - (Vector2)v2);
        }

        public static Vector2<T> operator *(Vector2<T> v1, Vector2<T> v2)
        {
            return ((Vector2)v1 * (Vector2)v2);
        }

        public static Vector2<T> operator /(Vector2<T> v1, Vector2<T> v2)
        {
            return ((Vector2)v1 / (Vector2)v2);
        }

        public static Vector2<T> operator &(Vector2<T> v1, Vector2<T> v2)
        {
            return ((Vector2)v1 & (Vector2)v2);
        }

        public static Vector2<T> operator %(Vector2<T> v1, Vector2<T> v2)
        {
            return ((Vector2)v1 % (Vector2)v2);
        }


        public override string ToString()
        {
            return this.X.ToString() + "; " + this.Y.ToString();
        }

        IntPtr IPointable.Ptr
        {
            get { return PointerManager.GetPtr(ref X); }
        }

        int IPointable.Size
        {
            get { return PointerManager.GetSize(ref X) * 2; }
        }
    }

    #endregion

    #region Vector3<T>

    public struct Vector3<T> : IPointable where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        public T X;

        public T Y;

        public T Z;

        public Vector3(T x, T y, T z)
        {
            
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3(T x)
            : this(x, x, x)
        {
        }

        public T this[int index]
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

        public static implicit operator Vector3(Vector3<T> v)
        {
            return new Vector3(v.X.AsFloating(), v.Y.AsFloating(), v.Z.AsFloating());
        }

        public static implicit operator Vector3<T>(Vector3 v)
        {
            return new Vector3<T>(v.X.As<T>(), v.Y.As<T>(), v.Z.As<T>());
        }

        public static explicit operator Vector2<T>(Vector3<T> v)
        {
            return new Vector2<T>(v.X, v.Y);
        }

        public static explicit operator T(Vector3<T> v)
        {
            return v.X;
        }


        public static Vector3<T> operator +(Vector3<T> v1, Vector3<T> v2)
        {
            return ((Vector3)v1 + (Vector3)v2);
        }

        public static Vector3<T> operator -(Vector3<T> v1, Vector3<T> v2)
        {
            return ((Vector3)v1 - (Vector3)v2);
        }

        public static Vector3<T> operator *(Vector3<T> v1, Vector3<T> v2)
        {
            return ((Vector3)v1 * (Vector3)v2);
        }

        public static Vector3<T> operator /(Vector3<T> v1, Vector3<T> v2)
        {
            return ((Vector3)v1 / (Vector3)v2);
        }

        public static Vector3<T> operator &(Vector3<T> v1, Vector3<T> v2)
        {
            return ((Vector3)v1 & (Vector3)v2);
        }

        public static Vector3<T> operator %(Vector3<T> v1, Vector3<T> v2)
        {
            return ((Vector3)v1 % (Vector3)v2);
        }

    
        public override string ToString()
        {
            return this.X.ToString() + "; " + this.Y.ToString() + "; "+this.Z.ToString ();
        }

        IntPtr IPointable.Ptr
        {
            get { return PointerManager.GetPtr(ref X); }
        }

        int IPointable.Size
        {
            get { return PointerManager.GetSize(ref X) * 3; }
        }
    }

    #endregion

    #region Vector4<T>

    public struct Vector4<T> : IPointable where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        public T X;

        public T Y;

        public T Z;

        public T W;

        public Vector4(T x, T y, T z, T w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public Vector4(Vector3<T> v, T w)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
            this.W = w;
        }

        public Vector4(T x)
            : this(x, x, x, x)
        {
        }

        public T this[int index]
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

        public static implicit operator Vector4(Vector4<T> v)
        {
            return new Vector4(v.X.AsFloating(), v.Y.AsFloating(), v.Z.AsFloating(), v.W.AsFloating ());
        }

        public static implicit operator Vector4<T>(Vector4 v)
        {
            return new Vector4<T>(v.X.As<T>(), v.Y.As<T>(), v.Z.As<T>(), v.W.As<T>());
        }

        public static explicit operator Vector3<T>(Vector4<T> v)
        {
            return new Vector3<T>(v.X, v.Y, v.Z);
        }

        public static explicit operator Vector2<T>(Vector4<T> v)
        {
            return new Vector2<T>(v.X, v.Y);
        }

        public static explicit operator Vector1<T>(Vector4<T> v)
        {
            return new Vector1<T>(v.X);
        }

        public static Vector4<T> operator +(Vector4<T> v1, Vector4<T> v2)
        {
            return ((Vector4)v1 + (Vector4)v2);
        }

        public static Vector4<T> operator -(Vector4<T> v1, Vector4<T> v2)
        {
            return ((Vector4)v1 - (Vector4)v2);
        }

        public static Vector4<T> operator *(Vector4<T> v1, Vector4<T> v2)
        {
            return ((Vector4)v1 * (Vector4)v2);
        }

        public static Vector4<T> operator /(Vector4<T> v1, Vector4<T> v2)
        {
            return ((Vector4)v1 / (Vector4)v2);
        }

        public static Vector4<T> operator &(Vector4<T> v1, Vector4<T> v2)
        {
            return ((Vector4)v1 & (Vector4)v2);
        }

        public static Vector4<T> operator %(Vector4<T> v1, Vector4<T> v2)
        {
            return ((Vector4)v1 % (Vector4)v2);
        }


        public override string ToString()
        {
            return this.X.ToString() + "; " + this.Y.ToString() + "; " + this.Z.ToString() + "; " + this.W.ToString();
        }

        IntPtr IPointable.Ptr
        {
            get { return PointerManager.GetPtr (ref X); }
        }

        int IPointable.Size
        {
            get { return PointerManager.GetSize(ref X) * 4; }
        }
    }

    #endregion

}
