using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Modeling;
using System.Maths;
using System.Rendering.Resourcing;

namespace System.Rendering
{
    public static class Models
    {
        static IModel _diskTop32x32;
        internal static IModel diskTop32x32 { get { return _diskTop32x32 ?? (_diskTop32x32 = CreateDiskTop(32, 2)); } }

        internal static IModel CreateDiskTop(int slices, int stacks)
        {
            return Surface((u, v) =>
            {
                float a = u * (float)Math.PI * 2;
                float b = v;

                return new Vector3<float>(
                  (float)Math.Cos(a)*b,
                  1,
                  (float)Math.Sin(a)*b
                );
            }, slices, stacks, true);
        }

        static IModel _diskBottom32x32;
        internal static IModel diskBottom32x32 { get { return _diskBottom32x32 ?? (_diskBottom32x32 = CreateDiskBottom(32, 32)); } }

        internal static IModel CreateDiskBottom(int slices, int stacks)
        {
            return Surface((u, v) =>
            {
                float a = u * (float)Math.PI * 2;
                float b = v;

                return new Vector3<float>(
                  (float)Math.Cos(a) * (1 - b),
                  0,
                  (float)Math.Sin(a) * (1 - b)
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
            return Surface((u, v) =>
            {
                float a = u * (float)Math.PI * 2;
                float b = v;

                return new Vector3<float>(
                  (float)Math.Cos(a),
                  1-b,
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
        public static Mesh Cube
        {
            get { return _Cube ?? (_Cube = new Cube()); }
        }

        private static SurfaceModel _Sphere;
        public static Mesh Sphere
        {
            get
            {
                return _Sphere ?? (_Sphere = Surface((u, v) =>
                    {
                        float a = u * (float)Math.PI * 2;
                        float b = v * (float)Math.PI;

                        return new Vector3<float>(
                          (float)(Math.Cos(a) * Math.Sin(b)),
                          (float)(Math.Cos(b)),
                          (float)(Math.Sin(a) * Math.Sin(b))
                        );
                    }));
            }
        }

        private static SurfaceModel _LowSphere;
        public static Mesh LowSphere
        {
            get
            {
                return _LowSphere ?? (_LowSphere = Surface((u, v) =>
                {
                    float a = u * (float)Math.PI * 2;
                    float b = v * (float)Math.PI;

                    return new Vector3<float>(
                      (float)(Math.Cos(a) * Math.Sin(b)),
                      (float)(Math.Cos(b)),
                      (float)(Math.Sin(a) * Math.Sin(b))
                    );
                }, 16, 7, true));
            }
        }

        private static SurfaceModel _HighSphere;
        public static Mesh HighSphere
        {
            get
            {
                return _HighSphere ?? (_HighSphere = Surface((u, v) =>
                {
                    float a = u * (float)Math.PI * 2;
                    float b = v * (float)Math.PI;

                    return new Vector3<float>(
                      (float)(Math.Cos(a) * Math.Sin(b)),
                      (float)(Math.Cos(b)),
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

        static SurfaceModel _PlaneXZ;
        public static Mesh PlaneXZ
        {
            get
            {
                return _PlaneXZ ?? (_PlaneXZ = Surface((u, v) =>
                {
                    return new Vector3<float>((u - 0.5f) * 10, 0, (0.5f - v) * 10);
                }, 2, 2, true));
            }
        }

        public static SurfaceModel Surface(Func<float, float, Vector3> surface)
        {
            return Surface(surface, 32, 32, true);
        }
        public static SurfaceModel Surface(Func<float, float, Vector3> surface, int slices, int stacks, bool generateNormals)
        {
            return new SurfaceModel(Manifold.Surface(surface), slices, stacks, generateNormals);
        }
        public static SurfaceModel Surface(Func<float, float, Vector3> surface, int slices, int stacks)
        {
            return Surface (surface, slices, stacks, true);
        }

        public static CurveModel Curve(Func<float, Vector3> curve)
        {
            return new CurveModel(Manifold.Curve(curve));
        }

        public static CurveModel Curve(Func<float, Vector3> curve, int slices)
        {
            return new CurveModel(Manifold.Curve(curve), slices);
        }

        public static IModel Single<GP>(GP primitive) where GP : struct, IGraphicPrimitive
        {
            return new SingleModel<GP>(primitive);
        }

				public static IModel Union(IModel model1, IModel model2)
				{
					return CSGOperations.Union(model1, model2);
				}

        public static IModel Union(params IModel[] models)
        {
            if (models.Length == 0)
                return Mesh<PositionNormalData>.Empty;
            
            IModel result = models[0];

            for (int i = 1; i < models.Length; i++)
                result = Models.Union(result, models[i]);

            return result;
        }
        public static IModel Intersection(IModel model1, IModel model2)
        {
            return CSGOperations.Intersect(model1, model2);
        }

        public static IModel Subtract(IModel model1, IModel model2)
        {
            return CSGOperations.Subtract(model1, model2);
        }

        public static ModelGroup Group(params IModel[] models)
        {
            return new ModelGroup(models);
        }
    }

    public static class ModelExtensors
    {
        public static IModel Transformed(this IModel model, params Matrix4x4[] transforms)
        {
            return model.Transformed(transforms.Aggregate(Matrices.I, (a, m) => GMath.mul(a, m)));
        }

        public static IModel AsModel<GP>(this GP graphicPrimitive) where GP : struct, IGraphicPrimitive
        {
            return Models.Single(graphicPrimitive);
        }

        public static IModel Translated(this IModel model, float dx, float dy, float dz)
        {
            return model.Transformed(Matrices.Translate(dx, dy, dz));
        }

        public static IModel Rotated(this IModel model, float angle, Vector3 direction)
        {
            return model.Transformed(Matrices.Rotate(angle, direction));
        }

        public static IModel Scaled(this IModel model, float sx, float sy, float sz)
        {
            return model.Transformed(Matrices.Scale(sx, sy, sz));
        }

        public static IModel Rotated(this IModel model, Vector3 xAxis, Vector3 yAxis, Vector3 zAxis)
        {
            return model.Transformed(new Matrix4x4(
                xAxis.X, xAxis.Y, xAxis.Z, 0,
                yAxis.X, yAxis.Y, yAxis.Z, 0,
                zAxis.X, zAxis.Y, zAxis.Z, 0,
                0, 0, 0, 1
                ));
        }

        class ModelToMeshTransformer : ITessellator , ITessellatorOf<Basic>
        {
            public IRenderDevice Render
            {
                get { return null; }
            }

            public void Draw<GP>(GP primitive) where GP : struct, IGraphicPrimitive
            {
                if (this is ITessellatorOf<GP>)
                    ((ITessellatorOf<GP>)this).Draw((GP)primitive);
            }

            public bool IsSupported<GP>() where GP : struct, IGraphicPrimitive
            {
                return typeof(GP) == typeof(Basic);
            }

            List<PositionNormalData> data = new List<PositionNormalData>();
            List<uint> indexes = new List<uint>();

            void ITessellatorOf<Basic>.Draw(Basic primitive)
            {
                uint startIndex = (uint) data.Count;
                foreach (var t in primitive.GetTriangles())
                {
                    data.Add(new PositionNormalData { Position = t.V1, Normal = t.N1 });
                    data.Add(new PositionNormalData { Position = t.V2, Normal = t.N2 });
                    data.Add(new PositionNormalData { Position = t.V3, Normal = t.N3 });

                    indexes.AddRange (new uint[] { startIndex, startIndex + 1, startIndex + 2 });

                    startIndex += 3;
                }
            }

            public Mesh Mesh
            {
                get
                {
                    return new Mesh<PositionNormalData>(data.ToArray(), indexes.ToArray());
                }
            }
        }

        public static Mesh ToMesh(this IModel model)
        {
            if (model is Mesh)
                return (Mesh)model;

            ModelToMeshTransformer transformer = new ModelToMeshTransformer();

            model.Tesselate(transformer);

            return transformer.Mesh;
        }

    }
}
