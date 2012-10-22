using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Maths;
using System.Compilers.Shaders;
using System.Rendering.Modeling;
using System.Rendering.Effects;
using System.Rendering.RenderStates;

namespace System.Rendering
{
    public static class TextureBufferExtensors
    {

        public static void Fill1D(this TextureBuffer texture, Func<Vector1, Vector4> fill)
        {
            if (texture.Rank != 1)
                throw new InvalidOperationException("Only 1D textured can be filled with a 1D-4D function");

            var diffuse = new int[texture.Width];
            int width = texture.Width;
            float projectionWidth = Math.Max(0, texture.Width - 1);

            for (int px = 0; px < width; px++)
                diffuse[px] = fill(new Vector1(px / projectionWidth)).ToARGB();

            texture.Update(diffuse);
        }

        public static void Fill2D(this TextureBuffer texture, Func<Vector2, Vector4> fill)
        {
            if (texture.Rank != 2 && !(texture is CubeTextureBuffer))
                throw new InvalidOperationException("Only 2D textured can be filled with a 2D-4D function");

            if (texture.Render != null && texture.Render is ITextureRenderDevice && texture.Render.RenderStateInfo.IsSupported<VertexShaderState>() && texture.Render.RenderStateInfo.IsSupported<PixelShaderState>())
            {
                var render = texture.Render as ITextureRenderDevice;

                render.BeginScene(texture);

                render.Draw(Basic.TriangleFan(quadVertexes).AsModel(),
                    //new VertexShaderEffect<Shader.VSIn, Shader.VSOut>(Shader.DefaultVertexProcess), 
                    Projecting.From(new ProjectionState(Matrices.I)),
                    new PixelShaderEffect<Shader.PSIn, Shader.PSOut>(new Shader { Map = fill }.Main)
                    //Buffering.Clear(new Vector4<float>(0, 1, 0, 1)), Buffering.ClearDepth(), Projecting.OrthoRH(1, 1, 0.1f, 100), Viewing.LookAtRH(new Vector3f(0, 0, 1), new Vector3f(0, 0, 0), new Vector3f(0, 1, 0))
                    );

                render.EndScene();
            }
            else  // CPU processing
            {
                var diffuse = new CustomPixels.ARGB[texture.Height, texture.Width];
                int width = texture.Width;
                int height = texture.Height;
                float projectionWidth = Math.Max(0, texture.Width - 1);
                float projectionHeight = Math.Max(0, texture.Height - 1);

                for (int py = 0; py < height; py++)
                    for (int px = 0; px < width; px++)
                    {
                        var v = fill(new Vector2<float>(px / projectionWidth, py / projectionHeight));
                        diffuse[py, px] = new CustomPixels.ARGB(v.X, v.Y, v.Z, v.W);
                    }

                texture.Update(diffuse);
            }
        }

        public static void Fill3D(this TextureBuffer texture, Func<Vector3, Vector4> fill)
        {
            if (texture.Rank != 3)
                throw new InvalidOperationException("Only 3D textured can be filled with a 3D-4D function");

            var diffuse = new int[texture.Depth, texture.Height, texture.Width];
            int depth = texture.Depth;
            int width = texture.Width;
            int height = texture.Height;
            float projectionWidth = Math.Max(0, texture.Width - 1);
            float projectionHeight = Math.Max(0, texture.Height - 1);
            float projectionDepth = Math.Max(0, texture.Depth - 1);

            for (int pz = 0; pz < depth; pz++)
                for (int py = 0; py < height; py++)
                    for (int px = 0; px < width; px++)
                        diffuse[pz, py, px] = fill(new Vector3<float>(px / projectionWidth, py / projectionHeight, pz / projectionDepth)).ToARGB();

            texture.Update(diffuse);
        }

        public static void Draw(this TextureBuffer texture, Action<ITextureRenderDevice> rendering)
        {
            if (texture.Render == null)
                throw new InvalidOperationException("Only allocated textures can be rendered.");

            if (!(texture.Render is ITextureRenderDevice))
                throw new InvalidOperationException("Render should be a ITextureRenderDevice to render a texture");

            var render = texture.Render as ITextureRenderDevice;

            render.BeginScene(texture);

            rendering(render);

            render.EndScene();
        }

        public static void Draw(this CubeTextureBuffer texture, CubeFaces face, Action<ITextureRenderDevice> rendering)
        {
            texture.ActiveFace = face;
            texture.Draw(rendering);
        }

        private static void BuildNoise(double[] data, int start, int end, Random rnd, double factor)
        {
            int middle = (start + end) / 2;

            data[middle] = (data[start] + data[end]) / 2 + (1 - 2 * rnd.NextDouble()) * (end - start) / data.Length;

            if (start < end - 1)
            {
                BuildNoise(data, start, middle, rnd, factor);
                BuildNoise(data, middle, end, rnd, factor);
            }
        }

        private static void BuildNoise(double[,] data, int startRow, int startCol, int endRow, int endCol, Random rnd, double factor)
        {
            int centerRow = (endRow + startRow) / 2;
            int centerCol = (endCol + startCol) / 2;

            double cLU = data[startRow, startCol];
            double cLD = data[endRow, startCol];
            double cRU = data[startRow, endCol];
            double cRD = data[endRow, endCol];

            if (data[startRow, centerCol] == 0)
                data[startRow, centerCol] = (cLU + cRU) / 2 + factor * (1 - 2 * rnd.NextDouble()) * (endCol - startCol) / data.GetLength(1);
            if (data[endRow, centerCol] == 0)
                data[endRow, centerCol] = (cLD + cRD) / 2 + factor * (1 - 2 * rnd.NextDouble()) * (endCol - startCol) / data.GetLength(1);
            if (data[centerRow, startCol] == 0)
                data[centerRow, startCol] = (cLU + cLD) / 2 + factor * (1 - 2 * rnd.NextDouble()) * (endRow - startRow) / data.GetLength(0);
            if (data[centerRow, endCol] == 0)
                data[centerRow, endCol] = (cRU + cRD) / 2 + factor * (1 - 2 * rnd.NextDouble()) * (endRow - startRow) / data.GetLength(0);

            data[centerRow, centerCol] = (data[startRow, centerCol] + data[endRow, centerCol] + data[centerRow, startCol] + data[centerRow, endCol]) / 4
                + factor * (1 - 2 * rnd.NextDouble()) * (endCol - startCol) * (endRow - startRow) / (data.GetLength(1) * data.GetLength(0));

            if (startCol < endCol - 1 && startRow < endRow - 1)
            {
                BuildNoise(data, startRow, startCol, centerRow, centerCol, rnd, factor);
                BuildNoise(data, startRow, centerCol, centerRow, endCol, rnd, factor);
                BuildNoise(data, centerRow, startCol, endRow, centerCol, rnd, factor);
                BuildNoise(data, centerRow, centerCol, endRow, endCol, rnd, factor);
            }
        }

        private static double Interpolate(double alpha, Random rnd, double factor, params double[] values)
        {
            return values.Sum() / values.Length + (1 - 2 * rnd.NextDouble()) * alpha * factor;
        }

        private static void BuildNoise(double[, ,] data, int startDepth, int startRow, int startCol, int endDepth, int endRow, int endCol, Random rnd, double factor)
        {
            int centerDepth = (endDepth + startDepth) / 2;
            int centerRow = (endRow + startRow) / 2;
            int centerCol = (endCol + startCol) / 2;

            double cFLU = data[startDepth, startRow, startCol];
            double cFLD = data[startDepth, endRow, startCol];
            double cFRU = data[startDepth, startRow, endCol];
            double cFRD = data[startDepth, endRow, endCol];

            double cBLU = data[endDepth, startRow, startCol];
            double cBLD = data[endDepth, endRow, startCol];
            double cBRU = data[endDepth, startRow, endCol];
            double cBRD = data[endDepth, endRow, endCol];

            double depth = data.GetLength(0);
            double height = data.GetLength(1);
            double width = data.GetLength(2);

            // edges interpolation

            // front-top
            if (data[startDepth, startRow, centerCol] == 0)
                data[startDepth, startRow, centerCol] = Interpolate((endCol - startCol) / width, rnd, factor, cFLU, cFRU);

            // front-left
            if (data[startDepth, centerRow, startCol] == 0)
                data[startDepth, centerRow, startCol] = Interpolate((endRow - startRow) / height, rnd, factor, cFLU, cFLD);

            // front-right
            if (data[startDepth, centerRow, endCol] == 0)
                data[startDepth, centerRow, endCol] = Interpolate((endRow - startRow) / height, rnd, factor, cFRU, cFRD);

            // front-down
            if (data[startDepth, endRow, centerCol] == 0)
                data[startDepth, endRow, centerCol] = Interpolate((endCol - startCol) / width, rnd, factor, cFLD, cFRD);

            // left-top
            if (data[centerDepth, startRow, startCol] == 0)
                data[centerDepth, startRow, startCol] = Interpolate((endDepth - startDepth) / depth, rnd, factor, cFLU, cBLU);

            // left-down
            if (data[centerDepth, endRow, startCol] == 0)
                data[centerDepth, endRow, startCol] = Interpolate((endDepth - startDepth) / depth, rnd, factor, cFLD, cBLD);

            // right-top
            if (data[centerDepth, startRow, endCol] == 0)
                data[centerDepth, startRow, endCol] = Interpolate((endDepth - startDepth) / depth, rnd, factor, cFRU, cBRU);

            // right-down
            if (data[centerDepth, endRow, endCol] == 0)
                data[centerDepth, endRow, endCol] = Interpolate((endDepth - startDepth) / depth, rnd, factor, cFRD, cBRD);

            // back-top
            if (data[endDepth, startRow, centerCol] == 0)
                data[endDepth, startRow, centerCol] = Interpolate((endCol - startCol) / width, rnd, factor, cBLU, cBRU);

            // back-left
            if (data[endDepth, centerRow, startCol] == 0)
                data[endDepth, centerRow, startCol] = Interpolate((endRow - startRow) / height, rnd, factor, cBLU, cBLD);

            // back-right
            if (data[endDepth, centerRow, endCol] == 0)
                data[endDepth, centerRow, endCol] = Interpolate((endRow - startRow) / height, rnd, factor, cBRU, cBRD);

            // back-down
            if (data[endDepth, endRow, centerCol] == 0)
                data[endDepth, endRow, centerCol] = Interpolate((endCol - startCol) / width, rnd, factor, cBLD, cBRD);

            // faces interpolation

            // front
            if (data[startDepth, centerRow, centerCol] == 0)
                data[startDepth, centerRow, centerCol] = Interpolate((endRow - startRow) * (endCol - startCol) / (width * height), rnd, factor,
                    data[startDepth, centerRow, startCol], data[startDepth, startRow, centerCol], data[startDepth, centerRow, endCol], data[startDepth, endRow, centerCol]);

            // left
            if (data[centerDepth, centerRow, startCol] == 0)
                data[centerDepth, centerRow, startCol] = Interpolate((endRow - startRow) * (endDepth - startDepth) / (depth * height), rnd, factor,
                    data[startDepth, centerRow, startCol], data[centerDepth, startRow, startCol], data[endDepth, centerRow, startCol], data[centerDepth, endRow, startCol]);

            // right
            if (data[centerDepth, centerRow, endCol] == 0)
                data[centerDepth, centerRow, endCol] = Interpolate((endRow - startRow) * (endDepth - startDepth) / (depth * height), rnd, factor,
                    data[startDepth, centerRow, endCol], data[centerDepth, startRow, endCol], data[endDepth, centerRow, endCol], data[centerDepth, endRow, endCol]);

            // back
            if (data[endDepth, centerRow, centerCol] == 0)
                data[endDepth, centerRow, centerCol] = Interpolate((endRow - startRow) * (endCol - startCol) / (width * height), rnd, factor,
                    data[endDepth, centerRow, startCol], data[endDepth, startRow, centerCol], data[endDepth, centerRow, endCol], data[endDepth, endRow, centerCol]);

            // top
            if (data[centerDepth, startRow, centerCol] == 0)
                data[centerDepth, startRow, centerCol] = Interpolate((endDepth - startDepth) * (endCol - startCol) / (width * depth), rnd, factor,
                    data[startDepth, startRow, centerCol], data[centerDepth, startRow, startCol], data[endDepth, startRow, centerCol], data[centerDepth, startRow, endCol]);

            // bottom
            if (data[centerDepth, endRow, centerCol] == 0)
                data[centerDepth, endRow, centerCol] = Interpolate((endDepth - startDepth) * (endCol - startCol) / (width * depth), rnd, factor,
                    data[startDepth, endRow, centerCol], data[centerDepth, endRow, startCol], data[endDepth, endRow, centerCol], data[centerDepth, endRow, endCol]);

            // center interpolation
            if (data[centerDepth, centerRow, centerCol] == 0)
                data[centerDepth, centerRow, centerCol] = Interpolate((endDepth - startDepth) * (endRow - startRow) * (endCol - startCol) / (width * height * depth), rnd, factor,
                    data[startDepth, centerRow, centerCol], data[endDepth, centerRow, centerCol], // front-back
                    data[centerDepth, startRow, centerCol], data[centerDepth, endRow, centerCol], // top-down
                    data[centerDepth, centerRow, startCol], data[centerDepth, centerRow, endCol] // left-right
                    );

            if (startDepth < endDepth - 1 && startRow < endRow - 1 && startCol < endCol - 1)
            {
                BuildNoise(data, startDepth, startRow, startCol, centerDepth, centerRow, centerCol, rnd, factor);
                BuildNoise(data, startDepth, startRow, centerCol, centerDepth, centerRow, endCol, rnd, factor);
                BuildNoise(data, startDepth, centerRow, startCol, centerDepth, endRow, centerCol, rnd, factor);
                BuildNoise(data, startDepth, centerRow, centerCol, centerDepth, endRow, endCol, rnd, factor);

                BuildNoise(data, centerDepth, startRow, startCol, endDepth, centerRow, centerCol, rnd, factor);
                BuildNoise(data, centerDepth, startRow, centerCol, endDepth, centerRow, endCol, rnd, factor);
                BuildNoise(data, centerDepth, centerRow, startCol, endDepth, endRow, centerCol, rnd, factor);
                BuildNoise(data, centerDepth, centerRow, centerCol, endDepth, endRow, endCol, rnd, factor);
            }
        }

        private static CustomPixels.ARGB ColorFrom(double intensity)
        {
            intensity = Math.Max(0, Math.Min(1, intensity));
            return new CustomPixels.ARGB((byte)(255 * intensity), (byte)(255 * intensity), (byte)(255 * intensity), 255);
        }

        public static void GenerateNoise(this TextureBuffer texture)
        {
            texture.GenerateNoise(new Random().Next());
        }

        public static void GenerateNoise(this TextureBuffer texture, int seed)
        {
            texture.GenerateNoise(0.7f, seed);
        }

        public static void GenerateNoise(this TextureBuffer texture, double factor, int seed)
        {
            Random rnd = new Random(seed);
            switch (texture.Rank)
            {
                case 1:
                    {
                        double[] data = new double[texture.Width];
                        data[0] = rnd.NextDouble();
                        data[data.Length - 1] = rnd.NextDouble();
                        BuildNoise(data, 0, data.Length - 1, rnd, factor);
                        CustomPixels.ARGB[] realData = new CustomPixels.ARGB[data.Length];
                        for (int i = 0; i < realData.Length; i++)
                            realData[i] = ColorFrom(data[i]);

                        texture.Update(realData);
                        break;
                    }
                case 2:
                    {
                        double[,] data = new double[texture.Height, texture.Width];

                        data[0, 0] = rnd.NextDouble();
                        data[0, data.GetLength(1) - 1] = rnd.NextDouble();
                        data[data.GetLength(0) - 1, 0] = rnd.NextDouble();
                        data[data.GetLength(0) - 1, data.GetLength(1) - 1] = rnd.NextDouble();

                        BuildNoise(data, 0, 0, data.GetLength(0) - 1, data.GetLength(1) - 1, rnd, factor);
                        CustomPixels.ARGB[,] realData = new CustomPixels.ARGB[data.GetLength(0), data.GetLength(1)];
                        for (int i = 0; i < data.GetLength(0); i++)
                            for (int j = 0; j < data.GetLength(1); j++)
                                realData[i, j] = ColorFrom(data[i, j]);
                        texture.Update(realData);
                        break;
                    }
                case 3:
                    {
                        double[, ,] data = new double[texture.Depth, texture.Height, texture.Width];

                        data[0, 0, 0] = rnd.NextDouble();
                        data[0, 0, texture.Width - 1] = rnd.NextDouble();
                        data[0, texture.Height - 1, 0] = rnd.NextDouble();
                        data[0, texture.Height - 1, texture.Width - 1] = rnd.NextDouble();
                        data[texture.Depth - 1, 0, 0] = rnd.NextDouble();
                        data[texture.Depth - 1, 0, texture.Width - 1] = rnd.NextDouble();
                        data[texture.Depth - 1, texture.Height - 1, 0] = rnd.NextDouble();
                        data[texture.Depth - 1, texture.Height - 1, texture.Width - 1] = rnd.NextDouble();

                        BuildNoise(data, 0, 0, 0, texture.Depth - 1, texture.Height - 1, texture.Width - 1, rnd, factor);

                        CustomPixels.ARGB[, ,] realData = new CustomPixels.ARGB[data.GetLength(0), data.GetLength(1), data.GetLength(2)];

                        for (int i = 0; i < data.GetLength(0); i++)
                            for (int j = 0; j < data.GetLength(1); j++)
                                for (int k = 0; k < data.GetLength(2); k++)
                                    realData[i, j, k] = ColorFrom(data[i, j, k]);

                        texture.Update(realData);
                        break;
                    }
            }
        }

        class Shader
        {
            [ShaderType]
            public struct PSIn
            {
                [Coordinates]
                public Vector2<float> TextureCoordinates;
            }
            [ShaderType]
            public struct PSOut
            {
                [Color]
                public Vector4<float> Diffuse;
            }

            [ShaderType]
            public struct VSIn
            {
                [Position]
                public Vector4<float> Position;
            }

            [ShaderType]
            public struct VSOut
            {
                [Projected]
                public Vector4<float> Projected;

                [Coordinates(0)]
                public Vector2<float> TextureCoordinates;
            }

            public static VSOut DefaultVertexProcess(VSIn In)
            {
                VSOut Out = new VSOut();

                Out.Projected = In.Position;
                float x = In.Position.X + 1f;
                float y = In.Position.Y + 1f;
                Out.TextureCoordinates = new Vector2<float>(x / 2f, y / 2f);

                return Out;
            }

            public Func<Vector2, Vector4> Map;

            public PSOut Main(PSIn In)
            {
                PSOut Out = new PSOut();
                Out.Diffuse = Map(In.TextureCoordinates);
                return Out;
            }
        }

        static PTV[] quadVertexes = new PTV[] {
            new PTV { Projected = new Vector4 (-1f,1,0,1), TextureCoordinates = new Vector2 (0,0) },
            new PTV { Projected = new Vector4 (1f,1,0,1), TextureCoordinates = new Vector2 (1,0) },
            new PTV { Projected = new Vector4 (1f,-1f,0,1), TextureCoordinates = new Vector2 (1,1) },
            new PTV { Projected = new Vector4 (-1f,-1f,0,1), TextureCoordinates = new Vector2 (0,1) }
        };

        struct PTV
        {
            [Projected]
            public Vector4 Projected;
            [Coordinates]
            public Vector2 TextureCoordinates;
        }
    }
}
