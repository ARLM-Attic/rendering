using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Maths;
using System.Compilers.Shaders;

namespace System.Rendering
{
    [ShaderType]
    public struct PositionData
    {
        [Position]
        public Vector3 Position;
        public static PositionData Create(Vector3 position)
        {
            PositionData p = new PositionData();
            p.Position = position;
            return p;
        }
    }

    [ShaderType]
    public struct ProjectedData
    {
        [Projected]
        public Vector4 Position;
    }

    [ShaderType]
    public struct ColorData
    {
        [Color]
        public Vector4 Color;
    }

    public struct SecondaryColorData
    {
        [Color(1)]
        public Vector4 Color;
    }

    [ShaderType]
    public struct SurfaceData
    {
        [Position]
        public Vector3 Position;
        [Normal]
        public Vector3 Normal;
        [Tangent]
        public Vector3 Tangent;
        [Binormal]
        public Vector3 Binormal;
        [Coordinates]
        public Vector2 Coordinates;
        [Color(0)]
        public Vector4 Diffuse;
        [Color(1)]
        public Vector3 Specular;
    }

    [ShaderType]
    public struct ProjectedSurfaceData
    {
        [Projected]
        public Vector4 Projected;
        [Position]
        public Vector3 Position;
        [Normal]
        public Vector3 Normal;
        [Tangent]
        public Vector3 Tangent;
        [Binormal]
        public Vector3 Binormal;
        [Coordinates]
        public Vector2 Coordinates;
        [Color(0)]
        public Vector4 Diffuse;
        [Color(1)]
        public Vector3 Specular;
    }

    [ShaderType]
    public struct ColorsData
    {
        [Color(0)]
        public Vector4 Diffuse;
        [Color(1)]
        public Vector3 Specular;
    }

    [ShaderType]
    public struct NormalData
    {
        [Normal]
        public Vector3 Normal;
    }

    [ShaderType]
    public struct CoordinatesData
    {
        [Coordinates]
        public Vector2 Coordinates;
    }

    [ShaderType]
    public struct ColorCoordinatesData
    {
        [Color]
        public Vector4 Color;
        [Coordinates]
        public Vector2 Coordinates;
    }
    [ShaderType]
    public struct SecondaryColorCoordinatesData
    {
        [Color(1)]
        public Vector4 Color;
        [Coordinates]
        public Vector2 Coordinates;
    }



    [ShaderType]
    public struct Coordinates3DData
    {
        [Coordinates]
        public Vector3 Coordinates;
    }

    [ShaderType]
    public struct ColorCoordinates3DData
    {
        [Color]
        public Vector4 Color;
        [Coordinates]
        public Vector3 Coordinates;
    }

    public struct SecondaryColorCoordinates3DData
    {
        [Color(1)]
        public Vector4 Color;
        [Coordinates]
        public Vector3 Coordinates;
    }


    [ShaderType]
    public struct PositionNormalData
    {
        [Position]
        public Vector3 Position;
        [Normal]
        public Vector3 Normal;
        public static PositionNormalData Create(Vector3 position, Vector3 normal)
        {
            return new PositionNormalData { Position = position, Normal = normal };
        }
    }

    [ShaderType]
    public struct ProjectedPositionNormalData
    {
        [Projected]
        public Vector4 Projected;
        [Position]
        public Vector3 Position;
        [Normal]
        public Vector3 Normal;
    }


    [ShaderType]
    public struct PositionColorData
    {
        [Position]
        public Vector3 Position;
        [Color]
        public Vector4 Color;

        public static PositionColorData Create(Vector3 position, Vector4 color)
        {
            return new PositionColorData { Position = position, Color = color };
        }
    }

    [ShaderType]
    public struct PositionDiffuseTextureData
    {
        [Position]
        public Vector3 Position;
        [Color]
        public int Color;
        [Coordinates]
        public Vector2 TextureCoordinates;

        public static PositionDiffuseTextureData Create(Vector3 position, int color, Vector2 coordinates)
        {
            return new PositionDiffuseTextureData { Position = position, Color = color, TextureCoordinates = coordinates };
        }
    }

    [ShaderType]
    public struct PositionNormalColorData
    {
        [Position]
        public Vector3 Position;
        [Normal]
        public Vector3 Normal;
        [Color]
        public Vector4 Color;

        public static PositionNormalColorData Create(Vector3 position, Vector3 normal, Vector4 color)
        {
            return new PositionNormalColorData { Position = position, Normal = normal, Color = color };
        }
    }

    [ShaderType]
    public struct PositionNormalCoordinatesData
    {
        [Position]
        public Vector3 Position;
        [Normal]
        public Vector3 Normal;
        [Coordinates]
        public Vector2 Coordinates;

        public static PositionNormalCoordinatesData Create(Vector3 position, Vector3 normal, Vector2 coordinates)
        {
            return new PositionNormalCoordinatesData { Position = position, Normal = normal, Coordinates = coordinates };
        }
    }

    [ShaderType]
    public struct PositionCoordinatesData
    {
        [Position]
        public Vector3 Position;
        [Coordinates]
        public Vector2 Coordinates;
    }
    [ShaderType]
    public struct PositionCoordinates3DData
    {
        [Position]
        public Vector3 Position;
        [Coordinates]
        public Vector3 Coordinates;
    }

    [ShaderType]
    public struct PositionNormalColorCoordinatesData
    {
        [Position]
        public Vector3 Position;
        [Normal]
        public Vector3 Normal;
        [Color]
        public int Color;
        [Coordinates]
        public Vector2 Coordinates;
    }
}
