using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Maths.Scalaring;

namespace System.Maths
{

    #region Matrix1x1
    public struct Matrix1x1
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix1x1(float M00)
        {
            this.M00 = M00;
        }
        #endregion

        #region Operators
        public static Matrix1x1 operator +(Matrix1x1 m1, Matrix1x1 m2)
        {
            return new Matrix1x1(m1.M00 + (m2.M00));
        }

        public static Matrix1x1 operator *(Matrix1x1 m1, float a)
        {
            return new Matrix1x1(m1.M00 * (a));
        }

        public static Matrix1x1 operator *(float a, Matrix1x1 m) { return m * a; }

        public static Matrix1x1 operator /(Matrix1x1 m1, float a)
        {
            return new Matrix1x1(m1.M00 / (a));
        }

        public static Matrix1x1 operator -(Matrix1x1 m1, Matrix1x1 m2)
        {
            return new Matrix1x1(m1.M00 - (m2.M00));
        }

        public static Matrix1x1 operator /(Matrix1x1 m1, Matrix1x1 m2)
        {
            return new Matrix1x1(m1.M00 / (m2.M00));
        }

        public static Matrix1x1 operator *(Matrix1x1 m1, Matrix1x1 m2)
        {
            return new Matrix1x1(m1.M00 * (m2.M00));
        }

        public Matrix1x1 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions


        public static implicit operator Vector1(Matrix1x1 m)
        {
            return new Vector1(m.M00);
        }

        public static implicit operator Matrix1x1(Vector1 v)
        {
            return new Matrix1x1(v.X);
        }

        #endregion

        public Matrix1x1 Inverse
        {
            get { return new Matrix1x1(1 / this.M00); }
        }

        public float Determinant
        {
            get { return GMath.determinant(this); }
        }

        public bool IsSingular { get { return Determinant == 0; } }
    }
    #endregion


    #region Matrix1x2
    public struct Matrix1x2
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public float M01;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix1x2(float value)
        {
            M00 = value;
            M01 = value;
        }

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix1x2(float M00, float M01)
        {
            this.M00 = M00;
            this.M01 = M01;
        }
        #endregion

        #region Operators
        public static Matrix1x2 operator +(Matrix1x2 m1, Matrix1x2 m2)
        {
            return new Matrix1x2(m1.M00 + (m2.M00), m1.M01 + (m2.M01));
        }

        public static Matrix1x2 operator *(Matrix1x2 m1, float a)
        {
            return new Matrix1x2(m1.M00 * (a), m1.M01 * (a));
        }

        public static Matrix1x2 operator *(float a, Matrix1x2 m) { return m * a; }

        public static Matrix1x2 operator /(Matrix1x2 m1, float a)
        {
            return new Matrix1x2(m1.M00 / (a), m1.M01 / (a));
        }

        public static Matrix1x2 operator -(Matrix1x2 m1, Matrix1x2 m2)
        {
            return new Matrix1x2(m1.M00 - (m2.M00), m1.M01 - (m2.M01));
        }

        public static Matrix1x2 operator /(Matrix1x2 m1, Matrix1x2 m2)
        {
            return new Matrix1x2(m1.M00 / (m2.M00), m1.M01 / (m2.M01));
        }

        public static Matrix1x2 operator *(Matrix1x2 m1, Matrix1x2 m2)
        {
            return new Matrix1x2(m1.M00 * (m2.M00), m1.M01 * (m2.M01));
        }

        public Matrix2x1 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix1x2 m)
        {
            return new Matrix1x1(m.M00);
        }

        public static implicit operator Vector2(Matrix1x2 m)
        {
            return new Vector2(m.M00, m.M01);
        }

        public static implicit operator Matrix1x2(Vector2 v)
        {
            return new Matrix1x2(v.X, v.Y);
        }

        #endregion

    }
    #endregion


    #region Matrix1x3
    public struct Matrix1x3
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public float M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public float M02;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix1x3(float value)
        {
            M00 = value;
            M01 = value;
            M02 = value;
        }

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix1x3(float M00, float M01, float M02)
        {
            this.M00 = M00;
            this.M01 = M01;
            this.M02 = M02;
        }
        #endregion

        #region Operators
        public static Matrix1x3 operator +(Matrix1x3 m1, Matrix1x3 m2)
        {
            return new Matrix1x3(m1.M00 + (m2.M00), m1.M01 + (m2.M01), m1.M02 + (m2.M02));
        }

        public static Matrix1x3 operator *(Matrix1x3 m1, float a)
        {
            return new Matrix1x3(m1.M00 * (a), m1.M01 * (a), m1.M02 * (a));
        }

        public static Matrix1x3 operator *(float a, Matrix1x3 m) { return m * a; }

        public static Matrix1x3 operator /(Matrix1x3 m1, float a)
        {
            return new Matrix1x3(m1.M00 / (a), m1.M01 / (a), m1.M02 / (a));
        }

        public static Matrix1x3 operator -(Matrix1x3 m1, Matrix1x3 m2)
        {
            return new Matrix1x3(m1.M00 - (m2.M00), m1.M01 - (m2.M01), m1.M02 - (m2.M02));
        }

        public static Matrix1x3 operator /(Matrix1x3 m1, Matrix1x3 m2)
        {
            return new Matrix1x3(m1.M00 / (m2.M00), m1.M01 / (m2.M01), m1.M02 / (m2.M02));
        }

        public static Matrix1x3 operator *(Matrix1x3 m1, Matrix1x3 m2)
        {
            return new Matrix1x3(m1.M00 * (m2.M00), m1.M01 * (m2.M01), m1.M02 * (m2.M02));
        }

        public Matrix3x1 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix1x3 m)
        {
            return new Matrix1x1(m.M00);
        }
        public static explicit operator Matrix1x2(Matrix1x3 m)
        {
            return new Matrix1x2(m.M00, m.M01);
        }

        public static implicit operator Vector3(Matrix1x3 m)
        {
            return new Vector3(m.M00, m.M01, m.M02);
        }

        public static implicit operator Matrix1x3(Vector3 v)
        {
            return new Matrix1x3(v.X, v.Y, v.Z);
        }

        #endregion

    }
    #endregion


    #region Matrix1x4
    public struct Matrix1x4
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public float M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public float M02;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 3
        /// </sumary>
        public float M03;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix1x4(float value)
        {
            M00 = value;
            M01 = value;
            M02 = value;
            M03 = value;
        }

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix1x4(float M00, float M01, float M02, float M03)
        {
            this.M00 = M00;
            this.M01 = M01;
            this.M02 = M02;
            this.M03 = M03;
        }
        #endregion

        #region Operators
        public static Matrix1x4 operator +(Matrix1x4 m1, Matrix1x4 m2)
        {
            return new Matrix1x4(m1.M00 + (m2.M00), m1.M01 + (m2.M01), m1.M02 + (m2.M02), m1.M03 + (m2.M03));
        }

        public static Matrix1x4 operator *(Matrix1x4 m1, float a)
        {
            return new Matrix1x4(m1.M00 * (a), m1.M01 * (a), m1.M02 * (a), m1.M03 * (a));
        }

        public static Matrix1x4 operator *(float a, Matrix1x4 m) { return m * a; }

        public static Matrix1x4 operator /(Matrix1x4 m1, float a)
        {
            return new Matrix1x4(m1.M00 / (a), m1.M01 / (a), m1.M02 / (a), m1.M03 / (a));
        }

        public static Matrix1x4 operator -(Matrix1x4 m1, Matrix1x4 m2)
        {
            return new Matrix1x4(m1.M00 - (m2.M00), m1.M01 - (m2.M01), m1.M02 - (m2.M02), m1.M03 - (m2.M03));
        }

        public static Matrix1x4 operator /(Matrix1x4 m1, Matrix1x4 m2)
        {
            return new Matrix1x4(m1.M00 / (m2.M00), m1.M01 / (m2.M01), m1.M02 / (m2.M02), m1.M03 / (m2.M03));
        }

        public static Matrix1x4 operator *(Matrix1x4 m1, Matrix1x4 m2)
        {
            return new Matrix1x4(m1.M00 * (m2.M00), m1.M01 * (m2.M01), m1.M02 * (m2.M02), m1.M03 * (m2.M03));
        }

        public Matrix4x1 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix1x4 m)
        {
            return new Matrix1x1(m.M00);
        }
        public static explicit operator Matrix1x2(Matrix1x4 m)
        {
            return new Matrix1x2(m.M00, m.M01);
        }
        public static explicit operator Matrix1x3(Matrix1x4 m)
        {
            return new Matrix1x3(m.M00, m.M01, m.M02);
        }

        public static implicit operator Vector4(Matrix1x4 m)
        {
            return new Vector4(m.M00, m.M01, m.M02, m.M03);
        }

        public static implicit operator Matrix1x4(Vector4 v)
        {
            return new Matrix1x4(v.X, v.Y, v.Z, v.W);
        }

        #endregion

    }
    #endregion


    #region Matrix2x1
    public struct Matrix2x1
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public float M10;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix2x1(float value)
        {
            M00 = value;
            M10 = value;
        }

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix2x1(float M00, float M10)
        {
            this.M00 = M00;
            this.M10 = M10;
        }
        #endregion

        #region Operators
        public static Matrix2x1 operator +(Matrix2x1 m1, Matrix2x1 m2)
        {
            return new Matrix2x1(m1.M00 + (m2.M00), m1.M10 + (m2.M10));
        }

        public static Matrix2x1 operator *(Matrix2x1 m1, float a)
        {
            return new Matrix2x1(m1.M00 * (a), m1.M10 * (a));
        }

        public static Matrix2x1 operator *(float a, Matrix2x1 m) { return m * a; }

        public static Matrix2x1 operator /(Matrix2x1 m1, float a)
        {
            return new Matrix2x1(m1.M00 / (a), m1.M10 / (a));
        }

        public static Matrix2x1 operator -(Matrix2x1 m1, Matrix2x1 m2)
        {
            return new Matrix2x1(m1.M00 - (m2.M00), m1.M10 - (m2.M10));
        }

        public static Matrix2x1 operator /(Matrix2x1 m1, Matrix2x1 m2)
        {
            return new Matrix2x1(m1.M00 / (m2.M00), m1.M10 / (m2.M10));
        }

        public static Matrix2x1 operator *(Matrix2x1 m1, Matrix2x1 m2)
        {
            return new Matrix2x1(m1.M00 * (m2.M00), m1.M10 * (m2.M10));
        }

        public Matrix1x2 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix2x1 m)
        {
            return new Matrix1x1(m.M00);
        }

        public static explicit operator Vector2(Matrix2x1 m)
        {
            return new Vector2(m.M00, m.M10);
        }

        public static explicit operator Matrix2x1(Vector2 v)
        {
            return new Matrix2x1(v.X, v.Y);
        }

        #endregion

    }
    #endregion


    #region Matrix2x2
    public struct Matrix2x2
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public float M01;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public float M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public float M11;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix2x2(float value)
        {
            M00 = value;
            M01 = value;
            M10 = value;
            M11 = value;
        }

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix2x2(float M00, float M01, float M10, float M11)
        {
            this.M00 = M00;
            this.M01 = M01;
            this.M10 = M10;
            this.M11 = M11;
        }
        #endregion

        #region Operators
        public static Matrix2x2 operator +(Matrix2x2 m1, Matrix2x2 m2)
        {
            return new Matrix2x2(m1.M00 + (m2.M00), m1.M01 + (m2.M01), m1.M10 + (m2.M10), m1.M11 + (m2.M11));
        }

        public static Matrix2x2 operator *(Matrix2x2 m1, float a)
        {
            return new Matrix2x2(m1.M00 * (a), m1.M01 * (a), m1.M10 * (a), m1.M11 * (a));
        }

        public static Matrix2x2 operator *(float a, Matrix2x2 m) { return m * a; }

        public static Matrix2x2 operator /(Matrix2x2 m1, float a)
        {
            return new Matrix2x2(m1.M00 / (a), m1.M01 / (a), m1.M10 / (a), m1.M11 / (a));
        }

        public static Matrix2x2 operator -(Matrix2x2 m1, Matrix2x2 m2)
        {
            return new Matrix2x2(m1.M00 - (m2.M00), m1.M01 - (m2.M01), m1.M10 - (m2.M10), m1.M11 - (m2.M11));
        }

        public static Matrix2x2 operator /(Matrix2x2 m1, Matrix2x2 m2)
        {
            return new Matrix2x2(m1.M00 / (m2.M00), m1.M01 / (m2.M01), m1.M10 / (m2.M10), m1.M11 / (m2.M11));
        }

        public static Matrix2x2 operator *(Matrix2x2 m1, Matrix2x2 m2)
        {
            return new Matrix2x2(m1.M00 * (m2.M00), m1.M01 * (m2.M01), m1.M10 * (m2.M10), m1.M11 * (m2.M11));
        }

        public Matrix2x2 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix2x2 m)
        {
            return new Matrix1x1(m.M00);
        }
        public static explicit operator Matrix1x2(Matrix2x2 m)
        {
            return new Matrix1x2(m.M00, m.M01);
        }
        public static explicit operator Matrix2x1(Matrix2x2 m)
        {
            return new Matrix2x1(m.M00, m.M10);
        }

        public static explicit operator Vector4(Matrix2x2 m)
        {
            return new Vector4(m.M00, m.M01, m.M10, m.M11);
        }

        public static explicit operator Matrix2x2(Vector4 v)
        {
            return new Matrix2x2(v.X, v.Y, v.Z, v.W);
        }

        #endregion

        public Matrix2x2 Inverse
        {
            get
            {
                /// 00 01
                /// 10 11
                float Min00 = M11 ;
                float Min01 = M10 ;

                float det = Min00 * M00 - Min01 * M01;

                if (det == 0)
                    throw new InvalidOperationException("Singular matrices have not inverse");

                float Min10 = M01;
                float Min11 = M00;

                return new Matrix2x2(
                    (+Min00 / det), (-Min10 / det),
                    (-Min01 / det), (+Min11 / det));
            }
        }

        public float Determinant
        {
            get { return GMath.determinant(this); }
        }

        public bool IsSingular { get { return Determinant == 0; } }
    }
    #endregion


    #region Matrix2x3
    public struct Matrix2x3
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public float M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public float M02;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public float M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public float M11;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 2
        /// </sumary>
        public float M12;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix2x3(float value)
        {
            M00 = value;
            M01 = value;
            M02 = value;
            M10 = value;
            M11 = value;
            M12 = value;
        }

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix2x3(float M00, float M01, float M02, float M10, float M11, float M12)
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
        public static Matrix2x3 operator +(Matrix2x3 m1, Matrix2x3 m2)
        {
            return new Matrix2x3(m1.M00 + (m2.M00), m1.M01 + (m2.M01), m1.M02 + (m2.M02), m1.M10 + (m2.M10), m1.M11 + (m2.M11), m1.M12 + (m2.M12));
        }

        public static Matrix2x3 operator *(Matrix2x3 m1, float a)
        {
            return new Matrix2x3(m1.M00 * (a), m1.M01 * (a), m1.M02 * (a), m1.M10 * (a), m1.M11 * (a), m1.M12 * (a));
        }

        public static Matrix2x3 operator *(float a, Matrix2x3 m) { return m * a; }

        public static Matrix2x3 operator /(Matrix2x3 m1, float a)
        {
            return new Matrix2x3(m1.M00 / (a), m1.M01 / (a), m1.M02 / (a), m1.M10 / (a), m1.M11 / (a), m1.M12 / (a));
        }

        public static Matrix2x3 operator -(Matrix2x3 m1, Matrix2x3 m2)
        {
            return new Matrix2x3(m1.M00 - (m2.M00), m1.M01 - (m2.M01), m1.M02 - (m2.M02), m1.M10 - (m2.M10), m1.M11 - (m2.M11), m1.M12 - (m2.M12));
        }

        public static Matrix2x3 operator /(Matrix2x3 m1, Matrix2x3 m2)
        {
            return new Matrix2x3(m1.M00 / (m2.M00), m1.M01 / (m2.M01), m1.M02 / (m2.M02), m1.M10 / (m2.M10), m1.M11 / (m2.M11), m1.M12 / (m2.M12));
        }

        public static Matrix2x3 operator *(Matrix2x3 m1, Matrix2x3 m2)
        {
            return new Matrix2x3(m1.M00 * (m2.M00), m1.M01 * (m2.M01), m1.M02 * (m2.M02), m1.M10 * (m2.M10), m1.M11 * (m2.M11), m1.M12 * (m2.M12));
        }

        public Matrix3x2 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix2x3 m)
        {
            return new Matrix1x1(m.M00);
        }
        public static explicit operator Matrix1x2(Matrix2x3 m)
        {
            return new Matrix1x2(m.M00, m.M01);
        }
        public static explicit operator Matrix1x3(Matrix2x3 m)
        {
            return new Matrix1x3(m.M00, m.M01, m.M02);
        }
        public static explicit operator Matrix2x1(Matrix2x3 m)
        {
            return new Matrix2x1(m.M00, m.M10);
        }
        public static explicit operator Matrix2x2(Matrix2x3 m)
        {
            return new Matrix2x2(m.M00, m.M01, m.M10, m.M11);
        }


        #endregion

    }
    #endregion


    #region Matrix2x4
    public struct Matrix2x4
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public float M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public float M02;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 3
        /// </sumary>
        public float M03;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public float M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public float M11;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 2
        /// </sumary>
        public float M12;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 3
        /// </sumary>
        public float M13;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix2x4(float value)
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

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix2x4(float M00, float M01, float M02, float M03, float M10, float M11, float M12, float M13)
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
        public static Matrix2x4 operator +(Matrix2x4 m1, Matrix2x4 m2)
        {
            return new Matrix2x4(m1.M00 + (m2.M00), m1.M01 + (m2.M01), m1.M02 + (m2.M02), m1.M03 + (m2.M03), m1.M10 + (m2.M10), m1.M11 + (m2.M11), m1.M12 + (m2.M12), m1.M13 + (m2.M13));
        }

        public static Matrix2x4 operator *(Matrix2x4 m1, float a)
        {
            return new Matrix2x4(m1.M00 * (a), m1.M01 * (a), m1.M02 * (a), m1.M03 * (a), m1.M10 * (a), m1.M11 * (a), m1.M12 * (a), m1.M13 * (a));
        }

        public static Matrix2x4 operator *(float a, Matrix2x4 m) { return m * a; }

        public static Matrix2x4 operator /(Matrix2x4 m1, float a)
        {
            return new Matrix2x4(m1.M00 / (a), m1.M01 / (a), m1.M02 / (a), m1.M03 / (a), m1.M10 / (a), m1.M11 / (a), m1.M12 / (a), m1.M13 / (a));
        }

        public static Matrix2x4 operator -(Matrix2x4 m1, Matrix2x4 m2)
        {
            return new Matrix2x4(m1.M00 - (m2.M00), m1.M01 - (m2.M01), m1.M02 - (m2.M02), m1.M03 - (m2.M03), m1.M10 - (m2.M10), m1.M11 - (m2.M11), m1.M12 - (m2.M12), m1.M13 - (m2.M13));
        }

        public static Matrix2x4 operator /(Matrix2x4 m1, Matrix2x4 m2)
        {
            return new Matrix2x4(m1.M00 / (m2.M00), m1.M01 / (m2.M01), m1.M02 / (m2.M02), m1.M03 / (m2.M03), m1.M10 / (m2.M10), m1.M11 / (m2.M11), m1.M12 / (m2.M12), m1.M13 / (m2.M13));
        }

        public static Matrix2x4 operator *(Matrix2x4 m1, Matrix2x4 m2)
        {
            return new Matrix2x4(m1.M00 * (m2.M00), m1.M01 * (m2.M01), m1.M02 * (m2.M02), m1.M03 * (m2.M03), m1.M10 * (m2.M10), m1.M11 * (m2.M11), m1.M12 * (m2.M12), m1.M13 * (m2.M13));
        }

        public Matrix4x2 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix2x4 m)
        {
            return new Matrix1x1(m.M00);
        }
        public static explicit operator Matrix1x2(Matrix2x4 m)
        {
            return new Matrix1x2(m.M00, m.M01);
        }
        public static explicit operator Matrix1x3(Matrix2x4 m)
        {
            return new Matrix1x3(m.M00, m.M01, m.M02);
        }
        public static explicit operator Matrix1x4(Matrix2x4 m)
        {
            return new Matrix1x4(m.M00, m.M01, m.M02, m.M03);
        }
        public static explicit operator Matrix2x1(Matrix2x4 m)
        {
            return new Matrix2x1(m.M00, m.M10);
        }
        public static explicit operator Matrix2x2(Matrix2x4 m)
        {
            return new Matrix2x2(m.M00, m.M01, m.M10, m.M11);
        }
        public static explicit operator Matrix2x3(Matrix2x4 m)
        {
            return new Matrix2x3(m.M00, m.M01, m.M02, m.M10, m.M11, m.M12);
        }


        #endregion

    }
    #endregion


    #region Matrix3x1
    public struct Matrix3x1
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public float M10;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public float M20;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix3x1(float value)
        {
            M00 = value;
            M10 = value;
            M20 = value;
        }

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix3x1(float M00, float M10, float M20)
        {
            this.M00 = M00;
            this.M10 = M10;
            this.M20 = M20;
        }
        #endregion

        #region Operators
        public static Matrix3x1 operator +(Matrix3x1 m1, Matrix3x1 m2)
        {
            return new Matrix3x1(m1.M00 + (m2.M00), m1.M10 + (m2.M10), m1.M20 + (m2.M20));
        }

        public static Matrix3x1 operator *(Matrix3x1 m1, float a)
        {
            return new Matrix3x1(m1.M00 * (a), m1.M10 * (a), m1.M20 * (a));
        }

        public static Matrix3x1 operator *(float a, Matrix3x1 m) { return m * a; }

        public static Matrix3x1 operator /(Matrix3x1 m1, float a)
        {
            return new Matrix3x1(m1.M00 / (a), m1.M10 / (a), m1.M20 / (a));
        }

        public static Matrix3x1 operator -(Matrix3x1 m1, Matrix3x1 m2)
        {
            return new Matrix3x1(m1.M00 - (m2.M00), m1.M10 - (m2.M10), m1.M20 - (m2.M20));
        }

        public static Matrix3x1 operator /(Matrix3x1 m1, Matrix3x1 m2)
        {
            return new Matrix3x1(m1.M00 / (m2.M00), m1.M10 / (m2.M10), m1.M20 / (m2.M20));
        }

        public static Matrix3x1 operator *(Matrix3x1 m1, Matrix3x1 m2)
        {
            return new Matrix3x1(m1.M00 * (m2.M00), m1.M10 * (m2.M10), m1.M20 * (m2.M20));
        }

        public Matrix1x3 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix3x1 m)
        {
            return new Matrix1x1(m.M00);
        }
        public static explicit operator Matrix2x1(Matrix3x1 m)
        {
            return new Matrix2x1(m.M00, m.M10);
        }

        public static explicit operator Vector3(Matrix3x1 m)
        {
            return new Vector3(m.M00, m.M10, m.M20);
        }

        public static explicit operator Matrix3x1(Vector3 v)
        {
            return new Matrix3x1(v.X, v.Y, v.Z);
        }

        #endregion

    }
    #endregion


    #region Matrix3x2
    public struct Matrix3x2
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public float M01;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public float M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public float M11;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public float M20;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 1
        /// </sumary>
        public float M21;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix3x2(float value)
        {
            M00 = value;
            M01 = value;
            M10 = value;
            M11 = value;
            M20 = value;
            M21 = value;
        }

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix3x2(float M00, float M01, float M10, float M11, float M20, float M21)
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
        public static Matrix3x2 operator +(Matrix3x2 m1, Matrix3x2 m2)
        {
            return new Matrix3x2(m1.M00 + (m2.M00), m1.M01 + (m2.M01), m1.M10 + (m2.M10), m1.M11 + (m2.M11), m1.M20 + (m2.M20), m1.M21 + (m2.M21));
        }

        public static Matrix3x2 operator *(Matrix3x2 m1, float a)
        {
            return new Matrix3x2(m1.M00 * (a), m1.M01 * (a), m1.M10 * (a), m1.M11 * (a), m1.M20 * (a), m1.M21 * (a));
        }

        public static Matrix3x2 operator *(float a, Matrix3x2 m) { return m * a; }

        public static Matrix3x2 operator /(Matrix3x2 m1, float a)
        {
            return new Matrix3x2(m1.M00 / (a), m1.M01 / (a), m1.M10 / (a), m1.M11 / (a), m1.M20 / (a), m1.M21 / (a));
        }

        public static Matrix3x2 operator -(Matrix3x2 m1, Matrix3x2 m2)
        {
            return new Matrix3x2(m1.M00 - (m2.M00), m1.M01 - (m2.M01), m1.M10 - (m2.M10), m1.M11 - (m2.M11), m1.M20 - (m2.M20), m1.M21 - (m2.M21));
        }

        public static Matrix3x2 operator /(Matrix3x2 m1, Matrix3x2 m2)
        {
            return new Matrix3x2(m1.M00 / (m2.M00), m1.M01 / (m2.M01), m1.M10 / (m2.M10), m1.M11 / (m2.M11), m1.M20 / (m2.M20), m1.M21 / (m2.M21));
        }

        public static Matrix3x2 operator *(Matrix3x2 m1, Matrix3x2 m2)
        {
            return new Matrix3x2(m1.M00 * (m2.M00), m1.M01 * (m2.M01), m1.M10 * (m2.M10), m1.M11 * (m2.M11), m1.M20 * (m2.M20), m1.M21 * (m2.M21));
        }

        public Matrix2x3 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix3x2 m)
        {
            return new Matrix1x1(m.M00);
        }
        public static explicit operator Matrix1x2(Matrix3x2 m)
        {
            return new Matrix1x2(m.M00, m.M01);
        }
        public static explicit operator Matrix2x1(Matrix3x2 m)
        {
            return new Matrix2x1(m.M00, m.M10);
        }
        public static explicit operator Matrix2x2(Matrix3x2 m)
        {
            return new Matrix2x2(m.M00, m.M01, m.M10, m.M11);
        }
        public static explicit operator Matrix3x1(Matrix3x2 m)
        {
            return new Matrix3x1(m.M00, m.M10, m.M20);
        }


        #endregion

    }
    #endregion


    #region Matrix3x3
    public struct Matrix3x3
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public float M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public float M02;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public float M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public float M11;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 2
        /// </sumary>
        public float M12;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public float M20;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 1
        /// </sumary>
        public float M21;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 2
        /// </sumary>
        public float M22;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix3x3(float value)
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

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix3x3(float M00, float M01, float M02, float M10, float M11, float M12, float M20, float M21, float M22)
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
        public static Matrix3x3 operator +(Matrix3x3 m1, Matrix3x3 m2)
        {
            return new Matrix3x3(m1.M00 + (m2.M00), m1.M01 + (m2.M01), m1.M02 + (m2.M02), m1.M10 + (m2.M10), m1.M11 + (m2.M11), m1.M12 + (m2.M12), m1.M20 + (m2.M20), m1.M21 + (m2.M21), m1.M22 + (m2.M22));
        }

        public static Matrix3x3 operator *(Matrix3x3 m1, float a)
        {
            return new Matrix3x3(m1.M00 * (a), m1.M01 * (a), m1.M02 * (a), m1.M10 * (a), m1.M11 * (a), m1.M12 * (a), m1.M20 * (a), m1.M21 * (a), m1.M22 * (a));
        }

        public static Matrix3x3 operator *(float a, Matrix3x3 m) { return m * a; }

        public static Matrix3x3 operator /(Matrix3x3 m1, float a)
        {
            return new Matrix3x3(m1.M00 / (a), m1.M01 / (a), m1.M02 / (a), m1.M10 / (a), m1.M11 / (a), m1.M12 / (a), m1.M20 / (a), m1.M21 / (a), m1.M22 / (a));
        }

        public static Matrix3x3 operator -(Matrix3x3 m1, Matrix3x3 m2)
        {
            return new Matrix3x3(m1.M00 - (m2.M00), m1.M01 - (m2.M01), m1.M02 - (m2.M02), m1.M10 - (m2.M10), m1.M11 - (m2.M11), m1.M12 - (m2.M12), m1.M20 - (m2.M20), m1.M21 - (m2.M21), m1.M22 - (m2.M22));
        }

        public static Matrix3x3 operator /(Matrix3x3 m1, Matrix3x3 m2)
        {
            return new Matrix3x3(m1.M00 / (m2.M00), m1.M01 / (m2.M01), m1.M02 / (m2.M02), m1.M10 / (m2.M10), m1.M11 / (m2.M11), m1.M12 / (m2.M12), m1.M20 / (m2.M20), m1.M21 / (m2.M21), m1.M22 / (m2.M22));
        }

        public static Matrix3x3 operator *(Matrix3x3 m1, Matrix3x3 m2)
        {
            return new Matrix3x3(m1.M00 * (m2.M00), m1.M01 * (m2.M01), m1.M02 * (m2.M02), m1.M10 * (m2.M10), m1.M11 * (m2.M11), m1.M12 * (m2.M12), m1.M20 * (m2.M20), m1.M21 * (m2.M21), m1.M22 * (m2.M22));
        }

        public Matrix3x3 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix3x3 m)
        {
            return new Matrix1x1(m.M00);
        }
        public static explicit operator Matrix1x2(Matrix3x3 m)
        {
            return new Matrix1x2(m.M00, m.M01);
        }
        public static explicit operator Matrix1x3(Matrix3x3 m)
        {
            return new Matrix1x3(m.M00, m.M01, m.M02);
        }
        public static explicit operator Matrix2x1(Matrix3x3 m)
        {
            return new Matrix2x1(m.M00, m.M10);
        }
        public static explicit operator Matrix2x2(Matrix3x3 m)
        {
            return new Matrix2x2(m.M00, m.M01, m.M10, m.M11);
        }
        public static explicit operator Matrix2x3(Matrix3x3 m)
        {
            return new Matrix2x3(m.M00, m.M01, m.M02, m.M10, m.M11, m.M12);
        }
        public static explicit operator Matrix3x1(Matrix3x3 m)
        {
            return new Matrix3x1(m.M00, m.M10, m.M20);
        }
        public static explicit operator Matrix3x2(Matrix3x3 m)
        {
            return new Matrix3x2(m.M00, m.M01, m.M10, m.M11, m.M20, m.M21);
        }


        #endregion

        public Matrix3x3 Inverse
        {
            get
            {
                /// 00 01 02
                /// 10 11 12
                /// 20 21 22
                float Min00 = M11 * M22 - M12*M21;
                float Min01 = M10 * M22 - M12 * M20;
                float Min02 = M10 * M21 - M11 * M20;

                float det = Min00 * M00 - Min01 * M01 + Min02 * M02;

                if (det == 0)
                    throw new InvalidOperationException("Singular matrices have not inverse");

                float Min10 = M01 * M22 - M02 * M21;
                float Min11 = M00 * M22 - M02 * M20;
                float Min12 = M00 * M21 - M01 * M20;

                float Min20 = M01 * M12 - M02 * M11;
                float Min21 = M00 * M12 - M02 * M10;
                float Min22 = M00 * M11 - M01 * M10;

                return new Matrix3x3(
                    (+Min00 / det), (-Min10 / det), (+Min20 / det),
                    (-Min01 / det), (+Min11 / det), (-Min21 / det),
                    (+Min02 / det), (-Min12 / det), (+Min22 / det));
            }
        }

        public float Determinant
        {
            get { return GMath.determinant(this); }
        }

        public bool IsSingular { get { return Determinant == 0; } }
    }
    #endregion


    #region Matrix3x4
    public struct Matrix3x4
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public float M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public float M02;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 3
        /// </sumary>
        public float M03;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public float M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public float M11;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 2
        /// </sumary>
        public float M12;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 3
        /// </sumary>
        public float M13;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public float M20;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 1
        /// </sumary>
        public float M21;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 2
        /// </sumary>
        public float M22;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 3
        /// </sumary>
        public float M23;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix3x4(float value)
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

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix3x4(float M00, float M01, float M02, float M03, float M10, float M11, float M12, float M13, float M20, float M21, float M22, float M23)
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
        public static Matrix3x4 operator +(Matrix3x4 m1, Matrix3x4 m2)
        {
            return new Matrix3x4(m1.M00 + (m2.M00), m1.M01 + (m2.M01), m1.M02 + (m2.M02), m1.M03 + (m2.M03), m1.M10 + (m2.M10), m1.M11 + (m2.M11), m1.M12 + (m2.M12), m1.M13 + (m2.M13), m1.M20 + (m2.M20), m1.M21 + (m2.M21), m1.M22 + (m2.M22), m1.M23 + (m2.M23));
        }

        public static Matrix3x4 operator *(Matrix3x4 m1, float a)
        {
            return new Matrix3x4(m1.M00 * (a), m1.M01 * (a), m1.M02 * (a), m1.M03 * (a), m1.M10 * (a), m1.M11 * (a), m1.M12 * (a), m1.M13 * (a), m1.M20 * (a), m1.M21 * (a), m1.M22 * (a), m1.M23 * (a));
        }

        public static Matrix3x4 operator *(float a, Matrix3x4 m) { return m * a; }

        public static Matrix3x4 operator /(Matrix3x4 m1, float a)
        {
            return new Matrix3x4(m1.M00 / (a), m1.M01 / (a), m1.M02 / (a), m1.M03 / (a), m1.M10 / (a), m1.M11 / (a), m1.M12 / (a), m1.M13 / (a), m1.M20 / (a), m1.M21 / (a), m1.M22 / (a), m1.M23 / (a));
        }

        public static Matrix3x4 operator -(Matrix3x4 m1, Matrix3x4 m2)
        {
            return new Matrix3x4(m1.M00 - (m2.M00), m1.M01 - (m2.M01), m1.M02 - (m2.M02), m1.M03 - (m2.M03), m1.M10 - (m2.M10), m1.M11 - (m2.M11), m1.M12 - (m2.M12), m1.M13 - (m2.M13), m1.M20 - (m2.M20), m1.M21 - (m2.M21), m1.M22 - (m2.M22), m1.M23 - (m2.M23));
        }

        public static Matrix3x4 operator /(Matrix3x4 m1, Matrix3x4 m2)
        {
            return new Matrix3x4(m1.M00 / (m2.M00), m1.M01 / (m2.M01), m1.M02 / (m2.M02), m1.M03 / (m2.M03), m1.M10 / (m2.M10), m1.M11 / (m2.M11), m1.M12 / (m2.M12), m1.M13 / (m2.M13), m1.M20 / (m2.M20), m1.M21 / (m2.M21), m1.M22 / (m2.M22), m1.M23 / (m2.M23));
        }

        public static Matrix3x4 operator *(Matrix3x4 m1, Matrix3x4 m2)
        {
            return new Matrix3x4(m1.M00 * (m2.M00), m1.M01 * (m2.M01), m1.M02 * (m2.M02), m1.M03 * (m2.M03), m1.M10 * (m2.M10), m1.M11 * (m2.M11), m1.M12 * (m2.M12), m1.M13 * (m2.M13), m1.M20 * (m2.M20), m1.M21 * (m2.M21), m1.M22 * (m2.M22), m1.M23 * (m2.M23));
        }

        public Matrix4x3 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix3x4 m)
        {
            return new Matrix1x1(m.M00);
        }
        public static explicit operator Matrix1x2(Matrix3x4 m)
        {
            return new Matrix1x2(m.M00, m.M01);
        }
        public static explicit operator Matrix1x3(Matrix3x4 m)
        {
            return new Matrix1x3(m.M00, m.M01, m.M02);
        }
        public static explicit operator Matrix1x4(Matrix3x4 m)
        {
            return new Matrix1x4(m.M00, m.M01, m.M02, m.M03);
        }
        public static explicit operator Matrix2x1(Matrix3x4 m)
        {
            return new Matrix2x1(m.M00, m.M10);
        }
        public static explicit operator Matrix2x2(Matrix3x4 m)
        {
            return new Matrix2x2(m.M00, m.M01, m.M10, m.M11);
        }
        public static explicit operator Matrix2x3(Matrix3x4 m)
        {
            return new Matrix2x3(m.M00, m.M01, m.M02, m.M10, m.M11, m.M12);
        }
        public static explicit operator Matrix2x4(Matrix3x4 m)
        {
            return new Matrix2x4(m.M00, m.M01, m.M02, m.M03, m.M10, m.M11, m.M12, m.M13);
        }
        public static explicit operator Matrix3x1(Matrix3x4 m)
        {
            return new Matrix3x1(m.M00, m.M10, m.M20);
        }
        public static explicit operator Matrix3x2(Matrix3x4 m)
        {
            return new Matrix3x2(m.M00, m.M01, m.M10, m.M11, m.M20, m.M21);
        }
        public static explicit operator Matrix3x3(Matrix3x4 m)
        {
            return new Matrix3x3(m.M00, m.M01, m.M02, m.M10, m.M11, m.M12, m.M20, m.M21, m.M22);
        }


        #endregion

    }
    #endregion


    #region Matrix4x1
    public struct Matrix4x1
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public float M10;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public float M20;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 0
        /// </sumary>
        public float M30;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix4x1(float value)
        {
            M00 = value;
            M10 = value;
            M20 = value;
            M30 = value;
        }

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix4x1(float M00, float M10, float M20, float M30)
        {
            this.M00 = M00;
            this.M10 = M10;
            this.M20 = M20;
            this.M30 = M30;
        }
        #endregion

        #region Operators
        public static Matrix4x1 operator +(Matrix4x1 m1, Matrix4x1 m2)
        {
            return new Matrix4x1(m1.M00 + (m2.M00), m1.M10 + (m2.M10), m1.M20 + (m2.M20), m1.M30 + (m2.M30));
        }

        public static Matrix4x1 operator *(Matrix4x1 m1, float a)
        {
            return new Matrix4x1(m1.M00 * (a), m1.M10 * (a), m1.M20 * (a), m1.M30 * (a));
        }

        public static Matrix4x1 operator *(float a, Matrix4x1 m) { return m * a; }

        public static Matrix4x1 operator /(Matrix4x1 m1, float a)
        {
            return new Matrix4x1(m1.M00 / (a), m1.M10 / (a), m1.M20 / (a), m1.M30 / (a));
        }

        public static Matrix4x1 operator -(Matrix4x1 m1, Matrix4x1 m2)
        {
            return new Matrix4x1(m1.M00 - (m2.M00), m1.M10 - (m2.M10), m1.M20 - (m2.M20), m1.M30 - (m2.M30));
        }

        public static Matrix4x1 operator /(Matrix4x1 m1, Matrix4x1 m2)
        {
            return new Matrix4x1(m1.M00 / (m2.M00), m1.M10 / (m2.M10), m1.M20 / (m2.M20), m1.M30 / (m2.M30));
        }

        public static Matrix4x1 operator *(Matrix4x1 m1, Matrix4x1 m2)
        {
            return new Matrix4x1(m1.M00 * (m2.M00), m1.M10 * (m2.M10), m1.M20 * (m2.M20), m1.M30 * (m2.M30));
        }

        public Matrix1x4 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix4x1 m)
        {
            return new Matrix1x1(m.M00);
        }
        public static explicit operator Matrix2x1(Matrix4x1 m)
        {
            return new Matrix2x1(m.M00, m.M10);
        }
        public static explicit operator Matrix3x1(Matrix4x1 m)
        {
            return new Matrix3x1(m.M00, m.M10, m.M20);
        }

        public static explicit operator Vector4(Matrix4x1 m)
        {
            return new Vector4(m.M00, m.M10, m.M20, m.M30);
        }

        public static explicit operator Matrix4x1(Vector4 v)
        {
            return new Matrix4x1(v.X, v.Y, v.Z, v.W);
        }

        #endregion

    }
    #endregion


    #region Matrix4x2
    public struct Matrix4x2
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public float M01;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public float M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public float M11;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public float M20;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 1
        /// </sumary>
        public float M21;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 0
        /// </sumary>
        public float M30;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 1
        /// </sumary>
        public float M31;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix4x2(float value)
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

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix4x2(float M00, float M01, float M10, float M11, float M20, float M21, float M30, float M31)
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
        public static Matrix4x2 operator +(Matrix4x2 m1, Matrix4x2 m2)
        {
            return new Matrix4x2(m1.M00 + (m2.M00), m1.M01 + (m2.M01), m1.M10 + (m2.M10), m1.M11 + (m2.M11), m1.M20 + (m2.M20), m1.M21 + (m2.M21), m1.M30 + (m2.M30), m1.M31 + (m2.M31));
        }

        public static Matrix4x2 operator *(Matrix4x2 m1, float a)
        {
            return new Matrix4x2(m1.M00 * (a), m1.M01 * (a), m1.M10 * (a), m1.M11 * (a), m1.M20 * (a), m1.M21 * (a), m1.M30 * (a), m1.M31 * (a));
        }

        public static Matrix4x2 operator *(float a, Matrix4x2 m) { return m * a; }

        public static Matrix4x2 operator /(Matrix4x2 m1, float a)
        {
            return new Matrix4x2(m1.M00 / (a), m1.M01 / (a), m1.M10 / (a), m1.M11 / (a), m1.M20 / (a), m1.M21 / (a), m1.M30 / (a), m1.M31 / (a));
        }

        public static Matrix4x2 operator -(Matrix4x2 m1, Matrix4x2 m2)
        {
            return new Matrix4x2(m1.M00 - (m2.M00), m1.M01 - (m2.M01), m1.M10 - (m2.M10), m1.M11 - (m2.M11), m1.M20 - (m2.M20), m1.M21 - (m2.M21), m1.M30 - (m2.M30), m1.M31 - (m2.M31));
        }

        public static Matrix4x2 operator /(Matrix4x2 m1, Matrix4x2 m2)
        {
            return new Matrix4x2(m1.M00 / (m2.M00), m1.M01 / (m2.M01), m1.M10 / (m2.M10), m1.M11 / (m2.M11), m1.M20 / (m2.M20), m1.M21 / (m2.M21), m1.M30 / (m2.M30), m1.M31 / (m2.M31));
        }

        public static Matrix4x2 operator *(Matrix4x2 m1, Matrix4x2 m2)
        {
            return new Matrix4x2(m1.M00 * (m2.M00), m1.M01 * (m2.M01), m1.M10 * (m2.M10), m1.M11 * (m2.M11), m1.M20 * (m2.M20), m1.M21 * (m2.M21), m1.M30 * (m2.M30), m1.M31 * (m2.M31));
        }

        public Matrix2x4 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix4x2 m)
        {
            return new Matrix1x1(m.M00);
        }
        public static explicit operator Matrix1x2(Matrix4x2 m)
        {
            return new Matrix1x2(m.M00, m.M01);
        }
        public static explicit operator Matrix2x1(Matrix4x2 m)
        {
            return new Matrix2x1(m.M00, m.M10);
        }
        public static explicit operator Matrix2x2(Matrix4x2 m)
        {
            return new Matrix2x2(m.M00, m.M01, m.M10, m.M11);
        }
        public static explicit operator Matrix3x1(Matrix4x2 m)
        {
            return new Matrix3x1(m.M00, m.M10, m.M20);
        }
        public static explicit operator Matrix3x2(Matrix4x2 m)
        {
            return new Matrix3x2(m.M00, m.M01, m.M10, m.M11, m.M20, m.M21);
        }
        public static explicit operator Matrix4x1(Matrix4x2 m)
        {
            return new Matrix4x1(m.M00, m.M10, m.M20, m.M30);
        }


        #endregion

    }
    #endregion


    #region Matrix4x3
    public struct Matrix4x3
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public float M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public float M02;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public float M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public float M11;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 2
        /// </sumary>
        public float M12;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public float M20;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 1
        /// </sumary>
        public float M21;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 2
        /// </sumary>
        public float M22;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 0
        /// </sumary>
        public float M30;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 1
        /// </sumary>
        public float M31;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 2
        /// </sumary>
        public float M32;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix4x3(float value)
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

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix4x3(float M00, float M01, float M02, float M10, float M11, float M12, float M20, float M21, float M22, float M30, float M31, float M32)
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
        public static Matrix4x3 operator +(Matrix4x3 m1, Matrix4x3 m2)
        {
            return new Matrix4x3(m1.M00 + (m2.M00), m1.M01 + (m2.M01), m1.M02 + (m2.M02), m1.M10 + (m2.M10), m1.M11 + (m2.M11), m1.M12 + (m2.M12), m1.M20 + (m2.M20), m1.M21 + (m2.M21), m1.M22 + (m2.M22), m1.M30 + (m2.M30), m1.M31 + (m2.M31), m1.M32 + (m2.M32));
        }

        public static Matrix4x3 operator *(Matrix4x3 m1, float a)
        {
            return new Matrix4x3(m1.M00 * (a), m1.M01 * (a), m1.M02 * (a), m1.M10 * (a), m1.M11 * (a), m1.M12 * (a), m1.M20 * (a), m1.M21 * (a), m1.M22 * (a), m1.M30 * (a), m1.M31 * (a), m1.M32 * (a));
        }

        public static Matrix4x3 operator *(float a, Matrix4x3 m) { return m * a; }

        public static Matrix4x3 operator /(Matrix4x3 m1, float a)
        {
            return new Matrix4x3(m1.M00 / (a), m1.M01 / (a), m1.M02 / (a), m1.M10 / (a), m1.M11 / (a), m1.M12 / (a), m1.M20 / (a), m1.M21 / (a), m1.M22 / (a), m1.M30 / (a), m1.M31 / (a), m1.M32 / (a));
        }

        public static Matrix4x3 operator -(Matrix4x3 m1, Matrix4x3 m2)
        {
            return new Matrix4x3(m1.M00 - (m2.M00), m1.M01 - (m2.M01), m1.M02 - (m2.M02), m1.M10 - (m2.M10), m1.M11 - (m2.M11), m1.M12 - (m2.M12), m1.M20 - (m2.M20), m1.M21 - (m2.M21), m1.M22 - (m2.M22), m1.M30 - (m2.M30), m1.M31 - (m2.M31), m1.M32 - (m2.M32));
        }

        public static Matrix4x3 operator /(Matrix4x3 m1, Matrix4x3 m2)
        {
            return new Matrix4x3(m1.M00 / (m2.M00), m1.M01 / (m2.M01), m1.M02 / (m2.M02), m1.M10 / (m2.M10), m1.M11 / (m2.M11), m1.M12 / (m2.M12), m1.M20 / (m2.M20), m1.M21 / (m2.M21), m1.M22 / (m2.M22), m1.M30 / (m2.M30), m1.M31 / (m2.M31), m1.M32 / (m2.M32));
        }

        public static Matrix4x3 operator *(Matrix4x3 m1, Matrix4x3 m2)
        {
            return new Matrix4x3(m1.M00 * (m2.M00), m1.M01 * (m2.M01), m1.M02 * (m2.M02), m1.M10 * (m2.M10), m1.M11 * (m2.M11), m1.M12 * (m2.M12), m1.M20 * (m2.M20), m1.M21 * (m2.M21), m1.M22 * (m2.M22), m1.M30 * (m2.M30), m1.M31 * (m2.M31), m1.M32 * (m2.M32));
        }

        public Matrix3x4 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix4x3 m)
        {
            return new Matrix1x1(m.M00);
        }
        public static explicit operator Matrix1x2(Matrix4x3 m)
        {
            return new Matrix1x2(m.M00, m.M01);
        }
        public static explicit operator Matrix1x3(Matrix4x3 m)
        {
            return new Matrix1x3(m.M00, m.M01, m.M02);
        }
        public static explicit operator Matrix2x1(Matrix4x3 m)
        {
            return new Matrix2x1(m.M00, m.M10);
        }
        public static explicit operator Matrix2x2(Matrix4x3 m)
        {
            return new Matrix2x2(m.M00, m.M01, m.M10, m.M11);
        }
        public static explicit operator Matrix2x3(Matrix4x3 m)
        {
            return new Matrix2x3(m.M00, m.M01, m.M02, m.M10, m.M11, m.M12);
        }
        public static explicit operator Matrix3x1(Matrix4x3 m)
        {
            return new Matrix3x1(m.M00, m.M10, m.M20);
        }
        public static explicit operator Matrix3x2(Matrix4x3 m)
        {
            return new Matrix3x2(m.M00, m.M01, m.M10, m.M11, m.M20, m.M21);
        }
        public static explicit operator Matrix3x3(Matrix4x3 m)
        {
            return new Matrix3x3(m.M00, m.M01, m.M02, m.M10, m.M11, m.M12, m.M20, m.M21, m.M22);
        }
        public static explicit operator Matrix4x1(Matrix4x3 m)
        {
            return new Matrix4x1(m.M00, m.M10, m.M20, m.M30);
        }
        public static explicit operator Matrix4x2(Matrix4x3 m)
        {
            return new Matrix4x2(m.M00, m.M01, m.M10, m.M11, m.M20, m.M21, m.M30, m.M31);
        }


        #endregion

    }
    #endregion


    #region Matrix4x4
    public struct Matrix4x4
    {
        #region Instances
        /// <sumary>
        /// Gets or sets the element at row 0 and column 0
        /// </sumary>
        public float M00;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 1
        /// </sumary>
        public float M01;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 2
        /// </sumary>
        public float M02;
        /// <sumary>
        /// Gets or sets the element at row 0 and column 3
        /// </sumary>
        public float M03;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 0
        /// </sumary>
        public float M10;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 1
        /// </sumary>
        public float M11;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 2
        /// </sumary>
        public float M12;
        /// <sumary>
        /// Gets or sets the element at row 1 and column 3
        /// </sumary>
        public float M13;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 0
        /// </sumary>
        public float M20;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 1
        /// </sumary>
        public float M21;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 2
        /// </sumary>
        public float M22;
        /// <sumary>
        /// Gets or sets the element at row 2 and column 3
        /// </sumary>
        public float M23;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 0
        /// </sumary>
        public float M30;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 1
        /// </sumary>
        public float M31;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 2
        /// </sumary>
        public float M32;
        /// <sumary>
        /// Gets or sets the element at row 3 and column 3
        /// </sumary>
        public float M33;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the matrix with the same value for all its components.
        /// </summary>
        public Matrix4x4(float value)
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

        /// <summary>
        /// Initializes the matrix with the specified values.
        /// </summary>
        public Matrix4x4(float M00, float M01, float M02, float M03, float M10, float M11, float M12, float M13, float M20, float M21, float M22, float M23, float M30, float M31, float M32, float M33)
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
        public static Matrix4x4 operator +(Matrix4x4 m1, Matrix4x4 m2)
        {
            return new Matrix4x4(m1.M00 + (m2.M00), m1.M01 + (m2.M01), m1.M02 + (m2.M02), m1.M03 + (m2.M03), m1.M10 + (m2.M10), m1.M11 + (m2.M11), m1.M12 + (m2.M12), m1.M13 + (m2.M13), m1.M20 + (m2.M20), m1.M21 + (m2.M21), m1.M22 + (m2.M22), m1.M23 + (m2.M23), m1.M30 + (m2.M30), m1.M31 + (m2.M31), m1.M32 + (m2.M32), m1.M33 + (m2.M33));
        }

        public static Matrix4x4 operator *(Matrix4x4 m1, float a)
        {
            return new Matrix4x4(m1.M00 * (a), m1.M01 * (a), m1.M02 * (a), m1.M03 * (a), m1.M10 * (a), m1.M11 * (a), m1.M12 * (a), m1.M13 * (a), m1.M20 * (a), m1.M21 * (a), m1.M22 * (a), m1.M23 * (a), m1.M30 * (a), m1.M31 * (a), m1.M32 * (a), m1.M33 * (a));
        }

        public static Matrix4x4 operator *(float a, Matrix4x4 m) { return m * a; }

        public static Matrix4x4 operator /(Matrix4x4 m1, float a)
        {
            return new Matrix4x4(m1.M00 / (a), m1.M01 / (a), m1.M02 / (a), m1.M03 / (a), m1.M10 / (a), m1.M11 / (a), m1.M12 / (a), m1.M13 / (a), m1.M20 / (a), m1.M21 / (a), m1.M22 / (a), m1.M23 / (a), m1.M30 / (a), m1.M31 / (a), m1.M32 / (a), m1.M33 / (a));
        }

        public static Matrix4x4 operator -(Matrix4x4 m1, Matrix4x4 m2)
        {
            return new Matrix4x4(m1.M00 - (m2.M00), m1.M01 - (m2.M01), m1.M02 - (m2.M02), m1.M03 - (m2.M03), m1.M10 - (m2.M10), m1.M11 - (m2.M11), m1.M12 - (m2.M12), m1.M13 - (m2.M13), m1.M20 - (m2.M20), m1.M21 - (m2.M21), m1.M22 - (m2.M22), m1.M23 - (m2.M23), m1.M30 - (m2.M30), m1.M31 - (m2.M31), m1.M32 - (m2.M32), m1.M33 - (m2.M33));
        }

        public static Matrix4x4 operator /(Matrix4x4 m1, Matrix4x4 m2)
        {
            return new Matrix4x4(m1.M00 / (m2.M00), m1.M01 / (m2.M01), m1.M02 / (m2.M02), m1.M03 / (m2.M03), m1.M10 / (m2.M10), m1.M11 / (m2.M11), m1.M12 / (m2.M12), m1.M13 / (m2.M13), m1.M20 / (m2.M20), m1.M21 / (m2.M21), m1.M22 / (m2.M22), m1.M23 / (m2.M23), m1.M30 / (m2.M30), m1.M31 / (m2.M31), m1.M32 / (m2.M32), m1.M33 / (m2.M33));
        }

        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2)
        {
            return new Matrix4x4(m1.M00 * (m2.M00), m1.M01 * (m2.M01), m1.M02 * (m2.M02), m1.M03 * (m2.M03), m1.M10 * (m2.M10), m1.M11 * (m2.M11), m1.M12 * (m2.M12), m1.M13 * (m2.M13), m1.M20 * (m2.M20), m1.M21 * (m2.M21), m1.M22 * (m2.M22), m1.M23 * (m2.M23), m1.M30 * (m2.M30), m1.M31 * (m2.M31), m1.M32 * (m2.M32), m1.M33 * (m2.M33));
        }

        public Matrix4x4 Transpose
        {
            get
            {
                return GMath.transpose(this);
            }
        }

        #endregion

        #region Conversions

        public static explicit operator Matrix1x1(Matrix4x4 m)
        {
            return new Matrix1x1(m.M00);
        }
        public static explicit operator Matrix1x2(Matrix4x4 m)
        {
            return new Matrix1x2(m.M00, m.M01);
        }
        public static explicit operator Matrix1x3(Matrix4x4 m)
        {
            return new Matrix1x3(m.M00, m.M01, m.M02);
        }
        public static explicit operator Matrix1x4(Matrix4x4 m)
        {
            return new Matrix1x4(m.M00, m.M01, m.M02, m.M03);
        }
        public static explicit operator Matrix2x1(Matrix4x4 m)
        {
            return new Matrix2x1(m.M00, m.M10);
        }
        public static explicit operator Matrix2x2(Matrix4x4 m)
        {
            return new Matrix2x2(m.M00, m.M01, m.M10, m.M11);
        }
        public static explicit operator Matrix2x3(Matrix4x4 m)
        {
            return new Matrix2x3(m.M00, m.M01, m.M02, m.M10, m.M11, m.M12);
        }
        public static explicit operator Matrix2x4(Matrix4x4 m)
        {
            return new Matrix2x4(m.M00, m.M01, m.M02, m.M03, m.M10, m.M11, m.M12, m.M13);
        }
        public static explicit operator Matrix3x1(Matrix4x4 m)
        {
            return new Matrix3x1(m.M00, m.M10, m.M20);
        }
        public static explicit operator Matrix3x2(Matrix4x4 m)
        {
            return new Matrix3x2(m.M00, m.M01, m.M10, m.M11, m.M20, m.M21);
        }
        public static explicit operator Matrix3x3(Matrix4x4 m)
        {
            return new Matrix3x3(m.M00, m.M01, m.M02, m.M10, m.M11, m.M12, m.M20, m.M21, m.M22);
        }
        public static explicit operator Matrix3x4(Matrix4x4 m)
        {
            return new Matrix3x4(m.M00, m.M01, m.M02, m.M03, m.M10, m.M11, m.M12, m.M13, m.M20, m.M21, m.M22, m.M23);
        }
        public static explicit operator Matrix4x1(Matrix4x4 m)
        {
            return new Matrix4x1(m.M00, m.M10, m.M20, m.M30);
        }
        public static explicit operator Matrix4x2(Matrix4x4 m)
        {
            return new Matrix4x2(m.M00, m.M01, m.M10, m.M11, m.M20, m.M21, m.M30, m.M31);
        }
        public static explicit operator Matrix4x3(Matrix4x4 m)
        {
            return new Matrix4x3(m.M00, m.M01, m.M02, m.M10, m.M11, m.M12, m.M20, m.M21, m.M22, m.M30, m.M31, m.M32);
        }


        #endregion

        public Matrix4x4 Inverse
        {
            get {
                float Min00 = M11 * M22 * M33 + M12 * M23 * M31 + M13 * M21 * M32 - M11 * M23 * M32 - M12 * M21 * M33 - M13 * M22 * M31;
                float Min01 = M10 * M22 * M33 + M12 * M23 * M30 + M13 * M20 * M32 - M10 * M23 * M32 - M12 * M20 * M33 - M13 * M22 * M30;
                float Min02 = M10 * M21 * M33 + M11 * M23 * M30 + M13 * M20 * M31 - M10 * M23 * M31 - M11 * M20 * M33 - M13 * M21 * M30;
                float Min03 = M10 * M21 * M32 + M11 * M22 * M30 + M12 * M20 * M31 - M10 * M22 * M31 - M11 * M20 * M32 - M12 * M21 * M30;

                float det = Min00 * M00 - Min01 * M01 + Min02 * M02 - Min03 * M03;

                if (det == 0)
                    throw new InvalidOperationException("Singular matrices have not inverse");

                float Min10 = M01 * M22 * M33 + M02 * M23 * M31 + M03 * M21 * M32 - M01 * M23 * M32 - M02 * M21 * M33 - M03 * M22 * M31;
                float Min11 = M00 * M22 * M33 + M02 * M23 * M30 + M03 * M20 * M32 - M00 * M23 * M32 - M02 * M20 * M33 - M03 * M22 * M30;
                float Min12 = M00 * M21 * M33 + M01 * M23 * M30 + M03 * M20 * M31 - M00 * M23 * M31 - M01 * M20 * M33 - M03 * M21 * M30;
                float Min13 = M00 * M21 * M32 + M01 * M22 * M30 + M02 * M20 * M31 - M00 * M22 * M31 - M01 * M20 * M32 - M02 * M21 * M30;

                float Min20 = M01 * M12 * M33 + M02 * M13 * M31 + M03 * M11 * M32 - M01 * M13 * M32 - M02 * M11 * M33 - M03 * M12 * M31;
                float Min21 = M00 * M12 * M33 + M02 * M13 * M30 + M03 * M10 * M32 - M00 * M13 * M32 - M02 * M10 * M33 - M03 * M12 * M30;
                float Min22 = M00 * M11 * M33 + M01 * M13 * M30 + M03 * M10 * M31 - M00 * M13 * M31 - M01 * M10 * M33 - M03 * M11 * M30;
                float Min23 = M00 * M11 * M32 + M01 * M12 * M30 + M02 * M10 * M31 - M00 * M12 * M31 - M01 * M10 * M32 - M02 * M11 * M30;

                float Min30 = M01 * M12 * M23 + M02 * M13 * M21 + M03 * M11 * M22 - M01 * M13 * M22 - M02 * M11 * M23 - M03 * M12 * M21;
                float Min31 = M00 * M12 * M23 + M02 * M13 * M20 + M03 * M10 * M22 - M00 * M13 * M22 - M02 * M10 * M23 - M03 * M12 * M20;
                float Min32 = M00 * M11 * M23 + M01 * M13 * M20 + M03 * M10 * M21 - M00 * M13 * M21 - M01 * M10 * M23 - M03 * M11 * M20;
                float Min33 = M00 * M11 * M22 + M01 * M12 * M20 + M02 * M10 * M21 - M00 * M12 * M21 - M01 * M10 * M22 - M02 * M11 * M20;

                return new Matrix4x4(
                    (+Min00 / det), (-Min10 / det), (+Min20 / det), (-Min30 / det),
                    (-Min01 / det), (+Min11 / det), (-Min21 / det), (+Min31 / det),
                    (+Min02 / det), (-Min12 / det), (+Min22 / det), (-Min32 / det),
                    (-Min03 / det), (+Min13 / det), (-Min23 / det), (+Min33 / det));
            }
        }

        public float Determinant
        {
            get { return GMath.determinant(this); }
        }

        public bool IsSingular { get { return Determinant == 0; } }

    }
    #endregion


}
