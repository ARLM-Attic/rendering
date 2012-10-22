using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Modelling;
using Microsoft.Xna.Framework.Graphics;

namespace System.Rendering.Xna
{
  partial class Direct3DRender
  {
    public class Tessellator : TessellatorBase,
      ITessellatorOf<Basic>
    {
      public Tessellator(Direct3DRender render)
        : base(render)
      {
      }

      private GraphicsDevice Device
      {
        get { return ((Direct3DRender)render).device; }
      }

      private Microsoft.Xna.Framework.Graphics.Effect CurrentEffect
      {
        get { return ((RenderStatesManager)((Direct3DRender)render).RenderStates).basicEffect; }
      }

      #region [ ITessellatorOf<Basic> ]

      void ITessellatorOf<Basic>.Draw(Basic primitive)
      {
        var primitiveType = Direct3DTools.ToXnaPrimitiveType(primitive.Type);

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
          var vb = ((ResourcesManager.VertexBufferResourceOnDeviceManager)this.Resources.GetManagerFor<VertexBuffer>(finalVertexBuffer)).VertexBuffer;

          Device.SetVertexBuffer(vb);

          CurrentEffect.CurrentTechnique.Passes[0].Apply();
          Device.DrawPrimitives(primitiveType, 0, Direct3DTools.PrimitiveCount(primitive.VertexBuffer.Length, primitive.Type));
        }
        else // Draw indexed primitive
        {
          var vb = ((ResourcesManager.VertexBufferResourceOnDeviceManager)this.Resources.GetManagerFor<VertexBuffer>(finalVertexBuffer)).VertexBuffer;
          var ib = ((ResourcesManager.IndexBufferResourceOnDeviceManager)this.Resources.GetManagerFor<IndexBuffer>(finalIndexBuffer)).IndexBuffer;

          Device.SetVertexBuffer(vb);
          Device.Indices = ib;

          CurrentEffect.CurrentTechnique.Passes[0].Apply();
          Device.DrawIndexedPrimitives(primitiveType, 0, 0, primitive.VertexBuffer.Length, 0, Direct3DTools.PrimitiveCount(primitive.Indexes.Length, primitive.Type));
        }

        Device.Indices = null;
        Device.SetVertexBuffer(null);

        if (primitive.VertexBuffer != finalVertexBuffer)
          finalVertexBuffer.Dispose();
        if (primitive.Indexes != finalIndexBuffer)
          finalIndexBuffer.Dispose();
      }

      #endregion
    }
  }
}
