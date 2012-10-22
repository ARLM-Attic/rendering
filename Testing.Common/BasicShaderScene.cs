using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering;
using System.Rendering.Forms;
using System.Rendering.Effects;
using System.Maths;
using System.Rendering.Services;
using System.Rendering.RenderStates;
using System.Compilers.Shaders;

namespace Testing.Common
{
	/// <summary>
	/// Checks the normal shader support.
	/// </summary>
	public class BasicShaderScene : Scene
	{
		TextureBuffer texture;

		IModel model;

		public override void Initialize(System.Rendering.Forms.IControlRenderDevice render)
		{
			texture = render.Services.LoadTexture("Resources/Tulips_Small.jpg").Allocate(render);

			var teapot = Models.Teapot.Allocate(render);
            model = teapot;
			//teapot.WeldVertexes(0.01f);
			//teapot.ComputeNormals();
			//var t = teapot.Tessellated(4);
			//model = t;
			//teapot.Dispose();
		}

        abstract class ColorFilter
        {
            [ShaderMethod]
            public abstract Vector4 Filter(Vector4 color);

            public ColorData Process(ColorData In)
            {
                return new ColorData { Color = Filter(In.Color) };
            }
        }

        class MultiplyFilter : ColorFilter
        {
            Vector4 filter;

            public MultiplyFilter(Vector4 filter)
                : base()
            {
                this.filter = filter;
            }

            public override Vector4 Filter(Vector4 color)
            {
                return color * filter;
            }
        }

        class RedFilter : MultiplyFilter
        {
            public RedFilter()
                : base(new Vector4(1, 0, 0, 1))
            {
            }

            public override Vector4 Filter(Vector4 color)
            {
                return new Vector4(color.X, 0, 0, 1);
            }
        }

        class GreenFilter : MultiplyFilter
        {
            public GreenFilter()
                : base(new Vector4(0, 1, 0, 1))
            {
            }

            public override Vector4 Filter(Vector4 color)
            {
                return new Vector4(0, color.Y, 0, 1);
            }
        }

        ColorFilter teapot1Filter = new RedFilter();
        ColorFilter teapot2Filter = new GreenFilter();
        ColorFilter teapot3Filter = new MultiplyFilter(new Vector4(0, 0, 1, 1));
        ColorFilter teapot4Filter = new MultiplyFilter(new Vector4(1, 1, 1, 1));

		public override void Draw(System.Rendering.Forms.IControlRenderDevice render)
		{
			render.BeginScene();

			render.Draw(() =>
			{
                render.Draw(model
                        , Transforming.Rotate(Environment.TickCount / 1000f, Axis.Y)
                        , Transforming.Translate(new System.Maths.Vector3(-2, 0, 0))
                        , new PixelShaderEffect<ColorData, ColorData>(teapot1Filter.Process)
                        );
                render.Draw(model
                        , Transforming.Rotate(Environment.TickCount / 1000f, Axis.Y)
                        , Transforming.Translate(new System.Maths.Vector3(2, 0, 0))
                        , new PixelShaderEffect<ColorData, ColorData>(teapot2Filter.Process)
                        );
                render.Draw(model
                        , Transforming.Rotate(Environment.TickCount / 1000f, Axis.Y)
                        , Transforming.Translate(new System.Maths.Vector3(0, -2, 0))
                        , new PixelShaderEffect<ColorData, ColorData>(teapot3Filter.Process)
                        );
                render.Draw(model
                        , Transforming.Rotate(Environment.TickCount / 1000f, Axis.Y)
                        , Transforming.Translate(new System.Maths.Vector3(0, 2, 0))
                        , new PixelShaderEffect<ColorData, ColorData>(teapot4Filter.Process)
                        );
				},
				//Filling.Lines,
			Materials.White.Plastic.Plastic.Glossy.Glossy.Shinness.Shinness,
            //Shading.DiffuseMap (v=>new Vector4(GMath.sin(Environment.TickCount/300f), GMath.cos(Environment.TickCount/300f), 0, 1)),
            Shading.DiffuseMap(texture),
            Shading.Phong,
			Lighting.AmbientLight(new Vector3(0.2f, 0.2f, 0.2f)),
			Lighting.PointLight(new System.Maths.Vector3(1, 4, 4), Vectors.White),
			Viewing.LookAtLH(new Vector3(GMath.sin((float)Environment.TickCount / 1000.0f), 0, 6), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
			Projecting.PerspectiveFovLH(GMath.PiOver4, render.ImageHeight / (float)render.ImageWidth, 1, 1000),
			Buffering.Clear(new Vector4(0f, 0f, 0f, 1)),
			Buffering.ClearDepth());

			render.EndScene();
		}
	}
}
