using System;
using System.Collections.Generic;
using System.Compilers.Generators;
using System.Compilers.Shaders.HLSL;
using System.Compilers.Shaders.ShaderAST;
using System.Linq;
using System.Maths;
using System.Rendering.Effects;
using System.Rendering.Effects.Shaders;
using System.Rendering.Modeling;
using System.Rendering.RenderStates;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaEffect = Microsoft.Xna.Framework.Graphics.Effect;
using XnaSamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;

namespace System.Rendering.Xna
{
    partial class XnaRender
    {
        public class XnaRenderStatesManager : RenderStatesManagerBase,
          IRenderStateSetterOf<FrameBufferState>,
          IRenderStateSetterOf<DepthBufferState>,
          IRenderStateSetterOf<StencilBufferState>,
          IRenderStateSetterOf<FillModeState>,
          IRenderStateSetterOf<CullModeState>,
          IRenderStateSetterOf<BlendOperationState>,
          IRenderStateSetterOf<MaterialState>,
          IRenderStateSetterOf<LightingState>,
          IRenderStateSetterOf<TextureState>,
          IRenderStateSetterOf<SamplersState>,
          IRenderStateSetterOf<WorldState>,
          IRenderStateSetterOf<ViewState>,
          IRenderStateSetterOf<ProjectionState>,
          IRenderStateSetterOf<VertexShaderState>,
          IRenderStateSetterOf<PixelShaderState>,

          IBasicContext
        {
            internal XnaEffectManager effectManager;

            private DepthStencilState depthStencilState;
            private RasterizerState rasterizerState;
            private BlendState blendState;

            public XnaRenderStatesManager(XnaRender render)
                : base(render)
            {
                render.Created += new EventHandler(render_Created);
            }

            private void render_Created(object sender, EventArgs e)
            {
                effectManager = new XnaEffectManager((XnaRender)render);
                depthStencilState = DepthStencilState.None.Clone();
                rasterizerState = RasterizerState.CullNone.Clone();
                blendState = BlendState.Opaque.Clone();

                Create<FrameBufferState>(FrameBufferState.Default);
                Create<DepthBufferState>(DepthBufferState.Default);
                Create<StencilBufferState>(StencilBufferState.Default);
                Create<FillModeState>(FillModeState.Default);
                Create<CullModeState>(CullModeState.Default);
                Create<BlendOperationState>(BlendOperationState.Default);
                Create<MaterialState>(MaterialState.Default);
                Create<LightingState>(LightingState.Default);
                Create<TextureState>(TextureState.Default);
                Create<SamplersState>(SamplersState.Default);
                Create<WorldState>(WorldState.Default);
                Create<ViewState>(ViewState.Default);
                Create<ProjectionState>(ProjectionState.Default);
                Create<SamplersState>(SamplersState.Default);

                NoPipeline initialPipeline = new NoPipeline(this);

                VertexShaderState vss = new VertexShaderState(((IVertexProcessor)initialPipeline).Processor);
                this.Create<VertexShaderState>(vss);

                PixelShaderState pss = new PixelShaderState(((IVertexProcessor)initialPipeline).Processor);
                this.Create<PixelShaderState>(pss);
            }

            private GraphicsDevice Device
            {
                get { return ((XnaRender)render)._device; }
            }

            private XnaResourcesManager ResourcesManager
            {
                get { return (XnaResourcesManager)Resources; }
            }

            #region [ IRenderStateSetterOf<FrameBufferState> ]

            FrameBufferState IRenderStateSetterOf<FrameBufferState>.State
            {
                set
                {
                    int red = Convert.ToInt32((value.Mask & ColorMask.Red) != 0);
                    int green = Convert.ToInt32((value.Mask & ColorMask.Green) != 0);
                    int blue = Convert.ToInt32((value.Mask & ColorMask.Blue) != 0);
                    int alpha = Convert.ToInt32((value.Mask & ColorMask.Alpha) != 0);

                    if (value.ClearOnSet && !Restoring)
                        Device.Clear(ClearOptions.Target, XnaTools.ToXnaVector(value.DefaultValue), 0, 0);
                }
            }

            #endregion

            #region [ IRenderStateSetterOf<DepthBufferState> ]

            DepthBufferState IRenderStateSetterOf<DepthBufferState>.State
            {
                set
                {
                    depthStencilState.DepthBufferEnable = value.EnableTest;
                    depthStencilState.DepthBufferWriteEnable = value.WriteEnable;
                    depthStencilState.DepthBufferFunction = XnaTools.ToCompareFunction(value.Function);

                    if (value.ClearOnSet && !Restoring)
                        Device.Clear(ClearOptions.DepthBuffer, XnaTools.ToXnaColor(Vectors.Black), (float)value.DefaultValue, 0);

                    Device.DepthStencilState = depthStencilState.Clone();
                }
            }

            #endregion

            #region [ IRenderStateSetterOf<StencilBufferState> ]

            StencilBufferState IRenderStateSetterOf<StencilBufferState>.State
            {
                set
                {
                    //depthStencilState.StencilEnable = value.TestEnable;
                    //depthStencilState.StencilWriteMask = Convert.ToInt32(value.WriteMask);
                    //depthStencilState.StencilMask = Convert.ToInt32(value.CompareMask);
                    //depthStencilState.StencilPass = Direct3DTools.ToStencilOperation(value.Pass);
                    //depthStencilState.StencilFail = Direct3DTools.ToStencilOperation(value.StencilFails);
                    //depthStencilState.StencilFunction = Direct3DTools.ToCompareFunction(value.Function);
                    //depthStencilState.StencilDepthBufferFail = Direct3DTools.ToStencilOperation(value.DepthFails);
                    //depthStencilState.ReferenceStencil = value.Reference;

                    //if (value.ClearOnSet && !Restoring)
                    //  Device.Clear(ClearOptions.Stencil, Direct3DTools.ToXnaVector(Vectors.Black), 0, 0);

                    //Device.DepthStencilState = depthStencilState.Clone();
                }
            }

            #endregion

            #region [ IRenderStateSetterOf<FillModeState> ]

            FillModeState IRenderStateSetterOf<FillModeState>.State
            {
                set
                {
                    rasterizerState.FillMode = XnaTools.ToXnaFillMode(value);

                    Device.RasterizerState = rasterizerState.Clone();
                }
            }

            #endregion

            #region [ IRenderStateSetterOf<CullModeState> ]

            CullModeState IRenderStateSetterOf<CullModeState>.State
            {
                set
                {
                    rasterizerState.CullMode = XnaTools.ToXnaCullMode(value);

                    Device.RasterizerState = rasterizerState.Clone();
                }
            }

            #endregion

            #region [ IRenderStateSetterOf<BlendOperationState> ]

            BlendOperationState IRenderStateSetterOf<BlendOperationState>.State
            {
                set
                {
                    blendState.ColorSourceBlend = XnaTools.ToXnaBlend(value.Source);
                    blendState.ColorDestinationBlend = XnaTools.ToXnaBlend(value.Destination);

                    Device.BlendState = blendState.Clone();
                }
            }

            #endregion

            #region [ IRenderStateSetterOf<MaterialState> ]

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

            #region [ IRenderStateSetterOf<LightingState> ]

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

            #region [ IRenderStateSetterOf<TextureState> ]

            TextureState IRenderStateSetterOf<TextureState>.State
            {
                set
                {
                }
            }

            #endregion

            #region [ IRenderStateSetterOf<SamplersState> ]

            private int numberOfSamplers;

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

            #region [ IRenderStateSetterOf<WorldState> ]

            WorldState IRenderStateSetterOf<WorldState>.State
            {
                set
                {
                    _World = value.Matrix;
                }
            }

            #endregion

            #region [ IRenderStateSetterOf<ViewState> ]

            ViewState IRenderStateSetterOf<ViewState>.State
            {
                set
                {
                    _View = value.Matrix;
                }
            }

            #endregion

            #region [ IRenderStateSetterOf<ProjectionState> ]

            ProjectionState IRenderStateSetterOf<ProjectionState>.State
            {
                set
                {
                    _Projection = value.Matrix;
                }
            }

            #endregion

            #region [ IRenderStateSetterOf<VertexShaderState> ]

            VertexShaderState IRenderStateSetterOf<VertexShaderState>.State
            {
                set
                {
                    try
                    {
                        GlobalBindings bindings;
                        effectManager.SetShaderFor(ShaderStage.Vertex, value.BuildASTFor(HLSLBasic.HLSL, out bindings), bindings);
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

            #region [ IRenderStateSetterOf<PixelShaderState> ]

            PixelShaderState IRenderStateSetterOf<PixelShaderState>.State
            {
                set
                {
                    try
                    {
                        GlobalBindings bindings;
                        effectManager.SetShaderFor(ShaderStage.Pixel, value.BuildASTFor(HLSLBasic.HLSL, out bindings), bindings);
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

						#region [ IBasicContext Memebers ]

            Matrix4x4 _World;
            public Matrix4x4 World
            {
                get { return _World; }
            }

            Matrix4x4 _View;
            public Matrix4x4 View
            {
                get { return _View; }
            }

            Matrix4x4 _Projection;
            public Matrix4x4 Projection
            {
                get { return _Projection; }
            }

            MaterialContext _Material;
            public MaterialContext Material
            {
                get
                {
                    return _Material;
                }
            }

            Vector3 _Ambient;
            public Vector3 AmbientColor { get { return _Ambient; } }

            int _NumberOfLights;
            public int NumberOfLights { get { return _NumberOfLights; } }

            LightingContext[] _Lights = new LightingContext[3];
            public LightingContext Light0
            {
                get
                {
                    return _Lights[0];
                }
            }

            public LightingContext Light1
            {
                get
                {
                    return _Lights[1];
                }
            }

            public LightingContext Light2
            {
                get
                {
                    return _Lights[2];
                }
            }

            bool _HasSampling;
            public bool HasSampling
            {
                get { return _HasSampling; }
            }

            Sampler2D _Sampler;

            public Sampler2D Sampler
            {
                get
                {
                    return _Sampler;
                }
            }

            #endregion

        }
    }
}
