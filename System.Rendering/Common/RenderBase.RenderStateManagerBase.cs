using System;
using System.Collections.Generic;
using System.Text;
using System.Rendering.RenderStates;
using System.Rendering.Effects.Shaders;
using System.Compilers.Shaders.Info;

namespace System.Rendering
{
    public abstract partial class RenderBase
    {
        public abstract class RenderStatesManagerBase : IRenderStatesManager
        {
            protected RenderBase render;

            Stacking stacking = new Stacking ();

            public RenderStatesManagerBase (RenderBase render)
            {
                this.render = render;
            }

            public IRenderDevice Render
            {
                get
                {
                    return render;
                }
            }

            /// <summary>
            /// Gets the render state manager of render associated to this Render State Manager
            /// </summary>
            protected IRenderStatesManager RenderState
            {
                get { return render.RenderStates; }
            }

            /// <summary>
            /// Gets the resource manager of render associated to this Render State Manager
            /// </summary>
            protected IResourcesManager Resources
            {
                get { return render.Resources; }
            }

            /// <summary>
            /// Gets the tessellator of render associated to this Render State Manager
            /// </summary>
            protected ITessellator Modelling
            {
                get { return render.Tessellator; }
            }

            #region IRenderState Members

            public virtual RS GetState<RS>() where RS : struct
            {
                if (this is IRenderStateManagerOf<RS>)
                    return ((IRenderStateManagerOf<RS>)this).State;
                if (stacking != null)
                    return stacking.GetCurrent<RS>();
                return default(RS);
            }

            public virtual void SetState<RS>(RS state) where RS : struct
            {
                if (this is IRenderStateManagerOf<RS>)
                    ((IRenderStateManagerOf<RS>)this).State = state;
                else
                {
                    if (this is IRenderStateSetterOf<RS>)
                        ((IRenderStateSetterOf<RS>)this).State = state;
                    if (stacking != null)
                        stacking.SetCurrent<RS>(state);
                }
            }

            #endregion

            #region IRenderState Members

            public virtual void Save<RS>() where RS : struct
            {
                if (this is IRenderStateManagerOf<RS>)
                    ((IRenderStateManagerOf<RS>)this).Save();
                else
                {
                    stacking.Push<RS>();
                }
            }

            public virtual void Restore<RS>() where RS : struct
            {
                if (this is IRenderStateManagerOf<RS>)
                    ((IRenderStateManagerOf<RS>)this).Restore();
                else
                {
                    stacking.Pop<RS>();
                    SetState<RS>(stacking.GetCurrent<RS>());
                }
            }

            #endregion

            #region IRenderStateInfo Members

            public bool IsSupported<RS>() where RS : struct
            {
                return this is IRenderStateManagerOf<RS> || stacking.IsCreated<RS>();
            }

            #endregion

            protected void Create<RS>(RS _default) where RS : struct
            {
                stacking.Create<RS>(_default);
                SetState<RS>(_default);
            }

            public virtual void Dispose()
            {
                stacking.Dispose();
            }
        }

        //public abstract class ShaderBasedRenderStatesManagerBase : RenderStatesManagerBase
        //    , IRenderStateSetterOf<ShadeModeState>
        //{
        //    private Pipeline __Pipeline;

        //    protected Pipeline Pipeline
        //    {
        //        get { return __Pipeline; }
        //        set
        //        {
        //            if (__Pipeline != null)
        //                __Pipeline.FieldUpdated -= new Action<string>(__Pipeline_FieldUpdated);
        //            __Pipeline = value;
        //            if (__Pipeline != null)
        //                __Pipeline.FieldUpdated += new Action<string>(__Pipeline_FieldUpdated);
        //            OnChangePipeline();
        //        }
        //    }

        //    void  __Pipeline_FieldUpdated(string fieldName)
        //    {
        //        UpdateValueFor (fieldName);
        //    }

        //    protected ShaderSource VertexShader
        //    {
        //        get { return __Pipeline.VertexShaderSource; }
        //    }

        //    protected ShaderSource PixelShader
        //    {
        //        get { return __Pipeline.PixelShaderSource; }
        //    }

        //    protected virtual void OnChangePipeline()
        //    {
                
        //    }

        //    protected abstract void UpdateValueFor(string fieldName);

        //    public ShaderBasedRenderStatesManagerBase(RenderBase render)
        //        : base(render)
        //    {
        //    }

        //    public void InitializePipeline()
        //    {
        //        Create<ShadeModeState>(ShadeModeState.Default);
        //    }

        //    public override void SetState<RS>(RS state)
        //    {
        //        base.SetState<RS>(state);

        //        if (__Pipeline is IRenderStateSetterOf<RS>)
        //        {
        //            ((IRenderStateSetterOf<RS>)__Pipeline).State = state;
        //        }                
        //    }

        //    ShadeModeState IRenderStateSetterOf<ShadeModeState>.State
        //    {
        //        set
        //        {
        //            this.Pipeline = value.Pipeline;
        //        }
        //    }
        //}
    }
}
