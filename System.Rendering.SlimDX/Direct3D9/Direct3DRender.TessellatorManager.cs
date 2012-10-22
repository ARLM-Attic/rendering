using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Modeling;
using D3D = SlimDX.Direct3D9;
using SlimDX.Direct3D9;
using System.Rendering.Resourcing;

namespace System.Rendering.Direct3D9
{
    partial class Direct3DRender
    {
        public class TessellatorManager : Direct3DRender.TessellatorBase, IDisposable,
            ITessellatorOf<Basic>
        {
            public TessellatorManager(Direct3DRender render)
                : base(render)
            {
            }

            protected Device device { get { return ((Direct3DRender)render).device; } }

            struct DeclarationInfo
            {
                public VertexDeclaration Declaration;
                public DataDescription Description;
                public int Stride;

                internal void Dispose()
                {
                    Declaration.Dispose();
                }
            }
            private Dictionary<int, DeclarationInfo> __cachedDeclarations = new Dictionary<int, DeclarationInfo>();

            void ITessellatorOf<Basic>.Draw(Basic primitive)
            {
                var primitiveType = Direct3D9Tools.Convert (primitive.Type);
                var vertexElementToken = primitive.VertexBuffer.InnerElementType.MetadataToken;

                if (!__cachedDeclarations.ContainsKey(vertexElementToken))
                {
                    DeclarationInfo info = new DeclarationInfo();

                    info.Declaration = new VertexDeclaration(device, Direct3D9Tools.GetVertexDeclaration(primitive.VertexBuffer.InnerElementType, out info.Stride, out info.Description));
                    __cachedDeclarations.Add(vertexElementToken, info);
                }

                var declarationInfo = __cachedDeclarations[vertexElementToken];

                device.VertexDeclaration = declarationInfo.Declaration;

                //var vertexFormat = VertexInformation.FormatFromDeclarator(vertexDec.GetDeclaration());

                var effectManager = (render as Direct3DRender).Manager;

                if (effectManager != null)
                    effectManager.UpdateAndApplyEffect(declarationInfo.Description);

                VertexBuffer finalVertexBuffer;
                IndexBuffer finalIndexBuffer;

                if (primitive.VertexBuffer.Render != this.render)
                    finalVertexBuffer = primitive.VertexBuffer.Clone(this.render) as VertexBuffer; // allocates temporaly the vertex buffer at render.
                else
                    finalVertexBuffer = primitive.VertexBuffer;

                if (primitive.Indexes != null)
                {
                    if (primitive.Indexes.Render != this.render)
                        finalIndexBuffer = primitive.Indexes.Clone(this.render) as IndexBuffer; // allocates temporaly the index buffer at render.
                    else
                        finalIndexBuffer = primitive.Indexes;
                }
                else
                    finalIndexBuffer = null;

                if (finalIndexBuffer == null) // Draw primitive
                {
                    var vb = ((Direct3DResourcesManager.VertexBufferResourceOnDeviceManager) this.Resources.GetManagerFor<VertexBuffer>(finalVertexBuffer)).VertexBuffer;
                    
                    device.SetStreamSource (0, vb, 0, declarationInfo.Stride);

                    device.DrawPrimitives(primitiveType, primitive.StartIndex, Direct3D9Tools.PrimitiveCount(primitive.Count, primitive.Type));
                }
                else // Draw indexed primitive
                {
                    var vb = ((Direct3DResourcesManager.VertexBufferResourceOnDeviceManager)this.Resources.GetManagerFor<VertexBuffer>(finalVertexBuffer)).VertexBuffer;
                    var ib = ((Direct3DResourcesManager.IndexBufferResourceOnDeviceManager)this.Resources.GetManagerFor<IndexBuffer>(finalIndexBuffer)).IndexBuffer;

                    device.Indices = ib;

                    device.SetStreamSource(0, vb, 0, declarationInfo.Stride);

                    device.DrawIndexedPrimitives(primitiveType, 0, 0, primitive.VertexBuffer.Length, primitive.StartIndex, Direct3D9Tools.PrimitiveCount(primitive.Count, primitive.Type));
                }

                if (effectManager != null)
                    effectManager.ClearAndUnApplyEffect();

                device.Indices = null;
                device.SetStreamSource(0, null, 0, 0);
                device.VertexDeclaration = null;

                if (primitive.VertexBuffer != finalVertexBuffer)
                    finalVertexBuffer.Dispose();
                if (primitive.Indexes != finalIndexBuffer)
                    finalIndexBuffer.Dispose();
            }

            public void Dispose()
            {
                foreach (var dec in __cachedDeclarations.Values)
                    dec.Dispose();
            }
        }
    }

}
