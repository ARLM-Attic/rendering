using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Types
{
  public enum GLSLTypeCode
  {
    None,
    Integer,
    Float,
    Bool,
    FloatVec2,
    FloatVec3,
    FloatVec4,
    IntegerVec2,
    IntegerVec3,
    IntegerVec4,
    BoolVec2,
    BoolVec3,
    BoolVec4,
    Mat2x2,
    Mat2x3,
    Mat2x4,
    Mat3x2,
    Mat3x3,
    Mat3x4,
    Mat4x2,
    Mat4x3,
    Mat4x4,
    Sampler1D,
    Sampler2D,
    Sampler3D,
    SamplerCube,
    Sampler1DShadow,
    Sampler2DShadow,
    Struct,
    Array,
    Void,
  }
}
