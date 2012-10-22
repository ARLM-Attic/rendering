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
using System.Text;
using System.Rendering.Effects;
using OpenTK.Graphics.OpenGL;
using System.ComponentModel;
using System.Rendering.Effects.Shaders;
using System.Linq;
using System.Rendering.RenderStates;
using System.Compilers.Shaders;
using System.Maths;
using System.Compilers.Shaders.Info;
using System.Compilers.Shaders.ShaderAST;
using System.Runtime.InteropServices;
using System.Compilers.Shaders.GLSL;
using System.Rendering.OpenTK;

namespace System.Rendering.OpenGL
{
    partial class OpenGLRender
    {
        public class OpenGLRenderStates : RenderStatesManagerBase,
            IRenderStateSetterOf<FrameBufferState>, 
            IRenderStateSetterOf<DepthBufferState>, 
            IRenderStateSetterOf<AlphaTestState>,
            IRenderStateSetterOf<WorldState>, 
            IRenderStateSetterOf<ViewState>, 
            IRenderStateSetterOf<ProjectionState>, 
            IRenderStateSetterOf<LightingState>, 
            IRenderStateSetterOf<FillModeState>, 
            IRenderStateSetterOf<MaterialState>, 
            IRenderStateSetterOf<BlendOperationState>, 
            IRenderStateSetterOf<CullModeState>, 
            IRenderStateSetterOf<TextureState>,
            IRenderStateSetterOf<SamplersState>,
            IRenderStateSetterOf<StencilBufferState>,
            IRenderStateSetterOf<VertexShaderState>,
            IRenderStateSetterOf<PixelShaderState>,
            
            IBasicContext
        {
            public OpenGLRenderStates(OpenGLRender render)
                : base(render)
            {
                render.Created += new EventHandler(render_Created);
            }

            internal OpenGLEffectManager effectManager;

            public override void SetState<RS>(RS state)
            {
                ((OpenGLRender)render).SyncExecute(() =>
                {
                    base.SetState<RS>(state);
                });
            }

            public override void Save<RS>()
            {
                ((OpenGLRender)render).SyncExecute(() =>
                {
                    base.Save<RS>();
                });
            }

            public override void Restore<RS>()
            {
                ((OpenGLRender)render).SyncExecute(() =>
                {
                    base.Restore<RS>();
                });
            }

            void render_Created(object sender, EventArgs e)
            {
                effectManager = new OpenGLEffectManager(render as OpenGLRender);

                this.Create<FrameBufferState>(FrameBufferState.Default);
                this.Create<DepthBufferState>(DepthBufferState.Default);
                this.Create<AlphaTestState>(AlphaTestState.Default);
                this.Create<WorldState>(WorldState.Default);
                this.Create<ViewState>(ViewState.Default);
                this.Create<ProjectionState>(ProjectionState.Default);

                this.Create<LightingState>(LightingState.Default);

                this.Create<FillModeState>(FillModeState.Default);
                this.Create<MaterialState>(MaterialState.Default);
                this.Create<BlendOperationState>(BlendOperationState.Default);
                this.Create<CullModeState>(CullModeState.Default);
                this.Create<TextureState>(TextureState.Default);
                this.Create<SamplersState>(SamplersState.Default);

                this.Create<StencilBufferState>(StencilBufferState.Default);

                NoPipeline initialPipeline = new NoPipeline(this);

                VertexShaderState vss = new VertexShaderState(((IVertexProcessor)initialPipeline).Processor);
                this.Create<VertexShaderState>(vss);

                PixelShaderState pss = new PixelShaderState(((IPixelProcessor)initialPipeline).Processor);
                this.Create<PixelShaderState>(pss);
            }

            FrameBufferState IRenderStateSetterOf<FrameBufferState>.State
            {
                set
                {
                    GL.ClearColor((float)value.DefaultValue.X, (float)value.DefaultValue.Y, (float)value.DefaultValue.Z, (float)value.DefaultValue.W);
                    bool red = (value.Mask & ColorMask.Red) != 0;
                    bool green = (value.Mask & ColorMask.Green) != 0;
                    bool blue = (value.Mask & ColorMask.Blue) != 0;
                    bool alpha = (value.Mask & ColorMask.Alpha) != 0;
                    GL.ColorMask(red, green, blue, alpha);

                    if (value.ClearOnSet)
                        GL.Clear(ClearBufferMask.ColorBufferBit);
                }
            }

            DepthFunction FromCompare(Compare compare)
            {
                switch (compare)
                {
                    case Compare.Always: return DepthFunction.Always;
                    case Compare.Equal: return DepthFunction.Equal;
                    case Compare.Greater: return DepthFunction.Greater;
                    case Compare.GreaterEqual: return DepthFunction.Gequal;
                    case Compare.Less: return DepthFunction.Less;
                    case Compare.LessEqual: return DepthFunction.Lequal;
                    case Compare.Never: return DepthFunction.Never;
                    case Compare.NotEqual: return DepthFunction.Notequal;
                }
                return DepthFunction.Always;
            }

            DepthBufferState IRenderStateSetterOf<DepthBufferState>.State
            {
                set
                {
                    GL.ClearDepth(value.DefaultValue);

                    if (value.EnableTest)
                        GL.Enable(EnableCap.DepthTest);
                    else
                        GL.Disable(EnableCap.DepthTest);

                    GL.DepthFunc(FromCompare(value.Function));

                    if (value.WriteEnable)
                        GL.DepthMask(true);
                    else
                        GL.DepthMask(false);

                    if (value.ClearOnSet)
                        GL.Clear(ClearBufferMask.DepthBufferBit);
                }
            }

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
                    if (((OpenGLRender)render).renderTargets.Peek().FB != 0)
                        _Projection.M11 *= -1;
                }
            }

            #endregion

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

            FillModeState IRenderStateSetterOf<FillModeState>.State
            {
                set
                {
                    switch (value)
                    {
                        case FillModeState.Fill: GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                            break;
                        case FillModeState.Lines: GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                            break;
                        case FillModeState.Points: GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
                            break;
                    }
                }
            }

            float[] ToFloatfv(Vector4 v)
            {
                return new float[] { (float)v.X, (float)v.Y, (float)v.Z, (float)v.W };
            }

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
                        Specular = value.Specular
                    };
                }
            }

            BlendingFactorDest ToDst(Blend blend)
            {
                switch (blend)
                {
                    case Blend.DestinationAlpha: return BlendingFactorDest.DstAlpha;
                    case Blend.DestinationColor: return BlendingFactorDest.DstColor;
                    case Blend.InvDestinationAlpha: return BlendingFactorDest.OneMinusDstAlpha;
                    case Blend.InvDestinationColor: return BlendingFactorDest.OneMinusDstColor;
                    case Blend.InvSourceAlpha: return BlendingFactorDest.OneMinusSrcAlpha;
                    case Blend.InvSourceColor: return BlendingFactorDest.OneMinusSrcColor;
                    case Blend.One: return BlendingFactorDest.One;
                    case Blend.SourceAlpha: return BlendingFactorDest.SrcAlpha;
                    case Blend.SourceColor: return BlendingFactorDest.SrcColor;
                    case Blend.Zero: return BlendingFactorDest.Zero;
                    default: return BlendingFactorDest.One;
                }
            }

            BlendingFactorSrc ToSrc(Blend blend)
            {
                switch (blend)
                {
                    case Blend.DestinationAlpha: return BlendingFactorSrc.DstAlpha;
                    case Blend.DestinationColor: return BlendingFactorSrc.DstColor;
                    case Blend.InvDestinationAlpha: return BlendingFactorSrc.OneMinusDstAlpha;
                    case Blend.InvDestinationColor: return BlendingFactorSrc.OneMinusDstColor;
                    case Blend.InvSourceAlpha: return BlendingFactorSrc.OneMinusSrcAlpha;
                    case Blend.One: return BlendingFactorSrc.One;
                    case Blend.SourceAlpha: return BlendingFactorSrc.SrcAlpha;
                    case Blend.SourceAlphaSat: return BlendingFactorSrc.SrcAlphaSaturate;
                    case Blend.Zero: return BlendingFactorSrc.Zero;
                    default: return BlendingFactorSrc.One;
                }
            }

            BlendOperationState IRenderStateSetterOf<BlendOperationState>.State
            {
                set
                {
                    if (value.Enable)
                        GL.Enable(EnableCap.Blend);
                    else
                        GL.Disable(EnableCap.Blend);

                    GL.BlendFunc(ToSrc(value.Source), ToDst(value.Destination));
                }
            }

            CullModeState IRenderStateSetterOf<CullModeState>.State
            {
                set
                {
                    switch (value)
                    {
                        case CullModeState.None: GL.Disable(EnableCap.CullFace);
                            break;
                        case CullModeState.Front: GL.Enable(EnableCap.CullFace);
                            GL.CullFace(CullFaceMode.Front);
                            break;
                        case CullModeState.Front_and_Back: GL.Enable(EnableCap.CullFace);
                            GL.CullFace(CullFaceMode.FrontAndBack);
                            break;
                        case CullModeState.Back: GL.Enable(EnableCap.CullFace);
                            GL.CullFace(CullFaceMode.Back);
                            break;
                    }
                }
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

            global::OpenTK.Graphics.OpenGL.StencilOp FromStencilOp(System.Rendering.RenderStates.StencilOp op)
            {
                switch (op)
                {
                    case System.Rendering.RenderStates.StencilOp.Decrement: return global::OpenTK.Graphics.OpenGL.StencilOp.Decr;
                    case System.Rendering.RenderStates.StencilOp.Increment: return global::OpenTK.Graphics.OpenGL.StencilOp.Incr;
                    case System.Rendering.RenderStates.StencilOp.Invert: return global::OpenTK.Graphics.OpenGL.StencilOp.Invert;
                    case System.Rendering.RenderStates.StencilOp.Keep: return global::OpenTK.Graphics.OpenGL.StencilOp.Keep;
                    case System.Rendering.RenderStates.StencilOp.Replace: return global::OpenTK.Graphics.OpenGL.StencilOp.Replace;
                    case System.Rendering.RenderStates.StencilOp.Zero: return global::OpenTK.Graphics.OpenGL.StencilOp.Zero;
                    default: return global::OpenTK.Graphics.OpenGL.StencilOp.Keep;
                }
            }

            StencilBufferState IRenderStateSetterOf<StencilBufferState>.State
            {
                set
                {
                    if (value.TestEnable)
                        GL.Enable(EnableCap.StencilTest);
                    else
                        GL.Disable(EnableCap.StencilTest);

                    GL.StencilMask(255);//(int)value.WriteMask);
                    GL.StencilFunc((StencilFunction)this.FromCompare(value.Function), value.Reference, value.CompareMask);
                    GL.StencilOp(
                        FromStencilOp(value.StencilFails),
                        FromStencilOp(value.DepthFails),
                        FromStencilOp(value.Pass));

                    if (value.ClearOnSet)
                    {
                        GL.ClearStencil(0);
                        GL.Clear(ClearBufferMask.StencilBufferBit);
                    }
                }
            }

            VertexShaderState IRenderStateSetterOf<VertexShaderState>.State
            {
                set
                {
                    try
                    {
                        GlobalBindings bindings;

                        var program = value.BuildASTFor(GLSLBasic.GLSL, out bindings);

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

            PixelShaderState IRenderStateSetterOf<PixelShaderState>.State
            {
                set
                {
                    try
                    {
                        GlobalBindings bindings;

                        var program = value.BuildASTFor(GLSLBasic.GLSL, out bindings);

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



            AlphaTestState IRenderStateSetterOf<AlphaTestState>.State
            {
                set
                {
                    GL.Enable(EnableCap.AlphaTest);
                    GL.AlphaFunc((AlphaFunction)FromCompare(value.Compare), value.Reference);
                }
            }

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
            int _NumberOfLights = 0;

            int IBasicContext.NumberOfLights
            {
                get { return _NumberOfLights; }
            }

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

            bool _HasSampling;
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
