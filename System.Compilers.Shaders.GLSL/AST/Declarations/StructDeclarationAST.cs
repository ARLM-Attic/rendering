using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Declarations
{
  public class StructDeclarationAST : TypeDeclarationAST, ITypeSpecifier
  {
    public StructDeclarationAST()
    {
      Fields = new List<MultipleVariableDeclarationAST>();
    }

    public StructDeclarationAST(int line, int column)
      : base(line, column)
    {
      Fields = new List<MultipleVariableDeclarationAST>();
    }

    public StructDeclarationAST(string name, int line, int column)
      : this(name, Enumerable.Empty<MultipleVariableDeclarationAST>(), line, column)
    {
    }

    public StructDeclarationAST(string name, IEnumerable<MultipleVariableDeclarationAST> fields, int line, int column)
      : base(name, line, column)
    {
      Fields = new List<MultipleVariableDeclarationAST>(fields);
    }

    public override void CheckSemantic(SemanticContext context)
    {
      if (Name.IsNullOrEmpty())
        Name = context.Scope.GetAnonymousTypeName();
      if (context.Scope.ContainsType(Name))
        context.Errors.Add(new TypeRedeclarationError(Name, Line, Column));
      else
      {
        context.MarkErrors();

        context.PushScope(ScopeType.StructDeclarationScope);
        foreach (var fieldDec in Fields)
          fieldDec.CheckSemantic(context);
        context.PopScope();
        
        if (!context.CheckForErrors())
        {
          var fInfos = Fields.SelectMany(multipleField => multipleField.VarDeclarations).Select(field => new StructType.FieldInfo(field.Name, field.TypeSpecifier.Type));
          
          if (context.Scope.ContainsFunction(Name, fInfos.Select(fInfo => fInfo.Type).ToArray()))
            context.Errors.Add(new FunctionRedeclarationError(Name, Line, Column));
          else
          {
            StructType sType = new StructType(Name, fInfos);
            GLSLTypes.RegisterType(sType);
            TypeInfo newType = new TypeInfo() { Name = sType.Name, Type = sType };
            context.Scope.AddType(newType);

            FunctionInfo constructorInfo = new FunctionInfo()
            {
              Name = Name,
              IsContructor = true,
              IsDefined = true,
              ReturnType = sType,
              ParamInfos = fInfos.Select(fInfo => new ParamInfo() 
              { 
                Name=fInfo.Name,
                Qualifier = ParamQualifier.Default,
                Type = fInfo.Type,
                DefaultValue = null
              }).ToList()
            };
            context.Scope.AddFunction(constructorInfo);

            Type = sType;
          }
        }

        context.UnMarkErrors();
      }
    }

    public List<MultipleVariableDeclarationAST> Fields { get; private set; }

    string ITypeSpecifier.Name
    {
      get { return Name; }
    }

    int ILocation.Line
    {
      get { return Line; }
    }

    int ILocation.Column
    {
      get { return Column; }
    }

    public GLSLType Type { get; set; }
  }
}
