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

            public bool Restoring
            {
                get;
                private set;
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
                Restoring = true;
                if (this is IRenderStateManagerOf<RS>)
                    ((IRenderStateManagerOf<RS>)this).Restore();
                else
                {
                    stacking.Pop<RS>();
                    SetState<RS>(stacking.GetCurrent<RS>());
                }
                Restoring = false;
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
    }
}
