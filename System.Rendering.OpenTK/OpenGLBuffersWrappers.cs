using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Unsafe;
using System.Rendering.Resourcing;
using OpenTK.Graphics.OpenGL;
using System.Rendering.OpenTK;

namespace System.Rendering.OpenGL
{
    abstract class OpenGLBufferBase
    {
        /// <summary>
        /// When implemented, gets the buffer target
        /// </summary>
        protected internal abstract BufferTarget BufferTarget { get; }

        public int BufferID { get { return ID; } }

        /// <summary>
        /// Gets the ID of this buffer object
        /// </summary>
        int ID;
        /// <summary>
        /// Gets the element type stored in this buffer.
        /// </summary>
        Type type;
        /// <summary>
        /// Gets the total length of this buffer.
        /// </summary>
        int length;

        protected Type ElementType { get { return type; } }

        protected OpenGLBufferBase(Type elementType, int length)
        {
            this.type = elementType;
            this.length = length;
            GL.GenBuffers(1, out ID);
            GL.BindBuffer(BufferTarget, ID);
            GL.BufferData(BufferTarget, (IntPtr)(length * Marshal.SizeOf(elementType)), IntPtr.Zero, BufferUsageHint.StaticDraw);
        }

        public abstract void SetupPointers();

        public abstract void Activate();

        public abstract void Deactivate();

        public void Dispose()
        {
            GL.DeleteBuffers(1, ref ID);
        }

        public Array GetData(int startLock, int count)
        {
            GL.BindBuffer(BufferTarget, BufferID);
            Array toGet = Array.CreateInstance(this.type, count);
            GL.GetBufferSubData(BufferTarget, (IntPtr)(startLock * Marshal.SizeOf(this.type)), (IntPtr)(count * Marshal.SizeOf(this.type)), Marshal.UnsafeAddrOfPinnedArrayElement(toGet, 0));

            return toGet;
        }

        public void SetData(Array data, int start)
        {
            GL.BindBuffer(BufferTarget, BufferID);
            GL.BufferSubData(BufferTarget, (IntPtr)(start * Marshal.SizeOf(this.type)), (IntPtr)(data.Length * Marshal.SizeOf(this.type)), Marshal.UnsafeAddrOfPinnedArrayElement(data, 0));
        }
    }

    class OpenGLIndexBuffer : OpenGLBufferBase
    {
        protected override internal BufferTarget BufferTarget
        {
            get { return BufferTarget.ElementArrayBuffer; }
        }

        public OpenGLIndexBuffer(Type type, int length) : base(type, length) { }

        public override void SetupPointers()
        {
            GL.BindBuffer(BufferTarget, BufferID);
            GL.IndexPointer((IndexPointerType)OpenGLTools.GetBaseType(ElementType), Marshal.SizeOf(ElementType), IntPtr.Zero);
        }

        public override void Activate()
        {
            GL.BindBuffer(BufferTarget, BufferID);

            GL.EnableClientState(ArrayCap.IndexArray);
        }

        public override void Deactivate()
        {
            GL.BindBuffer(BufferTarget, BufferID);

            GL.DisableClientState(ArrayCap.IndexArray);
        }
    }

    class OpenGLVertexBuffer : OpenGLBufferBase
    {
        protected override internal BufferTarget BufferTarget
        {
            get { return BufferTarget.ArrayBuffer; }
        }

        OpenGLRender.OpenGLRenderStates renderStateManager;

        public DataDescription Descriptor { get; private set; }

        public OpenGLVertexBuffer(Type type, int length, OpenGLRender.OpenGLRenderStates renderStateManager)
            : base(type, length)
        {
            this.renderStateManager = renderStateManager;
            this.Descriptor = DataComponentExtensors.GetDescriptor(type);
        }

        public override void SetupPointers()
        {
            GL.BindBuffer(BufferTarget, BufferID);
            int elementSize = Marshal.SizeOf(this.ElementType);

            foreach (var d in Descriptor.Declaration)
            {
                var sem = d.Semantic;

                if (sem is PositionAttribute)
                {
                    GL.VertexPointer(d.Components, (VertexPointerType)OpenGLTools.GetBaseType(d.BasicType), elementSize, (IntPtr)d.Offset);
                    continue;
                }
                if (sem is ProjectedAttribute)
                {
                    GL.VertexPointer(d.Components, (VertexPointerType)OpenGLTools.GetBaseType(d.BasicType), elementSize, (IntPtr)d.Offset);
                    continue;
                }

                if (sem is NormalAttribute)
                {
                    GL.NormalPointer((NormalPointerType)OpenGLTools.GetBaseType(d.BasicType), elementSize, (IntPtr)d.Offset);
                    continue;
                }

                if (sem is CoordinatesAttribute)
                {
                    int textureUnit = ((CoordinatesAttribute)d.Semantic).Index;
                    GL.ClientActiveTexture(TextureUnit.Texture0 + textureUnit);
                    GL.EnableClientState(ArrayCap.TextureCoordArray);
                    GL.TexCoordPointer(d.Components, (TexCoordPointerType)OpenGLTools.GetBaseType(d.BasicType), elementSize, (IntPtr)d.Offset);
                    continue;
                }
                if (sem is ColorAttribute && d.Components == 1)
                {
                    switch (((ColorAttribute)sem).Index)
                    {
                        case 0:
                            GL.ColorPointer((int)PixelFormat.Bgra, ColorPointerType.UnsignedByte, elementSize, (IntPtr)d.Offset);
                            break;
                        case 1:
                            GL.SecondaryColorPointer((int)PixelFormat.Bgra, ColorPointerType.UnsignedByte, elementSize, (IntPtr)d.Offset);
                            break;
                    }
                    continue;
                }

                if (sem is ColorAttribute)
                {
                    GL.ColorPointer(d.Components, (ColorPointerType)OpenGLTools.GetBaseType(d.BasicType), elementSize, (IntPtr)d.Offset);
                    continue;
                }

                var programID = ((OpenGLEffectManager)renderStateManager.effectManager).Effect.ProgramID;
                var attributeIndex = GL.GetAttribLocation(programID, "attribute_" + sem.ToString());

                if (attributeIndex >= 0)
                    GL.VertexAttribPointer(attributeIndex, d.Components, (VertexAttribPointerType)OpenGLTools.GetBaseType(d.BasicType), false, elementSize, (IntPtr)d.Offset);
            }
        }

        public override void Activate()
        {
            GL.BindBuffer(BufferTarget, BufferID);

            foreach (var d in DataComponentExtensors.GetDescriptor(ElementType).Declaration)
            {
                var sem = d.Semantic;
                if (sem is PositionAttribute)
                {
                    GL.EnableClientState(ArrayCap.VertexArray);
                    continue;
                }
                if (sem is ProjectedAttribute)
                {
                    GL.EnableClientState(ArrayCap.VertexArray);
                    continue;
                }
                if (sem is NormalAttribute)
                {
                    GL.EnableClientState(ArrayCap.NormalArray);
                    continue;
                }
                if (sem is CoordinatesAttribute)
                {
                    int textureUnit = ((CoordinatesAttribute)d.Semantic).Index;
                    GL.ClientActiveTexture(TextureUnit.Texture0 + textureUnit);
                    GL.EnableClientState(ArrayCap.TextureCoordArray);
                    continue;
                }
                if (sem is ColorAttribute)
                {
                    switch (((ColorAttribute)sem).Index)
                    {
                        case 0:
                            GL.EnableClientState(ArrayCap.ColorArray);
                            break;
                        case 1:
                            GL.EnableClientState(ArrayCap.SecondaryColorArray);
                            break;
                    }
                    continue;
                }

                var programID = ((OpenGLEffectManager)renderStateManager.effectManager).Effect.ProgramID;
                var attributeIndex = GL.GetAttribLocation(programID, "attribute_" + sem.ToString());

                if (attributeIndex >= 0)
                    GL.EnableVertexAttribArray(attributeIndex);
            }
        }

        public override void Deactivate()
        {
            GL.BindBuffer(BufferTarget, BufferID);
            foreach (var d in DataComponentExtensors.GetDescriptor(ElementType).Declaration)
            {
                var sem = d.Semantic;

                if (sem is PositionAttribute)
                {
                    GL.DisableClientState(ArrayCap.VertexArray);
                    continue;
                }
                if (sem is ProjectedAttribute)
                {
                    GL.DisableClientState(ArrayCap.VertexArray);
                    continue;
                }

                if (sem is NormalAttribute)
                {
                    GL.DisableClientState(ArrayCap.NormalArray);
                    continue;
                }

                if (sem is CoordinatesAttribute)
                {
                    int index = ((CoordinatesAttribute)d.Semantic).Index;
                    GL.ClientActiveTexture(TextureUnit.Texture0 + index);
                    GL.DisableClientState(ArrayCap.TextureCoordArray);
                    continue;
                }
                if (sem is ColorAttribute)
                {
                    switch (((ColorAttribute)sem).Index)
                    {
                        case 0:
                            GL.DisableClientState(ArrayCap.ColorArray);
                            break;
                        case 1:
                            GL.DisableClientState(ArrayCap.SecondaryColorArray);
                            break;
                    }
                    continue;
                }

                var programID = ((OpenGLEffectManager)renderStateManager.effectManager).Effect.ProgramID;
                var attributeIndex = GL.GetAttribLocation(programID, "attribute_" + sem.ToString());

                if (attributeIndex >= 0)
                    GL.DisableVertexAttribArray(attributeIndex);
            }
        }
    }

    //public class OpenGLTextureBuffer : OpenGLBufferBase
    //{
    //    protected internal override int BufferTarget
    //    {
    //        get { return GL.TEXTUREBUFFEREXT; }
    //    }

    //    public OpenGLTextureBuffer(Type pixelType, int[] ranks)
    //        : base(pixelType, ranks.Aggregate(1, (a, b) => a * b))
    //    {
    //    }
    //}

    class OpenGLTextureBuffer
    {
        TextureTarget TextureTarget;

        int __ID;
        Type type;
        int[] ranks;
        PixelInternalFormat internalFormat;

        DataType componentType;
        PixelFormat pixelFormat;

        public int ID { get { return __ID; } }

        public OpenGLTextureBuffer(Type pixelType, int[] ranks,  TextureTarget textureTarget)
        {
            this.TextureTarget = textureTarget;
            this.type = pixelType;
            this.ranks = ranks;
            GL.GenTextures(1, out __ID);
            GL.BindTexture(textureTarget, __ID);

            if (pixelType == typeof(int) || pixelType == typeof(uint))
            {
                internalFormat = PixelInternalFormat.Rgba32ui;
                pixelFormat = PixelFormat.Bgra;
                componentType = DataType.UnsignedByte;
            }
            else
                {
                    var pixelDescriptor = DataComponentExtensors.GetDescriptor(pixelType);
                    if (!OpenGLTools.GetPixelFormat(pixelDescriptor, out internalFormat, out pixelFormat, out componentType))
                        throw new NotSupportedException("Invalid type for texels");
                }

            switch (textureTarget)
            {
                case TextureTarget.Texture1D:
                    GL.TexImage1D(textureTarget, 0, internalFormat, ranks[0], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    break;
                case TextureTarget.Texture2D:
                    GL.TexImage2D(textureTarget, 0, internalFormat, ranks[1], ranks[0], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    break;
                case TextureTarget.Texture3D:
                    GL.TexImage3D(textureTarget, 0, internalFormat, ranks[2], ranks[1], ranks[0], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    break;
                case TextureTarget.TextureCubeMap:
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX, 0, (PixelInternalFormat)internalFormat, ranks[2], ranks[1], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    GL.TexImage2D(TextureTarget.TextureCubeMapNegativeX, 0, (PixelInternalFormat)internalFormat, ranks[2], ranks[1], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveY, 0, (PixelInternalFormat)internalFormat, ranks[2], ranks[1], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    GL.TexImage2D(TextureTarget.TextureCubeMapNegativeY, 0, (PixelInternalFormat)internalFormat, ranks[2], ranks[1], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveZ, 0, (PixelInternalFormat)internalFormat, ranks[2], ranks[1], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    GL.TexImage2D(TextureTarget.TextureCubeMapNegativeZ, 0, (PixelInternalFormat)internalFormat, ranks[2], ranks[1], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    break;
            }
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget, ID);
        }

        public void Dispose()
        {
            Bind();
            switch (TextureTarget)
            {
                case TextureTarget.Texture1D:
                    GL.TexImage1D(TextureTarget, 0, (PixelInternalFormat)internalFormat, ranks[0], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    break;
                case TextureTarget.Texture2D:
                    GL.TexImage2D(TextureTarget, 0, PixelInternalFormat.Rgba, ranks[1], ranks[0], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    break;
                case TextureTarget.Texture3D:
                    GL.TexImage3D(TextureTarget, 0, (PixelInternalFormat)internalFormat, ranks[2], ranks[1], ranks[0], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    break;
                case TextureTarget.TextureCubeMap:
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX, 0, (PixelInternalFormat)internalFormat, ranks[2], ranks[1], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    GL.TexImage2D(TextureTarget.TextureCubeMapNegativeX, 0, (PixelInternalFormat)internalFormat, ranks[2], ranks[1], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveY, 0, (PixelInternalFormat)internalFormat, ranks[2], ranks[1], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    GL.TexImage2D(TextureTarget.TextureCubeMapNegativeY, 0, (PixelInternalFormat)internalFormat, ranks[2], ranks[1], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveZ, 0, (PixelInternalFormat)internalFormat, ranks[2], ranks[1], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                    GL.TexImage2D(TextureTarget.TextureCubeMapNegativeZ, 0, (PixelInternalFormat)internalFormat, ranks[2], ranks[1], 0, pixelFormat, (PixelType)componentType, IntPtr.Zero);
                  break;

            }
            GL.DeleteTextures(1, ref __ID);
        }

        public Array GetData(int[] start, int[] ranks)
        {
            GL.BindTexture(TextureTarget, ID);

            switch (TextureTarget)
            {
                case TextureTarget.Texture1D:
                case TextureTarget.Texture2D:
                case TextureTarget.Texture3D:
                    {
                        Array fullData = Array.CreateInstance(this.type, this.ranks);

                        GL.GetTexImage(TextureTarget, 0, pixelFormat, (PixelType)componentType, Marshal.UnsafeAddrOfPinnedArrayElement(fullData, 0));

                        if (start == null && ranks == null)
                        {
                            return fullData;
                        }
                        if (start == null || ranks == null)
                            throw new ArgumentNullException();

                        TextureBuffer onMemory = fullData;
                        return onMemory.GetData(start, ranks);
                    }
                case TextureTarget.TextureCubeMap:
                    {
                        if (start == null && ranks == null)
                        {
                            start = new int[3];
                            ranks = this.ranks;
                        }

                        if (start == null || ranks == null)
                            throw new ArgumentNullException();

                        // Full face of only selected faces...
                        Array fullData = Array.CreateInstance(this.type, ranks[0], this.ranks[1], this.ranks[2]);
    
                        IntPtr pointer = Marshal.UnsafeAddrOfPinnedArrayElement (fullData,0);
                        int faceNumberOfBytes = this.ranks[1]*this.ranks[2]*Marshal.SizeOf(this.type);

                        for (int i = start[0]; i < start[0] + ranks[0]; i++)
                            GL.GetTexImage((TextureTarget)((int)TextureTarget.TextureCubeMapPositiveX + i), 0, pixelFormat, (PixelType)componentType, (IntPtr)((int)pointer + (i - start[0]) * faceNumberOfBytes));

                        TextureBuffer onMemory = fullData;
                        return onMemory.GetData(start, ranks);
                    }
                default:
                    throw new NotSupportedException("TargetType not supportted");
            }
        }

        public void SetData(Array data, int[] start, int[] ranks)
        {
            GL.BindTexture(TextureTarget, ID);

            Array toSet = data;

            if (start == null)
            {
                start = new int[this.ranks.Length];
                ranks = this.ranks;
            }

            IntPtr toSetPtr = Marshal.UnsafeAddrOfPinnedArrayElement(toSet, 0);

            switch (TextureTarget)
            {
                case TextureTarget.Texture1D:
                    GL.TexSubImage1D(TextureTarget.Texture1D, 0, start[0], data.GetLength(0), pixelFormat, (PixelType)componentType, toSetPtr);
                    break;
                case TextureTarget.Texture2D:
                    GL.TexSubImage2D(TextureTarget.Texture2D, 0, start[1], start[0], data.GetLength(1), data.GetLength(0), pixelFormat, (PixelType)componentType, toSetPtr);
                    break;
                case TextureTarget.Texture3D:
                    GL.TexSubImage3D(TextureTarget.Texture3D, 0, start[2], start[1], start[0], data.GetLength(2), data.GetLength(1), data.GetLength(0), pixelFormat, (PixelType)componentType, toSetPtr);
                    break;
                case TextureTarget.TextureCubeMap:
                    IntPtr pointer = Marshal.UnsafeAddrOfPinnedArrayElement(toSet, 0);
                    int faceNumberOfBytes = toSet.GetLength(1)*toSet.GetLength(2)*Marshal.SizeOf(type);
                    for (int i = start[0]; i < start[0] + data.GetLength(0); i++)
                    {
                        GL.TexSubImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, start[2], start[1], data.GetLength(2), data.GetLength(1), pixelFormat, (PixelType)componentType, (IntPtr)((int)pointer + (i - start[0]) * faceNumberOfBytes));
                    }
                    break;
                default :
                    throw new NotSupportedException("TargetType not supported");
            }
        }
    }
}
