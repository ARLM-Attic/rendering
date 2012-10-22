using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Maths.Scalaring;

namespace System.Maths
{
    public static class GMath
    {
        public const float Pi = (float)Math.PI;
        public const float PiOver4 = Pi/4;
        
        #region abs

        public static float abs(float x)
        {
            return (float)Math.Abs(x);
        }

        /// <summary>
        /// Performs the abs function to the specified Vector1 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Vector1 abs(Vector1 v)
        {
            return new Vector1((float)Math.Abs(v.X));
        }

        /// <summary>
        /// Performs the abs function to the specified Vector2 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Vector2 abs(Vector2 v)
        {
            return new Vector2((float)Math.Abs(v.X), (float)Math.Abs(v.Y));
        }

        /// <summary>
        /// Performs the abs function to the specified Vector3 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Vector3 abs(Vector3 v)
        {
            return new Vector3((float)Math.Abs(v.X), (float)Math.Abs(v.Y), (float)Math.Abs(v.Z));
        }

        /// <summary>
        /// Performs the abs function to the specified Vector4 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Vector4 abs(Vector4 v)
        {
            return new Vector4((float)Math.Abs(v.X), (float)Math.Abs(v.Y), (float)Math.Abs(v.Z), (float)Math.Abs(v.W));
        }


        /// <summary>
        /// Performs the abs function to the specified Matrix1x1 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix1x1 abs(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Abs(v.M00));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix1x2 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix1x2 abs(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Abs(v.M00), (float)Math.Abs(v.M01));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix1x3 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix1x3 abs(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Abs(v.M00), (float)Math.Abs(v.M01), (float)Math.Abs(v.M02));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix1x4 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix1x4 abs(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Abs(v.M00), (float)Math.Abs(v.M01), (float)Math.Abs(v.M02), (float)Math.Abs(v.M03));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix2x1 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix2x1 abs(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Abs(v.M00), (float)Math.Abs(v.M10));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix2x2 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix2x2 abs(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Abs(v.M00), (float)Math.Abs(v.M01), (float)Math.Abs(v.M10), (float)Math.Abs(v.M11));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix2x3 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix2x3 abs(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Abs(v.M00), (float)Math.Abs(v.M01), (float)Math.Abs(v.M02), (float)Math.Abs(v.M10), (float)Math.Abs(v.M11), (float)Math.Abs(v.M12));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix2x4 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix2x4 abs(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Abs(v.M00), (float)Math.Abs(v.M01), (float)Math.Abs(v.M02), (float)Math.Abs(v.M03), (float)Math.Abs(v.M10), (float)Math.Abs(v.M11), (float)Math.Abs(v.M12), (float)Math.Abs(v.M13));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix3x1 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix3x1 abs(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Abs(v.M00), (float)Math.Abs(v.M10), (float)Math.Abs(v.M20));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix3x2 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix3x2 abs(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Abs(v.M00), (float)Math.Abs(v.M01), (float)Math.Abs(v.M10), (float)Math.Abs(v.M11), (float)Math.Abs(v.M20), (float)Math.Abs(v.M21));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix3x3 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix3x3 abs(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Abs(v.M00), (float)Math.Abs(v.M01), (float)Math.Abs(v.M02), (float)Math.Abs(v.M10), (float)Math.Abs(v.M11), (float)Math.Abs(v.M12), (float)Math.Abs(v.M20), (float)Math.Abs(v.M21), (float)Math.Abs(v.M22));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix3x4 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix3x4 abs(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Abs(v.M00), (float)Math.Abs(v.M01), (float)Math.Abs(v.M02), (float)Math.Abs(v.M03), (float)Math.Abs(v.M10), (float)Math.Abs(v.M11), (float)Math.Abs(v.M12), (float)Math.Abs(v.M13), (float)Math.Abs(v.M20), (float)Math.Abs(v.M21), (float)Math.Abs(v.M22), (float)Math.Abs(v.M23));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix4x1 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix4x1 abs(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Abs(v.M00), (float)Math.Abs(v.M10), (float)Math.Abs(v.M20), (float)Math.Abs(v.M30));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix4x2 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix4x2 abs(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Abs(v.M00), (float)Math.Abs(v.M01), (float)Math.Abs(v.M10), (float)Math.Abs(v.M11), (float)Math.Abs(v.M20), (float)Math.Abs(v.M21), (float)Math.Abs(v.M30), (float)Math.Abs(v.M31));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix4x3 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix4x3 abs(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Abs(v.M00), (float)Math.Abs(v.M01), (float)Math.Abs(v.M02), (float)Math.Abs(v.M10), (float)Math.Abs(v.M11), (float)Math.Abs(v.M12), (float)Math.Abs(v.M20), (float)Math.Abs(v.M21), (float)Math.Abs(v.M22), (float)Math.Abs(v.M30), (float)Math.Abs(v.M31), (float)Math.Abs(v.M32));
        }

        /// <summary>
        /// Performs the abs function to the specified Matrix4x4 object.
        /// Gets the absolute value for each component.
        /// </summary>
        public static Matrix4x4 abs(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Abs(v.M00), (float)Math.Abs(v.M01), (float)Math.Abs(v.M02), (float)Math.Abs(v.M03), (float)Math.Abs(v.M10), (float)Math.Abs(v.M11), (float)Math.Abs(v.M12), (float)Math.Abs(v.M13), (float)Math.Abs(v.M20), (float)Math.Abs(v.M21), (float)Math.Abs(v.M22), (float)Math.Abs(v.M23), (float)Math.Abs(v.M30), (float)Math.Abs(v.M31), (float)Math.Abs(v.M32), (float)Math.Abs(v.M33));
        }

        #endregion
        #region acos

        /// <summary>
        /// Performs the acos function to the specified Vector1 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Vector1 acos(Vector1 v)
        {
            return new Vector1((float)Math.Acos(v.X));
        }

        /// <summary>
        /// Performs the acos function to the specified Vector2 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Vector2 acos(Vector2 v)
        {
            return new Vector2((float)Math.Acos(v.X), (float)Math.Acos(v.Y));
        }

        /// <summary>
        /// Performs the acos function to the specified Vector3 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Vector3 acos(Vector3 v)
        {
            return new Vector3((float)Math.Acos(v.X), (float)Math.Acos(v.Y), (float)Math.Acos(v.Z));
        }

        /// <summary>
        /// Performs the acos function to the specified Vector4 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Vector4 acos(Vector4 v)
        {
            return new Vector4((float)Math.Acos(v.X), (float)Math.Acos(v.Y), (float)Math.Acos(v.Z), (float)Math.Acos(v.W));
        }


        /// <summary>
        /// Performs the acos function to the specified Matrix1x1 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix1x1 acos(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Acos(v.M00));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix1x2 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix1x2 acos(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Acos(v.M00), (float)Math.Acos(v.M01));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix1x3 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix1x3 acos(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Acos(v.M00), (float)Math.Acos(v.M01), (float)Math.Acos(v.M02));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix1x4 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix1x4 acos(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Acos(v.M00), (float)Math.Acos(v.M01), (float)Math.Acos(v.M02), (float)Math.Acos(v.M03));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix2x1 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix2x1 acos(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Acos(v.M00), (float)Math.Acos(v.M10));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix2x2 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix2x2 acos(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Acos(v.M00), (float)Math.Acos(v.M01), (float)Math.Acos(v.M10), (float)Math.Acos(v.M11));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix2x3 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix2x3 acos(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Acos(v.M00), (float)Math.Acos(v.M01), (float)Math.Acos(v.M02), (float)Math.Acos(v.M10), (float)Math.Acos(v.M11), (float)Math.Acos(v.M12));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix2x4 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix2x4 acos(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Acos(v.M00), (float)Math.Acos(v.M01), (float)Math.Acos(v.M02), (float)Math.Acos(v.M03), (float)Math.Acos(v.M10), (float)Math.Acos(v.M11), (float)Math.Acos(v.M12), (float)Math.Acos(v.M13));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix3x1 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix3x1 acos(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Acos(v.M00), (float)Math.Acos(v.M10), (float)Math.Acos(v.M20));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix3x2 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix3x2 acos(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Acos(v.M00), (float)Math.Acos(v.M01), (float)Math.Acos(v.M10), (float)Math.Acos(v.M11), (float)Math.Acos(v.M20), (float)Math.Acos(v.M21));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix3x3 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix3x3 acos(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Acos(v.M00), (float)Math.Acos(v.M01), (float)Math.Acos(v.M02), (float)Math.Acos(v.M10), (float)Math.Acos(v.M11), (float)Math.Acos(v.M12), (float)Math.Acos(v.M20), (float)Math.Acos(v.M21), (float)Math.Acos(v.M22));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix3x4 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix3x4 acos(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Acos(v.M00), (float)Math.Acos(v.M01), (float)Math.Acos(v.M02), (float)Math.Acos(v.M03), (float)Math.Acos(v.M10), (float)Math.Acos(v.M11), (float)Math.Acos(v.M12), (float)Math.Acos(v.M13), (float)Math.Acos(v.M20), (float)Math.Acos(v.M21), (float)Math.Acos(v.M22), (float)Math.Acos(v.M23));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix4x1 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix4x1 acos(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Acos(v.M00), (float)Math.Acos(v.M10), (float)Math.Acos(v.M20), (float)Math.Acos(v.M30));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix4x2 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix4x2 acos(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Acos(v.M00), (float)Math.Acos(v.M01), (float)Math.Acos(v.M10), (float)Math.Acos(v.M11), (float)Math.Acos(v.M20), (float)Math.Acos(v.M21), (float)Math.Acos(v.M30), (float)Math.Acos(v.M31));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix4x3 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix4x3 acos(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Acos(v.M00), (float)Math.Acos(v.M01), (float)Math.Acos(v.M02), (float)Math.Acos(v.M10), (float)Math.Acos(v.M11), (float)Math.Acos(v.M12), (float)Math.Acos(v.M20), (float)Math.Acos(v.M21), (float)Math.Acos(v.M22), (float)Math.Acos(v.M30), (float)Math.Acos(v.M31), (float)Math.Acos(v.M32));
        }

        /// <summary>
        /// Performs the acos function to the specified Matrix4x4 object.
        /// Gets the arc cosine of each component.
        /// </summary>
        public static Matrix4x4 acos(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Acos(v.M00), (float)Math.Acos(v.M01), (float)Math.Acos(v.M02), (float)Math.Acos(v.M03), (float)Math.Acos(v.M10), (float)Math.Acos(v.M11), (float)Math.Acos(v.M12), (float)Math.Acos(v.M13), (float)Math.Acos(v.M20), (float)Math.Acos(v.M21), (float)Math.Acos(v.M22), (float)Math.Acos(v.M23), (float)Math.Acos(v.M30), (float)Math.Acos(v.M31), (float)Math.Acos(v.M32), (float)Math.Acos(v.M33));
        }

        #endregion
        #region all

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Vector1 v)
        {
            {
                return v.X != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Vector2 v)
        {
            {
                return v.X != 0 && v.Y != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Vector3 v)
        {
            {
                return v.X != 0 && v.Y != 0 && v.Z != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Vector4 v)
        {
            {
                return v.X != 0 && v.Y != 0 && v.Z != 0 && v.W != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix1x1 v)
        {
            {
                return v.M00 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix1x2 v)
        {
            {
                return v.M00 != 0 && v.M01 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix1x3 v)
        {
            {
                return v.M00 != 0 && v.M01 != 0 && v.M02 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix1x4 v)
        {
            {
                return v.M00 != 0 && v.M01 != 0 && v.M02 != 0 && v.M03 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix2x1 v)
        {
            {
                return v.M00 != 0 && v.M10 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix2x2 v)
        {
            {
                return v.M00 != 0 && v.M01 != 0 && v.M10 != 0 && v.M11 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix2x3 v)
        {
            {
                return v.M00 != 0 && v.M01 != 0 && v.M02 != 0 && v.M10 != 0 && v.M11 != 0 && v.M12 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix2x4 v)
        {
            {
                return v.M00 != 0 && v.M01 != 0 && v.M02 != 0 && v.M03 != 0 && v.M10 != 0 && v.M11 != 0 && v.M12 != 0 && v.M13 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix3x1 v)
        {
            {
                return v.M00 != 0 && v.M10 != 0 && v.M20 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix3x2 v)
        {
            {
                return v.M00 != 0 && v.M01 != 0 && v.M10 != 0 && v.M11 != 0 && v.M20 != 0 && v.M21 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix3x3 v)
        {
            {
                return v.M00 != 0 && v.M01 != 0 && v.M02 != 0 && v.M10 != 0 && v.M11 != 0 && v.M12 != 0 && v.M20 != 0 && v.M21 != 0 && v.M22 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix3x4 v)
        {
            {
                return v.M00 != 0 && v.M01 != 0 && v.M02 != 0 && v.M03 != 0 && v.M10 != 0 && v.M11 != 0 && v.M12 != 0 && v.M13 != 0 && v.M20 != 0 && v.M21 != 0 && v.M22 != 0 && v.M23 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix4x1 v)
        {
            {
                return v.M00 != 0 && v.M10 != 0 && v.M20 != 0 && v.M30 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix4x2 v)
        {
            {
                return v.M00 != 0 && v.M01 != 0 && v.M10 != 0 && v.M11 != 0 && v.M20 != 0 && v.M21 != 0 && v.M30 != 0 && v.M31 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix4x3 v)
        {
            {
                return v.M00 != 0 && v.M01 != 0 && v.M02 != 0 && v.M10 != 0 && v.M11 != 0 && v.M12 != 0 && v.M20 != 0 && v.M21 != 0 && v.M22 != 0 && v.M30 != 0 && v.M31 != 0 && v.M32 != 0;
            }
        }

        /// <summary>
        /// Tests if all components of v are nonzero.
        /// </summary>
        public static bool all(Matrix4x4 v)
        {
            {
                return v.M00 != 0 && v.M01 != 0 && v.M02 != 0 && v.M03 != 0 && v.M10 != 0 && v.M11 != 0 && v.M12 != 0 && v.M13 != 0 && v.M20 != 0 && v.M21 != 0 && v.M22 != 0 && v.M23 != 0 && v.M30 != 0 && v.M31 != 0 && v.M32 != 0 && v.M33 != 0;
            }
        }
        #endregion
        #region any

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Vector1 v)
        {
            {
                return v.X != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Vector2 v)
        {
            {
                return v.X != 0 || v.Y != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Vector3 v)
        {
            {
                return v.X != 0 || v.Y != 0 || v.Z != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Vector4 v)
        {
            {
                return v.X != 0 || v.Y != 0 || v.Z != 0 || v.W != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix1x1 v)
        {
            {
                return v.M00 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix1x2 v)
        {
            {
                return v.M00 != 0 || v.M01 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix1x3 v)
        {
            {
                return v.M00 != 0 || v.M01 != 0 || v.M02 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix1x4 v)
        {
            {
                return v.M00 != 0 || v.M01 != 0 || v.M02 != 0 || v.M03 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix2x1 v)
        {
            {
                return v.M00 != 0 || v.M10 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix2x2 v)
        {
            {
                return v.M00 != 0 || v.M01 != 0 || v.M10 != 0 || v.M11 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix2x3 v)
        {
            {
                return v.M00 != 0 || v.M01 != 0 || v.M02 != 0 || v.M10 != 0 || v.M11 != 0 || v.M12 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix2x4 v)
        {
            {
                return v.M00 != 0 || v.M01 != 0 || v.M02 != 0 || v.M03 != 0 || v.M10 != 0 || v.M11 != 0 || v.M12 != 0 || v.M13 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix3x1 v)
        {
            {
                return v.M00 != 0 || v.M10 != 0 || v.M20 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix3x2 v)
        {
            {
                return v.M00 != 0 || v.M01 != 0 || v.M10 != 0 || v.M11 != 0 || v.M20 != 0 || v.M21 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix3x3 v)
        {
            {
                return v.M00 != 0 || v.M01 != 0 || v.M02 != 0 || v.M10 != 0 || v.M11 != 0 || v.M12 != 0 || v.M20 != 0 || v.M21 != 0 || v.M22 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix3x4 v)
        {
            {
                return v.M00 != 0 || v.M01 != 0 || v.M02 != 0 || v.M03 != 0 || v.M10 != 0 || v.M11 != 0 || v.M12 != 0 || v.M13 != 0 || v.M20 != 0 || v.M21 != 0 || v.M22 != 0 || v.M23 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix4x1 v)
        {
            {
                return v.M00 != 0 || v.M10 != 0 || v.M20 != 0 || v.M30 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix4x2 v)
        {
            {
                return v.M00 != 0 || v.M01 != 0 || v.M10 != 0 || v.M11 != 0 || v.M20 != 0 || v.M21 != 0 || v.M30 != 0 || v.M31 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix4x3 v)
        {
            {
                return v.M00 != 0 || v.M01 != 0 || v.M02 != 0 || v.M10 != 0 || v.M11 != 0 || v.M12 != 0 || v.M20 != 0 || v.M21 != 0 || v.M22 != 0 || v.M30 != 0 || v.M31 != 0 || v.M32 != 0;
            }
        }

        /// <summary>
        /// Tests if any component of v is nonzero.
        /// </summary>
        public static bool any(Matrix4x4 v)
        {
            {
                return v.M00 != 0 || v.M01 != 0 || v.M02 != 0 || v.M03 != 0 || v.M10 != 0 || v.M11 != 0 || v.M12 != 0 || v.M13 != 0 || v.M20 != 0 || v.M21 != 0 || v.M22 != 0 || v.M23 != 0 || v.M30 != 0 || v.M31 != 0 || v.M32 != 0 || v.M33 != 0;
            }
        }
        #endregion
        #region asin

        /// <summary>
        /// Performs the asin function to the specified Vector1 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Vector1 asin(Vector1 v)
        {
            return new Vector1((float)Math.Asin(v.X));
        }

        /// <summary>
        /// Performs the asin function to the specified Vector2 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Vector2 asin(Vector2 v)
        {
            return new Vector2((float)Math.Asin(v.X), (float)Math.Asin(v.Y));
        }

        /// <summary>
        /// Performs the asin function to the specified Vector3 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Vector3 asin(Vector3 v)
        {
            return new Vector3((float)Math.Asin(v.X), (float)Math.Asin(v.Y), (float)Math.Asin(v.Z));
        }

        /// <summary>
        /// Performs the asin function to the specified Vector4 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Vector4 asin(Vector4 v)
        {
            return new Vector4((float)Math.Asin(v.X), (float)Math.Asin(v.Y), (float)Math.Asin(v.Z), (float)Math.Asin(v.W));
        }


        /// <summary>
        /// Performs the asin function to the specified Matrix1x1 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix1x1 asin(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Asin(v.M00));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix1x2 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix1x2 asin(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Asin(v.M00), (float)Math.Asin(v.M01));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix1x3 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix1x3 asin(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Asin(v.M00), (float)Math.Asin(v.M01), (float)Math.Asin(v.M02));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix1x4 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix1x4 asin(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Asin(v.M00), (float)Math.Asin(v.M01), (float)Math.Asin(v.M02), (float)Math.Asin(v.M03));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix2x1 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix2x1 asin(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Asin(v.M00), (float)Math.Asin(v.M10));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix2x2 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix2x2 asin(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Asin(v.M00), (float)Math.Asin(v.M01), (float)Math.Asin(v.M10), (float)Math.Asin(v.M11));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix2x3 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix2x3 asin(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Asin(v.M00), (float)Math.Asin(v.M01), (float)Math.Asin(v.M02), (float)Math.Asin(v.M10), (float)Math.Asin(v.M11), (float)Math.Asin(v.M12));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix2x4 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix2x4 asin(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Asin(v.M00), (float)Math.Asin(v.M01), (float)Math.Asin(v.M02), (float)Math.Asin(v.M03), (float)Math.Asin(v.M10), (float)Math.Asin(v.M11), (float)Math.Asin(v.M12), (float)Math.Asin(v.M13));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix3x1 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix3x1 asin(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Asin(v.M00), (float)Math.Asin(v.M10), (float)Math.Asin(v.M20));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix3x2 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix3x2 asin(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Asin(v.M00), (float)Math.Asin(v.M01), (float)Math.Asin(v.M10), (float)Math.Asin(v.M11), (float)Math.Asin(v.M20), (float)Math.Asin(v.M21));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix3x3 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix3x3 asin(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Asin(v.M00), (float)Math.Asin(v.M01), (float)Math.Asin(v.M02), (float)Math.Asin(v.M10), (float)Math.Asin(v.M11), (float)Math.Asin(v.M12), (float)Math.Asin(v.M20), (float)Math.Asin(v.M21), (float)Math.Asin(v.M22));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix3x4 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix3x4 asin(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Asin(v.M00), (float)Math.Asin(v.M01), (float)Math.Asin(v.M02), (float)Math.Asin(v.M03), (float)Math.Asin(v.M10), (float)Math.Asin(v.M11), (float)Math.Asin(v.M12), (float)Math.Asin(v.M13), (float)Math.Asin(v.M20), (float)Math.Asin(v.M21), (float)Math.Asin(v.M22), (float)Math.Asin(v.M23));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix4x1 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix4x1 asin(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Asin(v.M00), (float)Math.Asin(v.M10), (float)Math.Asin(v.M20), (float)Math.Asin(v.M30));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix4x2 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix4x2 asin(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Asin(v.M00), (float)Math.Asin(v.M01), (float)Math.Asin(v.M10), (float)Math.Asin(v.M11), (float)Math.Asin(v.M20), (float)Math.Asin(v.M21), (float)Math.Asin(v.M30), (float)Math.Asin(v.M31));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix4x3 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix4x3 asin(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Asin(v.M00), (float)Math.Asin(v.M01), (float)Math.Asin(v.M02), (float)Math.Asin(v.M10), (float)Math.Asin(v.M11), (float)Math.Asin(v.M12), (float)Math.Asin(v.M20), (float)Math.Asin(v.M21), (float)Math.Asin(v.M22), (float)Math.Asin(v.M30), (float)Math.Asin(v.M31), (float)Math.Asin(v.M32));
        }

        /// <summary>
        /// Performs the asin function to the specified Matrix4x4 object.
        /// Gets the arc sine of each component.
        /// </summary>
        public static Matrix4x4 asin(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Asin(v.M00), (float)Math.Asin(v.M01), (float)Math.Asin(v.M02), (float)Math.Asin(v.M03), (float)Math.Asin(v.M10), (float)Math.Asin(v.M11), (float)Math.Asin(v.M12), (float)Math.Asin(v.M13), (float)Math.Asin(v.M20), (float)Math.Asin(v.M21), (float)Math.Asin(v.M22), (float)Math.Asin(v.M23), (float)Math.Asin(v.M30), (float)Math.Asin(v.M31), (float)Math.Asin(v.M32), (float)Math.Asin(v.M33));
        }

        #endregion
        #region atan

        /// <summary>
        /// Performs the atan function to the specified Vector1 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Vector1 atan(Vector1 v)
        {
            return new Vector1((float)Math.Atan(v.X));
        }

        /// <summary>
        /// Performs the atan function to the specified Vector2 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Vector2 atan(Vector2 v)
        {
            return new Vector2((float)Math.Atan(v.X), (float)Math.Atan(v.Y));
        }

        /// <summary>
        /// Performs the atan function to the specified Vector3 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Vector3 atan(Vector3 v)
        {
            return new Vector3((float)Math.Atan(v.X), (float)Math.Atan(v.Y), (float)Math.Atan(v.Z));
        }

        /// <summary>
        /// Performs the atan function to the specified Vector4 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Vector4 atan(Vector4 v)
        {
            return new Vector4((float)Math.Atan(v.X), (float)Math.Atan(v.Y), (float)Math.Atan(v.Z), (float)Math.Atan(v.W));
        }


        /// <summary>
        /// Performs the atan function to the specified Matrix1x1 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix1x1 atan(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Atan(v.M00));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix1x2 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix1x2 atan(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Atan(v.M00), (float)Math.Atan(v.M01));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix1x3 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix1x3 atan(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Atan(v.M00), (float)Math.Atan(v.M01), (float)Math.Atan(v.M02));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix1x4 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix1x4 atan(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Atan(v.M00), (float)Math.Atan(v.M01), (float)Math.Atan(v.M02), (float)Math.Atan(v.M03));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix2x1 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix2x1 atan(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Atan(v.M00), (float)Math.Atan(v.M10));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix2x2 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix2x2 atan(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Atan(v.M00), (float)Math.Atan(v.M01), (float)Math.Atan(v.M10), (float)Math.Atan(v.M11));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix2x3 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix2x3 atan(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Atan(v.M00), (float)Math.Atan(v.M01), (float)Math.Atan(v.M02), (float)Math.Atan(v.M10), (float)Math.Atan(v.M11), (float)Math.Atan(v.M12));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix2x4 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix2x4 atan(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Atan(v.M00), (float)Math.Atan(v.M01), (float)Math.Atan(v.M02), (float)Math.Atan(v.M03), (float)Math.Atan(v.M10), (float)Math.Atan(v.M11), (float)Math.Atan(v.M12), (float)Math.Atan(v.M13));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix3x1 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix3x1 atan(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Atan(v.M00), (float)Math.Atan(v.M10), (float)Math.Atan(v.M20));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix3x2 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix3x2 atan(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Atan(v.M00), (float)Math.Atan(v.M01), (float)Math.Atan(v.M10), (float)Math.Atan(v.M11), (float)Math.Atan(v.M20), (float)Math.Atan(v.M21));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix3x3 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix3x3 atan(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Atan(v.M00), (float)Math.Atan(v.M01), (float)Math.Atan(v.M02), (float)Math.Atan(v.M10), (float)Math.Atan(v.M11), (float)Math.Atan(v.M12), (float)Math.Atan(v.M20), (float)Math.Atan(v.M21), (float)Math.Atan(v.M22));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix3x4 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix3x4 atan(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Atan(v.M00), (float)Math.Atan(v.M01), (float)Math.Atan(v.M02), (float)Math.Atan(v.M03), (float)Math.Atan(v.M10), (float)Math.Atan(v.M11), (float)Math.Atan(v.M12), (float)Math.Atan(v.M13), (float)Math.Atan(v.M20), (float)Math.Atan(v.M21), (float)Math.Atan(v.M22), (float)Math.Atan(v.M23));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix4x1 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix4x1 atan(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Atan(v.M00), (float)Math.Atan(v.M10), (float)Math.Atan(v.M20), (float)Math.Atan(v.M30));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix4x2 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix4x2 atan(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Atan(v.M00), (float)Math.Atan(v.M01), (float)Math.Atan(v.M10), (float)Math.Atan(v.M11), (float)Math.Atan(v.M20), (float)Math.Atan(v.M21), (float)Math.Atan(v.M30), (float)Math.Atan(v.M31));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix4x3 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix4x3 atan(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Atan(v.M00), (float)Math.Atan(v.M01), (float)Math.Atan(v.M02), (float)Math.Atan(v.M10), (float)Math.Atan(v.M11), (float)Math.Atan(v.M12), (float)Math.Atan(v.M20), (float)Math.Atan(v.M21), (float)Math.Atan(v.M22), (float)Math.Atan(v.M30), (float)Math.Atan(v.M31), (float)Math.Atan(v.M32));
        }

        /// <summary>
        /// Performs the atan function to the specified Matrix4x4 object.
        /// Gets the arc tangent of v. Return value is in range [-pi/2, pi/2].
        /// </summary>
        public static Matrix4x4 atan(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Atan(v.M00), (float)Math.Atan(v.M01), (float)Math.Atan(v.M02), (float)Math.Atan(v.M03), (float)Math.Atan(v.M10), (float)Math.Atan(v.M11), (float)Math.Atan(v.M12), (float)Math.Atan(v.M13), (float)Math.Atan(v.M20), (float)Math.Atan(v.M21), (float)Math.Atan(v.M22), (float)Math.Atan(v.M23), (float)Math.Atan(v.M30), (float)Math.Atan(v.M31), (float)Math.Atan(v.M32), (float)Math.Atan(v.M33));
        }

        #endregion
        #region atan2

        /// <summary>
        /// Performs the atan2 function to the specified Vector1 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Vector1 atan2(Vector1 v1, Vector1 v2)
        {
            return new Vector1((float)Math.Atan2(v1.X, v2.X));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Vector2 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Vector2 atan2(Vector2 v1, Vector2 v2)
        {
            return new Vector2((float)Math.Atan2(v1.X, v2.X), (float)Math.Atan2(v1.Y, v2.Y));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Vector3 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Vector3 atan2(Vector3 v1, Vector3 v2)
        {
            return new Vector3((float)Math.Atan2(v1.X, v2.X), (float)Math.Atan2(v1.Y, v2.Y), (float)Math.Atan2(v1.Z, v2.Z));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Vector4 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Vector4 atan2(Vector4 v1, Vector4 v2)
        {
            return new Vector4((float)Math.Atan2(v1.X, v2.X), (float)Math.Atan2(v1.Y, v2.Y), (float)Math.Atan2(v1.Z, v2.Z), (float)Math.Atan2(v1.W, v2.W));
        }


        /// <summary>
        /// Performs the atan2 function to the specified Matrix1x1 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix1x1 atan2(Matrix1x1 v1, Matrix1x1 v2)
        {
            return new Matrix1x1((float)Math.Atan2(v1.M00, v2.M00));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix1x2 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix1x2 atan2(Matrix1x2 v1, Matrix1x2 v2)
        {
            return new Matrix1x2((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M01, v2.M01));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix1x3 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix1x3 atan2(Matrix1x3 v1, Matrix1x3 v2)
        {
            return new Matrix1x3((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M01, v2.M01), (float)Math.Atan2(v1.M02, v2.M02));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix1x4 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix1x4 atan2(Matrix1x4 v1, Matrix1x4 v2)
        {
            return new Matrix1x4((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M01, v2.M01), (float)Math.Atan2(v1.M02, v2.M02), (float)Math.Atan2(v1.M03, v2.M03));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix2x1 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix2x1 atan2(Matrix2x1 v1, Matrix2x1 v2)
        {
            return new Matrix2x1((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M10, v2.M10));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix2x2 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix2x2 atan2(Matrix2x2 v1, Matrix2x2 v2)
        {
            return new Matrix2x2((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M01, v2.M01), (float)Math.Atan2(v1.M10, v2.M10), (float)Math.Atan2(v1.M11, v2.M11));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix2x3 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix2x3 atan2(Matrix2x3 v1, Matrix2x3 v2)
        {
            return new Matrix2x3((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M01, v2.M01), (float)Math.Atan2(v1.M02, v2.M02), (float)Math.Atan2(v1.M10, v2.M10), (float)Math.Atan2(v1.M11, v2.M11), (float)Math.Atan2(v1.M12, v2.M12));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix2x4 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix2x4 atan2(Matrix2x4 v1, Matrix2x4 v2)
        {
            return new Matrix2x4((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M01, v2.M01), (float)Math.Atan2(v1.M02, v2.M02), (float)Math.Atan2(v1.M03, v2.M03), (float)Math.Atan2(v1.M10, v2.M10), (float)Math.Atan2(v1.M11, v2.M11), (float)Math.Atan2(v1.M12, v2.M12), (float)Math.Atan2(v1.M13, v2.M13));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix3x1 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix3x1 atan2(Matrix3x1 v1, Matrix3x1 v2)
        {
            return new Matrix3x1((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M10, v2.M10), (float)Math.Atan2(v1.M20, v2.M20));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix3x2 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix3x2 atan2(Matrix3x2 v1, Matrix3x2 v2)
        {
            return new Matrix3x2((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M01, v2.M01), (float)Math.Atan2(v1.M10, v2.M10), (float)Math.Atan2(v1.M11, v2.M11), (float)Math.Atan2(v1.M20, v2.M20), (float)Math.Atan2(v1.M21, v2.M21));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix3x3 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix3x3 atan2(Matrix3x3 v1, Matrix3x3 v2)
        {
            return new Matrix3x3((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M01, v2.M01), (float)Math.Atan2(v1.M02, v2.M02), (float)Math.Atan2(v1.M10, v2.M10), (float)Math.Atan2(v1.M11, v2.M11), (float)Math.Atan2(v1.M12, v2.M12), (float)Math.Atan2(v1.M20, v2.M20), (float)Math.Atan2(v1.M21, v2.M21), (float)Math.Atan2(v1.M22, v2.M22));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix3x4 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix3x4 atan2(Matrix3x4 v1, Matrix3x4 v2)
        {
            return new Matrix3x4((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M01, v2.M01), (float)Math.Atan2(v1.M02, v2.M02), (float)Math.Atan2(v1.M03, v2.M03), (float)Math.Atan2(v1.M10, v2.M10), (float)Math.Atan2(v1.M11, v2.M11), (float)Math.Atan2(v1.M12, v2.M12), (float)Math.Atan2(v1.M13, v2.M13), (float)Math.Atan2(v1.M20, v2.M20), (float)Math.Atan2(v1.M21, v2.M21), (float)Math.Atan2(v1.M22, v2.M22), (float)Math.Atan2(v1.M23, v2.M23));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix4x1 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix4x1 atan2(Matrix4x1 v1, Matrix4x1 v2)
        {
            return new Matrix4x1((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M10, v2.M10), (float)Math.Atan2(v1.M20, v2.M20), (float)Math.Atan2(v1.M30, v2.M30));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix4x2 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix4x2 atan2(Matrix4x2 v1, Matrix4x2 v2)
        {
            return new Matrix4x2((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M01, v2.M01), (float)Math.Atan2(v1.M10, v2.M10), (float)Math.Atan2(v1.M11, v2.M11), (float)Math.Atan2(v1.M20, v2.M20), (float)Math.Atan2(v1.M21, v2.M21), (float)Math.Atan2(v1.M30, v2.M30), (float)Math.Atan2(v1.M31, v2.M31));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix4x3 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix4x3 atan2(Matrix4x3 v1, Matrix4x3 v2)
        {
            return new Matrix4x3((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M01, v2.M01), (float)Math.Atan2(v1.M02, v2.M02), (float)Math.Atan2(v1.M10, v2.M10), (float)Math.Atan2(v1.M11, v2.M11), (float)Math.Atan2(v1.M12, v2.M12), (float)Math.Atan2(v1.M20, v2.M20), (float)Math.Atan2(v1.M21, v2.M21), (float)Math.Atan2(v1.M22, v2.M22), (float)Math.Atan2(v1.M30, v2.M30), (float)Math.Atan2(v1.M31, v2.M31), (float)Math.Atan2(v1.M32, v2.M32));
        }

        /// <summary>
        /// Performs the atan2 function to the specified Matrix4x4 objects.
        /// Gets the arc tangent of (v2/v1).
        /// </summary>
        public static Matrix4x4 atan2(Matrix4x4 v1, Matrix4x4 v2)
        {
            return new Matrix4x4((float)Math.Atan2(v1.M00, v2.M00), (float)Math.Atan2(v1.M01, v2.M01), (float)Math.Atan2(v1.M02, v2.M02), (float)Math.Atan2(v1.M03, v2.M03), (float)Math.Atan2(v1.M10, v2.M10), (float)Math.Atan2(v1.M11, v2.M11), (float)Math.Atan2(v1.M12, v2.M12), (float)Math.Atan2(v1.M13, v2.M13), (float)Math.Atan2(v1.M20, v2.M20), (float)Math.Atan2(v1.M21, v2.M21), (float)Math.Atan2(v1.M22, v2.M22), (float)Math.Atan2(v1.M23, v2.M23), (float)Math.Atan2(v1.M30, v2.M30), (float)Math.Atan2(v1.M31, v2.M31), (float)Math.Atan2(v1.M32, v2.M32), (float)Math.Atan2(v1.M33, v2.M33));
        }

        #endregion
        #region ceil

        /// <summary>
        /// Performs the ceil function to the specified Vector1 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Vector1 ceil(Vector1 v)
        {
            return new Vector1((float)Math.Ceiling(v.X));
        }

        /// <summary>
        /// Performs the ceil function to the specified Vector2 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Vector2 ceil(Vector2 v)
        {
            return new Vector2((float)Math.Ceiling(v.X), (float)Math.Ceiling(v.Y));
        }

        /// <summary>
        /// Performs the ceil function to the specified Vector3 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Vector3 ceil(Vector3 v)
        {
            return new Vector3((float)Math.Ceiling(v.X), (float)Math.Ceiling(v.Y), (float)Math.Ceiling(v.Z));
        }

        /// <summary>
        /// Performs the ceil function to the specified Vector4 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Vector4 ceil(Vector4 v)
        {
            return new Vector4((float)Math.Ceiling(v.X), (float)Math.Ceiling(v.Y), (float)Math.Ceiling(v.Z), (float)Math.Ceiling(v.W));
        }


        /// <summary>
        /// Performs the ceil function to the specified Matrix1x1 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix1x1 ceil(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Ceiling(v.M00));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix1x2 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix1x2 ceil(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M01));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix1x3 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix1x3 ceil(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M01), (float)Math.Ceiling(v.M02));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix1x4 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix1x4 ceil(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M01), (float)Math.Ceiling(v.M02), (float)Math.Ceiling(v.M03));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix2x1 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix2x1 ceil(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M10));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix2x2 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix2x2 ceil(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M01), (float)Math.Ceiling(v.M10), (float)Math.Ceiling(v.M11));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix2x3 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix2x3 ceil(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M01), (float)Math.Ceiling(v.M02), (float)Math.Ceiling(v.M10), (float)Math.Ceiling(v.M11), (float)Math.Ceiling(v.M12));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix2x4 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix2x4 ceil(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M01), (float)Math.Ceiling(v.M02), (float)Math.Ceiling(v.M03), (float)Math.Ceiling(v.M10), (float)Math.Ceiling(v.M11), (float)Math.Ceiling(v.M12), (float)Math.Ceiling(v.M13));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix3x1 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix3x1 ceil(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M10), (float)Math.Ceiling(v.M20));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix3x2 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix3x2 ceil(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M01), (float)Math.Ceiling(v.M10), (float)Math.Ceiling(v.M11), (float)Math.Ceiling(v.M20), (float)Math.Ceiling(v.M21));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix3x3 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix3x3 ceil(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M01), (float)Math.Ceiling(v.M02), (float)Math.Ceiling(v.M10), (float)Math.Ceiling(v.M11), (float)Math.Ceiling(v.M12), (float)Math.Ceiling(v.M20), (float)Math.Ceiling(v.M21), (float)Math.Ceiling(v.M22));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix3x4 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix3x4 ceil(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M01), (float)Math.Ceiling(v.M02), (float)Math.Ceiling(v.M03), (float)Math.Ceiling(v.M10), (float)Math.Ceiling(v.M11), (float)Math.Ceiling(v.M12), (float)Math.Ceiling(v.M13), (float)Math.Ceiling(v.M20), (float)Math.Ceiling(v.M21), (float)Math.Ceiling(v.M22), (float)Math.Ceiling(v.M23));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix4x1 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix4x1 ceil(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M10), (float)Math.Ceiling(v.M20), (float)Math.Ceiling(v.M30));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix4x2 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix4x2 ceil(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M01), (float)Math.Ceiling(v.M10), (float)Math.Ceiling(v.M11), (float)Math.Ceiling(v.M20), (float)Math.Ceiling(v.M21), (float)Math.Ceiling(v.M30), (float)Math.Ceiling(v.M31));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix4x3 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix4x3 ceil(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M01), (float)Math.Ceiling(v.M02), (float)Math.Ceiling(v.M10), (float)Math.Ceiling(v.M11), (float)Math.Ceiling(v.M12), (float)Math.Ceiling(v.M20), (float)Math.Ceiling(v.M21), (float)Math.Ceiling(v.M22), (float)Math.Ceiling(v.M30), (float)Math.Ceiling(v.M31), (float)Math.Ceiling(v.M32));
        }

        /// <summary>
        /// Performs the ceil function to the specified Matrix4x4 object.
        /// Gets the smallest integer that is greater than or equal to each component.
        /// </summary>
        public static Matrix4x4 ceil(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Ceiling(v.M00), (float)Math.Ceiling(v.M01), (float)Math.Ceiling(v.M02), (float)Math.Ceiling(v.M03), (float)Math.Ceiling(v.M10), (float)Math.Ceiling(v.M11), (float)Math.Ceiling(v.M12), (float)Math.Ceiling(v.M13), (float)Math.Ceiling(v.M20), (float)Math.Ceiling(v.M21), (float)Math.Ceiling(v.M22), (float)Math.Ceiling(v.M23), (float)Math.Ceiling(v.M30), (float)Math.Ceiling(v.M31), (float)Math.Ceiling(v.M32), (float)Math.Ceiling(v.M33));
        }

        #endregion
        #region clamp

        public static float clamp(float x, float min, float max)
        {
            return GMath.max(max, GMath.min(min, x));
        }
        /// <summary>
        /// Performs the clamp function to the specified Vector1 objects.
        /// Gets the clamp of each component to range [v2, s]
        /// </summary>
        public static Vector1 clamp(Vector1 v1, Vector1 v2, Vector1 s)
        {

            return max(v2, min(v1, s));

        }


        /// <summary>
        /// Performs the clamp function to the specified Vector2 objects.
        /// Gets the clamp of each component to range [v2, s]
        /// </summary>
        public static Vector2 clamp(Vector2 v1, Vector2 v2, Vector2 s)
        {

            return max(v2, min(v1, s));

        }


        /// <summary>
        /// Performs the clamp function to the specified Vector3 objects.
        /// Gets the clamp of each component to range [v2, s]
        /// </summary>
        public static Vector3 clamp(Vector3 v1, Vector3 v2, Vector3 s)
        {

            return max(v2, min(v1, s));

        }


        /// <summary>
        /// Performs the clamp function to the specified Vector4 objects.
        /// Gets the clamp of each component to range [v2, s]
        /// </summary>
        public static Vector4 clamp(Vector4 v1, Vector4 v2, Vector4 s)
        {

            return max(v2, min(v1, s));

        }

        #endregion
        #region cos

        /// <summary>
        /// Performs the cos function to the specified object.
        /// Gets the cosine of the element.
        /// </summary>
        public static float cos(float x)
        {
            return (float)Math.Cos(x);
        }

        /// <summary>
        /// Performs the cos function to the specified Vector1 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Vector1 cos(Vector1 v)
        {
            return new Vector1((float)Math.Cos(v.X));
        }

        /// <summary>
        /// Performs the cos function to the specified Vector2 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Vector2 cos(Vector2 v)
        {
            return new Vector2((float)Math.Cos(v.X), (float)Math.Cos(v.Y));
        }

        /// <summary>
        /// Performs the cos function to the specified Vector3 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Vector3 cos(Vector3 v)
        {
            return new Vector3((float)Math.Cos(v.X), (float)Math.Cos(v.Y), (float)Math.Cos(v.Z));
        }

        /// <summary>
        /// Performs the cos function to the specified Vector4 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Vector4 cos(Vector4 v)
        {
            return new Vector4((float)Math.Cos(v.X), (float)Math.Cos(v.Y), (float)Math.Cos(v.Z), (float)Math.Cos(v.W));
        }


        /// <summary>
        /// Performs the cos function to the specified Matrix1x1 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix1x1 cos(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Cos(v.M00));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix1x2 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix1x2 cos(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Cos(v.M00), (float)Math.Cos(v.M01));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix1x3 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix1x3 cos(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Cos(v.M00), (float)Math.Cos(v.M01), (float)Math.Cos(v.M02));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix1x4 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix1x4 cos(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Cos(v.M00), (float)Math.Cos(v.M01), (float)Math.Cos(v.M02), (float)Math.Cos(v.M03));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix2x1 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix2x1 cos(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Cos(v.M00), (float)Math.Cos(v.M10));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix2x2 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix2x2 cos(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Cos(v.M00), (float)Math.Cos(v.M01), (float)Math.Cos(v.M10), (float)Math.Cos(v.M11));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix2x3 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix2x3 cos(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Cos(v.M00), (float)Math.Cos(v.M01), (float)Math.Cos(v.M02), (float)Math.Cos(v.M10), (float)Math.Cos(v.M11), (float)Math.Cos(v.M12));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix2x4 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix2x4 cos(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Cos(v.M00), (float)Math.Cos(v.M01), (float)Math.Cos(v.M02), (float)Math.Cos(v.M03), (float)Math.Cos(v.M10), (float)Math.Cos(v.M11), (float)Math.Cos(v.M12), (float)Math.Cos(v.M13));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix3x1 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix3x1 cos(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Cos(v.M00), (float)Math.Cos(v.M10), (float)Math.Cos(v.M20));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix3x2 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix3x2 cos(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Cos(v.M00), (float)Math.Cos(v.M01), (float)Math.Cos(v.M10), (float)Math.Cos(v.M11), (float)Math.Cos(v.M20), (float)Math.Cos(v.M21));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix3x3 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix3x3 cos(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Cos(v.M00), (float)Math.Cos(v.M01), (float)Math.Cos(v.M02), (float)Math.Cos(v.M10), (float)Math.Cos(v.M11), (float)Math.Cos(v.M12), (float)Math.Cos(v.M20), (float)Math.Cos(v.M21), (float)Math.Cos(v.M22));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix3x4 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix3x4 cos(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Cos(v.M00), (float)Math.Cos(v.M01), (float)Math.Cos(v.M02), (float)Math.Cos(v.M03), (float)Math.Cos(v.M10), (float)Math.Cos(v.M11), (float)Math.Cos(v.M12), (float)Math.Cos(v.M13), (float)Math.Cos(v.M20), (float)Math.Cos(v.M21), (float)Math.Cos(v.M22), (float)Math.Cos(v.M23));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix4x1 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix4x1 cos(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Cos(v.M00), (float)Math.Cos(v.M10), (float)Math.Cos(v.M20), (float)Math.Cos(v.M30));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix4x2 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix4x2 cos(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Cos(v.M00), (float)Math.Cos(v.M01), (float)Math.Cos(v.M10), (float)Math.Cos(v.M11), (float)Math.Cos(v.M20), (float)Math.Cos(v.M21), (float)Math.Cos(v.M30), (float)Math.Cos(v.M31));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix4x3 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix4x3 cos(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Cos(v.M00), (float)Math.Cos(v.M01), (float)Math.Cos(v.M02), (float)Math.Cos(v.M10), (float)Math.Cos(v.M11), (float)Math.Cos(v.M12), (float)Math.Cos(v.M20), (float)Math.Cos(v.M21), (float)Math.Cos(v.M22), (float)Math.Cos(v.M30), (float)Math.Cos(v.M31), (float)Math.Cos(v.M32));
        }

        /// <summary>
        /// Performs the cos function to the specified Matrix4x4 object.
        /// Gets the cosine of each component.
        /// </summary>
        public static Matrix4x4 cos(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Cos(v.M00), (float)Math.Cos(v.M01), (float)Math.Cos(v.M02), (float)Math.Cos(v.M03), (float)Math.Cos(v.M10), (float)Math.Cos(v.M11), (float)Math.Cos(v.M12), (float)Math.Cos(v.M13), (float)Math.Cos(v.M20), (float)Math.Cos(v.M21), (float)Math.Cos(v.M22), (float)Math.Cos(v.M23), (float)Math.Cos(v.M30), (float)Math.Cos(v.M31), (float)Math.Cos(v.M32), (float)Math.Cos(v.M33));
        }

        #endregion
        #region cosh

        /// <summary>
        /// Performs the cosh function to the specified Vector1 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Vector1 cosh(Vector1 v)
        {
            return new Vector1((float)Math.Cosh(v.X));
        }

        /// <summary>
        /// Performs the cosh function to the specified Vector2 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Vector2 cosh(Vector2 v)
        {
            return new Vector2((float)Math.Cosh(v.X), (float)Math.Cosh(v.Y));
        }

        /// <summary>
        /// Performs the cosh function to the specified Vector3 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Vector3 cosh(Vector3 v)
        {
            return new Vector3((float)Math.Cosh(v.X), (float)Math.Cosh(v.Y), (float)Math.Cosh(v.Z));
        }

        /// <summary>
        /// Performs the cosh function to the specified Vector4 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Vector4 cosh(Vector4 v)
        {
            return new Vector4((float)Math.Cosh(v.X), (float)Math.Cosh(v.Y), (float)Math.Cosh(v.Z), (float)Math.Cosh(v.W));
        }


        /// <summary>
        /// Performs the cosh function to the specified Matrix1x1 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix1x1 cosh(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Cosh(v.M00));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix1x2 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix1x2 cosh(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M01));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix1x3 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix1x3 cosh(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M01), (float)Math.Cosh(v.M02));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix1x4 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix1x4 cosh(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M01), (float)Math.Cosh(v.M02), (float)Math.Cosh(v.M03));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix2x1 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix2x1 cosh(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M10));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix2x2 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix2x2 cosh(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M01), (float)Math.Cosh(v.M10), (float)Math.Cosh(v.M11));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix2x3 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix2x3 cosh(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M01), (float)Math.Cosh(v.M02), (float)Math.Cosh(v.M10), (float)Math.Cosh(v.M11), (float)Math.Cosh(v.M12));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix2x4 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix2x4 cosh(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M01), (float)Math.Cosh(v.M02), (float)Math.Cosh(v.M03), (float)Math.Cosh(v.M10), (float)Math.Cosh(v.M11), (float)Math.Cosh(v.M12), (float)Math.Cosh(v.M13));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix3x1 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix3x1 cosh(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M10), (float)Math.Cosh(v.M20));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix3x2 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix3x2 cosh(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M01), (float)Math.Cosh(v.M10), (float)Math.Cosh(v.M11), (float)Math.Cosh(v.M20), (float)Math.Cosh(v.M21));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix3x3 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix3x3 cosh(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M01), (float)Math.Cosh(v.M02), (float)Math.Cosh(v.M10), (float)Math.Cosh(v.M11), (float)Math.Cosh(v.M12), (float)Math.Cosh(v.M20), (float)Math.Cosh(v.M21), (float)Math.Cosh(v.M22));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix3x4 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix3x4 cosh(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M01), (float)Math.Cosh(v.M02), (float)Math.Cosh(v.M03), (float)Math.Cosh(v.M10), (float)Math.Cosh(v.M11), (float)Math.Cosh(v.M12), (float)Math.Cosh(v.M13), (float)Math.Cosh(v.M20), (float)Math.Cosh(v.M21), (float)Math.Cosh(v.M22), (float)Math.Cosh(v.M23));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix4x1 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix4x1 cosh(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M10), (float)Math.Cosh(v.M20), (float)Math.Cosh(v.M30));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix4x2 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix4x2 cosh(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M01), (float)Math.Cosh(v.M10), (float)Math.Cosh(v.M11), (float)Math.Cosh(v.M20), (float)Math.Cosh(v.M21), (float)Math.Cosh(v.M30), (float)Math.Cosh(v.M31));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix4x3 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix4x3 cosh(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M01), (float)Math.Cosh(v.M02), (float)Math.Cosh(v.M10), (float)Math.Cosh(v.M11), (float)Math.Cosh(v.M12), (float)Math.Cosh(v.M20), (float)Math.Cosh(v.M21), (float)Math.Cosh(v.M22), (float)Math.Cosh(v.M30), (float)Math.Cosh(v.M31), (float)Math.Cosh(v.M32));
        }

        /// <summary>
        /// Performs the cosh function to the specified Matrix4x4 object.
        /// Gets the hyperbolic cosine of each component.
        /// </summary>
        public static Matrix4x4 cosh(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Cosh(v.M00), (float)Math.Cosh(v.M01), (float)Math.Cosh(v.M02), (float)Math.Cosh(v.M03), (float)Math.Cosh(v.M10), (float)Math.Cosh(v.M11), (float)Math.Cosh(v.M12), (float)Math.Cosh(v.M13), (float)Math.Cosh(v.M20), (float)Math.Cosh(v.M21), (float)Math.Cosh(v.M22), (float)Math.Cosh(v.M23), (float)Math.Cosh(v.M30), (float)Math.Cosh(v.M31), (float)Math.Cosh(v.M32), (float)Math.Cosh(v.M33));
        }

        #endregion
        #region cross
        public static Vector3 cross(Vector3 pto1, Vector3 pto2)
        {
            return new Vector3(
                pto1.Y * pto2.Z - pto1.Z * pto2.Y,
                pto1.Z * pto2.X - pto1.X * pto2.Z,
                pto1.X * pto2.Y - pto1.Y * pto2.X);
        }
        #endregion
        #region degrees

        /// <summary>
        /// Performs the degrees function to the specified Vector1 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Vector1 degrees(Vector1 v)
        {
            return new Vector1((float)(180 * v.X / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Vector2 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Vector2 degrees(Vector2 v)
        {
            return new Vector2((float)(180 * v.X / Math.PI), (float)(180 * v.Y / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Vector3 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Vector3 degrees(Vector3 v)
        {
            return new Vector3((float)(180 * v.X / Math.PI), (float)(180 * v.Y / Math.PI), (float)(180 * v.Z / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Vector4 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Vector4 degrees(Vector4 v)
        {
            return new Vector4((float)(180 * v.X / Math.PI), (float)(180 * v.Y / Math.PI), (float)(180 * v.Z / Math.PI), (float)(180 * v.W / Math.PI));
        }


        /// <summary>
        /// Performs the degrees function to the specified Matrix1x1 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix1x1 degrees(Matrix1x1 v)
        {
            return new Matrix1x1((float)(180 * v.M00 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix1x2 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix1x2 degrees(Matrix1x2 v)
        {
            return new Matrix1x2((float)(180 * v.M00 / Math.PI), (float)(180 * v.M01 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix1x3 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix1x3 degrees(Matrix1x3 v)
        {
            return new Matrix1x3((float)(180 * v.M00 / Math.PI), (float)(180 * v.M01 / Math.PI), (float)(180 * v.M02 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix1x4 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix1x4 degrees(Matrix1x4 v)
        {
            return new Matrix1x4((float)(180 * v.M00 / Math.PI), (float)(180 * v.M01 / Math.PI), (float)(180 * v.M02 / Math.PI), (float)(180 * v.M03 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix2x1 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix2x1 degrees(Matrix2x1 v)
        {
            return new Matrix2x1((float)(180 * v.M00 / Math.PI), (float)(180 * v.M10 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix2x2 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix2x2 degrees(Matrix2x2 v)
        {
            return new Matrix2x2((float)(180 * v.M00 / Math.PI), (float)(180 * v.M01 / Math.PI), (float)(180 * v.M10 / Math.PI), (float)(180 * v.M11 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix2x3 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix2x3 degrees(Matrix2x3 v)
        {
            return new Matrix2x3((float)(180 * v.M00 / Math.PI), (float)(180 * v.M01 / Math.PI), (float)(180 * v.M02 / Math.PI), (float)(180 * v.M10 / Math.PI), (float)(180 * v.M11 / Math.PI), (float)(180 * v.M12 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix2x4 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix2x4 degrees(Matrix2x4 v)
        {
            return new Matrix2x4((float)(180 * v.M00 / Math.PI), (float)(180 * v.M01 / Math.PI), (float)(180 * v.M02 / Math.PI), (float)(180 * v.M03 / Math.PI), (float)(180 * v.M10 / Math.PI), (float)(180 * v.M11 / Math.PI), (float)(180 * v.M12 / Math.PI), (float)(180 * v.M13 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix3x1 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix3x1 degrees(Matrix3x1 v)
        {
            return new Matrix3x1((float)(180 * v.M00 / Math.PI), (float)(180 * v.M10 / Math.PI), (float)(180 * v.M20 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix3x2 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix3x2 degrees(Matrix3x2 v)
        {
            return new Matrix3x2((float)(180 * v.M00 / Math.PI), (float)(180 * v.M01 / Math.PI), (float)(180 * v.M10 / Math.PI), (float)(180 * v.M11 / Math.PI), (float)(180 * v.M20 / Math.PI), (float)(180 * v.M21 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix3x3 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix3x3 degrees(Matrix3x3 v)
        {
            return new Matrix3x3((float)(180 * v.M00 / Math.PI), (float)(180 * v.M01 / Math.PI), (float)(180 * v.M02 / Math.PI), (float)(180 * v.M10 / Math.PI), (float)(180 * v.M11 / Math.PI), (float)(180 * v.M12 / Math.PI), (float)(180 * v.M20 / Math.PI), (float)(180 * v.M21 / Math.PI), (float)(180 * v.M22 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix3x4 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix3x4 degrees(Matrix3x4 v)
        {
            return new Matrix3x4((float)(180 * v.M00 / Math.PI), (float)(180 * v.M01 / Math.PI), (float)(180 * v.M02 / Math.PI), (float)(180 * v.M03 / Math.PI), (float)(180 * v.M10 / Math.PI), (float)(180 * v.M11 / Math.PI), (float)(180 * v.M12 / Math.PI), (float)(180 * v.M13 / Math.PI), (float)(180 * v.M20 / Math.PI), (float)(180 * v.M21 / Math.PI), (float)(180 * v.M22 / Math.PI), (float)(180 * v.M23 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix4x1 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix4x1 degrees(Matrix4x1 v)
        {
            return new Matrix4x1((float)(180 * v.M00 / Math.PI), (float)(180 * v.M10 / Math.PI), (float)(180 * v.M20 / Math.PI), (float)(180 * v.M30 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix4x2 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix4x2 degrees(Matrix4x2 v)
        {
            return new Matrix4x2((float)(180 * v.M00 / Math.PI), (float)(180 * v.M01 / Math.PI), (float)(180 * v.M10 / Math.PI), (float)(180 * v.M11 / Math.PI), (float)(180 * v.M20 / Math.PI), (float)(180 * v.M21 / Math.PI), (float)(180 * v.M30 / Math.PI), (float)(180 * v.M31 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix4x3 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix4x3 degrees(Matrix4x3 v)
        {
            return new Matrix4x3((float)(180 * v.M00 / Math.PI), (float)(180 * v.M01 / Math.PI), (float)(180 * v.M02 / Math.PI), (float)(180 * v.M10 / Math.PI), (float)(180 * v.M11 / Math.PI), (float)(180 * v.M12 / Math.PI), (float)(180 * v.M20 / Math.PI), (float)(180 * v.M21 / Math.PI), (float)(180 * v.M22 / Math.PI), (float)(180 * v.M30 / Math.PI), (float)(180 * v.M31 / Math.PI), (float)(180 * v.M32 / Math.PI));
        }

        /// <summary>
        /// Performs the degrees function to the specified Matrix4x4 object.
        /// Gets the values of each component converted from radiands to degrees.
        /// </summary>
        public static Matrix4x4 degrees(Matrix4x4 v)
        {
            return new Matrix4x4((float)(180 * v.M00 / Math.PI), (float)(180 * v.M01 / Math.PI), (float)(180 * v.M02 / Math.PI), (float)(180 * v.M03 / Math.PI), (float)(180 * v.M10 / Math.PI), (float)(180 * v.M11 / Math.PI), (float)(180 * v.M12 / Math.PI), (float)(180 * v.M13 / Math.PI), (float)(180 * v.M20 / Math.PI), (float)(180 * v.M21 / Math.PI), (float)(180 * v.M22 / Math.PI), (float)(180 * v.M23 / Math.PI), (float)(180 * v.M30 / Math.PI), (float)(180 * v.M31 / Math.PI), (float)(180 * v.M32 / Math.PI), (float)(180 * v.M33 / Math.PI));
        }

        #endregion
        #region determinant

        public static float determinant(Matrix1x1 m)
        {
            return m.M00;
        }

        public static float determinant(Matrix2x2 m)
        {
            return m.M00 * m.M11 - m.M01 * m.M10;
        }

        public static float determinant(Matrix3x3 m)
        {
            /// 00 01 02
            /// 10 11 12
            /// 20 21 22
            float Min00 = m.M11 * m.M22 - m.M12 * m.M21;
            float Min01 = m.M10 * m.M22 - m.M12 * m.M20;
            float Min02 = m.M10 * m.M21 - m.M11 * m.M20;

            return Min00 * m.M00 - Min01 * m.M01 + Min02 * m.M02;
        }

        public static float determinant(Matrix4x4 m)
        {
            float Min00 = m.M11 * m.M22 * m.M33 + m.M12 * m.M23 * m.M31 + m.M13 * m.M21 * m.M32 - m.M11 * m.M23 * m.M32 - m.M12 * m.M21 * m.M33 - m.M13 * m.M22 * m.M31;
            float Min01 = m.M10 * m.M22 * m.M33 + m.M12 * m.M23 * m.M30 + m.M13 * m.M20 * m.M32 - m.M10 * m.M23 * m.M32 - m.M12 * m.M20 * m.M33 - m.M13 * m.M22 * m.M30;
            float Min02 = m.M10 * m.M21 * m.M33 + m.M11 * m.M23 * m.M30 + m.M13 * m.M20 * m.M31 - m.M10 * m.M23 * m.M31 - m.M11 * m.M20 * m.M33 - m.M13 * m.M21 * m.M30;
            float Min03 = m.M10 * m.M21 * m.M32 + m.M11 * m.M22 * m.M30 + m.M12 * m.M20 * m.M31 - m.M10 * m.M22 * m.M31 - m.M11 * m.M20 * m.M32 - m.M12 * m.M21 * m.M30;

            return Min00 * m.M00 - Min01 * m.M01 + Min02 * m.M02 - Min03 * m.M03;
        }

        #endregion
        #region distance

        /// <summary>
        /// Performs the distance function to the specified Vector1 objects.
        /// Gets the distance between v1 and v2.
        /// </summary>
        public static float distance(Vector1 v1, Vector1 v2)
        {

            Vector1 d = v1 - v2;
            return (float)Math.Sqrt(dot(d, d));

        }


        /// <summary>
        /// Performs the distance function to the specified Vector2 objects.
        /// Gets the distance between v1 and v2.
        /// </summary>
        public static float distance(Vector2 v1, Vector2 v2)
        {

            Vector2 d = v1 - v2;
            return (float)Math.Sqrt(dot(d, d));

        }


        /// <summary>
        /// Performs the distance function to the specified Vector3 objects.
        /// Gets the distance between v1 and v2.
        /// </summary>
        public static float distance(Vector3 v1, Vector3 v2)
        {

            Vector3 d = v1 - v2;
            return (float)Math.Sqrt(dot(d, d));

        }


        /// <summary>
        /// Performs the distance function to the specified Vector4 objects.
        /// Gets the distance between v1 and v2.
        /// </summary>
        public static float distance(Vector4 v1, Vector4 v2)
        {

            Vector4 d = v1 - v2;
            return (float)Math.Sqrt(dot(d, d));

        }

        #endregion
        #region dot

        /// <summary>
        /// Performs the dot function to the specified Vector1 objects.
        /// Gets the dot product of v1 and v2.
        /// </summary>
        public static float dot(Vector1 v1, Vector1 v2)
        {
            return v1.X*v2.X;
        }


        /// <summary>
        /// Performs the dot function to the specified Vector2 objects.
        /// Gets the dot product of v1 and v2.
        /// </summary>
        public static float dot(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }


        /// <summary>
        /// Performs the dot function to the specified Vector3 objects.
        /// Gets the dot product of v1 and v2.
        /// </summary>
        public static float dot(Vector3 v1, Vector3 v2)
        {
            return v1.X*v2.X+v1.Y*v2.Y+v1.Z*v2.Z;
        }


        /// <summary>
        /// Performs the dot function to the specified Vector4 objects.
        /// Gets the dot product of v1 and v2.
        /// </summary>
        public static float dot(Vector4 v1, Vector4 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z + v1.W * v2.W;
        }

        #endregion
        #region exp

        /// <summary>
        /// Performs the exp function to the specified Vector1 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Vector1 exp(Vector1 v)
        {
            return new Vector1((float)Math.Exp(v.X));
        }

        /// <summary>
        /// Performs the exp function to the specified Vector2 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Vector2 exp(Vector2 v)
        {
            return new Vector2((float)Math.Exp(v.X), (float)Math.Exp(v.Y));
        }

        /// <summary>
        /// Performs the exp function to the specified Vector3 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Vector3 exp(Vector3 v)
        {
            return new Vector3((float)Math.Exp(v.X), (float)Math.Exp(v.Y), (float)Math.Exp(v.Z));
        }

        /// <summary>
        /// Performs the exp function to the specified Vector4 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Vector4 exp(Vector4 v)
        {
            return new Vector4((float)Math.Exp(v.X), (float)Math.Exp(v.Y), (float)Math.Exp(v.Z), (float)Math.Exp(v.W));
        }


        /// <summary>
        /// Performs the exp function to the specified Matrix1x1 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix1x1 exp(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Exp(v.M00));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix1x2 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix1x2 exp(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Exp(v.M00), (float)Math.Exp(v.M01));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix1x3 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix1x3 exp(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Exp(v.M00), (float)Math.Exp(v.M01), (float)Math.Exp(v.M02));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix1x4 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix1x4 exp(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Exp(v.M00), (float)Math.Exp(v.M01), (float)Math.Exp(v.M02), (float)Math.Exp(v.M03));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix2x1 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix2x1 exp(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Exp(v.M00), (float)Math.Exp(v.M10));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix2x2 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix2x2 exp(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Exp(v.M00), (float)Math.Exp(v.M01), (float)Math.Exp(v.M10), (float)Math.Exp(v.M11));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix2x3 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix2x3 exp(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Exp(v.M00), (float)Math.Exp(v.M01), (float)Math.Exp(v.M02), (float)Math.Exp(v.M10), (float)Math.Exp(v.M11), (float)Math.Exp(v.M12));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix2x4 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix2x4 exp(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Exp(v.M00), (float)Math.Exp(v.M01), (float)Math.Exp(v.M02), (float)Math.Exp(v.M03), (float)Math.Exp(v.M10), (float)Math.Exp(v.M11), (float)Math.Exp(v.M12), (float)Math.Exp(v.M13));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix3x1 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix3x1 exp(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Exp(v.M00), (float)Math.Exp(v.M10), (float)Math.Exp(v.M20));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix3x2 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix3x2 exp(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Exp(v.M00), (float)Math.Exp(v.M01), (float)Math.Exp(v.M10), (float)Math.Exp(v.M11), (float)Math.Exp(v.M20), (float)Math.Exp(v.M21));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix3x3 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix3x3 exp(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Exp(v.M00), (float)Math.Exp(v.M01), (float)Math.Exp(v.M02), (float)Math.Exp(v.M10), (float)Math.Exp(v.M11), (float)Math.Exp(v.M12), (float)Math.Exp(v.M20), (float)Math.Exp(v.M21), (float)Math.Exp(v.M22));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix3x4 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix3x4 exp(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Exp(v.M00), (float)Math.Exp(v.M01), (float)Math.Exp(v.M02), (float)Math.Exp(v.M03), (float)Math.Exp(v.M10), (float)Math.Exp(v.M11), (float)Math.Exp(v.M12), (float)Math.Exp(v.M13), (float)Math.Exp(v.M20), (float)Math.Exp(v.M21), (float)Math.Exp(v.M22), (float)Math.Exp(v.M23));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix4x1 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix4x1 exp(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Exp(v.M00), (float)Math.Exp(v.M10), (float)Math.Exp(v.M20), (float)Math.Exp(v.M30));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix4x2 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix4x2 exp(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Exp(v.M00), (float)Math.Exp(v.M01), (float)Math.Exp(v.M10), (float)Math.Exp(v.M11), (float)Math.Exp(v.M20), (float)Math.Exp(v.M21), (float)Math.Exp(v.M30), (float)Math.Exp(v.M31));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix4x3 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix4x3 exp(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Exp(v.M00), (float)Math.Exp(v.M01), (float)Math.Exp(v.M02), (float)Math.Exp(v.M10), (float)Math.Exp(v.M11), (float)Math.Exp(v.M12), (float)Math.Exp(v.M20), (float)Math.Exp(v.M21), (float)Math.Exp(v.M22), (float)Math.Exp(v.M30), (float)Math.Exp(v.M31), (float)Math.Exp(v.M32));
        }

        /// <summary>
        /// Performs the exp function to the specified Matrix4x4 object.
        /// Gets the exponentiation of e to each component.
        /// </summary>
        public static Matrix4x4 exp(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Exp(v.M00), (float)Math.Exp(v.M01), (float)Math.Exp(v.M02), (float)Math.Exp(v.M03), (float)Math.Exp(v.M10), (float)Math.Exp(v.M11), (float)Math.Exp(v.M12), (float)Math.Exp(v.M13), (float)Math.Exp(v.M20), (float)Math.Exp(v.M21), (float)Math.Exp(v.M22), (float)Math.Exp(v.M23), (float)Math.Exp(v.M30), (float)Math.Exp(v.M31), (float)Math.Exp(v.M32), (float)Math.Exp(v.M33));
        }

        #endregion
        #region exp2

        /// <summary>
        /// Performs the exp2 function to the specified Vector1 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Vector1 exp2(Vector1 v)
        {
            return new Vector1((float)Math.Pow(2, v.X));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Vector2 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Vector2 exp2(Vector2 v)
        {
            return new Vector2((float)Math.Pow(2, v.X), (float)Math.Pow(2, v.Y));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Vector3 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Vector3 exp2(Vector3 v)
        {
            return new Vector3((float)Math.Pow(2, v.X), (float)Math.Pow(2, v.Y), (float)Math.Pow(2, v.Z));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Vector4 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Vector4 exp2(Vector4 v)
        {
            return new Vector4((float)Math.Pow(2, v.X), (float)Math.Pow(2, v.Y), (float)Math.Pow(2, v.Z), (float)Math.Pow(2, v.W));
        }


        /// <summary>
        /// Performs the exp2 function to the specified Matrix1x1 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix1x1 exp2(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Pow(2, v.M00));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix1x2 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix1x2 exp2(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M01));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix1x3 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix1x3 exp2(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M01), (float)Math.Pow(2, v.M02));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix1x4 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix1x4 exp2(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M01), (float)Math.Pow(2, v.M02), (float)Math.Pow(2, v.M03));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix2x1 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix2x1 exp2(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M10));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix2x2 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix2x2 exp2(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M01), (float)Math.Pow(2, v.M10), (float)Math.Pow(2, v.M11));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix2x3 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix2x3 exp2(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M01), (float)Math.Pow(2, v.M02), (float)Math.Pow(2, v.M10), (float)Math.Pow(2, v.M11), (float)Math.Pow(2, v.M12));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix2x4 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix2x4 exp2(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M01), (float)Math.Pow(2, v.M02), (float)Math.Pow(2, v.M03), (float)Math.Pow(2, v.M10), (float)Math.Pow(2, v.M11), (float)Math.Pow(2, v.M12), (float)Math.Pow(2, v.M13));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix3x1 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix3x1 exp2(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M10), (float)Math.Pow(2, v.M20));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix3x2 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix3x2 exp2(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M01), (float)Math.Pow(2, v.M10), (float)Math.Pow(2, v.M11), (float)Math.Pow(2, v.M20), (float)Math.Pow(2, v.M21));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix3x3 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix3x3 exp2(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M01), (float)Math.Pow(2, v.M02), (float)Math.Pow(2, v.M10), (float)Math.Pow(2, v.M11), (float)Math.Pow(2, v.M12), (float)Math.Pow(2, v.M20), (float)Math.Pow(2, v.M21), (float)Math.Pow(2, v.M22));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix3x4 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix3x4 exp2(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M01), (float)Math.Pow(2, v.M02), (float)Math.Pow(2, v.M03), (float)Math.Pow(2, v.M10), (float)Math.Pow(2, v.M11), (float)Math.Pow(2, v.M12), (float)Math.Pow(2, v.M13), (float)Math.Pow(2, v.M20), (float)Math.Pow(2, v.M21), (float)Math.Pow(2, v.M22), (float)Math.Pow(2, v.M23));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix4x1 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix4x1 exp2(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M10), (float)Math.Pow(2, v.M20), (float)Math.Pow(2, v.M30));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix4x2 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix4x2 exp2(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M01), (float)Math.Pow(2, v.M10), (float)Math.Pow(2, v.M11), (float)Math.Pow(2, v.M20), (float)Math.Pow(2, v.M21), (float)Math.Pow(2, v.M30), (float)Math.Pow(2, v.M31));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix4x3 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix4x3 exp2(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M01), (float)Math.Pow(2, v.M02), (float)Math.Pow(2, v.M10), (float)Math.Pow(2, v.M11), (float)Math.Pow(2, v.M12), (float)Math.Pow(2, v.M20), (float)Math.Pow(2, v.M21), (float)Math.Pow(2, v.M22), (float)Math.Pow(2, v.M30), (float)Math.Pow(2, v.M31), (float)Math.Pow(2, v.M32));
        }

        /// <summary>
        /// Performs the exp2 function to the specified Matrix4x4 object.
        /// Gets the exponentiation of 2 to each component.
        /// </summary>
        public static Matrix4x4 exp2(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Pow(2, v.M00), (float)Math.Pow(2, v.M01), (float)Math.Pow(2, v.M02), (float)Math.Pow(2, v.M03), (float)Math.Pow(2, v.M10), (float)Math.Pow(2, v.M11), (float)Math.Pow(2, v.M12), (float)Math.Pow(2, v.M13), (float)Math.Pow(2, v.M20), (float)Math.Pow(2, v.M21), (float)Math.Pow(2, v.M22), (float)Math.Pow(2, v.M23), (float)Math.Pow(2, v.M30), (float)Math.Pow(2, v.M31), (float)Math.Pow(2, v.M32), (float)Math.Pow(2, v.M33));
        }

        #endregion
        #region floor

        /// <summary>
        /// Performs the floor function to the specified Vector1 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Vector1 floor(Vector1 v)
        {
            return new Vector1((float)Math.Floor(v.X));
        }

        /// <summary>
        /// Performs the floor function to the specified Vector2 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Vector2 floor(Vector2 v)
        {
            return new Vector2((float)Math.Floor(v.X), (float)Math.Floor(v.Y));
        }

        /// <summary>
        /// Performs the floor function to the specified Vector3 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Vector3 floor(Vector3 v)
        {
            return new Vector3((float)Math.Floor(v.X), (float)Math.Floor(v.Y), (float)Math.Floor(v.Z));
        }

        /// <summary>
        /// Performs the floor function to the specified Vector4 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Vector4 floor(Vector4 v)
        {
            return new Vector4((float)Math.Floor(v.X), (float)Math.Floor(v.Y), (float)Math.Floor(v.Z), (float)Math.Floor(v.W));
        }


        /// <summary>
        /// Performs the floor function to the specified Matrix1x1 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix1x1 floor(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Floor(v.M00));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix1x2 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix1x2 floor(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Floor(v.M00), (float)Math.Floor(v.M01));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix1x3 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix1x3 floor(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Floor(v.M00), (float)Math.Floor(v.M01), (float)Math.Floor(v.M02));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix1x4 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix1x4 floor(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Floor(v.M00), (float)Math.Floor(v.M01), (float)Math.Floor(v.M02), (float)Math.Floor(v.M03));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix2x1 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix2x1 floor(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Floor(v.M00), (float)Math.Floor(v.M10));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix2x2 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix2x2 floor(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Floor(v.M00), (float)Math.Floor(v.M01), (float)Math.Floor(v.M10), (float)Math.Floor(v.M11));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix2x3 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix2x3 floor(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Floor(v.M00), (float)Math.Floor(v.M01), (float)Math.Floor(v.M02), (float)Math.Floor(v.M10), (float)Math.Floor(v.M11), (float)Math.Floor(v.M12));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix2x4 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix2x4 floor(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Floor(v.M00), (float)Math.Floor(v.M01), (float)Math.Floor(v.M02), (float)Math.Floor(v.M03), (float)Math.Floor(v.M10), (float)Math.Floor(v.M11), (float)Math.Floor(v.M12), (float)Math.Floor(v.M13));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix3x1 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix3x1 floor(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Floor(v.M00), (float)Math.Floor(v.M10), (float)Math.Floor(v.M20));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix3x2 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix3x2 floor(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Floor(v.M00), (float)Math.Floor(v.M01), (float)Math.Floor(v.M10), (float)Math.Floor(v.M11), (float)Math.Floor(v.M20), (float)Math.Floor(v.M21));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix3x3 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix3x3 floor(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Floor(v.M00), (float)Math.Floor(v.M01), (float)Math.Floor(v.M02), (float)Math.Floor(v.M10), (float)Math.Floor(v.M11), (float)Math.Floor(v.M12), (float)Math.Floor(v.M20), (float)Math.Floor(v.M21), (float)Math.Floor(v.M22));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix3x4 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix3x4 floor(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Floor(v.M00), (float)Math.Floor(v.M01), (float)Math.Floor(v.M02), (float)Math.Floor(v.M03), (float)Math.Floor(v.M10), (float)Math.Floor(v.M11), (float)Math.Floor(v.M12), (float)Math.Floor(v.M13), (float)Math.Floor(v.M20), (float)Math.Floor(v.M21), (float)Math.Floor(v.M22), (float)Math.Floor(v.M23));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix4x1 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix4x1 floor(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Floor(v.M00), (float)Math.Floor(v.M10), (float)Math.Floor(v.M20), (float)Math.Floor(v.M30));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix4x2 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix4x2 floor(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Floor(v.M00), (float)Math.Floor(v.M01), (float)Math.Floor(v.M10), (float)Math.Floor(v.M11), (float)Math.Floor(v.M20), (float)Math.Floor(v.M21), (float)Math.Floor(v.M30), (float)Math.Floor(v.M31));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix4x3 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix4x3 floor(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Floor(v.M00), (float)Math.Floor(v.M01), (float)Math.Floor(v.M02), (float)Math.Floor(v.M10), (float)Math.Floor(v.M11), (float)Math.Floor(v.M12), (float)Math.Floor(v.M20), (float)Math.Floor(v.M21), (float)Math.Floor(v.M22), (float)Math.Floor(v.M30), (float)Math.Floor(v.M31), (float)Math.Floor(v.M32));
        }

        /// <summary>
        /// Performs the floor function to the specified Matrix4x4 object.
        /// Gets the greatest integer that is less than or equal to each component.
        /// </summary>
        public static Matrix4x4 floor(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Floor(v.M00), (float)Math.Floor(v.M01), (float)Math.Floor(v.M02), (float)Math.Floor(v.M03), (float)Math.Floor(v.M10), (float)Math.Floor(v.M11), (float)Math.Floor(v.M12), (float)Math.Floor(v.M13), (float)Math.Floor(v.M20), (float)Math.Floor(v.M21), (float)Math.Floor(v.M22), (float)Math.Floor(v.M23), (float)Math.Floor(v.M30), (float)Math.Floor(v.M31), (float)Math.Floor(v.M32), (float)Math.Floor(v.M33));
        }

        #endregion
        #region fmod

        /// <summary>
        /// Performs the fmod function to the specified Vector1 objects.
        /// 
        /// </summary>
        public static Vector1 fmod(Vector1 v1, Vector1 v2)
        {
            return new Vector1(v1.X % v2.X);
        }

        /// <summary>
        /// Performs the fmod function to the specified Vector2 objects.
        /// 
        /// </summary>
        public static Vector2 fmod(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X % v2.X, v1.Y % v2.Y);
        }

        /// <summary>
        /// Performs the fmod function to the specified Vector3 objects.
        /// 
        /// </summary>
        public static Vector3 fmod(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X % v2.X, v1.Y % v2.Y, v1.Z % v2.Z);
        }

        /// <summary>
        /// Performs the fmod function to the specified Vector4 objects.
        /// 
        /// </summary>
        public static Vector4 fmod(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.X % v2.X, v1.Y % v2.Y, v1.Z % v2.Z, v1.W % v2.W);
        }


        /// <summary>
        /// Performs the fmod function to the specified Matrix1x1 objects.
        /// 
        /// </summary>
        public static Matrix1x1 fmod(Matrix1x1 v1, Matrix1x1 v2)
        {
            return new Matrix1x1(v1.M00 % v2.M00);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix1x2 objects.
        /// 
        /// </summary>
        public static Matrix1x2 fmod(Matrix1x2 v1, Matrix1x2 v2)
        {
            return new Matrix1x2(v1.M00 % v2.M00, v1.M01 % v2.M01);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix1x3 objects.
        /// 
        /// </summary>
        public static Matrix1x3 fmod(Matrix1x3 v1, Matrix1x3 v2)
        {
            return new Matrix1x3(v1.M00 % v2.M00, v1.M01 % v2.M01, v1.M02 % v2.M02);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix1x4 objects.
        /// 
        /// </summary>
        public static Matrix1x4 fmod(Matrix1x4 v1, Matrix1x4 v2)
        {
            return new Matrix1x4(v1.M00 % v2.M00, v1.M01 % v2.M01, v1.M02 % v2.M02, v1.M03 % v2.M03);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix2x1 objects.
        /// 
        /// </summary>
        public static Matrix2x1 fmod(Matrix2x1 v1, Matrix2x1 v2)
        {
            return new Matrix2x1(v1.M00 % v2.M00, v1.M10 % v2.M10);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix2x2 objects.
        /// 
        /// </summary>
        public static Matrix2x2 fmod(Matrix2x2 v1, Matrix2x2 v2)
        {
            return new Matrix2x2(v1.M00 % v2.M00, v1.M01 % v2.M01, v1.M10 % v2.M10, v1.M11 % v2.M11);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix2x3 objects.
        /// 
        /// </summary>
        public static Matrix2x3 fmod(Matrix2x3 v1, Matrix2x3 v2)
        {
            return new Matrix2x3(v1.M00 % v2.M00, v1.M01 % v2.M01, v1.M02 % v2.M02, v1.M10 % v2.M10, v1.M11 % v2.M11, v1.M12 % v2.M12);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix2x4 objects.
        /// 
        /// </summary>
        public static Matrix2x4 fmod(Matrix2x4 v1, Matrix2x4 v2)
        {
            return new Matrix2x4(v1.M00 % v2.M00, v1.M01 % v2.M01, v1.M02 % v2.M02, v1.M03 % v2.M03, v1.M10 % v2.M10, v1.M11 % v2.M11, v1.M12 % v2.M12, v1.M13 % v2.M13);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix3x1 objects.
        /// 
        /// </summary>
        public static Matrix3x1 fmod(Matrix3x1 v1, Matrix3x1 v2)
        {
            return new Matrix3x1(v1.M00 % v2.M00, v1.M10 % v2.M10, v1.M20 % v2.M20);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix3x2 objects.
        /// 
        /// </summary>
        public static Matrix3x2 fmod(Matrix3x2 v1, Matrix3x2 v2)
        {
            return new Matrix3x2(v1.M00 % v2.M00, v1.M01 % v2.M01, v1.M10 % v2.M10, v1.M11 % v2.M11, v1.M20 % v2.M20, v1.M21 % v2.M21);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix3x3 objects.
        /// 
        /// </summary>
        public static Matrix3x3 fmod(Matrix3x3 v1, Matrix3x3 v2)
        {
            return new Matrix3x3(v1.M00 % v2.M00, v1.M01 % v2.M01, v1.M02 % v2.M02, v1.M10 % v2.M10, v1.M11 % v2.M11, v1.M12 % v2.M12, v1.M20 % v2.M20, v1.M21 % v2.M21, v1.M22 % v2.M22);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix3x4 objects.
        /// 
        /// </summary>
        public static Matrix3x4 fmod(Matrix3x4 v1, Matrix3x4 v2)
        {
            return new Matrix3x4(v1.M00 % v2.M00, v1.M01 % v2.M01, v1.M02 % v2.M02, v1.M03 % v2.M03, v1.M10 % v2.M10, v1.M11 % v2.M11, v1.M12 % v2.M12, v1.M13 % v2.M13, v1.M20 % v2.M20, v1.M21 % v2.M21, v1.M22 % v2.M22, v1.M23 % v2.M23);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix4x1 objects.
        /// 
        /// </summary>
        public static Matrix4x1 fmod(Matrix4x1 v1, Matrix4x1 v2)
        {
            return new Matrix4x1(v1.M00 % v2.M00, v1.M10 % v2.M10, v1.M20 % v2.M20, v1.M30 % v2.M30);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix4x2 objects.
        /// 
        /// </summary>
        public static Matrix4x2 fmod(Matrix4x2 v1, Matrix4x2 v2)
        {
            return new Matrix4x2(v1.M00 % v2.M00, v1.M01 % v2.M01, v1.M10 % v2.M10, v1.M11 % v2.M11, v1.M20 % v2.M20, v1.M21 % v2.M21, v1.M30 % v2.M30, v1.M31 % v2.M31);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix4x3 objects.
        /// 
        /// </summary>
        public static Matrix4x3 fmod(Matrix4x3 v1, Matrix4x3 v2)
        {
            return new Matrix4x3(v1.M00 % v2.M00, v1.M01 % v2.M01, v1.M02 % v2.M02, v1.M10 % v2.M10, v1.M11 % v2.M11, v1.M12 % v2.M12, v1.M20 % v2.M20, v1.M21 % v2.M21, v1.M22 % v2.M22, v1.M30 % v2.M30, v1.M31 % v2.M31, v1.M32 % v2.M32);
        }

        /// <summary>
        /// Performs the fmod function to the specified Matrix4x4 objects.
        /// 
        /// </summary>
        public static Matrix4x4 fmod(Matrix4x4 v1, Matrix4x4 v2)
        {
            return new Matrix4x4(v1.M00 % v2.M00, v1.M01 % v2.M01, v1.M02 % v2.M02, v1.M03 % v2.M03, v1.M10 % v2.M10, v1.M11 % v2.M11, v1.M12 % v2.M12, v1.M13 % v2.M13, v1.M20 % v2.M20, v1.M21 % v2.M21, v1.M22 % v2.M22, v1.M23 % v2.M23, v1.M30 % v2.M30, v1.M31 % v2.M31, v1.M32 % v2.M32, v1.M33 % v2.M33);
        }

        #endregion
        #region frac

        /// <summary>
        /// Performs the frac function to the specified Vector1 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Vector1 frac(Vector1 v)
        {
            return new Vector1(v.X % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Vector2 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Vector2 frac(Vector2 v)
        {
            return new Vector2(v.X % 1, v.Y % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Vector3 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Vector3 frac(Vector3 v)
        {
            return new Vector3(v.X % 1, v.Y % 1, v.Z % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Vector4 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Vector4 frac(Vector4 v)
        {
            return new Vector4(v.X % 1, v.Y % 1, v.Z % 1, v.W % 1);
        }


        /// <summary>
        /// Performs the frac function to the specified Matrix1x1 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix1x1 frac(Matrix1x1 v)
        {
            return new Matrix1x1(v.M00 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix1x2 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix1x2 frac(Matrix1x2 v)
        {
            return new Matrix1x2(v.M00 % 1, v.M01 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix1x3 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix1x3 frac(Matrix1x3 v)
        {
            return new Matrix1x3(v.M00 % 1, v.M01 % 1, v.M02 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix1x4 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix1x4 frac(Matrix1x4 v)
        {
            return new Matrix1x4(v.M00 % 1, v.M01 % 1, v.M02 % 1, v.M03 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix2x1 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix2x1 frac(Matrix2x1 v)
        {
            return new Matrix2x1(v.M00 % 1, v.M10 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix2x2 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix2x2 frac(Matrix2x2 v)
        {
            return new Matrix2x2(v.M00 % 1, v.M01 % 1, v.M10 % 1, v.M11 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix2x3 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix2x3 frac(Matrix2x3 v)
        {
            return new Matrix2x3(v.M00 % 1, v.M01 % 1, v.M02 % 1, v.M10 % 1, v.M11 % 1, v.M12 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix2x4 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix2x4 frac(Matrix2x4 v)
        {
            return new Matrix2x4(v.M00 % 1, v.M01 % 1, v.M02 % 1, v.M03 % 1, v.M10 % 1, v.M11 % 1, v.M12 % 1, v.M13 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix3x1 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix3x1 frac(Matrix3x1 v)
        {
            return new Matrix3x1(v.M00 % 1, v.M10 % 1, v.M20 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix3x2 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix3x2 frac(Matrix3x2 v)
        {
            return new Matrix3x2(v.M00 % 1, v.M01 % 1, v.M10 % 1, v.M11 % 1, v.M20 % 1, v.M21 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix3x3 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix3x3 frac(Matrix3x3 v)
        {
            return new Matrix3x3(v.M00 % 1, v.M01 % 1, v.M02 % 1, v.M10 % 1, v.M11 % 1, v.M12 % 1, v.M20 % 1, v.M21 % 1, v.M22 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix3x4 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix3x4 frac(Matrix3x4 v)
        {
            return new Matrix3x4(v.M00 % 1, v.M01 % 1, v.M02 % 1, v.M03 % 1, v.M10 % 1, v.M11 % 1, v.M12 % 1, v.M13 % 1, v.M20 % 1, v.M21 % 1, v.M22 % 1, v.M23 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix4x1 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix4x1 frac(Matrix4x1 v)
        {
            return new Matrix4x1(v.M00 % 1, v.M10 % 1, v.M20 % 1, v.M30 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix4x2 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix4x2 frac(Matrix4x2 v)
        {
            return new Matrix4x2(v.M00 % 1, v.M01 % 1, v.M10 % 1, v.M11 % 1, v.M20 % 1, v.M21 % 1, v.M30 % 1, v.M31 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix4x3 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix4x3 frac(Matrix4x3 v)
        {
            return new Matrix4x3(v.M00 % 1, v.M01 % 1, v.M02 % 1, v.M10 % 1, v.M11 % 1, v.M12 % 1, v.M20 % 1, v.M21 % 1, v.M22 % 1, v.M30 % 1, v.M31 % 1, v.M32 % 1);
        }

        /// <summary>
        /// Performs the frac function to the specified Matrix4x4 object.
        /// Gets the fractional part of each component.
        /// </summary>
        public static Matrix4x4 frac(Matrix4x4 v)
        {
            return new Matrix4x4(v.M00 % 1, v.M01 % 1, v.M02 % 1, v.M03 % 1, v.M10 % 1, v.M11 % 1, v.M12 % 1, v.M13 % 1, v.M20 % 1, v.M21 % 1, v.M22 % 1, v.M23 % 1, v.M30 % 1, v.M31 % 1, v.M32 % 1, v.M33 % 1);
        }

        #endregion
        #region isfinite

        /// <summary>
        /// Performs the isfinite function to the specified Vector1 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Vector1<bool> isfinite(Vector1 v)
        {
            return new Vector1<bool>(!float.IsInfinity(v.X));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Vector2 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Vector2<bool> isfinite(Vector2 v)
        {
            return new Vector2<bool>(!float.IsInfinity(v.X), !float.IsInfinity(v.Y));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Vector3 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Vector3<bool> isfinite(Vector3 v)
        {
            return new Vector3<bool>(!float.IsInfinity(v.X), !float.IsInfinity(v.Y), !float.IsInfinity(v.Z));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Vector4 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Vector4<bool> isfinite(Vector4 v)
        {
            return new Vector4<bool>(!float.IsInfinity(v.X), !float.IsInfinity(v.Y), !float.IsInfinity(v.Z), !float.IsInfinity(v.W));
        }


        /// <summary>
        /// Performs the isfinite function to the specified Matrix1x1 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix1x1<bool> isfinite(Matrix1x1 v)
        {
            return new Matrix1x1<bool>(!float.IsInfinity(v.M00));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix1x2 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix1x2<bool> isfinite(Matrix1x2 v)
        {
            return new Matrix1x2<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M01));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix1x3 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix1x3<bool> isfinite(Matrix1x3 v)
        {
            return new Matrix1x3<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M01), !float.IsInfinity(v.M02));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix1x4 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix1x4<bool> isfinite(Matrix1x4 v)
        {
            return new Matrix1x4<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M01), !float.IsInfinity(v.M02), !float.IsInfinity(v.M03));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix2x1 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix2x1<bool> isfinite(Matrix2x1 v)
        {
            return new Matrix2x1<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M10));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix2x2 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix2x2<bool> isfinite(Matrix2x2 v)
        {
            return new Matrix2x2<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M01), !float.IsInfinity(v.M10), !float.IsInfinity(v.M11));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix2x3 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix2x3<bool> isfinite(Matrix2x3 v)
        {
            return new Matrix2x3<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M01), !float.IsInfinity(v.M02), !float.IsInfinity(v.M10), !float.IsInfinity(v.M11), !float.IsInfinity(v.M12));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix2x4 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix2x4<bool> isfinite(Matrix2x4 v)
        {
            return new Matrix2x4<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M01), !float.IsInfinity(v.M02), !float.IsInfinity(v.M03), !float.IsInfinity(v.M10), !float.IsInfinity(v.M11), !float.IsInfinity(v.M12), !float.IsInfinity(v.M13));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix3x1 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix3x1<bool> isfinite(Matrix3x1 v)
        {
            return new Matrix3x1<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M10), !float.IsInfinity(v.M20));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix3x2 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix3x2<bool> isfinite(Matrix3x2 v)
        {
            return new Matrix3x2<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M01), !float.IsInfinity(v.M10), !float.IsInfinity(v.M11), !float.IsInfinity(v.M20), !float.IsInfinity(v.M21));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix3x3 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix3x3<bool> isfinite(Matrix3x3 v)
        {
            return new Matrix3x3<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M01), !float.IsInfinity(v.M02), !float.IsInfinity(v.M10), !float.IsInfinity(v.M11), !float.IsInfinity(v.M12), !float.IsInfinity(v.M20), !float.IsInfinity(v.M21), !float.IsInfinity(v.M22));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix3x4 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix3x4<bool> isfinite(Matrix3x4 v)
        {
            return new Matrix3x4<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M01), !float.IsInfinity(v.M02), !float.IsInfinity(v.M03), !float.IsInfinity(v.M10), !float.IsInfinity(v.M11), !float.IsInfinity(v.M12), !float.IsInfinity(v.M13), !float.IsInfinity(v.M20), !float.IsInfinity(v.M21), !float.IsInfinity(v.M22), !float.IsInfinity(v.M23));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix4x1 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix4x1<bool> isfinite(Matrix4x1 v)
        {
            return new Matrix4x1<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M10), !float.IsInfinity(v.M20), !float.IsInfinity(v.M30));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix4x2 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix4x2<bool> isfinite(Matrix4x2 v)
        {
            return new Matrix4x2<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M01), !float.IsInfinity(v.M10), !float.IsInfinity(v.M11), !float.IsInfinity(v.M20), !float.IsInfinity(v.M21), !float.IsInfinity(v.M30), !float.IsInfinity(v.M31));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix4x3 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix4x3<bool> isfinite(Matrix4x3 v)
        {
            return new Matrix4x3<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M01), !float.IsInfinity(v.M02), !float.IsInfinity(v.M10), !float.IsInfinity(v.M11), !float.IsInfinity(v.M12), !float.IsInfinity(v.M20), !float.IsInfinity(v.M21), !float.IsInfinity(v.M22), !float.IsInfinity(v.M30), !float.IsInfinity(v.M31), !float.IsInfinity(v.M32));
        }

        /// <summary>
        /// Performs the isfinite function to the specified Matrix4x4 object.
        /// Gets for each component if it is a finite value.
        /// </summary>
        public static Matrix4x4<bool> isfinite(Matrix4x4 v)
        {
            return new Matrix4x4<bool>(!float.IsInfinity(v.M00), !float.IsInfinity(v.M01), !float.IsInfinity(v.M02), !float.IsInfinity(v.M03), !float.IsInfinity(v.M10), !float.IsInfinity(v.M11), !float.IsInfinity(v.M12), !float.IsInfinity(v.M13), !float.IsInfinity(v.M20), !float.IsInfinity(v.M21), !float.IsInfinity(v.M22), !float.IsInfinity(v.M23), !float.IsInfinity(v.M30), !float.IsInfinity(v.M31), !float.IsInfinity(v.M32), !float.IsInfinity(v.M33));
        }

        #endregion
        #region isinf

        /// <summary>
        /// Performs the isinf function to the specified Vector1 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Vector1<bool> isinf(Vector1 v)
        {
            return new Vector1<bool>(float.IsInfinity(v.X));
        }

        /// <summary>
        /// Performs the isinf function to the specified Vector2 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Vector2<bool> isinf(Vector2 v)
        {
            return new Vector2<bool>(float.IsInfinity(v.X), float.IsInfinity(v.Y));
        }

        /// <summary>
        /// Performs the isinf function to the specified Vector3 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Vector3<bool> isinf(Vector3 v)
        {
            return new Vector3<bool>(float.IsInfinity(v.X), float.IsInfinity(v.Y), float.IsInfinity(v.Z));
        }

        /// <summary>
        /// Performs the isinf function to the specified Vector4 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Vector4<bool> isinf(Vector4 v)
        {
            return new Vector4<bool>(float.IsInfinity(v.X), float.IsInfinity(v.Y), float.IsInfinity(v.Z), float.IsInfinity(v.W));
        }


        /// <summary>
        /// Performs the isinf function to the specified Matrix1x1 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix1x1<bool> isinf(Matrix1x1 v)
        {
            return new Matrix1x1<bool>(float.IsInfinity(v.M00));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix1x2 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix1x2<bool> isinf(Matrix1x2 v)
        {
            return new Matrix1x2<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M01));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix1x3 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix1x3<bool> isinf(Matrix1x3 v)
        {
            return new Matrix1x3<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M01), float.IsInfinity(v.M02));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix1x4 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix1x4<bool> isinf(Matrix1x4 v)
        {
            return new Matrix1x4<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M01), float.IsInfinity(v.M02), float.IsInfinity(v.M03));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix2x1 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix2x1<bool> isinf(Matrix2x1 v)
        {
            return new Matrix2x1<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M10));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix2x2 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix2x2<bool> isinf(Matrix2x2 v)
        {
            return new Matrix2x2<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M01), float.IsInfinity(v.M10), float.IsInfinity(v.M11));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix2x3 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix2x3<bool> isinf(Matrix2x3 v)
        {
            return new Matrix2x3<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M01), float.IsInfinity(v.M02), float.IsInfinity(v.M10), float.IsInfinity(v.M11), float.IsInfinity(v.M12));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix2x4 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix2x4<bool> isinf(Matrix2x4 v)
        {
            return new Matrix2x4<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M01), float.IsInfinity(v.M02), float.IsInfinity(v.M03), float.IsInfinity(v.M10), float.IsInfinity(v.M11), float.IsInfinity(v.M12), float.IsInfinity(v.M13));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix3x1 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix3x1<bool> isinf(Matrix3x1 v)
        {
            return new Matrix3x1<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M10), float.IsInfinity(v.M20));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix3x2 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix3x2<bool> isinf(Matrix3x2 v)
        {
            return new Matrix3x2<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M01), float.IsInfinity(v.M10), float.IsInfinity(v.M11), float.IsInfinity(v.M20), float.IsInfinity(v.M21));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix3x3 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix3x3<bool> isinf(Matrix3x3 v)
        {
            return new Matrix3x3<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M01), float.IsInfinity(v.M02), float.IsInfinity(v.M10), float.IsInfinity(v.M11), float.IsInfinity(v.M12), float.IsInfinity(v.M20), float.IsInfinity(v.M21), float.IsInfinity(v.M22));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix3x4 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix3x4<bool> isinf(Matrix3x4 v)
        {
            return new Matrix3x4<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M01), float.IsInfinity(v.M02), float.IsInfinity(v.M03), float.IsInfinity(v.M10), float.IsInfinity(v.M11), float.IsInfinity(v.M12), float.IsInfinity(v.M13), float.IsInfinity(v.M20), float.IsInfinity(v.M21), float.IsInfinity(v.M22), float.IsInfinity(v.M23));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix4x1 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix4x1<bool> isinf(Matrix4x1 v)
        {
            return new Matrix4x1<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M10), float.IsInfinity(v.M20), float.IsInfinity(v.M30));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix4x2 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix4x2<bool> isinf(Matrix4x2 v)
        {
            return new Matrix4x2<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M01), float.IsInfinity(v.M10), float.IsInfinity(v.M11), float.IsInfinity(v.M20), float.IsInfinity(v.M21), float.IsInfinity(v.M30), float.IsInfinity(v.M31));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix4x3 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix4x3<bool> isinf(Matrix4x3 v)
        {
            return new Matrix4x3<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M01), float.IsInfinity(v.M02), float.IsInfinity(v.M10), float.IsInfinity(v.M11), float.IsInfinity(v.M12), float.IsInfinity(v.M20), float.IsInfinity(v.M21), float.IsInfinity(v.M22), float.IsInfinity(v.M30), float.IsInfinity(v.M31), float.IsInfinity(v.M32));
        }

        /// <summary>
        /// Performs the isinf function to the specified Matrix4x4 object.
        /// Gets for each component if it is an infinite value.
        /// </summary>
        public static Matrix4x4<bool> isinf(Matrix4x4 v)
        {
            return new Matrix4x4<bool>(float.IsInfinity(v.M00), float.IsInfinity(v.M01), float.IsInfinity(v.M02), float.IsInfinity(v.M03), float.IsInfinity(v.M10), float.IsInfinity(v.M11), float.IsInfinity(v.M12), float.IsInfinity(v.M13), float.IsInfinity(v.M20), float.IsInfinity(v.M21), float.IsInfinity(v.M22), float.IsInfinity(v.M23), float.IsInfinity(v.M30), float.IsInfinity(v.M31), float.IsInfinity(v.M32), float.IsInfinity(v.M33));
        }

        #endregion
        #region isnan

        /// <summary>
        /// Performs the isnan function to the specified Vector1 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Vector1<bool> isnan(Vector1 v)
        {
            return new Vector1<bool>(float.IsNaN(v.X));
        }

        /// <summary>
        /// Performs the isnan function to the specified Vector2 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Vector2<bool> isnan(Vector2 v)
        {
            return new Vector2<bool>(float.IsNaN(v.X), float.IsNaN(v.Y));
        }

        /// <summary>
        /// Performs the isnan function to the specified Vector3 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Vector3<bool> isnan(Vector3 v)
        {
            return new Vector3<bool>(float.IsNaN(v.X), float.IsNaN(v.Y), float.IsNaN(v.Z));
        }

        /// <summary>
        /// Performs the isnan function to the specified Vector4 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Vector4<bool> isnan(Vector4 v)
        {
            return new Vector4<bool>(float.IsNaN(v.X), float.IsNaN(v.Y), float.IsNaN(v.Z), float.IsNaN(v.W));
        }


        /// <summary>
        /// Performs the isnan function to the specified Matrix1x1 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix1x1<bool> isnan(Matrix1x1 v)
        {
            return new Matrix1x1<bool>(float.IsNaN(v.M00));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix1x2 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix1x2<bool> isnan(Matrix1x2 v)
        {
            return new Matrix1x2<bool>(float.IsNaN(v.M00), float.IsNaN(v.M01));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix1x3 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix1x3<bool> isnan(Matrix1x3 v)
        {
            return new Matrix1x3<bool>(float.IsNaN(v.M00), float.IsNaN(v.M01), float.IsNaN(v.M02));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix1x4 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix1x4<bool> isnan(Matrix1x4 v)
        {
            return new Matrix1x4<bool>(float.IsNaN(v.M00), float.IsNaN(v.M01), float.IsNaN(v.M02), float.IsNaN(v.M03));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix2x1 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix2x1<bool> isnan(Matrix2x1 v)
        {
            return new Matrix2x1<bool>(float.IsNaN(v.M00), float.IsNaN(v.M10));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix2x2 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix2x2<bool> isnan(Matrix2x2 v)
        {
            return new Matrix2x2<bool>(float.IsNaN(v.M00), float.IsNaN(v.M01), float.IsNaN(v.M10), float.IsNaN(v.M11));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix2x3 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix2x3<bool> isnan(Matrix2x3 v)
        {
            return new Matrix2x3<bool>(float.IsNaN(v.M00), float.IsNaN(v.M01), float.IsNaN(v.M02), float.IsNaN(v.M10), float.IsNaN(v.M11), float.IsNaN(v.M12));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix2x4 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix2x4<bool> isnan(Matrix2x4 v)
        {
            return new Matrix2x4<bool>(float.IsNaN(v.M00), float.IsNaN(v.M01), float.IsNaN(v.M02), float.IsNaN(v.M03), float.IsNaN(v.M10), float.IsNaN(v.M11), float.IsNaN(v.M12), float.IsNaN(v.M13));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix3x1 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix3x1<bool> isnan(Matrix3x1 v)
        {
            return new Matrix3x1<bool>(float.IsNaN(v.M00), float.IsNaN(v.M10), float.IsNaN(v.M20));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix3x2 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix3x2<bool> isnan(Matrix3x2 v)
        {
            return new Matrix3x2<bool>(float.IsNaN(v.M00), float.IsNaN(v.M01), float.IsNaN(v.M10), float.IsNaN(v.M11), float.IsNaN(v.M20), float.IsNaN(v.M21));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix3x3 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix3x3<bool> isnan(Matrix3x3 v)
        {
            return new Matrix3x3<bool>(float.IsNaN(v.M00), float.IsNaN(v.M01), float.IsNaN(v.M02), float.IsNaN(v.M10), float.IsNaN(v.M11), float.IsNaN(v.M12), float.IsNaN(v.M20), float.IsNaN(v.M21), float.IsNaN(v.M22));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix3x4 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix3x4<bool> isnan(Matrix3x4 v)
        {
            return new Matrix3x4<bool>(float.IsNaN(v.M00), float.IsNaN(v.M01), float.IsNaN(v.M02), float.IsNaN(v.M03), float.IsNaN(v.M10), float.IsNaN(v.M11), float.IsNaN(v.M12), float.IsNaN(v.M13), float.IsNaN(v.M20), float.IsNaN(v.M21), float.IsNaN(v.M22), float.IsNaN(v.M23));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix4x1 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix4x1<bool> isnan(Matrix4x1 v)
        {
            return new Matrix4x1<bool>(float.IsNaN(v.M00), float.IsNaN(v.M10), float.IsNaN(v.M20), float.IsNaN(v.M30));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix4x2 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix4x2<bool> isnan(Matrix4x2 v)
        {
            return new Matrix4x2<bool>(float.IsNaN(v.M00), float.IsNaN(v.M01), float.IsNaN(v.M10), float.IsNaN(v.M11), float.IsNaN(v.M20), float.IsNaN(v.M21), float.IsNaN(v.M30), float.IsNaN(v.M31));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix4x3 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix4x3<bool> isnan(Matrix4x3 v)
        {
            return new Matrix4x3<bool>(float.IsNaN(v.M00), float.IsNaN(v.M01), float.IsNaN(v.M02), float.IsNaN(v.M10), float.IsNaN(v.M11), float.IsNaN(v.M12), float.IsNaN(v.M20), float.IsNaN(v.M21), float.IsNaN(v.M22), float.IsNaN(v.M30), float.IsNaN(v.M31), float.IsNaN(v.M32));
        }

        /// <summary>
        /// Performs the isnan function to the specified Matrix4x4 object.
        /// Gets for each component if it is non a number.
        /// </summary>
        public static Matrix4x4<bool> isnan(Matrix4x4 v)
        {
            return new Matrix4x4<bool>(float.IsNaN(v.M00), float.IsNaN(v.M01), float.IsNaN(v.M02), float.IsNaN(v.M03), float.IsNaN(v.M10), float.IsNaN(v.M11), float.IsNaN(v.M12), float.IsNaN(v.M13), float.IsNaN(v.M20), float.IsNaN(v.M21), float.IsNaN(v.M22), float.IsNaN(v.M23), float.IsNaN(v.M30), float.IsNaN(v.M31), float.IsNaN(v.M32), float.IsNaN(v.M33));
        }

        #endregion
        #region ldexp

        /// <summary>
        /// Performs the ldexp function to the specified Vector1 objects.
        /// 
        /// </summary>
        public static float ldexp(Vector1 v1, Vector1 v2)
        {
            return (v1.X * (float)Math.Pow(2, v2.X));
        }

        /// <summary>
        /// Performs the ldexp function to the specified Vector2 objects.
        /// 
        /// </summary>
        public static float ldexp(Vector2 v1, Vector2 v2)
        {
            return (v1.X * (float)Math.Pow(2, v2.X) + v1.Y * (float)Math.Pow(2, v2.Y));
        }

        /// <summary>
        /// Performs the ldexp function to the specified Vector3 objects.
        /// 
        /// </summary>
        public static float ldexp(Vector3 v1, Vector3 v2)
        {
            return (v1.X * (float)Math.Pow(2, v2.X) + v1.Y * (float)Math.Pow(2, v2.Y) + v1.Z * (float)Math.Pow(2, v2.Z));
        }

        /// <summary>
        /// Performs the ldexp function to the specified Vector4 objects.
        /// 
        /// </summary>
        public static float ldexp(Vector4 v1, Vector4 v2)
        {
            return (v1.X * (float)Math.Pow(2, v2.X) + v1.Y * (float)Math.Pow(2, v2.Y) + v1.Z * (float)Math.Pow(2, v2.Z) + v1.W * (float)Math.Pow(2, v2.W));
        }

        #endregion
        #region length

        /// <summary>
        /// Performs the length function to the specified Vector1 object.
        /// Gets the length of the v vector.
        /// </summary>
        public static float length(Vector1 v)
        {

            return (float)Math.Sqrt(dot(v, v));

        }


        /// <summary>
        /// Performs the length function to the specified Vector2 object.
        /// Gets the length of the v vector.
        /// </summary>
        public static float length(Vector2 v)
        {

            return (float)Math.Sqrt(dot(v, v));

        }


        /// <summary>
        /// Performs the length function to the specified Vector3 object.
        /// Gets the length of the v vector.
        /// </summary>
        public static float length(Vector3 v)
        {

            return (float)Math.Sqrt(dot(v, v));

        }


        /// <summary>
        /// Performs the length function to the specified Vector4 object.
        /// Gets the length of the v vector.
        /// </summary>
        public static float length(Vector4 v)
        {

            return (float)Math.Sqrt(dot(v, v));

        }

        #endregion
        #region lerp

        /// <summary>
        /// Performs the lerp function to the specified Vector1 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Vector1 lerp(Vector1 v1, Vector1 v2, Vector1 s)
        {
            return new Vector1(v1.X + s.X * (v2.X - v1.X));
        }

        /// <summary>
        /// Performs the lerp function to the specified Vector2 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Vector2 lerp(Vector2 v1, Vector2 v2, Vector2 s)
        {
            return new Vector2(v1.X + s.X * (v2.X - v1.X), v1.Y + s.Y * (v2.Y - v1.Y));
        }

        /// <summary>
        /// Performs the lerp function to the specified Vector3 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Vector3 lerp(Vector3 v1, Vector3 v2, Vector3 s)
        {
            return new Vector3(v1.X + s.X * (v2.X - v1.X), v1.Y + s.Y * (v2.Y - v1.Y), v1.Z + s.Z * (v2.Z - v1.Z));
        }

        /// <summary>
        /// Performs the lerp function to the specified Vector4 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Vector4 lerp(Vector4 v1, Vector4 v2, Vector4 s)
        {
            return new Vector4(v1.X + s.X * (v2.X - v1.X), v1.Y + s.Y * (v2.Y - v1.Y), v1.Z + s.Z * (v2.Z - v1.Z), v1.W + s.W * (v2.W - v1.W));
        }


        /// <summary>
        /// Performs the lerp function to the specified Matrix1x1 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix1x1 lerp(Matrix1x1 v1, Matrix1x1 v2, Matrix1x1 s)
        {
            return new Matrix1x1(v1.M00 + s.M00 * (v2.M00 - v1.M00));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix1x2 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix1x2 lerp(Matrix1x2 v1, Matrix1x2 v2, Matrix1x2 s)
        {
            return new Matrix1x2(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M01 + s.M01 * (v2.M01 - v1.M01));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix1x3 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix1x3 lerp(Matrix1x3 v1, Matrix1x3 v2, Matrix1x3 s)
        {
            return new Matrix1x3(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M01 + s.M01 * (v2.M01 - v1.M01), v1.M02 + s.M02 * (v2.M02 - v1.M02));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix1x4 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix1x4 lerp(Matrix1x4 v1, Matrix1x4 v2, Matrix1x4 s)
        {
            return new Matrix1x4(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M01 + s.M01 * (v2.M01 - v1.M01), v1.M02 + s.M02 * (v2.M02 - v1.M02), v1.M03 + s.M03 * (v2.M03 - v1.M03));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix2x1 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix2x1 lerp(Matrix2x1 v1, Matrix2x1 v2, Matrix2x1 s)
        {
            return new Matrix2x1(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M10 + s.M10 * (v2.M10 - v1.M10));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix2x2 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix2x2 lerp(Matrix2x2 v1, Matrix2x2 v2, Matrix2x2 s)
        {
            return new Matrix2x2(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M01 + s.M01 * (v2.M01 - v1.M01), v1.M10 + s.M10 * (v2.M10 - v1.M10), v1.M11 + s.M11 * (v2.M11 - v1.M11));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix2x3 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix2x3 lerp(Matrix2x3 v1, Matrix2x3 v2, Matrix2x3 s)
        {
            return new Matrix2x3(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M01 + s.M01 * (v2.M01 - v1.M01), v1.M02 + s.M02 * (v2.M02 - v1.M02), v1.M10 + s.M10 * (v2.M10 - v1.M10), v1.M11 + s.M11 * (v2.M11 - v1.M11), v1.M12 + s.M12 * (v2.M12 - v1.M12));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix2x4 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix2x4 lerp(Matrix2x4 v1, Matrix2x4 v2, Matrix2x4 s)
        {
            return new Matrix2x4(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M01 + s.M01 * (v2.M01 - v1.M01), v1.M02 + s.M02 * (v2.M02 - v1.M02), v1.M03 + s.M03 * (v2.M03 - v1.M03), v1.M10 + s.M10 * (v2.M10 - v1.M10), v1.M11 + s.M11 * (v2.M11 - v1.M11), v1.M12 + s.M12 * (v2.M12 - v1.M12), v1.M13 + s.M13 * (v2.M13 - v1.M13));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix3x1 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix3x1 lerp(Matrix3x1 v1, Matrix3x1 v2, Matrix3x1 s)
        {
            return new Matrix3x1(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M10 + s.M10 * (v2.M10 - v1.M10), v1.M20 + s.M20 * (v2.M20 - v1.M20));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix3x2 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix3x2 lerp(Matrix3x2 v1, Matrix3x2 v2, Matrix3x2 s)
        {
            return new Matrix3x2(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M01 + s.M01 * (v2.M01 - v1.M01), v1.M10 + s.M10 * (v2.M10 - v1.M10), v1.M11 + s.M11 * (v2.M11 - v1.M11), v1.M20 + s.M20 * (v2.M20 - v1.M20), v1.M21 + s.M21 * (v2.M21 - v1.M21));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix3x3 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix3x3 lerp(Matrix3x3 v1, Matrix3x3 v2, Matrix3x3 s)
        {
            return new Matrix3x3(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M01 + s.M01 * (v2.M01 - v1.M01), v1.M02 + s.M02 * (v2.M02 - v1.M02), v1.M10 + s.M10 * (v2.M10 - v1.M10), v1.M11 + s.M11 * (v2.M11 - v1.M11), v1.M12 + s.M12 * (v2.M12 - v1.M12), v1.M20 + s.M20 * (v2.M20 - v1.M20), v1.M21 + s.M21 * (v2.M21 - v1.M21), v1.M22 + s.M22 * (v2.M22 - v1.M22));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix3x4 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix3x4 lerp(Matrix3x4 v1, Matrix3x4 v2, Matrix3x4 s)
        {
            return new Matrix3x4(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M01 + s.M01 * (v2.M01 - v1.M01), v1.M02 + s.M02 * (v2.M02 - v1.M02), v1.M03 + s.M03 * (v2.M03 - v1.M03), v1.M10 + s.M10 * (v2.M10 - v1.M10), v1.M11 + s.M11 * (v2.M11 - v1.M11), v1.M12 + s.M12 * (v2.M12 - v1.M12), v1.M13 + s.M13 * (v2.M13 - v1.M13), v1.M20 + s.M20 * (v2.M20 - v1.M20), v1.M21 + s.M21 * (v2.M21 - v1.M21), v1.M22 + s.M22 * (v2.M22 - v1.M22), v1.M23 + s.M23 * (v2.M23 - v1.M23));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix4x1 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix4x1 lerp(Matrix4x1 v1, Matrix4x1 v2, Matrix4x1 s)
        {
            return new Matrix4x1(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M10 + s.M10 * (v2.M10 - v1.M10), v1.M20 + s.M20 * (v2.M20 - v1.M20), v1.M30 + s.M30 * (v2.M30 - v1.M30));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix4x2 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix4x2 lerp(Matrix4x2 v1, Matrix4x2 v2, Matrix4x2 s)
        {
            return new Matrix4x2(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M01 + s.M01 * (v2.M01 - v1.M01), v1.M10 + s.M10 * (v2.M10 - v1.M10), v1.M11 + s.M11 * (v2.M11 - v1.M11), v1.M20 + s.M20 * (v2.M20 - v1.M20), v1.M21 + s.M21 * (v2.M21 - v1.M21), v1.M30 + s.M30 * (v2.M30 - v1.M30), v1.M31 + s.M31 * (v2.M31 - v1.M31));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix4x3 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix4x3 lerp(Matrix4x3 v1, Matrix4x3 v2, Matrix4x3 s)
        {
            return new Matrix4x3(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M01 + s.M01 * (v2.M01 - v1.M01), v1.M02 + s.M02 * (v2.M02 - v1.M02), v1.M10 + s.M10 * (v2.M10 - v1.M10), v1.M11 + s.M11 * (v2.M11 - v1.M11), v1.M12 + s.M12 * (v2.M12 - v1.M12), v1.M20 + s.M20 * (v2.M20 - v1.M20), v1.M21 + s.M21 * (v2.M21 - v1.M21), v1.M22 + s.M22 * (v2.M22 - v1.M22), v1.M30 + s.M30 * (v2.M30 - v1.M30), v1.M31 + s.M31 * (v2.M31 - v1.M31), v1.M32 + s.M32 * (v2.M32 - v1.M32));
        }

        /// <summary>
        /// Performs the lerp function to the specified Matrix4x4 objects.
        /// Gets the linnear interpolation between v1 and v2 using s. v1 + (v2 - v1)*s.
        /// </summary>
        public static Matrix4x4 lerp(Matrix4x4 v1, Matrix4x4 v2, Matrix4x4 s)
        {
            return new Matrix4x4(v1.M00 + s.M00 * (v2.M00 - v1.M00), v1.M01 + s.M01 * (v2.M01 - v1.M01), v1.M02 + s.M02 * (v2.M02 - v1.M02), v1.M03 + s.M03 * (v2.M03 - v1.M03), v1.M10 + s.M10 * (v2.M10 - v1.M10), v1.M11 + s.M11 * (v2.M11 - v1.M11), v1.M12 + s.M12 * (v2.M12 - v1.M12), v1.M13 + s.M13 * (v2.M13 - v1.M13), v1.M20 + s.M20 * (v2.M20 - v1.M20), v1.M21 + s.M21 * (v2.M21 - v1.M21), v1.M22 + s.M22 * (v2.M22 - v1.M22), v1.M23 + s.M23 * (v2.M23 - v1.M23), v1.M30 + s.M30 * (v2.M30 - v1.M30), v1.M31 + s.M31 * (v2.M31 - v1.M31), v1.M32 + s.M32 * (v2.M32 - v1.M32), v1.M33 + s.M33 * (v2.M33 - v1.M33));
        }

        #endregion
        #region lit
        public static Vector4 lit(float NdotL, float NdotH, float power)
        {
            {
                return new Vector4(1, NdotL < 0 ? 0 : NdotL, NdotL < 0 || NdotH < 0 ? 0 : (float)Math.Pow(NdotH, power), 1);
            }
        }
        #endregion
        #region log

        /// <summary>
        /// Performs the log function to the specified Vector1 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Vector1 log(Vector1 v)
        {
            return new Vector1((float)Math.Log(v.X));
        }

        /// <summary>
        /// Performs the log function to the specified Vector2 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Vector2 log(Vector2 v)
        {
            return new Vector2((float)Math.Log(v.X), (float)Math.Log(v.Y));
        }

        /// <summary>
        /// Performs the log function to the specified Vector3 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Vector3 log(Vector3 v)
        {
            return new Vector3((float)Math.Log(v.X), (float)Math.Log(v.Y), (float)Math.Log(v.Z));
        }

        /// <summary>
        /// Performs the log function to the specified Vector4 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Vector4 log(Vector4 v)
        {
            return new Vector4((float)Math.Log(v.X), (float)Math.Log(v.Y), (float)Math.Log(v.Z), (float)Math.Log(v.W));
        }


        /// <summary>
        /// Performs the log function to the specified Matrix1x1 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix1x1 log(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Log(v.M00));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix1x2 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix1x2 log(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Log(v.M00), (float)Math.Log(v.M01));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix1x3 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix1x3 log(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Log(v.M00), (float)Math.Log(v.M01), (float)Math.Log(v.M02));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix1x4 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix1x4 log(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Log(v.M00), (float)Math.Log(v.M01), (float)Math.Log(v.M02), (float)Math.Log(v.M03));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix2x1 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix2x1 log(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Log(v.M00), (float)Math.Log(v.M10));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix2x2 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix2x2 log(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Log(v.M00), (float)Math.Log(v.M01), (float)Math.Log(v.M10), (float)Math.Log(v.M11));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix2x3 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix2x3 log(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Log(v.M00), (float)Math.Log(v.M01), (float)Math.Log(v.M02), (float)Math.Log(v.M10), (float)Math.Log(v.M11), (float)Math.Log(v.M12));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix2x4 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix2x4 log(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Log(v.M00), (float)Math.Log(v.M01), (float)Math.Log(v.M02), (float)Math.Log(v.M03), (float)Math.Log(v.M10), (float)Math.Log(v.M11), (float)Math.Log(v.M12), (float)Math.Log(v.M13));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix3x1 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix3x1 log(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Log(v.M00), (float)Math.Log(v.M10), (float)Math.Log(v.M20));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix3x2 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix3x2 log(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Log(v.M00), (float)Math.Log(v.M01), (float)Math.Log(v.M10), (float)Math.Log(v.M11), (float)Math.Log(v.M20), (float)Math.Log(v.M21));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix3x3 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix3x3 log(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Log(v.M00), (float)Math.Log(v.M01), (float)Math.Log(v.M02), (float)Math.Log(v.M10), (float)Math.Log(v.M11), (float)Math.Log(v.M12), (float)Math.Log(v.M20), (float)Math.Log(v.M21), (float)Math.Log(v.M22));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix3x4 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix3x4 log(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Log(v.M00), (float)Math.Log(v.M01), (float)Math.Log(v.M02), (float)Math.Log(v.M03), (float)Math.Log(v.M10), (float)Math.Log(v.M11), (float)Math.Log(v.M12), (float)Math.Log(v.M13), (float)Math.Log(v.M20), (float)Math.Log(v.M21), (float)Math.Log(v.M22), (float)Math.Log(v.M23));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix4x1 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix4x1 log(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Log(v.M00), (float)Math.Log(v.M10), (float)Math.Log(v.M20), (float)Math.Log(v.M30));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix4x2 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix4x2 log(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Log(v.M00), (float)Math.Log(v.M01), (float)Math.Log(v.M10), (float)Math.Log(v.M11), (float)Math.Log(v.M20), (float)Math.Log(v.M21), (float)Math.Log(v.M30), (float)Math.Log(v.M31));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix4x3 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix4x3 log(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Log(v.M00), (float)Math.Log(v.M01), (float)Math.Log(v.M02), (float)Math.Log(v.M10), (float)Math.Log(v.M11), (float)Math.Log(v.M12), (float)Math.Log(v.M20), (float)Math.Log(v.M21), (float)Math.Log(v.M22), (float)Math.Log(v.M30), (float)Math.Log(v.M31), (float)Math.Log(v.M32));
        }

        /// <summary>
        /// Performs the log function to the specified Matrix4x4 object.
        /// Gets the base-e logarithm for each component.
        /// </summary>
        public static Matrix4x4 log(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Log(v.M00), (float)Math.Log(v.M01), (float)Math.Log(v.M02), (float)Math.Log(v.M03), (float)Math.Log(v.M10), (float)Math.Log(v.M11), (float)Math.Log(v.M12), (float)Math.Log(v.M13), (float)Math.Log(v.M20), (float)Math.Log(v.M21), (float)Math.Log(v.M22), (float)Math.Log(v.M23), (float)Math.Log(v.M30), (float)Math.Log(v.M31), (float)Math.Log(v.M32), (float)Math.Log(v.M33));
        }

        #endregion
        #region log10

        /// <summary>
        /// Performs the log10 function to the specified Vector1 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Vector1 log10(Vector1 v)
        {
            return new Vector1((float)Math.Log10(v.X));
        }

        /// <summary>
        /// Performs the log10 function to the specified Vector2 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Vector2 log10(Vector2 v)
        {
            return new Vector2((float)Math.Log10(v.X), (float)Math.Log10(v.Y));
        }

        /// <summary>
        /// Performs the log10 function to the specified Vector3 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Vector3 log10(Vector3 v)
        {
            return new Vector3((float)Math.Log10(v.X), (float)Math.Log10(v.Y), (float)Math.Log10(v.Z));
        }

        /// <summary>
        /// Performs the log10 function to the specified Vector4 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Vector4 log10(Vector4 v)
        {
            return new Vector4((float)Math.Log10(v.X), (float)Math.Log10(v.Y), (float)Math.Log10(v.Z), (float)Math.Log10(v.W));
        }


        /// <summary>
        /// Performs the log10 function to the specified Matrix1x1 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix1x1 log10(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Log10(v.M00));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix1x2 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix1x2 log10(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Log10(v.M00), (float)Math.Log10(v.M01));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix1x3 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix1x3 log10(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Log10(v.M00), (float)Math.Log10(v.M01), (float)Math.Log10(v.M02));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix1x4 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix1x4 log10(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Log10(v.M00), (float)Math.Log10(v.M01), (float)Math.Log10(v.M02), (float)Math.Log10(v.M03));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix2x1 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix2x1 log10(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Log10(v.M00), (float)Math.Log10(v.M10));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix2x2 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix2x2 log10(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Log10(v.M00), (float)Math.Log10(v.M01), (float)Math.Log10(v.M10), (float)Math.Log10(v.M11));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix2x3 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix2x3 log10(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Log10(v.M00), (float)Math.Log10(v.M01), (float)Math.Log10(v.M02), (float)Math.Log10(v.M10), (float)Math.Log10(v.M11), (float)Math.Log10(v.M12));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix2x4 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix2x4 log10(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Log10(v.M00), (float)Math.Log10(v.M01), (float)Math.Log10(v.M02), (float)Math.Log10(v.M03), (float)Math.Log10(v.M10), (float)Math.Log10(v.M11), (float)Math.Log10(v.M12), (float)Math.Log10(v.M13));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix3x1 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix3x1 log10(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Log10(v.M00), (float)Math.Log10(v.M10), (float)Math.Log10(v.M20));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix3x2 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix3x2 log10(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Log10(v.M00), (float)Math.Log10(v.M01), (float)Math.Log10(v.M10), (float)Math.Log10(v.M11), (float)Math.Log10(v.M20), (float)Math.Log10(v.M21));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix3x3 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix3x3 log10(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Log10(v.M00), (float)Math.Log10(v.M01), (float)Math.Log10(v.M02), (float)Math.Log10(v.M10), (float)Math.Log10(v.M11), (float)Math.Log10(v.M12), (float)Math.Log10(v.M20), (float)Math.Log10(v.M21), (float)Math.Log10(v.M22));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix3x4 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix3x4 log10(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Log10(v.M00), (float)Math.Log10(v.M01), (float)Math.Log10(v.M02), (float)Math.Log10(v.M03), (float)Math.Log10(v.M10), (float)Math.Log10(v.M11), (float)Math.Log10(v.M12), (float)Math.Log10(v.M13), (float)Math.Log10(v.M20), (float)Math.Log10(v.M21), (float)Math.Log10(v.M22), (float)Math.Log10(v.M23));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix4x1 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix4x1 log10(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Log10(v.M00), (float)Math.Log10(v.M10), (float)Math.Log10(v.M20), (float)Math.Log10(v.M30));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix4x2 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix4x2 log10(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Log10(v.M00), (float)Math.Log10(v.M01), (float)Math.Log10(v.M10), (float)Math.Log10(v.M11), (float)Math.Log10(v.M20), (float)Math.Log10(v.M21), (float)Math.Log10(v.M30), (float)Math.Log10(v.M31));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix4x3 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix4x3 log10(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Log10(v.M00), (float)Math.Log10(v.M01), (float)Math.Log10(v.M02), (float)Math.Log10(v.M10), (float)Math.Log10(v.M11), (float)Math.Log10(v.M12), (float)Math.Log10(v.M20), (float)Math.Log10(v.M21), (float)Math.Log10(v.M22), (float)Math.Log10(v.M30), (float)Math.Log10(v.M31), (float)Math.Log10(v.M32));
        }

        /// <summary>
        /// Performs the log10 function to the specified Matrix4x4 object.
        /// Gets the base-10 logarithm for each component.
        /// </summary>
        public static Matrix4x4 log10(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Log10(v.M00), (float)Math.Log10(v.M01), (float)Math.Log10(v.M02), (float)Math.Log10(v.M03), (float)Math.Log10(v.M10), (float)Math.Log10(v.M11), (float)Math.Log10(v.M12), (float)Math.Log10(v.M13), (float)Math.Log10(v.M20), (float)Math.Log10(v.M21), (float)Math.Log10(v.M22), (float)Math.Log10(v.M23), (float)Math.Log10(v.M30), (float)Math.Log10(v.M31), (float)Math.Log10(v.M32), (float)Math.Log10(v.M33));
        }

        #endregion
        #region log2

        /// <summary>
        /// Performs the log2 function to the specified Vector1 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Vector1 log2(Vector1 v)
        {
            return new Vector1((float)Math.Log(v.X, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Vector2 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Vector2 log2(Vector2 v)
        {
            return new Vector2((float)Math.Log(v.X, 2), (float)Math.Log(v.Y, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Vector3 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Vector3 log2(Vector3 v)
        {
            return new Vector3((float)Math.Log(v.X, 2), (float)Math.Log(v.Y, 2), (float)Math.Log(v.Z, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Vector4 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Vector4 log2(Vector4 v)
        {
            return new Vector4((float)Math.Log(v.X, 2), (float)Math.Log(v.Y, 2), (float)Math.Log(v.Z, 2), (float)Math.Log(v.W, 2));
        }


        /// <summary>
        /// Performs the log2 function to the specified Matrix1x1 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix1x1 log2(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Log(v.M00, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix1x2 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix1x2 log2(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Log(v.M00, 2), (float)Math.Log(v.M01, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix1x3 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix1x3 log2(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Log(v.M00, 2), (float)Math.Log(v.M01, 2), (float)Math.Log(v.M02, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix1x4 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix1x4 log2(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Log(v.M00, 2), (float)Math.Log(v.M01, 2), (float)Math.Log(v.M02, 2), (float)Math.Log(v.M03, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix2x1 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix2x1 log2(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Log(v.M00, 2), (float)Math.Log(v.M10, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix2x2 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix2x2 log2(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Log(v.M00, 2), (float)Math.Log(v.M01, 2), (float)Math.Log(v.M10, 2), (float)Math.Log(v.M11, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix2x3 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix2x3 log2(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Log(v.M00, 2), (float)Math.Log(v.M01, 2), (float)Math.Log(v.M02, 2), (float)Math.Log(v.M10, 2), (float)Math.Log(v.M11, 2), (float)Math.Log(v.M12, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix2x4 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix2x4 log2(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Log(v.M00, 2), (float)Math.Log(v.M01, 2), (float)Math.Log(v.M02, 2), (float)Math.Log(v.M03, 2), (float)Math.Log(v.M10, 2), (float)Math.Log(v.M11, 2), (float)Math.Log(v.M12, 2), (float)Math.Log(v.M13, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix3x1 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix3x1 log2(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Log(v.M00, 2), (float)Math.Log(v.M10, 2), (float)Math.Log(v.M20, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix3x2 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix3x2 log2(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Log(v.M00, 2), (float)Math.Log(v.M01, 2), (float)Math.Log(v.M10, 2), (float)Math.Log(v.M11, 2), (float)Math.Log(v.M20, 2), (float)Math.Log(v.M21, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix3x3 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix3x3 log2(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Log(v.M00, 2), (float)Math.Log(v.M01, 2), (float)Math.Log(v.M02, 2), (float)Math.Log(v.M10, 2), (float)Math.Log(v.M11, 2), (float)Math.Log(v.M12, 2), (float)Math.Log(v.M20, 2), (float)Math.Log(v.M21, 2), (float)Math.Log(v.M22, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix3x4 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix3x4 log2(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Log(v.M00, 2), (float)Math.Log(v.M01, 2), (float)Math.Log(v.M02, 2), (float)Math.Log(v.M03, 2), (float)Math.Log(v.M10, 2), (float)Math.Log(v.M11, 2), (float)Math.Log(v.M12, 2), (float)Math.Log(v.M13, 2), (float)Math.Log(v.M20, 2), (float)Math.Log(v.M21, 2), (float)Math.Log(v.M22, 2), (float)Math.Log(v.M23, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix4x1 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix4x1 log2(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Log(v.M00, 2), (float)Math.Log(v.M10, 2), (float)Math.Log(v.M20, 2), (float)Math.Log(v.M30, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix4x2 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix4x2 log2(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Log(v.M00, 2), (float)Math.Log(v.M01, 2), (float)Math.Log(v.M10, 2), (float)Math.Log(v.M11, 2), (float)Math.Log(v.M20, 2), (float)Math.Log(v.M21, 2), (float)Math.Log(v.M30, 2), (float)Math.Log(v.M31, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix4x3 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix4x3 log2(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Log(v.M00, 2), (float)Math.Log(v.M01, 2), (float)Math.Log(v.M02, 2), (float)Math.Log(v.M10, 2), (float)Math.Log(v.M11, 2), (float)Math.Log(v.M12, 2), (float)Math.Log(v.M20, 2), (float)Math.Log(v.M21, 2), (float)Math.Log(v.M22, 2), (float)Math.Log(v.M30, 2), (float)Math.Log(v.M31, 2), (float)Math.Log(v.M32, 2));
        }

        /// <summary>
        /// Performs the log2 function to the specified Matrix4x4 object.
        /// Gets the base-2 logarithm for each component.
        /// </summary>
        public static Matrix4x4 log2(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Log(v.M00, 2), (float)Math.Log(v.M01, 2), (float)Math.Log(v.M02, 2), (float)Math.Log(v.M03, 2), (float)Math.Log(v.M10, 2), (float)Math.Log(v.M11, 2), (float)Math.Log(v.M12, 2), (float)Math.Log(v.M13, 2), (float)Math.Log(v.M20, 2), (float)Math.Log(v.M21, 2), (float)Math.Log(v.M22, 2), (float)Math.Log(v.M23, 2), (float)Math.Log(v.M30, 2), (float)Math.Log(v.M31, 2), (float)Math.Log(v.M32, 2), (float)Math.Log(v.M33, 2));
        }

        #endregion
        #region max

        public static float max(float a, float b)
        {
            return Math.Max(a, b);
        }

        /// <summary>
        /// Performs the max function to the specified Vector1 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Vector1 max(Vector1 v1, Vector1 v2)
        {
            return new Vector1((float)Math.Max(v1.X, v2.X));
        }

        /// <summary>
        /// Performs the max function to the specified Vector2 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Vector2 max(Vector2 v1, Vector2 v2)
        {
            return new Vector2((float)Math.Max(v1.X, v2.X), (float)Math.Max(v1.Y, v2.Y));
        }

        /// <summary>
        /// Performs the max function to the specified Vector3 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Vector3 max(Vector3 v1, Vector3 v2)
        {
            return new Vector3((float)Math.Max(v1.X, v2.X), (float)Math.Max(v1.Y, v2.Y), (float)Math.Max(v1.Z, v2.Z));
        }

        /// <summary>
        /// Performs the max function to the specified Vector4 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Vector4 max(Vector4 v1, Vector4 v2)
        {
            return new Vector4((float)Math.Max(v1.X, v2.X), (float)Math.Max(v1.Y, v2.Y), (float)Math.Max(v1.Z, v2.Z), (float)Math.Max(v1.W, v2.W));
        }


        /// <summary>
        /// Performs the max function to the specified Matrix1x1 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix1x1 max(Matrix1x1 v1, Matrix1x1 v2)
        {
            return new Matrix1x1((float)Math.Max(v1.M00, v2.M00));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix1x2 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix1x2 max(Matrix1x2 v1, Matrix1x2 v2)
        {
            return new Matrix1x2((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M01, v2.M01));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix1x3 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix1x3 max(Matrix1x3 v1, Matrix1x3 v2)
        {
            return new Matrix1x3((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M01, v2.M01), (float)Math.Max(v1.M02, v2.M02));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix1x4 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix1x4 max(Matrix1x4 v1, Matrix1x4 v2)
        {
            return new Matrix1x4((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M01, v2.M01), (float)Math.Max(v1.M02, v2.M02), (float)Math.Max(v1.M03, v2.M03));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix2x1 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix2x1 max(Matrix2x1 v1, Matrix2x1 v2)
        {
            return new Matrix2x1((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M10, v2.M10));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix2x2 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix2x2 max(Matrix2x2 v1, Matrix2x2 v2)
        {
            return new Matrix2x2((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M01, v2.M01), (float)Math.Max(v1.M10, v2.M10), (float)Math.Max(v1.M11, v2.M11));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix2x3 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix2x3 max(Matrix2x3 v1, Matrix2x3 v2)
        {
            return new Matrix2x3((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M01, v2.M01), (float)Math.Max(v1.M02, v2.M02), (float)Math.Max(v1.M10, v2.M10), (float)Math.Max(v1.M11, v2.M11), (float)Math.Max(v1.M12, v2.M12));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix2x4 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix2x4 max(Matrix2x4 v1, Matrix2x4 v2)
        {
            return new Matrix2x4((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M01, v2.M01), (float)Math.Max(v1.M02, v2.M02), (float)Math.Max(v1.M03, v2.M03), (float)Math.Max(v1.M10, v2.M10), (float)Math.Max(v1.M11, v2.M11), (float)Math.Max(v1.M12, v2.M12), (float)Math.Max(v1.M13, v2.M13));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix3x1 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix3x1 max(Matrix3x1 v1, Matrix3x1 v2)
        {
            return new Matrix3x1((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M10, v2.M10), (float)Math.Max(v1.M20, v2.M20));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix3x2 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix3x2 max(Matrix3x2 v1, Matrix3x2 v2)
        {
            return new Matrix3x2((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M01, v2.M01), (float)Math.Max(v1.M10, v2.M10), (float)Math.Max(v1.M11, v2.M11), (float)Math.Max(v1.M20, v2.M20), (float)Math.Max(v1.M21, v2.M21));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix3x3 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix3x3 max(Matrix3x3 v1, Matrix3x3 v2)
        {
            return new Matrix3x3((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M01, v2.M01), (float)Math.Max(v1.M02, v2.M02), (float)Math.Max(v1.M10, v2.M10), (float)Math.Max(v1.M11, v2.M11), (float)Math.Max(v1.M12, v2.M12), (float)Math.Max(v1.M20, v2.M20), (float)Math.Max(v1.M21, v2.M21), (float)Math.Max(v1.M22, v2.M22));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix3x4 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix3x4 max(Matrix3x4 v1, Matrix3x4 v2)
        {
            return new Matrix3x4((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M01, v2.M01), (float)Math.Max(v1.M02, v2.M02), (float)Math.Max(v1.M03, v2.M03), (float)Math.Max(v1.M10, v2.M10), (float)Math.Max(v1.M11, v2.M11), (float)Math.Max(v1.M12, v2.M12), (float)Math.Max(v1.M13, v2.M13), (float)Math.Max(v1.M20, v2.M20), (float)Math.Max(v1.M21, v2.M21), (float)Math.Max(v1.M22, v2.M22), (float)Math.Max(v1.M23, v2.M23));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix4x1 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix4x1 max(Matrix4x1 v1, Matrix4x1 v2)
        {
            return new Matrix4x1((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M10, v2.M10), (float)Math.Max(v1.M20, v2.M20), (float)Math.Max(v1.M30, v2.M30));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix4x2 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix4x2 max(Matrix4x2 v1, Matrix4x2 v2)
        {
            return new Matrix4x2((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M01, v2.M01), (float)Math.Max(v1.M10, v2.M10), (float)Math.Max(v1.M11, v2.M11), (float)Math.Max(v1.M20, v2.M20), (float)Math.Max(v1.M21, v2.M21), (float)Math.Max(v1.M30, v2.M30), (float)Math.Max(v1.M31, v2.M31));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix4x3 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix4x3 max(Matrix4x3 v1, Matrix4x3 v2)
        {
            return new Matrix4x3((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M01, v2.M01), (float)Math.Max(v1.M02, v2.M02), (float)Math.Max(v1.M10, v2.M10), (float)Math.Max(v1.M11, v2.M11), (float)Math.Max(v1.M12, v2.M12), (float)Math.Max(v1.M20, v2.M20), (float)Math.Max(v1.M21, v2.M21), (float)Math.Max(v1.M22, v2.M22), (float)Math.Max(v1.M30, v2.M30), (float)Math.Max(v1.M31, v2.M31), (float)Math.Max(v1.M32, v2.M32));
        }

        /// <summary>
        /// Performs the max function to the specified Matrix4x4 objects.
        /// Gets the greater value between v1 and v2 components.
        /// </summary>
        public static Matrix4x4 max(Matrix4x4 v1, Matrix4x4 v2)
        {
            return new Matrix4x4((float)Math.Max(v1.M00, v2.M00), (float)Math.Max(v1.M01, v2.M01), (float)Math.Max(v1.M02, v2.M02), (float)Math.Max(v1.M03, v2.M03), (float)Math.Max(v1.M10, v2.M10), (float)Math.Max(v1.M11, v2.M11), (float)Math.Max(v1.M12, v2.M12), (float)Math.Max(v1.M13, v2.M13), (float)Math.Max(v1.M20, v2.M20), (float)Math.Max(v1.M21, v2.M21), (float)Math.Max(v1.M22, v2.M22), (float)Math.Max(v1.M23, v2.M23), (float)Math.Max(v1.M30, v2.M30), (float)Math.Max(v1.M31, v2.M31), (float)Math.Max(v1.M32, v2.M32), (float)Math.Max(v1.M33, v2.M33));
        }

        #endregion
        #region min

        public static float min(float v1, float v2)
        {
            return Math.Min(v1, v2);
        }

        /// <summary>
        /// Performs the min function to the specified Vector1 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Vector1 min(Vector1 v1, Vector1 v2)
        {
            return new Vector1((float)Math.Min(v1.X, v2.X));
        }

        /// <summary>
        /// Performs the min function to the specified Vector2 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Vector2 min(Vector2 v1, Vector2 v2)
        {
            return new Vector2((float)Math.Min(v1.X, v2.X), (float)Math.Min(v1.Y, v2.Y));
        }

        /// <summary>
        /// Performs the min function to the specified Vector3 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Vector3 min(Vector3 v1, Vector3 v2)
        {
            return new Vector3((float)Math.Min(v1.X, v2.X), (float)Math.Min(v1.Y, v2.Y), (float)Math.Min(v1.Z, v2.Z));
        }

        /// <summary>
        /// Performs the min function to the specified Vector4 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Vector4 min(Vector4 v1, Vector4 v2)
        {
            return new Vector4((float)Math.Min(v1.X, v2.X), (float)Math.Min(v1.Y, v2.Y), (float)Math.Min(v1.Z, v2.Z), (float)Math.Min(v1.W, v2.W));
        }


        /// <summary>
        /// Performs the min function to the specified Matrix1x1 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix1x1 min(Matrix1x1 v1, Matrix1x1 v2)
        {
            return new Matrix1x1((float)Math.Min(v1.M00, v2.M00));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix1x2 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix1x2 min(Matrix1x2 v1, Matrix1x2 v2)
        {
            return new Matrix1x2((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M01, v2.M01));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix1x3 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix1x3 min(Matrix1x3 v1, Matrix1x3 v2)
        {
            return new Matrix1x3((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M01, v2.M01), (float)Math.Min(v1.M02, v2.M02));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix1x4 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix1x4 min(Matrix1x4 v1, Matrix1x4 v2)
        {
            return new Matrix1x4((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M01, v2.M01), (float)Math.Min(v1.M02, v2.M02), (float)Math.Min(v1.M03, v2.M03));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix2x1 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix2x1 min(Matrix2x1 v1, Matrix2x1 v2)
        {
            return new Matrix2x1((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M10, v2.M10));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix2x2 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix2x2 min(Matrix2x2 v1, Matrix2x2 v2)
        {
            return new Matrix2x2((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M01, v2.M01), (float)Math.Min(v1.M10, v2.M10), (float)Math.Min(v1.M11, v2.M11));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix2x3 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix2x3 min(Matrix2x3 v1, Matrix2x3 v2)
        {
            return new Matrix2x3((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M01, v2.M01), (float)Math.Min(v1.M02, v2.M02), (float)Math.Min(v1.M10, v2.M10), (float)Math.Min(v1.M11, v2.M11), (float)Math.Min(v1.M12, v2.M12));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix2x4 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix2x4 min(Matrix2x4 v1, Matrix2x4 v2)
        {
            return new Matrix2x4((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M01, v2.M01), (float)Math.Min(v1.M02, v2.M02), (float)Math.Min(v1.M03, v2.M03), (float)Math.Min(v1.M10, v2.M10), (float)Math.Min(v1.M11, v2.M11), (float)Math.Min(v1.M12, v2.M12), (float)Math.Min(v1.M13, v2.M13));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix3x1 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix3x1 min(Matrix3x1 v1, Matrix3x1 v2)
        {
            return new Matrix3x1((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M10, v2.M10), (float)Math.Min(v1.M20, v2.M20));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix3x2 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix3x2 min(Matrix3x2 v1, Matrix3x2 v2)
        {
            return new Matrix3x2((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M01, v2.M01), (float)Math.Min(v1.M10, v2.M10), (float)Math.Min(v1.M11, v2.M11), (float)Math.Min(v1.M20, v2.M20), (float)Math.Min(v1.M21, v2.M21));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix3x3 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix3x3 min(Matrix3x3 v1, Matrix3x3 v2)
        {
            return new Matrix3x3((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M01, v2.M01), (float)Math.Min(v1.M02, v2.M02), (float)Math.Min(v1.M10, v2.M10), (float)Math.Min(v1.M11, v2.M11), (float)Math.Min(v1.M12, v2.M12), (float)Math.Min(v1.M20, v2.M20), (float)Math.Min(v1.M21, v2.M21), (float)Math.Min(v1.M22, v2.M22));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix3x4 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix3x4 min(Matrix3x4 v1, Matrix3x4 v2)
        {
            return new Matrix3x4((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M01, v2.M01), (float)Math.Min(v1.M02, v2.M02), (float)Math.Min(v1.M03, v2.M03), (float)Math.Min(v1.M10, v2.M10), (float)Math.Min(v1.M11, v2.M11), (float)Math.Min(v1.M12, v2.M12), (float)Math.Min(v1.M13, v2.M13), (float)Math.Min(v1.M20, v2.M20), (float)Math.Min(v1.M21, v2.M21), (float)Math.Min(v1.M22, v2.M22), (float)Math.Min(v1.M23, v2.M23));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix4x1 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix4x1 min(Matrix4x1 v1, Matrix4x1 v2)
        {
            return new Matrix4x1((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M10, v2.M10), (float)Math.Min(v1.M20, v2.M20), (float)Math.Min(v1.M30, v2.M30));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix4x2 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix4x2 min(Matrix4x2 v1, Matrix4x2 v2)
        {
            return new Matrix4x2((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M01, v2.M01), (float)Math.Min(v1.M10, v2.M10), (float)Math.Min(v1.M11, v2.M11), (float)Math.Min(v1.M20, v2.M20), (float)Math.Min(v1.M21, v2.M21), (float)Math.Min(v1.M30, v2.M30), (float)Math.Min(v1.M31, v2.M31));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix4x3 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix4x3 min(Matrix4x3 v1, Matrix4x3 v2)
        {
            return new Matrix4x3((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M01, v2.M01), (float)Math.Min(v1.M02, v2.M02), (float)Math.Min(v1.M10, v2.M10), (float)Math.Min(v1.M11, v2.M11), (float)Math.Min(v1.M12, v2.M12), (float)Math.Min(v1.M20, v2.M20), (float)Math.Min(v1.M21, v2.M21), (float)Math.Min(v1.M22, v2.M22), (float)Math.Min(v1.M30, v2.M30), (float)Math.Min(v1.M31, v2.M31), (float)Math.Min(v1.M32, v2.M32));
        }

        /// <summary>
        /// Performs the min function to the specified Matrix4x4 objects.
        /// Gets the smaller value between v1 and v2 components.
        /// </summary>
        public static Matrix4x4 min(Matrix4x4 v1, Matrix4x4 v2)
        {
            return new Matrix4x4((float)Math.Min(v1.M00, v2.M00), (float)Math.Min(v1.M01, v2.M01), (float)Math.Min(v1.M02, v2.M02), (float)Math.Min(v1.M03, v2.M03), (float)Math.Min(v1.M10, v2.M10), (float)Math.Min(v1.M11, v2.M11), (float)Math.Min(v1.M12, v2.M12), (float)Math.Min(v1.M13, v2.M13), (float)Math.Min(v1.M20, v2.M20), (float)Math.Min(v1.M21, v2.M21), (float)Math.Min(v1.M22, v2.M22), (float)Math.Min(v1.M23, v2.M23), (float)Math.Min(v1.M30, v2.M30), (float)Math.Min(v1.M31, v2.M31), (float)Math.Min(v1.M32, v2.M32), (float)Math.Min(v1.M33, v2.M33));
        }

        #endregion
        #region mul
        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x1 mul(Matrix1x1 m1, Matrix1x1 m2)
        {
            return new Matrix1x1(m1.M00 * (m2.M00));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x2 mul(Matrix1x1 m1, Matrix1x2 m2)
        {
            return new Matrix1x2(m1.M00 * (m2.M00), m1.M00 * (m2.M01));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x3 mul(Matrix1x1 m1, Matrix1x3 m2)
        {
            return new Matrix1x3(m1.M00 * (m2.M00), m1.M00 * (m2.M01), m1.M00 * (m2.M02));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x4 mul(Matrix1x1 m1, Matrix1x4 m2)
        {
            return new Matrix1x4(m1.M00 * (m2.M00), m1.M00 * (m2.M01), m1.M00 * (m2.M02), m1.M00 * (m2.M03));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x1 mul(Matrix1x2 m1, Matrix2x1 m2)
        {
            return new Matrix1x1(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x2 mul(Matrix1x2 m1, Matrix2x2 m2)
        {
            return new Matrix1x2(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x3 mul(Matrix1x2 m1, Matrix2x3 m2)
        {
            return new Matrix1x3(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x4 mul(Matrix1x2 m1, Matrix2x4 m2)
        {
            return new Matrix1x4(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)), m1.M00 * (m2.M03) + (m1.M01 * (m2.M13)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x1 mul(Matrix1x3 m1, Matrix3x1 m2)
        {
            return new Matrix1x1(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)));
        }

        /// <summary>
        /// Performs matrix multiplication between v and m.
        /// </summary>
        public static Vector1 mul(Vector3 v, Matrix3x1 m)
        {
            return new Vector1(v.X * (m.M00) + (v.Y * (m.M10)) + (v.Z * (m.M20)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x2 mul(Matrix1x3 m1, Matrix3x2 m2)
        {
            return new Matrix1x2(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)));
        }
        /// <summary>
        /// Performs matrix multiplication between v and m.
        /// </summary>
        public static Vector2 mul(Vector3 v, Matrix3x2 m)
        {
            return new Vector2(v.X * (m.M00) + (v.Y * (m.M10)) + (v.Z * (m.M20)), v.X * (m.M01) + (v.Y * (m.M11)) + (v.Z * (m.M21)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x3 mul(Matrix1x3 m1, Matrix3x3 m2)
        {
            return new Matrix1x3(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)));
        }

        /// <summary>
        /// Performs matrix multiplication between v and m.
        /// </summary>
        public static Vector3 mul(Vector3 v, Matrix3x3 m)
        {
            return new Vector3(v.X * (m.M00) + (v.Y * (m.M10)) + (v.Z * (m.M20)), v.X * (m.M01) + (v.Y * (m.M11)) + (v.Z * (m.M21)), v.X * (m.M02) + (v.Y * (m.M12)) + (v.Z * (m.M22)));
        }


        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x4 mul(Matrix1x3 m1, Matrix3x4 m2)
        {
            return new Matrix1x4(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)), m1.M00 * (m2.M03) + (m1.M01 * (m2.M13)) + (m1.M02 * (m2.M23)));
        }

        /// <summary>
        /// Performs matrix multiplication between v and m.
        /// </summary>
        public static Vector4 mul(Vector3 v, Matrix3x4 m)
        {
            return new Vector4(v.X * (m.M00) + (v.Y * (m.M10)) + (v.Z * (m.M20)), v.X * (m.M01) + (v.Y * (m.M11)) + (v.Z * (m.M21)), v.X * (m.M02) + (v.Y * (m.M12)) + (v.Z * (m.M22)), v.X * (m.M03) + (v.Y * (m.M13)) + (v.Z * (m.M23)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x1 mul(Matrix1x4 m1, Matrix4x1 m2)
        {
            return new Matrix1x1(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)));
        }
        /// <summary>
        /// Performs matrix multiplication between v and m.
        /// </summary>
        public static Vector1 mul(Vector4 v, Matrix4x1 m)
        {
            return new Vector1(v.X * (m.M00) + (v.Y * (m.M10)) + (v.Z * (m.M20)) + (v.W * (m.M30)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x2 mul(Matrix1x4 m1, Matrix4x2 m2)
        {
            return new Matrix1x2(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)) + (m1.M03 * (m2.M31)));
        }

        /// <summary>
        /// Performs matrix multiplication between v and m.
        /// </summary>
        public static Vector2 mul(Vector4 v, Matrix4x2 m)
        {
            return new Vector2(v.X * (m.M00) + (v.Y * (m.M10)) + (v.Z * (m.M20)) + (v.W * (m.M30)), v.X * (m.M01) + (v.Y * (m.M11)) + (v.Z * (m.M21)) + (v.W * (m.M31)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x3 mul(Matrix1x4 m1, Matrix4x3 m2)
        {
            return new Matrix1x3(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)) + (m1.M03 * (m2.M31)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)) + (m1.M03 * (m2.M32)));
        }

        /// <summary>
        /// Performs matrix multiplication between v and m.
        /// </summary>
        public static Vector3 mul(Vector4 v, Matrix4x3 m)
        {
            return new Vector3(v.X * (m.M00) + (v.Y * (m.M10)) + (v.Z * (m.M20)) + (v.W * (m.M30)), v.X * (m.M01) + (v.Y * (m.M11)) + (v.Z * (m.M21)) + (v.W * (m.M31)), v.X * (m.M02) + (v.Y * (m.M12)) + (v.Z * (m.M22)) + (v.W * (m.M32)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix1x4 mul(Matrix1x4 m1, Matrix4x4 m2)
        {
            return new Matrix1x4(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)) + (m1.M03 * (m2.M31)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)) + (m1.M03 * (m2.M32)), m1.M00 * (m2.M03) + (m1.M01 * (m2.M13)) + (m1.M02 * (m2.M23)) + (m1.M03 * (m2.M33)));
        }

        /// <summary>
        /// Performs matrix multiplication between a vector and a matrix.
        /// </summary>
        public static Vector4 mul(Vector4 v, Matrix4x4 m)
        {
            return new Vector4(v.X * (m.M00) + (v.Y * (m.M10)) + (v.Z * (m.M20)) + (v.W * (m.M30)), v.X * (m.M01) + (v.Y * (m.M11)) + (v.Z * (m.M21)) + (v.W * (m.M31)), v.X * (m.M02) + (v.Y * (m.M12)) + (v.Z * (m.M22)) + (v.W * (m.M32)), v.X * (m.M03) + (v.Y * (m.M13)) + (v.Z * (m.M23)) + (v.W * (m.M33)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x1 mul(Matrix2x1 m1, Matrix1x1 m2)
        {
            return new Matrix2x1(m1.M00 * (m2.M00), m1.M10 * (m2.M00));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x2 mul(Matrix2x1 m1, Matrix1x2 m2)
        {
            return new Matrix2x2(m1.M00 * (m2.M00), m1.M00 * (m2.M01), m1.M10 * (m2.M00), m1.M10 * (m2.M01));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x3 mul(Matrix2x1 m1, Matrix1x3 m2)
        {
            return new Matrix2x3(m1.M00 * (m2.M00), m1.M00 * (m2.M01), m1.M00 * (m2.M02), m1.M10 * (m2.M00), m1.M10 * (m2.M01), m1.M10 * (m2.M02));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x4 mul(Matrix2x1 m1, Matrix1x4 m2)
        {
            return new Matrix2x4(m1.M00 * (m2.M00), m1.M00 * (m2.M01), m1.M00 * (m2.M02), m1.M00 * (m2.M03), m1.M10 * (m2.M00), m1.M10 * (m2.M01), m1.M10 * (m2.M02), m1.M10 * (m2.M03));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x1 mul(Matrix2x2 m1, Matrix2x1 m2)
        {
            return new Matrix2x1(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x2 mul(Matrix2x2 m1, Matrix2x2 m2)
        {
            return new Matrix2x2(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x3 mul(Matrix2x2 m1, Matrix2x3 m2)
        {
            return new Matrix2x3(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x4 mul(Matrix2x2 m1, Matrix2x4 m2)
        {
            return new Matrix2x4(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)), m1.M00 * (m2.M03) + (m1.M01 * (m2.M13)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)), m1.M10 * (m2.M03) + (m1.M11 * (m2.M13)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x1 mul(Matrix2x3 m1, Matrix3x1 m2)
        {
            return new Matrix2x1(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x2 mul(Matrix2x3 m1, Matrix3x2 m2)
        {
            return new Matrix2x2(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x3 mul(Matrix2x3 m1, Matrix3x3 m2)
        {
            return new Matrix2x3(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)) + (m1.M12 * (m2.M22)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x4 mul(Matrix2x3 m1, Matrix3x4 m2)
        {
            return new Matrix2x4(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)), m1.M00 * (m2.M03) + (m1.M01 * (m2.M13)) + (m1.M02 * (m2.M23)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)) + (m1.M12 * (m2.M22)), m1.M10 * (m2.M03) + (m1.M11 * (m2.M13)) + (m1.M12 * (m2.M23)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x1 mul(Matrix2x4 m1, Matrix4x1 m2)
        {
            return new Matrix2x1(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)) + (m1.M13 * (m2.M30)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x2 mul(Matrix2x4 m1, Matrix4x2 m2)
        {
            return new Matrix2x2(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)) + (m1.M03 * (m2.M31)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)) + (m1.M13 * (m2.M30)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)) + (m1.M13 * (m2.M31)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x3 mul(Matrix2x4 m1, Matrix4x3 m2)
        {
            return new Matrix2x3(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)) + (m1.M03 * (m2.M31)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)) + (m1.M03 * (m2.M32)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)) + (m1.M13 * (m2.M30)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)) + (m1.M13 * (m2.M31)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)) + (m1.M12 * (m2.M22)) + (m1.M13 * (m2.M32)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix2x4 mul(Matrix2x4 m1, Matrix4x4 m2)
        {
            return new Matrix2x4(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)) + (m1.M03 * (m2.M31)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)) + (m1.M03 * (m2.M32)), m1.M00 * (m2.M03) + (m1.M01 * (m2.M13)) + (m1.M02 * (m2.M23)) + (m1.M03 * (m2.M33)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)) + (m1.M13 * (m2.M30)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)) + (m1.M13 * (m2.M31)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)) + (m1.M12 * (m2.M22)) + (m1.M13 * (m2.M32)), m1.M10 * (m2.M03) + (m1.M11 * (m2.M13)) + (m1.M12 * (m2.M23)) + (m1.M13 * (m2.M33)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x1 mul(Matrix3x1 m1, Matrix1x1 m2)
        {
            return new Matrix3x1(m1.M00 * (m2.M00), m1.M10 * (m2.M00), m1.M20 * (m2.M00));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x2 mul(Matrix3x1 m1, Matrix1x2 m2)
        {
            return new Matrix3x2(m1.M00 * (m2.M00), m1.M00 * (m2.M01), m1.M10 * (m2.M00), m1.M10 * (m2.M01), m1.M20 * (m2.M00), m1.M20 * (m2.M01));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x3 mul(Matrix3x1 m1, Matrix1x3 m2)
        {
            return new Matrix3x3(m1.M00 * (m2.M00), m1.M00 * (m2.M01), m1.M00 * (m2.M02), m1.M10 * (m2.M00), m1.M10 * (m2.M01), m1.M10 * (m2.M02), m1.M20 * (m2.M00), m1.M20 * (m2.M01), m1.M20 * (m2.M02));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x4 mul(Matrix3x1 m1, Matrix1x4 m2)
        {
            return new Matrix3x4(m1.M00 * (m2.M00), m1.M00 * (m2.M01), m1.M00 * (m2.M02), m1.M00 * (m2.M03), m1.M10 * (m2.M00), m1.M10 * (m2.M01), m1.M10 * (m2.M02), m1.M10 * (m2.M03), m1.M20 * (m2.M00), m1.M20 * (m2.M01), m1.M20 * (m2.M02), m1.M20 * (m2.M03));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x1 mul(Matrix3x2 m1, Matrix2x1 m2)
        {
            return new Matrix3x1(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x2 mul(Matrix3x2 m1, Matrix2x2 m2)
        {
            return new Matrix3x2(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x3 mul(Matrix3x2 m1, Matrix2x3 m2)
        {
            return new Matrix3x3(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)), m1.M20 * (m2.M02) + (m1.M21 * (m2.M12)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x4 mul(Matrix3x2 m1, Matrix2x4 m2)
        {
            return new Matrix3x4(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)), m1.M00 * (m2.M03) + (m1.M01 * (m2.M13)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)), m1.M10 * (m2.M03) + (m1.M11 * (m2.M13)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)), m1.M20 * (m2.M02) + (m1.M21 * (m2.M12)), m1.M20 * (m2.M03) + (m1.M21 * (m2.M13)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x1 mul(Matrix3x3 m1, Matrix3x1 m2)
        {
            return new Matrix3x1(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x2 mul(Matrix3x3 m1, Matrix3x2 m2)
        {
            return new Matrix3x2(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)) + (m1.M22 * (m2.M21)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x3 mul(Matrix3x3 m1, Matrix3x3 m2)
        {
            return new Matrix3x3(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)) + (m1.M12 * (m2.M22)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)) + (m1.M22 * (m2.M21)), m1.M20 * (m2.M02) + (m1.M21 * (m2.M12)) + (m1.M22 * (m2.M22)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x4 mul(Matrix3x3 m1, Matrix3x4 m2)
        {
            return new Matrix3x4(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)), m1.M00 * (m2.M03) + (m1.M01 * (m2.M13)) + (m1.M02 * (m2.M23)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)) + (m1.M12 * (m2.M22)), m1.M10 * (m2.M03) + (m1.M11 * (m2.M13)) + (m1.M12 * (m2.M23)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)) + (m1.M22 * (m2.M21)), m1.M20 * (m2.M02) + (m1.M21 * (m2.M12)) + (m1.M22 * (m2.M22)), m1.M20 * (m2.M03) + (m1.M21 * (m2.M13)) + (m1.M22 * (m2.M23)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x1 mul(Matrix3x4 m1, Matrix4x1 m2)
        {
            return new Matrix3x1(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)) + (m1.M13 * (m2.M30)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)) + (m1.M23 * (m2.M30)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x2 mul(Matrix3x4 m1, Matrix4x2 m2)
        {
            return new Matrix3x2(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)) + (m1.M03 * (m2.M31)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)) + (m1.M13 * (m2.M30)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)) + (m1.M13 * (m2.M31)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)) + (m1.M23 * (m2.M30)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)) + (m1.M22 * (m2.M21)) + (m1.M23 * (m2.M31)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x3 mul(Matrix3x4 m1, Matrix4x3 m2)
        {
            return new Matrix3x3(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)) + (m1.M03 * (m2.M31)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)) + (m1.M03 * (m2.M32)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)) + (m1.M13 * (m2.M30)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)) + (m1.M13 * (m2.M31)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)) + (m1.M12 * (m2.M22)) + (m1.M13 * (m2.M32)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)) + (m1.M23 * (m2.M30)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)) + (m1.M22 * (m2.M21)) + (m1.M23 * (m2.M31)), m1.M20 * (m2.M02) + (m1.M21 * (m2.M12)) + (m1.M22 * (m2.M22)) + (m1.M23 * (m2.M32)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix3x4 mul(Matrix3x4 m1, Matrix4x4 m2)
        {
            return new Matrix3x4(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)) + (m1.M03 * (m2.M31)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)) + (m1.M03 * (m2.M32)), m1.M00 * (m2.M03) + (m1.M01 * (m2.M13)) + (m1.M02 * (m2.M23)) + (m1.M03 * (m2.M33)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)) + (m1.M13 * (m2.M30)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)) + (m1.M13 * (m2.M31)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)) + (m1.M12 * (m2.M22)) + (m1.M13 * (m2.M32)), m1.M10 * (m2.M03) + (m1.M11 * (m2.M13)) + (m1.M12 * (m2.M23)) + (m1.M13 * (m2.M33)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)) + (m1.M23 * (m2.M30)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)) + (m1.M22 * (m2.M21)) + (m1.M23 * (m2.M31)), m1.M20 * (m2.M02) + (m1.M21 * (m2.M12)) + (m1.M22 * (m2.M22)) + (m1.M23 * (m2.M32)), m1.M20 * (m2.M03) + (m1.M21 * (m2.M13)) + (m1.M22 * (m2.M23)) + (m1.M23 * (m2.M33)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x1 mul(Matrix4x1 m1, Matrix1x1 m2)
        {
            return new Matrix4x1(m1.M00 * (m2.M00), m1.M10 * (m2.M00), m1.M20 * (m2.M00), m1.M30 * (m2.M00));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x2 mul(Matrix4x1 m1, Matrix1x2 m2)
        {
            return new Matrix4x2(m1.M00 * (m2.M00), m1.M00 * (m2.M01), m1.M10 * (m2.M00), m1.M10 * (m2.M01), m1.M20 * (m2.M00), m1.M20 * (m2.M01), m1.M30 * (m2.M00), m1.M30 * (m2.M01));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x3 mul(Matrix4x1 m1, Matrix1x3 m2)
        {
            return new Matrix4x3(m1.M00 * (m2.M00), m1.M00 * (m2.M01), m1.M00 * (m2.M02), m1.M10 * (m2.M00), m1.M10 * (m2.M01), m1.M10 * (m2.M02), m1.M20 * (m2.M00), m1.M20 * (m2.M01), m1.M20 * (m2.M02), m1.M30 * (m2.M00), m1.M30 * (m2.M01), m1.M30 * (m2.M02));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x4 mul(Matrix4x1 m1, Matrix1x4 m2)
        {
            return new Matrix4x4(m1.M00 * (m2.M00), m1.M00 * (m2.M01), m1.M00 * (m2.M02), m1.M00 * (m2.M03), m1.M10 * (m2.M00), m1.M10 * (m2.M01), m1.M10 * (m2.M02), m1.M10 * (m2.M03), m1.M20 * (m2.M00), m1.M20 * (m2.M01), m1.M20 * (m2.M02), m1.M20 * (m2.M03), m1.M30 * (m2.M00), m1.M30 * (m2.M01), m1.M30 * (m2.M02), m1.M30 * (m2.M03));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x1 mul(Matrix4x2 m1, Matrix2x1 m2)
        {
            return new Matrix4x1(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)), m1.M30 * (m2.M00) + (m1.M31 * (m2.M10)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x2 mul(Matrix4x2 m1, Matrix2x2 m2)
        {
            return new Matrix4x2(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)), m1.M30 * (m2.M00) + (m1.M31 * (m2.M10)), m1.M30 * (m2.M01) + (m1.M31 * (m2.M11)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x3 mul(Matrix4x2 m1, Matrix2x3 m2)
        {
            return new Matrix4x3(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)), m1.M20 * (m2.M02) + (m1.M21 * (m2.M12)), m1.M30 * (m2.M00) + (m1.M31 * (m2.M10)), m1.M30 * (m2.M01) + (m1.M31 * (m2.M11)), m1.M30 * (m2.M02) + (m1.M31 * (m2.M12)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x4 mul(Matrix4x2 m1, Matrix2x4 m2)
        {
            return new Matrix4x4(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)), m1.M00 * (m2.M03) + (m1.M01 * (m2.M13)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)), m1.M10 * (m2.M03) + (m1.M11 * (m2.M13)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)), m1.M20 * (m2.M02) + (m1.M21 * (m2.M12)), m1.M20 * (m2.M03) + (m1.M21 * (m2.M13)), m1.M30 * (m2.M00) + (m1.M31 * (m2.M10)), m1.M30 * (m2.M01) + (m1.M31 * (m2.M11)), m1.M30 * (m2.M02) + (m1.M31 * (m2.M12)), m1.M30 * (m2.M03) + (m1.M31 * (m2.M13)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x1 mul(Matrix4x3 m1, Matrix3x1 m2)
        {
            return new Matrix4x1(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)), m1.M30 * (m2.M00) + (m1.M31 * (m2.M10)) + (m1.M32 * (m2.M20)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x2 mul(Matrix4x3 m1, Matrix3x2 m2)
        {
            return new Matrix4x2(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)) + (m1.M22 * (m2.M21)), m1.M30 * (m2.M00) + (m1.M31 * (m2.M10)) + (m1.M32 * (m2.M20)), m1.M30 * (m2.M01) + (m1.M31 * (m2.M11)) + (m1.M32 * (m2.M21)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x3 mul(Matrix4x3 m1, Matrix3x3 m2)
        {
            return new Matrix4x3(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)) + (m1.M12 * (m2.M22)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)) + (m1.M22 * (m2.M21)), m1.M20 * (m2.M02) + (m1.M21 * (m2.M12)) + (m1.M22 * (m2.M22)), m1.M30 * (m2.M00) + (m1.M31 * (m2.M10)) + (m1.M32 * (m2.M20)), m1.M30 * (m2.M01) + (m1.M31 * (m2.M11)) + (m1.M32 * (m2.M21)), m1.M30 * (m2.M02) + (m1.M31 * (m2.M12)) + (m1.M32 * (m2.M22)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x4 mul(Matrix4x3 m1, Matrix3x4 m2)
        {
            return new Matrix4x4(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)), m1.M00 * (m2.M03) + (m1.M01 * (m2.M13)) + (m1.M02 * (m2.M23)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)) + (m1.M12 * (m2.M22)), m1.M10 * (m2.M03) + (m1.M11 * (m2.M13)) + (m1.M12 * (m2.M23)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)) + (m1.M22 * (m2.M21)), m1.M20 * (m2.M02) + (m1.M21 * (m2.M12)) + (m1.M22 * (m2.M22)), m1.M20 * (m2.M03) + (m1.M21 * (m2.M13)) + (m1.M22 * (m2.M23)), m1.M30 * (m2.M00) + (m1.M31 * (m2.M10)) + (m1.M32 * (m2.M20)), m1.M30 * (m2.M01) + (m1.M31 * (m2.M11)) + (m1.M32 * (m2.M21)), m1.M30 * (m2.M02) + (m1.M31 * (m2.M12)) + (m1.M32 * (m2.M22)), m1.M30 * (m2.M03) + (m1.M31 * (m2.M13)) + (m1.M32 * (m2.M23)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x1 mul(Matrix4x4 m1, Matrix4x1 m2)
        {
            return new Matrix4x1(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)) + (m1.M13 * (m2.M30)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)) + (m1.M23 * (m2.M30)), m1.M30 * (m2.M00) + (m1.M31 * (m2.M10)) + (m1.M32 * (m2.M20)) + (m1.M33 * (m2.M30)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x2 mul(Matrix4x4 m1, Matrix4x2 m2)
        {
            return new Matrix4x2(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)) + (m1.M03 * (m2.M31)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)) + (m1.M13 * (m2.M30)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)) + (m1.M13 * (m2.M31)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)) + (m1.M23 * (m2.M30)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)) + (m1.M22 * (m2.M21)) + (m1.M23 * (m2.M31)), m1.M30 * (m2.M00) + (m1.M31 * (m2.M10)) + (m1.M32 * (m2.M20)) + (m1.M33 * (m2.M30)), m1.M30 * (m2.M01) + (m1.M31 * (m2.M11)) + (m1.M32 * (m2.M21)) + (m1.M33 * (m2.M31)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x3 mul(Matrix4x4 m1, Matrix4x3 m2)
        {
            return new Matrix4x3(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)) + (m1.M03 * (m2.M31)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)) + (m1.M03 * (m2.M32)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)) + (m1.M13 * (m2.M30)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)) + (m1.M13 * (m2.M31)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)) + (m1.M12 * (m2.M22)) + (m1.M13 * (m2.M32)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)) + (m1.M23 * (m2.M30)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)) + (m1.M22 * (m2.M21)) + (m1.M23 * (m2.M31)), m1.M20 * (m2.M02) + (m1.M21 * (m2.M12)) + (m1.M22 * (m2.M22)) + (m1.M23 * (m2.M32)), m1.M30 * (m2.M00) + (m1.M31 * (m2.M10)) + (m1.M32 * (m2.M20)) + (m1.M33 * (m2.M30)), m1.M30 * (m2.M01) + (m1.M31 * (m2.M11)) + (m1.M32 * (m2.M21)) + (m1.M33 * (m2.M31)), m1.M30 * (m2.M02) + (m1.M31 * (m2.M12)) + (m1.M32 * (m2.M22)) + (m1.M33 * (m2.M32)));
        }

        /// <summary>
        /// Performs matrix multiplication between m1 and m2.
        /// </summary>
        public static Matrix4x4 mul(Matrix4x4 m1, Matrix4x4 m2)
        {
            return new Matrix4x4(m1.M00 * (m2.M00) + (m1.M01 * (m2.M10)) + (m1.M02 * (m2.M20)) + (m1.M03 * (m2.M30)), m1.M00 * (m2.M01) + (m1.M01 * (m2.M11)) + (m1.M02 * (m2.M21)) + (m1.M03 * (m2.M31)), m1.M00 * (m2.M02) + (m1.M01 * (m2.M12)) + (m1.M02 * (m2.M22)) + (m1.M03 * (m2.M32)), m1.M00 * (m2.M03) + (m1.M01 * (m2.M13)) + (m1.M02 * (m2.M23)) + (m1.M03 * (m2.M33)), m1.M10 * (m2.M00) + (m1.M11 * (m2.M10)) + (m1.M12 * (m2.M20)) + (m1.M13 * (m2.M30)), m1.M10 * (m2.M01) + (m1.M11 * (m2.M11)) + (m1.M12 * (m2.M21)) + (m1.M13 * (m2.M31)), m1.M10 * (m2.M02) + (m1.M11 * (m2.M12)) + (m1.M12 * (m2.M22)) + (m1.M13 * (m2.M32)), m1.M10 * (m2.M03) + (m1.M11 * (m2.M13)) + (m1.M12 * (m2.M23)) + (m1.M13 * (m2.M33)), m1.M20 * (m2.M00) + (m1.M21 * (m2.M10)) + (m1.M22 * (m2.M20)) + (m1.M23 * (m2.M30)), m1.M20 * (m2.M01) + (m1.M21 * (m2.M11)) + (m1.M22 * (m2.M21)) + (m1.M23 * (m2.M31)), m1.M20 * (m2.M02) + (m1.M21 * (m2.M12)) + (m1.M22 * (m2.M22)) + (m1.M23 * (m2.M32)), m1.M20 * (m2.M03) + (m1.M21 * (m2.M13)) + (m1.M22 * (m2.M23)) + (m1.M23 * (m2.M33)), m1.M30 * (m2.M00) + (m1.M31 * (m2.M10)) + (m1.M32 * (m2.M20)) + (m1.M33 * (m2.M30)), m1.M30 * (m2.M01) + (m1.M31 * (m2.M11)) + (m1.M32 * (m2.M21)) + (m1.M33 * (m2.M31)), m1.M30 * (m2.M02) + (m1.M31 * (m2.M12)) + (m1.M32 * (m2.M22)) + (m1.M33 * (m2.M32)), m1.M30 * (m2.M03) + (m1.M31 * (m2.M13)) + (m1.M32 * (m2.M23)) + (m1.M33 * (m2.M33)));
        }

        #endregion
        #region normalize

        /// <summary>
        /// Performs the normalize function to the specified Vector1 object.
        /// Gets the normalized vector.
        /// </summary>
        public static Vector1 normalize(Vector1 v)
        {

            return v / (float)Math.Sqrt(dot(v, v));

        }


        /// <summary>
        /// Performs the normalize function to the specified Vector2 object.
        /// Gets the normalized vector.
        /// </summary>
        public static Vector2 normalize(Vector2 v)
        {

            return v / (float)Math.Sqrt(dot(v, v));

        }


        /// <summary>
        /// Performs the normalize function to the specified Vector3 object.
        /// Gets the normalized vector.
        /// </summary>
        public static Vector3 normalize(Vector3 v)
        {
            float dot = GMath.dot(v, v);
            if (dot == 0) return new Vector3();

            return v / (float)Math.Sqrt(dot);
        }


        /// <summary>
        /// Performs the normalize function to the specified Vector4 object.
        /// Gets the normalized vector.
        /// </summary>
        public static Vector4 normalize(Vector4 v)
        {

            return v / (float)Math.Sqrt(dot(v, v));

        }

        #endregion
        #region pow

        /// <summary>
        /// Performs the pow function to the specified values.
        /// Gets the power of a to b.
        /// </summary>
        public static float pow(float a, float b)
        {
            return ((float)Math.Pow(a, b));
        }


        /// <summary>
        /// Performs the pow function to the specified Vector1 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Vector1 pow(Vector1 v1, Vector1 v2)
        {
            return new Vector1((float)Math.Pow(v1.X, v2.X));
        }

        /// <summary>
        /// Performs the pow function to the specified Vector2 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Vector2 pow(Vector2 v1, Vector2 v2)
        {
            return new Vector2((float)Math.Pow(v1.X, v2.X), (float)Math.Pow(v1.Y, v2.Y));
        }

        /// <summary>
        /// Performs the pow function to the specified Vector3 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Vector3 pow(Vector3 v1, Vector3 v2)
        {
            return new Vector3((float)Math.Pow(v1.X, v2.X), (float)Math.Pow(v1.Y, v2.Y), (float)Math.Pow(v1.Z, v2.Z));
        }

        /// <summary>
        /// Performs the pow function to the specified Vector4 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Vector4 pow(Vector4 v1, Vector4 v2)
        {
            return new Vector4((float)Math.Pow(v1.X, v2.X), (float)Math.Pow(v1.Y, v2.Y), (float)Math.Pow(v1.Z, v2.Z), (float)Math.Pow(v1.W, v2.W));
        }


        /// <summary>
        /// Performs the pow function to the specified Matrix1x1 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix1x1 pow(Matrix1x1 v1, Matrix1x1 v2)
        {
            return new Matrix1x1((float)Math.Pow(v1.M00, v2.M00));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix1x2 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix1x2 pow(Matrix1x2 v1, Matrix1x2 v2)
        {
            return new Matrix1x2((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M01, v2.M01));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix1x3 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix1x3 pow(Matrix1x3 v1, Matrix1x3 v2)
        {
            return new Matrix1x3((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M01, v2.M01), (float)Math.Pow(v1.M02, v2.M02));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix1x4 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix1x4 pow(Matrix1x4 v1, Matrix1x4 v2)
        {
            return new Matrix1x4((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M01, v2.M01), (float)Math.Pow(v1.M02, v2.M02), (float)Math.Pow(v1.M03, v2.M03));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix2x1 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix2x1 pow(Matrix2x1 v1, Matrix2x1 v2)
        {
            return new Matrix2x1((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M10, v2.M10));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix2x2 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix2x2 pow(Matrix2x2 v1, Matrix2x2 v2)
        {
            return new Matrix2x2((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M01, v2.M01), (float)Math.Pow(v1.M10, v2.M10), (float)Math.Pow(v1.M11, v2.M11));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix2x3 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix2x3 pow(Matrix2x3 v1, Matrix2x3 v2)
        {
            return new Matrix2x3((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M01, v2.M01), (float)Math.Pow(v1.M02, v2.M02), (float)Math.Pow(v1.M10, v2.M10), (float)Math.Pow(v1.M11, v2.M11), (float)Math.Pow(v1.M12, v2.M12));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix2x4 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix2x4 pow(Matrix2x4 v1, Matrix2x4 v2)
        {
            return new Matrix2x4((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M01, v2.M01), (float)Math.Pow(v1.M02, v2.M02), (float)Math.Pow(v1.M03, v2.M03), (float)Math.Pow(v1.M10, v2.M10), (float)Math.Pow(v1.M11, v2.M11), (float)Math.Pow(v1.M12, v2.M12), (float)Math.Pow(v1.M13, v2.M13));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix3x1 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix3x1 pow(Matrix3x1 v1, Matrix3x1 v2)
        {
            return new Matrix3x1((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M10, v2.M10), (float)Math.Pow(v1.M20, v2.M20));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix3x2 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix3x2 pow(Matrix3x2 v1, Matrix3x2 v2)
        {
            return new Matrix3x2((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M01, v2.M01), (float)Math.Pow(v1.M10, v2.M10), (float)Math.Pow(v1.M11, v2.M11), (float)Math.Pow(v1.M20, v2.M20), (float)Math.Pow(v1.M21, v2.M21));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix3x3 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix3x3 pow(Matrix3x3 v1, Matrix3x3 v2)
        {
            return new Matrix3x3((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M01, v2.M01), (float)Math.Pow(v1.M02, v2.M02), (float)Math.Pow(v1.M10, v2.M10), (float)Math.Pow(v1.M11, v2.M11), (float)Math.Pow(v1.M12, v2.M12), (float)Math.Pow(v1.M20, v2.M20), (float)Math.Pow(v1.M21, v2.M21), (float)Math.Pow(v1.M22, v2.M22));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix3x4 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix3x4 pow(Matrix3x4 v1, Matrix3x4 v2)
        {
            return new Matrix3x4((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M01, v2.M01), (float)Math.Pow(v1.M02, v2.M02), (float)Math.Pow(v1.M03, v2.M03), (float)Math.Pow(v1.M10, v2.M10), (float)Math.Pow(v1.M11, v2.M11), (float)Math.Pow(v1.M12, v2.M12), (float)Math.Pow(v1.M13, v2.M13), (float)Math.Pow(v1.M20, v2.M20), (float)Math.Pow(v1.M21, v2.M21), (float)Math.Pow(v1.M22, v2.M22), (float)Math.Pow(v1.M23, v2.M23));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix4x1 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix4x1 pow(Matrix4x1 v1, Matrix4x1 v2)
        {
            return new Matrix4x1((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M10, v2.M10), (float)Math.Pow(v1.M20, v2.M20), (float)Math.Pow(v1.M30, v2.M30));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix4x2 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix4x2 pow(Matrix4x2 v1, Matrix4x2 v2)
        {
            return new Matrix4x2((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M01, v2.M01), (float)Math.Pow(v1.M10, v2.M10), (float)Math.Pow(v1.M11, v2.M11), (float)Math.Pow(v1.M20, v2.M20), (float)Math.Pow(v1.M21, v2.M21), (float)Math.Pow(v1.M30, v2.M30), (float)Math.Pow(v1.M31, v2.M31));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix4x3 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix4x3 pow(Matrix4x3 v1, Matrix4x3 v2)
        {
            return new Matrix4x3((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M01, v2.M01), (float)Math.Pow(v1.M02, v2.M02), (float)Math.Pow(v1.M10, v2.M10), (float)Math.Pow(v1.M11, v2.M11), (float)Math.Pow(v1.M12, v2.M12), (float)Math.Pow(v1.M20, v2.M20), (float)Math.Pow(v1.M21, v2.M21), (float)Math.Pow(v1.M22, v2.M22), (float)Math.Pow(v1.M30, v2.M30), (float)Math.Pow(v1.M31, v2.M31), (float)Math.Pow(v1.M32, v2.M32));
        }

        /// <summary>
        /// Performs the pow function to the specified Matrix4x4 objects.
        /// Gets the power of v1 to v2 components.
        /// </summary>
        public static Matrix4x4 pow(Matrix4x4 v1, Matrix4x4 v2)
        {
            return new Matrix4x4((float)Math.Pow(v1.M00, v2.M00), (float)Math.Pow(v1.M01, v2.M01), (float)Math.Pow(v1.M02, v2.M02), (float)Math.Pow(v1.M03, v2.M03), (float)Math.Pow(v1.M10, v2.M10), (float)Math.Pow(v1.M11, v2.M11), (float)Math.Pow(v1.M12, v2.M12), (float)Math.Pow(v1.M13, v2.M13), (float)Math.Pow(v1.M20, v2.M20), (float)Math.Pow(v1.M21, v2.M21), (float)Math.Pow(v1.M22, v2.M22), (float)Math.Pow(v1.M23, v2.M23), (float)Math.Pow(v1.M30, v2.M30), (float)Math.Pow(v1.M31, v2.M31), (float)Math.Pow(v1.M32, v2.M32), (float)Math.Pow(v1.M33, v2.M33));
        }

        #endregion
        #region radians

        public static float radians(float x)
        {
            return (float)(x * Math.PI / 180);
        }

        /// <summary>
        /// Performs the radians function to the specified Vector1 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Vector1 radians(Vector1 v)
        {
            return new Vector1((float)(v.X * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Vector2 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Vector2 radians(Vector2 v)
        {
            return new Vector2((float)(v.X * Math.PI / 180), (float)(v.Y * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Vector3 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Vector3 radians(Vector3 v)
        {
            return new Vector3((float)(v.X * Math.PI / 180), (float)(v.Y * Math.PI / 180), (float)(v.Z * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Vector4 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Vector4 radians(Vector4 v)
        {
            return new Vector4((float)(v.X * Math.PI / 180), (float)(v.Y * Math.PI / 180), (float)(v.Z * Math.PI / 180), (float)(v.W * Math.PI / 180));
        }


        /// <summary>
        /// Performs the radians function to the specified Matrix1x1 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix1x1 radians(Matrix1x1 v)
        {
            return new Matrix1x1((float)(v.M00 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix1x2 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix1x2 radians(Matrix1x2 v)
        {
            return new Matrix1x2((float)(v.M00 * Math.PI / 180), (float)(v.M01 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix1x3 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix1x3 radians(Matrix1x3 v)
        {
            return new Matrix1x3((float)(v.M00 * Math.PI / 180), (float)(v.M01 * Math.PI / 180), (float)(v.M02 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix1x4 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix1x4 radians(Matrix1x4 v)
        {
            return new Matrix1x4((float)(v.M00 * Math.PI / 180), (float)(v.M01 * Math.PI / 180), (float)(v.M02 * Math.PI / 180), (float)(v.M03 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix2x1 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix2x1 radians(Matrix2x1 v)
        {
            return new Matrix2x1((float)(v.M00 * Math.PI / 180), (float)(v.M10 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix2x2 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix2x2 radians(Matrix2x2 v)
        {
            return new Matrix2x2((float)(v.M00 * Math.PI / 180), (float)(v.M01 * Math.PI / 180), (float)(v.M10 * Math.PI / 180), (float)(v.M11 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix2x3 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix2x3 radians(Matrix2x3 v)
        {
            return new Matrix2x3((float)(v.M00 * Math.PI / 180), (float)(v.M01 * Math.PI / 180), (float)(v.M02 * Math.PI / 180), (float)(v.M10 * Math.PI / 180), (float)(v.M11 * Math.PI / 180), (float)(v.M12 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix2x4 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix2x4 radians(Matrix2x4 v)
        {
            return new Matrix2x4((float)(v.M00 * Math.PI / 180), (float)(v.M01 * Math.PI / 180), (float)(v.M02 * Math.PI / 180), (float)(v.M03 * Math.PI / 180), (float)(v.M10 * Math.PI / 180), (float)(v.M11 * Math.PI / 180), (float)(v.M12 * Math.PI / 180), (float)(v.M13 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix3x1 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix3x1 radians(Matrix3x1 v)
        {
            return new Matrix3x1((float)(v.M00 * Math.PI / 180), (float)(v.M10 * Math.PI / 180), (float)(v.M20 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix3x2 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix3x2 radians(Matrix3x2 v)
        {
            return new Matrix3x2((float)(v.M00 * Math.PI / 180), (float)(v.M01 * Math.PI / 180), (float)(v.M10 * Math.PI / 180), (float)(v.M11 * Math.PI / 180), (float)(v.M20 * Math.PI / 180), (float)(v.M21 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix3x3 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix3x3 radians(Matrix3x3 v)
        {
            return new Matrix3x3((float)(v.M00 * Math.PI / 180), (float)(v.M01 * Math.PI / 180), (float)(v.M02 * Math.PI / 180), (float)(v.M10 * Math.PI / 180), (float)(v.M11 * Math.PI / 180), (float)(v.M12 * Math.PI / 180), (float)(v.M20 * Math.PI / 180), (float)(v.M21 * Math.PI / 180), (float)(v.M22 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix3x4 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix3x4 radians(Matrix3x4 v)
        {
            return new Matrix3x4((float)(v.M00 * Math.PI / 180), (float)(v.M01 * Math.PI / 180), (float)(v.M02 * Math.PI / 180), (float)(v.M03 * Math.PI / 180), (float)(v.M10 * Math.PI / 180), (float)(v.M11 * Math.PI / 180), (float)(v.M12 * Math.PI / 180), (float)(v.M13 * Math.PI / 180), (float)(v.M20 * Math.PI / 180), (float)(v.M21 * Math.PI / 180), (float)(v.M22 * Math.PI / 180), (float)(v.M23 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix4x1 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix4x1 radians(Matrix4x1 v)
        {
            return new Matrix4x1((float)(v.M00 * Math.PI / 180), (float)(v.M10 * Math.PI / 180), (float)(v.M20 * Math.PI / 180), (float)(v.M30 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix4x2 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix4x2 radians(Matrix4x2 v)
        {
            return new Matrix4x2((float)(v.M00 * Math.PI / 180), (float)(v.M01 * Math.PI / 180), (float)(v.M10 * Math.PI / 180), (float)(v.M11 * Math.PI / 180), (float)(v.M20 * Math.PI / 180), (float)(v.M21 * Math.PI / 180), (float)(v.M30 * Math.PI / 180), (float)(v.M31 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix4x3 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix4x3 radians(Matrix4x3 v)
        {
            return new Matrix4x3((float)(v.M00 * Math.PI / 180), (float)(v.M01 * Math.PI / 180), (float)(v.M02 * Math.PI / 180), (float)(v.M10 * Math.PI / 180), (float)(v.M11 * Math.PI / 180), (float)(v.M12 * Math.PI / 180), (float)(v.M20 * Math.PI / 180), (float)(v.M21 * Math.PI / 180), (float)(v.M22 * Math.PI / 180), (float)(v.M30 * Math.PI / 180), (float)(v.M31 * Math.PI / 180), (float)(v.M32 * Math.PI / 180));
        }

        /// <summary>
        /// Performs the radians function to the specified Matrix4x4 object.
        /// Gets the values of each component converted from degrees to radians.
        /// </summary>
        public static Matrix4x4 radians(Matrix4x4 v)
        {
            return new Matrix4x4((float)(v.M00 * Math.PI / 180), (float)(v.M01 * Math.PI / 180), (float)(v.M02 * Math.PI / 180), (float)(v.M03 * Math.PI / 180), (float)(v.M10 * Math.PI / 180), (float)(v.M11 * Math.PI / 180), (float)(v.M12 * Math.PI / 180), (float)(v.M13 * Math.PI / 180), (float)(v.M20 * Math.PI / 180), (float)(v.M21 * Math.PI / 180), (float)(v.M22 * Math.PI / 180), (float)(v.M23 * Math.PI / 180), (float)(v.M30 * Math.PI / 180), (float)(v.M31 * Math.PI / 180), (float)(v.M32 * Math.PI / 180), (float)(v.M33 * Math.PI / 180));
        }

        #endregion
        #region reflect

        /// <summary>
        /// Performs the reflect function to the specified Vector1 objects.
        /// Gets the reflection vector.
        /// </summary>
        public static Vector1 reflect(Vector1 v1, Vector1 v2)
        {

            return v1 - 2 * dot(v1, v2) * v2;

        }


        /// <summary>
        /// Performs the reflect function to the specified Vector2 objects.
        /// Gets the reflection vector.
        /// </summary>
        public static Vector2 reflect(Vector2 v1, Vector2 v2)
        {

            return v1 - 2 * dot(v1, v2) * v2;

        }


        /// <summary>
        /// Performs the reflect function to the specified Vector3 objects.
        /// Gets the reflection vector.
        /// </summary>
        public static Vector3 reflect(Vector3 v1, Vector3 v2)
        {

            return v1 - 2 * dot(v1, v2) * v2;

        }


        /// <summary>
        /// Performs the reflect function to the specified Vector4 objects.
        /// Gets the reflection vector.
        /// </summary>
        public static Vector4 reflect(Vector4 v1, Vector4 v2)
        {

            return v1 - 2 * dot(v1, v2) * v2;

        }

        #endregion
        #region refract

        /// <summary>
        /// Performs the refract function to the specified Vector1 objects.
        /// Gets the refraction vector.
        /// </summary>
        public static Vector1 refract(Vector1 v1, Vector1 v2, float s)
        {

            float k = 1.0f - s * s *
                  (1.0f - dot(v1, v2) * dot(v1, v2));
            if (k < 0.0f)
                return new Vector1(0);
            else
                return s * v1 -
                           (s * dot(v1, v2) * (float)Math.Sqrt(k)) * v2;

        }


        /// <summary>
        /// Performs the refract function to the specified Vector2 objects.
        /// Gets the refraction vector.
        /// </summary>
        public static Vector2 refract(Vector2 v1, Vector2 v2, float s)
        {

            float k = 1.0f - s * s *
                  (1.0f - dot(v1, v2) * dot(v1, v2));
            if (k < 0.0f)
                return new Vector2(0);
            else
                return s * v1 -
                           (s * dot(v1, v2) * (float)Math.Sqrt(k)) * v2;

        }


        /// <summary>
        /// Performs the refract function to the specified Vector3 objects.
        /// Gets the refraction vector.
        /// </summary>
        public static Vector3 refract(Vector3 v1, Vector3 v2, float s)
        {

            float k = 1.0f - s * s *
                  (1.0f - dot(v1, v2) * dot(v1, v2));
            if (k < 0.0f)
                return new Vector3(0);
            else
                return s * v1 -
                           (s * dot(v1, v2) * (float)Math.Sqrt(k)) * v2;
        }


        /// <summary>
        /// Performs the refract function to the specified Vector4 objects.
        /// Gets the refraction vector.
        /// </summary>
        public static Vector4 refract(Vector4 v1, Vector4 v2, float s)
        {

            float k = 1.0f - s * s *
                  (1.0f - dot(v1, v2) * dot(v1, v2));
            if (k < 0.0f)
                return new Vector4(0);
            else
                return s * v1 -
                           (s * dot(v1, v2) * (float)Math.Sqrt(k)) * v2;
        }

        #endregion
        #region round

        /// <summary>
        /// Performs the round function to the specified Vector1 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Vector1 round(Vector1 v)
        {
            return new Vector1((float)Math.Round(v.X));
        }

        /// <summary>
        /// Performs the round function to the specified Vector2 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Vector2 round(Vector2 v)
        {
            return new Vector2((float)Math.Round(v.X), (float)Math.Round(v.Y));
        }

        /// <summary>
        /// Performs the round function to the specified Vector3 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Vector3 round(Vector3 v)
        {
            return new Vector3((float)Math.Round(v.X), (float)Math.Round(v.Y), (float)Math.Round(v.Z));
        }

        /// <summary>
        /// Performs the round function to the specified Vector4 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Vector4 round(Vector4 v)
        {
            return new Vector4((float)Math.Round(v.X), (float)Math.Round(v.Y), (float)Math.Round(v.Z), (float)Math.Round(v.W));
        }


        /// <summary>
        /// Performs the round function to the specified Matrix1x1 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix1x1 round(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Round(v.M00));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix1x2 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix1x2 round(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Round(v.M00), (float)Math.Round(v.M01));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix1x3 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix1x3 round(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Round(v.M00), (float)Math.Round(v.M01), (float)Math.Round(v.M02));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix1x4 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix1x4 round(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Round(v.M00), (float)Math.Round(v.M01), (float)Math.Round(v.M02), (float)Math.Round(v.M03));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix2x1 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix2x1 round(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Round(v.M00), (float)Math.Round(v.M10));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix2x2 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix2x2 round(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Round(v.M00), (float)Math.Round(v.M01), (float)Math.Round(v.M10), (float)Math.Round(v.M11));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix2x3 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix2x3 round(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Round(v.M00), (float)Math.Round(v.M01), (float)Math.Round(v.M02), (float)Math.Round(v.M10), (float)Math.Round(v.M11), (float)Math.Round(v.M12));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix2x4 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix2x4 round(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Round(v.M00), (float)Math.Round(v.M01), (float)Math.Round(v.M02), (float)Math.Round(v.M03), (float)Math.Round(v.M10), (float)Math.Round(v.M11), (float)Math.Round(v.M12), (float)Math.Round(v.M13));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix3x1 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix3x1 round(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Round(v.M00), (float)Math.Round(v.M10), (float)Math.Round(v.M20));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix3x2 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix3x2 round(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Round(v.M00), (float)Math.Round(v.M01), (float)Math.Round(v.M10), (float)Math.Round(v.M11), (float)Math.Round(v.M20), (float)Math.Round(v.M21));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix3x3 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix3x3 round(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Round(v.M00), (float)Math.Round(v.M01), (float)Math.Round(v.M02), (float)Math.Round(v.M10), (float)Math.Round(v.M11), (float)Math.Round(v.M12), (float)Math.Round(v.M20), (float)Math.Round(v.M21), (float)Math.Round(v.M22));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix3x4 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix3x4 round(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Round(v.M00), (float)Math.Round(v.M01), (float)Math.Round(v.M02), (float)Math.Round(v.M03), (float)Math.Round(v.M10), (float)Math.Round(v.M11), (float)Math.Round(v.M12), (float)Math.Round(v.M13), (float)Math.Round(v.M20), (float)Math.Round(v.M21), (float)Math.Round(v.M22), (float)Math.Round(v.M23));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix4x1 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix4x1 round(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Round(v.M00), (float)Math.Round(v.M10), (float)Math.Round(v.M20), (float)Math.Round(v.M30));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix4x2 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix4x2 round(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Round(v.M00), (float)Math.Round(v.M01), (float)Math.Round(v.M10), (float)Math.Round(v.M11), (float)Math.Round(v.M20), (float)Math.Round(v.M21), (float)Math.Round(v.M30), (float)Math.Round(v.M31));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix4x3 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix4x3 round(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Round(v.M00), (float)Math.Round(v.M01), (float)Math.Round(v.M02), (float)Math.Round(v.M10), (float)Math.Round(v.M11), (float)Math.Round(v.M12), (float)Math.Round(v.M20), (float)Math.Round(v.M21), (float)Math.Round(v.M22), (float)Math.Round(v.M30), (float)Math.Round(v.M31), (float)Math.Round(v.M32));
        }

        /// <summary>
        /// Performs the round function to the specified Matrix4x4 object.
        /// Rounds each component of v to the nearest integer.
        /// </summary>
        public static Matrix4x4 round(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Round(v.M00), (float)Math.Round(v.M01), (float)Math.Round(v.M02), (float)Math.Round(v.M03), (float)Math.Round(v.M10), (float)Math.Round(v.M11), (float)Math.Round(v.M12), (float)Math.Round(v.M13), (float)Math.Round(v.M20), (float)Math.Round(v.M21), (float)Math.Round(v.M22), (float)Math.Round(v.M23), (float)Math.Round(v.M30), (float)Math.Round(v.M31), (float)Math.Round(v.M32), (float)Math.Round(v.M33));
        }

        #endregion
        #region rsqrt

        /// <summary>
        /// Performs the rsqrt function to the specified Vector1 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Vector1 rsqrt(Vector1 v)
        {
            return new Vector1(1 / (float)Math.Sqrt(v.X));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Vector2 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Vector2 rsqrt(Vector2 v)
        {
            return new Vector2(1 / (float)Math.Sqrt(v.X), 1 / (float)Math.Sqrt(v.Y));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Vector3 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Vector3 rsqrt(Vector3 v)
        {
            return new Vector3(1 / (float)Math.Sqrt(v.X), 1 / (float)Math.Sqrt(v.Y), 1 / (float)Math.Sqrt(v.Z));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Vector4 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Vector4 rsqrt(Vector4 v)
        {
            return new Vector4(1 / (float)Math.Sqrt(v.X), 1 / (float)Math.Sqrt(v.Y), 1 / (float)Math.Sqrt(v.Z), 1 / (float)Math.Sqrt(v.W));
        }


        /// <summary>
        /// Performs the rsqrt function to the specified Matrix1x1 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix1x1 rsqrt(Matrix1x1 v)
        {
            return new Matrix1x1(1 / (float)Math.Sqrt(v.M00));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix1x2 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix1x2 rsqrt(Matrix1x2 v)
        {
            return new Matrix1x2(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M01));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix1x3 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix1x3 rsqrt(Matrix1x3 v)
        {
            return new Matrix1x3(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M01), 1 / (float)Math.Sqrt(v.M02));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix1x4 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix1x4 rsqrt(Matrix1x4 v)
        {
            return new Matrix1x4(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M01), 1 / (float)Math.Sqrt(v.M02), 1 / (float)Math.Sqrt(v.M03));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix2x1 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix2x1 rsqrt(Matrix2x1 v)
        {
            return new Matrix2x1(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M10));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix2x2 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix2x2 rsqrt(Matrix2x2 v)
        {
            return new Matrix2x2(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M01), 1 / (float)Math.Sqrt(v.M10), 1 / (float)Math.Sqrt(v.M11));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix2x3 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix2x3 rsqrt(Matrix2x3 v)
        {
            return new Matrix2x3(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M01), 1 / (float)Math.Sqrt(v.M02), 1 / (float)Math.Sqrt(v.M10), 1 / (float)Math.Sqrt(v.M11), 1 / (float)Math.Sqrt(v.M12));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix2x4 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix2x4 rsqrt(Matrix2x4 v)
        {
            return new Matrix2x4(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M01), 1 / (float)Math.Sqrt(v.M02), 1 / (float)Math.Sqrt(v.M03), 1 / (float)Math.Sqrt(v.M10), 1 / (float)Math.Sqrt(v.M11), 1 / (float)Math.Sqrt(v.M12), 1 / (float)Math.Sqrt(v.M13));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix3x1 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix3x1 rsqrt(Matrix3x1 v)
        {
            return new Matrix3x1(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M10), 1 / (float)Math.Sqrt(v.M20));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix3x2 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix3x2 rsqrt(Matrix3x2 v)
        {
            return new Matrix3x2(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M01), 1 / (float)Math.Sqrt(v.M10), 1 / (float)Math.Sqrt(v.M11), 1 / (float)Math.Sqrt(v.M20), 1 / (float)Math.Sqrt(v.M21));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix3x3 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix3x3 rsqrt(Matrix3x3 v)
        {
            return new Matrix3x3(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M01), 1 / (float)Math.Sqrt(v.M02), 1 / (float)Math.Sqrt(v.M10), 1 / (float)Math.Sqrt(v.M11), 1 / (float)Math.Sqrt(v.M12), 1 / (float)Math.Sqrt(v.M20), 1 / (float)Math.Sqrt(v.M21), 1 / (float)Math.Sqrt(v.M22));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix3x4 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix3x4 rsqrt(Matrix3x4 v)
        {
            return new Matrix3x4(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M01), 1 / (float)Math.Sqrt(v.M02), 1 / (float)Math.Sqrt(v.M03), 1 / (float)Math.Sqrt(v.M10), 1 / (float)Math.Sqrt(v.M11), 1 / (float)Math.Sqrt(v.M12), 1 / (float)Math.Sqrt(v.M13), 1 / (float)Math.Sqrt(v.M20), 1 / (float)Math.Sqrt(v.M21), 1 / (float)Math.Sqrt(v.M22), 1 / (float)Math.Sqrt(v.M23));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix4x1 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix4x1 rsqrt(Matrix4x1 v)
        {
            return new Matrix4x1(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M10), 1 / (float)Math.Sqrt(v.M20), 1 / (float)Math.Sqrt(v.M30));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix4x2 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix4x2 rsqrt(Matrix4x2 v)
        {
            return new Matrix4x2(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M01), 1 / (float)Math.Sqrt(v.M10), 1 / (float)Math.Sqrt(v.M11), 1 / (float)Math.Sqrt(v.M20), 1 / (float)Math.Sqrt(v.M21), 1 / (float)Math.Sqrt(v.M30), 1 / (float)Math.Sqrt(v.M31));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix4x3 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix4x3 rsqrt(Matrix4x3 v)
        {
            return new Matrix4x3(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M01), 1 / (float)Math.Sqrt(v.M02), 1 / (float)Math.Sqrt(v.M10), 1 / (float)Math.Sqrt(v.M11), 1 / (float)Math.Sqrt(v.M12), 1 / (float)Math.Sqrt(v.M20), 1 / (float)Math.Sqrt(v.M21), 1 / (float)Math.Sqrt(v.M22), 1 / (float)Math.Sqrt(v.M30), 1 / (float)Math.Sqrt(v.M31), 1 / (float)Math.Sqrt(v.M32));
        }

        /// <summary>
        /// Performs the rsqrt function to the specified Matrix4x4 object.
        /// Gets the inverse of the square root of each component.
        /// </summary>
        public static Matrix4x4 rsqrt(Matrix4x4 v)
        {
            return new Matrix4x4(1 / (float)Math.Sqrt(v.M00), 1 / (float)Math.Sqrt(v.M01), 1 / (float)Math.Sqrt(v.M02), 1 / (float)Math.Sqrt(v.M03), 1 / (float)Math.Sqrt(v.M10), 1 / (float)Math.Sqrt(v.M11), 1 / (float)Math.Sqrt(v.M12), 1 / (float)Math.Sqrt(v.M13), 1 / (float)Math.Sqrt(v.M20), 1 / (float)Math.Sqrt(v.M21), 1 / (float)Math.Sqrt(v.M22), 1 / (float)Math.Sqrt(v.M23), 1 / (float)Math.Sqrt(v.M30), 1 / (float)Math.Sqrt(v.M31), 1 / (float)Math.Sqrt(v.M32), 1 / (float)Math.Sqrt(v.M33));
        }

        #endregion
        #region saturate

        public static float saturate(float x)
        {
            return Math.Max(0, Math.Min(1, x));
        }
        /// <summary>
        /// Performs the saturate function to the specified Vector1 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Vector1 saturate(Vector1 v)
        {
            return new Vector1(Math.Max(0, Math.Min(1, v.X)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Vector2 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Vector2 saturate(Vector2 v)
        {
            return new Vector2(Math.Max(0, Math.Min(1, v.X)), Math.Max(0, Math.Min(1, v.Y)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Vector3 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Vector3 saturate(Vector3 v)
        {
            return new Vector3(Math.Max(0, Math.Min(1, v.X)), Math.Max(0, Math.Min(1, v.Y)), Math.Max(0, Math.Min(1, v.Z)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Vector4 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Vector4 saturate(Vector4 v)
        {
            return new Vector4(Math.Max(0, Math.Min(1, v.X)), Math.Max(0, Math.Min(1, v.Y)), Math.Max(0, Math.Min(1, v.Z)), Math.Max(0, Math.Min(1, v.W)));
        }


        /// <summary>
        /// Performs the saturate function to the specified Matrix1x1 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix1x1 saturate(Matrix1x1 v)
        {
            return new Matrix1x1(Math.Max(0, Math.Min(1, v.M00)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix1x2 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix1x2 saturate(Matrix1x2 v)
        {
            return new Matrix1x2(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M01)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix1x3 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix1x3 saturate(Matrix1x3 v)
        {
            return new Matrix1x3(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M01)), Math.Max(0, Math.Min(1, v.M02)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix1x4 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix1x4 saturate(Matrix1x4 v)
        {
            return new Matrix1x4(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M01)), Math.Max(0, Math.Min(1, v.M02)), Math.Max(0, Math.Min(1, v.M03)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix2x1 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix2x1 saturate(Matrix2x1 v)
        {
            return new Matrix2x1(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M10)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix2x2 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix2x2 saturate(Matrix2x2 v)
        {
            return new Matrix2x2(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M01)), Math.Max(0, Math.Min(1, v.M10)), Math.Max(0, Math.Min(1, v.M11)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix2x3 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix2x3 saturate(Matrix2x3 v)
        {
            return new Matrix2x3(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M01)), Math.Max(0, Math.Min(1, v.M02)), Math.Max(0, Math.Min(1, v.M10)), Math.Max(0, Math.Min(1, v.M11)), Math.Max(0, Math.Min(1, v.M12)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix2x4 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix2x4 saturate(Matrix2x4 v)
        {
            return new Matrix2x4(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M01)), Math.Max(0, Math.Min(1, v.M02)), Math.Max(0, Math.Min(1, v.M03)), Math.Max(0, Math.Min(1, v.M10)), Math.Max(0, Math.Min(1, v.M11)), Math.Max(0, Math.Min(1, v.M12)), Math.Max(0, Math.Min(1, v.M13)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix3x1 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix3x1 saturate(Matrix3x1 v)
        {
            return new Matrix3x1(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M10)), Math.Max(0, Math.Min(1, v.M20)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix3x2 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix3x2 saturate(Matrix3x2 v)
        {
            return new Matrix3x2(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M01)), Math.Max(0, Math.Min(1, v.M10)), Math.Max(0, Math.Min(1, v.M11)), Math.Max(0, Math.Min(1, v.M20)), Math.Max(0, Math.Min(1, v.M21)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix3x3 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix3x3 saturate(Matrix3x3 v)
        {
            return new Matrix3x3(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M01)), Math.Max(0, Math.Min(1, v.M02)), Math.Max(0, Math.Min(1, v.M10)), Math.Max(0, Math.Min(1, v.M11)), Math.Max(0, Math.Min(1, v.M12)), Math.Max(0, Math.Min(1, v.M20)), Math.Max(0, Math.Min(1, v.M21)), Math.Max(0, Math.Min(1, v.M22)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix3x4 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix3x4 saturate(Matrix3x4 v)
        {
            return new Matrix3x4(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M01)), Math.Max(0, Math.Min(1, v.M02)), Math.Max(0, Math.Min(1, v.M03)), Math.Max(0, Math.Min(1, v.M10)), Math.Max(0, Math.Min(1, v.M11)), Math.Max(0, Math.Min(1, v.M12)), Math.Max(0, Math.Min(1, v.M13)), Math.Max(0, Math.Min(1, v.M20)), Math.Max(0, Math.Min(1, v.M21)), Math.Max(0, Math.Min(1, v.M22)), Math.Max(0, Math.Min(1, v.M23)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix4x1 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix4x1 saturate(Matrix4x1 v)
        {
            return new Matrix4x1(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M10)), Math.Max(0, Math.Min(1, v.M20)), Math.Max(0, Math.Min(1, v.M30)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix4x2 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix4x2 saturate(Matrix4x2 v)
        {
            return new Matrix4x2(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M01)), Math.Max(0, Math.Min(1, v.M10)), Math.Max(0, Math.Min(1, v.M11)), Math.Max(0, Math.Min(1, v.M20)), Math.Max(0, Math.Min(1, v.M21)), Math.Max(0, Math.Min(1, v.M30)), Math.Max(0, Math.Min(1, v.M31)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix4x3 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix4x3 saturate(Matrix4x3 v)
        {
            return new Matrix4x3(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M01)), Math.Max(0, Math.Min(1, v.M02)), Math.Max(0, Math.Min(1, v.M10)), Math.Max(0, Math.Min(1, v.M11)), Math.Max(0, Math.Min(1, v.M12)), Math.Max(0, Math.Min(1, v.M20)), Math.Max(0, Math.Min(1, v.M21)), Math.Max(0, Math.Min(1, v.M22)), Math.Max(0, Math.Min(1, v.M30)), Math.Max(0, Math.Min(1, v.M31)), Math.Max(0, Math.Min(1, v.M32)));
        }

        /// <summary>
        /// Performs the saturate function to the specified Matrix4x4 object.
        /// Clamps each component between 0 and 1.
        /// </summary>
        public static Matrix4x4 saturate(Matrix4x4 v)
        {
            return new Matrix4x4(Math.Max(0, Math.Min(1, v.M00)), Math.Max(0, Math.Min(1, v.M01)), Math.Max(0, Math.Min(1, v.M02)), Math.Max(0, Math.Min(1, v.M03)), Math.Max(0, Math.Min(1, v.M10)), Math.Max(0, Math.Min(1, v.M11)), Math.Max(0, Math.Min(1, v.M12)), Math.Max(0, Math.Min(1, v.M13)), Math.Max(0, Math.Min(1, v.M20)), Math.Max(0, Math.Min(1, v.M21)), Math.Max(0, Math.Min(1, v.M22)), Math.Max(0, Math.Min(1, v.M23)), Math.Max(0, Math.Min(1, v.M30)), Math.Max(0, Math.Min(1, v.M31)), Math.Max(0, Math.Min(1, v.M32)), Math.Max(0, Math.Min(1, v.M33)));
        }

        #endregion
        #region sign

        /// <summary>
        /// Performs the sign function to the specified Vector1 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Vector1 sign(Vector1 v)
        {
            return new Vector1((float)Math.Sign(v.X));
        }

        /// <summary>
        /// Performs the sign function to the specified Vector2 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Vector2 sign(Vector2 v)
        {
            return new Vector2((float)Math.Sign(v.X), (float)Math.Sign(v.Y));
        }

        /// <summary>
        /// Performs the sign function to the specified Vector3 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Vector3 sign(Vector3 v)
        {
            return new Vector3((float)Math.Sign(v.X), (float)Math.Sign(v.Y), (float)Math.Sign(v.Z));
        }

        /// <summary>
        /// Performs the sign function to the specified Vector4 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Vector4 sign(Vector4 v)
        {
            return new Vector4((float)Math.Sign(v.X), (float)Math.Sign(v.Y), (float)Math.Sign(v.Z), (float)Math.Sign(v.W));
        }


        /// <summary>
        /// Performs the sign function to the specified Matrix1x1 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix1x1 sign(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Sign(v.M00));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix1x2 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix1x2 sign(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Sign(v.M00), (float)Math.Sign(v.M01));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix1x3 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix1x3 sign(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Sign(v.M00), (float)Math.Sign(v.M01), (float)Math.Sign(v.M02));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix1x4 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix1x4 sign(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Sign(v.M00), (float)Math.Sign(v.M01), (float)Math.Sign(v.M02), (float)Math.Sign(v.M03));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix2x1 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix2x1 sign(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Sign(v.M00), (float)Math.Sign(v.M10));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix2x2 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix2x2 sign(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Sign(v.M00), (float)Math.Sign(v.M01), (float)Math.Sign(v.M10), (float)Math.Sign(v.M11));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix2x3 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix2x3 sign(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Sign(v.M00), (float)Math.Sign(v.M01), (float)Math.Sign(v.M02), (float)Math.Sign(v.M10), (float)Math.Sign(v.M11), (float)Math.Sign(v.M12));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix2x4 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix2x4 sign(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Sign(v.M00), (float)Math.Sign(v.M01), (float)Math.Sign(v.M02), (float)Math.Sign(v.M03), (float)Math.Sign(v.M10), (float)Math.Sign(v.M11), (float)Math.Sign(v.M12), (float)Math.Sign(v.M13));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix3x1 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix3x1 sign(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Sign(v.M00), (float)Math.Sign(v.M10), (float)Math.Sign(v.M20));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix3x2 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix3x2 sign(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Sign(v.M00), (float)Math.Sign(v.M01), (float)Math.Sign(v.M10), (float)Math.Sign(v.M11), (float)Math.Sign(v.M20), (float)Math.Sign(v.M21));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix3x3 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix3x3 sign(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Sign(v.M00), (float)Math.Sign(v.M01), (float)Math.Sign(v.M02), (float)Math.Sign(v.M10), (float)Math.Sign(v.M11), (float)Math.Sign(v.M12), (float)Math.Sign(v.M20), (float)Math.Sign(v.M21), (float)Math.Sign(v.M22));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix3x4 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix3x4 sign(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Sign(v.M00), (float)Math.Sign(v.M01), (float)Math.Sign(v.M02), (float)Math.Sign(v.M03), (float)Math.Sign(v.M10), (float)Math.Sign(v.M11), (float)Math.Sign(v.M12), (float)Math.Sign(v.M13), (float)Math.Sign(v.M20), (float)Math.Sign(v.M21), (float)Math.Sign(v.M22), (float)Math.Sign(v.M23));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix4x1 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix4x1 sign(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Sign(v.M00), (float)Math.Sign(v.M10), (float)Math.Sign(v.M20), (float)Math.Sign(v.M30));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix4x2 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix4x2 sign(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Sign(v.M00), (float)Math.Sign(v.M01), (float)Math.Sign(v.M10), (float)Math.Sign(v.M11), (float)Math.Sign(v.M20), (float)Math.Sign(v.M21), (float)Math.Sign(v.M30), (float)Math.Sign(v.M31));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix4x3 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix4x3 sign(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Sign(v.M00), (float)Math.Sign(v.M01), (float)Math.Sign(v.M02), (float)Math.Sign(v.M10), (float)Math.Sign(v.M11), (float)Math.Sign(v.M12), (float)Math.Sign(v.M20), (float)Math.Sign(v.M21), (float)Math.Sign(v.M22), (float)Math.Sign(v.M30), (float)Math.Sign(v.M31), (float)Math.Sign(v.M32));
        }

        /// <summary>
        /// Performs the sign function to the specified Matrix4x4 object.
        /// Gets the sign of each component of v.
        /// </summary>
        public static Matrix4x4 sign(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Sign(v.M00), (float)Math.Sign(v.M01), (float)Math.Sign(v.M02), (float)Math.Sign(v.M03), (float)Math.Sign(v.M10), (float)Math.Sign(v.M11), (float)Math.Sign(v.M12), (float)Math.Sign(v.M13), (float)Math.Sign(v.M20), (float)Math.Sign(v.M21), (float)Math.Sign(v.M22), (float)Math.Sign(v.M23), (float)Math.Sign(v.M30), (float)Math.Sign(v.M31), (float)Math.Sign(v.M32), (float)Math.Sign(v.M33));
        }

        #endregion
        #region sin

        public static float sin(float x)
        {
            return (float)Math.Sin(x);
        }

        /// <summary>
        /// Performs the sin function to the specified Vector1 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Vector1 sin(Vector1 v)
        {
            return new Vector1((float)Math.Sin(v.X));
        }

        /// <summary>
        /// Performs the sin function to the specified Vector2 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Vector2 sin(Vector2 v)
        {
            return new Vector2((float)Math.Sin(v.X), (float)Math.Sin(v.Y));
        }

        /// <summary>
        /// Performs the sin function to the specified Vector3 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Vector3 sin(Vector3 v)
        {
            return new Vector3((float)Math.Sin(v.X), (float)Math.Sin(v.Y), (float)Math.Sin(v.Z));
        }

        /// <summary>
        /// Performs the sin function to the specified Vector4 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Vector4 sin(Vector4 v)
        {
            return new Vector4((float)Math.Sin(v.X), (float)Math.Sin(v.Y), (float)Math.Sin(v.Z), (float)Math.Sin(v.W));
        }


        /// <summary>
        /// Performs the sin function to the specified Matrix1x1 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix1x1 sin(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Sin(v.M00));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix1x2 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix1x2 sin(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Sin(v.M00), (float)Math.Sin(v.M01));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix1x3 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix1x3 sin(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Sin(v.M00), (float)Math.Sin(v.M01), (float)Math.Sin(v.M02));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix1x4 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix1x4 sin(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Sin(v.M00), (float)Math.Sin(v.M01), (float)Math.Sin(v.M02), (float)Math.Sin(v.M03));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix2x1 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix2x1 sin(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Sin(v.M00), (float)Math.Sin(v.M10));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix2x2 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix2x2 sin(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Sin(v.M00), (float)Math.Sin(v.M01), (float)Math.Sin(v.M10), (float)Math.Sin(v.M11));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix2x3 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix2x3 sin(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Sin(v.M00), (float)Math.Sin(v.M01), (float)Math.Sin(v.M02), (float)Math.Sin(v.M10), (float)Math.Sin(v.M11), (float)Math.Sin(v.M12));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix2x4 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix2x4 sin(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Sin(v.M00), (float)Math.Sin(v.M01), (float)Math.Sin(v.M02), (float)Math.Sin(v.M03), (float)Math.Sin(v.M10), (float)Math.Sin(v.M11), (float)Math.Sin(v.M12), (float)Math.Sin(v.M13));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix3x1 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix3x1 sin(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Sin(v.M00), (float)Math.Sin(v.M10), (float)Math.Sin(v.M20));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix3x2 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix3x2 sin(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Sin(v.M00), (float)Math.Sin(v.M01), (float)Math.Sin(v.M10), (float)Math.Sin(v.M11), (float)Math.Sin(v.M20), (float)Math.Sin(v.M21));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix3x3 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix3x3 sin(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Sin(v.M00), (float)Math.Sin(v.M01), (float)Math.Sin(v.M02), (float)Math.Sin(v.M10), (float)Math.Sin(v.M11), (float)Math.Sin(v.M12), (float)Math.Sin(v.M20), (float)Math.Sin(v.M21), (float)Math.Sin(v.M22));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix3x4 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix3x4 sin(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Sin(v.M00), (float)Math.Sin(v.M01), (float)Math.Sin(v.M02), (float)Math.Sin(v.M03), (float)Math.Sin(v.M10), (float)Math.Sin(v.M11), (float)Math.Sin(v.M12), (float)Math.Sin(v.M13), (float)Math.Sin(v.M20), (float)Math.Sin(v.M21), (float)Math.Sin(v.M22), (float)Math.Sin(v.M23));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix4x1 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix4x1 sin(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Sin(v.M00), (float)Math.Sin(v.M10), (float)Math.Sin(v.M20), (float)Math.Sin(v.M30));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix4x2 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix4x2 sin(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Sin(v.M00), (float)Math.Sin(v.M01), (float)Math.Sin(v.M10), (float)Math.Sin(v.M11), (float)Math.Sin(v.M20), (float)Math.Sin(v.M21), (float)Math.Sin(v.M30), (float)Math.Sin(v.M31));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix4x3 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix4x3 sin(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Sin(v.M00), (float)Math.Sin(v.M01), (float)Math.Sin(v.M02), (float)Math.Sin(v.M10), (float)Math.Sin(v.M11), (float)Math.Sin(v.M12), (float)Math.Sin(v.M20), (float)Math.Sin(v.M21), (float)Math.Sin(v.M22), (float)Math.Sin(v.M30), (float)Math.Sin(v.M31), (float)Math.Sin(v.M32));
        }

        /// <summary>
        /// Performs the sin function to the specified Matrix4x4 object.
        /// Gets the sine of each component of v.
        /// </summary>
        public static Matrix4x4 sin(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Sin(v.M00), (float)Math.Sin(v.M01), (float)Math.Sin(v.M02), (float)Math.Sin(v.M03), (float)Math.Sin(v.M10), (float)Math.Sin(v.M11), (float)Math.Sin(v.M12), (float)Math.Sin(v.M13), (float)Math.Sin(v.M20), (float)Math.Sin(v.M21), (float)Math.Sin(v.M22), (float)Math.Sin(v.M23), (float)Math.Sin(v.M30), (float)Math.Sin(v.M31), (float)Math.Sin(v.M32), (float)Math.Sin(v.M33));
        }

        #endregion
        #region sinh

        /// <summary>
        /// Performs the sinh function to the specified Vector1 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Vector1 sinh(Vector1 v)
        {
            return new Vector1((float)Math.Sinh(v.X));
        }

        /// <summary>
        /// Performs the sinh function to the specified Vector2 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Vector2 sinh(Vector2 v)
        {
            return new Vector2((float)Math.Sinh(v.X), (float)Math.Sinh(v.Y));
        }

        /// <summary>
        /// Performs the sinh function to the specified Vector3 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Vector3 sinh(Vector3 v)
        {
            return new Vector3((float)Math.Sinh(v.X), (float)Math.Sinh(v.Y), (float)Math.Sinh(v.Z));
        }

        /// <summary>
        /// Performs the sinh function to the specified Vector4 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Vector4 sinh(Vector4 v)
        {
            return new Vector4((float)Math.Sinh(v.X), (float)Math.Sinh(v.Y), (float)Math.Sinh(v.Z), (float)Math.Sinh(v.W));
        }


        /// <summary>
        /// Performs the sinh function to the specified Matrix1x1 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix1x1 sinh(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Sinh(v.M00));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix1x2 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix1x2 sinh(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M01));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix1x3 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix1x3 sinh(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M01), (float)Math.Sinh(v.M02));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix1x4 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix1x4 sinh(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M01), (float)Math.Sinh(v.M02), (float)Math.Sinh(v.M03));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix2x1 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix2x1 sinh(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M10));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix2x2 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix2x2 sinh(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M01), (float)Math.Sinh(v.M10), (float)Math.Sinh(v.M11));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix2x3 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix2x3 sinh(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M01), (float)Math.Sinh(v.M02), (float)Math.Sinh(v.M10), (float)Math.Sinh(v.M11), (float)Math.Sinh(v.M12));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix2x4 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix2x4 sinh(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M01), (float)Math.Sinh(v.M02), (float)Math.Sinh(v.M03), (float)Math.Sinh(v.M10), (float)Math.Sinh(v.M11), (float)Math.Sinh(v.M12), (float)Math.Sinh(v.M13));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix3x1 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix3x1 sinh(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M10), (float)Math.Sinh(v.M20));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix3x2 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix3x2 sinh(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M01), (float)Math.Sinh(v.M10), (float)Math.Sinh(v.M11), (float)Math.Sinh(v.M20), (float)Math.Sinh(v.M21));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix3x3 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix3x3 sinh(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M01), (float)Math.Sinh(v.M02), (float)Math.Sinh(v.M10), (float)Math.Sinh(v.M11), (float)Math.Sinh(v.M12), (float)Math.Sinh(v.M20), (float)Math.Sinh(v.M21), (float)Math.Sinh(v.M22));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix3x4 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix3x4 sinh(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M01), (float)Math.Sinh(v.M02), (float)Math.Sinh(v.M03), (float)Math.Sinh(v.M10), (float)Math.Sinh(v.M11), (float)Math.Sinh(v.M12), (float)Math.Sinh(v.M13), (float)Math.Sinh(v.M20), (float)Math.Sinh(v.M21), (float)Math.Sinh(v.M22), (float)Math.Sinh(v.M23));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix4x1 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix4x1 sinh(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M10), (float)Math.Sinh(v.M20), (float)Math.Sinh(v.M30));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix4x2 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix4x2 sinh(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M01), (float)Math.Sinh(v.M10), (float)Math.Sinh(v.M11), (float)Math.Sinh(v.M20), (float)Math.Sinh(v.M21), (float)Math.Sinh(v.M30), (float)Math.Sinh(v.M31));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix4x3 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix4x3 sinh(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M01), (float)Math.Sinh(v.M02), (float)Math.Sinh(v.M10), (float)Math.Sinh(v.M11), (float)Math.Sinh(v.M12), (float)Math.Sinh(v.M20), (float)Math.Sinh(v.M21), (float)Math.Sinh(v.M22), (float)Math.Sinh(v.M30), (float)Math.Sinh(v.M31), (float)Math.Sinh(v.M32));
        }

        /// <summary>
        /// Performs the sinh function to the specified Matrix4x4 object.
        /// Gets the hyperbolic sine of each component of v.
        /// </summary>
        public static Matrix4x4 sinh(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Sinh(v.M00), (float)Math.Sinh(v.M01), (float)Math.Sinh(v.M02), (float)Math.Sinh(v.M03), (float)Math.Sinh(v.M10), (float)Math.Sinh(v.M11), (float)Math.Sinh(v.M12), (float)Math.Sinh(v.M13), (float)Math.Sinh(v.M20), (float)Math.Sinh(v.M21), (float)Math.Sinh(v.M22), (float)Math.Sinh(v.M23), (float)Math.Sinh(v.M30), (float)Math.Sinh(v.M31), (float)Math.Sinh(v.M32), (float)Math.Sinh(v.M33));
        }

        #endregion
        #region smoothstep

        /// <summary>
        /// Performs the smoothstep function to the specified Vector1 objects.
        /// Gets 0 if s < v1, 1 if s > v2, and performs an hermite interpolation between 0 and 1 on the other case.
        /// </summary>
        public static Vector1 smoothstep(Vector1 v1, Vector1 v2, Vector1 s)
        {

            Vector1 t = clamp((s - v1) / (v2 - v1), new Vector1(0), new Vector1(1));
            return t * t * (new Vector1(3.0f) - 2.0f * t);

        }


        /// <summary>
        /// Performs the smoothstep function to the specified Vector2 objects.
        /// Gets 0 if s < v1, 1 if s > v2, and performs an hermite interpolation between 0 and 1 on the other case.
        /// </summary>
        public static Vector2 smoothstep(Vector2 v1, Vector2 v2, Vector2 s)
        {

            Vector2 t = clamp((s - v1) / (v2 - v1), new Vector2(0), new Vector2(1));
            return t * t * (new Vector2(3.0f) - 2.0f * t);

        }


        /// <summary>
        /// Performs the smoothstep function to the specified Vector3 objects.
        /// Gets 0 if s < v1, 1 if s > v2, and performs an hermite interpolation between 0 and 1 on the other case.
        /// </summary>
        public static Vector3 smoothstep(Vector3 v1, Vector3 v2, Vector3 s)
        {

            Vector3 t = clamp((s - v1) / (v2 - v1), new Vector3(0), new Vector3(1));
            return t * t * (new Vector3(3.0f) - 2.0f * t);

        }


        /// <summary>
        /// Performs the smoothstep function to the specified Vector4 objects.
        /// Gets 0 if s < v1, 1 if s > v2, and performs an hermite interpolation between 0 and 1 on the other case.
        /// </summary>
        public static Vector4 smoothstep(Vector4 v1, Vector4 v2, Vector4 s)
        {

            Vector4 t = clamp((s - v1) / (v2 - v1), new Vector4(0), new Vector4(1));
            return t * t * (new Vector4(3.0f) - 2.0f * t);

        }

        #endregion
        #region sqrt

        public static float sqrt(float x)
        {
            return (float)Math.Sqrt(x);
        }

        /// <summary>
        /// Performs the sqrt function to the specified Vector1 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Vector1 sqrt(Vector1 v)
        {
            return new Vector1((float)Math.Sqrt(v.X));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Vector2 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Vector2 sqrt(Vector2 v)
        {
            return new Vector2((float)Math.Sqrt(v.X), (float)Math.Sqrt(v.Y));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Vector3 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Vector3 sqrt(Vector3 v)
        {
            return new Vector3((float)Math.Sqrt(v.X), (float)Math.Sqrt(v.Y), (float)Math.Sqrt(v.Z));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Vector4 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Vector4 sqrt(Vector4 v)
        {
            return new Vector4((float)Math.Sqrt(v.X), (float)Math.Sqrt(v.Y), (float)Math.Sqrt(v.Z), (float)Math.Sqrt(v.W));
        }


        /// <summary>
        /// Performs the sqrt function to the specified Matrix1x1 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix1x1 sqrt(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Sqrt(v.M00));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix1x2 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix1x2 sqrt(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M01));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix1x3 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix1x3 sqrt(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M01), (float)Math.Sqrt(v.M02));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix1x4 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix1x4 sqrt(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M01), (float)Math.Sqrt(v.M02), (float)Math.Sqrt(v.M03));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix2x1 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix2x1 sqrt(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M10));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix2x2 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix2x2 sqrt(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M01), (float)Math.Sqrt(v.M10), (float)Math.Sqrt(v.M11));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix2x3 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix2x3 sqrt(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M01), (float)Math.Sqrt(v.M02), (float)Math.Sqrt(v.M10), (float)Math.Sqrt(v.M11), (float)Math.Sqrt(v.M12));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix2x4 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix2x4 sqrt(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M01), (float)Math.Sqrt(v.M02), (float)Math.Sqrt(v.M03), (float)Math.Sqrt(v.M10), (float)Math.Sqrt(v.M11), (float)Math.Sqrt(v.M12), (float)Math.Sqrt(v.M13));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix3x1 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix3x1 sqrt(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M10), (float)Math.Sqrt(v.M20));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix3x2 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix3x2 sqrt(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M01), (float)Math.Sqrt(v.M10), (float)Math.Sqrt(v.M11), (float)Math.Sqrt(v.M20), (float)Math.Sqrt(v.M21));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix3x3 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix3x3 sqrt(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M01), (float)Math.Sqrt(v.M02), (float)Math.Sqrt(v.M10), (float)Math.Sqrt(v.M11), (float)Math.Sqrt(v.M12), (float)Math.Sqrt(v.M20), (float)Math.Sqrt(v.M21), (float)Math.Sqrt(v.M22));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix3x4 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix3x4 sqrt(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M01), (float)Math.Sqrt(v.M02), (float)Math.Sqrt(v.M03), (float)Math.Sqrt(v.M10), (float)Math.Sqrt(v.M11), (float)Math.Sqrt(v.M12), (float)Math.Sqrt(v.M13), (float)Math.Sqrt(v.M20), (float)Math.Sqrt(v.M21), (float)Math.Sqrt(v.M22), (float)Math.Sqrt(v.M23));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix4x1 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix4x1 sqrt(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M10), (float)Math.Sqrt(v.M20), (float)Math.Sqrt(v.M30));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix4x2 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix4x2 sqrt(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M01), (float)Math.Sqrt(v.M10), (float)Math.Sqrt(v.M11), (float)Math.Sqrt(v.M20), (float)Math.Sqrt(v.M21), (float)Math.Sqrt(v.M30), (float)Math.Sqrt(v.M31));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix4x3 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix4x3 sqrt(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M01), (float)Math.Sqrt(v.M02), (float)Math.Sqrt(v.M10), (float)Math.Sqrt(v.M11), (float)Math.Sqrt(v.M12), (float)Math.Sqrt(v.M20), (float)Math.Sqrt(v.M21), (float)Math.Sqrt(v.M22), (float)Math.Sqrt(v.M30), (float)Math.Sqrt(v.M31), (float)Math.Sqrt(v.M32));
        }

        /// <summary>
        /// Performs the sqrt function to the specified Matrix4x4 object.
        /// Gets the square root of each component.
        /// </summary>
        public static Matrix4x4 sqrt(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Sqrt(v.M00), (float)Math.Sqrt(v.M01), (float)Math.Sqrt(v.M02), (float)Math.Sqrt(v.M03), (float)Math.Sqrt(v.M10), (float)Math.Sqrt(v.M11), (float)Math.Sqrt(v.M12), (float)Math.Sqrt(v.M13), (float)Math.Sqrt(v.M20), (float)Math.Sqrt(v.M21), (float)Math.Sqrt(v.M22), (float)Math.Sqrt(v.M23), (float)Math.Sqrt(v.M30), (float)Math.Sqrt(v.M31), (float)Math.Sqrt(v.M32), (float)Math.Sqrt(v.M33));
        }

        #endregion
        #region step

        /// <summary>
        /// Performs the step function to the specified Vector1 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Vector1 step(Vector1 v1, Vector1 v2)
        {
            return new Vector1(v1.X >= v2.X ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Vector2 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Vector2 step(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X >= v2.X ? 1 : 0, v1.Y >= v2.Y ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Vector3 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Vector3 step(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X >= v2.X ? 1 : 0, v1.Y >= v2.Y ? 1 : 0, v1.Z >= v2.Z ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Vector4 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Vector4 step(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.X >= v2.X ? 1 : 0, v1.Y >= v2.Y ? 1 : 0, v1.Z >= v2.Z ? 1 : 0, v1.W >= v2.W ? 1 : 0);
        }


        /// <summary>
        /// Performs the step function to the specified Matrix1x1 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix1x1 step(Matrix1x1 v1, Matrix1x1 v2)
        {
            return new Matrix1x1(v1.M00 >= v2.M00 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix1x2 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix1x2 step(Matrix1x2 v1, Matrix1x2 v2)
        {
            return new Matrix1x2(v1.M00 >= v2.M00 ? 1 : 0, v1.M01 >= v2.M01 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix1x3 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix1x3 step(Matrix1x3 v1, Matrix1x3 v2)
        {
            return new Matrix1x3(v1.M00 >= v2.M00 ? 1 : 0, v1.M01 >= v2.M01 ? 1 : 0, v1.M02 >= v2.M02 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix1x4 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix1x4 step(Matrix1x4 v1, Matrix1x4 v2)
        {
            return new Matrix1x4(v1.M00 >= v2.M00 ? 1 : 0, v1.M01 >= v2.M01 ? 1 : 0, v1.M02 >= v2.M02 ? 1 : 0, v1.M03 >= v2.M03 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix2x1 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix2x1 step(Matrix2x1 v1, Matrix2x1 v2)
        {
            return new Matrix2x1(v1.M00 >= v2.M00 ? 1 : 0, v1.M10 >= v2.M10 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix2x2 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix2x2 step(Matrix2x2 v1, Matrix2x2 v2)
        {
            return new Matrix2x2(v1.M00 >= v2.M00 ? 1 : 0, v1.M01 >= v2.M01 ? 1 : 0, v1.M10 >= v2.M10 ? 1 : 0, v1.M11 >= v2.M11 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix2x3 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix2x3 step(Matrix2x3 v1, Matrix2x3 v2)
        {
            return new Matrix2x3(v1.M00 >= v2.M00 ? 1 : 0, v1.M01 >= v2.M01 ? 1 : 0, v1.M02 >= v2.M02 ? 1 : 0, v1.M10 >= v2.M10 ? 1 : 0, v1.M11 >= v2.M11 ? 1 : 0, v1.M12 >= v2.M12 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix2x4 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix2x4 step(Matrix2x4 v1, Matrix2x4 v2)
        {
            return new Matrix2x4(v1.M00 >= v2.M00 ? 1 : 0, v1.M01 >= v2.M01 ? 1 : 0, v1.M02 >= v2.M02 ? 1 : 0, v1.M03 >= v2.M03 ? 1 : 0, v1.M10 >= v2.M10 ? 1 : 0, v1.M11 >= v2.M11 ? 1 : 0, v1.M12 >= v2.M12 ? 1 : 0, v1.M13 >= v2.M13 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix3x1 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix3x1 step(Matrix3x1 v1, Matrix3x1 v2)
        {
            return new Matrix3x1(v1.M00 >= v2.M00 ? 1 : 0, v1.M10 >= v2.M10 ? 1 : 0, v1.M20 >= v2.M20 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix3x2 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix3x2 step(Matrix3x2 v1, Matrix3x2 v2)
        {
            return new Matrix3x2(v1.M00 >= v2.M00 ? 1 : 0, v1.M01 >= v2.M01 ? 1 : 0, v1.M10 >= v2.M10 ? 1 : 0, v1.M11 >= v2.M11 ? 1 : 0, v1.M20 >= v2.M20 ? 1 : 0, v1.M21 >= v2.M21 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix3x3 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix3x3 step(Matrix3x3 v1, Matrix3x3 v2)
        {
            return new Matrix3x3(v1.M00 >= v2.M00 ? 1 : 0, v1.M01 >= v2.M01 ? 1 : 0, v1.M02 >= v2.M02 ? 1 : 0, v1.M10 >= v2.M10 ? 1 : 0, v1.M11 >= v2.M11 ? 1 : 0, v1.M12 >= v2.M12 ? 1 : 0, v1.M20 >= v2.M20 ? 1 : 0, v1.M21 >= v2.M21 ? 1 : 0, v1.M22 >= v2.M22 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix3x4 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix3x4 step(Matrix3x4 v1, Matrix3x4 v2)
        {
            return new Matrix3x4(v1.M00 >= v2.M00 ? 1 : 0, v1.M01 >= v2.M01 ? 1 : 0, v1.M02 >= v2.M02 ? 1 : 0, v1.M03 >= v2.M03 ? 1 : 0, v1.M10 >= v2.M10 ? 1 : 0, v1.M11 >= v2.M11 ? 1 : 0, v1.M12 >= v2.M12 ? 1 : 0, v1.M13 >= v2.M13 ? 1 : 0, v1.M20 >= v2.M20 ? 1 : 0, v1.M21 >= v2.M21 ? 1 : 0, v1.M22 >= v2.M22 ? 1 : 0, v1.M23 >= v2.M23 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix4x1 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix4x1 step(Matrix4x1 v1, Matrix4x1 v2)
        {
            return new Matrix4x1(v1.M00 >= v2.M00 ? 1 : 0, v1.M10 >= v2.M10 ? 1 : 0, v1.M20 >= v2.M20 ? 1 : 0, v1.M30 >= v2.M30 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix4x2 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix4x2 step(Matrix4x2 v1, Matrix4x2 v2)
        {
            return new Matrix4x2(v1.M00 >= v2.M00 ? 1 : 0, v1.M01 >= v2.M01 ? 1 : 0, v1.M10 >= v2.M10 ? 1 : 0, v1.M11 >= v2.M11 ? 1 : 0, v1.M20 >= v2.M20 ? 1 : 0, v1.M21 >= v2.M21 ? 1 : 0, v1.M30 >= v2.M30 ? 1 : 0, v1.M31 >= v2.M31 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix4x3 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix4x3 step(Matrix4x3 v1, Matrix4x3 v2)
        {
            return new Matrix4x3(v1.M00 >= v2.M00 ? 1 : 0, v1.M01 >= v2.M01 ? 1 : 0, v1.M02 >= v2.M02 ? 1 : 0, v1.M10 >= v2.M10 ? 1 : 0, v1.M11 >= v2.M11 ? 1 : 0, v1.M12 >= v2.M12 ? 1 : 0, v1.M20 >= v2.M20 ? 1 : 0, v1.M21 >= v2.M21 ? 1 : 0, v1.M22 >= v2.M22 ? 1 : 0, v1.M30 >= v2.M30 ? 1 : 0, v1.M31 >= v2.M31 ? 1 : 0, v1.M32 >= v2.M32 ? 1 : 0);
        }

        /// <summary>
        /// Performs the step function to the specified Matrix4x4 objects.
        /// Gets v1 >= v2 ? 1 : 0
        /// </summary>
        public static Matrix4x4 step(Matrix4x4 v1, Matrix4x4 v2)
        {
            return new Matrix4x4(v1.M00 >= v2.M00 ? 1 : 0, v1.M01 >= v2.M01 ? 1 : 0, v1.M02 >= v2.M02 ? 1 : 0, v1.M03 >= v2.M03 ? 1 : 0, v1.M10 >= v2.M10 ? 1 : 0, v1.M11 >= v2.M11 ? 1 : 0, v1.M12 >= v2.M12 ? 1 : 0, v1.M13 >= v2.M13 ? 1 : 0, v1.M20 >= v2.M20 ? 1 : 0, v1.M21 >= v2.M21 ? 1 : 0, v1.M22 >= v2.M22 ? 1 : 0, v1.M23 >= v2.M23 ? 1 : 0, v1.M30 >= v2.M30 ? 1 : 0, v1.M31 >= v2.M31 ? 1 : 0, v1.M32 >= v2.M32 ? 1 : 0, v1.M33 >= v2.M33 ? 1 : 0);
        }

        #endregion
        #region tan

        public static float tan(float x)
        {
            return (float)Math.Tan(x);
        }

        /// <summary>
        /// Performs the tan function to the specified Vector1 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Vector1 tan(Vector1 v)
        {
            return new Vector1((float)Math.Tan(v.X));
        }

        /// <summary>
        /// Performs the tan function to the specified Vector2 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Vector2 tan(Vector2 v)
        {
            return new Vector2((float)Math.Tan(v.X), (float)Math.Tan(v.Y));
        }

        /// <summary>
        /// Performs the tan function to the specified Vector3 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Vector3 tan(Vector3 v)
        {
            return new Vector3((float)Math.Tan(v.X), (float)Math.Tan(v.Y), (float)Math.Tan(v.Z));
        }

        /// <summary>
        /// Performs the tan function to the specified Vector4 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Vector4 tan(Vector4 v)
        {
            return new Vector4((float)Math.Tan(v.X), (float)Math.Tan(v.Y), (float)Math.Tan(v.Z), (float)Math.Tan(v.W));
        }


        /// <summary>
        /// Performs the tan function to the specified Matrix1x1 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix1x1 tan(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Tan(v.M00));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix1x2 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix1x2 tan(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Tan(v.M00), (float)Math.Tan(v.M01));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix1x3 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix1x3 tan(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Tan(v.M00), (float)Math.Tan(v.M01), (float)Math.Tan(v.M02));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix1x4 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix1x4 tan(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Tan(v.M00), (float)Math.Tan(v.M01), (float)Math.Tan(v.M02), (float)Math.Tan(v.M03));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix2x1 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix2x1 tan(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Tan(v.M00), (float)Math.Tan(v.M10));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix2x2 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix2x2 tan(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Tan(v.M00), (float)Math.Tan(v.M01), (float)Math.Tan(v.M10), (float)Math.Tan(v.M11));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix2x3 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix2x3 tan(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Tan(v.M00), (float)Math.Tan(v.M01), (float)Math.Tan(v.M02), (float)Math.Tan(v.M10), (float)Math.Tan(v.M11), (float)Math.Tan(v.M12));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix2x4 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix2x4 tan(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Tan(v.M00), (float)Math.Tan(v.M01), (float)Math.Tan(v.M02), (float)Math.Tan(v.M03), (float)Math.Tan(v.M10), (float)Math.Tan(v.M11), (float)Math.Tan(v.M12), (float)Math.Tan(v.M13));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix3x1 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix3x1 tan(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Tan(v.M00), (float)Math.Tan(v.M10), (float)Math.Tan(v.M20));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix3x2 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix3x2 tan(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Tan(v.M00), (float)Math.Tan(v.M01), (float)Math.Tan(v.M10), (float)Math.Tan(v.M11), (float)Math.Tan(v.M20), (float)Math.Tan(v.M21));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix3x3 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix3x3 tan(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Tan(v.M00), (float)Math.Tan(v.M01), (float)Math.Tan(v.M02), (float)Math.Tan(v.M10), (float)Math.Tan(v.M11), (float)Math.Tan(v.M12), (float)Math.Tan(v.M20), (float)Math.Tan(v.M21), (float)Math.Tan(v.M22));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix3x4 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix3x4 tan(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Tan(v.M00), (float)Math.Tan(v.M01), (float)Math.Tan(v.M02), (float)Math.Tan(v.M03), (float)Math.Tan(v.M10), (float)Math.Tan(v.M11), (float)Math.Tan(v.M12), (float)Math.Tan(v.M13), (float)Math.Tan(v.M20), (float)Math.Tan(v.M21), (float)Math.Tan(v.M22), (float)Math.Tan(v.M23));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix4x1 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix4x1 tan(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Tan(v.M00), (float)Math.Tan(v.M10), (float)Math.Tan(v.M20), (float)Math.Tan(v.M30));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix4x2 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix4x2 tan(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Tan(v.M00), (float)Math.Tan(v.M01), (float)Math.Tan(v.M10), (float)Math.Tan(v.M11), (float)Math.Tan(v.M20), (float)Math.Tan(v.M21), (float)Math.Tan(v.M30), (float)Math.Tan(v.M31));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix4x3 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix4x3 tan(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Tan(v.M00), (float)Math.Tan(v.M01), (float)Math.Tan(v.M02), (float)Math.Tan(v.M10), (float)Math.Tan(v.M11), (float)Math.Tan(v.M12), (float)Math.Tan(v.M20), (float)Math.Tan(v.M21), (float)Math.Tan(v.M22), (float)Math.Tan(v.M30), (float)Math.Tan(v.M31), (float)Math.Tan(v.M32));
        }

        /// <summary>
        /// Performs the tan function to the specified Matrix4x4 object.
        /// Gets the tangent of each component of v.
        /// </summary>
        public static Matrix4x4 tan(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Tan(v.M00), (float)Math.Tan(v.M01), (float)Math.Tan(v.M02), (float)Math.Tan(v.M03), (float)Math.Tan(v.M10), (float)Math.Tan(v.M11), (float)Math.Tan(v.M12), (float)Math.Tan(v.M13), (float)Math.Tan(v.M20), (float)Math.Tan(v.M21), (float)Math.Tan(v.M22), (float)Math.Tan(v.M23), (float)Math.Tan(v.M30), (float)Math.Tan(v.M31), (float)Math.Tan(v.M32), (float)Math.Tan(v.M33));
        }

        #endregion
        #region tanh

        /// <summary>
        /// Performs the tanh function to the specified Vector1 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Vector1 tanh(Vector1 v)
        {
            return new Vector1((float)Math.Tanh(v.X));
        }

        /// <summary>
        /// Performs the tanh function to the specified Vector2 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Vector2 tanh(Vector2 v)
        {
            return new Vector2((float)Math.Tanh(v.X), (float)Math.Tanh(v.Y));
        }

        /// <summary>
        /// Performs the tanh function to the specified Vector3 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Vector3 tanh(Vector3 v)
        {
            return new Vector3((float)Math.Tanh(v.X), (float)Math.Tanh(v.Y), (float)Math.Tanh(v.Z));
        }

        /// <summary>
        /// Performs the tanh function to the specified Vector4 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Vector4 tanh(Vector4 v)
        {
            return new Vector4((float)Math.Tanh(v.X), (float)Math.Tanh(v.Y), (float)Math.Tanh(v.Z), (float)Math.Tanh(v.W));
        }


        /// <summary>
        /// Performs the tanh function to the specified Matrix1x1 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix1x1 tanh(Matrix1x1 v)
        {
            return new Matrix1x1((float)Math.Tanh(v.M00));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix1x2 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix1x2 tanh(Matrix1x2 v)
        {
            return new Matrix1x2((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M01));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix1x3 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix1x3 tanh(Matrix1x3 v)
        {
            return new Matrix1x3((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M01), (float)Math.Tanh(v.M02));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix1x4 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix1x4 tanh(Matrix1x4 v)
        {
            return new Matrix1x4((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M01), (float)Math.Tanh(v.M02), (float)Math.Tanh(v.M03));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix2x1 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix2x1 tanh(Matrix2x1 v)
        {
            return new Matrix2x1((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M10));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix2x2 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix2x2 tanh(Matrix2x2 v)
        {
            return new Matrix2x2((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M01), (float)Math.Tanh(v.M10), (float)Math.Tanh(v.M11));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix2x3 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix2x3 tanh(Matrix2x3 v)
        {
            return new Matrix2x3((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M01), (float)Math.Tanh(v.M02), (float)Math.Tanh(v.M10), (float)Math.Tanh(v.M11), (float)Math.Tanh(v.M12));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix2x4 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix2x4 tanh(Matrix2x4 v)
        {
            return new Matrix2x4((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M01), (float)Math.Tanh(v.M02), (float)Math.Tanh(v.M03), (float)Math.Tanh(v.M10), (float)Math.Tanh(v.M11), (float)Math.Tanh(v.M12), (float)Math.Tanh(v.M13));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix3x1 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix3x1 tanh(Matrix3x1 v)
        {
            return new Matrix3x1((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M10), (float)Math.Tanh(v.M20));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix3x2 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix3x2 tanh(Matrix3x2 v)
        {
            return new Matrix3x2((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M01), (float)Math.Tanh(v.M10), (float)Math.Tanh(v.M11), (float)Math.Tanh(v.M20), (float)Math.Tanh(v.M21));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix3x3 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix3x3 tanh(Matrix3x3 v)
        {
            return new Matrix3x3((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M01), (float)Math.Tanh(v.M02), (float)Math.Tanh(v.M10), (float)Math.Tanh(v.M11), (float)Math.Tanh(v.M12), (float)Math.Tanh(v.M20), (float)Math.Tanh(v.M21), (float)Math.Tanh(v.M22));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix3x4 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix3x4 tanh(Matrix3x4 v)
        {
            return new Matrix3x4((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M01), (float)Math.Tanh(v.M02), (float)Math.Tanh(v.M03), (float)Math.Tanh(v.M10), (float)Math.Tanh(v.M11), (float)Math.Tanh(v.M12), (float)Math.Tanh(v.M13), (float)Math.Tanh(v.M20), (float)Math.Tanh(v.M21), (float)Math.Tanh(v.M22), (float)Math.Tanh(v.M23));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix4x1 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix4x1 tanh(Matrix4x1 v)
        {
            return new Matrix4x1((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M10), (float)Math.Tanh(v.M20), (float)Math.Tanh(v.M30));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix4x2 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix4x2 tanh(Matrix4x2 v)
        {
            return new Matrix4x2((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M01), (float)Math.Tanh(v.M10), (float)Math.Tanh(v.M11), (float)Math.Tanh(v.M20), (float)Math.Tanh(v.M21), (float)Math.Tanh(v.M30), (float)Math.Tanh(v.M31));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix4x3 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix4x3 tanh(Matrix4x3 v)
        {
            return new Matrix4x3((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M01), (float)Math.Tanh(v.M02), (float)Math.Tanh(v.M10), (float)Math.Tanh(v.M11), (float)Math.Tanh(v.M12), (float)Math.Tanh(v.M20), (float)Math.Tanh(v.M21), (float)Math.Tanh(v.M22), (float)Math.Tanh(v.M30), (float)Math.Tanh(v.M31), (float)Math.Tanh(v.M32));
        }

        /// <summary>
        /// Performs the tanh function to the specified Matrix4x4 object.
        /// Gets the hyperbolic tangent of each component of v.
        /// </summary>
        public static Matrix4x4 tanh(Matrix4x4 v)
        {
            return new Matrix4x4((float)Math.Tanh(v.M00), (float)Math.Tanh(v.M01), (float)Math.Tanh(v.M02), (float)Math.Tanh(v.M03), (float)Math.Tanh(v.M10), (float)Math.Tanh(v.M11), (float)Math.Tanh(v.M12), (float)Math.Tanh(v.M13), (float)Math.Tanh(v.M20), (float)Math.Tanh(v.M21), (float)Math.Tanh(v.M22), (float)Math.Tanh(v.M23), (float)Math.Tanh(v.M30), (float)Math.Tanh(v.M31), (float)Math.Tanh(v.M32), (float)Math.Tanh(v.M33));
        }

        #endregion
        #region transpose
        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix1x1 transpose(Matrix1x1 m)
        {
            return new Matrix1x1(m.M00);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix2x1 transpose(Matrix1x2 m)
        {
            return new Matrix2x1(m.M00, m.M01);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix3x1 transpose(Matrix1x3 m)
        {
            return new Matrix3x1(m.M00, m.M01, m.M02);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix4x1 transpose(Matrix1x4 m)
        {
            return new Matrix4x1(m.M00, m.M01, m.M02, m.M03);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix1x2 transpose(Matrix2x1 m)
        {
            return new Matrix1x2(m.M00, m.M10);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix2x2 transpose(Matrix2x2 m)
        {
            return new Matrix2x2(m.M00, m.M10, m.M01, m.M11);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix3x2 transpose(Matrix2x3 m)
        {
            return new Matrix3x2(m.M00, m.M10, m.M01, m.M11, m.M02, m.M12);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix4x2 transpose(Matrix2x4 m)
        {
            return new Matrix4x2(m.M00, m.M10, m.M01, m.M11, m.M02, m.M12, m.M03, m.M13);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix1x3 transpose(Matrix3x1 m)
        {
            return new Matrix1x3(m.M00, m.M10, m.M20);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix2x3 transpose(Matrix3x2 m)
        {
            return new Matrix2x3(m.M00, m.M10, m.M20, m.M01, m.M11, m.M21);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix3x3 transpose(Matrix3x3 m)
        {
            return new Matrix3x3(m.M00, m.M10, m.M20, m.M01, m.M11, m.M21, m.M02, m.M12, m.M22);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix4x3 transpose(Matrix3x4 m)
        {
            return new Matrix4x3(m.M00, m.M10, m.M20, m.M01, m.M11, m.M21, m.M02, m.M12, m.M22, m.M03, m.M13, m.M23);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix1x4 transpose(Matrix4x1 m)
        {
            return new Matrix1x4(m.M00, m.M10, m.M20, m.M30);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix2x4 transpose(Matrix4x2 m)
        {
            return new Matrix2x4(m.M00, m.M10, m.M20, m.M30, m.M01, m.M11, m.M21, m.M31);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix3x4 transpose(Matrix4x3 m)
        {
            return new Matrix3x4(m.M00, m.M10, m.M20, m.M30, m.M01, m.M11, m.M21, m.M31, m.M02, m.M12, m.M22, m.M32);
        }

        /// <summary>
        /// Returns the transpose matrix for the specific object.
        /// </summary>
        public static Matrix4x4 transpose(Matrix4x4 m)
        {
            return new Matrix4x4(m.M00, m.M10, m.M20, m.M30, m.M01, m.M11, m.M21, m.M31, m.M02, m.M12, m.M22, m.M32, m.M03, m.M13, m.M23, m.M33);
        }

        #endregion
    }
}
