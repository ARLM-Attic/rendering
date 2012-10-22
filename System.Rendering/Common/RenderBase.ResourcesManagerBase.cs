using System;
using System.Collections.Generic;
using System.Text;

namespace System.Rendering
{
    public abstract partial class RenderBase
    {
        public class ResourcesManagerBase : IResourcesManager
        {
            protected RenderBase render;

            public ResourcesManagerBase(RenderBase render)
            {
                this.render = render;
            }

            /// <summary>
            /// Gets the render state manager of render associated to this Resource Manager
            /// </summary>
            protected IRenderStatesManager RenderState
            {
                get { return render.RenderStates; }
            }

            /// <summary>
            /// Gets the resource manager of render associated to this Resource Manager
            /// </summary>
            protected IResourcesManager Resources
            {
                get { return render.Resources; }
            }

            /// <summary>
            /// Gets the tessellator of render associated to this Resource Manager
            /// </summary>
            protected ITessellator Modelling
            {
                get { return render.Tessellator; }
            }

            #region IResourceManager Members

            public R Create<R, T>(params int[] size)
                where R : IGraphicResource
                where T : struct
            {
                if (this is IResourceManagerOf<R>)
                    return ((IResourceManagerOf<R>)this).Create(typeof(T), size);
                throw new NotSupportedException();
            }

            public SupportMode GetSupport<R>() where R : IGraphicResource
            {
                return (!(this is IResourceManagerOf<R>)) ? SupportMode.None :
                    ((IResourceManagerOf<R>)this).SupportMode;
            }

            public IResourceOnDeviceManager GetManagerFor<R>(R resource) where R:IGraphicResource
            {
                if (this is IResourceManagerOf<R>)
                    return ((IResourceManagerOf<R>)this).GetManagerFor(resource);

                throw new KeyNotFoundException("resource");
            }

            #endregion

            Dictionary<IGraphicResource, IResourceOnDeviceManager> managers = new Dictionary<IGraphicResource, IResourceOnDeviceManager>();
            Dictionary<IResourceOnDeviceManager, IGraphicResource> resources = new Dictionary<IResourceOnDeviceManager, IGraphicResource>();

            protected R Register<R>(IResourceOnDeviceManager manager) where R: IGraphicResource
            {
                R resource = GraphicResource.CreateInternalResource<R>(manager);
                resources.Add(manager, resource);
                managers.Add(resource, manager);
                return resource;
            }

            protected void UnregisterByResource<R>(R resource) where R : IGraphicResource
            {
                var manager = managers[resource];

                managers.Remove(resource);
                resources.Remove(manager);
            }

            protected void UnregisterByManager(IResourceOnDeviceManager manager)
            {
                var resource = resources[manager];

                managers.Remove(resource);
                resources.Remove(manager);
            }

            protected IResourceOnDeviceManager GetManager(IGraphicResource resource)
            {
                if (managers.ContainsKey(resource))
                    return managers[resource];
                throw new ArgumentOutOfRangeException("resource");
            }

            public virtual void Dispose()
            {
                foreach (var m in new List<IResourceOnDeviceManager>(managers.Values))
                    m.Release();
            }

            
        }
    }
}
