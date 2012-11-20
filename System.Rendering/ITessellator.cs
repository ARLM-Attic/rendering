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
using System.Runtime.InteropServices;
using System.Maths;

namespace System.Rendering
{
    /// <summary>
    /// Defines functionallities of tessellator component of a render.
    /// </summary>
    public interface ITessellator : ITessellatorInfo
    {
        /// <summary>
        /// Gets the render object this tessellator is acting over.
        /// </summary>
        IRenderDevice Render { get; }

        /// <summary>
        /// Draws a generic graphic primitive
        /// </summary>
        /// <typeparam name="GP">Graphic Primitive type</typeparam>
        void Draw<GP>(GP primitive) 
            where GP : struct, IGraphicPrimitive;
    }

    public interface ITessellatorInfo
    {
        /// <summary>
        /// Retrieves if a specified GP can be drawn with this tessellator
        /// </summary>
        /// <typeparam name="GP">Graphic primitive type</typeparam>
        /// <returns>True if tessellator can draw the primitive, false otherwise.</returns>
        bool IsSupported<GP>() where GP : struct, IGraphicPrimitive;
    }

    public static class TessellatorExtensors
    {
        /// <summary>
        /// Allows to model by composition of child models.
        /// </summary>
        /// <param name="tessellator"></param>
        /// <param name="model"></param>
        public static void Draw(this ITessellator tessellator, IModel model)
        {
            model.Tesselate(tessellator);
        }
    }

    /// <summary>
    /// Defines a struct type that can be used as graphic primitive by a render
    /// </summary>
    public interface IGraphicPrimitive
    {
    }

    /// <summary>
    /// Defines a graphic primitive that can be intersected.
    /// </summary>
    public interface IIntersectableGraphicPrimitive : IGraphicPrimitive
    {
        /// <summary>
        /// When implemented, returns a IEnumerable object with all intersections for certain ray.
        /// </summary>
        /// <param name="ray">Ray to hit test.</param>
        /// <returns>A IEnumerable object with intersections sorted by distance.</returns>
        IntersectInfo[] Intersect(Ray ray);
    }

    /// <summary>
    /// Defines a vertexed graphic primitive that can be intersected.
    /// </summary>
    public interface IIntersectableVertexedGraphicPrimitive : IIntersectableGraphicPrimitive, IVertexedGraphicPrimitive
    {
        /// <summary>
        /// When implemented, allows to check if some ray intersects the primitive after transformed.
        /// </summary>
        /// <typeparam name="FVF">Parameter Type to identify vertex type.</typeparam>
        /// <param name="ray">Ray to hit test.</param>
        /// <param name="transformed">Transformed vertex buffer for the primitive.</param>
        /// <returns>A IEnumerable object with intersections sorted by distance.</returns>
        IntersectInfo[] Intersect(Ray ray, VertexBuffer transformed);
    }

    /// <summary>
    /// Defines graphics builtins based on vertexes.
    /// </summary>
    public interface IVertexedGraphicPrimitive : IGraphicPrimitive
    {
        /// <summary>
        /// Allows to retrieve a formatted vertex buffer for this primitive.
        /// </summary>
        VertexBuffer VertexBuffer { get; }

        IVertexedGraphicPrimitive Transform(Matrix4x4 transform);

        IVertexedGraphicPrimitive Transform<FVFIn, FVFOut>(Func<FVFIn, FVFOut> transform)
            where FVFIn : struct
            where FVFOut : struct;
    }

    /// <summary>
    /// Defines functionallity of some ITessellator object for drawing a specific type of graphic primitive
    /// </summary>
    /// <typeparam name="GP">Graphic primitive type</typeparam>
    public interface ITessellatorOf<GP> where GP: IGraphicPrimitive
    {
        /// <summary>
        /// Draws a simple graphic primitive
        /// </summary>
        /// <param name="primitive">Graphic primitive</param>
        void Draw(GP primitive);
    }
}

