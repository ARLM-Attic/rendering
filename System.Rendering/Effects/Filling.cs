using System;
using System.Collections.Generic;
using System.Text;
using System.Rendering.RenderStates;

namespace System.Rendering.Effects
{
    public class Filling : Effect<FillModeState>
    {
        protected Filling() : base() { }

        private Filling(FillModeState state) : base(state) { }

        public static readonly Filling Fill = new Filling(FillModeState.Fill);
        public static readonly Filling Lines = new Filling(FillModeState.Lines);
        public static readonly Filling Points = new Filling(FillModeState.Points);

    }
}
