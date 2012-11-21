using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Resourcing;

namespace System.Rendering.Resourcing
{
    public abstract class AllocateableBase : IAllocateable
    {
        private IRenderDevice _Render;

        protected AllocateableBase()
            : this(null)
        {
        }

        protected AllocateableBase(IRenderDevice render)
        {
            this._Render = render;
            this._Location = render == null ? Location.User : Location.Device;
        }

        protected abstract Location OnClone(AllocateableBase toFill, IRenderDevice render);

        protected abstract void OnDispose();

        IRenderDevice IAllocateable.Render { get { return _Render; } }

        Location _Location;

        Location IAllocateable.Location { get { return _Location; } }

        IAllocateable IAllocateable.Clone(IRenderDevice render)
        {
            AllocateableBase a = this.MemberwiseClone () as AllocateableBase;
            a._Render = render;
            _Location = this.OnClone(a, render);
            return a;
        }

        public void Dispose()
        {
            OnDispose();
        }
    }

    
}

namespace System.Rendering
{
    public static class AllocateableBaseExtensors
    {
        public static T Allocate<T>(this T allocateable, IRenderDevice render) where T : IAllocateable
        {
            return (T)allocateable.Clone(render);
        }
    }
}
