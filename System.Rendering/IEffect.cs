using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Rendering
{
    /// <summary>
    /// Defines necessary methods for handle an effect over a renderState.
    /// This interface is designed for being used by a render class.
    /// </summary>
    public interface IEffect : System.Rendering.Resourcing.IAllocateable
    {
        /// <summary>
        /// When implemented indicates if effect will be suported by the render.
        /// </summary>
        /// <param name="render">Render where to check compatibility</param>
        /// <returns>True if the render supports this effect, false otherwise.</returns>
        bool IsSupported(IRenderDevice render);

        /// <summary>
        /// When implemented saves the states, gets the enumerable object to setup the states and restore the states when disposing the IEnumerable object.
        /// </summary>
        Technique GetTechnique(IRenderStatesManager renderStatesManager);
    }

    /// <summary>
    /// Represents a pass of an effect.
    /// </summary>
    public sealed class Pass
    {
        IRenderStatesInfo _RenderState;

        /// <summary>
        /// Retrieves the current render state for the pass
        /// </summary>
        public IRenderStatesInfo RenderStates
        {
            get { return _RenderState; }
        }

        public Pass(IRenderStatesInfo RenderStateInfo)
        {
            _RenderState = RenderStateInfo;
        }
    }

    /// <summary>
    /// Represents a shader techinque, which is compossed of a
    /// set of <see cref="Pass"/>.
    /// </summary>
    public abstract class Technique : IEnumerable<Pass>, IDisposable
    {
        protected IRenderStatesManager RenderStates { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Technique"/>.
        /// </summary>
        /// <param name="manager">An instance of <see cref="IRenderStatesManager"/> to be used by this technique.</param>
        public Technique(IRenderStatesManager manager)
        {
            if (manager == null)
                throw new ArgumentNullException("manager");

            this.RenderStates = manager;
        }

        internal void Initialize()
        {
            SaveStates();
        }

        /// <summary>
        /// When overriden starts the technique over the render state. The effect sould save the states it will modify in this method.
        /// <example>
        ///     RenderState.Save&lt;MaterialState&gt;();
        /// </example>
        /// </summary>
        protected abstract void SaveStates();

        /// <summary>
        /// When overriden indicates the description of the technique accessing to RenderState, modifying states and produccing passes.
        /// <example>
        /// RenderState.SetState(CullModeState.Front);
        /// RenderState.SetState(MaterialState.From (Vectors.Red));
        /// yield return NewPass();
        /// RenderState.SetState(CullModeState.Back);
        /// RenderState.SetState(MaterialState.From (Vectors.Blue));
        /// yield return NewPass();
        /// </example>
        /// </summary>
        protected abstract IEnumerable<Pass> Passes { get; }

        /// <summary>
        /// When overriden marks the end of the scope for this technique. The effect should restore the states it did modify in this method.
        /// <example>
        ///     RenderState.Restore&lt;MaterialState&gt;();
        /// </example>
        /// </summary>
        protected abstract void RestoreStates();

        /// <summary>
        /// Returns a pass using current render state settings.
        /// </summary>
        /// <returns>A Pass structure indicanting the current render state settings</returns>
        protected Pass NewPass()
        {
            return new Pass(RenderStates);
        }

        public IEnumerator<Pass> GetEnumerator()
        {
            return Passes.GetEnumerator();
        }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Disposes this technique object restoring render states values.
        /// </summary>
        public virtual void Dispose()
        {
            RestoreStates();
        }
    }
}
