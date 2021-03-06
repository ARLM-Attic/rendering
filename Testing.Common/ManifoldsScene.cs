﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering;
using System.Maths;
using System.Rendering.Effects;
using System.Rendering.Modeling;

namespace Testing.Common
{
    public class ManifoldsScene : Scene
    {
        IModel manifold;

        public override void Initialize(System.Rendering.Forms.IControlRenderDevice render)
        {
            manifold = Models.Surface((u, v) => new Vector3(u, v, 0), 128, 128, false).Allocate(render);
        }

        public override void Draw(System.Rendering.Forms.IControlRenderDevice render)
        {
            render.BeginScene();
            render.Draw(() =>
            {
                render.Draw(manifold, new VertexShaderEffect<PositionData, PositionData>(In =>
                {
                    var x = In.Position.X;
                    var y = In.Position.Y;
                    float z = 0;
                    for (int i = 0; i < 10; i++)
                        z += GMath.sin(x * i * 10) * y * GMath.sin(Environment.TickCount / 1000f);
                    return new PositionData { Position = new Vector3(x, z / 10, y) };
                }));
            },
            Transforms.Rotate(Environment.TickCount / 1000f, Axis.Y),
            Materials.White.Plastic.Plastic,
            Lights.Point(new Vector3(0, 5, -6), new Vector3(1, 1, 1)),
            Lights.Ambient(new Vector3(0.2f, 0.2f, 0.2f)),
                //AlphaBlending.BlendAlpha,
            Shaders.Phong,
            Cameras.LookAt(new Vector3(1f, 1f, 2), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
            Cameras.Perspective(render.GetAspectRatio()),
            Buffers.ClearDepth(),
            Buffers.Clear(new Vector4(0.6f, 0.5f, 0.5f, 1)));
            render.EndScene();
        }
    }
}
