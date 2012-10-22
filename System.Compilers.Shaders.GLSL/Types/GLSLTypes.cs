using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Types
{
  public static class GLSLTypes
  {
    public static readonly GLSLType IntegerType;
    public static readonly GLSLType FloatType;
    public static readonly GLSLType BoolType;
    public static readonly GLSLType VoidType;
    public static readonly GLSLType FloatVec2Type;
    public static readonly GLSLType FloatVec3Type;
    public static readonly GLSLType FloatVec4Type;
    public static readonly GLSLType IntegerVec2Type;
    public static readonly GLSLType IntegerVec3Type;
    public static readonly GLSLType IntegerVec4Type;
    public static readonly GLSLType BoolVec2Type;
    public static readonly GLSLType BoolVec3Type;
    public static readonly GLSLType BoolVec4Type;
    public static readonly GLSLType Mat2x2Type;
    public static readonly GLSLType Mat2x3Type;
    public static readonly GLSLType Mat2x4Type;
    public static readonly GLSLType Mat3x2Type;
    public static readonly GLSLType Mat3x3Type;
    public static readonly GLSLType Mat3x4Type;
    public static readonly GLSLType Mat4x2Type;
    public static readonly GLSLType Mat4x3Type;
    public static readonly GLSLType Mat4x4Type;
    public static readonly GLSLType Sampler1DType;
    public static readonly GLSLType Sampler2DType;
    public static readonly GLSLType Sampler3DType;
    public static readonly GLSLType SamplerCubeType;
    public static readonly GLSLType Sampler1DShadowType;
    public static readonly GLSLType Sampler2DShadowType;

    private static Dictionary<string, GLSLType> types;

    static GLSLTypes()
    {
      IntegerType = new IntegerType();
      FloatType = new FloatType();
      BoolType = new BoolType();
      VoidType = new VoidType();
      FloatVec2Type = new VecType(VecType.FloatVec2TypeName, GLSLTypes.FloatType, 2);
      FloatVec3Type = new VecType(VecType.FloatVec3TypeName, GLSLTypes.FloatType, 3);
      FloatVec4Type = new VecType(VecType.FloatVec4TypeName, GLSLTypes.FloatType, 4);
      IntegerVec2Type = new VecType(VecType.IntegerVec2TypeName, GLSLTypes.IntegerType, 2);
      IntegerVec3Type = new VecType(VecType.IntegerVec3TypeName, GLSLTypes.IntegerType, 3);
      IntegerVec4Type = new VecType(VecType.IntegerVec4TypeName, GLSLTypes.IntegerType, 4);
      BoolVec2Type = new VecType(VecType.BoolVec2TypeName, GLSLTypes.BoolType, 2);
      BoolVec3Type = new VecType(VecType.BoolVec3TypeName, GLSLTypes.BoolType, 3);
      BoolVec4Type = new VecType(VecType.BoolVec4TypeName, GLSLTypes.BoolType, 4);
      Mat2x2Type = new MatType(MatType.Mat2x2TypeName, GLSLTypes.FloatType, 2, 2);
      Mat2x3Type = new MatType(MatType.Mat2x3TypeName, GLSLTypes.FloatType, 2, 3);
      Mat2x4Type = new MatType(MatType.Mat2x4TypeName, GLSLTypes.FloatType, 2, 4);
      Mat3x2Type = new MatType(MatType.Mat3x2TypeName, GLSLTypes.FloatType, 3, 2);
      Mat3x3Type = new MatType(MatType.Mat3x3TypeName, GLSLTypes.FloatType, 3, 3);
      Mat3x4Type = new MatType(MatType.Mat3x4TypeName, GLSLTypes.FloatType, 3, 4);
      Mat4x2Type = new MatType(MatType.Mat4x2TypeName, GLSLTypes.FloatType, 4, 2);
      Mat4x3Type = new MatType(MatType.Mat4x3TypeName, GLSLTypes.FloatType, 4, 3);
      Mat4x4Type = new MatType(MatType.Mat4x4TypeName, GLSLTypes.FloatType, 4, 4);
      Sampler1DType = new Sampler1D();
      Sampler2DType = new Sampler2D();
      Sampler3DType = new Sampler3D();
      SamplerCubeType = new SamplerCube();
      Sampler1DShadowType = new Sampler1DShadow();
      Sampler2DShadowType = new Sampler2DShadow();

      Reset();
    }

    public static GLSLType GetTypeByName(string name)
    {
      GLSLType type;
      return types.TryGetValue(name, out type) ? type : null;
    }

    public static GLSLType GetTypeByCode(GLSLTypeCode typeCode)
    {
      switch (typeCode)
      {
        case GLSLTypeCode.None:
        case GLSLTypeCode.Struct:
        case GLSLTypeCode.Array:
          return null;
        default:
          break;
      } 
      foreach (var type in types.Values)
      {
        if (type.GetTypeCode() == typeCode)
          return type;
      }
      return null;
    }

    public static bool RegisterType(GLSLType type)
    {
      if (types.ContainsKey(type.Name))
        return false;
      types[type.Name] = type;
      return true;
    }

    public static void Reset()
    {
      types = new Dictionary<string, GLSLType>();
      types[IntegerType.Name] = IntegerType;
      types[FloatType.Name] = FloatType;
      types[BoolType.Name] = BoolType;
      types[VoidType.Name] = VoidType;
      types[FloatVec2Type.Name] = FloatVec2Type;
      types[FloatVec3Type.Name] = FloatVec3Type;
      types[FloatVec4Type.Name] = FloatVec4Type;
      types[IntegerVec2Type.Name] = IntegerVec2Type;
      types[IntegerVec3Type.Name] = IntegerVec3Type;
      types[IntegerVec4Type.Name] = IntegerVec4Type;
      types[BoolVec2Type.Name] = BoolVec2Type;
      types[BoolVec3Type.Name] = BoolVec3Type;
      types[BoolVec4Type.Name] = BoolVec4Type;
      types[MatType.Mat2x2ShortTypeName] = Mat2x2Type;
      types[Mat2x2Type.Name] = Mat2x2Type;
      types[Mat2x3Type.Name] = Mat2x3Type;
      types[Mat2x4Type.Name] = Mat2x4Type;
      types[Mat3x2Type.Name] = Mat3x2Type;
      types[MatType.Mat3x3ShortTypeName] = Mat3x3Type;
      types[Mat3x3Type.Name] = Mat3x3Type;
      types[Mat3x4Type.Name] = Mat3x4Type;
      types[Mat4x2Type.Name] = Mat4x2Type;
      types[Mat4x3Type.Name] = Mat4x3Type;
      types[MatType.Mat4x4ShortTypeName] = Mat4x4Type;
      types[Mat4x4Type.Name] = Mat4x4Type;
      types[Sampler1DType.Name] = Sampler1DType;
      types[Sampler2DType.Name] = Sampler2DType;
      types[Sampler3DType.Name] = Sampler3DType;
      types[SamplerCubeType.Name] = SamplerCubeType;
      types[Sampler1DShadowType.Name] = Sampler1DShadowType;
      types[Sampler2DShadowType.Name] = Sampler2DShadowType;
    }
  }
}
