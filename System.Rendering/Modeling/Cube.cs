#define FLOAT

#if DECIMAL
using FLOATINGTYPE = System.Decimal;
#endif
#if DOUBLE
using FLOATINGTYPE = System.Double;
#endif
#if FLOAT
using FLOATINGTYPE = System.Single;
#endif

using vec3 = System.Maths.Vector3<float>;
using vec2 = System.Maths.Vector2<float>;

using System;
using System.Collections.Generic;
using System.Text;
using CubeVertex = System.Rendering.PositionNormalCoordinatesData;

namespace System.Rendering.Modeling
{

    class Cube : Mesh
    {
        public Cube()
            : base(
            (VertexBuffer)new CubeVertex[] { 
                /*Cara Front*/
                new CubeVertex { Position = new vec3 (0,0,0), Normal = new vec3 (0,0,-1), Coordinates=new vec2 (0,1)  }, //0
                new CubeVertex { Position = new vec3 (1,0,0), Normal = new vec3 (0,0,-1), Coordinates=new vec2 (1,1)  }, //1
                new CubeVertex { Position = new vec3 (1,1,0), Normal = new vec3 (0,0,-1), Coordinates=new vec2 (1,0)  }, //2
                new CubeVertex { Position = new vec3 (0,1,0), Normal = new vec3 (0,0,-1), Coordinates=new vec2 (0,0)  }, //3
                /*Cara Back*/
                new CubeVertex { Position = new vec3 (0,0,1), Normal = new vec3 (0,0,1), Coordinates=new vec2 (0,1)  }, //4
                new CubeVertex { Position = new vec3 (1,0,1), Normal = new vec3 (0,0,1), Coordinates=new vec2 (1,1)  }, //5
                new CubeVertex { Position = new vec3 (1,1,1), Normal = new vec3 (0,0,1), Coordinates=new vec2 (1,0)  }, //6
                new CubeVertex { Position = new vec3 (0,1,1), Normal = new vec3 (0,0,1), Coordinates=new vec2 (0,0)  }, //7
                /*Cara Bottom*/
                new CubeVertex { Position = new vec3 (0,0,0), Normal = new vec3 (0,-1,0), Coordinates=new vec2 (0,1)  }, //8
                new CubeVertex { Position = new vec3 (1,0,0), Normal = new vec3 (0,-1,0), Coordinates=new vec2 (1,1)  }, //9
                new CubeVertex { Position = new vec3 (1,0,1), Normal = new vec3 (0,-1,0), Coordinates=new vec2 (1,0)  }, //10
                new CubeVertex { Position = new vec3 (0,0,1), Normal = new vec3 (0,-1,0), Coordinates=new vec2 (0,0)  }, //11
                /*Cara Top*/
                new CubeVertex { Position = new vec3 (0,1,0), Normal = new vec3 (0,1,0), Coordinates=new vec2 (0,1)  }, //12
                new CubeVertex { Position = new vec3 (1,1,0), Normal = new vec3 (0,1,0), Coordinates=new vec2 (1,1)  }, //13
                new CubeVertex { Position = new vec3 (1,1,1), Normal = new vec3 (0,1,0), Coordinates=new vec2 (1,0)  }, //14
                new CubeVertex { Position = new vec3 (0,1,1), Normal = new vec3 (0,1,0), Coordinates=new vec2 (0,0)  }, //15
                /*Cara Left*/
                new CubeVertex { Position = new vec3 (0,0,0), Normal = new vec3 (-1,0,0), Coordinates=new vec2 (0,1)  }, //16
                new CubeVertex { Position = new vec3 (0,1,0), Normal = new vec3 (-1,0,0), Coordinates=new vec2 (1,1)  }, //17
                new CubeVertex { Position = new vec3 (0,1,1), Normal = new vec3 (-1,0,0), Coordinates=new vec2 (1,0)  }, //18
                new CubeVertex { Position = new vec3 (0,0,1), Normal = new vec3 (-1,0,0), Coordinates=new vec2 (0,0)  }, //19
                /*Cara Right*/
                new CubeVertex { Position = new vec3 (1,0,0), Normal = new vec3 (1,0,0), Coordinates=new vec2 (0,1)  }, //20
                new CubeVertex { Position = new vec3 (1,1,0), Normal = new vec3 (1,0,0), Coordinates=new vec2 (1,1)  }, //21
                new CubeVertex { Position = new vec3 (1,1,1), Normal = new vec3 (1,0,0), Coordinates=new vec2 (1,0)  }, //22
                new CubeVertex { Position = new vec3 (1,0,1), Normal = new vec3 (1,0,0), Coordinates=new vec2 (0,0)  } //23
            }, 
            (IndexBuffer) new ushort[] { 
                0,1,2,0,2,3, // Cara front
                4,6,5,4,7,6, // Cara back
                8,10,9,8,11,10, // Cara bottom
                12,13,14,12,14,15, // Cara top
                16,17,18,16,18,19, // Cara left
                20,22,21,20,23,22, // cara right
            })
        {
        }
    }

}
