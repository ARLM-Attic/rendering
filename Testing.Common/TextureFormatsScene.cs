using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Effects;
using System.Maths;
using System.Rendering;
using System.Rendering.Modeling;

namespace Testing.Common
{
	public class TextureFormatsScene : Scene
	{
		TextureBuffer texture;
		IModel model;

		public override void Initialize(System.Rendering.Forms.IControlRenderDevice render)
		{
			model = Models.Cylinder.Allocate (render);
            texture = render.Services.LoadTexture("Resources/Tulips_Small.jpg").Allocate(render);
		}

		public override void Draw(System.Rendering.Forms.IControlRenderDevice render)
		{
			render.BeginScene();

			render.Draw(() =>
			{
				render.Draw(model,
						Transforming.Rotate(Environment.TickCount / 1000f, Axis.Y),
						Transforming.Translate(new System.Maths.Vector3(2, 0, 0)));
				render.Draw(model,
						Transforming.Rotate(Environment.TickCount / 1000f, Axis.X),
						Transforming.Translate(new System.Maths.Vector3(-2, 0, 0)));
			},
				//Filling.Lines,
			Materials.White.Plastic.Shinness.Shinness.Shinness.Glossy,
			Shading.DiffuseMap(texture),
			Lighting.AmbientLight(new Vector3(0.2f, 0.2f, 0.2f)),
			Lighting.PointLight(new System.Maths.Vector3(1, 4, 4), Vectors.White),
            Shading.Phong,
			Viewing.LookAtRH(new Vector3(GMath.sin((float)Environment.TickCount / 1000.0f), 0, 5), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
			Projecting.PerspectiveFovRH(GMath.PiOver4, render.ImageHeight / (float)render.ImageWidth, 1, 1000),
			Buffers.Clear(new Vector4(0.4f, 0.4f, 0.5f, 1)),
			Buffers.ClearDepth());

			render.EndScene();
		}
	}
}
