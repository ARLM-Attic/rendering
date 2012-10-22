using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Declarations;
using GLSLCompiler.Utils;

namespace GLSLCompiler.Types
{
  public class ArrayType : PrimitiveType
  {
    public ArrayType(GLSLType elementType, int? size)
      : base("{0}[{1}]".Fmt(elementType.Name, size))
    {
      ElementType = elementType;
      Size = size;
    }

    public ArrayType(string name, GLSLType elementType, int? size)
      : base(name)
    {
      ElementType = elementType;
      Size = size;
    }

    public override GLSLTypeCode GetTypeCode()
    {
      return GLSLTypeCode.Array;
    }

    public override bool Equals(GLSLType other)
    {
      ArrayType arrType = other as ArrayType;
      if(arrType == null)
        return false;
      return Size != null && arrType.Size != null && 
        Size == arrType.Size && ElementType.Equals(arrType.ElementType);
    }

    public override bool ImplicitConvert(GLSLType type)
    {
      return Equals(type);
    }

    public override int GetNumberOfComponents()
    {
      return Size != null ? Size.Value : -1;
    }

    public override GLSLType GetTypeBase()
    {
      return ElementType.GetTypeBase();
    }

    public override IEnumerable<GLSLType> GetFlatTypes()
    {
      int numberOfComponents = GetNumberOfComponents();
      for (int i = 0; i < numberOfComponents; i++)
        yield return ElementType;
    }

    public virtual bool IsInRange(int index)
    {
      return Size != null && index >= 0 && index < Size;
    }

    public virtual int GetLength(int dimension)
    {
      if (dimension != 0 || Size != null)
        throw new InvalidOperationException();
      return Size.Value;
    }

    public static string GetArrayTypeName(string baseTypeName, int arraySize)
    {
      return "{0}[{1}]".Fmt(baseTypeName, arraySize);
    }

    public GLSLType ElementType { get; protected set; }

    public int? Size { get; internal set; }

    
  }
}
