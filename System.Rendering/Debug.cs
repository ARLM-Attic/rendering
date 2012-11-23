using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Modeling;

namespace System.Rendering
{
    public static class RenderDebugger
    {
        public static void DrawNormals(IModel model, IRenderDevice render)
        {
            var mesh = model.ToMesh();

            PositionNormalData[] vertexes = mesh.Vertices.GetData<PositionNormalData>();

            PositionData[] lines = new PositionData[vertexes.Length * 2];

            for (int i = 0; i < vertexes.Length; i++)
            {
                lines[i * 2 + 0] = new PositionData { Position = vertexes[i].Position };
                lines[i * 2 + 1] = new PositionData { Position = vertexes[i].Position + vertexes[i].Normal * 0.1f };
            }

            render.Draw(Basic.Lines(lines));
        }
    }
}
