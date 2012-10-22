using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Effects;
using System.Maths;
using System.Compilers.Shaders;
using System.Rendering.Effects.Shaders;

namespace System.Rendering
{
    public static class Shading
    {
        public static Effect DiffuseMap(TextureBuffer texture)
        {
            if (texture is CubeTextureBuffer)
            {
                SamplerCube sampler = new SamplerCube(texture as CubeTextureBuffer) { Filters = new Filtering { Magnification = TextureFilterType.Linear, Minification = TextureFilterType.Linear, MipMapping = TextureFilterType.Linear } };
                return new PixelShaderEffect<ColorCoordinates3DData, ColorData>(In => new ColorData { Color = In.Color * sampler.Sample(In.Coordinates) }) { AppendMode = AppendMode.Prepend };
            }
            else
                switch (texture.Rank)
                {
                    case 1:
                        {
                            Sampler1D sampler = new Sampler1D(texture) { Filters = new Filtering { Magnification = TextureFilterType.Linear, Minification = TextureFilterType.Linear, MipMapping = TextureFilterType.Linear } };
                            return new PixelShaderEffect<ColorCoordinatesData, ColorData>(In => new ColorData { Color = In.Color * sampler.Sample((Vector1)In.Coordinates) }) { AppendMode = AppendMode.Prepend };
                        }
                    case 2:
                        {
                            Sampler2D sampler = new Sampler2D(texture) { Filters = new Filtering { Magnification = TextureFilterType.Linear, Minification = TextureFilterType.Linear, MipMapping = TextureFilterType.Linear } };
                            return new PixelShaderEffect<ColorCoordinatesData, ColorData>(In => new ColorData { Color = In.Color * sampler.Sample(In.Coordinates) }) { AppendMode = AppendMode.Prepend };
                        }
                    case 3:
                        {
                            Sampler3D sampler = new Sampler3D(texture) { Filters = new Filtering { Magnification = TextureFilterType.Linear, Minification = TextureFilterType.Linear, MipMapping = TextureFilterType.None } };
                            return new PixelShaderEffect<ColorCoordinates3DData, ColorData>(In => new ColorData { Color = In.Color * sampler.Sample(In.Coordinates) }) { AppendMode = AppendMode.Prepend };
                        }
                }

            throw new ArgumentException();
        }
        public static Effect DiffuseMap(Func<Vector2, Vector4> map)
        {
            return new PixelShaderEffect<CoordinatesData, ColorData>(In => new ColorData { Color = map(In.Coordinates) }) { AppendMode = AppendMode.Prepend };
        }
        public static Effect DiffuseMap3D(Func<Vector3, Vector4> map)
        {
            return new PixelShaderEffect<Coordinates3DData, ColorData>(In => new ColorData { Color = map(In.Coordinates) }) { AppendMode = AppendMode.Prepend };
        }
        public static Effect SpecularMap(TextureBuffer texture)
        {
            if (texture is CubeTextureBuffer)
            {
                SamplerCube sampler = new SamplerCube(texture as CubeTextureBuffer) { Filters = new Filtering { Magnification = TextureFilterType.Linear, Minification = TextureFilterType.Linear, MipMapping = TextureFilterType.Linear } };
                return new PixelShaderEffect<SecondaryColorCoordinates3DData, SecondaryColorData>(In => new SecondaryColorData { Color = In.Color * sampler.Sample(In.Coordinates) }) { AppendMode = AppendMode.Prepend };
            }
            else
                switch (texture.Rank)
                {
                    case 1:
                        {
                            Sampler1D sampler = new Sampler1D(texture) { Filters = new Filtering { Magnification = TextureFilterType.Linear, Minification = TextureFilterType.Linear, MipMapping = TextureFilterType.Linear } };
                            return new PixelShaderEffect<SecondaryColorCoordinatesData, SecondaryColorData>(In => new SecondaryColorData { Color = In.Color * sampler.Sample((Vector1)In.Coordinates) }) { AppendMode = AppendMode.Prepend };
                        }
                    case 2:
                        {
                            Sampler2D sampler = new Sampler2D(texture) { Filters = new Filtering { Magnification = TextureFilterType.Linear, Minification = TextureFilterType.Linear, MipMapping = TextureFilterType.Linear } };
                            return new PixelShaderEffect<SecondaryColorCoordinatesData, SecondaryColorData>(In => new SecondaryColorData { Color = In.Color * sampler.Sample(In.Coordinates) }) { AppendMode = AppendMode.Prepend };
                        }
                    case 3:
                        {
                            Sampler3D sampler = new Sampler3D(texture) { Filters = new Filtering { Magnification = TextureFilterType.Linear, Minification = TextureFilterType.Linear, MipMapping = TextureFilterType.Linear } };
                            return new PixelShaderEffect<SecondaryColorCoordinates3DData, SecondaryColorData>(In => new SecondaryColorData { Color = In.Color * sampler.Sample(In.Coordinates) }) { AppendMode = AppendMode.Prepend };
                        }
                }

            throw new ArgumentException();
        }
        public static Effect SpecularMap(Func<Vector2, Vector4> map)
        {
            return new PixelShaderEffect<CoordinatesData, SecondaryColorData>(In => new SecondaryColorData { Color = map(In.Coordinates) }) { AppendMode = AppendMode.Prepend };
        }
        public static Effect SpecularMap3D(Func<Vector3, Vector4> map)
        {
            return new PixelShaderEffect<Coordinates3DData, SecondaryColorData>(In => new SecondaryColorData { Color = map(In.Coordinates) }) { AppendMode = AppendMode.Prepend };
        }

        public static Effect UseOrthographicCoordinates
        {
            get { return new VertexShaderEffect<PositionData, Coordinates3DData>(In => new Coordinates3DData { Coordinates = In.Position }) { AppendMode = AppendMode.Append }; }
        }

        public static Effect UseNormalCoordinates
        {
            get { return new VertexShaderEffect<NormalData, Coordinates3DData>(In => new Coordinates3DData { Coordinates = In.Normal }) { AppendMode = AppendMode.Append }; }
        }

        public static Effect UseReflectionCoordinates
        {
            get
            {
                return new VertexShaderEffect<PositionNormalData, Coordinates3DData>(In =>
                {
                    Vector3 V = GMath.normalize(-1 * In.Position);
                    Vector3 R = 2 * GMath.dot(In.Normal, V) * In.Normal - V;

                    return new Coordinates3DData { Coordinates = R };
                }) { AppendMode = AppendMode.Append };
            }
        }

        public static Effect Use2DNormalCoordinates
        {
            get
            {
                return new VertexShaderEffect<NormalData, CoordinatesData>(In => new CoordinatesData { Coordinates = new Vector2(In.Normal.X / 2 + 0.5f, 0.5f - In.Normal.Y / 2) }) { AppendMode = AppendMode.Append };
            }
        }

        public static Effect Use2DReflectionCoordinates
        {
            get
            {
                return new VertexShaderEffect<PositionNormalData, CoordinatesData>(In =>
                {
                    Vector3 V = GMath.normalize(-1 * In.Position);
                    Vector3 R = 2 * GMath.dot(In.Normal, V) * In.Normal - V;

                    var m = GMath.sqrt(R.X * R.X + R.Y * R.Y + (1 + R.Z) * (1 + R.Z));

                    return new CoordinatesData { Coordinates = new Vector2(R.X / m + 0.5f, R.Y / m + 0.5f) };
                }) { AppendMode = AppendMode.Append };
            }
        }

        public static Effect None
        {
            get { return Set<NoPipeline>(); }
        }

        public static Effect VertexTransform { get { return Append<VertexTransformPipeline>(); } }

        class VertexTransformPipeline : Pipeline, IVertexProcessor
        {
            VertexTransformPipeline(IBasicContext context) : base(context) { }

            ProjectedPositionNormalData Transform(PositionNormalData In)
            {
                var worldView = GMath.mul(Context.World, Context.View);

                var P = GMath.mul (new Vector4(In.Position,1) , worldView);
                var N = GMath.mul (new Vector4(In.Normal, 0), worldView);

                return new ProjectedPositionNormalData
                {
                    Projected = GMath.mul (P, Context.Projection),
                    Position = (Vector3)P,
                    Normal = GMath.normalize ((Vector3)N)
                };
            }

            Effects.Shaders.ShaderSource IVertexProcessor.Processor
            {
                get { return CreateSourceFrom<PositionNormalData, ProjectedPositionNormalData>(Transform); }
            }
        }

        class LitPipeline : Pipeline
        {
            protected LitPipeline(IBasicContext context)
                : base(context)
            {
            }

            [ShaderMethod]
            void PerformLit(LightingContext light, SurfaceData In, out Vector3 diffuse, out Vector3 specular)
            {
                var vLightPos = GMath.mul(light.Position, Context.View);
                var lightDir = GMath.normalize((Vector3)vLightPos - In.Position);
                var lambert = GMath.max(0, GMath.dot(In.Normal, lightDir));
                diffuse = lambert * light.Diffuse;
                var H = GMath.normalize(lightDir - In.Position);
                var blinn = GMath.pow(GMath.max(0.01f, GMath.dot(H, In.Normal)), Context.Material.Shininess);
                //if (GMath.dot(lightDir, In.Normal) < 0.0001f)
                //    blinn = 0;
                specular = blinn * light.Specular;
            }

            ColorsData Lit(SurfaceData In)
            {
                var totaldiffuse = new Vector3(0, 0, 0);
                var totalspecular = new Vector3(0, 0, 0);

                Vector3 diffuse, specular;

                if (Context.NumberOfLights > 0)
                {
                    PerformLit(Context.Light0, In, out diffuse, out specular);
                    totaldiffuse += diffuse;
                    totalspecular += specular;
                }
                //if (Context.NumberOfLights > 1)
                //{
                //    PerformLit(Context.Light1, In, out diffuse, out specular);
                //    totaldiffuse += diffuse;
                //    totalspecular += specular;
                //}

                return new ColorsData { Diffuse = new Vector4((Context.AmbientColor * Context.Material.Ambient + totaldiffuse) * (Vector3)In.Diffuse, In.Diffuse.W), Specular = totalspecular * In.Specular };
            }

            protected ShaderSource LitShader
            {
                get
                {
                    return CreateSourceFrom<SurfaceData, ColorsData>(Lit);
                }
            }
        }

        class PixelLitPipeline : LitPipeline, IPixelProcessor
        {
            protected PixelLitPipeline(IBasicContext context) : base(context) { }

            ShaderSource IPixelProcessor.Processor
            {
                get { return LitShader; }
            }
        }

        class VertexLitPipeline : LitPipeline, IVertexProcessor
        {
            protected VertexLitPipeline(IBasicContext context) : base(context) { }

            ShaderSource IVertexProcessor.Processor
            {
                get { return LitShader; }
            }
        }

        public static Effect UseMaterial
        {
            get
            {
                return Prepend<UseMaterialPipeline>();
            }
        }

        class UseMaterialPipeline : Pipeline, IVertexProcessor
        {
            UseMaterialPipeline(IBasicContext context)
                : base(context)
            {
            }

            ColorsData LoadColors(ColorsData data)
            {
                //return new ColorsData { Diffuse = new Vector4 (Context.Material.Diffuse,1), Specular = new Vector3 () };
                return new ColorsData { Diffuse = new Vector4(Context.Material.Diffuse * (Vector3)data.Diffuse, Context.Material.Opacity * data.Diffuse.W), Specular = Context.Material.Specular * data.Specular };
            }

            ShaderSource IVertexProcessor.Processor
            {
                get { return CreateSourceFrom<ColorsData, ColorsData>(LoadColors); }
            }
        }

        public static Effect VertexLit
        {
            get
            {
                return Append<VertexLitPipeline>();
            }
        }

        public static Effect PixelLit
        {
            get
            {
                return Append<PixelLitPipeline>();
            }
        }

        class CombineLightPipeline : Pipeline, IPixelProcessor
        {
            CombineLightPipeline(IBasicContext context) : base(context) { }

            ColorData Combine(ColorsData data)
            {
                return new ColorData { Color = new Vector4((Vector3)data.Diffuse + data.Specular, data.Diffuse.W) };
            }

            ShaderSource IPixelProcessor.Processor
            {
                get
                {
                    return CreateSourceFrom<ColorsData, ColorData>(Combine);
                }
            }
        }

        public static Effect CombineLight
        {
            get { return Append<CombineLightPipeline>(); }
        }

        public static Effect BumpMap(TextureBuffer bump, float ratio)
        {
            return null;
        }

        public static Effect Gouraud
        {
            get { return Effect.Concat(None, VertexTransform, UseMaterial, VertexLit, CombineLight); }
        }

        public static Effect Phong
        {
            get { return Effect.Concat(None, VertexTransform, UseMaterial, PixelLit, CombineLight); }
        }

        public static Effect Set<P>() where P : Pipeline
        {
            return Use<P>(AppendMode.Replace);
        }
        public static Effect Append<P>() where P : Pipeline
        {
            return Use<P>(AppendMode.Append);
        }
        public static Effect Prepend<P>() where P : Pipeline
        {
            return Use<P>(AppendMode.Prepend);
        }
        public static Effect Use<P>(AppendMode mode) where P : Pipeline
        {
            return new PipelineEffect<P>(mode);
        }
    }
}
