using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Rendering
{
    public interface IRenderDeviceService
    {
        IRenderDevice Render { get; }
    }

    public interface IRenderDeviceServices
    {
        S Get<S>() where S : struct, IRenderDeviceService;

        bool Support<S>() where S : struct, IRenderDeviceService;
    }
}
