using System;
using System.Collections.Generic;
using System.Text;

namespace System.Rendering.RenderStates
{
    /// <summary>
    /// Defines supported cull mode state values
    /// </summary>
    public enum CullModeState 
    { 
        /// <summary>
        /// None faces are culled
        /// </summary>
        None = 0, 
        /// <summary>
        /// Front faces are culled
        /// </summary>
        Front = 1, 
        /// <summary>
        /// Back faces are culled
        /// </summary>
        Back = 2, 
        /// <summary>
        /// Both faces are culled
        /// </summary>
        Front_and_Back = 3, 
        /// <summary>
        /// None faces are culled
        /// </summary>
        Default = None
    }
}
