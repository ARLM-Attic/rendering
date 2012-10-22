using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Expressions;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Declarations
{
  public class LocalVariableDeclarationAST : VariableDeclarationAST, IVarSetter
  {
    public LocalVariableDeclarationAST()
    {
    }

    public LocalVariableDeclarationAST(int line, int column)
      : base(line, column)
    {
    }

    public LocalVariableDeclarationAST(string name, ExpressionAST initExpression, int line, int column)
      : base(name, line, column)
    {
      InitExpression = initExpression;
    }

    public LocalVariableDeclarationAST(string name, TypeQualifier qualifier, ExpressionAST initExpression, int line, int column)
      : base(name, line, column)
    {
      Qualifier = qualifier;
      InitExpression = initExpression;
    }

    public override void CheckSemantic(SemanticContext context)
    {
      context.MarkErrors();
      TypeSpecifier.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        CheckQualifier(context);
        if (IsArray && SizeExpression != null)
          CheckArrayWithSize(context);
        else if (IsArray && SizeExpression == null)
          CheckArrayWithoutSize(context);
        else
          CheckNonArray(context);
      }
      context.UnMarkErrors();
    }

    private void CheckQualifier(SemanticContext context)
    {
      switch (Qualifier)
      {
        case TypeQualifier.Const:
          if (InitExpression == null)
            context.Errors.Add(new SemanticError("Variables with qualifier 'const' must be initialized at declaration", Line, Column));
          break;

        case TypeQualifier.Attribute:
          if (context.ShaderType != ShaderType.VertexShader)
            context.Errors.Add(new SemanticError("Variables with qualifier 'attribute' only is declared in a vertex shader", Line, Column));
          if (!TypeSpecifier.Type.IsFloatBased() || TypeSpecifier.Type.IsArray() || TypeSpecifier.Type.IsStruct())
            context.Errors.Add(new SemanticError("Variables with qualifier 'attribute' can only be of type 'float', 'floating-point vectors' or 'matrices'", Line, Column));
          if (context.Scope.ScopeType != ScopeType.GlobalScope)
            context.Errors.Add(new SemanticError("Variables with qualifier 'attribute' can only be declared in global scope", Line, Column));
          if (InitExpression != null)
            context.Errors.Add(new SemanticError("Variables with qualifier 'attribute' cannot have init expressions", Line, Column));
          break;

        case TypeQualifier.Uniform:
          if (context.Scope.ScopeType != ScopeType.GlobalScope)
            context.Errors.Add(new SemanticError("Variables with qualifier 'uniform' can only be declared in global scope", Line, Column));
          break;

        case TypeQualifier.Varying:
          if (context.Scope.ScopeType != ScopeType.GlobalScope)
            context.Errors.Add(new SemanticError("Variables with qualifier 'varying' can only be declared in global scope", Line, Column));
          if (!TypeSpecifier.Type.IsFloatBased() || TypeSpecifier.Type.IsStruct())
            context.Errors.Add(new SemanticError("Variables with qualifier 'varying' can only be of type 'float', 'floating-point vectors' or 'matrices' or an array of these", Line, Column));
          break;

        case TypeQualifier.Centroid_Varying:
          if (context.Scope.ScopeType != ScopeType.GlobalScope)
            context.Errors.Add(new SemanticError("Variables with qualifier 'varying' can only be declared in global scope", Line, Column));
          if (TypeSpecifier.Type.IsStruct())
            context.Errors.Add(new SemanticError("Variables with qualifier 'varying' can only be of type 'float', 'floating-point vectors' or 'matrices' or an array of these", Line, Column));
          break;

        case TypeQualifier.Invariant:
          break;

        case TypeQualifier.Invariant_Varying:
          break;

        case TypeQualifier.Invariant_Centroid_Varying:
          break;

        default:
          break;
      }
    }

    private void CheckNonArray(SemanticContext context)
    {
      if (context.Scope.ContainsVariable(Name))
        context.Errors.Add(new VariableRedeclarationError(Name, Line, Column));
      else
      {
        GLSLType varType = TypeSpecifier.Type;
        LocalVariableInfo varInfo = new LocalVariableInfo()
        {
          Name = Name,
          Type = varType,
          Qualifier = Qualifier
        };
        if (InitExpression != null)
          CheckInitExpression(context, varType, varInfo);

        context.Scope.AddVariable(varInfo);
        VarInfo = varInfo;
      }
    }

    private void CheckArrayWithoutSize(SemanticContext context)
    {
      if (context.Scope.ContainsVariable(Name))
        context.Errors.Add(new VariableRedeclarationError(Name, Line, Column));
      else
      {
        GLSLType varType = new ArrayType(TypeSpecifier.Type, null);

        LocalVariableInfo varInfo = new LocalVariableInfo()
        {
          Name = Name,
          Qualifier = Qualifier,
          Type = varType
        };

        // El tratamiento aqui es distinto a como se hace normalmente con los InitExpression
        if (InitExpression != null)
        {
          context.MarkErrors();
          InitExpression.CheckSemantic(context);
          if (!context.CheckForErrors())
          {
            if (!InitExpression.Type.IsArrayOf(TypeSpecifier.Type))
              context.Errors.Add(new CannotImplicitConvertError(InitExpression.Type.Name, varType.Name, InitExpression.Line, InitExpression.Column));
            else
              varType = InitExpression.Type;
            if (InitExpression.IsConstant)
              varInfo.ConstantValue = InitExpression.GetConstantValue();
          }
          context.UnMarkErrors();
        }
        context.Scope.AddVariable(varInfo);
        VarInfo = varInfo;
      }
    }

    private void CheckArrayWithSize(SemanticContext context)
    {
      VariableInfo varInfo;
      if (TryFindRedeclaredVariable(context, out varInfo))
      {
        GLSLType newVarType;
        CheckArraySize(context, TypeSpecifier.Type, out newVarType);
        varInfo.Type = newVarType;

        if (InitExpression != null)
          CheckInitExpression(context, newVarType, varInfo.Cast<LocalVariableInfo>());
        VarInfo = varInfo;
      }
      else
      {
        if (varInfo != null)
          context.Errors.Add(new VariableRedeclarationError(Name, Line, Column));
        else
        {
          GLSLType varType;
          CheckArraySize(context, TypeSpecifier.Type, out varType);

          varInfo = new LocalVariableInfo()
          {
            Name = Name,
            Type = varType,
            Qualifier = Qualifier
          };

          if (InitExpression != null)
            CheckInitExpression(context, varType, varInfo.Cast<LocalVariableInfo>());

          context.Scope.AddVariable(varInfo);
          VarInfo = varInfo;
        }
      }
    }

    private bool TryFindRedeclaredVariable(SemanticContext context, out VariableInfo vInfo)
    {
      vInfo = null;
      if (!context.Scope.ContainsVariable(Name))
        return false;
      vInfo = context.Scope.GetVariableInfo(Name);
      if (!vInfo.Type.IsArray())
        return false;
      ArrayType arrType = vInfo.Type.Cast<ArrayType>();
      return arrType.Size == null && arrType.ElementType.Equals(TypeSpecifier.Type);
    }

    private void CheckInitExpression(SemanticContext context, GLSLType equalsTo, LocalVariableInfo vInfo)
    {
      context.MarkErrors();
      InitExpression.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        if (!InitExpression.Type.Equals(equalsTo))
        {
          if (!InitExpression.Type.ImplicitConvert(equalsTo))
            context.Errors.Add(new CannotImplicitConvertError(InitExpression.Type.Name, equalsTo.Name, Line, Column));
          else
            context.Warnings.Add(new ImplicitConversionWarning(InitExpression.Type.Name, equalsTo.Name, InitExpression.Line, InitExpression.Column));
        }
        if (Qualifier == TypeQualifier.Const)
        {
          if (InitExpression.IsConstant)
            vInfo.ConstantValue = InitExpression.GetConstantValue();
          else
            context.Errors.Add(new SemanticError("Init expression for const variables must be constant", InitExpression.Line, InitExpression.Column));
        }
      }
      context.UnMarkErrors();
    }

    public List<VariableInfo> GetVarsSetted()
    {
      return InitExpression.Is<IVarSetter>() ? 
              InitExpression.Cast<IVarSetter>().GetVarsSetted() : 
              Enumerable.Empty<VariableInfo>().ToList();
    }

    public TypeQualifier Qualifier { get; set; }

    public ExpressionAST InitExpression { get; set; }

  }
}
