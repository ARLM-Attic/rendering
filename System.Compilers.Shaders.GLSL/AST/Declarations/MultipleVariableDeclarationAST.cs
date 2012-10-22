using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Declarations
{
  public class MultipleVariableDeclarationAST : DeclarationAST, IVarSetter
  {
    public MultipleVariableDeclarationAST()
    {
      VarDeclarations = new List<VariableDeclarationAST>();
    }

    public MultipleVariableDeclarationAST(int line, int column)
      : base(line, column)
    {
      VarDeclarations = new List<VariableDeclarationAST>();
    }

    public MultipleVariableDeclarationAST(ITypeSpecifier typeSpecifier, IEnumerable<VariableDeclarationAST> varDecls, int line, int column)
      : base(typeSpecifier, line, column)
    {
      VarDeclarations = new List<VariableDeclarationAST>(varDecls);
    }

    public MultipleVariableDeclarationAST(IEnumerable<VariableDeclarationAST> varDecls, int line, int column)
      : base(line, column)
    {
      VarDeclarations = new List<VariableDeclarationAST>(varDecls);
    }

    public override void CheckSemantic(SemanticContext context)
    {
      ITypeSpecifier tSpecifier = TypeSpecifier;
      if (tSpecifier.Is<ArrayTypeSpecifier>())
        tSpecifier = tSpecifier.Cast<ArrayTypeSpecifier>().TypeSpecifier;

      context.MarkErrors();

      if (tSpecifier.Is<StructDeclarationAST>())
        tSpecifier.CheckSemantic(context);

      tSpecifier = new NamedTypeSpecifier(tSpecifier.Name, tSpecifier.Line, tSpecifier.Column);
      if (!context.CheckForErrors())
      {
        foreach (var varDec in VarDeclarations)
        {
          if (TypeSpecifier.Is<ArrayTypeSpecifier>())
          {
            if (varDec.IsArray)
              context.Errors.Add(new SemanticError("Multidimensional arrays are not allowed", Line, Column));
            else
            {
              varDec.IsArray = true;
              varDec.SizeExpression = TypeSpecifier.Cast<ArrayTypeSpecifier>().SizeExpression;
            }
          }
          if (!context.CheckForErrors())
          {
            if (varDec.Is<LocalVariableDeclarationAST>())
              varDec.Cast<LocalVariableDeclarationAST>().Qualifier = Qualifier;
            varDec.TypeSpecifier = tSpecifier;
            varDec.CheckSemantic(context);
          }
        }
      }
      context.UnMarkErrors();
    }

    public List<VariableInfo> GetVarsSetted()
    {
      List<VariableInfo> varsSetted = new List<VariableInfo>();
      foreach (var varDec in VarDeclarations.Where(vDec => vDec.Is<IVarSetter>()))
        varsSetted.AddRange(varDec.Cast<IVarSetter>().GetVarsSetted());
      return varsSetted;
    }

    public List<VariableDeclarationAST> VarDeclarations { get; private set; }

    public TypeQualifier Qualifier { get; set; }

  }
}
