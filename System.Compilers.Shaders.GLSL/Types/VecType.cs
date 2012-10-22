using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Utils;

namespace GLSLCompiler.Types
{
  public sealed class VecType : ArrayType
  {
    public const string FloatVec2TypeName = "vec2";
    public const string FloatVec3TypeName = "vec3";
    public const string FloatVec4TypeName = "vec4";
    public const string IntegerVec2TypeName = "ivec2";
    public const string IntegerVec3TypeName = "ivec3";
    public const string IntegerVec4TypeName = "ivec4";
    public const string BoolVec2TypeName = "bvec2";
    public const string BoolVec3TypeName = "bvec3";
    public const string BoolVec4TypeName = "bvec4";

    HashSet<char> components = new HashSet<char>() { 'x', 'y', 'z', 'w', 'r', 'g', 'b', 'a', 's', 't', 'p', 'q' };

    public VecType(string name, GLSLType elementType, int size)
      : base(name, elementType, size)
    {
    }

    public override GLSLTypeCode GetTypeCode()
    {
      GLSLTypeCode typeCode = ElementType.GetTypeCode();
      string typeCodeString = "{0}Vec{1}".Fmt(typeCode.ToString(), Size);
      if (Enum.IsDefined(typeof(GLSLTypeCode), typeCodeString))
        return (GLSLTypeCode)Enum.Parse(typeof(GLSLTypeCode), typeCodeString);
      return GLSLTypeCode.None;
    }

    public override bool ImplicitConvert(GLSLType type)
    {
      VecType vecType = type as VecType;
      if (vecType == null)
        return false;
      return Size == vecType.Size && ElementType.ImplicitConvert(vecType.ElementType);
    }

    internal bool GroupContains(char component, VecFieldGroups group)
    {
      return group.ToString().Contains(char.ToUpper(component));
    }

    internal VecFieldGroups GetGroupFromComponent(char component)
    {
      string[] names = Enum.GetNames(typeof(VecFieldGroups));
      char upper = char.ToUpper(component);
      foreach (string name in names)
      {
        if (name.Contains(upper))
          return Enum.Parse(typeof(VecFieldGroups), name).Cast<VecFieldGroups>();
      }
      return (VecFieldGroups)0;
    }

    internal bool IsLargeEnough(char component)
    {
      switch (component)
      {
        case 'z':
        case 'b':
        case 'p':
          return Size >= 3;
        case 'w':
        case 'a':
        case 'q':
          return Size >= 4;
        default:
          break;
      }
      return components.Contains(component);
    }

    internal bool IsValidComponent(char component)
    {
      return components.Contains(component);
    }

    public override int GetLength(int dimension)
    {
      if (dimension != 0)
        throw new InvalidOperationException();
      return Size;
    }

    public override bool IsInRange(int index)
    {
      return index >= 0 && index < Size;
    }

    public new int Size
    {
      get { return base.Size.Value; }
    }

    public enum VecFieldGroups
    {
      XYZW = 1,
      RGBA = 2,
      STPQ = 3
    }
  }
}
