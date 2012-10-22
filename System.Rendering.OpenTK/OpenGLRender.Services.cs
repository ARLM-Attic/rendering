using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Services;
using System.Rendering.Forms;

namespace System.Rendering.OpenGL
{
    partial class OpenGLRender
    {
        public class OpenGLServices : ServicesManagerBase
        {
            public OpenGLServices(OpenGLRender render)
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
