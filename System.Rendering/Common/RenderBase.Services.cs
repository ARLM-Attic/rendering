using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Rendering
{
    partial class RenderBase
    {
        public class ServicesManagerBase : IRenderDeviceServices
        {
            protected ServicesManagerBase(RenderBase render)
            {
                this.Render = render;

                InitializeServices();
            }

            protected IRenderDevice Render { get; private set; }

            Dictionary<int, IRenderDeviceService> services = new Dictionary<int, IRenderDeviceService>();

            protected internal virtual void InitializeServices()
            {

            }

            protected S Create<S>() where S : struct, IRenderDeviceService
            {
                var service = (S)Activator.CreateInstance(typeof(S), this.Render);
                services[typeof(S).MetadataToken] = service;
                return service;
            }

            public S Get<S>() where S : struct, IRenderDeviceService
            {
                int index = typeof(S).MetadataToken;
                if (services.ContainsKey(index))
                    return (S)services[index];
                return default(S);
            }

            public bool Support<S>() where S : struct, IRenderDeviceService
            {
                return services.ContainsKey(typeof(S).MetadataToken);
            }
        }
    }
}
