using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using D3D = SlimDX.Direct3D9;
using SlimDX.Direct3D9;
using System.Rendering.Resourcing;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Unsafe;
using System.IO;
using SlimDX;

namespace System.Rendering.Direct3D9
{
    partial class Direct3DRender
    {
        public class Direct3DResourcesManager : RenderBase.ResourcesManagerBase, 
            IResourceManagerOf<VertexBuffer>,
            IResourceManagerOf<IndexBuffer>,
            IResourceManagerOf<TextureBuffer>,
            IResourceManagerOf<CubeTextureBuffer>
        {

            public Direct3DResourcesManager(Direct3DRender render): base (render)
            {
            }

            protected Device device { get { return ((Direct3DRender)render).device; } }

            #region GetSet Buffers

            internal abstract class GSBufferResourceOnDeviceManager : ISettableResourceOnDeviceManager, ID3D9ResourceManager
            {
                int[] sizes;
                int length;
                Type internalType;
                IRenderDevice render;

                public abstract Resource Resource { get; }

                protected GSBufferResourceOnDeviceManager(IRenderDevice render, Type type, int[] sizes)
                {
                    this.sizes = sizes;
                    length = sizes.Aggregate(1, (e, a) => a * e);
                    this.internalType = type;
                    this.render = render;
                }

                /// <summary>
                /// Copies data from a DataStream object to the destination array starting in start index element of source stream.
                /// </summary>
                protected void CopyData(DataStream streamSource, Array destination, int countOfBytes)
                {
                    PointerManager.Copy(streamSource.DataPointer, Marshal.UnsafeAddrOfPinnedArrayElement(destination, 0), countOfBytes);
                }

                protected void CopyData(Array source, DataStream destination, int countOfBytes)
                {
                    PointerManager.Copy(Marshal.UnsafeAddrOfPinnedArrayElement(source, 0), destination.DataPointer, countOfBytes);
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
                    ((Direct3DRender.Direct3DResourcesManager)render.ResourcesManager).UnregisterByManager(this);
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

            #region VertexBuffer

            #region Vertex Buffer Manager

            internal class VertexBufferResourceOnDeviceManager : GSBufferResourceOnDeviceManager
            {
                public VertexBufferResourceOnDeviceManager(IRenderDevice render, D3D.VertexBuffer vb, Type type, int[] sizes)
                    : base (render, type, sizes)
                {
                    this.vb = vb;
                }

                D3D.VertexBuffer vb;

                public D3D.VertexBuffer VertexBuffer
                {
                    get { return vb; }
                }

                public override Array GetData(int[] start, int[] ranks)
                {
                    int typeSize = Marshal.SizeOf(ElementType);
                    if (start == null || ranks == null)
                    {
                        using (var data = vb.Lock(0, Length * typeSize, LockFlags.None))
                        {
                            Array userArray = Array.CreateInstance(ElementType, Length);
                            CopyData(data, userArray, (int)data.Length);
                            vb.Unlock();
                            return userArray;
                        }
                    }
                    else
                        if (ranks.Length == 1 && this.Rank == 1)
                        {
                            using (var data = vb.Lock(start[0] * typeSize, ranks[0] * typeSize, LockFlags.None))
                            {
                                Array userArray = Array.CreateInstance(ElementType, ranks[0]);
                                CopyData(data, userArray, (int)data.Length);
                                vb.Unlock();
                                return userArray;
                            }
                        }
                        else
                        {
                            using (var data = vb.Lock(0, Length * typeSize, LockFlags.None))
                            {
                                Array userArray = Array.CreateInstance(ElementType, this.GetRanks());
                                CopyData(data, userArray, this.Length);
                                vb.Unlock();
                                return ((VertexBuffer)userArray).GetData(start, ranks);
                            }
                        }
                }

                public override void SetData(Array arrayData, int[] start, int[] ranks)
                {
                    int typeSize = Marshal.SizeOf(arrayData.GetType().GetElementType());
                    if (arrayData.Length == this.Length)
                    {
                        using (var data = vb.Lock(0, Length * typeSize, LockFlags.None))
                        {
                            using (DataStream user = new DataStream(arrayData, true, true))
                            {
                                user.CopyTo(data);
                                data.Position = 0;
                                vb.Unlock();
                            }
                        }
                        return;
                    }

                    if (arrayData.GetRanks().Length == 1)
                    {
                        using (var data = vb.Lock(start[0] * typeSize, arrayData.Length * typeSize, LockFlags.None))
                        {
                            using (DataStream user = new DataStream(arrayData, true, true))
                            {
                                user.CopyTo(data);
                                vb.Unlock();
                                return;
                            }
                        }
                    }
                    else
                    {
                        Array userArray = Array.CreateInstance(ElementType, this.GetRanks());
                        using (var data = vb.Lock(0, Length * typeSize, LockFlags.None))
                        {
                            using (DataStream user = new DataStream(userArray, true, true))
                            {
                                data.CopyTo(user);

                                ((VertexBuffer)userArray).SetData(arrayData, start, arrayData.GetRanks());

                                user.CopyTo(data);
                                vb.Unlock();
                            }
                        }
                        return;
                    }
                }

                protected override void DisposeResource()
                {
                    vb.Dispose();
                }

                public override Resource Resource
                {
                    get { return VertexBuffer; }
                }
            }

            #endregion

            protected internal VertexBuffer Wrap(D3D.VertexBuffer vb, Type elementType, params int[] sizes)
            {
                var manager = new VertexBufferResourceOnDeviceManager(this.render, vb, elementType, sizes);
                return Register<VertexBuffer>(manager);
            }

            VertexBuffer IResourceManagerOf<VertexBuffer>.Create(Type elementType, int[] sizes)
            {
                int length = sizes.Aggregate (1, (e,a)=>e*a);

                int stride;
                DataDescription descriptor;
                VertexElement[] vertexElements = Direct3D9Tools.GetVertexDeclaration(elementType, out stride, out descriptor);

                return Wrap (new D3D.VertexBuffer(device, Marshal.SizeOf(elementType)*length, Usage.None, D3D.VertexFormat.None, Pool.SystemMemory), elementType, sizes);
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

            #region IndexBuffer

            #region Index Buffer Manager

            internal class IndexBufferResourceOnDeviceManager : GSBufferResourceOnDeviceManager
            {
                public IndexBufferResourceOnDeviceManager(IRenderDevice render, D3D.IndexBuffer ib, Type type, int[] sizes):
                    base (render, type, sizes)
                {
                    this.ib = ib;
                }

                D3D.IndexBuffer ib;

                public D3D.IndexBuffer IndexBuffer
                {
                    get { return ib; }
                }

                public override Array GetData(int[] start, int[] ranks)
                {
                    int typeSize = Marshal.SizeOf(ElementType);
                    if (start == null || ranks == null)
                    {
                        using (var data = ib.Lock(0, Length * typeSize, LockFlags.None))
                        {
                            Array userArray = Array.CreateInstance(ElementType, this.GetRanks());
                            using (DataStream user = new DataStream(userArray, true, true))
                            {
                                data.CopyTo(user);
                                ib.Unlock();
                                return userArray;
                            }
                        }
                    }

                    if (ranks.Length == 1)
                    {
                        using (var data = ib.Lock(start[0] * typeSize, ranks[0] * typeSize, LockFlags.None))
                        {
                            Array userArray = Array.CreateInstance(ElementType, ranks[0]);
                            using (DataStream user = new DataStream(userArray, true, true))
                            {
                                data.CopyTo(user);
                                ib.Unlock();
                                return userArray;
                            }
                        }
                    }
                    else
                    {
                        using (var data = ib.Lock(0, Length * typeSize, LockFlags.None))
                        {
                            Array userArray = Array.CreateInstance(ElementType, this.GetRanks());
                            using (DataStream user = new DataStream(userArray, true, true))
                            {
                                data.CopyTo(user);
                                ib.Unlock();
                                return ((VertexBuffer)userArray).GetData(start, ranks);
                            }
                        }
                    }
                }

                public override void SetData(Array arrayData, int[] start, int[] ranks)
                {
                    int typeSize = Marshal.SizeOf(ElementType);
                    if (arrayData.Length == this.Length)
                    {
                        using (var data = ib.Lock(0, Length * typeSize, LockFlags.None))
                        {
                            using (DataStream user = new DataStream(arrayData, true, true))
                            {
                                user.CopyTo(data);
                                data.Position = 0;
                                ib.Unlock();
                                return;
                            }
                        }
                    }

                    if (arrayData.GetRanks().Length == 1)
                    {
                        using (var data = ib.Lock(start[0] * typeSize, arrayData.Length * typeSize, LockFlags.None))
                        {
                            using (DataStream user = new DataStream(arrayData, true, true))
                            {
                                user.CopyTo(data);
                                ib.Unlock();
                                return;
                            }
                        }
                    }
                    else
                    {
                        Array userArray = Array.CreateInstance(ElementType, this.GetRanks());

                        using (var data = ib.Lock(0, Length * typeSize, LockFlags.None))
                        {
                            using (DataStream user = new DataStream(userArray, true, true))
                            {
                                data.CopyTo(user);

                                ((VertexBuffer)userArray).SetData(arrayData, start, arrayData.GetRanks());

                                user.CopyTo(data);
                                ib.Unlock();
                                return;
                            }
                        }
                    }
                }

                protected override void DisposeResource()
                {
                    ib.Dispose();
                }

                public override Resource Resource
                {
                    get { return IndexBuffer; }
                }
            }

            #endregion

            protected internal IndexBuffer Wrap(D3D.IndexBuffer ib, Type elementType, params int[] sizes)
            {
                var manager = new IndexBufferResourceOnDeviceManager(this.render, ib, elementType, sizes);
                return Register<IndexBuffer>(manager);
            }

            IndexBuffer IResourceManagerOf<IndexBuffer>.Create(Type elementType, int[] sizes)
            {
                int length = sizes.Aggregate(1, (e, a) => e * a);

                if (elementType == typeof(int) || elementType == typeof(short) || elementType == typeof(uint) || elementType == typeof(ushort))
                {
                    return Wrap(new D3D.IndexBuffer(device, Marshal.SizeOf(elementType) * length, Usage.None, Pool.SystemMemory, Marshal.SizeOf(elementType) == 2),
                        elementType, sizes);
                }
                else
                    throw new NotSupportedException();
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

            #region TextureBuffer

            #region Texture Buffer Manager

            internal class TextureBufferResourceOnDeviceManager : GSBufferResourceOnDeviceManager
            {
                public TextureBufferResourceOnDeviceManager(IRenderDevice render, D3D.BaseTexture texture, Type type, int[] sizes)
                    :base (render, type, sizes)
                {
                    this.texture = texture;
                }

                D3D.BaseTexture texture;

                public BaseTexture TextureBuffer
                {
                    get { return texture; }
                }

                protected override void DisposeResource()
                {
                    texture.Dispose();
                }

                public override Array GetData(int[] start, int[] ranks)
                {
                    var type = ElementType;

                    if (start == null && ranks != null || start != null && ranks == null)
                        throw new ArgumentNullException("Ranks or start are null but not null both.");

                    if (start == null) start = new int[Rank];
                    if (ranks == null) ranks = this.GetRanks();

                    Array result = Array.CreateInstance(type, ranks.Aggregate(1, (a, x) => a * x));

                    DataStream datastream = null;

                    switch (Rank)
                    {
                        case 1:
                        case 2:
                            Point location = new Point(start[1], start[0]);
                            Size size = new Size(ranks[1], ranks[0]);
                            Rectangle r = new Rectangle(location, size);
                            var dataRectangle = ((Texture)texture).LockRectangle(0, r, LockFlags.None);
                            datastream = dataRectangle.Data;
                            break;

                        case 3:
                            Box b = new Box()
                            {
                                Back = start[0] + ranks[0],
                                Top = start[1],
                                Left = start[2],
                                Front = start[0],
                                Bottom = start[1] + ranks[1],
                                Right = start[2] + ranks[2]
                            };
                            var dataBox = ((VolumeTexture)texture).LockBox(0, b, LockFlags.None);
                            datastream = dataBox.Data;
                            break;
                    }

                    PointerManager.Copy(datastream.DataPointer, Marshal.UnsafeAddrOfPinnedArrayElement(result, 0), (int)datastream.Length);

                    switch (Rank)
                    {
                        case 1:
                        case 2:
                            ((Texture)texture).UnlockRectangle(0);
                            break;
                        case 3:
                            ((VolumeTexture)texture).UnlockBox(0);
                            break;
                    }
                    datastream.Dispose();

                    return result;
                }

                public override void SetData(Array data, int[] start, int[] ranks)
                {
                    Type type = ElementType;

                    if (type == null)
                        type = ElementType;

                    if (start == null)
                    {
                        start = new int[Rank];
                        ranks = this.GetRanks();
                    }

                    DataStream datastream = null;

                    switch (Rank)
                    {
                        case 1:
                        case 2:
                            Point location = new Point(start[1], start[0]);
                            Size size = new Size(ranks[1], ranks[0]);
                            Rectangle r = new Rectangle(location, size);
                            var dataRectangle = ((Texture)texture).LockRectangle(0, r, LockFlags.None);
                            datastream = dataRectangle.Data;
                            break;

                        case 3:
                            Box b = new Box()
                            {
                                Back = start[0] + ranks[0],
                                Top = start[1],
                                Left = start[2],
                                Front = start[0],
                                Bottom = start[1] + ranks[1],
                                Right = start[2] + ranks[2]
                            };
                            var dataBox = ((VolumeTexture)texture).LockBox(0, b, LockFlags.None);
                            datastream = dataBox.Data;
                            break;
                    }

                    PointerManager.Copy(Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), datastream.DataPointer, (int)datastream.Length);

                    switch (Rank)
                    {
                        case 1:
                        case 2:
                            ((Texture)texture).UnlockRectangle(0);
                            break;
                        case 3:
                            ((VolumeTexture)texture).UnlockBox(0);
                            break;
                    }
                    datastream.Dispose();

                    texture.GenerateMipSublevels();
                }

                public Device Device { get { return ((Direct3DRender)this.Render).device; } }

                public override Resource Resource
                {
                    get { return TextureBuffer; }
                }
            }

            #endregion

            TextureBuffer IResourceManagerOf<TextureBuffer>.Create(Type elementType, int[] sizes)
            {
                BaseTexture texture;
                var format = Direct3D9Tools.GetPixelFormat(elementType);

                if (!device.Direct3D.CheckDeviceFormat(0, DeviceType.Hardware, Format.X8R8G8B8, Usage.None, ResourceType.Texture, format))
                {
                    elementType = typeof(CustomPixels.SRGBA);
                    format = Format.A32B32G32R32F;
                }
                switch (sizes.Length)
                {
                    case 1:
                        texture = new D3D.Texture(device, sizes[0], 1, 0, Usage.AutoGenerateMipMap, format, Pool.Managed);
                        break;
                    case 2:
                        texture = new D3D.Texture(device, sizes[1], sizes[0], 0, Usage.AutoGenerateMipMap, format, Pool.Managed);
                        break;
                    case 3:
                        texture = new D3D.VolumeTexture(device, sizes[2], sizes[1], sizes[0], 0, Usage.None, format, Pool.Managed);
                        break;
                    default:
                        throw new NotSupportedException();
                }

                TextureBufferResourceOnDeviceManager manager = new TextureBufferResourceOnDeviceManager(render, texture, elementType, sizes);
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

            #region CubeTextureBuffer

            internal class CubeTextureBufferResourceOnDeviceManager : GSBufferResourceOnDeviceManager
            {
                public CubeTextureBufferResourceOnDeviceManager(IRenderDevice render, D3D.CubeTexture texture, Type type, int[] sizes)
                    : base(render, type, sizes)
                {
                    this.texture = texture;
                }

                D3D.CubeTexture texture;

                public CubeTexture TextureBuffer
                {
                    get { return texture; }
                }

                protected override void DisposeResource()
                {
                    texture.Dispose();
                }

                public int ActiveFace { get; set; }

                public override Array GetData(int[] start, int[] ranks)
                {
                    if (start == null && ranks == null)
                    {
                        start = new int[3]; ranks = this.GetRanks();
                    }

                    if (start == null || ranks == null)
                        throw new ArgumentNullException();

                    if (start.Length != ranks.Length || start.Length != 3)
                        throw new ArgumentException("Cube textures should be locked as 3D textures.");

                    var fullArray = Array.CreateInstance(ElementType, ranks);

                    for (int i = start[0]; i < start[0] + ranks[0]; i++)
                    {
                        var faceData = ((CubeTexture)texture).LockRectangle((CubeMapFace)i, 0, new Rectangle(start[2], start[1], ranks[2], ranks[1]), LockFlags.None);
                        PointerManager.Copy(faceData.Data.DataPointer, Marshal.UnsafeAddrOfPinnedArrayElement(fullArray, 0), (int)faceData.Data.Length);
                        ((CubeTexture)texture).UnlockRectangle((CubeMapFace)i, 0);
                    }

                    return fullArray;
                }

                public override void SetData(Array data, int[] start, int[] ranks)
                {
                    if (start == null)
                    {
                        start = new int[3];
                        ranks = this.GetRanks();
                    }

                    if (start.Length != ranks.Length || start.Length != 3)
                        throw new ArgumentException("Cube textures should be locked as 3D textures.");

                    for (int i = start[0]; i < start[0] + ranks[0]; i++)
                    {
                        var faceData = ((CubeTexture)texture).LockRectangle((CubeMapFace)i, 0, new Rectangle(start[2], start[1], ranks[2], ranks[1]), LockFlags.None);
                        PointerManager.Copy(Marshal.UnsafeAddrOfPinnedArrayElement(data, i * ranks[1] * ranks[2]), faceData.Data.DataPointer, (int)faceData.Data.Length);
                        ((CubeTexture)texture).UnlockRectangle((CubeMapFace)i, 0);
                    }
                }

                public override Resource Resource
                {
                    get { return TextureBuffer; }
                }
            }

            CubeTextureBuffer IResourceManagerOf<CubeTextureBuffer>.Create(Type elementType, int[] sizes)
            {
                if (sizes.Length != 3 || sizes[0] != 6 || sizes[1] != sizes[2])
                    throw new ArgumentException();

                var format = Direct3D9Tools.GetPixelFormat(elementType);

                if (!device.Direct3D.CheckDeviceFormat(0, DeviceType.Hardware, Format.X8R8G8B8, Usage.AutoGenerateMipMap, ResourceType.CubeTexture, format))
                {
                    elementType = typeof(CustomPixels.SRGBA);
                    format = Format.A32B32G32R32F;
                }

                var texture = new D3D.CubeTexture(device, sizes[1], 0, Usage.AutoGenerateMipMap, format, Pool.Managed);

                var manager = new CubeTextureBufferResourceOnDeviceManager(render, texture, elementType, sizes);
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

        public interface ID3D9ResourceManager
        {
            Resource Resource { get; }
        }
    }
}
