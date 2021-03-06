﻿using System;
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
		IModel model1;
		IModel model2;

		IModel union;

		public override void Initialize(IControlRenderDevice render)
		{
			model1 = Models.Cube;
			
			model2 = Models.Cube.Transformed(Matrices.Translate(0.0f, 0.5f, 0.0f));
			
			union = Models.Union (model1, model2);
		}

		public override void Draw(IControlRenderDevice render)
		{
			render.BeginScene();

			render.Draw(() =>
			{
				render.Draw(union);
			},
			Materials.Red.Plastic.Plastic.Glossy.Glossy.Shinness.Shinness,
			Shaders.Gouraud,
			Shaders.DefaultVertexTransform,
			Shaders.UseMaterial,
			Lights.Ambient(new Vector3(0.2f, 0.2f, 0.2f)),
			Lights.Point(new System.Maths.Vector3(1, 4, 4), Vectors.White),
			Cameras.LookAt(new Vector3(GMath.sin((float)Environment.TickCount / 1000.0f), 3, 6), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
			Cameras.Perspective(render.GetAspectRatio()),
			Buffers.Clear(new Vector4(0.2f, 0.2f, 0.2f, 1.0f)),
			Buffers.ClearDepth());

			render.EndScene();
		}
	}
}
