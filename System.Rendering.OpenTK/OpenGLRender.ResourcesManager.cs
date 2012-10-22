using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Rendering.Resourcing;
using System.Unsafe;
using OpenTK.Graphics.OpenGL;

namespace System.Rendering.OpenGL
{
    partial class OpenGLRender
    {
        public class OpenGLResourcesManager : RenderBase.ResourcesManagerBase, 
            IResourceManagerOf<VertexBuffer>,
            IResourceManagerOf<IndexBuffer>,
            IResourceManagerOf<TextureBuffer>,
            IResourceManagerOf<CubeTextureBuffer>
        {
            public OpenGLResourcesManager(OpenGLRender render)
                : base(render)
            {
            }

            Dictionary<GraphicResource, IResourceOnDeviceManager> bufferManagers = new Dictionary<GraphicResource, IResourceOnDeviceManager>();

            #region IResourceManagerOf<VertexBuffer>

            #region Vertex Buffer Manager

            internal class VertexBufferResourceOnDeviceManager : ISettableResourceOnDeviceManager
            {
                public VertexBufferResourceOnDeviceManager(IRenderDevice render, OpenGLVertexBuffer vb, Type type, int[] sizes)
                {
                    this.vb = vb;
                    this.sizes = sizes;
                    length = sizes.Aggregate(1, (e, a) => a * e);
                    this.internalType = type;
                    this.render = render;
                }

                OpenGLVertexBuffer vb;
                int[] sizes;
                int length;
                Type internalType;
                IRenderDevice render;

                public OpenGLVertexBuffer VertexBuffer
                {
                    get { return vb; }
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
                    vb.Dispose();
                }

                public Type ElementType
                {
                    get { return internalType; }
                }

                public Array GetData(int[] start, int[] size)
                {
                    Array result = null;
                    ((OpenGLRender)render).SyncExecute(() =>
                    {
                        if (start == null || size == null)
                            result = vb.GetData(0, this.length);
                        else
                            result = vb.GetData(start[0], size[0]);
                    });
                    return result;
                }

                public void SetData(Array data, int[] start, int[] ranks)
                {
                    ((OpenGLRender)render).SyncExecute(() =>
                    {
                        if (start == null)
                            vb.SetData(data, 0);
                        else
                            vb.SetData(data, start[0]);
                    });
                }

                public IRenderDevice Render
                {
                    get { return render; }
                }
            }

            #endregion

            VertexBuffer IResourceManagerOf<VertexBuffer>.Create(Type elementType, int[] sizes)
            {
                int length = sizes.Aggregate(1, (e, a) => e * a);

                VertexBufferResourceOnDeviceManager manager = null;

                ((OpenGLRender)render).SyncExecute(() =>
                {
                    manager = new VertexBufferResourceOnDeviceManager(this.render,
                        new OpenGLVertexBuffer(elementType, length, ((OpenGLRender)render).RenderStates as OpenGLRender.OpenGLRenderStates), elementType, sizes);
                });

                VertexBuffer vb = VertexBuffer.CreateInternalResource<VertexBuffer>(manager);
                bufferManagers.Add(vb, manager);
                return vb;
            }

            IResourceOnDeviceManager IResourceManagerOf<VertexBuffer>.GetManagerFor(VertexBuffer resource)
            {
                if (bufferManagers.ContainsKey(resource))
                    return bufferManagers[resource];

                throw new ArgumentOutOfRangeException("resource");
            }

            SupportMode IResourceManagerOf<VertexBuffer>.SupportMode
            {
                get { return SupportMode.Device; }
            }

            #endregion

            #region IResourceManagerOf<IndexBuffer>

            #region Index Buffer Manager

            internal class IndexBufferResourceOnDeviceManager : ISettableResourceOnDeviceManager
            {
                public IndexBufferResourceOnDeviceManager(IRenderDevice render, OpenGLIndexBuffer ib, Type type, int[] sizes)
                {
                    this.ib = ib;
                    this.sizes = sizes;
                    length = sizes.Aggregate(1, (e, a) => a * e);
                    this.internalType = type;
                    this.render = render;
                }

                OpenGLIndexBuffer ib;
                int[] sizes;
                int length;
                Type internalType;
                IRenderDevice render;

                public OpenGLIndexBuffer IndexBuffer
                {
                    get { return ib; }
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
                    ib.Dispose();
                }

                public Type ElementType
                {
                    get { return internalType; }
                }

                public Array GetData(int[] start, int[] size)
                {
                    Array data = null;
                    ((OpenGLRender)render).SyncExecute(() =>
                    {
                        if (start == null || size == null)
                            data = ib.GetData(0, this.length);
                        else
                            data = ib.GetData(start[0], size[0]);
                    });
                    return data;
                }

                public void SetData(Array data, int[] start, int[] ranks)
                {
                    ((OpenGLRender)render).SyncExecute(() =>
                    {
                        if (start == null)
                            ib.SetData(data, 0);
                        else
                            ib.SetData(data, start[0]);
                    });
                }

                public IRenderDevice Render
                {
                    get { return render; }
                }
            }

            #endregion

            IndexBuffer IResourceManagerOf<IndexBuffer>.Create(Type elementType, int[] sizes)
            {
                int length = sizes.Aggregate(1, (e, a) => e * a);

                if (elementType == typeof(int) || elementType == typeof(short) || elementType == typeof(uint) || elementType == typeof(ushort))
                {
                    IndexBufferResourceOnDeviceManager manager = null;

                    ((OpenGLRender)render).SyncExecute(() =>
                    {
                        manager = new IndexBufferResourceOnDeviceManager(this.render,
                            new OpenGLIndexBuffer(elementType, length), elementType, sizes);
                    });

                    IndexBuffer ib = IndexBuffer.CreateInternalResource<IndexBuffer>(manager);
                    bufferManagers.Add(ib, manager);
                    return ib;
                }
                else
                    throw new NotSupportedException();
            }

            IResourceOnDeviceManager IResourceManagerOf<IndexBuffer>.GetManagerFor(IndexBuffer resource)
            {
                if (bufferManagers.ContainsKey(resource))
                    return bufferManagers[resource];

                throw new ArgumentOutOfRangeException("resource");
            }

            SupportMode IResourceManagerOf<IndexBuffer>.SupportMode
            {
                get { return SupportMode.Device; }
            }

            #endregion

            #region IResourceManagerOf<TextureBuffer>

            #region Texture Buffer Manager

            internal class TextureBufferResourceOnDeviceManager : ISettableResourceOnDeviceManager
            {
                public TextureBufferResourceOnDeviceManager(IRenderDevice render, OpenGLTextureBuffer texture, Type type, int[] sizes)
                {
                    this.texture = texture;
                    this.sizes = sizes;
                    length = sizes.Aggregate(1, (e, a) => a * e);
                    this.internalType = type;
                    this.render = render;
                }

                OpenGLTextureBuffer texture;
                int[] sizes;
                int length;
                Type internalType;
                IRenderDevice render;

                public OpenGLTextureBuffer TextureBuffer
                {
                    get { return texture; }
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
                    texture.Dispose();
                }

                public Type ElementType
                {
                    get { return internalType; }
                }

                public Array GetData(int[] start, int[] size)
                {
                    if (start == null)
                        start = new int[this.Rank];
                    if (size == null)
                        size = this.GetRanks();

                    Array data = null;
                    ((OpenGLRender)render).SyncExecute(() =>
                    {
                        data = texture.GetData(start, size);
                    });
                    return data;
                }

                public void SetData(Array data, int[] start, int[] ranks)
                {
                    if (start == null || ranks == null)
                    {
                        start = new int[Rank];
                        ranks = this.GetRanks();
                    }

                    ((OpenGLRender)render).SyncExecute(() =>
                    {
                        texture.SetData(data, start, ranks);
                    });
                }

                public IRenderDevice Render
                {
                    get { return render; }
                }

                public void PrepareForRendering()
                {
                }
            }

            #endregion

            TextureBuffer IResourceManagerOf<TextureBuffer>.Create(Type elementType, int[] sizes)
            {
                OpenGLTextureBuffer texture = null;
                ((OpenGLRender)render).SyncExecute(() =>
                {
                    switch (sizes.Length)
                    {
                        case 1:
                            texture = new OpenGLTextureBuffer(elementType, sizes, TextureTarget.Texture1D);
                            break;
                        case 2:
                            texture = new OpenGLTextureBuffer(elementType, sizes, TextureTarget.Texture2D);
                            break;
                        case 3:
                            texture = new OpenGLTextureBuffer(elementType, sizes, TextureTarget.Texture3D);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                });

                TextureBufferResourceOnDeviceManager manager = new TextureBufferResourceOnDeviceManager(render, texture, elementType, sizes);
                TextureBuffer tb = TextureBuffer.CreateInternalResource<TextureBuffer>(manager);
                bufferManagers.Add(tb, manager);

                return tb;
            }

            IResourceOnDeviceManager IResourceManagerOf<TextureBuffer>.GetManagerFor(TextureBuffer resource)
            {
                if (bufferManagers.ContainsKey(resource))
                    return bufferManagers[resource];

                throw new KeyNotFoundException();
            }

            SupportMode IResourceManagerOf<TextureBuffer>.SupportMode
            {
                get { return SupportMode.Device; }
            }

            #endregion

            #region IResourceManagerOf<CubeTextureBuffer>

            CubeTextureBuffer IResourceManagerOf<CubeTextureBuffer>.Create(Type elementType, int[] sizes)
            {
                OpenGLTextureBuffer texture = null;
                ((OpenGLRender)render).SyncExecute(() =>
                {
                    texture = new OpenGLTextureBuffer(elementType, sizes, TextureTarget.TextureCubeMap);
                });

                TextureBufferResourceOnDeviceManager manager = new TextureBufferResourceOnDeviceManager(render, texture, elementType, sizes);
                CubeTextureBuffer tb = CubeTextureBuffer.CreateInternalResource<CubeTextureBuffer>(manager);
                bufferManagers.Add(tb, manager);

                return tb;
            }

            IResourceOnDeviceManager IResourceManagerOf<CubeTextureBuffer>.GetManagerFor(CubeTextureBuffer resource)
            {
                if (bufferManagers.ContainsKey(resource))
                    return bufferManagers[resource];

                throw new KeyNotFoundException();
            }

            SupportMode IResourceManagerOf<CubeTextureBuffer>.SupportMode
            {
                get { return SupportMode.Device; }
            }

            #endregion
        }
    }
}
