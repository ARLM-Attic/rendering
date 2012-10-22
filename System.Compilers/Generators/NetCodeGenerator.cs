using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;
using System.IO;

namespace System.Compilers.Generators
{
    public abstract class NetCodeGenerator : CodeGenerator<NetAstNode>,
 ICodeGeneratorOf<NetLocalVariable>,
 ICodeGeneratorOf<NetTypeDeclarationAST>,
 ICodeGeneratorOf<NetMethodBodyDeclarationAST>,
 ICodeGeneratorOf<NetMethodDeclarationAST>,
 ICodeGeneratorOf<NetConstructorDeclarationAST>,
 ICodeGeneratorOf<NetFieldDeclarationAST>,
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

        public NetCodeGenerator() : base() { }

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetLocalVariable>.GetCode(System.Compilers.AST.NetLocalVariable ast)
        {
            return GetCodeNetLocalVariable(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetLocalVariable(System.Compilers.AST.NetLocalVariable ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetTypeDeclarationAST>.GetCode(System.Compilers.AST.NetTypeDeclarationAST ast)
        {
            return GetCodeNetTypeDeclarationAST(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetTypeDeclarationAST(System.Compilers.AST.NetTypeDeclarationAST ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetMethodBodyDeclarationAST>.GetCode(System.Compilers.AST.NetMethodBodyDeclarationAST ast)
        {
            return GetCodeNetMethodBodyDeclarationAST(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetMethodBodyDeclarationAST(System.Compilers.AST.NetMethodBodyDeclarationAST ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetMethodDeclarationAST>.GetCode(System.Compilers.AST.NetMethodDeclarationAST ast)
        {
            return GetCodeNetMethodDeclarationAST(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetMethodDeclarationAST(System.Compilers.AST.NetMethodDeclarationAST ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetConstructorDeclarationAST>.GetCode(System.Compilers.AST.NetConstructorDeclarationAST ast)
        {
            return GetCodeNetConstructorDeclarationAST(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetConstructorDeclarationAST(System.Compilers.AST.NetConstructorDeclarationAST ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetFieldDeclarationAST>.GetCode(System.Compilers.AST.NetFieldDeclarationAST ast)
        {
            return GetCodeNetFieldDeclarationAST(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetFieldDeclarationAST(System.Compilers.AST.NetFieldDeclarationAST ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstStatement>.GetCode(System.Compilers.AST.NetAstStatement ast)
        {
            return GetCodeNetAstStatement(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstStatement(System.Compilers.AST.NetAstStatement ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstLocalDeclaration>.GetCode(System.Compilers.AST.NetAstLocalDeclaration ast)
        {
            return GetCodeNetAstLocalDeclaration(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstLocalDeclaration(System.Compilers.AST.NetAstLocalDeclaration ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstExpressionStatement>.GetCode(System.Compilers.AST.NetAstExpressionStatement ast)
        {
            return GetCodeNetAstExpressionStatement(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstExpressionStatement(System.Compilers.AST.NetAstExpressionStatement ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstConstantExpression>.GetCode(System.Compilers.AST.NetAstConstantExpression ast)
        {
            return GetCodeNetAstConstantExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstConstantExpression(System.Compilers.AST.NetAstConstantExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstArgumentExpression>.GetCode(System.Compilers.AST.NetAstArgumentExpression ast)
        {
            return GetCodeNetAstArgumentExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstArgumentExpression(System.Compilers.AST.NetAstArgumentExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstFieldExpression>.GetCode(System.Compilers.AST.NetAstFieldExpression ast)
        {
            return GetCodeNetAstFieldExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstFieldExpression(System.Compilers.AST.NetAstFieldExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstPropertyExpression>.GetCode(System.Compilers.AST.NetAstPropertyExpression ast)
        {
            return GetCodeNetAstPropertyExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstPropertyExpression(System.Compilers.AST.NetAstPropertyExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstLocalExpression>.GetCode(System.Compilers.AST.NetAstLocalExpression ast)
        {
            return GetCodeNetAstLocalExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstLocalExpression(System.Compilers.AST.NetAstLocalExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstThisExpression>.GetCode(System.Compilers.AST.NetAstThisExpression ast)
        {
            return GetCodeNetAstThisExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstThisExpression(System.Compilers.AST.NetAstThisExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstBinaryOperatorExpression>.GetCode(System.Compilers.AST.NetAstBinaryOperatorExpression ast)
        {
            return GetCodeNetAstBinaryOperatorExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstBinaryOperatorExpression(System.Compilers.AST.NetAstBinaryOperatorExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstTernaryOperatorExpressionv>.GetCode(System.Compilers.AST.NetAstTernaryOperatorExpressionv ast)
        {
            return GetCodeNetAstTernaryOperatorExpressionv(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstTernaryOperatorExpressionv(System.Compilers.AST.NetAstTernaryOperatorExpressionv ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstUnaryOperatorExpression>.GetCode(System.Compilers.AST.NetAstUnaryOperatorExpression ast)
        {
            return GetCodeNetAstUnaryOperatorExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstUnaryOperatorExpression(System.Compilers.AST.NetAstUnaryOperatorExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstConvertOperatorExpression>.GetCode(System.Compilers.AST.NetAstConvertOperatorExpression ast)
        {
            return GetCodeNetAstConvertOperatorExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstConvertOperatorExpression(System.Compilers.AST.NetAstConvertOperatorExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstMethodCallExpression>.GetCode(System.Compilers.AST.NetAstMethodCallExpression ast)
        {
            return GetCodeNetAstMethodCallExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstMethodCallExpression(System.Compilers.AST.NetAstMethodCallExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstConstructorCallExpression>.GetCode(System.Compilers.AST.NetAstConstructorCallExpression ast)
        {
            return GetCodeNetAstConstructorCallExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstConstructorCallExpression(System.Compilers.AST.NetAstConstructorCallExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstInitObjectExpression>.GetCode(System.Compilers.AST.NetAstInitObjectExpression ast)
        {
            return GetCodeNetAstInitObjectExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstInitObjectExpression(System.Compilers.AST.NetAstInitObjectExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetInitMDArrayExpression>.GetCode(System.Compilers.AST.NetInitMDArrayExpression ast)
        {
            return GetCodeNetInitMDArrayExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetInitMDArrayExpression(System.Compilers.AST.NetInitMDArrayExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstAssignamentStatement>.GetCode(System.Compilers.AST.NetAstAssignamentStatement ast)
        {
            return GetCodeNetAstAssignamentStatement(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstAssignamentStatement(System.Compilers.AST.NetAstAssignamentStatement ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetMDArrayElementAssignament>.GetCode(System.Compilers.AST.NetMDArrayElementAssignament ast)
        {
            return GetCodeNetMDArrayElementAssignament(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetMDArrayElementAssignament(System.Compilers.AST.NetMDArrayElementAssignament ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetMDArrayAccessExpression>.GetCode(System.Compilers.AST.NetMDArrayAccessExpression ast)
        {
            return GetCodeNetMDArrayAccessExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetMDArrayAccessExpression(System.Compilers.AST.NetMDArrayAccessExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetInitArrayExpression>.GetCode(System.Compilers.AST.NetInitArrayExpression ast)
        {
            return GetCodeNetInitArrayExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetInitArrayExpression(System.Compilers.AST.NetInitArrayExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetArrayElementAssignament>.GetCode(System.Compilers.AST.NetArrayElementAssignament ast)
        {
            return GetCodeNetArrayElementAssignament(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetArrayElementAssignament(System.Compilers.AST.NetArrayElementAssignament ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetArrayAccessExpression>.GetCode(System.Compilers.AST.NetArrayAccessExpression ast)
        {
            return GetCodeNetArrayAccessExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetArrayAccessExpression(System.Compilers.AST.NetArrayAccessExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstIf>.GetCode(System.Compilers.AST.NetAstIf ast)
        {
            return GetCodeNetAstIf(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstIf(System.Compilers.AST.NetAstIf ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstFor>.GetCode(System.Compilers.AST.NetAstFor ast)
        {
            return GetCodeNetAstFor(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstFor(System.Compilers.AST.NetAstFor ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstDoWhile>.GetCode(System.Compilers.AST.NetAstDoWhile ast)
        {
            return GetCodeNetAstDoWhile(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstDoWhile(System.Compilers.AST.NetAstDoWhile ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstWhile>.GetCode(System.Compilers.AST.NetAstWhile ast)
        {
            return GetCodeNetAstWhile(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstWhile(System.Compilers.AST.NetAstWhile ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstContinue>.GetCode(System.Compilers.AST.NetAstContinue ast)
        {
            return GetCodeNetAstContinue(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstContinue(System.Compilers.AST.NetAstContinue ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstBreak>.GetCode(System.Compilers.AST.NetAstBreak ast)
        {
            return GetCodeNetAstBreak(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstBreak(System.Compilers.AST.NetAstBreak ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstBlock>.GetCode(System.Compilers.AST.NetAstBlock ast)
        {
            return GetCodeNetAstBlock(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstBlock(System.Compilers.AST.NetAstBlock ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstNullComparitionExpression>.GetCode(System.Compilers.AST.NetAstNullComparitionExpression ast)
        {
            return GetCodeNetAstNullComparitionExpression(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstNullComparitionExpression(System.Compilers.AST.NetAstNullComparitionExpression ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstReturn>.GetCode(System.Compilers.AST.NetAstReturn ast)
        {
            return GetCodeNetAstReturn(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstReturn(System.Compilers.AST.NetAstReturn ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstUnconditionalGoto>.GetCode(System.Compilers.AST.NetAstUnconditionalGoto ast)
        {
            return GetCodeNetAstUnconditionalGoto(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstUnconditionalGoto(System.Compilers.AST.NetAstUnconditionalGoto ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstConditionalGoto>.GetCode(System.Compilers.AST.NetAstConditionalGoto ast)
        {
            return GetCodeNetAstConditionalGoto(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstConditionalGoto(System.Compilers.AST.NetAstConditionalGoto ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstLabel>.GetCode(System.Compilers.AST.NetAstLabel ast)
        {
            return GetCodeNetAstLabel(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstLabel(System.Compilers.AST.NetAstLabel ast);

        IEnumerable<string> ICodeGeneratorOf<System.Compilers.AST.NetAstSwitch>.GetCode(System.Compilers.AST.NetAstSwitch ast)
        {
            return GetCodeNetAstSwitch(ast);
        }

        protected abstract IEnumerable<string> GetCodeNetAstSwitch(System.Compilers.AST.NetAstSwitch ast);
    }

}
