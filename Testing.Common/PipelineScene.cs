using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering;
using System.Rendering.Effects;
using System.Maths;
using System.Compilers.Shaders;

namespace Testing.Common
{
    public class PipelineScene : Scene
    {
        IModel model;

        public override void Initialize(System.Rendering.Forms.IControlRenderDevice render)
        {
            model = Models.Teapot.Allocate(render);
        }

        public override void Draw(System.Rendering.Forms.IControlRenderDevice render)
        {
            render.BeginScene();

            var context = render.RenderStateInfo as IBasicContext;

            render.Draw(() =>
            {
                render.Draw(model,
                new VertexShaderEffect<ProjectedData, PositionProjectedData>(In => new PositionProjectedData { Projected = In.Position }) { AppendMode = AppendMode.Append },
                new PixelShaderEffect<PositionProjectedData, ColorData>(In => new ColorData { Color = new Vector4(In.Projected.Z / In.Projected.W, In.Projected.Z / In.Projected.W, In.Projected.Z / In.Projected.W, 1) }));
            },
                Materials.White.Plastic.Shinness.Glossy,
                Lights.Point(new Vector3(2, 5, 3), new Vector3(1, 1, 1)),
                Cameras.LookAt(new Vector3(1, 1, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
                Cameras.Perspective(GMath.PiOver4, render.GetAspectRatio(), 1f, 10),
                Buffers.Clear(new Vector4(0.3f, 0.3f, 0.3f, 1)),
                Buffers.ClearDepth(),
                Shaders.DefaultVertexTransform);

            render.EndScene();
        }
    }

    [ShaderType]
    public struct PositionProjectedData
    {
        [PositionProjected]
        public Vector4 Projected;
    }

    public class PositionProjectedSemantic : DataSemantic
    {
    }

    [CompileSemanticAs(SemanticType = typeof(PositionProjectedSemantic))]
    public class PositionProjectedAttribute : DataComponentAttribute
    {
    }
}
