using System;
using System.Collections.Generic;
using System.Text;
using System.Maths;

namespace System.Rendering
{
    /// <summary>
    /// Base implementation for rendering and interaction porpouses.
    /// </summary>
    public abstract partial class RenderBase : IRenderDevice, IInteractiveRenderDevice
    {
        /// <summary>
        /// Resources manager module for this render.
        /// </summary>
        protected readonly ResourcesManagerBase Resources;
        /// <summary>
        /// Tessellator module for this render.
        /// </summary>
        protected readonly TessellatorBase Tessellator;
        /// <summary>
        /// RenderStates module for this render
        /// </summary>
        protected readonly RenderStatesManagerBase RenderStates;

        /// <summary>
        /// Services module for this render
        /// </summary>
        protected readonly ServicesManagerBase ServicesManager;

        /// <summary>
        /// This method sould be implemented to create the services manager for this render.
        /// </summary>
        /// <returns>The services manager object binded to this render to use as service provider.</returns>
        protected abstract ServicesManagerBase CreateServicesManager();

        /// <summary>
        /// This method should be implemented to create the resource manager for this render.
        /// </summary>
        /// <returns>The resources manager object used to manage resources.</returns>
        protected abstract ResourcesManagerBase CreateResourcesManager();

        /// <summary>
        /// This method should be implemented to create the tessellator for this render.
        /// </summary>
        /// <returns>The tessellator object used to draw models.</returns>
        protected abstract TessellatorBase CreateTessellator();

        /// <summary>
        /// This method should be implemented to create the Render States manager for this render.
        /// </summary>
        /// <returns>The RenderStatesManager object used to set render states.</returns>
        protected abstract RenderStatesManagerBase CreateRenderStatesManager();

        /// <summary>
        /// Initializes the render object creating the modules for Render states, tessellator and resources manager.
        /// </summary>
        public RenderBase()
        {
            RenderStates = CreateRenderStatesManager();
            Tessellator = CreateTessellator();
            Resources = CreateResourcesManager();
            ServicesManager = CreateServicesManager();
        }

        #region IRender Members

        /// <summary>
        /// Initializes the scene of a generic render. This method initializes the interaction methods.
        /// </summary>
        public virtual void BeginScene()
        {
            foreach (IRayPicker rayPicker in rayPickerManager)
            {
                RayPickerManager.InfoByRayPicker info = rayPickerManager[rayPicker];
                info.Clear();
            }
        }

        /// <summary>
        /// Applies some effect in this render.
        /// </summary>
        /// <param name="effect">The effect object to be applied. This parameter can be null, but it should not be used and its assumed as no effect at all.</param>
        /// <returns>The IEnumerable object used to iterate by all the passes.
        /// Each pass setup certain configuration of the render by the effect.</returns>
        public virtual IEnumerable<Pass> Apply(IEffect effect)
        {
            effects.Push(effect);

            if (effect == null)
            {
                clones.Push(null);
                yield return new Pass(RenderStates);
                yield break;
            }

            var technique = effect.GetTechnique(this.RenderStates);

            clones.Push(technique);

            foreach (Pass p in technique)
                yield return p;
        }

        /// <summary>
        /// Stack for managing effects being applied and unapplied.
        /// </summary>
        Stack<IEffect> effects = new Stack<IEffect>();

        /// <summary>
        /// Stack of techniques being applied.
        /// </summary>
        Stack<Technique> clones = new Stack<Technique>();

        /// <summary>
        /// Ends the scope for certain effect. This effect should be the same at top of applied effect, otherwise, an Exception is thrown.
        /// </summary>
        /// <param name="effect">Effect object to be unapplied.</param>
        public virtual void EndScope(IEffect effect)
        {
            var _effect = effects.Pop();
            var _technique = clones.Pop();

            if (_effect != effect)
                throw new InvalidOperationException();

            if (effect == null) return;

            _technique.Dispose();
        }

        /// <summary>
        /// Ends the scene visualization and raise all notifications of interaction.
        /// </summary>
        public virtual void EndScene()
        {
            //foreach (IRayPicker rayPicker in rayPickerManager)
            //{
            //    RayPickerManager.InfoByRayPicker info = rayPickerManager[rayPicker];
            //    if (info.Closest != null)
            //    {
            //        RayInteractionEventArgs e = new RayInteractionEventArgs(rayPicker, info.Closest, true);
            //        foreach (IRayListener rayListener in rayListeners)
            //            rayListener.Notify(e);
            //    }
            //}

            if (effects.Count > 0)
                Console.WriteLine("effects without end scopes " + effects.Count);
        }

        public void Draw(IModel model)
        {
            if (model != null)
                model.Tesselate(this.Tessellator);
        }

        public virtual void Draw(IDrawable drawable)
        {
            if (drawable is IModel)
            {
                (drawable as IModel).Tesselate(Tessellator);
                return;
            }
            if (drawable is IGraphic)
            {
                ((IGraphic)drawable).ViewAt(this);
                return;
            }

            CallUnknownDrawable(drawable);
        }

        public virtual void CallUnknownDrawable(IDrawable drawable)
        {
            // Do nothing, dont require to implement.
            throw new NotSupportedException ();
        }

        #endregion

        public virtual void Dispose()
        {
            Resources.Dispose();
            RenderStates.Dispose();
        }

        #region IInteractiveRender Members

        /// <summary>
        /// Special stack used to storage listener objects during interaction.
        /// </summary>
        /// <typeparam name="T">Type of an element in the stack.</typeparam>
        internal class StackEx <T> : IEnumerable<T>
        {
            List<T> inner = new List<T>();

            public void Push(T item)
            {
                inner.Add(item);
            }
            public T Pop()
            {
                T result = inner[inner.Count - 1];
                if (inner.Count > 1)
                    inner.RemoveAt(inner.Count - 1);
                return result;
            }
            public void ConvertWhile(T item, Func<T, bool> pred)
            {
                int i = inner.Count - 1;
                while (i >= 0 && pred(inner[i]))
                {
                    inner[i] = item;
                    i--;
                }
            }
            public T Top
            {
                get { return inner[inner.Count - 1]; }
            }

            public T Bottom
            {
                get { return inner[0]; }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return inner.GetEnumerator();
            }

            Collections.IEnumerator Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        #region RayPickerManager Class

        /// <summary>
        /// Class used to manage Ray pickers.
        /// </summary>
        internal class RayPickerManager : IList<IRayPicker>
        {
            /// <summary>
            /// Particular info for a ray picker. Each ray picker have a special stack with the hierarchy of intersected objects.
            /// </summary>
            public class InfoByRayPicker
            {
                public InfoByRayPicker()
                {
                    Clear();
                }

                public IntersectInfo Closest { get { return Stack.Bottom; } }

                public StackEx<IntersectInfo> Stack { get; private set; }

                public void Clear()
                {
                    Stack = new StackEx<IntersectInfo>();
                }

            }

            Dictionary<IRayPicker, InfoByRayPicker> dictionary = new Dictionary<IRayPicker, InfoByRayPicker>();
            List<IRayPicker> innerList = new List<IRayPicker>();
            
            #region IList<IRayPicker> Members

            public int IndexOf(IRayPicker item)
            {
                return innerList.IndexOf(item);
            }

            public void Insert(int index, IRayPicker item)
            {
                innerList.Insert(index, item);
                dictionary.Add(item, new InfoByRayPicker());
            }

            public void RemoveAt(int index)
            {
                IRayPicker item = this[index];
                innerList.RemoveAt(index);
                dictionary.Remove(item);
            }

            public IRayPicker this[int index]
            {
                get
                {
                    return innerList[index];
                }
                set
                {
                    innerList[index] = value;
                }
            }

            #endregion

            #region ICollection<IRayPicker> Members

            public void Add(IRayPicker item)
            {
                innerList.Add(item);
                dictionary.Add(item, new InfoByRayPicker ());
            }

            public void Clear()
            {
                innerList.Clear();
                dictionary.Clear();
            }

            public bool Contains(IRayPicker item)
            {
                return dictionary.ContainsKey(item);
            }

            public void CopyTo(IRayPicker[] array, int arrayIndex)
            {
                innerList.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return innerList.Count; }
            }

            public bool IsReadOnly
            {
                get { return false ; }
            }

            public bool Remove(IRayPicker item)
            {
                dictionary.Remove(item);
                return innerList.Remove(item);
            }

            #endregion

            #region IEnumerable<IRayPicker> Members

            public IEnumerator<IRayPicker> GetEnumerator()
            {
                return innerList.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return innerList.GetEnumerator();
            }

            #endregion

            public InfoByRayPicker this[IRayPicker rayPicker]
            {
                get { return dictionary[rayPicker]; }
            }
        }

        #endregion
        
        /// <summary>
        /// Manager for ray pickers. Those ray pickers can be used for interact with objects at the scene after rendering.
        /// </summary>
        internal RayPickerManager rayPickerManager = new RayPickerManager ();

        /// <summary>
        /// Gets the list of ray pickers used to interact with objects rendered at the end of the scene.
        /// </summary>
        public IList<IRayPicker> RayPickers
        {
            get { return rayPickerManager; }
        }

        internal Stack<IRayListener> rayListeners = new Stack<IRayListener>();

        /// <summary>
        /// Pushes a new ray listener at the listeners stack.
        /// </summary>
        /// <param name="obj">Ray listener that will receive ray notifications</param>
        public void PushRayListener(IRayListener obj)
        {
            rayListeners.Push(obj);
            foreach (IRayPicker rayPicker in rayPickerManager)
                rayPickerManager[rayPicker].Stack.Push(null);
        }

        IntersectInfo closest = null;

        public void PopRayListener()
        {
            IRayListener rayListener = rayListeners.Peek();
            foreach (IRayPicker rayPicker in rayPickerManager)
            {
                RayPickerManager.InfoByRayPicker infoByRayPicker = rayPickerManager[rayPicker];
                StackEx<IntersectInfo> stack = infoByRayPicker.Stack;
                IntersectInfo info = stack.Pop();
                rayListener.Notify(new RayInteractionEventArgs(rayPicker, info));
            }
        }

        #endregion

        public IResourcesManager ResourcesManager
        {
            get { return Resources; }
        }

        public IRenderStatesInfo RenderStateInfo
        {
            get { return RenderStates; }
        }

        public ITessellatorInfo TessellatorInfo
        {
            get { return this.Tessellator; }
        }

        public IRenderDeviceServices Services { get { return this.ServicesManager; } }

        public int ImageWidth { get; protected set; }

        public int ImageHeight { get; protected set; }
    }
}
