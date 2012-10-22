using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering;
using System.Rendering.Effects;
using System.Maths;

namespace Testing.Common
{
	public class TextureCubeScene : Scene
	{
		IModel model;
		CubeTextureBuffer cubeTexture;

		public override void Initialize(System.Rendering.Forms.IControlRenderDevice render)
		{
            model = Models.Teapot.Allocate(render);

            var posX = render.Services.LoadTexture("Resources/lobbyxpos.JPG");
            var negX = render.Services.LoadTexture("Resources/lobbyxneg.JPG");
            var posY = render.Services.LoadTexture("Resources/lobbyypos.JPG");
            var negY = render.Services.LoadTexture("Resources/lobbyyneg.JPG");
            var posZ = render.Services.LoadTexture("Resources/lobbyzpos.JPG");
            var negZ = render.Services.LoadTexture("Resources/lobbyzneg.JPG");

            //cubeTexture = CubeTextureBuffer.Empty<CustomPixels.ARGB>(512).Allocate(render);
            //SetDataToFace(CubeFaces.PositiveX, posX.GetData());
            //SetDataToFace(CubeFaces.NegativeX, posX.GetData());
            //SetDataToFace(CubeFaces.PositiveY, posX.GetData());
            //SetDataToFace(CubeFaces.NegativeY, posX.GetData());
            //SetDataToFace(CubeFaces.PositiveZ, posX.GetData());
            //SetDataToFace(CubeFaces.NegativeZ, posX.GetData());

            cubeTexture = CubeTextureBuffer.Create(
                (CustomPixels.ARGB[,])posX.GetData(),
                (CustomPixels.ARGB[,])negX.GetData(),
                (CustomPixels.ARGB[,])posY.GetData(),
                (CustomPixels.ARGB[,])negY.GetData(),
                (CustomPixels.ARGB[,])posZ.GetData(),
                (CustomPixels.ARGB[,])negZ.GetData()
            ).Allocate(render);
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
            Shading.UseReflectionCoordinates,
            //new VertexShaderEffect<NormalData, Coordinates3DData> (In=>new Coordinates3DData { Coordinates = In.Normal }),
				//Filling.Lines,
			Materials.White.Glossy.Glossy.Glossy.Shinness.Shinness,
			Shading.DiffuseMap(cubeTexture),
            Shading.Phong,
            Lighting.AmbientLight(new Vector3(0.4f, 0.4f, 0.4f)),
			Lighting.PointLight(new System.Maths.Vector3(1, 4, 4), Vectors.White),
			Viewing.LookAtLH(new Vector3(GMath.sin((float)Environment.TickCount / 1000.0f), 0, 5), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
			Projecting.PerspectiveFovLH(GMath.PiOver4, render.ImageHeight / (float)render.ImageWidth, 1, 1000),
			Buffering.Clear(new Vector4(0f, 0f, 0f, 1)),
			Buffering.ClearDepth());

			render.EndScene();
		}
	}
}
