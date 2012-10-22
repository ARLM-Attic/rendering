#define FLOAT

#if DECIMAL
using FLOATINGTYPE = System.Decimal;
#endif
#if FLOATINGTYPE
using FLOATINGTYPE = System.FLOATINGTYPE;
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
    public static class Matrices
    {
        #region Translation

        /// <summary>
        /// Builds a Matrix using specified offsets.
        /// </summary>
        /// <param name="xslide">x offsets</param>
        /// <param name="yslide">y offsets</param>
        /// <param name="zslide">z offsets</param>
        /// <returns>A Matrix structure that contains a translated transformation Matrices.</returns>
        public static Matrix4x4 Translate(FLOATINGTYPE xoffset, FLOATINGTYPE yoffset, FLOATINGTYPE zoffset)
        {
            return new Matrix4x4(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                xoffset, yoffset, zoffset, 1
                );
        }

        /// <summary>
        /// Builds a Matrix using specified offsets.
        /// </summary>
        /// <param name="vec">A Vector structure that contains the x-coordinate, y-coordinate, and z-coordinate offsets.</param>
        /// <returns>A Matrix structure that contains a translated transformation Matrices.</returns>
        public static Matrix4x4 Translate(Vector3 vec)
        {
            return Translate (vec.X, vec.Y, vec.Z);
        }

        #endregion

        #region Rotations
        
        /// <summary>
        /// Rotation Matrix around Z axis
        /// </summary>
        /// <param name="alpha">value in radian for rotation</param>
        public static Matrix4x4 RotateZ(FLOATINGTYPE alpha)
        {
            float cos = (float)Math.Cos(alpha);
            float sin = (float)Math.Sin(alpha);
            return new Matrix4x4(
                cos, -sin, 0 ,0,
                sin, cos, 0, 0,
                0,0,1,0,
                0,0,0,1
                );
        }

        /// <summary>
        /// Rotation Matrix around Z axis
        /// </summary>
        /// <param name="alpha">value in grades for rotation</param>
        public static Matrix4x4 RotateZGrad(FLOATINGTYPE alpha)
        {
            return RotateZ((FLOATINGTYPE)(alpha * System.Math.PI / 180));
        }

        /// <summary>
        /// Rotation Matrix around Z axis
        /// </summary>
        /// <param name="alpha">value in radian for rotation</param>
        public static Matrix4x4 RotateY(FLOATINGTYPE alpha)
        {
            float cos = (float)Math.Cos(alpha);
            float sin = (float)Math.Sin(alpha);
            return new Matrix4x4(
                cos,  0,  -sin,  0,
                0,  1,  0,  0,  
                sin,  0,  cos,  0,
                0,  0,  0,  1
                );
        }
        /// <summary>
        /// Rotation Matrix around Z axis
        /// </summary>
        /// <param name="alpha">value in grades for rotation</param>
        public static Matrix4x4 RotateYGrad(FLOATINGTYPE alpha)
        {
            return RotateY ((FLOATINGTYPE)(alpha * System.Math.PI / 180));
        }


        public static Matrix4x4 RotateRespectTo(Vector3 center, Vector3 direction, FLOATINGTYPE angle)
        {
            return GMath.mul(Matrices.Translate(center), GMath.mul(Matrices.Rotate(angle, direction), Matrices.Translate(center * -1)));
        }

        /// <summary>
        /// Rotation Matrix around Z axis
        /// </summary>
        /// <param name="alpha">value in radian for rotation</param>
        public static Matrix4x4 RotateX(FLOATINGTYPE alpha)
        {
            float cos = (float)Math.Cos(alpha);
            float sin = (float)Math.Sin(alpha);
            return new Matrix4x4 (
                1, 0, 0, 0,
                0, cos, -sin, 0,
                0, sin, cos, 0,
                0,  0,  0,  1
                );
        }

        /// <summary>
        /// Rotation Matrix around Z axis
        /// </summary>
        /// <param name="alpha">value in grades for rotation</param>
        public static Matrix4x4 RotateXGrad(FLOATINGTYPE alpha)
        {
            return RotateX((FLOATINGTYPE)(alpha * System.Math.PI / 180));
        }


        public static Matrix4x4 Rotate(FLOATINGTYPE angle, Vector3 dir)
        {
            float x = dir.X;
            float y = dir.Y;
            float z = dir.Z;
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            return new Matrix4x4(
                x * x * (1 - cos) + cos, y * x * (1 - cos) + z * sin, z * x * (1 - cos) - y * sin, 0,
                x * y * (1 - cos) - z * sin, y * y * (1 - cos) + cos, z * y * (1 - cos) + x * sin, 0,
                x * z * (1 - cos) + y * sin, y * z * (1 - cos) - x * sin, z * z * (1 - cos) + cos, 0,
                0, 0, 0, 1
                );
        }

        public static Matrix4x4 RotateGrad(FLOATINGTYPE angle, Vector3 dir)
        {
            return Rotate((FLOATINGTYPE)(System.Math.PI * angle / 180), dir);
        }

        #endregion

        #region Scale
        
        public static Matrix4x4 Scale(FLOATINGTYPE xscale, FLOATINGTYPE yscale, FLOATINGTYPE zscale)
        {
            return new Matrix4x4 (
                xscale, 0, 0, 0,
                0, yscale, 0, 0,
                0, 0, zscale, 0,
                0,  0,  0, 1);
        }
        public static Matrix4x4 Scale(Vector3 size)
        {
            return Scale (size.X, size.Y, size.Z);
        }

        public static Matrix4x4 ScaleRespectTo(Vector3 center, Vector3 size)
        {
            return GMath.mul (GMath.mul(Matrices.Translate(center), Matrices.Scale(size)), Matrices.Translate(center * -1));
        }
        public static Matrix4x4 ScaleRespectTo(Vector3 center, FLOATINGTYPE sx, FLOATINGTYPE sy, FLOATINGTYPE sz)
        {
            return ScaleRespectTo(center, new Vector3(sx, sy, sz));
        }

        #endregion

        #region Viewing

        public static Matrix4x4 LookAtLH(Vector3 camera, Vector3 target, Vector3 upVector)
        {
            Vector3 zaxis = GMath.normalize (target - camera);
            Vector3 xaxis = GMath.normalize(GMath.cross(upVector, zaxis));
            Vector3 yaxis = GMath.cross(zaxis, xaxis);

            return new Matrix4x4 (
                xaxis.X, yaxis.X, zaxis.X, 0,
				xaxis.Y, yaxis.Y, zaxis.Y, 0,
				xaxis.Z, yaxis.Z, zaxis.Z, 0,
				-GMath.dot(xaxis , camera), -GMath.dot(yaxis, camera),-GMath.dot(zaxis, camera), 1);
        }

        public static Matrix4x4 LookAtRH(Vector3 camera, Vector3 target, Vector3 upVector)
        {
            Vector3 zaxis = GMath.normalize(camera - target);
            Vector3 xaxis = GMath.normalize(GMath.cross(upVector, zaxis));
            Vector3 yaxis = GMath.cross(zaxis, xaxis);

            return new Matrix4x4 (
                xaxis.X, yaxis.X, zaxis.X, 0,
				xaxis.Y, yaxis.Y, zaxis.Y, 0,
				xaxis.Z, yaxis.Z, zaxis.Z, 0,
				-GMath.dot(xaxis , camera), -GMath.dot(yaxis, camera),-GMath.dot(zaxis, camera), 1);
        }

        #endregion

        #region Projection Methods

        /// <summary>
        /// Returns the near plane distance to a given projection
        /// </summary>
        /// <param name="proj">A Matrix structure containing the projection</param>
        /// <returns>A float value representing the distance.</returns>
        public static float ZnearPlane(Matrix4x4 proj)
        {
            Vector4 pos = GMath.mul (Vectors.Ow, proj.Inverse);
            return pos.Z / pos.W;
        }

        /// <summary>
        /// Returns the far plane distance to a given projection
        /// </summary>
        /// <param name="proj">A Matrix structure containing the projection</param>
        /// <returns>A float value representing the distance.</returns>
        public static float ZfarPlane(Matrix4x4 proj)
        {
            Vector4 targetPos = GMath.mul (new Vector4(Vectors.Front,1), proj.Inverse);
            return targetPos.Z / targetPos.W;
        }

        /// <summary>
        /// Returns if a given projection is Orthographic
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        public static bool IsOrthoProjection(Matrix4x4 proj)
        {
            if (proj.IsSingular) return true;

            Ray r = new Ray(Vectors.Up + Vectors.Right, Vectors.Front).Transform(proj.Inverse);
            return r.Direction.X == 0 && r.Direction.Y == 0;
        }

        public static Matrix4x4 PerspectiveFovLH(FLOATINGTYPE fieldOfView, FLOATINGTYPE aspectRatio, FLOATINGTYPE znearPlane, FLOATINGTYPE zfarPlane)
        {
            FLOATINGTYPE h = 1f / (float)Math.Tan(fieldOfView / 2);
            FLOATINGTYPE w = h * aspectRatio;
            
            return new Matrix4x4(
            w, 0, 0, 0,
            0, h, 0, 0,
            0, 0, zfarPlane / (zfarPlane - znearPlane), 1,
            0, 0, -znearPlane * zfarPlane / (zfarPlane - znearPlane), 0);
        }

        public static Matrix4x4 PerspectiveFovRH(FLOATINGTYPE fieldOfView, FLOATINGTYPE aspectRatio, FLOATINGTYPE znearPlane, FLOATINGTYPE zfarPlane)
        {
            FLOATINGTYPE h = 1f / (float)Math.Tan(fieldOfView / 2);
            FLOATINGTYPE w = h * aspectRatio;

            return new Matrix4x4(
            w, 0, 0, 0,
            0, h, 0, 0,
            0, 0, zfarPlane / (znearPlane - zfarPlane), -1,
            0, 0, znearPlane * zfarPlane / (znearPlane - zfarPlane), 0);
        }

        public static Matrix4x4 PerspectiveLH(FLOATINGTYPE width, FLOATINGTYPE height, FLOATINGTYPE znearPlane, FLOATINGTYPE zfarPlane)
        {
            return new Matrix4x4(
            2*znearPlane/width, 0, 0, 0,
            0, 2*znearPlane/height, 0, 0,
            0, 0, zfarPlane / (zfarPlane - znearPlane), 1,
            0, 0, znearPlane * zfarPlane / (znearPlane - zfarPlane), 0);
        }

        public static Matrix4x4 PerspectiveRH(FLOATINGTYPE width, FLOATINGTYPE height, FLOATINGTYPE znearPlane, FLOATINGTYPE zfarPlane)
        {
            return new Matrix4x4(
            2 * znearPlane / width, 0, 0, 0,
            0, 2 * znearPlane / height, 0, 0,
            0, 0, zfarPlane / (znearPlane - zfarPlane), -1,
            0, 0, znearPlane * zfarPlane / (znearPlane - zfarPlane), 0);
        }

        public static Matrix4x4 OrthoLH(FLOATINGTYPE width, FLOATINGTYPE height, FLOATINGTYPE znearPlane, FLOATINGTYPE zfarPlane)
        {
            return new Matrix4x4(
            2 / width, 0, 0, 0,
            0, 2 / height, 0, 0,
            0, 0, 1 / (zfarPlane - znearPlane), 0,
            0, 0, znearPlane / (znearPlane - zfarPlane), 1);
        }

        public static Matrix4x4 OrthoRH(FLOATINGTYPE width, FLOATINGTYPE height, FLOATINGTYPE znearPlane, FLOATINGTYPE zfarPlane)
        {
            return new Matrix4x4(
            2 / width, 0, 0, 0,
            0, 2 / height, 0, 0,
            0, 0, 1 / (znearPlane - zfarPlane), 0,
            0, 0, znearPlane / (znearPlane - zfarPlane), 1);
        }

        public static Matrix4x4 ProjectInPlane(Triangle plane, Vector3 source)
        {
            Vector4 normal = new Vector4(plane.Normal, GMath.length(plane.V2));

            FLOATINGTYPE dot =
                normal.X * source.X
                + normal.Y * source.Y
                + normal.Z * source.Z
                + normal.W * 0;

            FLOATINGTYPE M00 = dot - source.X * normal.X;
            FLOATINGTYPE M01 = 0 - source.X * normal.Y;
            FLOATINGTYPE M02 = 0 - source.X * normal.Z;
            FLOATINGTYPE M03 = 0 - source.X * normal.W;

            FLOATINGTYPE M10 = 0 - source.Y * normal.X;
            FLOATINGTYPE M11 = dot - source.Y * normal.Y;
            FLOATINGTYPE M12 = 0 - source.Y * normal.Z;
            FLOATINGTYPE M13 = 0 - source.Y * normal.W;

            FLOATINGTYPE M20 = 0 - source.Z * normal.X;
            FLOATINGTYPE M21 = 0 - source.Z * normal.Y;
            FLOATINGTYPE M22 = dot - source.Z * normal.Z;
            FLOATINGTYPE M23 = 0 - source.Z * normal.W;

            FLOATINGTYPE M30 = 0 - 0 * normal.X;
            FLOATINGTYPE M31 = 0 - 0 * normal.Y;
            FLOATINGTYPE M32 = 0 - 0 * normal.Z;
            FLOATINGTYPE M33 = dot - 0 * normal.W;

            return new Matrix4x4(
                M00, M01, M02, M03, 
                M10, M11, M12, M13,
                M20, M21, M22, M23, 
                M30, M31, M32, M33);
        }

        #endregion

        static Matrix4x4 _I, _O;

        static Matrices()
        {
            _I = new Matrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
            _O = new Matrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        /// <summary>
        /// Retrieves the Identity Matrix
        /// </summary>
        public static Matrix4x4 I { get { return _I; } }

        /// <summary>
        /// Retrieves the Zero Matrix
        /// </summary>
        public static Matrix4x4 O { get { return _O; } }

    }
}
