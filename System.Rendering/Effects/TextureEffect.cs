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
using System.Linq;
using System.ComponentModel;
using System.Rendering.RenderStates;
using System.Maths;

namespace System.Rendering.Effects
{
    public class Texturing : BlendingEffect<TextureState>
    {
        public Texturing(TextureBuffer[] textures, TextureStageState[] stageStates)
            : base(new TextureState(textures, stageStates))
        {
            this.BlendMode = StateBlendMode.Add;
        }

        protected Texturing() : base() { }

        protected override TextureState Blend(TextureState previus)
        {
            return TextureState.Blend(previus, this.state, BlendMode);
        }

        protected override void Begin()
        {
            try
            {
                base.Begin();
            }
            catch { }
        }
    }

    public class SingleTexture : Texturing
    {
        public SingleTexture(TextureBuffer texture)
            : base(new TextureBuffer[] { texture },
                new TextureStageState[] {
                new TextureStageState () 
                { 
                    TextureIndex = 0, 
                    Stage = CreateTextureStageCompatible(texture)
                }
            })
        {
        }

        protected SingleTexture()
            : base()
        {
        }

        private static TextureStage CreateTextureStageCompatible(TextureBuffer texture)
        {
            return new TextureStage(new Sampler());
        }

        public TextureBuffer Resource { get { return this.state.GetTexture(0); } }

        public TextureStage Stage
        {
            get { return this.state[0].Stage; }
            set
            {
                this.state = this.state.ChangeStage(0, value);
            }
        }

        public Matrix4x4<FLOATINGTYPE> Transform
        {
            get { return state[0].Stage.Transform; }
            set
            {
                Stage = Stage.ChangeTransform(value);
            }
        }

        public Sampler Sampler
        {
            get { return state[0].Stage.Sampler; }
            set { Stage = Stage.ChangeSampler(value); }
        }
    }
}
