using System;
using System.Collections.Generic;
using System.Text;
using System.Rendering.RenderStates;

namespace System.Rendering.Effects
{
    public class Culling : Effect<CullModeState>
    {
        protected Culling() : base() { }

        private Culling(CullModeState state)
            : base(state)
        {
        }

        public static readonly Culling None = new Culling(CullModeState.None);
        public static readonly Culling Front = new Culling(CullModeState.Front);
        public static readonly Culling Back = new Culling(CullModeState.Back);
        public static readonly Culling Front_and_Back = new Culling(CullModeState.Front_and_Back);
    }
}
