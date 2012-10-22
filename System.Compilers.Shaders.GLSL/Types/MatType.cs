using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Utils;

namespace GLSLCompiler.Types
{
  public sealed class MatType : ArrayType
  {
    public const string Mat2x2ShortTypeName = "mat2";
    public const string Mat2x2TypeName = "mat2x2";
    public const string Mat2x3TypeName = "mat2x3";
    public const string Mat2x4TypeName = "mat2x4";
    public const string Mat3x2TypeName = "mat3x2";
    public const string Mat3x3ShortTypeName = "mat3";
    public const string Mat3x3TypeName = "mat3x3";
    public const string Mat3x4TypeName = "mat3x4";
    public const string Mat4x2TypeName = "mat4x2";
    public const string Mat4x3TypeName = "mat4x3";
    public const string Mat4x4ShortTypeName = "mat4";
    public const string Mat4x4TypeName = "mat4x4";

    public MatType(string name, GLSLType elementType, int columns, int rows)
      : base(name, elementType, columns * rows)
    {
      Columns = columns;
      Rows = rows;
    }

    public override GLSLTypeCode GetTypeCode()
    {
      string typeCodeString = "Mat{0}x{1}".Fmt(Columns, Rows);
      if (Enum.IsDefined(typeof(GLSLTypeCode), typeCodeString))
        return (GLSLTypeCode)Enum.Parse(typeof(GLSLTypeCode), typeCodeString);
      return GLSLTypeCode.None;
    }

    public override bool Equals(GLSLType other)
    {
      MatType matType = other as MatType;
      if (matType == null)
        return false;
      return ElementType.Equals(matType.ElementType) && Columns == matType.Columns && Rows == matType.Rows;
    }

    public override int GetLength(int dimension)
    {
      if (dimension < 0 || dimension > 2)
        throw new InvalidOperationException();
      return dimension == 0 ? Columns : Rows;
    }

    public override bool IsInRange(int index)
    {
      return index >= 0 && index < Columns;
    }

    public int Columns { get; private set; }

    public int Rows { get; private set; }

    public new int Size
    {
      get { return base.Size.Value; }
    }
  }
}
