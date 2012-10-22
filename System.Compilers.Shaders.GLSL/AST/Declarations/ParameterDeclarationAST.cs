using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.AST.Expressions;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Declarations
{
  public class ParameterDeclarationAST : VariableDeclarationAST
  {
    public ParameterDeclarationAST()
    {
    }

    public ParameterDeclarationAST(int line, int column)
      : base(line, column)
    {
    }

    public ParameterDeclarationAST(string name, ITypeSpecifier typeSpecifier, ParamQualifier qualifier, ExpressionAST defaultExpression, int line, int column)
      : base(name, typeSpecifier, line, column)
    {
      Qualifier = qualifier;
      DefaultExpression = defaultExpression;
    }

    public override void CheckSemantic(SemanticContext context)
    {
      context.MarkErrors();
      TypeSpecifier.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        if (context.Scope.ContainsVariable(Name))
          context.Errors.Add(new VariableRedeclarationError(Name, Line, Column));
        else
        {
          CheckQualifier(context);

          GLSLType paramType;
          if (IsArray)
            CheckArraySize(context, TypeSpecifier.Type, out paramType);
          else
            paramType = TypeSpecifier.Type;

          ParamInfo paramInfo = new ParamInfo()
          {
            Name = Name,
            Type = paramType,
            Qualifier = Qualifier
          };

          if (DefaultExpression != null)
          {
            if (Qualifier == ParamQualifier.InOut || Qualifier == ParamQualifier.Out)
              context.Errors.Add(new SemanticError("Parameters with qualifier 'inout' or 'out' cannot have default values.", Line, Column));
            else
            {
              context.MarkErrors();
              CheckDefaultExpression(context, paramInfo);
              if (!context.CheckForErrors())
                paramInfo.DefaultValue = DefaultExpression.GetConstantValue();
              context.UnMarkErrors();
            }
          }
          context.Scope.AddVariable(paramInfo);
          VarInfo = paramInfo;
        }
      }
      context.UnMarkErrors();
    }

    protected void CheckQualifier(SemanticContext context)
    {
    }

    protected void CheckDefaultExpression(SemanticContext context, ParamInfo pInfo)
    {
      context.MarkErrors();
      DefaultExpression.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        if (!DefaultExpression.IsConstant)
          context.Errors.Add(new SemanticError("Default values for function parameters must be constants", Line, Column));
        else
        {
          if (!DefaultExpression.Type.Equals(TypeSpecifier.Type))
          {
            if (DefaultExpression.Type.ImplicitConvert(TypeSpecifier.Type))
              context.Warnings.Add(new ImplicitConversionWarning(DefaultExpression.Type.Name, TypeSpecifier.Type.Name, DefaultExpression.Line, DefaultExpression.Column));
            else
              context.Errors.Add(new CannotImplicitConvertError(DefaultExpression.Type.Name, TypeSpecifier.Type.Name, DefaultExpression.Line, DefaultExpression.Column));
          }
          pInfo.DefaultValue = DefaultExpression.GetConstantValue();
        }
      }
      context.UnMarkErrors();
    }

    public ParamQualifier Qualifier { get; set; }

    public ExpressionAST DefaultExpression { get; set; }

  }
}
