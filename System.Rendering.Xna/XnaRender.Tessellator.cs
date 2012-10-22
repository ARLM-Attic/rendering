using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Modeling;
using Microsoft.Xna.Framework.Graphics;

namespace System.Rendering.Xna
{
  partial class XnaRender
  {
    public class XnaTessellator : TessellatorBase,
      ITessellatorOf<Basic>
    {
      public XnaTessellator(XnaRender render)
        : base(render)
      {
      }

      private GraphicsDevice Device
      {
        get { return ((XnaRender)render)._device; }
      }

      private XnaEffectManager EffectManager
      {
        get { return ((XnaRenderStatesManager)((XnaRender)render).RenderStates).effectManager; }
      }

      #region [ ITessellatorOf<Basic> ]

      void ITessellatorOf<Basic>.Draw(Basic primitive)
      {
        var primitiveType = XnaTools.ToXnaPrimitiveType(primitive.Type);

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
            var resource = ((XnaResourcesManager.VertexBufferResourceOnDeviceManager)this.Resources.GetManagerFor<VertexBuffer>(finalVertexBuffer));
          var vb = resource.VertexBuffer;

          Device.SetVertexBuffer(vb);

          EffectManager.UpdateAndApplyEffect(resource.Descriptor);
          Device.DrawPrimitives(primitiveType, 0, XnaTools.PrimitiveCount(primitive.VertexBuffer.Length, primitive.Type));
          EffectManager.ClearAndUnApplyEffect();
        }
        else // Draw indexed primitive
        {
            var resource = ((XnaResourcesManager.VertexBufferResourceOnDeviceManager)this.Resources.GetManagerFor<VertexBuffer>(finalVertexBuffer));
          var vb = resource.VertexBuffer;
          var ib = ((XnaResourcesManager.IndexBufferResourceOnDeviceManager)this.Resources.GetManagerFor<IndexBuffer>(finalIndexBuffer)).IndexBuffer;

          Device.SetVertexBuffer(vb);
          Device.Indices = ib;

          EffectManager.UpdateAndApplyEffect(resource.Descriptor);
          Device.DrawIndexedPrimitives(primitiveType, 0, 0, primitive.VertexBuffer.Length, 0, XnaTools.PrimitiveCount(primitive.Indexes.Length, primitive.Type));
          EffectManager.ClearAndUnApplyEffect();
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
