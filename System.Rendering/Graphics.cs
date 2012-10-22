using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Rendering
{
    public class Graphics
    {
        public static IGraphic NearPlane { get; private set; }

        public static IGraphic FarPlane { get; private set; }

        public static IGraphic SkyBox { get; private set; }
    }
}
