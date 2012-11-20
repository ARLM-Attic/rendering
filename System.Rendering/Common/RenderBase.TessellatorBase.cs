#define FLOAT

#if DECIMAL
using FLOATINGTYPE = System.Decimal;
#endif
#if DOUBLE
using FLOATINGTYPE = System.Double;
#endif
#if FLOAT
using FLOATINGTYPE = System.Single;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Rendering.Effects;
using System.Maths;
using System.Rendering.RenderStates;

namespace System.Rendering
{
    public abstract partial class RenderBase
    {
        public abstract class TessellatorBase : ITessellator
        {
            protected RenderBase render;

            protected IEnumerable<IRayListener> RayListeners
            {
                get
                {
                    return render.rayListeners;
                }
            }

            protected IList<IRayPicker> RayPickers
            {
                get
                {
                    return render.rayPickerManager;
                }
            }

            public IRenderDevice Render { get { return render; } }

            /// <summary>
            /// Gets the render state manager of render associated to this Tessellator
            /// </summary>
            protected RenderStatesManagerBase RenderState
            {
                get { return render.RenderStates; }
            }

            /// <summary>
            /// Gets the resource manager of render associated to this Tessellator
            /// </summary>
            protected ResourcesManagerBase Resources
            {
                get { return render.Resources; }
            }

            /// <summary>
            /// Gets the tessellator of render associated to this Tessellator
            /// </summary>
            protected TessellatorBase Modelling
            {
                get { return render.Tessellator; }
            }

            public TessellatorBase(RenderBase render)
            {
                this.render = render;
            }

            public virtual void Draw<GP>(GP graphicPrimitive) where GP : struct, IGraphicPrimitive
            {
                ProcessInteraction(graphicPrimitive);

                ((ITessellatorOf<GP>)this).Draw(graphicPrimitive);
            }

            public virtual bool IsSupported<GP>() where GP : struct, IGraphicPrimitive
            {
                return this is ITessellatorOf<GP>;
            }

            protected void NotifyInteraction(IRayPicker rayPicker, IntersectInfo info)
            {
                StackEx<IntersectInfo> stack = render.rayPickerManager[rayPicker].Stack;
                stack.ConvertWhile(info, (i) => i == null || i.Distance > info.Distance);
            }

            #region ITessellator Members

            #region Interaction
            
            void ProcessInteraction(IGraphicPrimitive primitive)
            {
                if (primitive is IIntersectableGraphicPrimitive)
                {
                    IIntersectableGraphicPrimitive intersectableGP = (IIntersectableGraphicPrimitive)primitive;

                    foreach (IRayPicker rayPicker in RayPickers)
                    {
                        var infos = intersectableGP.Intersect(rayPicker.Ray);
                        if (infos.Length > 0)
                            foreach (IRayListener listener in RayListeners)
                                NotifyInteraction(rayPicker, infos[0]);
                    }
                }
            }

            protected virtual void ProcessInteraction(IGraphicPrimitive primitive, VertexBuffer processed)
            {
                if (primitive is IIntersectableVertexedGraphicPrimitive)
                {
                    IIntersectableVertexedGraphicPrimitive intersectableGP = (IIntersectableVertexedGraphicPrimitive)primitive;

                    foreach (IRayPicker rayPicker in RayPickers)
                    {
                        List<IntersectInfo> infos = new List<IntersectInfo> (intersectableGP.Intersect(rayPicker.Ray, processed));
                        infos.Sort();
                        if (infos.Count > 0)
                            foreach (IRayListener listener in RayListeners)
                                NotifyInteraction(rayPicker, infos[0]);
                    }
                }
                else
                    if (primitive is IIntersectableGraphicPrimitive)
                    {
                        IIntersectableGraphicPrimitive intersectableGP = (IIntersectableGraphicPrimitive)primitive;

                        foreach (IRayPicker rayPicker in RayPickers)
                        {
                            var infos = intersectableGP.Intersect(rayPicker.Ray);
                            if (infos.Length > 0)
                                foreach (IRayListener listener in RayListeners)
                                    NotifyInteraction(rayPicker, infos[0]);
                        }
                    }
            }

            #endregion

            #endregion
        }

    }
}
