using System;
using System.Collections.Generic;
using System.Text;
using System.Rendering.Resourcing;

namespace System.Rendering.Effects
{
    public abstract class BlendableEffect<ES> : Effect<ES> where ES : struct
    {
        public BlendableEffect(ES state)
            : base(state)
        {
            BlendMode = StateBlendMode.Modulate;
        }

        public StateBlendMode BlendMode
        {
            get;
            set;
        }

        protected override Location OnClone(AllocateableBase toFill, IRenderDevice render)
        {
            var location = base.OnClone(toFill, render);
            ((BlendableEffect<ES>)toFill).BlendMode = this.BlendMode;
            return location;
        }

        protected class BlendingTechnique : SingleTechnique
        {
            public BlendingTechnique(IRenderStatesManager manager, BlendableEffect<ES> effect)
                : base(manager, effect)
            {
            }

            protected override IEnumerable<Pass> Passes
            {
                get
                {
                    ES previus = RenderStates.GetState<ES>();
                    RenderStates.SetState<ES>(((BlendableEffect<ES>)effect).Blend(previus));
                    yield return NewPass();
                }
            }
        }

        protected override Technique GetTechnique(IRenderStatesManager manager)
        {
            return new BlendingTechnique(manager, this);
        }

        /// <summary>
        /// When overriden returns the blending between this effect states and the already set,
        /// according to the current BlendMode option.
        /// </summary>
        protected abstract ES Blend(ES previus);
    }

    public enum StateBlendMode
    {
        /// <summary>
        /// Current state doesnt take place in the pipeline.
        /// </summary>
        None,
        /// <summary>
        /// Current state subtract its values to the actual state in the pipeline.
        /// </summary>
        Subtract,
        /// <summary>
        /// Current state is set subtracting actual state in the pipeline.
        /// </summary>
        RevSubtract,
        /// <summary>
        /// Current state multiplies with actual state in the pipeline.
        /// Several cases this behaviour is similar to multiply.
        /// </summary>
        Multiply,
        /// <summary>
        /// Current state modulates its components with actual state in the pipeline.
        /// </summary>
        Modulate,
        /// <summary>
        /// Current state modulates its components and duplicates with actual state in the pipeline.
        /// </summary>
        ModulateX2,
        /// <summary>
        /// Current state modulates its components and multiply by 4 with actual state in the pipeline.
        /// </summary>
        ModulateX4,
        /// <summary>
        /// Current states adds its behaviour to the existing one.
        /// </summary>
        Add,
        /// <summary>
        /// Current states replaces the previous state in the pipeline.
        /// </summary>
        Replace
    }

    public abstract class AppendableEffect<ES> : Effect<ES> where ES : struct
    {
        protected AppendableEffect(ES state)
            : base(state)
        {
        }

        public AppendMode AppendMode { get; set; }

        protected abstract ES Append(ES previus);

        protected override Location OnClone(AllocateableBase toFill, IRenderDevice render)
        {
            var location = base.OnClone(toFill, render);
            ((AppendableEffect<ES>)toFill).AppendMode = this.AppendMode;
            return location;
        }

        protected override Technique GetTechnique(IRenderStatesManager manager)
        {
            return new AppendableTechnique(manager, this);
        }

        protected class AppendableTechnique : SingleTechnique
        {
            public AppendableTechnique(IRenderStatesManager manager, AppendableEffect<ES> effect)
                : base(manager, effect)
            {
            }

            protected override IEnumerable<Pass> Passes
            {
                get
                {
                    ES previus = RenderStates.GetState<ES>();
                    RenderStates.SetState<ES>(((AppendableEffect<ES>)effect).Append(previus));
                    yield return NewPass();
                }
            }
        }
    }

    public enum AppendMode
    {
        None,
        Prepend,
        Append,
        Replace
    }
}
