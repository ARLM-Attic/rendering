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
using System.Maths.Scalaring;
using System.Unsafe;
using System.Runtime.InteropServices;

namespace System.Maths
{

    #region Matrix1x1<T>
    public struct Matrix1x1<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        #endregion

        #region Constructors
        public Matrix1x1(T M00)
        {
            this.M00 = M00;
        }
        #endregion

        #region Operators


        public static Matrix1x1<T> operator +(Matrix1x1<T> m1, Matrix1x1<T> m2)
        {
            return (Matrix1x1)m1 + (Matrix1x1)m2;
        }
        public static Matrix1x1<T> operator *(Matrix1x1<T> m1, Matrix1x1<T> m2)
        {
            return (Matrix1x1)m1 * (Matrix1x1)m2;
        }
        public static Matrix1x1<T> operator -(Matrix1x1<T> m1, Matrix1x1<T> m2)
        {
            return (Matrix1x1)m1 - (Matrix1x1)m2;
        }
        public static Matrix1x1<T> operator /(Matrix1x1<T> m1, Matrix1x1<T> m2)
        {
            return (Matrix1x1)m1 / (Matrix1x1)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix1x1(Matrix1x1<T> m)
        {
            return new Matrix1x1(m.M00.AsFloating());
        }

        public static implicit operator Matrix1x1<T>(Matrix1x1 m)
        {
            return new Matrix1x1<T>(m.M00.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix1x2<T>
    public struct Matrix1x2<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public T M01;
        #endregion

        #region Constructors
        public Matrix1x2(T value)
        {
            M00 = value;
            M01 = value;
        }

        public Matrix1x2(T M00, T M01)
        {
            this.M00 = M00;
            this.M01 = M01;
        }
        #endregion

        #region Operators


        public static Matrix1x2<T> operator +(Matrix1x2<T> m1, Matrix1x2<T> m2)
        {
            return (Matrix1x2)m1 + (Matrix1x2)m2;
        }
        public static Matrix1x2<T> operator *(Matrix1x2<T> m1, Matrix1x2<T> m2)
        {
            return (Matrix1x2)m1 * (Matrix1x2)m2;
        }
        public static Matrix1x2<T> operator -(Matrix1x2<T> m1, Matrix1x2<T> m2)
        {
            return (Matrix1x2)m1 - (Matrix1x2)m2;
        }
        public static Matrix1x2<T> operator /(Matrix1x2<T> m1, Matrix1x2<T> m2)
        {
            return (Matrix1x2)m1 / (Matrix1x2)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix1x2(Matrix1x2<T> m)
        {
            return new Matrix1x2(m.M00.AsFloating(), m.M01.AsFloating());
        }

        public static implicit operator Matrix1x2<T>(Matrix1x2 m)
        {
            return new Matrix1x2<T>(m.M00.As<T>(), m.M01.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix1x3<T>
    public struct Matrix1x3<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public T M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public T M02;
        #endregion

        #region Constructors
        public Matrix1x3(T value)
        {
            M00 = value;
            M01 = value;
            M02 = value;
        }

        public Matrix1x3(T M00, T M01, T M02)
        {
            this.M00 = M00;
            this.M01 = M01;
            this.M02 = M02;
        }
        #endregion

        #region Operators


        public static Matrix1x3<T> operator +(Matrix1x3<T> m1, Matrix1x3<T> m2)
        {
            return (Matrix1x3)m1 + (Matrix1x3)m2;
        }
        public static Matrix1x3<T> operator *(Matrix1x3<T> m1, Matrix1x3<T> m2)
        {
            return (Matrix1x3)m1 * (Matrix1x3)m2;
        }
        public static Matrix1x3<T> operator -(Matrix1x3<T> m1, Matrix1x3<T> m2)
        {
            return (Matrix1x3)m1 - (Matrix1x3)m2;
        }
        public static Matrix1x3<T> operator /(Matrix1x3<T> m1, Matrix1x3<T> m2)
        {
            return (Matrix1x3)m1 / (Matrix1x3)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix1x3(Matrix1x3<T> m)
        {
            return new Matrix1x3(m.M00.AsFloating(), m.M01.AsFloating(), m.M02.AsFloating());
        }

        public static implicit operator Matrix1x3<T>(Matrix1x3 m)
        {
            return new Matrix1x3<T>(m.M00.As<T>(), m.M01.As<T>(), m.M02.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix1x4<T>
    public struct Matrix1x4<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public T M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public T M02;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 3
        /// </sumary>
        public T M03;
        #endregion

        #region Constructors
        public Matrix1x4(T value)
        {
            M00 = value;
            M01 = value;
            M02 = value;
            M03 = value;
        }

        public Matrix1x4(T M00, T M01, T M02, T M03)
        {
            this.M00 = M00;
            this.M01 = M01;
            this.M02 = M02;
            this.M03 = M03;
        }
        #endregion

        #region Operators


        public static Matrix1x4<T> operator +(Matrix1x4<T> m1, Matrix1x4<T> m2)
        {
            return (Matrix1x4)m1 + (Matrix1x4)m2;
        }
        public static Matrix1x4<T> operator *(Matrix1x4<T> m1, Matrix1x4<T> m2)
        {
            return (Matrix1x4)m1 * (Matrix1x4)m2;
        }
        public static Matrix1x4<T> operator -(Matrix1x4<T> m1, Matrix1x4<T> m2)
        {
            return (Matrix1x4)m1 - (Matrix1x4)m2;
        }
        public static Matrix1x4<T> operator /(Matrix1x4<T> m1, Matrix1x4<T> m2)
        {
            return (Matrix1x4)m1 / (Matrix1x4)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix1x4(Matrix1x4<T> m)
        {
            return new Matrix1x4(m.M00.AsFloating(), m.M01.AsFloating(), m.M02.AsFloating(), m.M03.AsFloating());
        }

        public static implicit operator Matrix1x4<T>(Matrix1x4 m)
        {
            return new Matrix1x4<T>(m.M00.As<T>(), m.M01.As<T>(), m.M02.As<T>(), m.M03.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix2x1<T>
    public struct Matrix2x1<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public T M10;
        #endregion

        #region Constructors
        public Matrix2x1(T value)
        {
            M00 = value;
            M10 = value;
        }

        public Matrix2x1(T M00, T M10)
        {
            this.M00 = M00;
            this.M10 = M10;
        }
        #endregion

        #region Operators


        public static Matrix2x1<T> operator +(Matrix2x1<T> m1, Matrix2x1<T> m2)
        {
            return (Matrix2x1)m1 + (Matrix2x1)m2;
        }
        public static Matrix2x1<T> operator *(Matrix2x1<T> m1, Matrix2x1<T> m2)
        {
            return (Matrix2x1)m1 * (Matrix2x1)m2;
        }
        public static Matrix2x1<T> operator -(Matrix2x1<T> m1, Matrix2x1<T> m2)
        {
            return (Matrix2x1)m1 - (Matrix2x1)m2;
        }
        public static Matrix2x1<T> operator /(Matrix2x1<T> m1, Matrix2x1<T> m2)
        {
            return (Matrix2x1)m1 / (Matrix2x1)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix2x1(Matrix2x1<T> m)
        {
            return new Matrix2x1(m.M00.AsFloating(), m.M10.AsFloating());
        }

        public static implicit operator Matrix2x1<T>(Matrix2x1 m)
        {
            return new Matrix2x1<T>(m.M00.As<T>(), m.M10.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix2x2<T>
    public struct Matrix2x2<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public T M01;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public T M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public T M11;
        #endregion

        #region Constructors
        public Matrix2x2(T value)
        {
            M00 = value;
            M01 = value;
            M10 = value;
            M11 = value;
        }

        public Matrix2x2(T M00, T M01, T M10, T M11)
        {
            this.M00 = M00;
            this.M01 = M01;
            this.M10 = M10;
            this.M11 = M11;
        }
        #endregion

        #region Operators


        public static Matrix2x2<T> operator +(Matrix2x2<T> m1, Matrix2x2<T> m2)
        {
            return (Matrix2x2)m1 + (Matrix2x2)m2;
        }
        public static Matrix2x2<T> operator *(Matrix2x2<T> m1, Matrix2x2<T> m2)
        {
            return (Matrix2x2)m1 * (Matrix2x2)m2;
        }
        public static Matrix2x2<T> operator -(Matrix2x2<T> m1, Matrix2x2<T> m2)
        {
            return (Matrix2x2)m1 - (Matrix2x2)m2;
        }
        public static Matrix2x2<T> operator /(Matrix2x2<T> m1, Matrix2x2<T> m2)
        {
            return (Matrix2x2)m1 / (Matrix2x2)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix2x2(Matrix2x2<T> m)
        {
            return new Matrix2x2(m.M00.AsFloating(), m.M01.AsFloating(), m.M10.AsFloating(), m.M11.AsFloating());
        }

        public static implicit operator Matrix2x2<T>(Matrix2x2 m)
        {
            return new Matrix2x2<T>(m.M00.As<T>(), m.M01.As<T>(), m.M10.As<T>(), m.M11.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix2x3<T>
    public struct Matrix2x3<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public T M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public T M02;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public T M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public T M11;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 2
        /// </sumary>
        public T M12;
        #endregion

        #region Constructors
        public Matrix2x3(T value)
        {
            M00 = value;
            M01 = value;
            M02 = value;
            M10 = value;
            M11 = value;
            M12 = value;
        }

        public Matrix2x3(T M00, T M01, T M02, T M10, T M11, T M12)
        {
            this.M00 = M00;
            this.M01 = M01;
            this.M02 = M02;
            this.M10 = M10;
            this.M11 = M11;
            this.M12 = M12;
        }
        #endregion

        #region Operators


        public static Matrix2x3<T> operator +(Matrix2x3<T> m1, Matrix2x3<T> m2)
        {
            return (Matrix2x3)m1 + (Matrix2x3)m2;
        }
        public static Matrix2x3<T> operator *(Matrix2x3<T> m1, Matrix2x3<T> m2)
        {
            return (Matrix2x3)m1 * (Matrix2x3)m2;
        }
        public static Matrix2x3<T> operator -(Matrix2x3<T> m1, Matrix2x3<T> m2)
        {
            return (Matrix2x3)m1 - (Matrix2x3)m2;
        }
        public static Matrix2x3<T> operator /(Matrix2x3<T> m1, Matrix2x3<T> m2)
        {
            return (Matrix2x3)m1 / (Matrix2x3)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix2x3(Matrix2x3<T> m)
        {
            return new Matrix2x3(m.M00.AsFloating(), m.M01.AsFloating(), m.M02.AsFloating(), m.M10.AsFloating(), m.M11.AsFloating(), m.M12.AsFloating());
        }

        public static implicit operator Matrix2x3<T>(Matrix2x3 m)
        {
            return new Matrix2x3<T>(m.M00.As<T>(), m.M01.As<T>(), m.M02.As<T>(), m.M10.As<T>(), m.M11.As<T>(), m.M12.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix2x4<T>
    public struct Matrix2x4<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public T M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public T M02;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 3
        /// </sumary>
        public T M03;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public T M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public T M11;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 2
        /// </sumary>
        public T M12;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 3
        /// </sumary>
        public T M13;
        #endregion

        #region Constructors
        public Matrix2x4(T value)
        {
            M00 = value;
            M01 = value;
            M02 = value;
            M03 = value;
            M10 = value;
            M11 = value;
            M12 = value;
            M13 = value;
        }

        public Matrix2x4(T M00, T M01, T M02, T M03, T M10, T M11, T M12, T M13)
        {
            this.M00 = M00;
            this.M01 = M01;
            this.M02 = M02;
            this.M03 = M03;
            this.M10 = M10;
            this.M11 = M11;
            this.M12 = M12;
            this.M13 = M13;
        }
        #endregion

        #region Operators


        public static Matrix2x4<T> operator +(Matrix2x4<T> m1, Matrix2x4<T> m2)
        {
            return (Matrix2x4)m1 + (Matrix2x4)m2;
        }
        public static Matrix2x4<T> operator *(Matrix2x4<T> m1, Matrix2x4<T> m2)
        {
            return (Matrix2x4)m1 * (Matrix2x4)m2;
        }
        public static Matrix2x4<T> operator -(Matrix2x4<T> m1, Matrix2x4<T> m2)
        {
            return (Matrix2x4)m1 - (Matrix2x4)m2;
        }
        public static Matrix2x4<T> operator /(Matrix2x4<T> m1, Matrix2x4<T> m2)
        {
            return (Matrix2x4)m1 / (Matrix2x4)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix2x4(Matrix2x4<T> m)
        {
            return new Matrix2x4(m.M00.AsFloating(), m.M01.AsFloating(), m.M02.AsFloating(), m.M03.AsFloating(), m.M10.AsFloating(), m.M11.AsFloating(), m.M12.AsFloating(), m.M13.AsFloating());
        }

        public static implicit operator Matrix2x4<T>(Matrix2x4 m)
        {
            return new Matrix2x4<T>(m.M00.As<T>(), m.M01.As<T>(), m.M02.As<T>(), m.M03.As<T>(), m.M10.As<T>(), m.M11.As<T>(), m.M12.As<T>(), m.M13.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix3x1<T>
    public struct Matrix3x1<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public T M10;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public T M20;
        #endregion

        #region Constructors
        public Matrix3x1(T value)
        {
            M00 = value;
            M10 = value;
            M20 = value;
        }

        public Matrix3x1(T M00, T M10, T M20)
        {
            this.M00 = M00;
            this.M10 = M10;
            this.M20 = M20;
        }
        #endregion

        #region Operators


        public static Matrix3x1<T> operator +(Matrix3x1<T> m1, Matrix3x1<T> m2)
        {
            return (Matrix3x1)m1 + (Matrix3x1)m2;
        }
        public static Matrix3x1<T> operator *(Matrix3x1<T> m1, Matrix3x1<T> m2)
        {
            return (Matrix3x1)m1 * (Matrix3x1)m2;
        }
        public static Matrix3x1<T> operator -(Matrix3x1<T> m1, Matrix3x1<T> m2)
        {
            return (Matrix3x1)m1 - (Matrix3x1)m2;
        }
        public static Matrix3x1<T> operator /(Matrix3x1<T> m1, Matrix3x1<T> m2)
        {
            return (Matrix3x1)m1 / (Matrix3x1)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix3x1(Matrix3x1<T> m)
        {
            return new Matrix3x1(m.M00.AsFloating(), m.M10.AsFloating(), m.M20.AsFloating());
        }

        public static implicit operator Matrix3x1<T>(Matrix3x1 m)
        {
            return new Matrix3x1<T>(m.M00.As<T>(), m.M10.As<T>(), m.M20.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix3x2<T>
    public struct Matrix3x2<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public T M01;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public T M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public T M11;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public T M20;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 1
        /// </sumary>
        public T M21;
        #endregion

        #region Constructors
        public Matrix3x2(T value)
        {
            M00 = value;
            M01 = value;
            M10 = value;
            M11 = value;
            M20 = value;
            M21 = value;
        }

        public Matrix3x2(T M00, T M01, T M10, T M11, T M20, T M21)
        {
            this.M00 = M00;
            this.M01 = M01;
            this.M10 = M10;
            this.M11 = M11;
            this.M20 = M20;
            this.M21 = M21;
        }
        #endregion

        #region Operators


        public static Matrix3x2<T> operator +(Matrix3x2<T> m1, Matrix3x2<T> m2)
        {
            return (Matrix3x2)m1 + (Matrix3x2)m2;
        }
        public static Matrix3x2<T> operator *(Matrix3x2<T> m1, Matrix3x2<T> m2)
        {
            return (Matrix3x2)m1 * (Matrix3x2)m2;
        }
        public static Matrix3x2<T> operator -(Matrix3x2<T> m1, Matrix3x2<T> m2)
        {
            return (Matrix3x2)m1 - (Matrix3x2)m2;
        }
        public static Matrix3x2<T> operator /(Matrix3x2<T> m1, Matrix3x2<T> m2)
        {
            return (Matrix3x2)m1 / (Matrix3x2)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix3x2(Matrix3x2<T> m)
        {
            return new Matrix3x2(m.M00.AsFloating(), m.M01.AsFloating(), m.M10.AsFloating(), m.M11.AsFloating(), m.M20.AsFloating(), m.M21.AsFloating());
        }

        public static implicit operator Matrix3x2<T>(Matrix3x2 m)
        {
            return new Matrix3x2<T>(m.M00.As<T>(), m.M01.As<T>(), m.M10.As<T>(), m.M11.As<T>(), m.M20.As<T>(), m.M21.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix3x3<T>
    public struct Matrix3x3<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public T M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public T M02;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public T M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public T M11;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 2
        /// </sumary>
        public T M12;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public T M20;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 1
        /// </sumary>
        public T M21;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 2
        /// </sumary>
        public T M22;
        #endregion

        #region Constructors
        public Matrix3x3(T value)
        {
            M00 = value;
            M01 = value;
            M02 = value;
            M10 = value;
            M11 = value;
            M12 = value;
            M20 = value;
            M21 = value;
            M22 = value;
        }

        public Matrix3x3(T M00, T M01, T M02, T M10, T M11, T M12, T M20, T M21, T M22)
        {
            this.M00 = M00;
            this.M01 = M01;
            this.M02 = M02;
            this.M10 = M10;
            this.M11 = M11;
            this.M12 = M12;
            this.M20 = M20;
            this.M21 = M21;
            this.M22 = M22;
        }
        #endregion

        #region Operators


        public static Matrix3x3<T> operator +(Matrix3x3<T> m1, Matrix3x3<T> m2)
        {
            return (Matrix3x3)m1 + (Matrix3x3)m2;
        }
        public static Matrix3x3<T> operator *(Matrix3x3<T> m1, Matrix3x3<T> m2)
        {
            return (Matrix3x3)m1 * (Matrix3x3)m2;
        }
        public static Matrix3x3<T> operator -(Matrix3x3<T> m1, Matrix3x3<T> m2)
        {
            return (Matrix3x3)m1 - (Matrix3x3)m2;
        }
        public static Matrix3x3<T> operator /(Matrix3x3<T> m1, Matrix3x3<T> m2)
        {
            return (Matrix3x3)m1 / (Matrix3x3)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix3x3(Matrix3x3<T> m)
        {
            return new Matrix3x3(m.M00.AsFloating(), m.M01.AsFloating(), m.M02.AsFloating(), m.M10.AsFloating(), m.M11.AsFloating(), m.M12.AsFloating(), m.M20.AsFloating(), m.M21.AsFloating(), m.M22.AsFloating());
        }

        public static implicit operator Matrix3x3<T>(Matrix3x3 m)
        {
            return new Matrix3x3<T>(m.M00.As<T>(), m.M01.As<T>(), m.M02.As<T>(), m.M10.As<T>(), m.M11.As<T>(), m.M12.As<T>(), m.M20.As<T>(), m.M21.As<T>(), m.M22.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix3x4<T>
    public struct Matrix3x4<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public T M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public T M02;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 3
        /// </sumary>
        public T M03;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public T M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public T M11;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 2
        /// </sumary>
        public T M12;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 3
        /// </sumary>
        public T M13;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public T M20;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 1
        /// </sumary>
        public T M21;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 2
        /// </sumary>
        public T M22;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 3
        /// </sumary>
        public T M23;
        #endregion

        #region Constructors
        public Matrix3x4(T value)
        {
            M00 = value;
            M01 = value;
            M02 = value;
            M03 = value;
            M10 = value;
            M11 = value;
            M12 = value;
            M13 = value;
            M20 = value;
            M21 = value;
            M22 = value;
            M23 = value;
        }

        public Matrix3x4(T M00, T M01, T M02, T M03, T M10, T M11, T M12, T M13, T M20, T M21, T M22, T M23)
        {
            this.M00 = M00;
            this.M01 = M01;
            this.M02 = M02;
            this.M03 = M03;
            this.M10 = M10;
            this.M11 = M11;
            this.M12 = M12;
            this.M13 = M13;
            this.M20 = M20;
            this.M21 = M21;
            this.M22 = M22;
            this.M23 = M23;
        }
        #endregion

        #region Operators


        public static Matrix3x4<T> operator +(Matrix3x4<T> m1, Matrix3x4<T> m2)
        {
            return (Matrix3x4)m1 + (Matrix3x4)m2;
        }
        public static Matrix3x4<T> operator *(Matrix3x4<T> m1, Matrix3x4<T> m2)
        {
            return (Matrix3x4)m1 * (Matrix3x4)m2;
        }
        public static Matrix3x4<T> operator -(Matrix3x4<T> m1, Matrix3x4<T> m2)
        {
            return (Matrix3x4)m1 - (Matrix3x4)m2;
        }
        public static Matrix3x4<T> operator /(Matrix3x4<T> m1, Matrix3x4<T> m2)
        {
            return (Matrix3x4)m1 / (Matrix3x4)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix3x4(Matrix3x4<T> m)
        {
            return new Matrix3x4(m.M00.AsFloating(), m.M01.AsFloating(), m.M02.AsFloating(), m.M03.AsFloating(), m.M10.AsFloating(), m.M11.AsFloating(), m.M12.AsFloating(), m.M13.AsFloating(), m.M20.AsFloating(), m.M21.AsFloating(), m.M22.AsFloating(), m.M23.AsFloating());
        }

        public static implicit operator Matrix3x4<T>(Matrix3x4 m)
        {
            return new Matrix3x4<T>(m.M00.As<T>(), m.M01.As<T>(), m.M02.As<T>(), m.M03.As<T>(), m.M10.As<T>(), m.M11.As<T>(), m.M12.As<T>(), m.M13.As<T>(), m.M20.As<T>(), m.M21.As<T>(), m.M22.As<T>(), m.M23.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix4x1<T>
    public struct Matrix4x1<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public T M10;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public T M20;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 0
        /// </sumary>
        public T M30;
        #endregion

        #region Constructors
        public Matrix4x1(T value)
        {
            M00 = value;
            M10 = value;
            M20 = value;
            M30 = value;
        }

        public Matrix4x1(T M00, T M10, T M20, T M30)
        {
            this.M00 = M00;
            this.M10 = M10;
            this.M20 = M20;
            this.M30 = M30;
        }
        #endregion

        #region Operators


        public static Matrix4x1<T> operator +(Matrix4x1<T> m1, Matrix4x1<T> m2)
        {
            return (Matrix4x1)m1 + (Matrix4x1)m2;
        }
        public static Matrix4x1<T> operator *(Matrix4x1<T> m1, Matrix4x1<T> m2)
        {
            return (Matrix4x1)m1 * (Matrix4x1)m2;
        }
        public static Matrix4x1<T> operator -(Matrix4x1<T> m1, Matrix4x1<T> m2)
        {
            return (Matrix4x1)m1 - (Matrix4x1)m2;
        }
        public static Matrix4x1<T> operator /(Matrix4x1<T> m1, Matrix4x1<T> m2)
        {
            return (Matrix4x1)m1 / (Matrix4x1)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix4x1(Matrix4x1<T> m)
        {
            return new Matrix4x1(m.M00.AsFloating(), m.M10.AsFloating(), m.M20.AsFloating(), m.M30.AsFloating());
        }

        public static implicit operator Matrix4x1<T>(Matrix4x1 m)
        {
            return new Matrix4x1<T>(m.M00.As<T>(), m.M10.As<T>(), m.M20.As<T>(), m.M30.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix4x2<T>
    public struct Matrix4x2<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public T M01;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public T M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public T M11;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public T M20;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 1
        /// </sumary>
        public T M21;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 0
        /// </sumary>
        public T M30;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 1
        /// </sumary>
        public T M31;
        #endregion

        #region Constructors
        public Matrix4x2(T value)
        {
            M00 = value;
            M01 = value;
            M10 = value;
            M11 = value;
            M20 = value;
            M21 = value;
            M30 = value;
            M31 = value;
        }

        public Matrix4x2(T M00, T M01, T M10, T M11, T M20, T M21, T M30, T M31)
        {
            this.M00 = M00;
            this.M01 = M01;
            this.M10 = M10;
            this.M11 = M11;
            this.M20 = M20;
            this.M21 = M21;
            this.M30 = M30;
            this.M31 = M31;
        }
        #endregion

        #region Operators


        public static Matrix4x2<T> operator +(Matrix4x2<T> m1, Matrix4x2<T> m2)
        {
            return (Matrix4x2)m1 + (Matrix4x2)m2;
        }
        public static Matrix4x2<T> operator *(Matrix4x2<T> m1, Matrix4x2<T> m2)
        {
            return (Matrix4x2)m1 * (Matrix4x2)m2;
        }
        public static Matrix4x2<T> operator -(Matrix4x2<T> m1, Matrix4x2<T> m2)
        {
            return (Matrix4x2)m1 - (Matrix4x2)m2;
        }
        public static Matrix4x2<T> operator /(Matrix4x2<T> m1, Matrix4x2<T> m2)
        {
            return (Matrix4x2)m1 / (Matrix4x2)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix4x2(Matrix4x2<T> m)
        {
            return new Matrix4x2(m.M00.AsFloating(), m.M01.AsFloating(), m.M10.AsFloating(), m.M11.AsFloating(), m.M20.AsFloating(), m.M21.AsFloating(), m.M30.AsFloating(), m.M31.AsFloating());
        }

        public static implicit operator Matrix4x2<T>(Matrix4x2 m)
        {
            return new Matrix4x2<T>(m.M00.As<T>(), m.M01.As<T>(), m.M10.As<T>(), m.M11.As<T>(), m.M20.As<T>(), m.M21.As<T>(), m.M30.As<T>(), m.M31.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix4x3<T>
    public struct Matrix4x3<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public T M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public T M02;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public T M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public T M11;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 2
        /// </sumary>
        public T M12;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public T M20;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 1
        /// </sumary>
        public T M21;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 2
        /// </sumary>
        public T M22;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 0
        /// </sumary>
        public T M30;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 1
        /// </sumary>
        public T M31;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 2
        /// </sumary>
        public T M32;
        #endregion

        #region Constructors
        public Matrix4x3(T value)
        {
            M00 = value;
            M01 = value;
            M02 = value;
            M10 = value;
            M11 = value;
            M12 = value;
            M20 = value;
            M21 = value;
            M22 = value;
            M30 = value;
            M31 = value;
            M32 = value;
        }

        public Matrix4x3(T M00, T M01, T M02, T M10, T M11, T M12, T M20, T M21, T M22, T M30, T M31, T M32)
        {
            this.M00 = M00;
            this.M01 = M01;
            this.M02 = M02;
            this.M10 = M10;
            this.M11 = M11;
            this.M12 = M12;
            this.M20 = M20;
            this.M21 = M21;
            this.M22 = M22;
            this.M30 = M30;
            this.M31 = M31;
            this.M32 = M32;
        }
        #endregion

        #region Operators


        public static Matrix4x3<T> operator +(Matrix4x3<T> m1, Matrix4x3<T> m2)
        {
            return (Matrix4x3)m1 + (Matrix4x3)m2;
        }
        public static Matrix4x3<T> operator *(Matrix4x3<T> m1, Matrix4x3<T> m2)
        {
            return (Matrix4x3)m1 * (Matrix4x3)m2;
        }
        public static Matrix4x3<T> operator -(Matrix4x3<T> m1, Matrix4x3<T> m2)
        {
            return (Matrix4x3)m1 - (Matrix4x3)m2;
        }
        public static Matrix4x3<T> operator /(Matrix4x3<T> m1, Matrix4x3<T> m2)
        {
            return (Matrix4x3)m1 / (Matrix4x3)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix4x3(Matrix4x3<T> m)
        {
            return new Matrix4x3(m.M00.AsFloating(), m.M01.AsFloating(), m.M02.AsFloating(), m.M10.AsFloating(), m.M11.AsFloating(), m.M12.AsFloating(), m.M20.AsFloating(), m.M21.AsFloating(), m.M22.AsFloating(), m.M30.AsFloating(), m.M31.AsFloating(), m.M32.AsFloating());
        }

        public static implicit operator Matrix4x3<T>(Matrix4x3 m)
        {
            return new Matrix4x3<T>(m.M00.As<T>(), m.M01.As<T>(), m.M02.As<T>(), m.M10.As<T>(), m.M11.As<T>(), m.M12.As<T>(), m.M20.As<T>(), m.M21.As<T>(), m.M22.As<T>(), m.M30.As<T>(), m.M31.As<T>(), m.M32.As<T>());
        }

        #endregion

    }
    #endregion


    #region Matrix4x4<T>
    public struct Matrix4x4<T> where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public T M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public T M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public T M02;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 3
        /// </sumary>
        public T M03;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public T M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public T M11;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 2
        /// </sumary>
        public T M12;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 3
        /// </sumary>
        public T M13;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public T M20;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 1
        /// </sumary>
        public T M21;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 2
        /// </sumary>
        public T M22;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 3
        /// </sumary>
        public T M23;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 0
        /// </sumary>
        public T M30;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 1
        /// </sumary>
        public T M31;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 2
        /// </sumary>
        public T M32;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 3
        /// </sumary>
        public T M33;
        #endregion

        #region Constructors
        public Matrix4x4(T value)
        {
            M00 = value;
            M01 = value;
            M02 = value;
            M03 = value;
            M10 = value;
            M11 = value;
            M12 = value;
            M13 = value;
            M20 = value;
            M21 = value;
            M22 = value;
            M23 = value;
            M30 = value;
            M31 = value;
            M32 = value;
            M33 = value;
        }

        public Matrix4x4(T M00, T M01, T M02, T M03, T M10, T M11, T M12, T M13, T M20, T M21, T M22, T M23, T M30, T M31, T M32, T M33)
        {
            this.M00 = M00;
            this.M01 = M01;
            this.M02 = M02;
            this.M03 = M03;
            this.M10 = M10;
            this.M11 = M11;
            this.M12 = M12;
            this.M13 = M13;
            this.M20 = M20;
            this.M21 = M21;
            this.M22 = M22;
            this.M23 = M23;
            this.M30 = M30;
            this.M31 = M31;
            this.M32 = M32;
            this.M33 = M33;
        }
        #endregion

        #region Operators


        public static Matrix4x4<T> operator +(Matrix4x4<T> m1, Matrix4x4<T> m2)
        {
            return (Matrix4x4)m1 + (Matrix4x4)m2;
        }
        public static Matrix4x4<T> operator *(Matrix4x4<T> m1, Matrix4x4<T> m2)
        {
            return (Matrix4x4)m1 * (Matrix4x4)m2;
        }
        public static Matrix4x4<T> operator -(Matrix4x4<T> m1, Matrix4x4<T> m2)
        {
            return (Matrix4x4)m1 - (Matrix4x4)m2;
        }
        public static Matrix4x4<T> operator /(Matrix4x4<T> m1, Matrix4x4<T> m2)
        {
            return (Matrix4x4)m1 / (Matrix4x4)m2;
        }
        #endregion

        #region Conversions

        public static implicit operator Matrix4x4(Matrix4x4<T> m)
        {
            return new Matrix4x4(m.M00.AsFloating(), m.M01.AsFloating(), m.M02.AsFloating(), m.M03.AsFloating(), m.M10.AsFloating(), m.M11.AsFloating(), m.M12.AsFloating(), m.M13.AsFloating(), m.M20.AsFloating(), m.M21.AsFloating(), m.M22.AsFloating(), m.M23.AsFloating(), m.M30.AsFloating(), m.M31.AsFloating(), m.M32.AsFloating(), m.M33.AsFloating());
        }

        public static implicit operator Matrix4x4<T>(Matrix4x4 m)
        {
            return new Matrix4x4<T>(m.M00.As<T>(), m.M01.As<T>(), m.M02.As<T>(), m.M03.As<T>(), m.M10.As<T>(), m.M11.As<T>(), m.M12.As<T>(), m.M13.As<T>(), m.M20.As<T>(), m.M21.As<T>(), m.M22.As<T>(), m.M23.As<T>(), m.M30.As<T>(), m.M31.As<T>(), m.M32.As<T>(), m.M33.As<T>());
        }

        #endregion

    }
    #endregion

}










