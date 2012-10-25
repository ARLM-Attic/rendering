using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Modeling;
using System.Maths;

namespace System.Rendering
{
    public static class Models
    {
        static IModel _diskTop32x32;
        internal static IModel diskTop32x32 { get { return _diskTop32x32 ?? (_diskTop32x32 = CreateDiskTop(32, 2)); } }

        internal static IModel CreateDiskTop(int slices, int stacks)
        {
            return ManifoldModel.Surface((u, v) =>
            {
                float a = u * (float)Math.PI * 2;
                float b = v;

                return new Vector3<float>(
                  (float)Math.Cos(a)*b,
                  1,
                  -(float)Math.Sin(a)*b
                );
            }, slices, stacks, true);
        }

        static IModel _diskBottom32x32;
        internal static IModel diskBottom32x32 { get { return _diskBottom32x32 ?? (_diskBottom32x32 = CreateDiskBottom(32, 32)); } }

        internal static IModel CreateDiskBottom(int slices, int stacks)
        {
            return ManifoldModel.Surface((u, v) =>
            {
                float a = u * (float)Math.PI * 2;
                float b = v;

                return new Vector3<float>(
                  (float)Math.Cos(a) * (1 - b),
                  0,
                  -(float)Math.Sin(a) * (1 - b)
                );
            }, slices, stacks, true);
        }

        static IModel _cylinderBorder32x32;
        internal static IModel cylinderBorder32x32
        {
            get { return _cylinderBorder32x32 ?? (_cylinderBorder32x32 = CreateCylinderBorder(32, 2)); }
        }

        internal static IModel CreateCylinderBorder(int slices, int stacks)
        {
            return ManifoldModel.Surface((u, v) =>
            {
                float a = u * (float)Math.PI * 2;
                float b = v;

                return new Vector3<float>(
                  (float)Math.Cos(a),
                  b,
                  (float)Math.Sin(a)
                );
            }, slices, stacks, true);
        }

        private static Teapot _Teapot = null;
        public static Teapot Teapot
        {
            get { return _Teapot ?? (_Teapot = new Teapot()); }
        }

        private static Cube _Cube;
        public static Cube Cube
        {
            get { return _Cube ?? (_Cube = new Cube()); }
        }

        private static ManifoldModel _Sphere;
        public static IModel Sphere
        {
            get
            {
                return _Sphere ?? (_Sphere = ManifoldModel.Surface((u, v) =>
                    {
                        float a = u * (float)Math.PI * 2;
                        float b = v * (float)Math.PI;

                        return new Vector3<float>(
                          (float)(Math.Cos(a) * Math.Sin(b)),
                          (float)(-Math.Cos(b)),
                          (float)(Math.Sin(a) * Math.Sin(b))
                        );
                    }));
            }
        }

        private static ManifoldModel _LowSphere;
        public static IModel LowSphere
        {
            get
            {
                return _LowSphere ?? (_LowSphere = ManifoldModel.Surface((u, v) =>
                {
                    float a = u * (float)Math.PI * 2;
                    float b = v * (float)Math.PI;

                    return new Vector3<float>(
                      (float)(Math.Cos(a) * Math.Sin(b)),
                      (float)(-Math.Cos(b)),
                      (float)(Math.Sin(a) * Math.Sin(b))
                    );
                }, 16, 7, true));
            }
        }

        private static ManifoldModel _HighSphere;
        public static IModel HighSphere
        {
            get
            {
                return _HighSphere ?? (_HighSphere = ManifoldModel.Surface((u, v) =>
                {
                    float a = u * (float)Math.PI * 2;
                    float b = v * (float)Math.PI;

                    return new Vector3<float>(
                      (float)(Math.Cos(a) * Math.Sin(b)),
                      (float)(-Math.Cos(b)),
                      (float)(Math.Sin(a) * Math.Sin(b))
                    );
                }, 64, 32, true));
            }
        }

        static IModel _Cylinder = null;
        public static IModel Cylinder
        {
            get { return _Cylinder ?? (_Cylinder = new Cylinder()); }
        }

        static ManifoldModel _PlaneXZ;
        public static IModel PlaneXZ
        {
            get
            {
                return _PlaneXZ ?? (_PlaneXZ = ManifoldModel.Surface((u, v) =>
                {
                    return new Vector3<float>((u - 0.5f) * 10, 0, (v - 0.5f) * 10);
                }, 2, 2, true));
            }
        }
    }
}
