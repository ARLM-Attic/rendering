using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Effects;
using System.Maths;
using System.Rendering;
using System.Rendering.RenderStates;
using System.Rendering.Modeling;
using System.Compilers.Shaders;

namespace Testing.Common
{
    public class VolumeTextureScene : Scene
    {
        IModel model;
        IModel plane;
        TextureBuffer volumeTexture;

        public override void Initialize(System.Rendering.Forms.IControlRenderDevice render)
        {
            plane = Models.PlaneXZ.Allocate(render);

            model = Models.Sphere.Allocate (render);//.Tessellated (3);
            //((Mesh)model).WeldVertexes(0);
            
            volumeTexture = TextureBuffer.Empty<CustomPixels.ARGB>(64, 64, 64);
            Random rnd = new Random(1020);
            volumeTexture.Fill3D(v => new Vector4(new Vector3(1f, 1f, 1) * ((float)rnd.NextDouble()/2+0.5f), 1));
            //volumeTexture.GenerateNoise(22020);
            volumeTexture = volumeTexture.Allocate(render);

            appTime.Start();
        }

        public override void Draw(System.Rendering.Forms.IControlRenderDevice render)
        {
            frames++;
            float time = Environment.TickCount % 1000f / 1000f;

            render.BeginScene();
            render.Draw(() =>
            {
                render.Draw(() =>
                {
                    //var context = render.PipelineContext;

                    render.Draw(() =>
                    {
                        render.Draw(plane, Transforms.Translate(0, -1, 0), Shaders.UseOrthographicCoordinates, Materials.Teal.Glossy.Glossy.Glossy.Shinness.Shinness);
                        render.Draw(model, Transforms.Rotate(Environment.TickCount / 1000f, Axis.Y)
                                 //, new VertexShaderEffect<PositionData, PositionCoordinates3DData>(In =>
                                 //{
                                 //    var v = In.Position + new Vector3(0, 0.05f, 0) * GMath.sin(10 * In.Position.X);
                                 //    return new PositionCoordinates3DData
                                 //    {
                                 //        Position = v,
                                 //        Coordinates = In.Position
                                 //    };
                                 //})
                         );
                    });
                });
            },
            Materials.White.Shinness.Glossy,
            Shaders.DiffuseMap(volumeTexture),
            Lights.Point(new Vector3(0, 5, -6), new Vector3(1, 1, 1)),
            Lights.Ambient(new Vector3(0.2f, 0.2f, 0.2f)),
            Cameras.LookAt(new Vector3(2, 1f, -8), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
            Cameras.Perspective(render.GetAspectRatio()),
            
            Shaders.Gouraud,
            
            Buffers.ClearDepth(),
            Buffers.Clear(new Vector4(0.6f, 0.5f, 0.5f, 1)));
            render.EndScene();
        }
    }
}