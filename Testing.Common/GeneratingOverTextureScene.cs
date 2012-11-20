using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Effects;
using System.Maths;
using System.Rendering;

namespace Testing.Common
{
	public class GeneratingOverTextureScene : Scene
	{
		TextureBuffer diffuseTexture;
		TextureBuffer diffuseTexture2;

        IModel model1, model2;

		public override void Initialize(System.Rendering.Forms.IControlRenderDevice render)
		{
			diffuseTexture = TextureBuffer.Empty<CustomPixels.ARGB>(512, 512);
			diffuseTexture.GenerateNoise(2030);
			diffuseTexture = diffuseTexture.Allocate(render);

            diffuseTexture2 = render.Services.LoadTexture("Resources/Tulips_Small.jpg").Allocate(render);

            model1 = Models.Teapot.Allocate(render);
            model2 = Models.Cube.Allocate(render);
		}

		public override void Draw(System.Rendering.Forms.IControlRenderDevice render)
		{
			ITextureRenderDevice textureRender = render as ITextureRenderDevice;

            render.BeginScene();

            textureRender.BeginScene(diffuseTexture);

            textureRender.Draw(model1,
                    Materials.White.Glossy.Glossy.Shinness.Shinness,
                    Transforming.Rotate(Environment.TickCount / 1000f, new Vector3(0, 1f, 0f)),
                    Lighting.PointLight(new Vector3(3, 1, -4), new Vector3(1, 1, 1)),
                    Viewing.LookAtLH(new Vector3(0, 0, -4), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
                    Projecting.PerspectiveFovLH((float)Math.PI / 4, 1, 1, 1000),
                    Shading.Phong,
                    Buffers.Clear(new Vector4(1, 0, 0, 1)),
                    Buffers.ClearDepth());

            textureRender.EndScene();

			render.Draw(
			() =>
			{
				render.Draw(model1, Transforming.Translate(2, 0, 0)
						, Materials.White.Shinness.Shinness.Glossy
                        , Shading.DiffuseMap(diffuseTexture)
						);

                render.Draw(model2,
                        Transforming.Rotate(Environment.TickCount / 1000f, Axis.Y),
                        Transforming.Translate(-2, 0, 0),
                        Materials.White.Shinness.Shinness.Glossy.Glossy,
                        Shading.DiffuseMap(diffuseTexture2)
                        );
			},

					Lighting.PointLight(new Vector3(0, 5, -3), new Vector3(1, 1, 1)),
					Viewing.LookAtLH(new Vector3(2, 1, -6), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
					Projecting.PerspectiveFovLH((float)Math.PI / 4, render.ImageHeight / (float)render.ImageWidth, 1, 1000),
                    Shading.Phong,
					Buffers.Clear(new Vector4(1, 0, 1, 1)),
					Buffers.ClearDepth());

			render.EndScene();
		}
	}
}
