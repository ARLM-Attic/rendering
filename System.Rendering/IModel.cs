using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace System.Rendering
{
    /// <summary>
    /// Defines functionallities for a Model Representation in a render device.
    /// </summary>
    public interface IModel : System.Rendering.Resourcing.IAllocateable, IDrawable
    {
        /// <summary>
        /// When implemented indicates if this model will be suported by the render.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        bool IsSupported(IRenderDevice device);

        /// <summary>
        /// Converts the model to basic drawn builtins in a render device.
        /// </summary>
        /// <param name="tessellator">Tessellator interface that access to modelling functionallities of render device.</param>
        void Tesselate(ITessellator tessellator);
    }
}


