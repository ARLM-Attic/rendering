using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.ShaderAST;
using System.Rendering.Effects.Shaders;
using System.Compilers.Shaders;
using System.Maths;
using System.Rendering.RenderStates;
using System.Rendering.Effects;

namespace System.Rendering
{
    public abstract class Pipeline
    {
        public readonly IBasicContext Context;

        public Pipeline(IBasicContext context)
        {
            this.Context = context;
            DesiredAppendMode = AppendMode.Replace;
        }

        public AppendMode DesiredAppendMode { get; set; }

        protected ShaderSource CreateSourceFrom<Input, Output>(Func<Input, Output> method)
        {
            return ShaderSource.From(method);
        }
    }

    public interface IVertexProcessor 
    {
        ShaderSource Processor { get; }
    }

    public interface IPixelProcessor 
    {
        ShaderSource Processor { get; }
    }

    public interface IGeometryProcessor 
    {
        ShaderSource Processor { get; }
    }

    class PipelineEffect<P> : Effect where P : Pipeline
    {
        public PipelineEffect()
            : this(AppendMode.Replace)
        {
        }

        AppendMode mode;

        public PipelineEffect(AppendMode mode)
        {
            this.mode = mode;
        }

        protected override Technique GetTechnique(IRenderStatesManager manager)
        {
            P pipeline = Activating.CreateInstance<P>(manager as IBasicContext);

            pipeline.DesiredAppendMode = mode;

            return new PipelineEffectTechnique(pipeline, manager);
        }

        protected override Location OnClone(Resourcing.AllocateableBase toFill, IRenderDevice render)
        {
            return Location.Device;
        }

        protected override void OnDispose()
        {
        }

        class PipelineEffectTechnique : Technique
        {
            public PipelineEffectTechnique(P pipeline, IRenderStatesManager manager) :base (manager)
            {
                this.pipeline = pipeline;
            }

            P pipeline;

            protected override void SaveStates()
            {
                if (pipeline is IVertexProcessor)
                    RenderStates.Save<VertexShaderState>();
                if (pipeline is IPixelProcessor)
                    RenderStates.Save<PixelShaderState>();
            }

            protected override IEnumerable<Pass> Passes
            {
                get
                {
                    if (pipeline is IVertexProcessor)
                    {
                        VertexShaderState vs;
                        switch (pipeline.DesiredAppendMode)
                        {
                            case AppendMode.Append:
                                vs = VertexShaderState.Join(RenderStates.GetState<VertexShaderState>(), new VertexShaderState(((IVertexProcessor)pipeline).Processor));
                                RenderStates.SetState<VertexShaderState>(vs);
                                break;
                            case AppendMode.Prepend:
                                vs = VertexShaderState.Join(new VertexShaderState(((IVertexProcessor)pipeline).Processor), RenderStates.GetState<VertexShaderState>());
                                RenderStates.SetState<VertexShaderState>(vs);
                                break;
                            case AppendMode.Replace:
                                vs = new VertexShaderState(((IVertexProcessor)pipeline).Processor);
                                RenderStates.SetState<VertexShaderState>(vs);
                                break;
                        }
                    }

                    if (pipeline is IPixelProcessor)
                    {
                        PixelShaderState ps;
                        switch (pipeline.DesiredAppendMode)
                        {
                            case AppendMode.Append:
                                ps = PixelShaderState.Join(RenderStates.GetState<PixelShaderState>(), new PixelShaderState(((IPixelProcessor)pipeline).Processor));
                                RenderStates.SetState<PixelShaderState>(ps);
                                break;
                            case AppendMode.Prepend:
                                ps = PixelShaderState.Join(new PixelShaderState(((IPixelProcessor)pipeline).Processor), RenderStates.GetState<PixelShaderState>());
                                RenderStates.SetState<PixelShaderState>(ps);
                                break;
                            case AppendMode.Replace:
                                ps = new PixelShaderState(((IPixelProcessor)pipeline).Processor);
                                RenderStates.SetState<PixelShaderState>(ps);
                                break;
                        }
                    }

                    yield return NewPass();
                }
            }

            protected override void RestoreStates()
            {
                if (pipeline is IPixelProcessor)
                    RenderStates.Restore<PixelShaderState>();
                if (pipeline is IVertexProcessor)
                    RenderStates.Restore<VertexShaderState>();
            }
        }
    }

    public class NoPipeline : Pipeline, IVertexProcessor, IPixelProcessor
    {
        public NoPipeline(IBasicContext context)
            : base(context)
        {
        }

        internal protected virtual ProjectedData VertexShader(PositionData In)
        {
            return new ProjectedData { Position = new Vector4(In.Position, 1) };
        }

        internal protected virtual ColorData PixelShader(ColorData In)
        {
            return new ColorData { Color = In.Color };
        }

        ShaderSource IVertexProcessor.Processor
        {
            get
            {
                return CreateSourceFrom<PositionData, ProjectedData>(VertexShader);
            }
        }

        ShaderSource IPixelProcessor.Processor
        {
            get
            {
                return CreateSourceFrom<ColorData, ColorData>(PixelShader);
            }
        }
    }

    public class GouraudPipeline : Pipeline, IVertexProcessor, IPixelProcessor
    {
        public GouraudPipeline(IBasicContext context)
            : base(context)
        {
        }

        [ShaderType]
        public struct VSIn
        {
            [Position]
            public Vector3 Position;
            [Normal]
            public Vector3 Normal;
            [Color]
            public Vector4 Diffuse;
            [Coordinates]
            public Vector4 TexCoord;
        }

        [ShaderType]
        public struct VSOut
        {
            [Projected]
            public Vector4 Projected;

            [Color(0)]
            public Vector4 Diffuse;
            [Color(1)]
            public Vector4 Specular;

            [Coordinates]
            public Vector4 TexCoord;
        }

        [ShaderType]
        public struct PSIn
        {
            [Color(0)]
            public Vector4 Diffuse;
            [Color(1)]
            public Vector4 Specular;

            [Coordinates]
            public Vector4 TexCoord;
        }

        [ShaderType]
        public struct PSOut
        {
            [Color]
            public Vector4 Diffuse;
        }

        internal protected virtual VSOut VertexShader(VSIn In)
        {
            VSOut Out = new VSOut();
            Matrix4x4 world = Context.World;
            Matrix4x4 view = Context.View;
            Matrix4x4 projection = Context.Projection;

            var worldPosition = GMath.mul(new Vector4(In.Position, 1), world);
            var viewPosition = GMath.mul(worldPosition, view);
            var viewNormal = GMath.normalize((Vector3)GMath.mul(new Vector4(GMath.normalize(In.Normal), 0), GMath.mul(world, view)));

            var viewLPosition = GMath.normalize((Vector3)GMath.mul(Context.Light0.Position - worldPosition, view));
            Vector3 V = GMath.normalize(-1*(Vector3)viewPosition);

            float NdotL = GMath.max(GMath.dot(viewNormal, viewLPosition), 0.0f);
            Vector3 H = GMath.normalize(viewLPosition + V);
            float NdotH = GMath.max(GMath.dot(viewNormal, H), 0.0f);

            Out.Diffuse = new Vector4(Context.Material.Ambient * Context.AmbientColor + Context.Material.Diffuse * (Vector3)In.Diffuse * Context.Light0.Diffuse * NdotL, Context.Material.Opacity * In.Diffuse.W);
            Out.Specular = new Vector4(Context.Material.Specular * Context.Light0.Specular * GMath.pow(NdotH, Context.Material.Shininess), 1);

            Out.TexCoord = In.TexCoord;
            Out.Projected = GMath.mul(viewPosition, projection);

            return Out;
        }

        internal protected virtual PSOut PixelShader(PSIn In)
        {
            PSOut Out = new PSOut();

            Vector4 texColor = new Vector4 (1,1,1,1);
            if (Context.HasSampling)
                texColor = Context.Sampler.Sample((Vector2)In.TexCoord);

            Out.Diffuse = texColor * In.Diffuse + new Vector4((Vector3)In.Specular, 0);

            return Out;
        }

        ShaderSource IVertexProcessor.Processor
        {
            get
            {
                return CreateSourceFrom<VSIn, VSOut>(VertexShader);
            }
        }
        
        ShaderSource IPixelProcessor.Processor
        {
            get
            {
                return CreateSourceFrom<PSIn,PSOut>(PixelShader);
            }
        }
    }
}
