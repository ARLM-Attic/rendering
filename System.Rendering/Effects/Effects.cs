using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Rendering.Resourcing;

namespace System.Rendering
{
    /// <summary>
    /// Base class for programming effects.
    /// </summary>
    [TypeConverter(typeof(ComponentConverter))]
    public abstract class Effect : AllocateableBase, IEffect
    {
        public const Effect None = null;

        protected Effect() { }

        /// <summary>
        /// When overriden, gets the techinque this effect will apply over render states manager.
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        protected abstract Technique GetTechnique(IRenderStatesManager manager);

        Technique IEffect.GetTechnique(IRenderStatesManager renderStatesManager)
        {
            var technique = GetTechnique(renderStatesManager);

            technique.Initialize();

            return technique;
        }

        bool IEffect.IsSupported(IRenderDevice render)
        {
            return IsSupported(render);
        }

        /// <summary>
        /// When overriden indicates the effect is supported in render state and resource manager.
        /// </summary>
        /// <param name="RenderState">IRenderState to check compatibility</param>
        /// <param name="Resources">IResourceManager to check compatibility</param>
        /// <returns>True if effect can be applied, false otherwise</returns>
        protected virtual bool IsSupported(IRenderDevice render)
        {
            return true;
        }

        /// <summary>
        /// Concats several effects in a single effect that will apply the first and the second.
        /// </summary>
        public static EffectCollection Concat(params IEffect[] effects)
        {
            return new EffectCollection(effects);
        }

        public static EffectCollection operator +(Effect e1, Effect e2)
        {
            return new EffectCollection(e1, e2);
        }
    }

    /// <summary>
    /// Class used for concat effects in just one. The effects are applied in same order of the collection.
    /// </summary>
    [TypeConverter(typeof (CollectionConverter))]
    public class EffectCollection : Effect, IList<IEffect>
    {
        List<IEffect> effects = new List<IEffect>();

        /// <summary>
        /// Disposes all effects in the collection.
        /// </summary>
        protected override void OnDispose()
        {
            base.Dispose();

            foreach (IEffect e in this)
                e.Dispose();
        }

        class TechniqueCollection: Technique
        {
            List<IEffect> effects;

            public TechniqueCollection(IRenderStatesManager manager, List<IEffect> effects) :
                base(manager)
            {
                this.effects = effects;
            }

            IEnumerable<Pass> GetPasses(int level)
            {
                if (level == effects.Count)
                    yield return NewPass();
                else
                {
                    /// saves the states for the current technique
                    var currentTechnique = effects[level].GetTechnique(RenderStates);
                    foreach (Pass p in currentTechnique)
                        foreach (Pass pass in GetPasses(level + 1))
                            yield return pass;
                    /// restores the states of the current technique.
                    currentTechnique.Dispose();
                }
            }

            protected override IEnumerable<Pass> Passes
            {
                get { return GetPasses (0); }
            }

            protected override void SaveStates()
            {
            }

            protected override void RestoreStates()
            {
            }
        }

        protected EffectCollection()
            : base()
        {
        }

        public EffectCollection(params IEffect[] effects)
            : this((IEnumerable<IEffect>)effects)
        {
        }

        public EffectCollection(IEnumerable<IEffect> effects) :
            this()
        {
            this.effects = new List<IEffect>(effects);
        }

        protected override Technique GetTechnique(IRenderStatesManager manager)
        {
            return new TechniqueCollection(manager, this.effects);
        }

        protected override Location OnClone(AllocateableBase toFill, IRenderDevice render)
        {
            EffectCollection filling = toFill as EffectCollection;
            filling.effects = this.effects.Select(e => (IEffect)e.Clone(render)).ToList();

            return (filling.effects.All(e => e.Location == Rendering.Location.Device) ? Location.Device :
                filling.effects.Any(e => e.Location == Rendering.Location.User) ? Location.User :
                Rendering.Location.Render);
        }

        #region IList<Effect> Members

        public int IndexOf(IEffect item)
        {
            return effects.IndexOf(item);
        }

        public void Insert(int index, IEffect item)
        {
            effects.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            effects.RemoveAt(index);
        }

        public IEffect this[int index]
        {
            get
            {
                return effects[index];
            }
            set
            {
                effects[index] = value;
            }
        }

        #endregion

        #region ICollection<IEffect> Members

        public void Add(IEffect item)
        {
            effects.Add(item);
        }

        public void Clear()
        {
            effects.Clear();
        }

        public bool Contains(IEffect item)
        {
            return effects.Contains(item);
        }

        public void CopyTo(IEffect[] array, int arrayIndex)
        {
            effects.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return effects.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(IEffect item)
        {
            return effects.Remove(item);
        }

        #endregion

        #region IEnumerable<Effect> Members

        public IEnumerator<IEffect> GetEnumerator()
        {
            return effects.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// Effect that only modifies one state in render.
    /// </summary>
    /// <typeparam name="ES">Type of RenderState it can modify.</typeparam>
    [TypeConverter(typeof(ComponentConverter))]
    public class Effect<RS> : Effect where RS : struct
    {
        protected RS state;

        public RS State { get { return state; } }

        /// <summary>
        /// Initialize a new instance of Effect using a state.
        /// </summary>
        /// <param name="state">State it will set in the render</param>
        public Effect(RS state) : base ()
        {
            this.state = state;
        }

        protected Effect() : base() { }

        /// <summary>
        /// Creates an instance of the effect using a state.
        /// </summary>
        /// <param name="state">State it will set in the render</param>
        /// <returns>Effect structure created</returns>
        public static Effect<RS> From(RS state)
        {
            return new Effect<RS>(state);
        }

        public static implicit operator Effect<RS>(RS state)
        {
            return new Effect<RS>(state);
        }

        protected class SingleTechnique : Technique
        {
            protected Effect<RS> effect;

            private bool needsDispose;

            public SingleTechnique(IRenderStatesManager manager, Effect<RS> effect)
                : base(manager)
            {
                needsDispose = ((IAllocateable)effect).Render != manager.Render;

                if (needsDispose)
                    this.effect = effect.Allocate(manager.Render);
                else
                    this.effect = effect;
            }

            protected override void SaveStates()
            {
                RenderStates.Save<RS>();
            }

            protected override IEnumerable<Pass> Passes
            {
                get
                {
                    RenderStates.SetState(effect.state);
                    yield return NewPass();
                }
            }

            protected override void RestoreStates()
            {
                RenderStates.Restore<RS>();
            }

            public override void Dispose()
            {
                base.Dispose();

                if (needsDispose)
                    this.effect.Dispose();
            }
        }

        protected override Technique GetTechnique(IRenderStatesManager manager)
        {
            return new SingleTechnique(manager, this);
        }

        protected override bool IsSupported(IRenderDevice render)
        {
            return render.RenderStateInfo.IsSupported<RS>();
        }

        protected override Location OnClone(AllocateableBase toFill, IRenderDevice render)
        {
            Effect<RS> filling = toFill as Effect<RS>;
            filling.state = state is IAllocateable ? (RS)((IAllocateable)state).Clone(render) : state;

            return (filling.state is IAllocateable) ? ((IAllocateable)filling.state).Location :
                    Location.Render;
        }

        protected override void OnDispose()
        {
            if (state is IAllocateable)
                ((IAllocateable)state).Dispose();
        }
    }
}
