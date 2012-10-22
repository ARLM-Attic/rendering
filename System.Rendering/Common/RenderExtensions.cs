using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Rendering
{
    /// <summary>
    /// Class used to extend IRender interface basic functionallities.
    /// </summary>
    public static class RenderExtensions
    {
        /// <summary>
        /// Draws a logic provided by an Action object affected by a secuence of effects.
        /// </summary>
        /// <param name="render">Render object used to draw.</param>
        /// <param name="rendering">Delegate object with the logic (uses of render) for drawing.</param>
        /// <param name="effects">A secuence of effects. From more local effects, to global ones.</param>
        public static void Draw(this IRenderDevice render, Action rendering, params IEffect[] effects)
        {
            _Draw(render, rendering, effects, effects.Length - 1);
        }

        struct SinglePrimitiveAsModel<GP> : IModel where GP:struct, IGraphicPrimitive
        {
            internal GP primitive;

            public void Tesselate(ITessellator tessellator)
            {
                tessellator.Draw<GP>(primitive);
            }

            public IRenderDevice Render
            {
                get { return null; }
            }

            public Location Location
            {
                get { return Rendering.Location.User; }
            }

            public Resourcing.IAllocateable Clone(IRenderDevice render)
            {
                throw new NotSupportedException();
            }

            public bool IsSupported(IRenderDevice render)
            {
                return render.TessellatorInfo.IsSupported<GP>();
            }

            public void Dispose()
            {
            }
        }

        public static void Draw<GP>(this IRenderDevice render, GP primitive) where GP : struct, IGraphicPrimitive
        {
            render.Draw(primitive.AsModel());
        }

        public static IModel AsModel<GP>(this GP graphicPrimitive) where GP : struct, IGraphicPrimitive
        {
            return new SinglePrimitiveAsModel<GP>() { primitive = graphicPrimitive };
        }

        /// <summary>
        /// Draws a logic provided by an Action object affected by a secuence of effects.
        /// </summary>
        /// <param name="render">Render object used to draw.</param>
        /// <param name="rendering">Delegate object with the logic (uses of render) for drawing.</param>
        /// <param name="effects">A secuence of effects. From more local effects, to global ones.</param>
        public static void Draw(this IRenderDevice render, Action<IRenderDevice> rendering, params IEffect[] effects)
        {
            Draw(render, () => rendering(render), effects);
        }

        /// <summary>
        /// Draws a Model affected by a secuence of effects.
        /// </summary>
        /// <param name="render">Render object used to draw.</param>
        /// <param name="model">Model to be drawn.</param>
        /// <param name="effects">A secuence of effects. From more local effects, to global ones.</param>
        public static void Draw(this IRenderDevice render, IDrawable drawable, params IEffect[] effects)
        {
            Draw(render, () => { render.Draw(drawable); }, effects);
        }

        /// <summary>
        /// Draws a Model affected by a secuence of effects.
        /// </summary>
        /// <param name="render">Render object used to draw.</param>
        /// <param name="model">Model to be drawn.</param>
        /// <param name="effects">A secuence of effects. From more local effects, to global ones.</param>
        public static void Draw(this IRenderDevice render, IModel model, params IEffect[] effects)
        {
            Draw(render, () => { render.Draw (model); }, effects);
        }

        /// <summary>
        /// Draws a Graphic affected by a secuence of effects.
        /// </summary>
        /// <param name="render">Render object used to draw.</param>
        /// <param name="graphic">Graphic to be drawn.</param>
        /// <param name="effects">A secuence of effects. From more local effects, to global ones.</param>
        public static void Draw(this IRenderDevice render, IGraphic graphic, params IEffect[] effects)
        {
            Draw(render, () => { graphic.ViewAt(render); }, effects);
        }

        private static void _Draw(IRenderDevice render, Action rendering, IEffect[] effects, int start)
        {
            if (start >= 0)
            {
                foreach (Pass p in render.Apply(effects[start]))
                    _Draw(render, rendering, effects, start - 1);
                render.EndScope(effects[start]);
            }
            else
                rendering();
        }

        ///// <summary>
        ///// Draws a logic provided by an Action object affected by a vertex shader and a pixel shader effect. 
        ///// </summary>
        ///// <typeparam name="VIn">Declared Vertex Type for Input</typeparam>
        ///// <typeparam name="VOut">Declared Vertex Type for Output</typeparam>
        ///// <typeparam name="PIn">Declared Pixel Type for Input</typeparam>
        ///// <typeparam name="POut">Declared Pixel Type for Output</typeparam>
        ///// <param name="render">Render object used to draw</param>
        ///// <param name="action">Delegate object with the visualization logic.</param>
        ///// <param name="vertexFunction">Vertex Shader Function.</param>
        ///// <param name="pixelFunction">Pixel Shader Function.</param>
        //public static void Draw<VIn, VOut, PIn, POut>(this IRenderer render, Action action,
        //    Func<VIn, VOut> vertexFunction,
        //    Func<PIn, POut> pixelFunction)
        //    where VIn : struct
        //    where VOut : struct
        //    where PIn : struct
        //    where POut : struct
        //{
        //    render.Draw(action, (VertexShaderEffect<VIn, VOut>)vertexFunction, (PixelShaderEffect<PIn, POut>)pixelFunction);
        //}
    }
}
