using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.ShaderAST;
using System.Compilers.Shaders.Info;
using System.Compilers.Shaders;

namespace System.Compilers.Generators
{
    public abstract class ShaderCodeGenerator : CodeGenerator<ShaderNodeAST>,
        // Declarations
        ICodeGeneratorOf<ShaderTypeDeclarationAST>,
        ICodeGeneratorOf<ShaderMethodDeclarationAST>,
        ICodeGeneratorOf<ShaderConstructorDeclarationAST>,
        ICodeGeneratorOf<ShaderFieldDeclarationAST>,
        ICodeGeneratorOf<ShaderMethodBodyAST>,

        // Expressions
        ICodeGeneratorOf<ShaderConstantExpressionAST>,
        ICodeGeneratorOf<ShaderLocalInvokeAST>,
        ICodeGeneratorOf<ShaderParameterInvokeAST>,
        ICodeGeneratorOf<ShaderFieldInvokeAST>,
        ICodeGeneratorOf<ShaderMethodInvokeAST>,
        ICodeGeneratorOf<ShaderConstructorInvokeAST>,
        ICodeGeneratorOf<ShaderInitializationInvokeAST>,
        ICodeGeneratorOf<ShaderOperationAST>,
        ICodeGeneratorOf<ShaderConversionAST>,

        // Statements
        ICodeGeneratorOf<ShaderExpressionStatementAST>,
        ICodeGeneratorOf<ShaderLocalDeclarationAST>,
        ICodeGeneratorOf<ShaderConditionalStatement>,
        ICodeGeneratorOf<ShaderWhileStatementAST>,
        ICodeGeneratorOf<ShaderDoWhileStatementAST>,
        ICodeGeneratorOf<ShaderForStatementAST>,
        ICodeGeneratorOf<ShaderReturnStatementAST>,
        ICodeGeneratorOf<ShaderAssignamentAST>,
        ICodeGeneratorOf<ShaderBlockStatementAST>
    {
        public override string GetUnresolvedCode(ShaderNodeAST ast)
        {
            throw new NotSupportedException(ast.GetType().ToString());
        }

        public abstract IEnumerable<string> GetCode(ShaderTypeDeclarationAST ast);

        public abstract IEnumerable<string> GetCode(ShaderMethodDeclarationAST ast);

        public abstract IEnumerable<string> GetCode(ShaderConstructorDeclarationAST ast);

        public abstract IEnumerable<string> GetCode(ShaderFieldDeclarationAST ast);

        public abstract IEnumerable<string> GetCode(ShaderMethodBodyAST ast);

        public abstract IEnumerable<string> GetCode(ShaderConstantExpressionAST ast);

        public abstract IEnumerable<string> GetCode(ShaderLocalInvokeAST ast);

        public abstract IEnumerable<string> GetCode(ShaderParameterInvokeAST ast);

        public abstract IEnumerable<string> GetCode(ShaderFieldInvokeAST ast);

        public abstract IEnumerable<string> GetCode(ShaderMethodInvokeAST ast);

        public abstract IEnumerable<string> GetCode(ShaderConstructorInvokeAST ast);

        public abstract IEnumerable<string> GetCode(ShaderInitializationInvokeAST ast);

        public abstract IEnumerable<string> GetCode(ShaderOperationAST ast);

        public abstract IEnumerable<string> GetCode(ShaderConversionAST ast);

        public abstract IEnumerable<string> GetCode(ShaderExpressionStatementAST ast);

        public abstract IEnumerable<string> GetCode(ShaderLocalDeclarationAST ast);

        public abstract IEnumerable<string> GetCode(ShaderConditionalStatement ast);

        public abstract IEnumerable<string> GetCode(ShaderWhileStatementAST ast);

        public abstract IEnumerable<string> GetCode(ShaderDoWhileStatementAST ast);

        public abstract IEnumerable<string> GetCode(ShaderForStatementAST ast);

        public abstract IEnumerable<string> GetCode(ShaderReturnStatementAST ast);

        public abstract IEnumerable<string> GetCode(ShaderAssignamentAST ast);

        public abstract IEnumerable<string> GetCode(ShaderBlockStatementAST ast);

        public abstract string GenerateCode(ShaderProgramAST ast, out Dictionary<ShaderMember, string> names);

        public abstract Builtins Builtins { get; }

    }
}
