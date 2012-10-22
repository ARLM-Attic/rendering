using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Expressions;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Declarations
{
  public abstract class VariableDeclarationAST : DeclarationAST
  {
    public VariableDeclarationAST()
    {
    }

    public VariableDeclarationAST(int line, int column)
      : base(line, column)
    {
    }

    public VariableDeclarationAST(string name, int line, int column)
      : base(line, column)
    {
      Name = name;
    }

    public VariableDeclarationAST(string name, ITypeSpecifier typeSpecifier, int line, int column)
      : base(typeSpecifier, line, column)
    {
      Name = name;
    }

    protected virtual void CheckArraySize(SemanticContext context, GLSLType elementType, out GLSLType varType)
    {
      varType = null;
      context.MarkErrors();
      SizeExpression.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        if (!SizeExpression.IsConstant || !SizeExpression.Type.IsInteger())
          context.Errors.Add(new NonConstantIntegerSizeExpressionError(SizeExpression.Line, SizeExpression.Column));
        else
        {
          int sizeValue = SizeExpression.GetConstantValue<int>();
          if (sizeValue < 0)
            context.Errors.Add(new SemanticError("Size expression of an array must be greater or equal than zero", SizeExpression.Line, SizeExpression.Column));

          string newTypeName = "{0}[{1}]".Fmt(elementType.Name, sizeValue);
          TypeInfo tInfo;
          if (!context.Scope.TryGetTypeInfo(newTypeName, out tInfo))
          {
            ArrayType newType = new ArrayType(newTypeName, TypeSpecifier.Type, sizeValue);
            tInfo = new TypeInfo() { Name = newTypeName, Type = newType };
            context.Scope.AddType(tInfo);
          }
          varType = tInfo.Type;
        }
      }
      context.UnMarkErrors();
    }

    public string Name { get; set; }

    public ExpressionAST SizeExpression { get; set; }

    public bool IsArray { get; set; }

    public VariableInfo VarInfo { get; set; }
  }
}
