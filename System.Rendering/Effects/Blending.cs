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
using System.Rendering.RenderStates;
using System.Rendering.Resourcing;
using System.Maths;

namespace System.Rendering.Effects
{
    public static class AlphaBlending
    {
        public static IEffect Light
        {
            get
            {
                return new Effect<BlendOperationState>(BlendOperationState.Light);
            }
        }

        public static IEffect BlendAlpha
        {
            get
            {
                return new Effect<BlendOperationState>(BlendOperationState.BlendSourceAlpha);
            }
        }
        
        public static Transparency Opacity(float opacity)
        {
            return new Transparency(opacity);
        }        
    }

    public class Transparency : Effect
    {
        public Transparency():this (1){
        }

        public Transparency(float opacity) : base ()
        {
            this.opacity = opacity;
        }

        float opacity = 1f;

        public float Opacity
        {
            get { return opacity; }
            set { opacity = value; }
        }

        protected override Technique GetTechnique(IRenderStatesManager manager)
        {
            return new TransparencyTechnique(manager, this);
        }

        protected class TransparencyTechnique : Technique
        {
            Transparency transparency;

            public TransparencyTechnique(IRenderStatesManager manager, Transparency transparency):base(manager)
            {
                this.transparency = transparency;
            }

            protected override void SaveStates()
            {
                RenderStates.Save<BlendOperationState>();
                RenderStates.Save<MaterialState>();
            }

            protected override IEnumerable<Pass> Passes
            {
                get
                {
                    RenderStates.SetState<BlendOperationState>(BlendOperationState.BlendSourceAlpha);
                    RenderStates.SetState<MaterialState>(
                        new MaterialState(
                            new Vector3(1, 1, 1),
                            new Vector3(1, 1, 1),
                            new Vector3(1, 1, 1), 1, new Vector3(1, 1, 1), this.transparency.opacity)
                            .Blend(RenderStates.GetState<MaterialState>(), StateBlendMode.Multiply)
                            );

                    yield return NewPass();
                }
            }

            protected override void RestoreStates()
            {
                RenderStates.Restore<MaterialState>();
                RenderStates.Restore<BlendOperationState>();
            }
        }

        protected override Location OnClone(AllocateableBase toFill, IRenderDevice render)
        {
            Transparency filling = toFill as Transparency;
            filling.opacity = this.opacity;
            return Rendering.Location.Render;
        }

        protected override void OnDispose()
        {
        }
    }
}
