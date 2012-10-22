using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.ShaderAST;
using System.Compilers.AST;
using System.Reflection;
using System.Compilers.Shaders.Info;

namespace System.Compilers.Shaders
{
    partial class ShaderProgramFactory
    {
        class ASTBuilder
        {
            public Scope Scope { get; private set; }

            public ConstantProxy ConstantProxy { get; private set; }

            Dictionary<Type, Func<MemberInfo, ShaderMember>> compilers = new Dictionary<Type, Func<MemberInfo, ShaderMember>>();

            Dictionary<Type, Func<NetAstNode, ShaderNodeAST>> translators = new Dictionary<Type, Func<NetAstNode, ShaderNodeAST>>();

            public ASTBuilder(Builtins builtins)
            {
                this.Scope = new Scope(builtins);
                this.Program = new ShaderProgramAST(builtins);
                this.ConstantProxy = new ConstantProxy();

                Register<MethodInfo>(BuildMethod);
                Register<Type>(BuildType);
                Register<FieldInfo>(BuildField);

                AddTranslator<NetAstLocalDeclaration, ShaderLocalDeclarationAST>(T_LocalDeclaration);
                AddTranslator<NetAstAssignamentStatement, ShaderExpressionStatementAST>(T_Assignament);
                AddTranslator<NetAstBinaryOperatorExpression, ShaderOperationAST>(T_BinaryOperationExpression);
                AddTranslator<NetAstUnaryOperatorExpression, ShaderOperationAST>(T_UnaryOperationExpression);
                AddTranslator<NetAstIf, ShaderConditionalStatement>(T_ConditionalStatement);
                AddTranslator<NetAstConstantExpression, ShaderConstantExpressionAST>(T_Constant);
                AddTranslator<NetAstConstructorCallExpression, ShaderConstructorInvokeAST>(T_ConstructorCallExpression);
                AddTranslator<NetAstConvertOperatorExpression, ShaderConversionAST>(T_ConversionCallExpression);
                AddTranslator<NetAstDoWhile, ShaderDoWhileStatementAST>(T_DoWhileStatement);
                AddTranslator<NetAstExpressionStatement, ShaderExpressionStatementAST>(T_ExpressionStatement);
                AddTranslator<NetAstFieldExpression, ShaderFieldInvokeAST>(T_FieldExpression);
                AddTranslator<NetAstFor, ShaderForStatementAST>(T_ForStatement);
                AddTranslator<NetAstLocalExpression, ShaderLocalInvokeAST>(T_LocalExpression);
                AddTranslator<NetAstMethodCallExpression, ShaderMethodInvokeAST>(T_MethodCallExpression);
                AddTranslator<NetAstArgumentExpression, ShaderParameterInvokeAST>(T_ParameterExpression);
                AddTranslator<NetAstReturn, ShaderReturnStatementAST>(T_ReturnStatement);
                AddTranslator<NetAstWhile, ShaderWhileStatementAST>(T_WhileStatement);
            }

            private void Register<MemberType>(Func<MemberType, ShaderMember> f) where MemberType : MemberInfo
            {
                compilers[typeof(MemberType)] = m => f(m as MemberType);
            }

            private ShaderMember Compile(MemberInfo member)
            {
                Type memberType = member.GetType();

                while (memberType != null && !compilers.ContainsKey(memberType))
                    memberType = memberType.BaseType;

                if (memberType == null)
                    throw new KeyNotFoundException();

                return compilers[memberType](member);
            }

            private void AddTranslator<NetNode, ShaderNode>(Func<NetNode, ShaderNode> translator)
                where NetNode : NetAstNode
                where ShaderNode : ShaderNodeAST
            {
                translators.Add(typeof(NetNode), n => translator((NetNode)n));
            }

            private bool IsNestedOnACompilableType(MemberInfo member, out Type compilableType)
            {
                Type declaringType = member.DeclaringType;

                while (declaringType != null)
                {
                    if (declaringType.GetCustomAttributes(typeof(ShaderTypeAttribute), false).Length > 0)
                    {
                        compilableType = declaringType;
                        return true;
                    }

                    declaringType = declaringType.DeclaringType;
                }
                compilableType = null;
                return false;
            }

            public T Resolve<T>(MemberInfo member) where T : ShaderMember
            {
                if (Scope.Contains(member))
                    return (T)Scope[member];

                Type compilableType;

                if (IsNestedOnACompilableType(member, out compilableType) && !(member is MethodInfo && (member as MethodInfo).IsStatic) && !(member is FieldInfo && (member as FieldInfo).IsStatic))
                {
                    // Resolve the container type and compile it.
                    Resolve<ShaderType>(compilableType);

                    // member should be on scope already.
                    if (Scope.Contains(member))
                        return (T)Scope[member];

                    // this is an error.
                    throw new ArgumentException("Member " + member + " is not public");
                }
                else
                {
                    if (member is FieldInfo && ((FieldInfo)member).IsStatic ||  // any field is compilable
                       (member is MethodInfo && (member == Scope.MainMethod || ((MethodInfo)member).GetCustomAttributes(typeof(ShaderMethodAttribute), false).Length > 0)) ||
                       (member is Type && (((Type)member).GetCustomAttributes(typeof(ShaderTypeAttribute), false).Length > 0))
                        ) // only marked methods are compilable
                        return (T)Compile(member);
                }

                throw new ArgumentException("Couldnt resolve member " + member);
            }

            #region Builders

            private Info.ParameterModifier GetModifier(ParameterInfo p)
            {
                Info.ParameterModifier modifier = Info.ParameterModifier.None;

                if (p.IsIn) modifier |= Info.ParameterModifier.In;
                if (p.IsOut) modifier |= Info.ParameterModifier.Out;

                return modifier;
            }

            private ShaderMethod BuildMethod(MethodInfo method)
            {
                ShaderMethodDeclarationAST methodDeclaration = (this.Scope.CurrentDeclaringType == null) ?
                    this.Program.DeclareNewMethod(method.Name,
                    Resolve<ShaderType>(method.ReturnType),
                    method.GetGenericArguments().Length)
                    :
                    Scope.CurrentDeclaringType.DeclareNewMethod(method.Name,
                    Resolve<ShaderType>(method.ReturnType),
                    method.GetGenericArguments().Length);

                var blockAST = IL.ILDecompiler.GetMethodBody(method);

                ShaderMethodBaseDeclarationAST lastMethod = Scope.CurrentDeclaringMethod;

                Scope.CurrentDeclaringMethod = methodDeclaration;

                foreach (var p in method.GetParameters())
                    methodDeclaration.DeclareNewParameter(Resolve<ShaderType>(p.ParameterType), p.Name, ShaderSemantics.Resolve(p), GetModifier(p));

                foreach (var instruction in blockAST.Instructions)
                    methodDeclaration.Body.Add(TranslateTo<ShaderStatementAST>(instruction));

                Scope.CurrentDeclaringMethod = lastMethod;

                Scope.Add(method, methodDeclaration.Method);

                return methodDeclaration.Method;
            }

            private ShaderType BuildType(Type type)
            {
                if (type.IsValueType)
                {
                    ShaderTypeDeclarationAST typeDeclaration = (this.Scope.CurrentDeclaringType == null) ?
                        this.Program.DeclareNewStruct(type.Name, type.GetGenericArguments().Length) :
                        Scope.CurrentDeclaringType.DeclareNewStruct(type.Name, type.GetGenericArguments().Length);

                    Scope.BeginType(typeDeclaration);

                    Scope.Add(type, typeDeclaration.Type);

                    foreach (var member in type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
                    {
                        if (member is MethodInfo)
                            BuildMethod(member as MethodInfo);

                        if (member is ConstructorInfo)
                            BuildConstructor(member as ConstructorInfo);

                        if (member is Type)
                            BuildType(member as Type);

                        if (member is FieldInfo)
                            BuildField(member as FieldInfo);
                    }

                    Scope.EndType();

                    Scope.Add(type, typeDeclaration.Type);

                    return typeDeclaration.Type;
                }

                return null;
            }

            private ShaderConstructor BuildConstructor(ConstructorInfo constructorInfo)
            {
                if (this.Scope.CurrentDeclaringType == null)
                    throw new Exception("Constructor can not be compiled for a program");

                ShaderConstructorDeclarationAST constructorDeclaration = Scope.CurrentDeclaringType.DeclareNewConstructor();

                var blockAST = IL.ILDecompiler.GetMethodBody(constructorInfo);

                Scope.CurrentDeclaringMethod = constructorDeclaration;

                foreach (var p in constructorInfo.GetParameters())
                    constructorDeclaration.DeclareNewParameter(Resolve<ShaderType>(p.ParameterType), p.Name, ShaderSemantics.Resolve(p), GetModifier(p));

                foreach (var instruction in blockAST.Instructions)
                    constructorDeclaration.Body.Add(TranslateTo<ShaderStatementAST>(instruction));

                Scope.CurrentDeclaringMethod = null;

                Scope.Add(constructorInfo, constructorDeclaration.Constructor);

                return constructorDeclaration.Constructor;
            }

            private ShaderField BuildField(FieldInfo field)
            {
                var fieldType = Resolve<ShaderType>(field.FieldType);
                var semantic = ShaderSemantics.Resolve(field);

                ShaderFieldDeclarationAST fieldDeclaration = Scope.CurrentDeclaringType == null || field.IsStatic ?
                    Program.DeclareNewField(field.Name, fieldType, semantic) :
                    Scope.CurrentDeclaringType.DeclareNewField(fieldType, field.Name, semantic);

                Scope.Add(field, fieldDeclaration.Field);

                if (field.IsStatic)
                    ConstantProxy.Add(fieldDeclaration.Field, (_this) => field.GetValue(_this));

                return fieldDeclaration.Field;
            }

            #endregion

            Dictionary<NetAstExpression, object> evaluations = new Dictionary<NetAstExpression, object>();

            bool errorDuringTransformation;

            static int GlobalID = 0;

            ShaderNodeAST Translate(NetAstNode node)
            {
                if (node == null) return null;

                if (node is NetAstExpression)
                {
                    var expression = node as NetAstExpression;

                    bool canEvaluate = expression.CanBeEvaluated;

                    if (canEvaluate)
                        evaluations.Add(expression, expression.Value);

                    ShaderNodeAST result = null;
                    try
                    {
                        result = translators[node.GetType()](node);
                    }
                    catch
                    {
                        errorDuringTransformation = true;
                    }

                    if (errorDuringTransformation && evaluations.Count == 1 && evaluations.ContainsKey(expression)) // more global expression evaluable
                    {
                        object value = evaluations[expression];
                        if (value != null)
                        {
                            try
                            {
                                ShaderType fieldType = Resolve<ShaderType>(value.GetType());

                                var fieldDeclaration = Program.DeclareNewField("__global" + (GlobalID++),
                                    fieldType, ShaderSemantics.Custom("GLOBAL"));

                                ConstantProxy.Add(fieldDeclaration.Field, (_this) => value);

                                return new ShaderFieldInvokeAST(Program,
                                    fieldDeclaration.Field, null);
                            }
                            catch
                            {
                            }
                        }
                    }

                    if (canEvaluate)
                        evaluations.Remove(expression);

                    return result;
                }
                else
                {
                    evaluations.Clear(); // just in case
                    errorDuringTransformation = false;
                    if (translators.ContainsKey(node.GetType()))
                        return translators[node.GetType()](node);
                }
                
                return null;
            }

            T TranslateTo<T>(NetAstNode node) where T : ShaderNodeAST
            {
                return (T)Translate(node);
            }

            #region TranslatorFunctions

            #region Statements

            ShaderLocalDeclarationAST T_LocalDeclaration(NetAstLocalDeclaration localDeclaration)
            {
                return new ShaderLocalDeclarationAST(Program,
                    Resolve<ShaderType>(localDeclaration.LocalInfo.Type),
                    localDeclaration.LocalInfo.Name);
            }

            ShaderExpressionStatementAST T_Assignament(NetAstAssignamentStatement assignament)
            {
                return new ShaderExpressionStatementAST(Program,
                    new ShaderAssignamentAST(
                    TranslateTo<ShaderExpressionAST>(assignament.LeftValue),
                    TranslateTo<ShaderExpressionAST>(assignament.Value)));
            }

            ShaderExpressionStatementAST T_ExpressionStatement(NetAstExpressionStatement statement)
            {
                return new ShaderExpressionStatementAST(Program, TranslateTo<ShaderExpressionAST>(statement.Expression));
            }

            ShaderReturnStatementAST T_ReturnStatement(NetAstReturn statement)
            {
                return new ShaderReturnStatementAST(Program, TranslateTo<ShaderExpressionAST>(statement.Expression));
            }

            ShaderWhileStatementAST T_WhileStatement(NetAstWhile statement)
            {
                return new ShaderWhileStatementAST(Program,
                    TranslateTo<ShaderExpressionAST>(statement.Condition),
                    TranslateTo<ShaderBlockStatementAST>(statement.Body));
            }

            ShaderDoWhileStatementAST T_DoWhileStatement(NetAstDoWhile statement)
            {
                return new ShaderDoWhileStatementAST(Program,
                    TranslateTo<ShaderExpressionAST>(statement.Condition),
                    TranslateTo<ShaderBlockStatementAST>(statement.Body));
            }

            ShaderForStatementAST T_ForStatement(NetAstFor statement)
            {
                return new ShaderForStatementAST(Program,
                    TranslateTo<ShaderStatementAST>(statement.InitAssign),
                    TranslateTo<ShaderExpressionAST>(statement.Condition),
                    TranslateTo<ShaderStatementAST>(statement.IterationStatement));
            }

            ShaderConditionalStatement T_ConditionalStatement(NetAstIf conditional)
            {
                return new ShaderConditionalStatement(Program,
                    TranslateTo<ShaderExpressionAST>(conditional.Condition),
                    TranslateTo<ShaderBlockStatementAST>(conditional.WhenTrue),
                    TranslateTo<ShaderBlockStatementAST>(conditional.WhenFalse));
            }

            #endregion

            #region Expressions

            ShaderConstantExpressionAST T_Constant(NetAstConstantExpression constant)
            {
                return new ShaderConstantExpressionAST(this.Program, Resolve<ShaderType>(constant.StaticType), constant.Value);
            }

            ShaderFieldInvokeAST T_FieldExpression(NetAstFieldExpression fieldExpression)
            {
                return new ShaderFieldInvokeAST(this.Program, Resolve<ShaderField>(fieldExpression.FieldInfo), TranslateTo<ShaderExpressionAST>(fieldExpression.LeftSide));
            }

            ShaderLocalInvokeAST T_LocalExpression(NetAstLocalExpression localExpression)
            {
                var local = Scope.CurrentDeclaringMethod.Body.Statements.OfType<ShaderLocalDeclarationAST>().
                    First(d => d.Name == localExpression.LocalInfo.Name).Local;

                return new ShaderLocalInvokeAST(Program, local);
            }

            ShaderParameterInvokeAST T_ParameterExpression(NetAstArgumentExpression argumentExpression)
            {
                return new ShaderParameterInvokeAST(Program, Scope.CurrentDeclaringMethod.Parameters.ElementAt(argumentExpression.ParameterInfo.Position));
            }

            ShaderOperationAST T_BinaryOperationExpression(NetAstBinaryOperatorExpression binaryExpression)
            {
                return new ShaderOperationAST(Program, (System.Compilers.Shaders.Operators)binaryExpression.Operator,
                    TranslateTo<ShaderExpressionAST>(binaryExpression.LeftOperand),
                    TranslateTo<ShaderExpressionAST>(binaryExpression.RightOperand));
            }

            ShaderOperationAST T_UnaryOperationExpression(NetAstUnaryOperatorExpression unaryExpression)
            {
                return new ShaderOperationAST(Program, (Operators)unaryExpression.Operator,
                    TranslateTo<ShaderExpressionAST>(unaryExpression.Operand));
            }

            ShaderMethodInvokeAST T_MethodCallExpression(NetAstMethodCallExpression methodCall)
            {
                ShaderExpressionAST leftSide = methodCall.MethodInfo.IsStatic ?
                    null : TranslateTo<ShaderExpressionAST>(methodCall.Arguments[0]);
                ShaderExpressionAST[] arguments = methodCall.MethodInfo.IsStatic ?
                    methodCall.Arguments.Select(a => TranslateTo<ShaderExpressionAST>(a)).ToArray() :
                    methodCall.Arguments.Skip(1).Select(a => TranslateTo<ShaderExpressionAST>(a)).ToArray();

                return new ShaderMethodInvokeAST(Program, Resolve<ShaderMethod>(methodCall.MethodInfo),
                    leftSide, arguments);
            }

            ShaderConstructorInvokeAST T_ConstructorCallExpression(NetAstConstructorCallExpression constructorCall)
            {
                ShaderExpressionAST[] arguments = constructorCall.Arguments.Select(a => TranslateTo<ShaderExpressionAST>(a)).ToArray();

                return new ShaderConstructorInvokeAST(Program, Resolve<ShaderConstructor>(constructorCall.ConstructorInfo),
                    arguments);
            }

            ShaderConversionAST T_ConversionCallExpression(NetAstConvertOperatorExpression conversion)
            {
                ShaderType targetType = Resolve<ShaderType>(conversion.TargetType);

                ShaderExpressionAST expression = TranslateTo<ShaderExpressionAST>(conversion.Operand);

                return new ShaderConversionAST(Program, targetType, expression);
            }

            #endregion

            #endregion

            public void ResolveMain(MethodInfo main)
            {
                Scope.BeginType(null);
                Scope.MainMethod = main;
                var m = Resolve<ShaderMethod>(main);
                Program.Main = m;
                Scope.EndType();
            }

            public ShaderProgramAST Program
            {
                get;
                private set;
            }
        }


    }
}
