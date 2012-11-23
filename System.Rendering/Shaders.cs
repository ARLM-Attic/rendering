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
    public static class Shaders
    {
        /// <summary>
        /// Prepends a per-pixel texture map of the diffuse color (Color0).
        /// </summary>
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
        /// <summary>
        /// Prepends a per-pixel function map of the diffuse color (Color0).
        /// </summary>
        /// <param name="map">A function representing the color for current surface coordinates in tangent space.</param>
        public static Effect DiffuseMap(Func<Vector2, Vector4> map)
        {
            return new PixelShaderEffect<CoordinatesData, ColorData>(In => new ColorData { Color = map(In.Coordinates) }) { AppendMode = AppendMode.Prepend };
        }
        /// <summary>
        /// Prepends a per-pixel function map of the diffuse color (Color0).
        /// </summary>
        /// <param name="map">A function representing the color for the current 3D surface coordinates in tangent space.</param>
        public static Effect DiffuseMap3D(Func<Vector3, Vector4> map)
        {
            return new PixelShaderEffect<Coordinates3DData, ColorData>(In => new ColorData { Color = map(In.Coordinates) }) { AppendMode = AppendMode.Prepend };
        }
        /// <summary>
        /// Prepends a per-pixel texture map of the specular color (Color1).
        /// </summary>
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
        /// <summary>
        /// Prepends a per-pixel function map of the specular color (Color1).
        /// </summary>
        /// <param name="map">A function representing the color for the current 3D surface coordinates in tangent space.</param>
        public static Effect SpecularMap(Func<Vector2, Vector4> map)
        {
            return new PixelShaderEffect<CoordinatesData, SecondaryColorData>(In => new SecondaryColorData { Color = map(In.Coordinates) }) { AppendMode = AppendMode.Prepend };
        }
        /// <summary>
        /// Prepends a per-pixel function map of the specular color (Color1).
        /// </summary>
        /// <param name="map">A function representing the color for the current 3D surface coordinates in tangent space.</param>
        public static Effect SpecularMap3D(Func<Vector3, Vector4> map)
        {
            return new PixelShaderEffect<Coordinates3DData, SecondaryColorData>(In => new SecondaryColorData { Color = map(In.Coordinates) }) { AppendMode = AppendMode.Prepend };
        }

        /// <summary>
        /// Appends a per-vertex setting of 3D coordinates using current vertex position.
        /// </summary>
        public static Effect UseOrthographicCoordinates
        {
            get { return new VertexShaderEffect<PositionData, Coordinates3DData>(In => new Coordinates3DData { Coordinates = In.Position }) { AppendMode = AppendMode.Append }; }
        }

        /// <summary>
        /// Appends a per-vertex setting of 3D coordinates using current vertex normal.
        /// </summary>
        public static Effect UseNormalCoordinates
        {
            get { return new VertexShaderEffect<NormalData, Coordinates3DData>(In => new Coordinates3DData { Coordinates = In.Normal }) { AppendMode = AppendMode.Append }; }
        }

        /// <summary>
        /// Appends a per-vertex setting of 3D coordinates using current vertex normal, position to get the reflection vector.
        /// </summary>
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

        /// <summary>
        /// Appends a per-vertex setting of 2D coordinates using current vertex normal normalized to 2D.
        /// </summary>
        public static Effect Use2DNormalCoordinates
        {
            get
            {
                return new VertexShaderEffect<NormalData, CoordinatesData>(In => new CoordinatesData { Coordinates = new Vector2(In.Normal.X / 2 + 0.5f, 0.5f - In.Normal.Y / 2) }) { AppendMode = AppendMode.Append };
            }
        }

        /// <summary>
        /// Appends a per-vertex setting of 2D coordinates using current vertex normal and position to get the reflection vector normalized to 2D.
        /// </summary>
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

        /// <summary>
        /// Replace all pipeline shaders to default. Allows to restart pipeline programming.
        /// </summary>
        public static Effect None
        {
            get { return Set<NoPipeline>(); }
        }

        /// <summary>
        /// Appends a default vertex transformation using World, View and Projection matrices render states. Also, modify position and normal to view space.
        /// </summary>
        public static Effect DefaultVertexTransform { get { return Append<VertexTransformPipeline>(); } }

        class VertexTransformPipeline : Pipeline, IVertexProcessor
        {
            VertexTransformPipeline(IBasicContext context) : base(context) { }

            ProjectedSurfaceData Transform(SurfaceData In)
            {
                var worldView = GMath.mul(Context.World, Context.View);

                var P = GMath.mul(new Vector4(In.Position, 1), worldView);

                Matrix3x3 worldViewRot = (Matrix3x3)worldView;

                var N = GMath.mul(In.Normal, worldViewRot);
                var T = GMath.mul(In.Tangent, worldViewRot);
                var B = GMath.mul(In.Binormal, worldViewRot);

                return new ProjectedSurfaceData
                {
                    Projected = GMath.mul(P, Context.Projection),
                    Position = (Vector3)P,
                    Normal = N,
                    Binormal = B,
                    Tangent = T,
                    Coordinates = In.Coordinates,
                    Diffuse = In.Diffuse,
                    Specular = In.Specular
                };
            }

            Effects.Shaders.ShaderSource IVertexProcessor.Processor
            {
                get { return CreateSourceFrom<SurfaceData, ProjectedSurfaceData>(Transform); }
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
                var cosH = GMath.dot(H, In.Normal);
                if (cosH > 0)
                    specular = GMath.pow(cosH, Context.Material.Shininess) * light.Specular;
                else
                    specular = new Vector3(0, 0, 0);
            }

            ColorsData Lit(SurfaceData In)
            {
                var totaldiffuse = new Vector3(0, 0, 0);
                var totalspecular = new Vector3(0, 0, 0);

                Vector3 diffuse, specular;

                PerformLit(Context.Light0, In, out diffuse, out specular);
                totaldiffuse += diffuse;
                totalspecular += specular;

                //PerformLit(Context.Light1, In, out diffuse, out specular);
                //totaldiffuse += diffuse;
                //totalspecular += specular;

                //PerformLit(Context.Light2, In, out diffuse, out specular);
                //totaldiffuse += diffuse;
                //totalspecular += specular;

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

        /// <summary>
        /// Prepends a per-vertex setting of diffuse and specular colors from material render state.
        /// </summary>
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

        /// <summary>
        /// Performs a per-vertex lit using Lighting render states.
        /// </summary>
        public static Effect VertexLit
        {
            get
            {
                return Append<VertexLitPipeline>();
            }
        }

        /// <summary>
        /// Performs a per-pixel lit using Lighting render states.
        /// </summary>
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

        /// <summary>
        /// Appends the sum of colors to get a final color.
        /// </summary>
        public static Effect CombineLight
        {
            get { return Append<CombineLightPipeline>(); }
        }

        public static Effect BumpMap(TextureBuffer bump, float ratio)
        {
            Sampler2D normalMapSampler = new Sampler2D(bump) { Filters = new Filtering { Magnification = TextureFilterType.Linear, Minification = TextureFilterType.Linear, MipMapping = TextureFilterType.Linear } };
            return new PixelShaderEffect<SurfaceData, NormalData>(In =>
            {
                var data = normalMapSampler.Sample(In.Coordinates);
                var bN = new Vector3(data.X * 2 - 1, data.Y * 2 - 1, data.Z * 2 - 1);

                var bin = In.Binormal;
                var tan = In.Tangent;

                Matrix3x3 tangentToView = new Matrix3x3(
                    bin.X, bin.Y, bin.Z,
                    tan.X, tan.Y, tan.Z,
                    In.Normal.X, In.Normal.Y, In.Normal.Z);

                bN = GMath.mul(bN, tangentToView);

                return new NormalData { Normal = GMath.normalize(In.Normal * (1 - ratio) + bN * ratio) };
            }) { AppendMode = AppendMode.Prepend };
        }

        const int NumberOfSteps = 50;

        public static Effect ParallaxMap(TextureBuffer height, float ratio)
        {
            Sampler2D heightMapSampler = new Sampler2D(height) { Filters = new Filtering { Magnification = TextureFilterType.Linear, Minification = TextureFilterType.Linear, MipMapping = TextureFilterType.Linear } };
            return new PixelShaderEffect<SurfaceData, CoordinatesData>(In =>
            {
                var bin = GMath.normalize(In.Binormal);
                var tan = GMath.normalize(In.Tangent);
                var nor = GMath.normalize(In.Normal);

                Matrix3x3 tangentToView = new Matrix3x3(
                    bin.X, bin.Y, bin.Z,
                    tan.X, tan.Y, tan.Z,
                    nor.X, nor.Y, nor.Z);

                Matrix3x3 viewToTangent = GMath.transpose(tangentToView);

                Vector3 viewer = GMath.normalize(-1 * In.Position);

                Vector3 viewerInTS = GMath.normalize(GMath.mul(viewer, viewToTangent)) / GMath.dot(In.Normal, viewer);

                viewerInTS *= -1;

                Vector3 T = new Vector3(In.Coordinates.X, In.Coordinates.Y, 1);

                //viewerInTS.Z = viewerInTS.Z / ratio;
                viewerInTS = viewerInTS * (1.0f / 50f);

                viewerInTS.Z *= -1;

                float h = 0;

                bool found = false;

                for (int i = 0; i < 50; i++)
                {
                    //if (invert)
                    //    h = heightMapSampler.Sample((Vector2)T).X;
                    //else
                    h = (1 - (ratio) * heightMapSampler.Sample((Vector2)T).X);

                    if (h >= T.Z)
                    {
                        if (!found)
                            In.Coordinates = (Vector2)T;
                        found = true;
                    }
                    else
                        //if (!found)
                        T += viewerInTS;
                }

                if (!found)
                    In.Coordinates = (Vector2)T;

                return new CoordinatesData { Coordinates = (Vector2)In.Coordinates };
            }) { AppendMode = AppendMode.Prepend };
        }

        /// <summary>
        /// Sets a gouraud efects given by VertexTransform, use of material, per-vertex lighting and combine.
        /// </summary>
        public static Effect Gouraud
        {
            get { return Effect.Concat(DefaultVertexTransform, UseMaterial, VertexLit, CombineLight); }
        }

        /// <summary>
        /// Sets a phong efects given by VertexTransform, use of material, per-pixel lighting and combine.
        /// </summary>
        public static Effect Phong
        {
            get { return Effect.Concat(DefaultVertexTransform, UseMaterial, PixelLit, CombineLight); }
        }

        /// <summary>
        /// Prepends a free-transform to each vertex of the model. Data will be mapped from a type to same type.
        /// </summary>
        /// <typeparam name="T">Type of the vertex to be transformed.</typeparam>
        /// <param name="transform">Transformation function.</param>
        /// <returns>A VertexShaderEffect object that performs the transformation.</returns>
        public static Effect FreeTransform<T>(Func<T, T> transform) where T : struct
        {
            return new VertexShaderEffect<T, T>(transform) { AppendMode = AppendMode.Prepend };
        }

        /// <summary>
        /// Prepends a free-transform to each pixel of the surface. Data will be mapped from a type to another type.
        /// </summary>
        /// <typeparam name="In">Input type of a pixel data.</typeparam>
        /// <typeparam name="Out">Output type of a pixel data.</typeparam>
        /// <param name="transform">Transformation function.</param>
        /// <returns>A PixelShaderEffect object that performs the pixel transformation.</returns>
        public static Effect PixelTransform<In, Out>(Func<In, Out> transform)
            where In : struct
            where Out : struct
        {
            return new PixelShaderEffect<In, Out>(transform) { AppendMode = AppendMode.Prepend };
        }

        /// <summary>
        /// Prepends a free-transform to vertex pixel of the surface. Data will be mapped from a type to another type.
        /// </summary>
        /// <typeparam name="In">Input type of a pixel data.</typeparam>
        /// <typeparam name="Out">Output type of a pixel data.</typeparam>
        /// <param name="transform">Transformation function.</param>
        /// <returns>A PixelShaderEffect object that performs the pixel transformation.</returns>
        public static Effect VertexTransform<In, Out>(Func<In, Out> transform)
            where In : struct
            where Out : struct
        {
            return new VertexShaderEffect<In, Out>(transform) { AppendMode = AppendMode.Prepend };
        }

        /// <summary>
        /// Sets a pipeline behaviour as an effect replacing existing behaviour on each stage.
        /// </summary>
        /// <typeparam name="P">Pipeline type used to replace the existing one.</typeparam>
        public static Effect Set<P>() where P : Pipeline
        {
            return new PipelineEffect<P>(AppendMode.Replace);
        }

        /// <summary>
        /// Appends a pipeline behaviour as an effect on each stage.
        /// </summary>
        /// <typeparam name="P">Pipeline type used to replace the existing one.</typeparam>
        public static Effect Append<P>() where P : Pipeline
        {
            return new PipelineEffect<P>(AppendMode.Append);
        }
        /// <summary>
        /// Prepends a pipeline behaviour as an effect on each stage.
        /// </summary>
        /// <typeparam name="P">Pipeline type used to replace the existing one.</typeparam>
        public static Effect Prepend<P>() where P : Pipeline
        {
            return new PipelineEffect<P>(AppendMode.Prepend);
        }
    }
}
