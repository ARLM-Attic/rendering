using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Rendering.Resourcing
{
    /// <summary>
    /// Defines functionallities for a graphic element that can be allocated in a render.
    /// </summary>
    public interface IAllocateable : IDisposable
    {
        /// <summary>
        /// Gets the render object storing this allocateable object.
        /// </summary>
        IRenderDevice Render { get; }

        /// <summary>
        /// Gets the Location type of this element.
        /// </summary>
        Location Location { get; }

        /// <summary>
        /// Allows this object to clone and save on a different render.
        /// </summary>
        /// <param name="render">Render object where the clone will be saved. 
        /// If this parameter is null, the clone will be saved on user memory.
        /// </param>
        /// <returns>A clone of this object located in specific render or in user memory.</returns>
        IAllocateable Clone(IRenderDevice render);
    }
}
