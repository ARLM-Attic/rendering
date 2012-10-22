using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Services;
using Microsoft.Xna.Framework.Graphics;
using System.Rendering.Forms;

namespace System.Rendering.Xna
{
    partial class XnaRender
    {
        protected class XnaServices : ServicesManagerBase
        {
            public XnaServices(XnaRender render)
                : base(render)
            {
            }

            protected override void InitializeServices()
            {
                var textureService = Create<LoaderService<TextureBuffer>>();
                textureService.Register(new Win32TextureLoader());
            }
        }
    }
}
