using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Utils;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public class ExpressionListExpressionAST : NaryExpressionAST, IVarSetter
  {
    public ExpressionListExpressionAST()
    {
    }

    public ExpressionListExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public ExpressionListExpressionAST(IEnumerable<ExpressionAST> exprs, int line, int column)
      : base(exprs, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      int count = Expressions.Count;
      if (count > 0)
      {
        for (int i = 0; i < count - 1; i++)
          Expressions[i].CheckSemantic(context);

        context.MarkErrors();
        Expressions[count - 1].CheckSemantic(context);
        if (!context.CheckForErrors())
          Type = Expressions[count - 1].Type;
        context.UnMarkErrors();
      }
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return Expressions.Count > 0 ? Expressions.Last().GetConstantValueInternal() : null;
    }

    public List<VariableInfo> GetVarsSetted()
    {
      List<VariableInfo> varsSetted = new List<VariableInfo>();
      foreach (var expr in Expressions.Where(e => e.Is<IVarSetter>()))
        varsSetted.AddRange(expr.Cast<IVarSetter>().GetVarsSetted());
      return varsSetted;
    }

    public override bool IsConstant
    {
      get { return Expressions.Count > 0 ? Expressions.Last().IsConstant : false; }
    }

    public List<ExpressionAST> Expressions
    {
      get { return Operands; }
    }

  }
}
