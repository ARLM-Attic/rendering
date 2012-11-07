using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Modeling;
using System.Rendering.Forms;
using System.Rendering.Effects;
using System.Maths;
using System.Rendering;

namespace Testing.Common
{
	public class CSGModelScene : Scene
	{
		Mesh cube1, cube2;

		CSGModel model1;
		CSGModel model2;

		CSGModel union, intersection, subtraction;

		public override void Initialize(IControlRenderDevice render)
		{
			cube1 = Models.Cube.Transform(Matrices.Translate(0.0f, 0.5f, 0.0f));
			model1 = new CSGModel(cube1);

			cube2 = Models.Cube.Transform(Matrices.Translate(0.5f, 0.0f, 0.5f));
			model2 = new CSGModel(cube2);

			union = model1.Union(model2);
			intersection = model1.Intersect(model2);
			subtraction = model1.Subtract(model2);
		}

		public override void Draw(IControlRenderDevice render)
		{
			render.BeginScene();

			render.Draw(() =>
			{
				render.Draw(() =>
				{
					render.Draw(model1,
						Materials.Blue.Plastic.Plastic.Glossy.Glossy.Shinness.Shinness,
						Transforming.Rotate((float)Environment.TickCount / 1000.0f, Axis.Y));

					render.Draw(model2,
						Materials.Yellow.Plastic.Plastic.Glossy.Glossy.Shinness.Shinness,
						Transforming.Rotate((float)Environment.TickCount / 1000.0f, Axis.Y));

				}, Transforming.Translate(-2.0f, 2.0f, 0.0f));

				render.Draw(() =>
				{
					render.Draw(union,
						Materials.Green.Plastic.Plastic.Glossy.Glossy.Shinness.Shinness,
						Transforming.Rotate((float)Environment.TickCount / 1000.0f, Axis.Y));

				}, Transforming.Translate(2.0f, 2.0f, 0.0f));

				render.Draw(() =>
				{
					render.Draw(intersection,
						Materials.Green.Plastic.Plastic.Glossy.Glossy.Shinness.Shinness,
						Transforming.Rotate((float)Environment.TickCount / 1000.0f, Axis.Y));

				}, Transforming.Translate(2.0f, 0.0f, 0.0f));

				render.Draw(() =>
				{
					render.Draw(subtraction,
						Materials.Green.Plastic.Plastic.Glossy.Glossy.Shinness.Shinness,
						Transforming.Rotate((float)Environment.TickCount / 1000.0f, Axis.Y));

				}, Transforming.Translate(-2.0f, 0.0f, 0.0f));

			},
			//Filling.Lines,
			Shading.Gouraud,
			Shading.VertexTransform,
			Shading.UseMaterial,
			Lighting.AmbientLight(new Vector3(0.2f, 0.2f, 0.2f)),
			Lighting.PointLight(new System.Maths.Vector3(1, 4, 0), Vectors.White),
			Viewing.LookAtLH(new Vector3(0.0f, 5.0f, -8.0f), new Vector3(0, 1, 0), new Vector3(0, 1, 0)),
			Projecting.PerspectiveFovLH(GMath.PiOver4, (float)render.ImageHeight / render.ImageWidth, 0.1f, 100.0f),
			Buffering.Clear(new Vector4(0.2f, 0.2f, 0.2f, 1.0f)),
			Buffering.ClearDepth());

			render.EndScene();
		}
	}
}
