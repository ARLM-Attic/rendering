using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaVertexBuffer = Microsoft.Xna.Framework.Graphics.VertexBuffer;
using XnaIndexBuffer = Microsoft.Xna.Framework.Graphics.IndexBuffer;
using System.Reflection;

namespace System.Rendering.Xna
{
  partial class Direct3DRender
  {
    public class ResourcesManager : ResourcesManagerBase,
      IResourceManagerOf<VertexBuffer>,
      IResourceManagerOf<IndexBuffer>
    {
      public ResourcesManager(Direct3DRender render)
        : base(render)
      {
      }

      private GraphicsDevice Device
      {
        get { return ((Direct3DRender)render).device; }
      }

      #region [ GetSet Buffers ]

      internal abstract class GSBufferResourceOnDeviceManager : ISettableResourceOnDeviceManager
      {
        int[] sizes;
        int length;
        Type internalType;
        IRenderDevice render;

        protected GSBufferResourceOnDeviceManager(IRenderDevice render, Type type, int[] sizes)
        {
          this.sizes = sizes;
          length = sizes.Aggregate(1, (e, a) => a * e);
          this.internalType = type;
          this.render = render;
        }

        public int Rank
        {
          get { return sizes.Length; }
        }

        public int GetLength(int dimension)
        {
          return sizes[dimension];
        }

        public int Length
        {
          get { return length; }
        }

        public void Release()
        {
          DisposeResource();
          ((Direct3DRender.ResourcesManager)render.ResourcesManager).UnregisterByManager(this);
        }

        protected abstract void DisposeResource();

        public abstract Array GetData(Type type, int[] start, int[] ranks);

        public abstract void SetData(Array data, int[] start);

        public Type ElementType
        {
          get { return internalType; }
        }

        public IRenderDevice Render
        {
          get { return render; }
        }
      }

      #endregion

      #region [ LockUnlock Buffers ]

      internal abstract class LUBufferResourceOnDeviceManager : ILockableResourceOnDeviceManager
      {
        int[] sizes;
        int length;
        Type internalType;
        IRenderDevice render;

        protected LUBufferResourceOnDeviceManager(IRenderDevice render, Type type, int[] sizes)
        {
          this.sizes = sizes;
          length = sizes.Aggregate(1, (e, a) => a * e);
          this.internalType = type;
          this.render = render;
        }

        public int Rank
        {
          get { return sizes.Length; }
        }

        public int GetLength(int dimension)
        {
          return sizes[dimension];
        }

        public int Length
        {
          get { return length; }
        }

        public void Release()
        {
          DisposeResource();
          ((Direct3DRender.ResourcesManager)render.ResourcesManager).UnregisterByManager(this);
        }

        protected abstract void DisposeResource();

        public abstract Array Lock(Type type, int[] start, int[] count);

        public abstract void Unlock();

        public Type ElementType
        {
          get { return internalType; }
        }

        public IRenderDevice Render
        {
          get { return render; }
        }
      }

      #endregion

      #region [ IResourceManagerOf<VertexBuffer> ]

      #region [ Vertex Buffer Manager ]

      internal class VertexBufferResourceOnDeviceManager : GSBufferResourceOnDeviceManager
      {
        XnaVertexBuffer vb;

        public VertexBufferResourceOnDeviceManager(IRenderDevice render, XnaVertexBuffer vb, Type type, int[] sizes)
          : base(render, type, sizes)
        {
          this.vb = vb;
        }

        public override Array GetData(Type type, int[] start, int[] ranks)
        {
          var getDataMethod = vb.GetType().GetMethods().Where(m => m.Name == "GetData").Skip(1).First().MakeGenericMethod(ElementType);
          Array data; 
          if (start == null && ranks == null)
          {
            data = Array.CreateInstance(ElementType, Length);
            getDataMethod.Invoke(vb, new object[] { data, 0, Length });
            return data;
          }
          
          data = Array.CreateInstance(ElementType, ranks[0]);
          getDataMethod.Invoke(vb, new object[] { data, start[0], ranks[0] });
          return data;
        }

        public override void SetData(Array data, int[] start)
        {
          var setDataMethod = vb.GetType().GetMethods().Where(m => m.Name == "SetData").Skip(1).First().MakeGenericMethod(ElementType);
          if (start == null)
            setDataMethod.Invoke(vb, new object[] { data, 0, data.Length });
          else
            setDataMethod.Invoke(vb, new object[] { data, start[0], data.Length });
        }

        protected override void DisposeResource()
        {
          vb.Dispose();
        }

        public XnaVertexBuffer VertexBuffer
        {
          get { return vb; }
        }
      }

      #endregion

      VertexBuffer IResourceManagerOf<VertexBuffer>.Create(Type elementType, int[] sizes)
      {
        int length = sizes.Aggregate(1, (e, a) => e * a);

        var vertexDec = Direct3DTools.GetVertexDeclaration(elementType);
        var manager = new VertexBufferResourceOnDeviceManager(render, new XnaVertexBuffer(Device, vertexDec, length, BufferUsage.None), elementType, sizes);
        return Register<VertexBuffer>(manager);
      }

      IResourceOnDeviceManager IResourceManagerOf<VertexBuffer>.GetManagerFor(VertexBuffer resource)
      {
        return GetManager(resource);
      }

      SupportMode IResourceManagerOf<VertexBuffer>.SupportMode
      {
        get { return SupportMode.Device; }
      }

      #endregion

      #region [ IResourceManagerOf<IndexBuffer> ]

      #region [ Index Buffer Manager ]

      internal class IndexBufferResourceOnDeviceManager : GSBufferResourceOnDeviceManager
      {
        XnaIndexBuffer ib;

        public IndexBufferResourceOnDeviceManager(IRenderDevice render, XnaIndexBuffer ib, Type type, int[] sizes) :
          base(render, type, sizes)
        {
          this.ib = ib;
        }

        public XnaIndexBuffer IndexBuffer
        {
          get { return ib; }
        }

        public override Array GetData(Type type, int[] start, int[] ranks)
        {
          var getDataMethod = ib.GetType().GetMethods().Where(m => m.Name == "GetData").Skip(1).First().MakeGenericMethod(ElementType);
          Array data;
          if (start == null && ranks == null)
          {
            data = Array.CreateInstance(ElementType, Length);
            getDataMethod.Invoke(ib, new object[] { data, 0, Length });
            return data;
          }

          data = Array.CreateInstance(ElementType, ranks[0]);
          getDataMethod.Invoke(ib, new object[] { data, start[0], ranks[0] });
          return data;
        }

        public override void SetData(Array data, int[] start)
        {
          var setDataMethod = ib.GetType().GetMethods().Where(m => m.Name == "SetData").Skip(1).First().MakeGenericMethod(ElementType);
          if (start == null)
            setDataMethod.Invoke(ib, new object[] { data, 0, Length });
          else
            setDataMethod.Invoke(ib, new object[] { data, start[0], data.Length });
        }

        protected override void DisposeResource()
        {
          ib.Dispose();
        }
      }

      #endregion

      IndexBuffer IResourceManagerOf<IndexBuffer>.Create(Type elementType, int[] sizes)
      {
        int length = sizes.Aggregate(1, (e, a) => e * a);

        var manager = new IndexBufferResourceOnDeviceManager(render, new XnaIndexBuffer(Device, elementType, length, BufferUsage.None), elementType, sizes);
        return Register<IndexBuffer>(manager);
      }

      IResourceOnDeviceManager IResourceManagerOf<IndexBuffer>.GetManagerFor(IndexBuffer resource)
      {
        return GetManager(resource);
      }

      SupportMode IResourceManagerOf<IndexBuffer>.SupportMode
      {
        get { return SupportMode.Device; }
      }

      #endregion
    }
  }
}
