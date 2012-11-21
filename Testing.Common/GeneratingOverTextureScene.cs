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
                    Transforms.Rotate(Environment.TickCount / 1000f, new Vector3(0, 1f, 0f)),
                    Lights.Point(new Vector3(3, 1, -4), new Vector3(1, 1, 1)),
                    Cameras.LookAt(new Vector3(0, 0, -4), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
                    Cameras.Perspective(1),
                    Shaders.Phong,
                    Buffers.Clear(new Vector4(1, 0, 0, 1)),
                    Buffers.ClearDepth());

            textureRender.EndScene();

			render.Draw(
			() =>
			{
				render.Draw(model1, Transforms.Translate(2, 0, 0)
						, Materials.White.Shinness.Shinness.Glossy
                        , Shaders.DiffuseMap(diffuseTexture)
						);

                render.Draw(model2,
                        Transforms.Rotate(Environment.TickCount / 1000f, Axis.Y),
                        Transforms.Translate(-2, 0, 0),
                        Materials.White.Shinness.Shinness.Glossy.Glossy,
                        Shaders.DiffuseMap(diffuseTexture2)
                        );
			},

					Lights.Point(new Vector3(0, 5, -3), new Vector3(1, 1, 1)),
					Cameras.LookAt(new Vector3(2, 1, -6), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
					Cameras.Perspective(render.GetAspectRatio()),
                    Shaders.Phong,
					Buffers.Clear(new Vector4(1, 0, 1, 1)),
					Buffers.ClearDepth());

			render.EndScene();
		}
	}
}
