#define FLOAT

#if DECIMAL
using FLOATINGTYPE = System.Decimal;
#endif
#if FLOATINGTYPE
using FLOATINGTYPE = System.FLOATINGTYPE;
#endif
#if FLOAT
using FLOATINGTYPE = System.Single;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Effects;
using System.Collections;
using System.Unsafe;
using System.Drawing;
using SRE = System.Rendering.RenderStates;
using System.Rendering.Effects.Shaders;
using System.Reflection;
using System.IO;
using System.Rendering.RenderStates;
using System.Maths;
using System.Compilers.Shaders.ShaderAST;
using System.Compilers.Shaders.Info;
using System.Runtime.InteropServices;
using System.Compilers.Shaders;
using System.Compilers.Shaders.HLSL;
using D3D = SlimDX.Direct3D9;
using DX = SlimDX;

namespace System.Rendering.Direct3D9
{
    partial class Direct3DRender
    {
        public class RenderStatesManager : Direct3DRender.RenderStatesManagerBase,
            IRenderStateSetterOf<FrameBufferState>,
            IRenderStateSetterOf<DepthBufferState>,
            IRenderStateSetterOf<StencilBufferState>,
            IRenderStateSetterOf<FillModeState>,
            IRenderStateSetterOf<CullModeState>,
            IRenderStateSetterOf<MaterialState>,
            IRenderStateSetterOf<LightingState>,
            IRenderStateSetterOf<TextureState>,
            IRenderStateSetterOf<SamplersState>,
            IRenderStateSetterOf<BlendOperationState>,
            IRenderStateSetterOf<WorldState>,
            IRenderStateSetterOf<ViewState>,
            IRenderStateSetterOf<ProjectionState>,
            IRenderStateSetterOf<VertexShaderState>,
            IRenderStateSetterOf<PixelShaderState>,

            IBasicContext
        {
            public RenderStatesManager(Direct3DRender render)
                : base(render)
            {
                render.Created += new EventHandler(render_Created);
            }

            void render_Created(object sender, EventArgs e)
            {
                effectManager = new Direct3DEffectManager(render as Direct3DRender);

                this.Create<FrameBufferState>(FrameBufferState.Default);
                this.Create<DepthBufferState>(DepthBufferState.Default);
                this.Create<StencilBufferState>(StencilBufferState.Default);
                this.Create<FillModeState>(FillModeState.Default);
                this.Create<CullModeState>(CullModeState.None);
                this.Create<MaterialState>(MaterialState.Default);
                this.Create<LightingState>(LightingState.Default);
                this.Create<TextureState>(TextureState.Default);
                this.Create<SamplersState>(SamplersState.Default);
                this.Create<TextureFactorState>(TextureFactorState.Default);
                this.Create<WorldState>(WorldState.Default);
                this.Create<ViewState>(ViewState.Default);
                this.Create<ProjectionState>(ProjectionState.Default);
                this.Create<BlendOperationState>(BlendOperationState.Default);

                NoPipeline initialPipeline = new NoPipeline(this);

                VertexShaderState vss = new VertexShaderState(((IVertexProcessor)initialPipeline).Processor);
                this.Create<VertexShaderState>(vss);

                PixelShaderState pss = new PixelShaderState(((IPixelProcessor)initialPipeline).Processor);
                this.Create<PixelShaderState>(pss);
            }

            D3D.Device device { get { return ((Direct3DRender)render).device; } }

            Direct3DEffectManager effectManager;

            internal Direct3DEffectManager PipelineManager { get { return effectManager; } }

            protected IResourcesManager Resources { get { return (IResourcesManager)((Direct3DRender)render).Resources; } }

            #region IRenderSetterOf<FrameBufferState> Members

            FrameBufferState IRenderStateSetterOf<FrameBufferState>.State
            {
                set
                {
                    int red = Convert.ToInt32((value.Mask & ColorMask.Red) != 0);
                    int green = Convert.ToInt32((value.Mask & ColorMask.Green) != 0);
                    int blue = Convert.ToInt32((value.Mask & ColorMask.Blue) != 0);
                    int alpha = Convert.ToInt32((value.Mask & ColorMask.Alpha) != 0);

                    D3D.ColorWriteEnable cwe = (D3D.ColorWriteEnable)((int)D3D.ColorWriteEnable.Red * red | (int)ColorMask.Green * green | (int)ColorMask.Blue * blue | (int)ColorMask.Alpha * alpha);
                    device.SetRenderState ( D3D.RenderState.ColorWriteEnable, cwe);

                    if (value.ClearOnSet && !Restoring)
                        device.Clear(D3D.ClearFlags.Target, value.DefaultValue.ToARGB(), 0, 0);
                }
            }

            #endregion

            #region IRenderSetterOf<DepthBufferState> Members

            DepthBufferState IRenderStateSetterOf<DepthBufferState>.State
            {
                set
                {
                    if (device == null)
                        return;

                    device.SetRenderState(D3D.RenderState.ZEnable ,value.EnableTest);
                    device.SetRenderState(D3D.RenderState.ZWriteEnable , value.WriteEnable);
                    device.SetRenderState(D3D.RenderState.ZFunc , Direct3D9Tools.FromCompare(value.Function));

                    if (value.ClearOnSet && !Restoring)
                        device.Clear(D3D.ClearFlags.ZBuffer, Color.Black, (float)value.DefaultValue, 0);
                }
            }

            #endregion

            #region IRenderSetterOf<StencilBufferState> Members

            StencilBufferState IRenderStateSetterOf<StencilBufferState>.State
            {
                set
                {

                }
            }

            #endregion

            #region IRenderSetterOf<FillModeState> Members

            FillModeState IRenderStateSetterOf<FillModeState>.State
            {
                set
                {
                    device.SetRenderState(D3D.RenderState.FillMode, (D3D.FillMode)value);
                }
            }

            #endregion

            #region IRenderSetterOf<MaterialState> Members

            MaterialState IRenderStateSetterOf<MaterialState>.State
            {
                set
                {
                    this._Material = new MaterialContext
                    {
                        Ambient = value.Ambient,
                        Diffuse = value.Diffuse,
                        Emissive = value.Emission,
                        Shininess = (float)value.SpecularSharpness,
                        Specular = value.Specular,
                        Opacity = value.Opacity
                    };
                }
            }

            #endregion

            #region IRenderSetterOf<LightingState> Members

            LightingState IRenderStateSetterOf<LightingState>.State
            {
                set
                {
                    var ambientLight = value.Lights.OfType<AmbientLightSource>().LastOrDefault();
                    if (ambientLight != null)
                        _Ambient = ambientLight.Ambient;

                    var lights = value.Lights.OfType<PointLightSource>().Select(l =>
                        {
                            LightingContext light = new LightingContext();

                            light.Diffuse = l.Diffuse;
                            light.Position = new Vector4(l.Position, 1);
                            light.Specular = l.Specular;

                            return light;
                        }).ToArray();
                    if (lights.Length > 3)
                        lights = lights.Skip(this._Lights.Length - 3).ToArray();
                    lights.CopyTo(_Lights, 0);
                    _NumberOfLights = lights.Length;
                }
            }

            #endregion

            #region IRenderSetterOf<TextureState> Members
            
            protected new Direct3DResourcesManager D3DResources
            {
                get { return base.Resources as Direct3DResourcesManager; }
            }

            TextureState IRenderStateSetterOf<TextureState>.State
            {
                set
                {
                }
            }

            SamplersState IRenderStateSetterOf<SamplersState>.State
            {
                set
                {
                    List<Sampler2D> samplers = value.GetSamplers().OfType<Sampler2D>().ToList();
                    if (samplers.Count > 0)
                    {
                        _HasSampling = true;
                        _Sampler = samplers[samplers.Count - 1];
                    }
                    else
                        _HasSampling = false;
                }
            }

            #endregion

            #region IRenderSetterOf<BlendOperationState> Members

            D3D.Blend To(SRE.Blend blend)
            {
                switch (blend)
                {
                    case SRE.Blend.DestinationAlpha: return D3D.Blend.DestinationAlpha;
                    case SRE.Blend.DestinationColor: return D3D.Blend.DestinationColor;
                    case SRE.Blend.InvDestinationAlpha: return D3D.Blend.InverseDestinationAlpha;
                    case SRE.Blend.InvDestinationColor: return D3D.Blend.InverseDestinationColor;
                    case SRE.Blend.InvSourceAlpha: return D3D.Blend.InverseSourceAlpha;
                    case SRE.Blend.InvSourceColor: return D3D.Blend.InverseSourceColor;
                    case SRE.Blend.One: return D3D.Blend.One;
                    case SRE.Blend.SourceAlpha: return D3D.Blend.SourceAlpha;
                    case SRE.Blend.SourceAlphaSat: return D3D.Blend.SourceAlphaSaturated;
                    case SRE.Blend.SourceColor: return D3D.Blend.SourceColor;
                    case SRE.Blend.Zero: return D3D.Blend.Zero;
                    default: return D3D.Blend.One;
                }
            }


            BlendOperationState IRenderStateSetterOf<BlendOperationState>.State
            {
                set
                {
                    device.SetRenderState(D3D.RenderState.SourceBlend, To(value.Source));
                    device.SetRenderState(D3D.RenderState.DestinationBlend, To(value.Destination));
                    device.SetRenderState(D3D.RenderState.AlphaBlendEnable, value.Enable);
                }
            }

            #endregion

            #region IRenderSetterOf<WorldState> Members

            WorldState IRenderStateSetterOf<WorldState>.State
            {
                set
                {
                    _World = value.Matrix;
                }
            }

            #endregion

            #region IRenderSetterOf<ViewState> Members

            ViewState IRenderStateSetterOf<ViewState>.State
            {
                set
                {
                    _View = value.Matrix;
                }
            }

            #endregion

            #region IRenderSetterOf<ProjectionState> Members

            ProjectionState IRenderStateSetterOf<ProjectionState>.State
            {
                set
                {
                    _Projection = value.Matrix;
                }
            }

            #endregion

            #region IRenderSetterOf<VertexShaderState> Members

            VertexShaderState IRenderStateSetterOf<VertexShaderState>.State
            {
                set
                {
                    try
                    {
                        GlobalBindings bindings;

                        var program = value.BuildASTFor(HLSLBasic.HLSL, out bindings);

                        effectManager.SetShaderFor(ShaderStage.Vertex, program, bindings);
                    }
                    catch (BuildingErrorException b)
                    {
                        throw new NotSupportedException("Some code is not supported by this compiler", b);
                    }
                    catch (Exception e)
                    {
                        throw new NotSupportedException("Some code is not supported by this compiler", e);
                    }
                }
            }

            #endregion

            #region IRenderSetterOf<PixelShaderState> Members

            PixelShaderState IRenderStateSetterOf<PixelShaderState>.State
            {
                set
                {
                    try
                    {
                        GlobalBindings bindings;

                        var program = value.BuildASTFor(HLSLBasic.HLSL, out bindings);

                        effectManager.SetShaderFor(ShaderStage.Pixel, program, bindings);
                    }
                    catch (BuildingErrorException b)
                    {
                        throw new NotSupportedException("Some code is not supported by this compiler", b);
                    }
                    catch (Exception e)
                    {
                        throw new NotSupportedException("Some code is not supported by this compiler", e);
                    }
                }
            }

            #endregion

            #region IRenderStateSetterOf<CullModeState> Members

            CullModeState IRenderStateSetterOf<CullModeState>.State
            {
                set
                {
                    switch (value)
                    {
                        case CullModeState.None: device.SetRenderState(D3D.RenderState.CullMode, D3D.Cull.None);
                            break;
                        case CullModeState.Back: device.SetRenderState(D3D.RenderState.CullMode, D3D.Cull.Clockwise);
                            break;
                        case CullModeState.Front: device.SetRenderState(D3D.RenderState.CullMode, D3D.Cull.Counterclockwise);
                            break;
                        case CullModeState.Front_and_Back: device.SetRenderState(D3D.RenderState.CullMode, D3D.Cull.Counterclockwise | D3D.Cull.Clockwise);
                            break;
                    }
                }
            }

            #endregion

            Matrix4x4 _World;
            Matrix4x4 IBasicContext.World
            {
                get { return _World; }
            }

            Matrix4x4 _View;
            Matrix4x4 IBasicContext.View
            {
                get { return _View; }
            }

            Matrix4x4 _Projection;
            Matrix4x4 IBasicContext.Projection
            {
                get { return _Projection; }
            }

            MaterialContext _Material;
            MaterialContext IBasicContext.Material
            {
                get { return _Material; }
            }

            LightingContext[] _Lights = new LightingContext[3];

            LightingContext IBasicContext.Light0
            {
                get { return _Lights[0]; }
            }
            LightingContext IBasicContext.Light1
            {
                get { return _Lights[1]; }
            }
            LightingContext IBasicContext.Light2
            {
                get { return _Lights[2]; }
            }

            Vector3 _Ambient;
            Vector3 IBasicContext.AmbientColor
            {
                get { return _Ambient; }
            }

            int _NumberOfLights;
            int IBasicContext.NumberOfLights
            {
                get { return _NumberOfLights; }
            }

            bool _HasSampling = false;
            bool IBasicContext.HasSampling
            {
                get { return _HasSampling; }
            }

            Sampler2D _Sampler;
            Sampler2D IBasicContext.Sampler
            {
                get { return _Sampler; }
            }
        }
    }
}