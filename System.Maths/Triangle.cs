using System;
using System.Collections.Generic;
using System.Text;

namespace System.Maths
{
    public sealed class Triangle
    {
        public readonly Vector3 V1, V2, V3;
        public Triangle(Vector3 V1, Vector3 V2, Vector3 V3)
        {
            this.V1 = V1;
            this.V2 = V2;
            this.V3 = V3;
        }

        public Triangle Transform(Matrix4x4 app)
        {
            return new Triangle(
                                (Vector3)GMath.mul(new Vector4(V1, 1), app),
                                (Vector3)GMath.mul(new Vector4(V2, 1), app),
                                (Vector3)GMath.mul(new Vector4(V3, 1), app));
        }


        public static readonly Triangle PlaneXY = new Triangle(Vectors.XAxis, Vectors.O, Vectors.YAxis);
        public static readonly Triangle PlaneYZ = new Triangle(Vectors.YAxis, Vectors.O, Vectors.ZAxis);
        public static readonly Triangle PlaneXZ = new Triangle(Vectors.XAxis, Vectors.O, Vectors.ZAxis);

        public Vector3 Normal
        {
            get
            {
                Vector3 U = V3 - V1;
                Vector3 V = V2 - V1;
								return GMath.normalize(GMath.cross(V, U));
            }
        }

        public Vector3 ClosestTo(Vector3 target)
        {
            Vector3 v1 = this.V2 - this.V1;
            Vector3 v2 = this.V3 - this.V1;
            Vector3 v = target - this.V1;

            if (GMath.dot(v1, v1) == 0)
            {
                Vector3 temp = v2;
                v2 = v1;
                v1 = temp;
            }

            float a11 = GMath.dot(v1, v1);
            float a12 = GMath.dot(v1, v2);
            float a13 = GMath.dot(v1, v);
            float a21 = GMath.dot(v2, v1);
            float a22 = GMath.dot(v2, v2);
            float a23 = GMath.dot(v2, v);

            if (a11 == 0)
                return target;

            a23 = a13 * a21 - a23 * a11;
            a22 = a12 * a21 - a22 * a11;
            a21 = 0;

            if (a22 == 0)
                return target;
            float beta = a23 / a22;
            float alpha = (a13 - a12 * beta) / a11;

            return (v1 * alpha + v2 * beta) + this.V1;
        }

        public Vector3 Reflect(Vector3 target)
        {
            Vector3 proj = ClosestTo(target);
            return (proj - target) + proj;
        }

        bool Inside(Ray ray, Vector3 P1, Vector3 P2)
        {
            Vector3 v1 = P1 - ray.Position;
            Vector3 v2 = P2 - ray.Position;
            Vector3 normal = GMath.cross(v1, v2);
            return GMath.dot (normal, ray.Direction) >= 0;
        }

        public bool Intersect(Ray ray, out IntersectInfo info)
        {
            info = null;
            if (Intersect(ray))
                return IntersectPlane(ray, out info) && info.U >= 0 && info.V >= 0 && info.U + info.V <= 1;
            return false;
        }

        public bool IntersectPlane(Ray ray, out IntersectInfo info)
        {
            float U = 0;
            float V = 0;
            float dist = 0;

            Matrix4x4 m = new Matrix4x4(
                V1.X - V2.X, V3.X - V2.X, -(float)ray.Direction.X, 0,
                V1.Y - V2.Y, V3.Y - V2.Y, -(float)ray.Direction.Y, 0,
                V1.Z - V2.Z, V3.Z - V2.Z, -(float)ray.Direction.Z, 0,
                0, 0, 0, 1
                );

            if (m.IsSingular)
            {
                info = null;
                return false;
            }

            Matrix4x4 inverse = m.Inverse;

            Vector4 res = GMath.mul (new Vector4(ray.Position.X - V2.X, ray.Position.Y - V2.Y, ray.Position.Z - V2.Z, 1), inverse);

            U = res.X;
            V = res.Y;
            dist = res.Z;

            info = new IntersectInfo(U, V, dist, this);

            return true;
        }

        public double Area
        {
            get
            {
                double A = GMath.length(V2 - V1);
                double B = GMath.length(V3 - V1);
                double C = GMath.length(V3 - V2);
                return A * System.Math.Sqrt(C * C - System.Math.Pow((A * A - B * B + C * C) / (2 * A), 2)) / 2;
            }
        }

        public double Angle(Ray ray)
        {
            return Math.Acos(GMath.dot(GMath.normalize(ray.Direction), this.Normal));
        }

        public bool Intersect(Ray ray)
        {
            bool first;
            return ((first = Inside(ray, this.V1, this.V2)) == Inside(ray, this.V2, this.V3) && (first == Inside(ray, this.V3, this.V1)));
        }
    }
}
