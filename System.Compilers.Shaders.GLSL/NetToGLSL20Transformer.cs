using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.ShaderAST;
using GLSLCompiler.AST;
using GLSLCompiler;

namespace System.Compilers.Shaders.GLSL
{
    public class NetToGLSL20Transformer : ASTTransformer<ShaderNodeAST, BaseAST>,
        ITransformerOf<ShaderExpressionStatementAST, GLSLCompiler.AST.Statements.ExpressionStatementAST>,
        ITransformerOf<ShaderLocalDeclarationAST, GLSLCompiler.AST.Declarations.LocalVariableDeclarationAST>,
        ITransformerOf<ShaderStatementAST, GLSLCompiler.AST.Statements.StatementAST>,
        ITransformerOf<ShaderReturnStatementAST, GLSLCompiler.AST.Statements.ReturnStatementAST>,
        ITransformerOf<ShaderBlockStatementAST, GLSLCompiler.AST.Statements.CompoundStatementAST>,
        ITransformerOf<ShaderConditionalStatement, GLSLCompiler.AST.Statements.IfControlStatementAST>,
        ITransformerOf<ShaderWhileStatementAST, GLSLCompiler.AST.Statements.WhileControlStatementAST>,
        ITransformerOf<ShaderDoWhileStatementAST, GLSLCompiler.AST.Statements.DoWhileControlStatementAST>,
        ITransformerOf<ShaderForStatementAST, GLSLCompiler.AST.Statements.ForControlStatementAST>,

        ITransformerOf<ShaderExpressionAST, GLSLCompiler.AST.Expressions.ExpressionAST>,
        ITransformerOf<ShaderOperationAST, GLSLCompiler.AST.Expressions.ExpressionAST>,
        ITransformerOf<ShaderMethodBaseInvokeAST, GLSLCompiler.AST.Expressions.NaryExpressionAST>,
        ITransformerOf<ShaderMethodInvokeAST, GLSLCompiler.AST.Expressions.FunctionCallExpressionAST>,
        ITransformerOf<ShaderConstructorInvokeAST, GLSLCompiler.AST.Expressions.ConstructionExpressionAST>,
        ITransformerOf<ShaderConversionAST, GLSLCompiler.AST.Expressions.ConstructionExpressionAST>,
        ITransformerOf<ShaderFieldInvokeAST, GLSLCompiler.AST.Expressions.FieldExpressionAST>,
        ITransformerOf<ShaderLocalInvokeAST, GLSLCompiler.AST.Expressions.LocalVariableExpressionAST>,
        ITransformerOf<ShaderParameterInvokeAST, GLSLCompiler.AST.Expressions.LocalVariableExpressionAST>,
        ITransformerOf<ShaderConstantExpressionAST, GLSLCompiler.AST.Expressions.ExpressionAST>,

        ITransformerOf<ShaderMemberDeclarationAST, GLSLCompiler.AST.Declarations.DeclarationAST>,
        ITransformerOf<ShaderMethodBaseDeclarationAST, GLSLCompiler.AST.Declarations.DeclarationAST>,
        ITransformerOf<ShaderMethodDeclarationAST, GLSLCompiler.AST.Declarations.FunctionDeclarationAST>,
        ITransformerOf<ShaderFieldDeclarationAST, GLSLCompiler.AST.Declarations.FieldDeclarationAST>,
        ITransformerOf<ShaderTypeDeclarationAST, GLSLCompiler.AST.Declarations.TypeDeclarationAST>
    {

        GLSLCompiler.Types.GLSLType ResolveType(System.Compilers.Shaders.Info.ShaderType type)
        {
            Builtins builtins = Program.Builtins;

            if (type.Equals(builtins.Boolean))
                return GLSLCompiler.Types.GLSLTypes.BoolType;
        }

        public ShaderProgramAST Program { get; private set; }

        public NetToGLSL20Transformer(ShaderProgramAST program)
        {
            this.Program = program;
        }

        public GLSLCompiler.AST.Statements.ExpressionStatementAST Transform(ShaderExpressionStatementAST source)
        {
            return new GLSLCompiler.AST.Statements.ExpressionStatementAST(Transform(source.Expression), 0, 0);
        }

        public GLSLCompiler.AST.Declarations.LocalVariableDeclarationAST Transform(ShaderLocalDeclarationAST source)
        {
            
            //GLSLCompiler.Types.StructTypeInstance
            //var l = new GLSLCompiler.AST.Declarations.LocalVariableDeclarationAST(source.Name, null, 0, 0);
            //LocalVariableInfo v = new LocalVariableInfo () { Name = source.Name,
            //    Type = GLSLCompiler.Types.GLSLTypes.RegisterType (
        }

        public GLSLCompiler.AST.Statements.StatementAST Transform(ShaderStatementAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Statements.ReturnStatementAST Transform(ShaderReturnStatementAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Statements.CompoundStatementAST Transform(ShaderBlockStatementAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Statements.IfControlStatementAST Transform(ShaderConditionalStatement source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Statements.WhileControlStatementAST Transform(ShaderWhileStatementAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Statements.DoWhileControlStatementAST Transform(ShaderDoWhileStatementAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Statements.ForControlStatementAST Transform(ShaderForStatementAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Expressions.ExpressionAST Transform(ShaderExpressionAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Expressions.ExpressionAST Transform(ShaderOperationAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Expressions.NaryExpressionAST Transform(ShaderMethodBaseInvokeAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Expressions.FunctionCallExpressionAST Transform(ShaderMethodInvokeAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Expressions.ConstructionExpressionAST Transform(ShaderConstructorInvokeAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Expressions.ConstructionExpressionAST Transform(ShaderConversionAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Expressions.FieldExpressionAST Transform(ShaderFieldInvokeAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Expressions.LocalVariableExpressionAST Transform(ShaderLocalInvokeAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Expressions.LocalVariableExpressionAST Transform(ShaderParameterInvokeAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Expressions.ExpressionAST Transform(ShaderConstantExpressionAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Declarations.DeclarationAST Transform(ShaderMemberDeclarationAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Declarations.DeclarationAST Transform(ShaderMethodBaseDeclarationAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Declarations.FunctionDeclarationAST Transform(ShaderMethodDeclarationAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Declarations.FieldDeclarationAST Transform(ShaderFieldDeclarationAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.Declarations.TypeDeclarationAST Transform(ShaderTypeDeclarationAST source)
        {
            throw new NotImplementedException();
        }

        public GLSLCompiler.AST.ShaderAST Transform(ShaderProgramAST source)
        {
            throw new NotImplementedException();
        }
    }
}
