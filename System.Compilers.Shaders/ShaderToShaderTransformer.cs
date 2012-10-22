using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.ShaderAST;
using System.Compilers.Shaders.Info;
using System.Compilers.Transformers;

namespace System.Compilers.Shaders
{
    public class ShaderToShaderTransformer : ASTTransformer<ShaderNodeAST, ShaderNodeAST>,
        
        ITransformerOf<ShaderProgramAST, ShaderProgramAST>,

        ITransformerOf<ShaderMemberDeclarationAST, ShaderMemberDeclarationAST>,
        ITransformerOf<ShaderTypeDeclarationAST, ShaderTypeDeclarationAST>,
        ITransformerOf<ShaderFieldDeclarationAST, ShaderFieldDeclarationAST>,
        ITransformerOf<ShaderMethodBaseDeclarationAST, ShaderMethodBaseDeclarationAST>,
        ITransformerOf<ShaderMethodDeclarationAST, ShaderMethodDeclarationAST>,
        ITransformerOf<ShaderConstructorDeclarationAST, ShaderConstructorDeclarationAST>,
        ITransformerOf<ShaderMethodBodyAST, ShaderMethodBodyAST>,
        
        ITransformerOf<ShaderExpressionAST, ShaderExpressionAST>,
        ITransformerOf<ShaderConstantExpressionAST, ShaderConstantExpressionAST>,
        ITransformerOf<ShaderAssignamentAST, ShaderAssignamentAST>,
        ITransformerOf<ShaderConversionAST, ShaderConversionAST>,
        ITransformerOf<ShaderOperationAST, ShaderOperationAST>,
        ITransformerOf<ShaderParameterInvokeAST, ShaderParameterInvokeAST>,
        ITransformerOf<ShaderMemberInvokeAST, ShaderMemberInvokeAST>,
        ITransformerOf<ShaderFieldInvokeAST, ShaderFieldInvokeAST>,
        ITransformerOf<ShaderMethodBaseInvokeAST, ShaderMethodBaseInvokeAST>,
        ITransformerOf<ShaderMethodInvokeAST, ShaderMethodInvokeAST>,
        ITransformerOf<ShaderConstructorInvokeAST, ShaderConstructorInvokeAST>,
        ITransformerOf<ShaderLocalInvokeAST, ShaderLocalInvokeAST>,
        ITransformerOf<ShaderInitializationInvokeAST, ShaderInitializationInvokeAST>,

        ITransformerOf<ShaderStatementAST, ShaderStatementAST>,
        ITransformerOf<ShaderExpressionStatementAST, ShaderExpressionStatementAST>,
        ITransformerOf<ShaderLocalDeclarationAST, ShaderLocalDeclarationAST>,
        ITransformerOf<ShaderConditionalStatement, ShaderConditionalStatement>,
        ITransformerOf<ShaderWhileStatementAST, ShaderWhileStatementAST>,
        ITransformerOf<ShaderDoWhileStatementAST, ShaderDoWhileStatementAST>,
        ITransformerOf<ShaderForStatementAST, ShaderForStatementAST>,
        ITransformerOf<ShaderReturnStatementAST, ShaderReturnStatementAST>,
        ITransformerOf<ShaderBlockStatementAST, ShaderBlockStatementAST>
    {
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
                this.compiledMembers = new Dictionary<ShaderMember, ShaderMember>();
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

            Dictionary<ShaderMember, ShaderMember> compiledMembers = new Dictionary<ShaderMember, ShaderMember>();

            public Dictionary<ShaderMember, ShaderMember> CompiledMembers { get { return compiledMembers; } }

            public void Add(ShaderMember member, ShaderMember converted)
            {
                if (member == null)
                    throw new ArgumentNullException("member");

                if (converted == null)
                    throw new ArgumentNullException("converted");

                try
                {
                    compiledMembers.Add(member, converted);
                }
                catch
                {
                }
            }

            public bool Contains(ShaderMember member)
            {
                if (member.IsBuiltin && Builtins.Contains (member))
                    return true;
                return compiledMembers.ContainsKey(member);
            }

            public ShaderMethod this[ShaderMethod method]
            {
                get
                {
                    if (method.IsBuiltin)
                        return method;

                    return (ShaderMethod)compiledMembers[method];
                }
            }

            public ShaderType this[ShaderType type]
            {
                get
                {
                    if (type.IsBuiltin)
                        return type;

                    return (ShaderType)compiledMembers[type];
                }
            }

            public ShaderConstructor this[ShaderConstructor constructor]
            {
                get
                {
                    if (constructor.IsBuiltin)
                        return constructor;

                    return (ShaderConstructor)compiledMembers[constructor];
                }
            }

            public ShaderField this[ShaderField field]
            {
                get
                {
                    if (field.IsBuiltin)
                        return field;

                    return (ShaderField)compiledMembers[field];
                }
            }

            public Scope CreateGlobalScope()
            {
                return new Scope(this);
            }
        }

        public ShaderProgramAST Program { get { return CurrentScope.Program; } }

        public Scope CurrentScope { get; private set; }

        public ConstantProxy ConstantProxy { get; private set; }

        public IConstantProxy SourceConstantProxy { get; private set; }

        #endregion

        public ShaderToShaderTransformer(ShaderProgramAST program)
        {
            this.CurrentScope = new Scope(program.Builtins, program);

            this.Program.Constants = this.ConstantProxy = new ConstantProxy();

            this.Maps = new Dictionary<ShaderMember, ShaderMember>();
        }

        /// <summary>
        /// Gets the conversion function from the current shader members to originals.
        /// </summary>
        public Dictionary<ShaderMember, ShaderMember> Maps { get; private set; }

        /// <summary>
        /// Gets the conversion function from the original shader members to compiled shader members.
        /// </summary>
        public Dictionary<ShaderMember, ShaderMember> Compilations { get { return CurrentScope.CompiledMembers; } }

        private ShaderType Resolve(ShaderType sourceShaderType)
        {
            if (sourceShaderType.IsArray)
            {
                var elementType = Resolve(sourceShaderType.ElementType);
                if (sourceShaderType.Ranks == null)
                    return elementType.MakeArray(sourceShaderType.Rank);
                else
                    return elementType.MakeFixedArray(sourceShaderType.Ranks);
            }

            if (CurrentScope.Contains(sourceShaderType))
                return CurrentScope[sourceShaderType];

            return null;
            //throw new InvalidOperationException (sourceShaderType + " has not been declared yet");
        }

        private ShaderMethod Resolve(ShaderMethod sourceShaderMethod)
        {
            if (CurrentScope.Contains(sourceShaderMethod))
                return CurrentScope[sourceShaderMethod];

            throw new InvalidOperationException (sourceShaderMethod+" has not been declared yet");
        }

        private ShaderConstructor Resolve(ShaderConstructor sourceShaderConstructor)
        {
            if (CurrentScope.Contains(sourceShaderConstructor))
                return CurrentScope[sourceShaderConstructor];
            
            throw new InvalidOperationException (sourceShaderConstructor+" has not been declared yet");
        }

        private ShaderField Resolve(ShaderField sourceShaderField)
        {
            if (CurrentScope.Contains(sourceShaderField))
                return CurrentScope[sourceShaderField];

            throw new InvalidOperationException(sourceShaderField + " has not been declared yet");
        }

        public ShaderProgramAST Transform(ShaderProgramAST source)
        {
            Program.PushMode(ShaderProgramAST.AddingMode.Append);
            this.SourceConstantProxy = source.Constants;

            foreach (var dec in source.Members)
                Transform(dec);

            Program.PopMode();

            return Program;
        }

        public ShaderMemberDeclarationAST Transform(ShaderMemberDeclarationAST source)
        {
            return ResolveDynamically<ShaderMemberDeclarationAST, ShaderMemberDeclarationAST>(source);
        }

        public ShaderTypeDeclarationAST Transform(ShaderTypeDeclarationAST source)
        {
            if (CurrentScope.Contains(source.Type))
                return ((UserShaderType)CurrentScope[source.Type]).AST;

            if (!source.IsStruct)
                throw new NotSupportedException();

            var typeDec = CurrentScope.CurrentType == null ?
                CurrentScope.Program.DeclareNewStruct(source.Name, source.GenericParametersCount) :
                CurrentScope.CurrentType.DeclareNewStruct(source.Name, source.GenericParametersCount);

            CurrentScope.Add(source.Type, typeDec.Type);
            Maps.Add(typeDec.Type, source.Type);

            CurrentScope = CurrentScope.CreateTypeScope(typeDec);

            foreach (var memberDeclaration in source.Members)
                Transform(memberDeclaration);

            CurrentScope = CurrentScope.Base;

            return typeDec;
        }

        

        public ShaderFieldDeclarationAST Transform(ShaderFieldDeclarationAST source)
        {
            var field = source.Field;
            var fieldType = Resolve(source.FieldType);
            
            if (fieldType == null)
                return null;

            var semantic = field.Semantic;

            ShaderFieldDeclarationAST fieldDeclaration = CurrentScope.CurrentType == null ?
                CurrentScope.Program.DeclareNewField(field.Name, fieldType, semantic) :
                CurrentScope.CurrentType.DeclareNewField(fieldType, field.Name, semantic);

            CurrentScope.Add(field, fieldDeclaration.Field);
            Maps.Add(fieldDeclaration.Field, field);

            if (source.DeclaringType == null)
                ConstantProxy.Add(fieldDeclaration.Field, (_this) => this.SourceConstantProxy.GetValueFor(_this, field));

            return fieldDeclaration;
        }

        public ShaderMethodBaseDeclarationAST Transform(ShaderMethodBaseDeclarationAST source)
        {
            return ResolveDynamically<ShaderMethodBaseDeclarationAST, ShaderMethodBaseDeclarationAST>(source);
        }

        public ShaderMethodDeclarationAST Transform(ShaderMethodDeclarationAST source)
        {
            var method = source.Method;

            ShaderMethodDeclarationAST methodDeclaration = CurrentScope.CurrentType == null ?
                CurrentScope.Program.DeclareNewMethod(method.Name, source.IsAbstract, method.GetGenericParameters().Length) :
                CurrentScope.CurrentType.DeclareNewMethod(method.Name, source.IsAbstract, method.GetGenericParameters().Length);

            var methodType = Resolve(method.ReturnType);

            Program.SetReturnType(methodDeclaration, methodType);

            CurrentScope.Add(method, methodDeclaration.Member);
            Maps.Add(methodDeclaration.Method, method);

            CurrentScope = CurrentScope.CreateMethodScope(methodDeclaration);

            foreach (var p in method.Parameters)
                methodDeclaration.DeclareNewParameter(Resolve(p.ParameterType),
                    p.Name, p.Semantic, p.Modifier);

            if (!source.IsAbstract)
                Transform(source.Body);

            CurrentScope = CurrentScope.Base;

            return methodDeclaration;
        }

        public ShaderConstructorDeclarationAST Transform(ShaderConstructorDeclarationAST source)
        {
            var constructor = source.Constructor;

            if (CurrentScope.CurrentType == null)
                throw new InvalidOperationException();

            ShaderConstructorDeclarationAST constructorDeclaration = CurrentScope.CurrentType.DeclareNewConstructor();

            CurrentScope.Add(constructor, constructorDeclaration.Constructor);
            Maps.Add(constructorDeclaration.Constructor, constructor);

            CurrentScope = CurrentScope.CreateMethodScope(constructorDeclaration);

            foreach (var p in constructor.Parameters)
                constructorDeclaration.DeclareNewParameter(Resolve(p.ParameterType),
                    p.Name, p.Semantic, p.Modifier);

            Transform(source.Body);

            CurrentScope = CurrentScope.Base;

            return constructorDeclaration;
        }

        public ShaderMethodBodyAST Transform(ShaderMethodBodyAST source)
        {
            var body = CurrentScope.CurrentMethod.Body;

            foreach (var s in source.Statements)
                body.StatementsList.Add(Transform(s));

            return body;
        }

        public ShaderExpressionAST Transform(ShaderExpressionAST source)
        {
            return ResolveDynamically<ShaderExpressionAST, ShaderExpressionAST>(source);
        }

        public ShaderConstantExpressionAST Transform(ShaderConstantExpressionAST source)
        {
            return Program.CreateConstant(CurrentScope[source.Type], source.Value);
        }

        public ShaderAssignamentAST Transform(ShaderAssignamentAST source)
        {
            return Program.CreateAssignament(
                Transform(source.LeftValue),
                Transform(source.RightValue));
        }

        public ShaderConversionAST Transform(ShaderConversionAST source)
        {
            return Program.CreateConversion(Resolve(source.Target),
                Transform(source.Expression));
        }

        public ShaderOperationAST Transform(ShaderOperationAST source)
        {
            return Program.CreateOperation(source.Operator, source.Operands.Select(o => Transform(o)).ToArray());
        }

        public ShaderParameterInvokeAST Transform(ShaderParameterInvokeAST source)
        {
            return Program.CreateInvoke(CurrentScope.CurrentMethod.Parameters.First(p => p.Name == source.Parameter.Name));
        }

        public ShaderMemberInvokeAST Transform(ShaderMemberInvokeAST source)
        {
            return ResolveDynamically<ShaderMemberInvokeAST, ShaderMemberInvokeAST>(source);
        }

        public ShaderFieldInvokeAST Transform(ShaderFieldInvokeAST source)
        {
            if (!CurrentScope.Contains(source.Field)) return null;

            return Program.CreateInvoke(CurrentScope[source.Field],
                source.LeftSide == null ? null :
                Transform(source.LeftSide));
        }

        public ShaderMethodBaseInvokeAST Transform(ShaderMethodBaseInvokeAST source)
        {
            return ResolveDynamically<ShaderMethodBaseInvokeAST, ShaderMethodBaseInvokeAST>(source);
        }

        public ShaderMethodInvokeAST Transform(ShaderMethodInvokeAST source)
        {
            if (!CurrentScope.Contains(source.Method))
                return null;

            return Program.CreateInvoke(CurrentScope[source.Method],
                source.LeftSide == null ? null :
                Transform(source.LeftSide),
                source.Arguments.Select(a => Transform(a)).ToArray());
        }

        public ShaderConstructorInvokeAST Transform(ShaderConstructorInvokeAST source)
        {
            if (!CurrentScope.Contains(source.Constructor))
                return null;

            return Program.CreateInvoke(CurrentScope[source.Constructor],
                source.Arguments.Select(a => Transform(a)).ToArray());
        }

        public ShaderLocalInvokeAST Transform(ShaderLocalInvokeAST source)
        {
            var local = CurrentScope.CurrentMethod.Body.Statements.OfType<ShaderLocalDeclarationAST>().
                    First(d => d.Name == source.Local.Name).Local;

            return Program.CreateInvoke(local);
        }

        public ShaderInitializationInvokeAST Transform(ShaderInitializationInvokeAST source)
        {
            return Program.CreateInitialization(Resolve(source.TargetType));
        }

        public ShaderStatementAST Transform(ShaderStatementAST source)
        {
            return ResolveDynamically<ShaderStatementAST, ShaderStatementAST>(source);
        }

        public ShaderExpressionStatementAST Transform(ShaderExpressionStatementAST source)
        {
            return Program.CreateExpressionStatement(Transform(source.Expression));
        }

        public ShaderLocalDeclarationAST Transform(ShaderLocalDeclarationAST source)
        {
            return Program.CreateLocalDeclaration(Resolve(source.Type), source.Name);
        }

        public ShaderConditionalStatement Transform(ShaderConditionalStatement source)
        {
            var conditional = Transform(source.Conditional);
            if (conditional == null)
                return null;

            return Program.CreateConditional(conditional,
                Transform(source.TrueBlock),
                source.FalseBlock == null ? null :
                Transform(source.FalseBlock));
        }

        public ShaderWhileStatementAST Transform(ShaderWhileStatementAST source)
        {
            return Program.CreateWhile(Transform(source.Conditional),
                Transform(source.Body));
        }

        public ShaderDoWhileStatementAST Transform(ShaderDoWhileStatementAST source)
        {
            return Program.CreateDoWhile(Transform(source.Conditional), Transform(source.Body));
        }

        public ShaderForStatementAST Transform(ShaderForStatementAST source)
        {
            return Program.CreateFor(Transform(source.Initialization), Transform(source.Conditional), Transform(source.Increment), Transform(source.Body));
        }

        public ShaderReturnStatementAST Transform(ShaderReturnStatementAST source)
        {
            return Program.CreateReturn(Transform(source.Expression));
        }

        public ShaderBlockStatementAST Transform(ShaderBlockStatementAST source)
        {
            return Program.CreateBlock(source.Statements.Select(s => Transform(s)).ToList());
        }

        public ShaderMethod MethodOverriden { get; set; }
    }
}
