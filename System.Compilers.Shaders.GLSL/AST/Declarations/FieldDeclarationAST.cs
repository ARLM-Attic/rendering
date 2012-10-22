using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.AST.Expressions;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Declarations
{
    public class FieldDeclarationAST : VariableDeclarationAST
    {
        public FieldDeclarationAST()
        {
        }

        public FieldDeclarationAST(int line, int column)
            : base(line, column)
        {
        }

        public FieldDeclarationAST(string name, int line, int column)
            : base(name, line, column)
        {
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
                    GLSLType fieldType;
                    if (IsArray)
                        CheckArraySize(context, TypeSpecifier.Type, out fieldType);
                    else
                        fieldType = TypeSpecifier.Type;

                    FieldVariableInfo fieldInfo = new FieldVariableInfo()
                    {
                        Name = Name,
                        Type = fieldType
                    };
                    context.Scope.AddVariable(fieldInfo);
                    VarInfo = fieldInfo;
                }
            }
            context.UnMarkErrors();
        }
    }
}
