using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Maths;
using System.Rendering.Modeling;
using System.Compilers.Shaders;
using System.Rendering.Effects;
using System.Rendering.RenderStates;
using System.Unsafe;
using System.Runtime.InteropServices;

namespace System.Rendering
{
    /// <summary>
    /// Allows to have a set of pixels as a texture.
    /// </summary>
    public class TextureBuffer : GraphicResource
    {
        protected TextureBuffer(TextureBuffer.GraphicResourceStrategy strategy) :base (strategy)
        {
        }

        public static implicit operator TextureBuffer(Array array)
        {
            return Create<TextureBuffer>(array, null);
        }

        public static TextureBuffer Create<T>(T[] unidimensionalData)
        {
            return (TextureBuffer)unidimensionalData;
        }

        public static TextureBuffer Create<T>(T[,] bidimensionalData)
        {
            return (TextureBuffer)bidimensionalData;
        }

        public static TextureBuffer Create<T>(T[,,] tridimensionalData)
        {
            return (TextureBuffer)tridimensionalData;
        }

        public static TextureBuffer Empty<T>(int size)
        {
            return Create<T>(new T[size]);
        }

        public static TextureBuffer Empty<T>(int width, int height)
        {
            return Create<T>(new T[height, width]);
        }

        public static TextureBuffer Empty<T>(int depth, int width, int height)
        {
            return Create<T>(new T[depth, height, width]);
        }

        public int Width { get { return this.GetLength(Rank - 1); } }

        public int Height
        {
            get
            {
                if (Rank < 2) return 1;
                return this.GetLength(Rank - 2);
            }
        }

        public int Depth
        {
            get
            {
                if (Rank < 3) return 1;
                return this.GetLength(Rank - 3);
            }
        }

        public override Resourcing.IAllocateable Clone(IRenderDevice render)
        {
            if (render == null) // User memory clone.
                return this.Clone<TextureBuffer>();

            return render.ResourcesManager.Load<TextureBuffer>(this.GetData());
        }
    }

    public interface IFacetedTexture
    {
        int ActiveFace { get; set; }

        int NumberOfFaces { get; }
    }

    public enum CubeFaces
    {
        PositiveX = 0,
        NegativeX = 1,
        PositiveY = 2,
        NegativeY = 3,
        PositiveZ = 4,
        NegativeZ = 5,
    }

    public class CubeTextureBuffer : TextureBuffer, IFacetedTexture
    {
        protected CubeTextureBuffer(TextureBuffer.GraphicResourceStrategy strategy) : base(strategy) { }

        public int NumberOfFaces { get { return 6; } }

        public CubeFaces ActiveFace { get; set; }

        int IFacetedTexture.ActiveFace
        {
            get { return (int)ActiveFace; }
            set { ActiveFace = (CubeFaces)value; }
        }

        public static CubeTextureBuffer Create<T>(T[,] positiveX, T[,] negativeX, T[,] positiveY, T[,] negativeY, T[,] positiveZ, T[,] negativeZ)
        {
            if (positiveX == null || negativeX == null || positiveY == null || negativeY == null || positiveZ == null || negativeZ == null)
                throw new ArgumentNullException();

            int[] dimensions = new int[] { 
                positiveX.GetLength(0), positiveX.GetLength(1),
                negativeX.GetLength(0), negativeX.GetLength(1),
                positiveY.GetLength(0), positiveY.GetLength(1),
                negativeY.GetLength(0), negativeY.GetLength(1),
                positiveZ.GetLength(0), positiveZ.GetLength(1),
                negativeZ.GetLength(0), negativeZ.GetLength(1)
            };

            if (dimensions.Min() != dimensions.Max())
                throw new ArgumentException("Dimensions should be a square in each face and should be the same for all faces");

            T[, ,] buffer = new T[6, positiveX.GetLength(0), positiveX.GetLength(1)];

            int faceNumberOfElements = positiveX.Length;

            PointerManager.Copy(positiveX, 0, buffer, 0 * faceNumberOfElements, faceNumberOfElements);
            PointerManager.Copy(negativeX, 0, buffer, 1 * faceNumberOfElements, faceNumberOfElements);
            PointerManager.Copy(positiveY, 0, buffer, 2 * faceNumberOfElements, faceNumberOfElements);
            PointerManager.Copy(negativeY, 0, buffer, 3 * faceNumberOfElements, faceNumberOfElements);
            PointerManager.Copy(positiveZ, 0, buffer, 4 * faceNumberOfElements, faceNumberOfElements);
            PointerManager.Copy(negativeZ, 0, buffer, 5 * faceNumberOfElements, faceNumberOfElements);

            return (CubeTextureBuffer)buffer;
        }

        public static CubeTextureBuffer Empty<T>(int faceSize) where T:struct
        {
            return Create<T>(new T[faceSize, faceSize], new T[faceSize, faceSize], new T[faceSize, faceSize], new T[faceSize, faceSize], new T[faceSize, faceSize], new T[faceSize, faceSize]);
        }

        public void SetData(GraphicResourceUpdateMode mode, Array data, params int[] start) 
        {
            if (data == null)
                throw new ArgumentNullException();

            if (data.Rank != 2)
                throw new ArgumentException("Array should be a bidimensional array");

            if (data.GetLength(0) != data.GetLength(1) || data.GetLength(0) != this.Height || data.GetLength(1) != this.Width)
                throw new ArgumentException("Array should be a square of same dimensions than a face");

            GraphicResourceExtensors.SetData(this, mode, data, (int)ActiveFace, start[0], start[1]);
        }

        public static implicit operator CubeTextureBuffer(Array array)
        {
            return Create<CubeTextureBuffer>(array, null);
        }

        public override Resourcing.IAllocateable Clone(IRenderDevice render)
        {
            if (render == null) // User memory clone.
                return this.Clone<CubeTextureBuffer>();

            return render.ResourcesManager.Load<CubeTextureBuffer>(this.GetData());
        }
    }

    
}
