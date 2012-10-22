using System;
using System.Collections.Generic;
using System.Text;

namespace System.Rendering.RenderStates
{
    /// <summary>
    /// Defines the supported blend factors.
    /// </summary>
    public enum Blend
    {
        Zero,
        One,
        SourceColor,
        InvSourceColor,
        SourceAlpha,
        InvSourceAlpha,
        DestinationAlpha,
        InvDestinationAlpha,
        DestinationColor,
        InvDestinationColor,
        SourceAlphaSat
    }

    /// <summary>
    /// Represents a per-pixel blend operation state of a standar raster pipeline.
    /// </summary>
    public struct BlendOperationState
    {
        public static readonly BlendOperationState Default = new BlendOperationState(Blend.One, Blend.Zero, false);

        public readonly bool Enable;
        public readonly Blend Source;
        public readonly Blend Destination;
        public BlendOperationState(Blend source, Blend destination, bool enable)
        {
            this.Source = source;
            this.Destination = destination;
            this.Enable = enable;
        }

        public static readonly BlendOperationState Disable = new BlendOperationState();
        public static readonly BlendOperationState Normal = new BlendOperationState(Blend.One, Blend.Zero, true);
        public static readonly BlendOperationState Light = new BlendOperationState(Blend.One, Blend.One, true);
        public static readonly BlendOperationState BlendSourceAlpha = new BlendOperationState(Blend.SourceAlpha, Blend.InvSourceAlpha, true);
        public static readonly BlendOperationState Multiply = new BlendOperationState(Blend.DestinationColor, Blend.Zero, true);
        public static readonly BlendOperationState Dark = new BlendOperationState(Blend.One, Blend.SourceColor, true);

    }

    
}
