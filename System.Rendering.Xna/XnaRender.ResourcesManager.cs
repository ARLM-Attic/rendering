using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Rendering.Resourcing;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaIndexBuffer = Microsoft.Xna.Framework.Graphics.IndexBuffer;
using XnaTexture = Microsoft.Xna.Framework.Graphics.Texture;
using XnaTextureCube = Microsoft.Xna.Framework.Graphics.TextureCube;
using XnaVertexBuffer = Microsoft.Xna.Framework.Graphics.VertexBuffer;
using System.Unsafe;

namespace System.Rendering.Xna
{
    partial class XnaRender
    {
        public class XnaResourcesManager : ResourcesManagerBase,
          IResourceManagerOf<VertexBuffer>,
          IResourceManagerOf<IndexBuffer>,
          IResourceManagerOf<TextureBuffer>,
          IResourceManagerOf<CubeTextureBuffer>
        {
            public XnaResourcesManager(XnaRender render)
                : base(render)
            {
            }

            private GraphicsDevice Device
            {
                get { return ((XnaRender)render)._device; }
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
                    ((XnaRender.XnaResourcesManager)render.ResourcesManager).UnregisterByManager(this);
                }

                protected abstract void DisposeResource();

                public abstract Array GetData(int[] start, int[] ranks);

                public abstract void SetData(Array data, int[] start, int[] ranks);

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
                MethodInfo getDataMethod;
                MethodInfo setDataMethod;

                public VertexBufferResourceOnDeviceManager(IRenderDevice render, XnaVertexBuffer vb, Type type, int[] sizes)
                    : base(render, type, sizes)
                {
                    this.vb = vb;
                    this.Descriptor = DataComponentExtensors.GetDescriptor(type);

                    getDataMethod = vb.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name == "GetData").First(m => m.GetParameters().Length == 3).MakeGenericMethod(ElementType);
                    setDataMethod = vb.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name == "SetData").First(m => m.GetParameters().Length == 3).MakeGenericMethod(ElementType);
                }

                public DataDescription Descriptor { get; private set; }

                public override Array GetData(int[] start, int[] ranks)
                {
                    if (start == null || ranks == null)
                    {
                        start = new int[1];
                        ranks = new int[] { Length };
                    }
                    Array result = Array.CreateInstance(this.ElementType, Length);

                    getDataMethod.Invoke(this.vb, new object[] { result, start[0], ranks[0] });

                    return result;
                }

                public override void SetData(Array data, int[] start, int[] ranks)
                {
                    if (start == null || ranks == null)
                    {
                        start = new int[1];
                        ranks = new int[] { Length };
                    }

                    setDataMethod.Invoke(this.vb, new object[] { data, start[0], ranks[0] });
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

                var vertexDec = XnaTools.GetVertexDeclaration(elementType);
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
                MethodInfo getDataMethod;
                MethodInfo setDataMethod;

                public IndexBufferResourceOnDeviceManager(IRenderDevice render, XnaIndexBuffer ib, Type type, int[] sizes) :
                    base(render, type, sizes)
                {
                    this.ib = ib;
                    getDataMethod = ib.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name == "GetData").First(m => m.GetParameters().Length == 3).MakeGenericMethod (ElementType);
                    setDataMethod = ib.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name == "SetData").First(m => m.GetParameters().Length == 3).MakeGenericMethod(ElementType);
                }

                public XnaIndexBuffer IndexBuffer
                {
                    get { return ib; }
                }

                public override Array GetData(int[] start, int[] ranks)
                {
                    if (start == null || ranks == null)
                    {
                        start = new int[1];
                        ranks = new int[] { Length };
                    }
                    Array result = Array.CreateInstance(this.ElementType, Length);

                    getDataMethod.Invoke(this.ib, new object[] { result, start[0], ranks[0] });

                    return result;
                }

                public override void SetData(Array data, int[] start, int[] ranks)
                {
                    if (start == null || ranks == null)
                    {
                        start = new int[1];
                        ranks = new int[] { Length };
                    }

                    setDataMethod.Invoke(this.ib, new object[] { data, start[0], ranks[0] });
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

            #region [ IResourceManagerOf<TextureBuffer> ]

            #region [ Texture Buffer Manager ]

            internal class TextureBufferResourceOnDeviceManager : GSBufferResourceOnDeviceManager
            {
                private XnaTexture texture;

                MethodInfo getDataMethod;
                MethodInfo setDataMethod;

                public TextureBufferResourceOnDeviceManager(IRenderDevice render, XnaTexture texture, int[] sizes)
                    : base(render, typeof (CustomPixels.SRGBA), sizes)
                {
                    this.texture = texture;

                    switch (Rank)
                    {
                        case 1:
                        case 2:
                            getDataMethod = texture.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name == "GetData").First(m => m.GetParameters().Length == 5).MakeGenericMethod (ElementType);
                            setDataMethod = texture.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name == "SetData").First(m => m.GetParameters().Length == 5).MakeGenericMethod(ElementType);
                            break;
                        case 3:
                            getDataMethod = texture.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name == "GetData").First(m => m.GetParameters().Length == 10).MakeGenericMethod (ElementType);
                            setDataMethod = texture.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name == "SetData").First(m => m.GetParameters().Length == 10).MakeGenericMethod(ElementType);
                            break;
                    }
                }

                protected override void DisposeResource()
                {
                    texture.Dispose();
                }

                public override Array GetData(int[] start, int[] ranks)
                {
                    if (start == null && ranks != null || start != null && ranks == null)
                        throw new ArgumentNullException("Ranks or start are null but not null both.");

                    if (start == null) start = new int[Rank];
                    if (ranks == null) ranks = this.GetRanks();

                    Array result = Array.CreateInstance(ElementType, ranks.Aggregate(1, (a, x) => a * x));

                    switch (Rank)
                    {
                        case 1:
                            Rectangle r1 = new Rectangle(start[0], 1, ranks[0], 1);
                            var texture1D = (RenderTarget2D)texture;
                            getDataMethod.Invoke(texture1D, new object[] { 0, r1, result, 0, result.Length });
                            break;
                        case 2:
                            Rectangle r2 = new Rectangle(start[1], start[0], ranks[1], ranks[0]);
                            var texture2D = (RenderTarget2D)texture;
                            getDataMethod.Invoke(texture2D, new object[] { 0, r2, result, 0, result.Length });
                            break;
                        case 3:
                            var texture3D = (Texture3D)texture;
                            getDataMethod.Invoke(texture3D, new object[] { 0, start[2], start[1], start[2] + ranks[2], start[1] + ranks[1], start[0], start[0] + ranks[0], result, 0, result.Length });
                            break;
                    }
                    
                    return result;
                }

                public override void SetData(Array data, int[] start, int[] ranks)
                {
                    if (start == null || ranks == null)
                    {
                        start = new int[Rank];
                        ranks = this.GetRanks();
                    }

                    switch (Rank)
                    {
                        case 1:
                            Rectangle r1 = new Rectangle(start[0], 1, ranks[0], 1);
                            var texture1D = (RenderTarget2D)texture;
                            setDataMethod.Invoke(texture1D, new object[] { 0, r1, data, 0, data.Length });
                            break;
                        case 2:
                            Rectangle r2 = new Rectangle(start[1], start[0], ranks[1], ranks[0]);
                            var texture2D = (RenderTarget2D)texture;
                            setDataMethod.Invoke(texture2D, new object[] { 0, r2, data, 0, data.Length });
                            break;
                        case 3:
                            var texture3D = (Texture3D)texture;
                            setDataMethod.Invoke(texture3D, new object[] { 0, start[2], start[1], start[2] + ranks[2], start[1] + ranks[1], start[0], start[0] + ranks[0], data, 0, data.Length });
                            break;
                    }
                }

                public XnaTexture TextureBuffer
                {
                    get { return texture; }
                }
            }

            #endregion

            TextureBuffer IResourceManagerOf<TextureBuffer>.Create(Type elementType, int[] sizes)
            {
                Texture texture = null;
                switch (sizes.Length)
                {
                    case 2:
                        // TODO: fix the mipmapping, doesn't work correctly
                        // TODO: fix the pixel format, only support RGBA right now
                        texture = new RenderTarget2D(Device, sizes[1], sizes[0], false, SurfaceFormat.Vector4, DepthFormat.Depth24Stencil8);
                        break;
                    case 3:
                        texture = new Texture3D(Device, sizes[2], sizes[1], sizes[0], false, SurfaceFormat.Vector4);
                        break;
                    default:
                        throw new NotSupportedException();
                }

                TextureBufferResourceOnDeviceManager manager = new TextureBufferResourceOnDeviceManager(render, texture, sizes);
                return Register<TextureBuffer>(manager);
            }

            IResourceOnDeviceManager IResourceManagerOf<TextureBuffer>.GetManagerFor(TextureBuffer resource)
            {
                return GetManager(resource);
            }

            SupportMode IResourceManagerOf<TextureBuffer>.SupportMode
            {
                get { return SupportMode.Device; }
            }

            #endregion

            #region [ IResourceManagerOf<CubeTextureBuffer> ]

            internal class CubeTextureResourceOnDeviceManager : GSBufferResourceOnDeviceManager
            {
                private XnaTextureCube _texture;

                MethodInfo getDataMethod;
                MethodInfo setDataMethod;


                public CubeTextureResourceOnDeviceManager(IRenderDevice render, XnaTextureCube textureCube, int[] sizes)
                    : base(render, typeof(CustomPixels.SRGBA), sizes)
                {
                    _texture = textureCube;

                    getDataMethod = textureCube.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name == "GetData").First(m => m.GetParameters().Length == 6).MakeGenericMethod(ElementType);
                    setDataMethod = textureCube.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.Name == "SetData").First(m => m.GetParameters().Length == 6).MakeGenericMethod(ElementType);
                }

                protected override void DisposeResource()
                {
                    _texture.Dispose();
                }

                public override Array GetData(int[] start, int[] ranks)
                {
                    if (start == null && ranks != null || start != null && ranks == null)
                        throw new ArgumentNullException("Ranks or start are null but not null both.");

                    if (start == null) start = new int[Rank];
                    if (ranks == null) ranks = this.GetRanks();

                    Array dataStream = Array.CreateInstance(ElementType, ranks.Aggregate(1, (a, x) => a * x));

                    for (int i = start[0]; i < start[0] + ranks[0]; i++)
                    {
                        Rectangle r = new Rectangle(start[1], start[2], ranks[1], ranks[2]);
                        getDataMethod.Invoke(_texture, new object[] { (CubeMapFace)i, 0, r, dataStream, i * ranks[1] * ranks[2], ranks[1] * ranks[2] });
                    }

                    return dataStream;
                }

                public override void SetData(Array data, int[] start, int[] ranks)
                {
                    if (start == null || ranks == null)
                    {
                        start = new int[Rank];
                        ranks = this.GetRanks();
                    }

                    for (int i = start[0]; i < start[0] + ranks[0]; i++)
                    {
                        Rectangle r = new Rectangle(start[1], start[2], ranks[1], ranks[2]);
                        setDataMethod.Invoke(_texture, new object[] { (CubeMapFace)i, 0, r, data, i * ranks[1] * ranks[2], ranks[1] * ranks[2] });
                    }
                }

                public XnaTextureCube TextureBuffer
                {
                    get { return _texture; }
                }
            }

            CubeTextureBuffer IResourceManagerOf<CubeTextureBuffer>.Create(Type elementType, int[] sizes)
            {
                if (sizes.Length != 3 || sizes[0] != 6 || sizes[1] != sizes[2])
                    throw new ArgumentException();

                var textureCube = new XnaTextureCube(Device, sizes[1], false, SurfaceFormat.Vector4);
                var manager = new CubeTextureResourceOnDeviceManager(render, textureCube, sizes);
                return Register<CubeTextureBuffer>(manager);
            }

            IResourceOnDeviceManager IResourceManagerOf<CubeTextureBuffer>.GetManagerFor(CubeTextureBuffer resource)
            {
                return GetManager(resource);
            }

            SupportMode IResourceManagerOf<CubeTextureBuffer>.SupportMode
            {
                get { return SupportMode.Device; }
            }

            #endregion
        }
    }
}
