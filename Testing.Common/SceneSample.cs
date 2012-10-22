using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Forms;
using System.Compilers.Shaders;
using System.Maths;
using System.Rendering.Effects;
using System.Rendering.RenderStates;
using System.Rendering;
using System.Diagnostics;
using System.Rendering.Modeling;

namespace Testing.Common
{
    public class SceneSample
    {
        //Scene scene = new BasicShaderScene();
        //Scene scene = new TextureCubeScene();
        //Scene scene = new ManifoldsScene();
        //Scene scene = new TextureFormatsScene();
        //Scene scene = new SimpleShaderScene();
        //Scene scene = new GeneratingOverTextureScene();
        Scene scene = new ShadowMapScene();
        //Scene scene = new VolumeTextureScene();
        //Scene scene = new PipelineScene();

        public void Initialize(IControlRenderDevice render)
        {
            scene.Initialize(render);
        }

        public void SameSample(IControlRenderDevice render)
        {
            scene.Draw(render);
        }
    }
}
