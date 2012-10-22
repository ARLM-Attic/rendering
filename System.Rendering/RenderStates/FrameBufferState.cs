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
using System.Maths;

namespace System.Rendering.RenderStates
{
    /// <summary>
    /// Represents a state to manage frame buffer in standar raster pipeline.
    /// </summary>
    public struct FrameBufferState
    {
        /// <summary>
        /// Storages a default value for frame buffer operation with default color black.
        /// </summary>
        public static readonly FrameBufferState Default = new FrameBufferState(new Vector4 (0,0,0,1), ColorMask.RGBA).DoNotClear();
        /// <summary>
        /// Storages a enable value for frame buffer operation that clears the buffer with transparent color.
        /// </summary>
        public static readonly FrameBufferState Enable = new FrameBufferState(Vectors.Zero, ColorMask.RGBA);
        /// <summary>
        /// Storages a disable value for frame buffer operation that set a none mask for colors.
        /// </summary>
        public static readonly FrameBufferState Disable = new FrameBufferState(Vectors.Zero, ColorMask.None).DoNotClear();

        /// <summary>
        /// Gets the default value used when clearing the buffer.
        /// </summary>
        public readonly Vector4 DefaultValue;
        /// <summary>
        /// Indicates the mask of color components used when a pixel is updated.
        /// </summary>
        public readonly ColorMask Mask;

        bool _ClearOnSet;

        /// <summary>
        /// Gets when the buffer with be cleared when the state is set.
        /// </summary>
        public bool ClearOnSet
        {
            get { return _ClearOnSet; }
        }

        /// <summary>
        /// Creates a copy of current structure that doesnt clear when set.
        /// </summary>
        /// <returns>A FrameBufferState structure with clear option changed.</returns>
        public FrameBufferState DoNotClear()
        {
            FrameBufferState fbs = new FrameBufferState(DefaultValue, Mask);
            fbs._ClearOnSet = false;
            return fbs;
        }

        /// <summary>
        /// Initialize the structure with the default color and the color mask.
        /// This structure will clear the buffer when set.
        /// </summary>
        /// <param name="backcolor">A Vector4 structure representing the default color used for clearing</param>
        /// <param name="mask">Mask for color components</param>
        public FrameBufferState(Vector4 backcolor, ColorMask mask)
        {
            this._ClearOnSet = true;
            this.DefaultValue = backcolor;
            this.Mask = mask;
        }

        /// <summary>
        /// Creates a copy of current structure changing the default value used when clearing.
        /// </summary>
        /// <param name="backcolor">A Vector4 structure representing the default color</param>
        /// <returns>A FrameBufferState structure with default color changed</returns>
        public FrameBufferState ChangeDefaultValue(Vector4 backcolor)
        {
            return new FrameBufferState (backcolor, Mask);
        }

        /// <summary>
        /// Creates a copy of current structure changing the mask used while a pixel is updated.
        /// </summary>
        /// <param name="mask">Color mask used</param>
        /// <returns>A FrameBufferState structure with mask changed</returns>
        public FrameBufferState ChangeMask(ColorMask mask)
        {
            return new FrameBufferState(DefaultValue, mask);
        }
    }
}
