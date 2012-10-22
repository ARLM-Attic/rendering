using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;
using System.IO;

namespace System.Compilers.Net
{
    public abstract class NetCodeGenerator : CodeGenerator<NetAstNode>,
 ICodeGeneratorOf<NetLocalVariable>,
 ICodeGeneratorOf<NetTypeDeclarationAST>,
 ICodeGeneratorOf<NetMethodBodyDeclarationAST>,
 ICodeGeneratorOf<NetMethodDeclarationAST>,
 ICodeGeneratorOf<NetConstructorDeclarationAST>,
 ICodeGeneratorOf<NetFieldDeclarationAST>,
 ICodeGeneratorOf<NetAstProgramByDeclaration>,
 ICodeGeneratorOf<NetAstProgramByComposition>,
 ICodeGeneratorOf<NetAstStatement>,
 ICodeGeneratorOf<NetAstLocalDeclaration>,
 ICodeGeneratorOf<NetAstExpressionStatement>,
 ICodeGeneratorOf<NetAstConstantExpression>,
 ICodeGeneratorOf<NetAstArgumentExpression>,
 ICodeGeneratorOf<NetAstFieldExpression>,
 ICodeGeneratorOf<NetAstPropertyExpression>,
 ICodeGeneratorOf<NetAstLocalExpression>,
 ICodeGeneratorOf<NetAstThisExpression>,
 ICodeGeneratorOf<NetAstBinaryOperatorExpression>,
 ICodeGeneratorOf<NetAstTernaryOperatorExpressionv>,
 ICodeGeneratorOf<NetAstUnaryOperatorExpression>,
 ICodeGeneratorOf<NetAstConvertOperatorExpression>,
 ICodeGeneratorOf<NetAstMethodCallExpression>,
 ICodeGeneratorOf<NetAstConstructorCallExpression>,
 ICodeGeneratorOf<NetAstInitObjectExpression>,
 ICodeGeneratorOf<NetInitMDArrayExpression>,
 ICodeGeneratorOf<NetAstAssignamentStatement>,
 ICodeGeneratorOf<NetMDArrayElementAssignament>,
 ICodeGeneratorOf<NetMDArrayAccessExpression>,
 ICodeGeneratorOf<NetInitArrayExpression>,
 ICodeGeneratorOf<NetArrayElementAssignament>,
 ICodeGeneratorOf<NetArrayAccessExpression>,
 ICodeGeneratorOf<NetAstIf>,
 ICodeGeneratorOf<NetAstFor>,
 ICodeGeneratorOf<NetAstDoWhile>,
 ICodeGeneratorOf<NetAstWhile>,
 ICodeGeneratorOf<NetAstContinue>,
 ICodeGeneratorOf<NetAstBreak>,
 ICodeGeneratorOf<NetAstBlock>,
 ICodeGeneratorOf<NetAstNullComparitionExpression>,
 ICodeGeneratorOf<NetAstReturn>,
 ICodeGeneratorOf<NetAstUnconditionalGoto>,
 ICodeGeneratorOf<NetAstConditionalGoto>,
 ICodeGeneratorOf<NetAstLabel>,
 ICodeGeneratorOf<NetAstSwitch>
    {

        public NetCodeGenerator(Stream outputStream) : base(outputStream) { }

        void ICodeGeneratorOf<System.Compilers.AST.NetLocalVariable>.Generate(System.Compilers.AST.NetLocalVariable ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetLocalVariable(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetLocalVariable(System.Compilers.AST.NetLocalVariable ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetTypeDeclarationAST>.Generate(System.Compilers.AST.NetTypeDeclarationAST ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetTypeDeclarationAST(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetTypeDeclarationAST(System.Compilers.AST.NetTypeDeclarationAST ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetMethodBodyDeclarationAST>.Generate(System.Compilers.AST.NetMethodBodyDeclarationAST ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetMethodBodyDeclarationAST(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetMethodBodyDeclarationAST(System.Compilers.AST.NetMethodBodyDeclarationAST ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetMethodDeclarationAST>.Generate(System.Compilers.AST.NetMethodDeclarationAST ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetMethodDeclarationAST(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetMethodDeclarationAST(System.Compilers.AST.NetMethodDeclarationAST ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetConstructorDeclarationAST>.Generate(System.Compilers.AST.NetConstructorDeclarationAST ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetConstructorDeclarationAST(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetConstructorDeclarationAST(System.Compilers.AST.NetConstructorDeclarationAST ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetFieldDeclarationAST>.Generate(System.Compilers.AST.NetFieldDeclarationAST ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetFieldDeclarationAST(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetFieldDeclarationAST(System.Compilers.AST.NetFieldDeclarationAST ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstProgramByDeclaration>.Generate(System.Compilers.AST.NetAstProgramByDeclaration ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstProgramByDeclaration(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstProgramByDeclaration(System.Compilers.AST.NetAstProgramByDeclaration ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstProgramByComposition>.Generate(System.Compilers.AST.NetAstProgramByComposition ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstProgramByComposition(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstProgramByComposition(System.Compilers.AST.NetAstProgramByComposition ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstStatement>.Generate(System.Compilers.AST.NetAstStatement ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstStatement(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstStatement(System.Compilers.AST.NetAstStatement ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstLocalDeclaration>.Generate(System.Compilers.AST.NetAstLocalDeclaration ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstLocalDeclaration(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstLocalDeclaration(System.Compilers.AST.NetAstLocalDeclaration ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstExpressionStatement>.Generate(System.Compilers.AST.NetAstExpressionStatement ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstExpressionStatement(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstExpressionStatement(System.Compilers.AST.NetAstExpressionStatement ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstConstantExpression>.Generate(System.Compilers.AST.NetAstConstantExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstConstantExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstConstantExpression(System.Compilers.AST.NetAstConstantExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstArgumentExpression>.Generate(System.Compilers.AST.NetAstArgumentExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstArgumentExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstArgumentExpression(System.Compilers.AST.NetAstArgumentExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstFieldExpression>.Generate(System.Compilers.AST.NetAstFieldExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstFieldExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstFieldExpression(System.Compilers.AST.NetAstFieldExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstPropertyExpression>.Generate(System.Compilers.AST.NetAstPropertyExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstPropertyExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstPropertyExpression(System.Compilers.AST.NetAstPropertyExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstLocalExpression>.Generate(System.Compilers.AST.NetAstLocalExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstLocalExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstLocalExpression(System.Compilers.AST.NetAstLocalExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstThisExpression>.Generate(System.Compilers.AST.NetAstThisExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstThisExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstThisExpression(System.Compilers.AST.NetAstThisExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstBinaryOperatorExpression>.Generate(System.Compilers.AST.NetAstBinaryOperatorExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstBinaryOperatorExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstBinaryOperatorExpression(System.Compilers.AST.NetAstBinaryOperatorExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstTernaryOperatorExpressionv>.Generate(System.Compilers.AST.NetAstTernaryOperatorExpressionv ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstTernaryOperatorExpressionv(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstTernaryOperatorExpressionv(System.Compilers.AST.NetAstTernaryOperatorExpressionv ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstUnaryOperatorExpression>.Generate(System.Compilers.AST.NetAstUnaryOperatorExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstUnaryOperatorExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstUnaryOperatorExpression(System.Compilers.AST.NetAstUnaryOperatorExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstConvertOperatorExpression>.Generate(System.Compilers.AST.NetAstConvertOperatorExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstConvertOperatorExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstConvertOperatorExpression(System.Compilers.AST.NetAstConvertOperatorExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstMethodCallExpression>.Generate(System.Compilers.AST.NetAstMethodCallExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstMethodCallExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstMethodCallExpression(System.Compilers.AST.NetAstMethodCallExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstConstructorCallExpression>.Generate(System.Compilers.AST.NetAstConstructorCallExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstConstructorCallExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstConstructorCallExpression(System.Compilers.AST.NetAstConstructorCallExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstInitObjectExpression>.Generate(System.Compilers.AST.NetAstInitObjectExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstInitObjectExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstInitObjectExpression(System.Compilers.AST.NetAstInitObjectExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetInitMDArrayExpression>.Generate(System.Compilers.AST.NetInitMDArrayExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetInitMDArrayExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetInitMDArrayExpression(System.Compilers.AST.NetInitMDArrayExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstAssignamentStatement>.Generate(System.Compilers.AST.NetAstAssignamentStatement ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstAssignamentStatement(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstAssignamentStatement(System.Compilers.AST.NetAstAssignamentStatement ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetMDArrayElementAssignament>.Generate(System.Compilers.AST.NetMDArrayElementAssignament ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetMDArrayElementAssignament(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetMDArrayElementAssignament(System.Compilers.AST.NetMDArrayElementAssignament ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetMDArrayAccessExpression>.Generate(System.Compilers.AST.NetMDArrayAccessExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetMDArrayAccessExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetMDArrayAccessExpression(System.Compilers.AST.NetMDArrayAccessExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetInitArrayExpression>.Generate(System.Compilers.AST.NetInitArrayExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetInitArrayExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetInitArrayExpression(System.Compilers.AST.NetInitArrayExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetArrayElementAssignament>.Generate(System.Compilers.AST.NetArrayElementAssignament ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetArrayElementAssignament(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetArrayElementAssignament(System.Compilers.AST.NetArrayElementAssignament ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetArrayAccessExpression>.Generate(System.Compilers.AST.NetArrayAccessExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetArrayAccessExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetArrayAccessExpression(System.Compilers.AST.NetArrayAccessExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstIf>.Generate(System.Compilers.AST.NetAstIf ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstIf(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstIf(System.Compilers.AST.NetAstIf ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstFor>.Generate(System.Compilers.AST.NetAstFor ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstFor(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstFor(System.Compilers.AST.NetAstFor ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstDoWhile>.Generate(System.Compilers.AST.NetAstDoWhile ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstDoWhile(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstDoWhile(System.Compilers.AST.NetAstDoWhile ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstWhile>.Generate(System.Compilers.AST.NetAstWhile ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstWhile(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstWhile(System.Compilers.AST.NetAstWhile ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstContinue>.Generate(System.Compilers.AST.NetAstContinue ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstContinue(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstContinue(System.Compilers.AST.NetAstContinue ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstBreak>.Generate(System.Compilers.AST.NetAstBreak ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstBreak(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstBreak(System.Compilers.AST.NetAstBreak ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstBlock>.Generate(System.Compilers.AST.NetAstBlock ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstBlock(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstBlock(System.Compilers.AST.NetAstBlock ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstNullComparitionExpression>.Generate(System.Compilers.AST.NetAstNullComparitionExpression ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstNullComparitionExpression(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstNullComparitionExpression(System.Compilers.AST.NetAstNullComparitionExpression ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstReturn>.Generate(System.Compilers.AST.NetAstReturn ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstReturn(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstReturn(System.Compilers.AST.NetAstReturn ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstUnconditionalGoto>.Generate(System.Compilers.AST.NetAstUnconditionalGoto ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstUnconditionalGoto(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstUnconditionalGoto(System.Compilers.AST.NetAstUnconditionalGoto ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstConditionalGoto>.Generate(System.Compilers.AST.NetAstConditionalGoto ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstConditionalGoto(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstConditionalGoto(System.Compilers.AST.NetAstConditionalGoto ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstLabel>.Generate(System.Compilers.AST.NetAstLabel ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstLabel(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstLabel(System.Compilers.AST.NetAstLabel ast, CodeWriters.ICodeWriter codeWriter);

        void ICodeGeneratorOf<System.Compilers.AST.NetAstSwitch>.Generate(System.Compilers.AST.NetAstSwitch ast, CodeWriters.ICodeWriter codeWriter)
        {
            OnCodeGenerateNetAstSwitch(ast, codeWriter);
        }

        protected abstract void OnCodeGenerateNetAstSwitch(System.Compilers.AST.NetAstSwitch ast, CodeWriters.ICodeWriter codeWriter);
    }

}
