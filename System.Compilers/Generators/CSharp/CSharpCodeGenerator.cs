using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Compilers.AST;
using System.Reflection;

namespace System.Compilers.Generators.CSharp
{
    public class CSharpCodeGenerator : NetCodeGenerator
    {
        public CSharpCodeGenerator()
            : base()
        {
        }
        
        public override string GetUnresolvedCode(NetAstNode ast)
        {
            throw new NotSupportedException(ast.ToString());
        }

        protected override IEnumerable<string> GetCodeNetLocalVariable(NetLocalVariable ast)
        {
            yield return ast.Name;
        }

        protected override IEnumerable<string> GetCodeNetTypeDeclarationAST(NetTypeDeclarationAST ast)
        {
            Type type = ast.Member as Type;

            string visibility = type.IsPublic ? "public" : type.IsNotPublic ? "private" :
                type.IsNestedFamANDAssem ? "protected internal" : type.IsNestedFamily ? "protected" : "internal";

            string kindType = type.IsClass ? "class" : type.IsValueType ? "struct" : type.IsEnum ? "enum" : type.IsInterface ? "interface": "unknown";

            yield return visibility + " " + kindType + " " + type.Name;
            yield return "{";
            foreach (var m in ast.Members)
                yield return Indent(GetCode(m));
            yield return "}";
        }

        protected override IEnumerable<string> GetCodeNetMethodBodyDeclarationAST(NetMethodBodyDeclarationAST ast)
        {
            yield return "{";
            foreach (var s in ast.Statements)
                yield return Indent(GetCode(s));
            yield return "}";
        }

        string GetVisibility(MethodBase method)
        {
         return    method.IsPublic ? "public" : method.IsFamily ? "protected" : method.IsFamilyAndAssembly ? "protected internal" :
               method.IsPrivate ? "private" : method.IsAssembly ? "internal" : "";
        }

        string GetVisibility(FieldInfo field)
        {
            return field.IsPublic ? "public" : field.IsFamily ? "protected" : field.IsFamilyAndAssembly ? "protected internal" :
                  field.IsPrivate ? "private" : field.IsAssembly ? "internal" : "";
        }

        string GetParametersDeclaration(MethodBase method)
        {
            return string.Join(", ", method.GetParameters().Select(p =>
            {
                string mod = p.IsIn && p.IsOut ? "ref " : p.IsOut ? "out " : "";

                return mod + p.ParameterType.FullName + " " + p.Name;
            }).ToArray());
        }

        string GetParameters(MethodBase method, NetAstExpression[] arguments)
        {
            return string.Join(", ", method.GetParameters().Select((p, index) =>
            {
                string mod = p.IsIn && p.IsOut ? "ref " : p.IsOut ? "out " : "";

                return mod + GetCode(arguments[index]);
            }).ToArray());
        }

        protected override IEnumerable<string> GetCodeNetMethodDeclarationAST(NetMethodDeclarationAST ast)
        {
            MethodInfo method = ast.Member;

            string visibility = GetVisibility(method);

            string parameters = GetParametersDeclaration(method);

            yield return visibility + " " + (method.IsStatic ? "static " : "") + method.ReturnType.FullName + " (" + parameters + ")";
            yield return GetCode(ast.Body);
        }

        protected override IEnumerable<string> GetCodeNetConstructorDeclarationAST(NetConstructorDeclarationAST ast)
        {
            ConstructorInfo constructor = ast.Member;

            string visibility = GetVisibility(constructor);

            string parameters = GetParametersDeclaration(constructor);

            yield return visibility + " " + (constructor.IsStatic ? "static " : "") + constructor.DeclaringType.Name + " (" + parameters + ")";
            yield return GetCode(ast.Body);
        }

        protected override IEnumerable<string> GetCodeNetFieldDeclarationAST(NetFieldDeclarationAST ast)
        {
            FieldInfo field = ast.Member;

            string visibility = GetVisibility(field);

            yield return visibility + " " + field.FieldType.FullName + " " + field.Name;
        }

        protected override IEnumerable<string> GetCodeNetAstStatement(NetAstStatement ast)
        {
            throw new NotSupportedException(ast.ToString());
        }

        protected override IEnumerable<string> GetCodeNetAstLocalDeclaration(NetAstLocalDeclaration ast)
        {
            yield return ast.LocalInfo.Type.FullName + " " + ast.LocalInfo.Name;
        }

        protected override IEnumerable<string> GetCodeNetAstExpressionStatement(NetAstExpressionStatement ast)
        {
            yield return GetCode(ast.Expression) + ";";
        }

        protected override IEnumerable<string> GetCodeNetAstConstantExpression(NetAstConstantExpression ast)
        {
            yield return ast.Value == null ? "null" :
                ast.Value is float ? ast.Value.ToString() + "f" :
                ast.Value is bool ? (((bool)ast.Value) ? "true" : "false") :
                ast.Value.ToString();
        }

        protected override IEnumerable<string> GetCodeNetAstArgumentExpression(NetAstArgumentExpression ast)
        {
            yield return ast.ParameterInfo.Name;
        }

        string GetLeftSideCode(NetAstInvokeExpression ast)
        {
            return ast.LeftSide == null ? ast.Member.DeclaringType.FullName :
                GetCode(ast.LeftSide);
        }

        protected override IEnumerable<string> GetCodeNetAstFieldExpression(NetAstFieldExpression ast)
        {
            yield return GetLeftSideCode (ast) + "." + ast.FieldInfo.Name;
        }

        protected override IEnumerable<string> GetCodeNetAstPropertyExpression(NetAstPropertyExpression ast)
        {
            yield return GetLeftSideCode(ast) + "." + ast.PropertyInfo.Name;
        }

        protected override IEnumerable<string> GetCodeNetAstLocalExpression(NetAstLocalExpression ast)
        {
            yield return ast.LocalInfo.Name;
        }

        protected override IEnumerable<string> GetCodeNetAstThisExpression(NetAstThisExpression ast)
        {
            yield return "this";
        }

        Dictionary<Operators, int> priorities = new Dictionary<Operators, int>
        {   { Operators.ConditionalOr, 1},
            {Operators.ConditionalAnd, 2 },
            { Operators.GreaterThan, 3 },
            { Operators.GreaterThanOrEquals, 3 },
            { Operators.LessThan, 3 },
            { Operators.LessThanOrEquals, 3} ,
            { Operators.Equality, 3},
            {Operators.Inequality, 3},
            { Operators.LogicAnd, 4 },
            { Operators.LogicOr, 4},
            { Operators.LogicXor, 4},
            { Operators.Addition, 5 },
            { Operators.Subtraction, 5 },
            { Operators.Multiply, 6 },
            { Operators.Division, 6 },
            { Operators.Modulus, 6 },
            { Operators.Cast , 7 },
            { Operators.Implicit, 7 },
            { Operators.Indexer, 8 },
            { Operators.UnaryPlus, 9},
            { Operators.UnaryNegation, 9}
        };

        protected override IEnumerable<string> GetCodeNetAstBinaryOperatorExpression(NetAstBinaryOperatorExpression ast)
        {
            int leftSidePriority =
                ast.LeftOperand is NetAstBinaryOperatorExpression ? priorities[((NetAstBinaryOperatorExpression)ast.LeftOperand).Operator] :
                ast.LeftOperand is NetAstUnaryOperatorExpression ? priorities[((NetAstUnaryOperatorExpression)ast.LeftOperand).Operator] :
                int.MaxValue;

            int rightSidePriorty =
                ast.RightOperand is NetAstBinaryOperatorExpression ? priorities[((NetAstBinaryOperatorExpression)ast.RightOperand).Operator] :
                ast.RightOperand is NetAstUnaryOperatorExpression ? priorities[((NetAstUnaryOperatorExpression)ast.RightOperand).Operator] :
                int.MaxValue;

            int thisPriority = priorities[ast.Operator];

            string leftExpression = (leftSidePriority < thisPriority) ?
                "(" + GetCode(ast.LeftOperand) + ")" :
                GetCode(ast.LeftOperand);

            string rightExpression = rightSidePriorty < thisPriority ?
                "(" + GetCode(ast.RightOperand) + ")" :
                GetCode(ast.RightOperand);

            yield return leftExpression + ast.Operator.Token() + rightExpression;

        }

        protected override IEnumerable<string> GetCodeNetAstTernaryOperatorExpressionv(NetAstTernaryOperatorExpressionv ast)
        {
            yield return "(" + GetCode(ast.Conditional) + ")?" + GetCode(ast.WhenTrue) + ":" + GetCode(ast.WhenFalse);
        }

        protected override IEnumerable<string> GetCodeNetAstUnaryOperatorExpression(NetAstUnaryOperatorExpression ast)
        {
            int rightSidePriorty =
               ast.Operand is NetAstBinaryOperatorExpression ? priorities[((NetAstBinaryOperatorExpression)ast.Operand).Operator] :
               ast.Operand is NetAstUnaryOperatorExpression ? priorities[((NetAstUnaryOperatorExpression)ast.Operand).Operator] :
               int.MaxValue;

            int thisPriority = priorities[ast.Operator];

            string operandExpression = rightSidePriorty < thisPriority ?
                "(" + GetCode(ast.Operand) + ")" :
                GetCode(ast.Operand);

            yield return ast.Operator.Token() + operandExpression;
        }

        protected override IEnumerable<string> GetCodeNetAstConvertOperatorExpression(NetAstConvertOperatorExpression ast)
        {
            string operandExpression = GetCode (ast.Operand);
            if (ast.Operand is NetAstBinaryOperatorExpression ||
                ast.Operand is NetAstUnaryOperatorExpression ||
                ast.Operand is NetAstTernaryOperatorExpressionv)
                operandExpression = "("+operandExpression+")";
            yield return "(" + ast.TargetType.FullName + ")" + operandExpression;
        }

        protected override IEnumerable<string> GetCodeNetAstMethodCallExpression(NetAstMethodCallExpression ast)
        {
            yield return GetLeftSideCode(ast) + "." + ast.MethodInfo.Name + "(" + GetParameters(ast.MethodInfo, ast.Arguments) + ")";
        }

        protected override IEnumerable<string> GetCodeNetAstConstructorCallExpression(NetAstConstructorCallExpression ast)
        {
            yield return "new " + ast.ConstructorInfo.DeclaringType.FullName + "(" + GetParameters(ast.ConstructorInfo, ast.Arguments) + ")";
        }

        protected override IEnumerable<string> GetCodeNetAstInitObjectExpression(NetAstInitObjectExpression ast)
        {
            yield return "new " + ast.TargetType.FullName + "()";
        }

        private string GetInitializationString(NetInitMDArrayExpression ast)
        {
            List<string> result = new List<string>();

            int totalPositions = ast.InitValues.Length;
            int rank = ast.InitValues.Rank;
            int[] arrayDimensions = new int[rank];

            for (int i = 0; i < rank; i++)
            {
                arrayDimensions[i] = ast.InitValues.GetLength(i);
            }

            int[] indices = new int[rank];
            for (int j = 0; j < totalPositions; j++)
            {
                int divisor = totalPositions / arrayDimensions[0];
                int reminder = j;
                for (int k = 0; k < rank; k++)
                {
                    indices[k] = reminder / divisor;
                    reminder = reminder % divisor;

                    if (k < rank - 1)
                        divisor /= arrayDimensions[k + 1];
                }

                result.Add(((NetAstConstantExpression)ast.InitValues.GetValue(indices)).Value.ToString());
            }

            for (int i = 0; i < result.Count; i++)
            {
                int divisor = 1;
                for (int j = 1; j <= rank; j++)
                {
                    divisor *= arrayDimensions[rank - j];
                    if (i % divisor == 0)
                        result[i] = "{ " + result[i];
                    if ((i + 1) % divisor == 0)
                        result[i] += " }";
                }
            }

            return String.Join(", ", result.ToArray());
        }
        protected override IEnumerable<string> GetCodeNetInitMDArrayExpression(NetInitMDArrayExpression ast)
        {
            string initializations = ast.InitValues != null ? GetInitializationString(ast) : "";

            yield return "(new " + ast.ArrayType.GetElementType().FullName + "[" + String.Join(",", ast.ArraySizes.Select(e => e.ToString()).ToArray()) + "] " + initializations + ")";

        }

        protected override IEnumerable<string> GetCodeNetAstAssignamentStatement(NetAstAssignamentStatement ast)
        {
            yield return GetCode(ast.LeftValue) + "=" + GetCode(ast.Value)+";";
        }

        protected override IEnumerable<string> GetCodeNetMDArrayElementAssignament(NetMDArrayElementAssignament ast)
        {
            yield return GetCode(ast.ArrayExpression) + "[" + String.Join(", ", ast.ArrayIndices.Select(e => GetCode(e)).ToArray()) + "]" + " = " + GetCode(ast.Value)+";";
        }

        protected override IEnumerable<string> GetCodeNetMDArrayAccessExpression(NetMDArrayAccessExpression ast)
        {
            yield return GetCode(ast.ArrayExpression) + "[" + String.Join(", ", ast.IndicesExpressions.Select(e => GetCode(e)).ToArray()) + "]";
        }

        protected override IEnumerable<string> GetCodeNetInitArrayExpression(NetInitArrayExpression ast)
        {
            yield return "(new " + ast.ArrayType.FullName + "[" + GetCode(ast.ArraySize) + "]" + (ast.InitValues != null ? "{" + String.Join(", ", ast.InitValues.Select(e => GetCode(e)).ToArray()) + "}" : "") + ")";
        }

        protected override IEnumerable<string> GetCodeNetArrayElementAssignament(NetArrayElementAssignament ast)
        {
            yield return GetCode(ast.ArrayExpression) + "[" + GetCode(ast.ArrayIndex) + "]" + "=" + GetCode(ast.Value)+";";
        }

        protected override IEnumerable<string> GetCodeNetArrayAccessExpression(NetArrayAccessExpression ast)
        {
            yield return GetCode(ast.ArrayExpression) + "[" + GetCode(ast.IndexExpression) + "]";

        }

        protected override IEnumerable<string> GetCodeNetAstIf(NetAstIf ast)
        {
            yield return "if (" + GetCode(ast.Condition) + ")";
            yield return GetCode(ast.WhenTrue);
            if (ast.WhenFalse != null)
            {
                yield return "else";
                yield return GetCode(ast.WhenFalse);
            }
        }

        protected override IEnumerable<string> GetCodeNetAstFor(NetAstFor ast)
        {
            yield return "for (" + GetCode(ast.InitAssign) + ast.Condition + ";" + GetCode(ast.IterationStatement).TrimEnd(';') + ")";
            yield return GetCode(ast.Body);
        }

        protected override IEnumerable<string> GetCodeNetAstDoWhile(NetAstDoWhile ast)
        {
            yield return "do";
            if (ast.Body.Instructions.Count <= 1)
                yield return "{";
            yield return GetCode(ast.Body);
            if (ast.Body.Instructions.Count <= 1)
                yield return "}";
            yield return "while (" + GetCode(ast.Condition) + ");";
        }

        protected override IEnumerable<string> GetCodeNetAstWhile(NetAstWhile ast)
        {
            yield return "while (" + GetCode(ast.Condition) + ")";
            yield return GetCode(ast.Body);
        }

        protected override IEnumerable<string> GetCodeNetAstContinue(NetAstContinue ast)
        {
            yield return "continue;";
        }

        protected override IEnumerable<string> GetCodeNetAstBreak(NetAstBreak ast)
        {
            yield return "break;";
        }

        protected override IEnumerable<string> GetCodeNetAstBlock(NetAstBlock ast)
        {
            switch (ast.Instructions.Count)
            {
                case 0: yield return Indent(";");
                    yield break;
                case 1: yield return Indent(GetCode(ast.Instructions[0]) + ";");
                    yield break;
                default:
                    yield return "{";
                    foreach (var i in ast.Instructions)
                        yield return Indent(GetCode(i));
                    yield return "}";
                    yield break;
            }
        }

        protected override IEnumerable<string> GetCodeNetAstNullComparitionExpression(NetAstNullComparitionExpression ast)
        {
            yield return "(" + GetCode(ast.Operand) + (ast.Operator.Equals(Operators.Equality) ? "==" : "!=") + "null" + ")";
        }

        protected override IEnumerable<string> GetCodeNetAstReturn(NetAstReturn ast)
        {
            yield return "return " + ((ast.Expression == null) ? "" : GetCode(ast.Expression)) + ";";
        }

        protected override IEnumerable<string> GetCodeNetAstUnconditionalGoto(NetAstUnconditionalGoto ast)
        {
            yield return "goto " + ast.Destination.Name+";";
        }

        protected override IEnumerable<string> GetCodeNetAstConditionalGoto(NetAstConditionalGoto ast)
        {
            yield return "if (" + GetCode(ast.Condition) + ") goto " + ast.Destination.Name + ";";
        }

        protected override IEnumerable<string> GetCodeNetAstLabel(NetAstLabel ast)
        {
            yield return "label " + ast.Name + ":";
        }

        protected override IEnumerable<string> GetCodeNetAstSwitch(NetAstSwitch ast)
        {
            yield return "switch (" + GetCode(ast.Condition) + ")";
            yield return "{";
            foreach (var c in ast.Cases)
                yield return Indent(GetCode(c));
            yield return "}";
        }
    }
}
