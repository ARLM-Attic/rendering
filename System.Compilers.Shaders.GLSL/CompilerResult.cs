using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Utils;
using GLSLCompiler.Types;

namespace GLSLCompiler
{
  public abstract class CompilerResult
  {
    public CompilerResult(int line, int column)
      : this("", line, column)
    {
    }

    public CompilerResult(string msg, int line, int column)
    {
      message = msg;
      Line = line;
      Column = column;
    }

    protected virtual string GetMessage()
    {
      return message;
    }

    public override string ToString()
    {
      return Message;
    }

    protected string message;
    public virtual string Message 
    {
      get { return "{0}:({1},{2})".Fmt(GetMessage(), Line, Column); }
      set { message = value; }
    }

    public int Line { get; set; }

    public int Column { get; set; }
  }

  #region [ Errors ]

  public class CompilerError : CompilerResult
  {
    public CompilerError(int line, int column)
      : this("", line, column)
    {
    }

    public CompilerError(string msg, int line, int column)
      : base(msg, line, column)
    {
    }
  }

  public class SyntacticError : CompilerError
  {
    public SyntacticError(int line, int column)
      : this("", line, column)
    {
    }

    public SyntacticError(string msg, int line, int column)
      : base(msg, line, column)
    {
    }

    public override sealed string Message
    {
      get
      {
        return "Syntatic Error: {0}:({1},{2})".Fmt(GetMessage(), Line, Column);
      }
    }
  }

  public class SemanticError : CompilerError
  {
    public SemanticError(int line, int column)
      : this("", line, column)
    {
    }

    public SemanticError(string msg, int line, int column)
      : base(msg, line, column)
    {
    }

    public override sealed string Message
    {
      get
      {
        return "Semantic Error: {0}:({1},{2})".Fmt(GetMessage(), Line, Column);
      }
    }
  }

  public class CannotImplicitConvertError : SemanticError
  {
    public CannotImplicitConvertError(string tFrom, string tTo, int line, int column)
      : base(line, column)
    {
      TypeFrom = tFrom;
      TypeTo = tTo;
    }

    protected override string GetMessage()
    {
      return "Cannot implicit convert type '{0}' to type '{1}'".Fmt(TypeFrom, TypeTo);
    }

    public string TypeFrom { get; set; }

    public string TypeTo { get; set; }
  }

  public class CannotAppliedOperatorError : SemanticError
  {
    public CannotAppliedOperatorError(string op, string t1, int line, int column)
      : base(line, column)
    {
      Type1 = t1;
      Operator = op;
    }

    public CannotAppliedOperatorError(string op, string t1, string t2, int line, int column)
      : base(line, column)
    {
      Type1 = t1;
      Type2 = t2;
      Operator = op;
    }

    protected override string GetMessage()
    {
      if (Type2 != null)
        return "Operator '{0}' cannot be applied to operands of type '{1}' and '{2}'".Fmt(Operator, Type1, Type2);
      return "Operator '{0}' cannot be applied to operand of type '{1}'".Fmt(Operator, Type1);
    }

    public string Type1 { get; set; }

    public string Type2 { get; set; }

    public string Operator { get; set; }
  }

  public class NoMainFunctionDefinedError : SemanticError
  {
    public NoMainFunctionDefinedError()
      : base(0, 0)
    {
    }

    protected override string GetMessage()
    {
      return "No 'main' function defined";
      //return "No 'main' function defined. GLSL requires a function 'main' without parameters and 'void' as return type as entry point";
    }
  }

  public class TypeNotDeclaredError : SemanticError
  {
    public TypeNotDeclaredError(string name, int line, int column)
      : base(line, column)
    {
      TypeName = name;
    }

    protected override string GetMessage()
    {
      return "The specified type wasn't declared: '{0}'".Fmt(TypeName);
    }

    public string TypeName { get; set; }
  }

  public class VariableNotDeclaredError : SemanticError
  {
    public VariableNotDeclaredError(string name, int line, int column)
      : base(line, column)
    {
      TypeName = name;
    }

    protected override string GetMessage()
    {
      return "The specified variable wasn't declared: '{0}'".Fmt(TypeName);
    }

    public string TypeName { get; set; }
  }

  public class VariableRedeclarationError : SemanticError
  {
    public VariableRedeclarationError(string name, int line, int column)
      : base(line, column)
    {
      VariableName = name;
    }

    protected override string GetMessage()
    {
      return "Another variable was declared previously in this scope with the same name: '{0}'".Fmt(VariableName);
    }

    public string VariableName { get; set; }
  }

  public class TypeRedeclarationError : SemanticError
  {
    public TypeRedeclarationError(string name, int line, int column)
      : base(line, column)
    {
      TypeName = name;
    }

    protected override string GetMessage()
    {
      return "Another type was declared previously in this scope with the same name: '{0}'".Fmt(TypeName);
    }

    public string TypeName { get; set; }
  }

  public class FunctionNotDeclaredError : SemanticError
  {
    public FunctionNotDeclaredError(string name, int line, int column)
      : base(line, column)
    {
      FunctionName = name;
    }

    protected override string GetMessage()
    {
      return "The specified function wasn't declared: '{0}'".Fmt(FunctionName);
    }

    public string FunctionName { get; set; }
  }

  public class FunctionRedeclarationError : SemanticError
  {
    public FunctionRedeclarationError(string name, int line, int column)
      : base(line, column)
    {
      FunctionName = name;
    }

    protected override string GetMessage()
    {
      return "Another function was declared previously in this scope with the same name: '{0}'".Fmt(FunctionName);
    }

    public string FunctionName { get; set; }
  }

  public class NotAllPathReturnError : SemanticError
  {
    public NotAllPathReturnError(string fName, int line, int column)
      : base(line, column)
    {
      FunctionName = fName;
    }

    protected override string GetMessage()
    {
      return "Not all code paths return a value: '{0}'".Fmt(FunctionName);
    }

    public string FunctionName { get; set; }
  }

  public class FunctionRedefinitionError : SemanticError
  {
    public FunctionRedefinitionError(string fName, int line, int column)
      : base(line, column)
    {
      FunctionName = fName;
    }

    protected override string GetMessage()
    {
      return "Multiple function definition is not allowed: '{0}'".Fmt(FunctionName);
    }

    public string FunctionName { get; set; }
  }

  public class IndexOutOfRangeError : SemanticError
  {
    public IndexOutOfRangeError(int low, int high, int line, int column)
      : base(line, column)
    {
      Low = low;
      High = high;
    }

    protected override string GetMessage()
    {
      return "Expression must be greater or equal than '{0}' and less than '{1}'".Fmt(Low, High);
    }

    public int Low { get; set; }

    public int High { get; set; }
  }

  public class TypeDoesNotContainsFieldError : SemanticError
  {
    public TypeDoesNotContainsFieldError(string type, string field, int line, int column)
      : base(line, column)
    {
      Type = type;
      Field = field;
    }

    protected override string GetMessage()
    {
      return "Type '{0}' doesn't contains field named '{1}'".Fmt(Type, Field);
    }

    public string Type { get; set; }

    public string Field { get; set; }
  }

  public class NoEnoughArgumentsError : SemanticError
  {
    public NoEnoughArgumentsError(string typeName, int line, int column)
      : base(line, column)
    {
      TypeName = typeName;
    }

    protected override string GetMessage()
    {
      return "No enough arguments for constructor of type '{0}'".Fmt(TypeName);
    }

    public string TypeName { get; set; }
  }

  public class TooManyArgumentsError : SemanticError
  {
    public TooManyArgumentsError(string typeName, int line, int column)
      : base(line, column)
    {
      TypeName = typeName;
    }

    protected override string GetMessage()
    {
      return "Too many arguments for constructor of type '{0}'".Fmt(TypeName);
    }

    public string TypeName { get; set; }
  }

  public class InvalidArgumentsError : SemanticError
  {
    public InvalidArgumentsError(string typeName, int line, int column)
      : base(line, column)
    {
      TypeName = typeName;
    }

    protected override string GetMessage()
    {
      return "Invalid arguments for constructor of type '{0}'".Fmt(TypeName);
    }

    public string TypeName { get; set; }
  }

  public class StatementNotAllowedError : SemanticError
  {
    public StatementNotAllowedError(string stmt, int line, int column)
      : base(line, column)
    {
      Statement = stmt;
    }

    protected override string GetMessage()
    {
      return "Statement '{0}' is not allowed in this context".Fmt(Statement);
    }

    public string Statement { get; set; }
  }

  public class NonConstantIntegerSizeExpressionError : SemanticError
  {
    public NonConstantIntegerSizeExpressionError(int line, int column)
      : base(line, column)
    {
    }

    protected override string GetMessage()
    {
      return "Size expression must be a constant integer value";
    }
  }

  public class CannotApplyIndexingError : SemanticError
  {
    public CannotApplyIndexingError(string type, int line, int column) : base(line, column)
    {
      TypeName = type;
    }

    protected override string GetMessage()
    {
      return "Cannot apply indexing with '[]' to an expression of type {0}".Fmt(TypeName);
    }

    public string TypeName { get; set; }
  }

  public class AmbiguityFunctionCallError : SemanticError
  {
    public AmbiguityFunctionCallError(IEnumerable<string> functionNames, int line, int column)
      : base(line, column)
    {
      FunctionNames = functionNames;
    }

    protected override string GetMessage()
    {
      return "Ambiguity function call between functions: {0}".Fmt(FunctionNames.ToStringList(", ", fInfo => fInfo), Line, Column);
    }

    public IEnumerable<string> FunctionNames { get; set; }
  }

  #endregion

  #region [ Warnings ]

  public class CompilerWarning : CompilerResult
  {
    public CompilerWarning(int line, int column)
      : this("", line, column)
    {
    }

    public CompilerWarning(string message, int line, int column)
      : base(message, line, column)
    {
    }

    public override sealed string Message
    {
      get
      {
        return "Warning: {0}:({1},{2})".Fmt(GetMessage(), Line, Column);
      }
    }
  }

  public class ImplicitConversionWarning : CompilerWarning
  {
    public ImplicitConversionWarning(string tFrom, string tTo, int line, int column)
      : base(line, column)
    {
      TypeFrom = tFrom;
      TypeTo = tTo;
    }

    protected override string GetMessage()
    {
      return "Implicit conversion from type '{0}' to type '{1}'".Fmt(TypeFrom, TypeTo);
    }

    public string TypeFrom { get; set; }

    public string TypeTo { get; set; }
  }

  #endregion
}
