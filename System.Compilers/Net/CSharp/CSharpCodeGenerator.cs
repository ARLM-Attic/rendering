using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Compilers.AST;

namespace System.Compilers.Net.CSharp
{
    public class CSharpCodeGenerator : NetCodeGenerator
    {
        public CSharpCodeGenerator(Stream outputStream)
            : base(outputStream)
        {
        }

        protected override void OnCodeGenerateNetLocalVariable(AST.NetLocalVariable ast, CodeWriters.ICodeWriter codeWriter)
        {
            codeWriter.Write(ast.Name);
        }

        protected override void OnCodeGenerateNetTypeDeclarationAST(AST.NetTypeDeclarationAST ast, CodeWriters.ICodeWriter codeWriter)
        {
            Type type = ast.Member as Type;
            
            string visibility = type.IsPublic ? "public" : type.IsNotPublic ? "private" : "protected";

            string typeType = ast.IsClass ? "class" : ast.IsEnum ? "enum" : ast.IsStruct ? "struct" : "unknown";

            codeWriter.WriteLine(visibility + " " + typeType + " " + ast.Member.Name);
            codeWriter.WriteLine("{{");
            codeWriter.Indent();
            foreach (var member in ast.Members)
                Generate<NetMemberDeclarationAST>(member);
            codeWriter.UnIndent();
            codeWriter.WriteLine("}}");
        }

        protected override void OnCodeGenerateNetMethodBodyDeclarationAST(AST.NetMethodBodyDeclarationAST ast, CodeWriters.ICodeWriter codeWriter)
        {
            codeWriter.WriteLine("{{");
            foreach (var statement in ast.Statements)
                Generate<NetAstStatement>(statement);
            codeWriter.WriteLine("}}");
        }

        protected override void OnCodeGenerateNetMethodDeclarationAST(AST.NetMethodDeclarationAST ast, CodeWriters.ICodeWriter codeWriter)
        {

        }

        protected override void OnCodeGenerateNetConstructorDeclarationAST(AST.NetConstructorDeclarationAST ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetFieldDeclarationAST(AST.NetFieldDeclarationAST ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstProgramByDeclaration(AST.NetAstProgramByDeclaration ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstProgramByComposition(AST.NetAstProgramByComposition ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstStatement(AST.NetAstStatement ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstLocalDeclaration(AST.NetAstLocalDeclaration ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstExpressionStatement(AST.NetAstExpressionStatement ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstConstantExpression(AST.NetAstConstantExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstArgumentExpression(AST.NetAstArgumentExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstFieldExpression(AST.NetAstFieldExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstPropertyExpression(AST.NetAstPropertyExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstLocalExpression(AST.NetAstLocalExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstThisExpression(AST.NetAstThisExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstBinaryOperatorExpression(AST.NetAstBinaryOperatorExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstTernaryOperatorExpressionv(AST.NetAstTernaryOperatorExpressionv ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstUnaryOperatorExpression(AST.NetAstUnaryOperatorExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstConvertOperatorExpression(AST.NetAstConvertOperatorExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstMethodCallExpression(AST.NetAstMethodCallExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstConstructorCallExpression(AST.NetAstConstructorCallExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstInitObjectExpression(AST.NetAstInitObjectExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetInitMDArrayExpression(AST.NetInitMDArrayExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstAssignamentStatement(AST.NetAstAssignamentStatement ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetMDArrayElementAssignament(AST.NetMDArrayElementAssignament ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetMDArrayAccessExpression(AST.NetMDArrayAccessExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetInitArrayExpression(AST.NetInitArrayExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetArrayElementAssignament(AST.NetArrayElementAssignament ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetArrayAccessExpression(AST.NetArrayAccessExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstIf(AST.NetAstIf ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstFor(AST.NetAstFor ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstDoWhile(AST.NetAstDoWhile ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstWhile(AST.NetAstWhile ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstContinue(AST.NetAstContinue ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstBreak(AST.NetAstBreak ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstBlock(AST.NetAstBlock ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstNullComparitionExpression(AST.NetAstNullComparitionExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstReturn(AST.NetAstReturn ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstUnconditionalGoto(AST.NetAstUnconditionalGoto ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstConditionalGoto(AST.NetAstConditionalGoto ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstLabel(AST.NetAstLabel ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        protected override void OnCodeGenerateNetAstSwitch(AST.NetAstSwitch ast, CodeWriters.ICodeWriter codeWriter)
        {
            throw new NotImplementedException();
        }

        public override void GenerateUnresolved(AST.NetAstNode ast)
        {
            throw new NotSupportedException("Ast " + ast);
        }
    }
}
