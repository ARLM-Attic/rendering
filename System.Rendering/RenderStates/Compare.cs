using System;
using System.Collections.Generic;
using System.Text;

namespace System.Rendering.RenderStates
{
    /// <summary>
    /// Defines the supported comparison functions for detph, stencil and alpha tests of a standar raster pipeline.
    /// </summary>
    public enum Compare
    {
        /// <summary>
        /// The test is always passed
        /// </summary>
        Always,
        /// <summary>
        /// The test is never passed
        /// </summary>
        Never,
        /// <summary>
        /// The test is passed only if the value is greater or equal than current value
        /// </summary>
        GreaterEqual,
        /// <summary>
        /// The test is passed if value is different to the current value
        /// </summary>
        NotEqual,
        /// <summary>
        /// The test is passed if value is greater than current value
        /// </summary>
        Greater,
        /// <summary>
        /// The test is passed if value is less or equal to the current value
        /// </summary>
        LessEqual,
        /// <summary>
        /// The test is passed if value is equal to the current value
        /// </summary>
        Equal,
        /// <summary>
        /// The test is passed if value is less than the current value
        /// </summary>
        Less
    }

}
