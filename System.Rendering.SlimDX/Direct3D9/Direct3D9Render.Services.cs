using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Services;
using System.Rendering.Forms;
using SlimDX.Direct3D9;
using System.Rendering.Modeling;
using System.Runtime.InteropServices;

namespace System.Rendering.Direct3D9
{
    partial class Direct3DRender
    {
        public class Direct3D9Services : ServicesManagerBase
        {
            public Direct3D9Services(Direct3DRender render) : base(render) { }

            protected override void InitializeServices()
            {
                var textureService = Create<LoaderService<TextureBuffer>>();
                textureService.Register(new Win32TextureLoader());

                var meshService = Create<MeshService>();
                meshService.Register(new Direct3D9MeshFactory(this.Render as Direct3DRender));

            }
        }
    }

    public class Direct3D9MeshFactory : IMeshFactory
    {
        Direct3DRender render;

        public Direct3D9MeshFactory(Direct3DRender render)
        {
            this.render = render;
        }

        public Modeling.IMeshManager Create(VertexBuffer vertexes, IndexBuffer indices)
        {
            return new Direct3D9MeshManager(render, vertexes, indices);
        }
    }

    class Direct3D9MeshManager : IMeshManager
    {
        Direct3DRender render;
        SlimDX.Direct3D9.Mesh internalMesh;

        public Direct3D9MeshManager(Direct3DRender render, VertexBuffer vertexes, IndexBuffer indices)
        {
            this.render = render;
            var resources = ((Direct3DRender.Direct3DResourcesManager)render.ResourcesManager);

            int stride;
            DataDescription description;
            var declaration = Direct3D9Tools.GetVertexDeclaration (vertexes.InnerElementType, out stride, out description);

            var flag = Marshal.SizeOf(indices.InnerElementType) == 4 ? MeshFlags.Managed | MeshFlags.Use32Bit : MeshFlags.Managed;

            internalMesh = new SlimDX.Direct3D9.Mesh(render.Device, indices.Length / 3, vertexes.Length, flag, declaration);

            Vertexes = resources.Wrap(internalMesh.VertexBuffer, vertexes.InnerElementType, vertexes.Length);
            Vertexes.Update(vertexes.GetData());

            Indices = resources.Wrap(internalMesh.IndexBuffer, indices.InnerElementType, indices.Length);
            Indices.Update(indices.GetData());
        }

        public Direct3D9MeshManager(Direct3DRender render, SlimDX.Direct3D9.Mesh mesh, Type vertexType, Type indexType)
        {
            this.render = render;
            var resources = ((Direct3DRender.Direct3DResourcesManager)render.ResourcesManager);

            internalMesh = mesh;

            Vertexes = resources.Wrap(internalMesh.VertexBuffer, vertexType, mesh.VertexCount);
            Indices = resources.Wrap(internalMesh.IndexBuffer, indexType, mesh.FaceCount * 3);
        }

        public VertexBuffer Vertexes
        {
            get;
            private set;
        }

        public IndexBuffer Indices
        {
            get;
            private set;
        }

        public IEnumerable<Maths.IntersectInfo> Intersect(Maths.Ray ray)
        {
            SlimDX.Direct3D9.IntersectInformation[] hits;
            float distance;
            int faceIndex;
            internalMesh.Intersects(new SlimDX.Ray(Direct3D9Tools.Convert(ray.Position), Direct3D9Tools.Convert(ray.Direction)), out distance, out faceIndex, out hits);
            return hits.Select(h => new Maths.IntersectInfo(h.U, h.V, h.Distance, new Maths.Triangle(new Maths.Vector3(0, 0, 0), new Maths.Vector3(0, 0, 0), new Maths.Vector3(0, 0, 0))));
        }

        public void ComputeNormals()
        {
            //int[] adj = new int[internalMesh.FaceCount * 3];
            //internalMesh.GenerateAdjacency(0.0001f);
            internalMesh.ComputeNormals();
        }

        public void ComputeTangents()
        {
            internalMesh.ComputeTangent(0, 0, 0, false);
        }

        public IMeshManager Tessellated(float segments)
        {
            var tessellated = SlimDX.Direct3D9.Mesh.TessellateNPatches(internalMesh, segments, false);
            return new Direct3D9MeshManager(render, tessellated, Vertexes.InnerElementType, Indices.InnerElementType);
        }

        public void InternalDraw(ITessellator tessellator)
        {
            if (tessellator.Render != this.render)
                throw new ArgumentException();

            internalMesh.DrawSubset(0);
        }

        public void Dispose()
        {
            internalMesh.Dispose();
        }

        public void WeldVertexes(float epsilon)
        {
            internalMesh.WeldVertices(WeldFlags.WeldAll, new WeldEpsilons { Normal = epsilon, Position = epsilon });
        }
    }
}
