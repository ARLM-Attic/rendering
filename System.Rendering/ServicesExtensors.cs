using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Services;

namespace System.Rendering
{
    public static class ServicesExtensors
    {
        public static void Save(this TextureBuffer texture, string path)
        {
            if (texture.Render != null)
                throw new InvalidOperationException("Texture needs to be binded to a render to allow this operation.");

            texture.Render.Services.Get<LoaderService<TextureBuffer>>().Save(texture, path);
        }

        public static TextureBuffer LoadTexture(this IRenderDeviceServices services, string path)
        {
            return services.Get<LoaderService<TextureBuffer>>().Load(path);
        }
    }
}
