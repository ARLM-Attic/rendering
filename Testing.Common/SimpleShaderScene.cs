using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Effects;
using System.Maths;
using System.Rendering;
using System.Compilers.Shaders;
using System.Rendering.Modeling;

namespace Testing.Common
{
	public class SimpleShaderScene : Scene
	{
		IModel model;
		TextureBuffer diffuseTexture;
		TextureBuffer coordinates;

		public override void Initialize(System.Rendering.Forms.IControlRenderDevice render)
		{
			model = new Cylinder();

			coordinates = TextureBuffer.Empty<CustomPixels.ARGB>(512, 512);
			coordinates.Fill2D(v => new Vector4(v.X, v.Y, 0, 0));
			coordinates = coordinates.Allocate(render);

			diffuseTexture = TextureBuffer.Empty<CustomPixels.ARGB>(512, 512);
			diffuseTexture.GenerateNoise(2030);
			diffuseTexture = diffuseTexture.Allocate(render);
		}

		public override void Draw(System.Rendering.Forms.IControlRenderDevice render)
		{
			frames++;
			renderStopwatch.Start();

			render.BeginScene();

			Sampler2D sampler = new Sampler2D(diffuseTexture)
			{
				Addresses = new Addressing2D { U = TextureAddress.Wrap, V = TextureAddress.Wrap },
				Filters = new Filtering { Magnification = TextureFilterType.Linear, Minification = TextureFilterType.Linear, MipMapping = TextureFilterType.Linear }
			};

			Sampler2D coordinatesSampler = new Sampler2D(coordinates)
			{
				Addresses = new Addressing2D { U = TextureAddress.Wrap, V = TextureAddress.Wrap },
				Filters = new Filtering { Magnification = TextureFilterType.Linear, Minification = TextureFilterType.Linear, MipMapping = TextureFilterType.Linear }
			};

			render.Draw(
					() =>
					{
                        render.Draw(
                                model
                                , new VertexShaderEffect<PositionData, PositionData>(In => new PositionData
                                {
                                    Position = In.Position + GMath.sin(In.Position.X * 20) * new Vector3(0, 0.05f, 0)
                                })
                                , new PixelShaderEffect<ColorData, ColorData>(In =>
                                {
                                    var diffuse = In.Color;

                                    return new ColorData
                                    {
                                        Color = new Vector4(diffuse.X, 0, 0, 1)
                                    };
                                }
                                )
                                , new PixelShaderEffect<CoordinatesData, ColorData>(In =>
                                {
                                    var tc = coordinatesSampler.Sample((Vector2)In.Coordinates);

                                    var diffuse = (Vector3)sampler.Sample((Vector2)tc);

                                    return new ColorData
                                    {
                                        Color = new Vector4(diffuse.X, diffuse.Y, diffuse.Z, 1)
                                    };
                                }
                                )
                                );

						//render.Draw(Models.Cube);
					}
					,
			Transforming.Rotate(Environment.TickCount / 500f, Axis.Y),
			Lighting.PointLight(new Vector3(4, 4, -6), new Vector3(1, 1, 1)),
			Viewing.LookAtLH(new Vector3(3, 2f, -5), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
			Materials.White.Shinness.Shinness.Glossy.Glossy,
			Projecting.PerspectiveFovLH((float)Math.PI / 4, render.ImageHeight / (float)render.ImageWidth, 1, 1000),
			
            Shading.Phong,
            
            Buffering.ClearDepth(),
			Buffering.Clear(new Vector4(0.6f, 0.5f, 0.5f, 1)));
			render.EndScene();

			renderStopwatch.Stop();

			if (appTime.ElapsedMilliseconds > 10000)
			{
				appTime.Restart();
				Console.WriteLine("!!! FPS : " + (int)(frames * 1000f / renderStopwatch.Elapsed.TotalMilliseconds));
				frames = 0;
				renderStopwatch.Restart();
			}
		}
	}
}
