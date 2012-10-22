using System;
using System.Collections.Generic;
using System.Text;

namespace System.Rendering
{
    /// <summary>
    /// Defines functionallities for graphics that can be obtain using a render device.
    /// </summary>
    public interface IGraphic : System.Rendering.Resourcing.IAllocateable, IDrawable
    {
        /// <summary>
        /// Obtains the graphic in a specific render device.
        /// </summary>
        /// <param name="render">A IRender object used for access to render device functionallities</param>
        void ViewAt(IRenderDevice render);
    }
}
