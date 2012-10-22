using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.AST;
using System.Compilers.Shaders.ShaderAST;
using System.Compilers.Shaders;
using System.Reflection;
using System.Compilers.Shaders.Info;
using System.Maths;
using System.Runtime.InteropServices;

namespace System.Compilers.Transformers
{
    public class NetToShaderASTTransformer : ASTTransformer<NetAstNode, ShaderNodeAST>,
        ITransformerOf<NetAstAssignamentStatement, ShaderExpressionStatementAST>,
        ITransformerOf<NetAstExpressionStatement, ShaderExpressionStatementAST>,
        ITransformerOf<NetAstLocalDeclaration, ShaderLocalDeclarationAST>,
        ITransformerOf<NetAstStatement, ShaderStatementAST>,
        ITransformerOf<NetAstReturn, ShaderReturnStatementAST>,
        ITransformerOf<NetAstBlock, ShaderBlockStatementAST>,
        ITransformerOf<NetAstIf, ShaderConditionalStatement>,
        ITransformerOf<NetAstWhile, ShaderWhileStatementAST>,
        ITransformerOf<NetAstDoWhile, ShaderDoWhileStatementAST>,
        ITransformerOf<NetAstFor, ShaderForStatementAST>,

        ITransformerOf<NetAstExpression, ShaderExpressionAST>,
        ITransformerOf<NetAstBinaryOperatorExpression, ShaderOperationAST>,
        ITransformerOf<NetAstUnaryOperatorExpression, ShaderOperationAST>,
        ITransformerOf<NetAstInvokeExpression, ShaderExpressionAST>,
        ITransformerOf<NetAstMethodBaseCallExpression, ShaderMethodBaseInvokeAST>,
        ITransformerOf<NetAstMethodCallExpression, ShaderExpressionAST>,
        ITransformerOf<NetAstConstructorCallExpression, ShaderConstructorInvokeAST>,
        ITransformerOf<NetAstInitObjectExpression, ShaderInitializationInvokeAST>,
        ITransformerOf<NetAstConvertOperatorExpression, ShaderConversionAST>,
        ITransformerOf<NetAstFieldExpression, ShaderFieldInvokeAST>,
        ITransformerOf<NetAstLocalExpression, ShaderLocalInvokeAST>,
        ITransformerOf<NetAstArgumentExpression, ShaderParameterInvokeAST>,
        ITransformerOf<NetAstConstantExpression, ShaderConstantExpressionAST>,

        ITransformerOf<NetMemberDeclarationAST, ShaderMemberDeclarationAST>,
        ITransformerOf<NetMethodBaseDeclarationAST, ShaderMethodBaseDeclarationAST>,
        ITransformerOf<NetMethodDeclarationAST, ShaderMethodDeclarationAST>,
        ITransformerOf<NetConstructorDeclarationAST, ShaderConstructorDeclarationAST>,
        ITransformerOf<NetFieldDeclarationAST, ShaderFieldDeclarationAST>,
        ITransformerOf<NetTypeDeclarationAST, ShaderTypeDeclarationAST>
    {

        #region Constructors

        public NetToShaderASTTransformer(ShaderProgramAST program)
        {
            this.CurrentScope = new Scope(program.Builtins, program);

            this.ConstantProxy = new ConstantProxy();

            this.Dynamics = new DynamicMembers();
        }

        public NetToShaderASTTransformer(Builtins builtins)
        {
            this.CurrentScope = new Scope(builtins, new ShaderProgramAST(builtins));

            this.ConstantProxy = new ConstantProxy();
            
            this.Dynamics = new DynamicMembers();
        }

        #endregion

        public Scope CurrentScope { get; private set; }

        public ConstantProxy ConstantProxy { get; private set; }

        public DynamicMembers Dynamics { get; private set; }

        #region Scope

        public class Scope
        {
            public Scope(Scope baseScope)
            {
                if (baseScope == null)
                    throw new ArgumentNullException();

                this.Base = baseScope;
                this.compiledMembers = baseScope.compiledMembers;
                this.Builtins = baseScope.Builtins;
                this.Program = baseScope.Program;
            }

            public Scope(Builtins builtins, ShaderProgramAST program)
            {
                this.Builtins = builtins;
                this.Program = program;
                this.Base = null;
                this.compiledMembers = new Dictionary<MemberInfo, ShaderMember>();
            }

            public Builtins Builtins { get; private set; }

            public ShaderProgramAST Program { get; private set; }

            public Scope Base { get; private set; }

            public ShaderTypeDeclarationAST CurrentType { get; private set; }

            public ShaderMethodBaseDeclarationAST CurrentMethod { get; private set; }

            public Scope CreateTypeScope(ShaderTypeDeclarationAST typeDeclaration)
            {
                return new Scope(this) { CurrentType = typeDeclaration };
            }

            public Scope CreateMethodScope(ShaderMethodBaseDeclarationAST methodDeclaration)
            {
                return new Scope(this) { CurrentMethod = methodDeclaration };
            }

            Dictionary<MemberInfo, ShaderMember> compiledMembers = new Dictionary<MemberInfo, ShaderMember>();

            public void Add(MemberInfo member, ShaderMember shaderMember)
            {
                if (member == null)
                    throw new ArgumentNullException("member");

                if (shaderMember == null)
                    throw new ArgumentNullException("shaderMember");

                compiledMembers.Add(member, shaderMember);
            }

            public bool Contains(MemberInfo member)
            {
                return Builtins.Contains(member) || compiledMembers.ContainsKey(member);
            }

            public ShaderMethod this[MethodInfo method]
            {
                get
                {
                    if (Builtins.Contains(method))
                        return Builtins.Resolve(method);

                    return (ShaderMethod)compiledMembers[method];
                }
            }

            public ShaderType this[Type type]
            {
                get
                {
                    if (Builtins.Contains(type))
                        return Builtins.Resolve(type);

                    return (ShaderType)compiledMembers[type];
                }
            }

            public ShaderConstructor this[ConstructorInfo constructor]
            {
                get
                {
                    if (Builtins.Contains(constructor))
                        return Builtins.Resolve(constructor);

                    return (ShaderConstructor)compiledMembers[constructor];
                }
            }

            public ShaderField this[FieldInfo field]
            {
                get
                {
                    if (Builtins.Contains(field))
                        return Builtins.Resolve(field);

                    return (ShaderField)compiledMembers[field];
                }
            }

            public Scope CreateGlobalScope()
            {
                return new Scope(this);
            }
        }

        public ShaderProgramAST Program { get { return CurrentScope.Program; } }

        #endregion

        #region Builders

        private System.Compilers.Shaders.Info.ParameterModifier GetModifier(ParameterInfo p)
        {
            System.Compilers.Shaders.Info.ParameterModifier modifier = System.Compilers.Shaders.Info.ParameterModifier.None;

            if (p.IsIn) modifier |= System.Compilers.Shaders.Info.ParameterModifier.In;
            if (p.IsOut) modifier |= System.Compilers.Shaders.Info.ParameterModifier.Out;

            return modifier;
        }

        private ShaderMethod Build(MethodInfo method)
        {
            bool isAbstract = method.DeclaringType.IsSubclassOf(typeof(Delegate)) || method.IsAbstract;

            ShaderMethodDeclarationAST methodDeclaration = (this.CurrentScope.CurrentType == null) ?
                this.Program.DeclareNewMethod(method.Name, isAbstract,
                method.GetGenericArguments().Length)
                :
                CurrentScope.CurrentType.DeclareNewMethod(method.Name, isAbstract,
                method.GetGenericArguments().Length);

            var returnType = Resolve(method.ReturnType);

            if (returnType == null)
                return null;
            
            Program.SetReturnType(methodDeclaration, returnType);

            if (!method.IsStatic && !method.DeclaringType.IsSubclassOf(typeof(Delegate)))
            {
                var declaringType = Resolve(method.DeclaringType);
                if (declaringType != null)
                    methodDeclaration.DeclareNewParameter(declaringType, "self", null, Shaders.Info.ParameterModifier.In);
            }

            foreach (var p in method.GetParameters())
            {
                var parameterType = Resolve(p.ParameterType);
                if (parameterType == null)
                    return null;
                methodDeclaration.DeclareNewParameter(parameterType, p.Name, ShaderSemantics.Resolve(p), GetModifier(p));
            }

            if (!isAbstract)
            {
                var blockAST = IL.ILDecompiler.GetMethodBody(method);

                CurrentScope = CurrentScope.CreateMethodScope(methodDeclaration);
                
                foreach (var instruction in blockAST.Instructions)
                    methodDeclaration.Body.StatementsList.Add(Transform(instruction));

                CurrentScope = CurrentScope.Base;
            }

            CurrentScope.Add(method, methodDeclaration.Method);

            return methodDeclaration.Method;
        }

        private ShaderType Build(Type type)
        {
            if (type.IsValueType)
            {
                ShaderTypeDeclarationAST typeDeclaration = (this.CurrentScope.CurrentType == null) ?
                    this.Program.DeclareNewStruct(type.Name, type.GetGenericArguments().Length) :
                    CurrentScope.CurrentType.DeclareNewStruct(type.Name, type.GetGenericArguments().Length);

                CurrentScope = CurrentScope.CreateTypeScope(typeDeclaration);

                CurrentScope.Add(type, typeDeclaration.Type);

                foreach (var member in type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public).OrderBy(m=>m is FieldInfo? (int)Marshal.OffsetOf(type, m.Name): int.MaxValue))
                    if (!CurrentScope.Contains(member))
                    {
                        if (member is MethodInfo)
                            Build(member as MethodInfo);

                        if (member is ConstructorInfo)
                            Build(member as ConstructorInfo);

                        if (member is Type)
                            Build(member as Type);

                        if (member is FieldInfo)
                            Build(member as FieldInfo);
                    }

                CurrentScope = CurrentScope.Base;

                return typeDeclaration.Type;
            }

            return null;
        }

        private ShaderConstructor Build(ConstructorInfo constructorInfo)
        {
            if (CurrentScope.CurrentType == null)
                throw new Exception("Constructor can not be compiled for a program");

            ShaderConstructorDeclarationAST constructorDeclaration = CurrentScope.CurrentType.DeclareNewConstructor();

            var blockAST = IL.ILDecompiler.GetMethodBody(constructorInfo);

            CurrentScope = CurrentScope.CreateMethodScope(constructorDeclaration);

            foreach (var p in constructorInfo.GetParameters())
                constructorDeclaration.DeclareNewParameter(Resolve(p.ParameterType), p.Name, ShaderSemantics.Resolve(p), GetModifier(p));

            foreach (var instruction in blockAST.Instructions)
                constructorDeclaration.Body.StatementsList.Add(Transform(instruction));

            CurrentScope = CurrentScope.Base;

            CurrentScope.Add(constructorInfo, constructorDeclaration.Constructor);

            return constructorDeclaration.Constructor;
        }

        private ShaderField Build(FieldInfo field)
        {
            var fieldType = Resolve(field.FieldType);

            if (fieldType == null)
                return null;

            if (fieldType.IsArray)
            {
                var atts = field.GetCustomAttributes(typeof(ArrayLengthAttribute), true) as ArrayLengthAttribute[];
                if (atts.Length == 0)
                    throw new ArgumentException("Fields used as arrays should have length specified via ArrayLength attribute");

                fieldType = fieldType.ElementType.MakeFixedArray(new int[] { atts[0].Length });
            }

            var semantic = ShaderSemantics.Resolve(field);

            ShaderFieldDeclarationAST fieldDeclaration = CurrentScope.CurrentType == null || field.IsStatic ?
                Program.DeclareNewField(field.Name, fieldType, semantic) :
                CurrentScope.CurrentType.DeclareNewField(fieldType, field.Name, semantic);

            CurrentScope.Add(field, fieldDeclaration.Field);

            return fieldDeclaration.Field;
        }

        #endregion

        private Type CreateDelegateType(Type returnType, params Type[] argumentTypes)
        {
            if (returnType == typeof(void))
            {
                Type[] typeArguments = argumentTypes;
                switch (argumentTypes.Length)
                {
                    case 0: return typeof(Action);
                    case 1: return typeof(Action<>).MakeGenericType(typeArguments);
                    case 2: return typeof(Action<,>).MakeGenericType(typeArguments);
                    case 3: return typeof(Action<,,>).MakeGenericType(typeArguments);
                    case 4: return typeof(Action<,,,>).MakeGenericType(typeArguments);
                    default: throw new NotSupportedException();
                }
            }
            else
            {
                Type[] typeArguments = argumentTypes.Concat(new Type[] { returnType }).ToArray();
                switch (argumentTypes.Length)
                {
                    case 0: return typeof(Func<>).MakeGenericType(typeArguments);
                    case 1: return typeof(Func<,>).MakeGenericType(typeArguments);
                    case 2: return typeof(Func<,,>).MakeGenericType(typeArguments);
                    case 3: return typeof(Func<,,,>).MakeGenericType(typeArguments);
                    case 4: return typeof(Func<,,,,>).MakeGenericType(typeArguments);
                    default: throw new NotSupportedException();
                }
            }
        }

        private ShaderMethod Resolve(MethodInfo method, NetAstExpression leftSide)
        {
            if (CurrentScope.Contains(method))
                return CurrentScope[method]; // it is already set.

            if (CheckIfItIsOnACompilableType(method) && CurrentScope.Contains(method)) // if has a compilable type as parent, compile it.
                return CurrentScope[method]; // it is already set.

            if (/*Program.Builtins.Contains(method.DeclaringType) ||*/
                method.DeclaringType.IsSubclassOf(typeof(Delegate)) || 
                method.GetCustomAttributes(typeof(ShaderMethodAttribute), false).Length > 0)
            {
                var m = Build(method);

                if (m.IsAbstract)
                {
                    if (leftSide == null || leftSide.CanBeEvaluated)
                    {
                        Func<object, Delegate> resolver;

                        if (method.DeclaringType.IsSubclassOf(typeof(Delegate))) // func members
                        {
                            resolver = (_this) => (Delegate)leftSide.GetValue(_this);
                        }
                        else // abstract methods
                        {
                            Type delegateType = CreateDelegateType(method.ReturnType, method.GetParameters().Select(p => p.ParameterType).ToArray());
                            resolver = (_this) => Delegate.CreateDelegate(delegateType, leftSide == null ? null : leftSide.GetValue(_this), method);
                        }

                        Dynamics.Add(m, resolver);
                    }
                    else
                    {
                        RaiseLog("Could not resolve method " + method +" because leftside can not be evaluated");
                        return null;
                    }
                }
                return m;
            }

            RaiseLog("Could not resolve method " + method + " because is not marked as ShaderMethod");

            return null;
        }

        private ShaderConstructor Resolve(ConstructorInfo constructor)
        {
            if (CurrentScope.Contains(constructor))
                return CurrentScope[constructor];

            if (CheckIfItIsOnACompilableType(constructor) && CurrentScope.Contains(constructor))
                return CurrentScope[constructor]; // it is already set.

            if (constructor.GetCustomAttributes(typeof(ShaderMethodAttribute), false).Length > 0)
                return Build(constructor);

            RaiseLog("Could not resolve constructor " + constructor);
            return null;
        }

        private ShaderType Resolve(Type type)
        {
            if (type.IsByRef)
                return Resolve(type.GetElementType());
            if (type.IsEnum)
                return CurrentScope.Builtins.Int;

            if (type.IsArray)
            {
                ShaderType elementType = Resolve(type.GetElementType());
                if (elementType == null)
                    return null;
                return elementType.MakeArray(type.GetArrayRank());
            }

            if (CurrentScope.Contains(type))
                return CurrentScope[type];

            if (CheckIfItIsOnACompilableType(type) && CurrentScope.Contains(type))
                return CurrentScope[type]; // it is already set.

            if (type.IsValueType && !type.IsPrimitive)
            {
                Program.PushMode(ShaderProgramAST.AddingMode.Prepend);
                CurrentScope = CurrentScope.CreateGlobalScope();
                var t = Build(type);
                CurrentScope = CurrentScope.Base;
                Program.PopMode();
                return t;
            }

            RaiseLog("Could not resolve type " + type);

            return null;
        }

        private ShaderField Resolve(FieldInfo field, NetAstExpression leftSide)
        {
            if (CurrentScope.Contains(field))
                return CurrentScope[field];

            if (CheckIfItIsOnACompilableType(field) && CurrentScope.Contains(field))
                return CurrentScope[field]; // it is already set.

            var fieldBuilt = Build(field);
            if (fieldBuilt == null)
                return null;

            if (leftSide == null || leftSide.CanBeEvaluated)
                ConstantProxy.Add(fieldBuilt, (_this) => field.GetValue(leftSide == null ? null :
                    leftSide.GetValue(_this)));

            return fieldBuilt;
        }

        private bool CheckIfItIsOnACompilableType(MemberInfo member)
        {
            if (member.DeclaringType == null)
                return false;

            if (CheckIfItIsOnACompilableType(member.DeclaringType))
                return true;

            if (member.DeclaringType.GetCustomAttributes(typeof(ShaderTypeAttribute), false).Length > 0)
            {
                var type = Resolve(member.DeclaringType);
                if (type == null)
                    return false;
                return true;
            }

            return false;
        }

        #region Statements

        public ShaderStatementAST Transform(NetAstStatement source)
        {
            return
                TryTransform<NetAstAssignamentStatement, ShaderExpressionStatementAST>(source) ??
                TryTransform<NetAstBlock, ShaderBlockStatementAST>(source) ??
                TryTransform<NetAstExpressionStatement, ShaderExpressionStatementAST>(source) ??
                TryTransform<NetAstFor, ShaderForStatementAST>(source) ??
                TryTransform<NetAstLocalDeclaration, ShaderLocalDeclarationAST>(source) ??
                TryTransform<NetAstIf, ShaderConditionalStatement>(source) ??
                ResolveDynamically<NetAstStatement, ShaderStatementAST>(source);
        }

        public ShaderExpressionStatementAST Transform(NetAstAssignamentStatement source)
        {
            if (source.LeftValue is NetAstLocalExpression && source.LeftValue.StaticType.GetCustomAttributes(typeof(OnlyGlobalDeclarationAttribute), true).Length > 0)
            {
                var globalExpression = Transform(source.Value) as ShaderFieldInvokeAST;

                var dec = CurrentScope.Program.Members.OfType<ShaderFieldDeclarationAST>().First(f => globalExpression.Field.Equals(f.Field));
                resolvedAsGlobals[source.LeftValue] = dec;
                return null;
            }
            var leftvalue = Transform(source.LeftValue);

            if (leftvalue == null)
                return null;

            var value = Transform(source.Value);

            if (value == null)
                return null;
 
            return Program.CreateExpressionStatement(Program.CreateAssignament(
                    leftvalue,
                    value));
        }

        public ShaderLocalDeclarationAST Transform(NetAstLocalDeclaration source)
        {
            if (source.LocalInfo.Type.GetCustomAttributes(typeof(OnlyGlobalDeclarationAttribute), true).Length > 0)
                return null;
            var localType = Resolve(source.LocalInfo.Type);
            if (localType == null)
                return null;
            return Program.CreateLocalDeclaration(localType, source.LocalInfo.Name);
        }

        public ShaderReturnStatementAST Transform(NetAstReturn source)
        {
            var exp = Transform(source.Expression);
         
            if (exp == null && source.Expression != null)
                return null;
            
            return Program.CreateReturn(exp);
        }

        public ShaderConditionalStatement Transform(NetAstIf source)
        {
            var condition = Transform(source.Condition);
            if (condition == null)
                return null;
            var whenTrue = Transform(source.WhenTrue);
            if (whenTrue == null)
                return null;
            var whenFalse = source.WhenFalse == null ? null : Transform(source.WhenFalse);
            if (whenFalse == null && source.WhenFalse != null)
                return null;
            
            return Program.CreateConditional(
                condition,
                whenTrue,
                whenFalse
                );
        }

        public ShaderBlockStatementAST Transform(NetAstBlock source)
        {
            return Program.CreateBlock(source.Instructions.Select(i =>
                Transform(i)).Where(shaderStatement => shaderStatement != null).ToArray());
        }

        public ShaderWhileStatementAST Transform(NetAstWhile source)
        {
            var condition = Transform(source.Condition);
            if (condition == null)
                return null;
            var body = Transform(source.Body);
            if (body == null)
                return null;
            return Program.CreateWhile(condition,
                body);
        }

        public ShaderDoWhileStatementAST Transform(NetAstDoWhile source)
        {
            var condition = Transform(source.Condition);
            if (condition == null)
                return null;
            var body = Transform(source.Body);
            if (body == null)
                return null;
            return Program.CreateDoWhile(condition,
                body);
        }

        public ShaderForStatementAST Transform(NetAstFor source)
        {
            var initAssign = Transform(source.InitAssign);
            if (initAssign == null)
                return null;

            var condition = Transform(source.Condition);
            if (condition == null)
                return null;

            var iterationStatement = Transform(source.IterationStatement);
            if (iterationStatement == null)
                return null;

            var body = Transform(source.Body);
            if (body == null)
                return null;

            return Program.CreateFor(
                initAssign,
                condition,
                iterationStatement,
                body);
        }

        #endregion

        #region Expressions

        static int globals = 0;

        Dictionary<NetAstExpression, ShaderFieldDeclarationAST> resolvedAsGlobals = new Dictionary<NetAstExpression, ShaderFieldDeclarationAST>();

        public ShaderExpressionAST Transform(NetAstExpression source)
        {
            if (resolvedAsGlobals.ContainsKey(source))
                return new ShaderFieldInvokeAST(Program, resolvedAsGlobals[source].Field, null);

            var exp =  
                    TryTransform<NetAstConstantExpression, ShaderConstantExpressionAST>(source)??
                    TryTransform<NetAstConvertOperatorExpression, ShaderConversionAST>(source)??
                    TryTransform<NetAstInvokeExpression, ShaderExpressionAST>(source)??
                    TryTransform<NetAstLocalExpression, ShaderLocalInvokeAST>(source)??
                    TryTransform<NetAstArgumentExpression, ShaderParameterInvokeAST>(source)??
                    TryTransform<NetAstBinaryOperatorExpression, ShaderOperationAST>(source)??
                    TryTransform<NetAstUnaryOperatorExpression, ShaderOperationAST>(source)??
                    TryTransform<NetAstTernaryOperatorExpressionv, ShaderOperationAST>(source)??
                    TryTransform<NetArrayAccessExpression, ShaderOperationAST>(source)??
                    TryTransform<NetAstThisExpression, ShaderExpressionAST>(source)??
                    ResolveDynamically<NetAstExpression, ShaderExpressionAST>(source);

            if (exp == null)
            {
                if (source.StaticType == typeof(void))
                {
                    RaiseLog("Error trying " + source.ToString() + " as shader expression.");
                    return null;
                }
                if (source.CanBeEvaluated)
                {
                    var type = Resolve(source.StaticType);

                    if (type == null)
                        return null;

                    var fieldDeclaration = Program.DeclareNewField("_global" + (globals++), type, null);
                    ConstantProxy.Add(fieldDeclaration.Field, (_this) =>
                    {
                        return source.GetValue(_this);
                    });

                    resolvedAsGlobals[source] = fieldDeclaration;

                    return new ShaderFieldInvokeAST(Program, fieldDeclaration.Field, null);
                }
                else
                {
                    RaiseLog("Can not convert " + source.ToString());
                    return null;
                }
            }
            else
                return exp;
        }

        public ShaderOperationAST Transform(NetAstBinaryOperatorExpression source)
        {
            var leftOp = Transform(source.LeftOperand);
            if (leftOp == null)
                return null;
            var rightOp = Transform(source.RightOperand);
            if (rightOp == null)
                return null;
            return Program.CreateOperation((Operators)source.Operator,
                        leftOp,
                        rightOp);
        }

        public ShaderOperationAST Transform(NetArrayAccessExpression source)
        {
            var arrayExpression = Transform(source.ArrayExpression);
            if (arrayExpression == null)
                return null;
            var indexExpression = Transform(source.IndexExpression);
            if (indexExpression == null)
                return null;
            return new ShaderOperationAST(Program, Operators.Indexer, arrayExpression, indexExpression);
        }

        public ShaderOperationAST Transform(NetAstUnaryOperatorExpression source)
        {
            if (source.Operator == Operators.Cast)
            {
                RaiseLog("Transforms of conversion should be treated as NetAstConvertOperatorExpression");
                return null;
            }
            var op = Transform(source.Operand);
            if (op == null)
                return null;
            return Program.CreateOperation((Operators)source.Operator,
                    op);
        }

        public ShaderMethodBaseInvokeAST Transform(NetAstMethodBaseCallExpression source)
        {
            return ResolveDynamically<NetAstMethodBaseCallExpression, ShaderMethodBaseInvokeAST>(source);
        }

        public ShaderExpressionAST Transform(NetAstMethodCallExpression source)
        {
            ShaderExpressionAST leftSide;
            if (
                source.LeftSide == null ||
                source.LeftSide is NetAstThisExpression ||
                source.LeftSide.StaticType.IsSubclassOf(typeof(Delegate)))
                leftSide = null;
            else
            {
                leftSide = Transform(source.LeftSide);
                if (leftSide == null)
                    return null;
            }

            ShaderExpressionAST[] arguments = source.Parameters.Select(a => Transform(a)).ToArray();
            if (arguments.Any(a => a == null))
                return null;

            var shaderMethod = Resolve(source.MethodInfo, source.LeftSide);
            if (shaderMethod == null)
                return null;

            if (shaderMethod.Operator == Operators.None)
                return Program.CreateInvoke(shaderMethod, leftSide, arguments);
            else
                return Program.CreateOperation(shaderMethod.Operator, arguments);
        }

        public ShaderConstructorInvokeAST Transform(NetAstConstructorCallExpression source)
        {
            ShaderExpressionAST[] arguments = source.Parameters.Select(a => Transform(a)).ToArray();

            if (arguments.Any(a => a == null))
                return null;

            var constructor = Resolve(source.ConstructorInfo);
            if (constructor == null)
                return null;

            return Program.CreateInvoke(constructor,
                arguments);
        }

        public ShaderConversionAST Transform(NetAstConvertOperatorExpression source)
        {
            ShaderType targetType = Resolve(source.TargetType);

            if (targetType == null)
                return null;

            ShaderExpressionAST expression = Transform(source.Operand);

            if (expression == null)
                return null;

            return Program.CreateConversion(targetType, expression);
        }

        public ShaderFieldInvokeAST Transform(NetAstFieldExpression source)
        {
            var leftSide = source.LeftSide == null || source.LeftSide is NetAstThisExpression ? null : Transform(source.LeftSide);

            if (leftSide == null && !(source.LeftSide == null || source.LeftSide is NetAstThisExpression))
                return null;

            var field = Resolve(source.FieldInfo, source.LeftSide);
            if (field == null)
                return null;

            return Program.CreateInvoke(field, leftSide);
        }

        public ShaderLocalInvokeAST Transform(NetAstLocalExpression source)
        {
            var local = CurrentScope.CurrentMethod.Body.Statements.OfType<ShaderLocalDeclarationAST>().
                    FirstOrDefault(d => d.Name == source.LocalInfo.Name).Local;

            if (local == null)
            {
                RaiseLog("Can not found local variable " + source.LocalInfo.Name);
                return null;
            }

            return Program.CreateInvoke(local);
        }

        public ShaderConstantExpressionAST Transform(NetAstConstantExpression source)
        {
            return Program.CreateConstant(source.StaticType, source.Value);
        }

        public ShaderParameterInvokeAST Transform(NetAstArgumentExpression source)
        {
            return Program.CreateInvoke(CurrentScope.CurrentMethod.Parameters.First(p => p.Name == source.ParameterInfo.Name));
        }

        #endregion

        public ShaderFieldDeclarationAST Transform(NetFieldDeclarationAST source)
        {
            var field = source.Member;
            var fieldType = Resolve(field.FieldType);

            if (fieldType == null)
                return null;

            var semantic = ShaderSemantics.Resolve(field);

            ShaderFieldDeclarationAST fieldDeclaration = CurrentScope.CurrentType == null ?
                CurrentScope.Program.DeclareNewField(field.Name, fieldType, semantic) :
                CurrentScope.CurrentType.DeclareNewField(fieldType, field.Name, semantic);

            CurrentScope.Add(field, fieldDeclaration.Member);

            return fieldDeclaration;
        }

        public ShaderMethodBaseDeclarationAST Transform(NetMethodBaseDeclarationAST source)
        {
            return ResolveDynamically<NetMethodBaseDeclarationAST, ShaderMethodBaseDeclarationAST>(source);
        }

        public ShaderMethodDeclarationAST Transform(NetMethodDeclarationAST source)
        {
            var method = source.Member;

            ShaderMethodDeclarationAST methodDeclaration = CurrentScope.CurrentType == null ?
                CurrentScope.Program.DeclareNewMethod(method.Name, method.GetGenericArguments().Length) :
                CurrentScope.CurrentType.DeclareNewMethod(method.Name, method.GetGenericArguments().Length);

            var methodType = Resolve(method.ReturnType);

            if (methodType == null)
                return null;

            Program.SetReturnType(methodDeclaration, methodType);

            CurrentScope.Add(method, methodDeclaration.Member);

            CurrentScope = CurrentScope.CreateMethodScope(methodDeclaration);

            foreach (var p in method.GetParameters())
            {
                var parameterType = Resolve(p.ParameterType);
                methodDeclaration.DeclareNewParameter(parameterType,
                    p.Name, ShaderSemantics.Resolve(p), GetModifier(p));
            }

            var instructions = IL.ILDecompiler.GetMethodBody(method).Instructions;
            foreach (var i in instructions)
            {
                var shaderStatement = Transform(i);
                if (shaderStatement != null)
                    methodDeclaration.Body.StatementsList.Add(shaderStatement);
            }

            CurrentScope = CurrentScope.Base;

            return methodDeclaration;
        }

        public ShaderTypeDeclarationAST Transform(NetTypeDeclarationAST source)
        {
            var type = source.Member as Type;

            if (!source.IsStruct)
                throw new NotSupportedException();

            ShaderTypeDeclarationAST typeDeclaration = CurrentScope.CurrentType == null ?
                CurrentScope.Program.DeclareNewStruct(type.Name, type.GetGenericArguments().Length) :
                CurrentScope.CurrentType.DeclareNewStruct(type.Name, type.GetGenericArguments().Length);

            CurrentScope.Add(type, typeDeclaration.Member);

            CurrentScope = CurrentScope.CreateTypeScope(typeDeclaration);

            foreach (var memberDeclaration in source.Members)
            {
                var m = Transform(memberDeclaration);
            }

            CurrentScope = CurrentScope.Base;

            return typeDeclaration;
        }

        public ShaderConstructorDeclarationAST Transform(NetConstructorDeclarationAST source)
        {
            var constructor = source.Member;

            if (CurrentScope.CurrentType == null)
                throw new InvalidOperationException();

            ShaderConstructorDeclarationAST constructorDeclaration = CurrentScope.CurrentType.DeclareNewConstructor();

            CurrentScope.Add(constructor, constructorDeclaration.Member);

            CurrentScope = CurrentScope.CreateMethodScope(constructorDeclaration);

            foreach (var p in constructor.GetParameters())
            {
                var parameterType = Resolve(p.ParameterType);
                constructorDeclaration.DeclareNewParameter(parameterType,
                    p.Name, ShaderSemantics.Resolve(p), GetModifier(p));
            }

            foreach (var i in IL.ILDecompiler.GetMethodBody(constructor).Instructions)
            {
                var statement = Transform(i);
                if (statement != null)
                    constructorDeclaration.Body.StatementsList.Add(statement);
            }

            CurrentScope = CurrentScope.Base;

            return constructorDeclaration;
        }

        public ShaderMemberDeclarationAST Transform(NetMemberDeclarationAST source)
        {
            return 
                TryTransform<NetFieldDeclarationAST, ShaderFieldDeclarationAST>(source)??
                TryTransform<NetMethodDeclarationAST, ShaderMethodDeclarationAST>(source) ??
                TryTransform<NetTypeDeclarationAST, ShaderTypeDeclarationAST>(source) ??
                TryTransform<NetConstructorDeclarationAST, ShaderConstructorDeclarationAST>(source) ??
                ResolveDynamically<NetMemberDeclarationAST, ShaderMemberDeclarationAST>(source);
        }

        public ShaderExpressionAST Transform(NetAstInvokeExpression source)
        {
            return 
                TryTransform<NetAstFieldExpression, ShaderFieldInvokeAST>(source)??
                TryTransform<NetAstMethodCallExpression, ShaderExpressionAST>(source)??
                TryTransform<NetAstConstructorCallExpression, ShaderConstructorInvokeAST>(source)??
                ResolveDynamically<NetAstInvokeExpression, ShaderMemberInvokeAST>(source);
        }

        public ShaderInitializationInvokeAST Transform(NetAstInitObjectExpression source)
        {
            var type = Resolve(source.TargetType);
            if (type == null)
                return null;
            return new ShaderInitializationInvokeAST(CurrentScope.Program, type);
        }

        public ShaderExpressionStatementAST Transform(NetAstExpressionStatement source)
        {
            var exp = Transform(source.Expression);

            if (exp == null)
                return Program.CreateExpressionStatement(null);
            else
                return Program.CreateExpressionStatement(Transform(source.Expression));
        }

        protected void RaiseLog(string message)
        {
            if (Log != null)
                Log(message);
        }

        protected void RaiseLog(Exception e)
        {
            RaiseLog(e.Message);
        }

        public event Action<string> Log;
    }
    #region Constant Proxy

    public class ConstantProxy : IConstantProxy
    {
        Dictionary<ShaderField, Func<object, object>> resolvers = new Dictionary<ShaderField, Func<object, object>>();

        public ConstantProxy()
        {
        }

        public void Add(ShaderField key, Func<object, object> resolver)
        {
            resolvers[key] = resolver;
        }

        public IEnumerable<ShaderField> Fields
        {
            get
            {
                return resolvers.Keys;
            }
        }

        public object GetValueFor(object target, ShaderField field)
        {
            return resolvers[field].Invoke(target);
        }
    }

    #endregion

    public class DynamicMembers : IDynamicMembers
    {
        Dictionary<ShaderMethod, Func<object, Delegate>> resolvers = new Dictionary<ShaderMethod, Func<object, Delegate>>();

        public void Add(ShaderMethod method, Func<object, Delegate> resolver)
        {
            this.resolvers[method] = resolver;
        }

        public IEnumerable<ShaderMethod> Methods
        {
            get
            {
                return resolvers.Keys;
            }
        }

        public Delegate GetMethod(object target, ShaderMethod method)
        {
            return resolvers[method].Invoke(target);
        }
    }
}
