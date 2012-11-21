using System;
using System.Collections.Generic;
using System.Text;
using System.Rendering.RenderStates;
using System.Rendering.Effects;

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

namespace System.Rendering
{
    public static class RasterOptions
    {
        public static Culling NoCull { get { return Culling.None; } }

        public static Culling CullBack { get { return Culling.Back; } }

        public static Culling CullFront { get { return Culling.Front; } }

        public static Culling CullBothSides { get { return Culling.Front_and_Back; } }

        public static Filling ViewSolid { get { return Filling.Fill; } }

        public static Filling ViewWireframe { get { return Filling.Lines; } }

        public static Filling ViewPoints { get { return Filling.Points; } }

        public static Effect RenderBackToFront { get { return new TwoSideRenderingBackToFront(); } }
        
        public static Effect RenderFrontToBack { get { return new TwoSideRenderingBackToFront(); } }
    }

    class TwoSideRenderingBackToFront : Effect
    {
        class _Technique : Technique
        {
            public _Technique(IRenderStatesManager rs) : base(rs) { }

            protected override void SaveStates()
            {
                RenderStates.Save<CullModeState>();
            }

            protected override void RestoreStates()
            {
                RenderStates.Restore<CullModeState>();
            }

            protected override IEnumerable<Pass> Passes
            {
                get
                {
                    RenderStates.SetState(CullModeState.Front);
                    yield return NewPass();

                    RenderStates.SetState(CullModeState.Back);
                    yield return NewPass();
                }
            }
        }

        protected override Technique GetTechnique(IRenderStatesManager manager)
        {
            return new _Technique(manager);
        }

        protected override Location OnClone(Resourcing.AllocateableBase toFill, IRenderDevice render)
        {
            return Location.Device;
        }

        protected override void OnDispose()
        {
        }
    }

    class TwoSideRenderingFrontToBack : Effect
    {
        class _Technique : Technique
        {
            public _Technique(IRenderStatesManager rs) : base(rs) { }

            protected override void SaveStates()
            {
                RenderStates.Save<CullModeState>();
            }

            protected override void RestoreStates()
            {
                RenderStates.Restore<CullModeState>();
            }

            protected override IEnumerable<Pass> Passes
            {
                get
                {
                    RenderStates.SetState(CullModeState.Front);
                    yield return NewPass();

                    RenderStates.SetState(CullModeState.Back);
                    yield return NewPass();
                }
            }
        }

        protected override Technique GetTechnique(IRenderStatesManager manager)
        {
            return new _Technique(manager);
        }

        protected override Location OnClone(Resourcing.AllocateableBase toFill, IRenderDevice render)
        {
            return Location.Device;
        }

        protected override void OnDispose()
        {
        }
    }
}
