using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Expressions;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST
{
  public class ArrayTypeSpecifier : BaseAST, ITypeSpecifier
  {
    public ArrayTypeSpecifier()
    {
      
    }

    public ArrayTypeSpecifier(int line, int column)
      : base(line, column)
    {
    }

    public ArrayTypeSpecifier(ITypeSpecifier typeSpecifier, ExpressionAST size, int line, int column)
      : base(line, column)
    {
      TypeSpecifier = typeSpecifier;
      SizeExpression = size;
    }

    public override void CheckSemantic(SemanticContext context)
    {
      if (TypeSpecifier.Is<ArrayTypeSpecifier>())
        context.Errors.Add(new SemanticError("Multidimensional arrays are not allowed", Line, Column));
      else
      {
        context.MarkErrors();
        TypeSpecifier.CheckSemantic(context);
        if (!context.CheckForErrors())
        {
          if (SizeExpression == null)
          {
            ArrayType arrType = new ArrayType(Name, TypeSpecifier.Type, null);
            TypeInfo newTypeInfo = new TypeInfo() { Name = arrType.Name, Type = arrType };
            Type = newTypeInfo.Type;
          }
          else
          {
            context.MarkErrors();
            SizeExpression.CheckSemantic(context);
            if (!context.CheckForErrors())
            {
              if (!SizeExpression.IsConstant || !SizeExpression.Type.IsInteger())
                context.Errors.Add(new SemanticError("Size expression of an array must be a constant integer value", SizeExpression.Line, SizeExpression.Column));
              else
              {
                int sizeValue = SizeExpression.GetConstantValue<int>();
                if (sizeValue < 0)
                  context.Errors.Add(new SemanticError("Size expression of an array must be greater or equal than zero", SizeExpression.Line, SizeExpression.Column));

                TypeInfo newTypeInfo;
                if (!context.Scope.TryGetTypeInfo(Name, out newTypeInfo))
                {
                  ArrayType arrType = new ArrayType(Name, TypeSpecifier.Type, sizeValue);
                  newTypeInfo = new TypeInfo() { Name = arrType.Name, Type = arrType };
                  context.Scope.AddType(newTypeInfo);
                }
                Type = newTypeInfo.Type;
              }
            }
            context.UnMarkErrors();
          }
        }
        context.UnMarkErrors();
      }
    }

    public string Name 
    {
      get
      {
        if (SizeExpression != null && SizeExpression.IsConstant && SizeExpression.Type.IsInteger())
          return "{0}[{1}]".Fmt(TypeSpecifier.Name, SizeExpression.GetConstantValue<int>());
        return "{0}[]".Fmt(TypeSpecifier.Name);
      }
    }

    public ITypeSpecifier TypeSpecifier { get; set; }

    public ExpressionAST SizeExpression { get; set; }

    public GLSLType Type { get; set; }

  }
}
