using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq.Expressions;
using System.Compilers.IL;
using System.Runtime.InteropServices;

namespace System.Compilers.AST
{
    public class NetLocalVariable : IEquatable<NetLocalVariable>
    {
        public string Name;
        public bool IsGenerated;
        public Type Type;
        public LocalVariableInfo OriginalVariable;

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(NetLocalVariable other)
        {
            return Name.Equals(other.Name)
                && (Type != null ? Type.Equals(other.Type) : true)
                && (OriginalVariable != null ? OriginalVariable.LocalIndex == other.OriginalVariable.LocalIndex : true);
        }
    }

    public class NetMemberDeclarationAST : NetAstNode
    {
        public MemberInfo Member { get; private set; }

        internal NetMemberDeclarationAST(MemberInfo member)
        {
            this.Member = member;
        }

        public virtual bool IsReadonly
        {
            get { return false; }
        }
    }

    public class NetTypeDeclarationAST : NetMemberDeclarationAST
    {
        internal NetTypeDeclarationAST(Type type)
            : base(type)
        {
        }

        public static NetTypeDeclarationAST CreateReadonly(Type type)
        {
            return new NetTypeDeclarationAST(type);
        }

        public static NetTypeDeclarationAST CreateWriteable(TypeBuilder type)
        {
            return new NetTypeDeclarationAST(type);
        }

        TypeBuilder Builder { get { return Member as TypeBuilder; } }

        public bool IsStruct { get { return (Member as Type).IsValueType; } }

        public bool IsClass { get { return (Member as Type).IsClass; } }

        public bool IsEnum { get { return (Member as Type).IsEnum; } }

        public NetMethodDeclarationAST DeclareNewMethod(string methodName, MethodAttributes attributes)
        {
            if (IsReadonly)
                throw new InvalidOperationException();

            var methodBuilder = Builder.DefineMethod(methodName, attributes);

            var m = new NetMethodDeclarationAST(methodBuilder);
            declaredMembers.Add(m);

            return m;
        }

        public NetFieldDeclarationAST DeclareNewField(string fieldName, Type fieldType, FieldAttributes attributes)
        {
            if (IsReadonly)
                throw new InvalidOperationException();

            var fieldBuilder = Builder.DefineField(fieldName, fieldType, attributes);

            var f = new NetFieldDeclarationAST(fieldBuilder);
            declaredMembers.Add(f);
            return f;
        }

        List<NetMemberDeclarationAST> declaredMembers = new List<NetMemberDeclarationAST>();

        public IEnumerable<NetMemberDeclarationAST> Members
        {
            get
            {
                if (IsReadonly)
                {
                    var members = ((Type)Member).GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Instance).OrderBy(m => m is FieldInfo ? (int)Marshal.OffsetOf((Type)Member, m.Name) : int.MaxValue);
                    foreach (var m in members)
                    {
                        if (m is Type)
                            yield return new NetTypeDeclarationAST(m as Type);

                        if (m is MethodInfo)
                            yield return NetMethodDeclarationAST.CreateReadonly(m as MethodInfo);

                        if (m is FieldInfo)
                            yield return new NetFieldDeclarationAST(m as FieldInfo);

                        if (m is ConstructorInfo)
                            yield return NetConstructorDeclarationAST.CreateReadonly(m as ConstructorInfo);
                    }
                }
                else
                {
                    foreach (var m in declaredMembers)
                        yield return m;
                }
            }
        }

        public override bool IsReadonly
        {
            get
            {
                return !(Member is TypeBuilder);
            }
        }
    }

    public class NetMethodBaseDeclarationAST : NetMemberDeclarationAST
    {
        internal NetMethodBaseDeclarationAST(MethodBuilder methodBase)
            : base(methodBase)
        {
            Body = new NetMethodBodyDeclarationAST();
        }

        internal NetMethodBaseDeclarationAST(ConstructorBuilder methodBase)
            : base(methodBase)
        {
            Body = new NetMethodBodyDeclarationAST();
        }

        internal NetMethodBaseDeclarationAST(MethodBase methodBase, IEnumerable<NetAstStatement> statements)
            : base(methodBase)
        {
            Body = new NetMethodBodyDeclarationAST(statements);
        }

        public NetMethodBodyDeclarationAST Body { get; private set; }

        public new MethodBase Member { get { return base.Member as MethodBase; } }

        public ParameterBuilder DeclareNewParameter(string name, ParameterAttributes attributes)
        {
            if (this is NetConstructorDeclarationAST)
            {
                var constructorBuilder = Member as ConstructorBuilder;

                return constructorBuilder.DefineParameter(Member.GetParameters().Length + 1, attributes, name);
            }

            if (this is NetMethodDeclarationAST)
            {
                var methodBuilder = Member as ConstructorBuilder;

                return methodBuilder.DefineParameter(Member.GetParameters().Length + 1, attributes, name);
            }

            throw new InvalidOperationException();
        }
    }

    public class NetMethodBodyDeclarationAST : NetAstNode
    {
        public IList<NetAstStatement> Statements { get; private set; }

        internal NetMethodBodyDeclarationAST()
        {
            Statements = new List<NetAstStatement>();
        }

        internal NetMethodBodyDeclarationAST(IEnumerable<NetAstStatement> statements)
        {
            Statements = new List<NetAstStatement>(statements).AsReadOnly();
        }
    }

    public class NetMethodDeclarationAST : NetMethodBaseDeclarationAST
    {
        internal NetMethodDeclarationAST(MethodBuilder method)
            : base(method)
        {
        }

        internal NetMethodDeclarationAST(MethodInfo method, IEnumerable<NetAstStatement> statements)
            : base(method, statements)
        {
        }

        public static NetMethodDeclarationAST CreateReadonly(MethodInfo method)
        {
            return new NetMethodDeclarationAST(method, ILDecompiler.GetMethodBody(method).Instructions);
        }

        public static NetMethodDeclarationAST CreateWriteable(MethodBuilder method)
        {
            return new NetMethodDeclarationAST(method);
        }

        public new MethodInfo Member { get { return base.Member as MethodInfo; } }

        public override bool IsReadonly
        {
            get
            {
                return !(Member is MethodBuilder);
            }
        }

        public Type ReturnType { get { return Member.ReturnType; } }

        public void SetReturnType(Type type)
        {
            if (IsReadonly)
                throw new InvalidOperationException();

            ((MethodBuilder)Member).SetReturnType(type);
        }
    }

    public class NetConstructorDeclarationAST : NetMethodBaseDeclarationAST
    {
        internal NetConstructorDeclarationAST(ConstructorBuilder constructor)
            : base(constructor)
        {
        }

        internal NetConstructorDeclarationAST(ConstructorInfo constructor, IEnumerable<NetAstStatement> statements)
            : base(constructor, statements)
        {
        }

        public static NetConstructorDeclarationAST CreateReadonly(ConstructorInfo constructor)
        {
            return new NetConstructorDeclarationAST(constructor, ILDecompiler.GetMethodBody(constructor).Instructions);
        }

        public static NetConstructorDeclarationAST CreateWriteable(ConstructorBuilder constructor)
        {
            return new NetConstructorDeclarationAST(constructor);
        }

        public new ConstructorInfo Member
        {
            get { return base.Member as ConstructorInfo; }
        }

        public override bool IsReadonly
        {
            get
            {
                return !(Member is ConstructorBuilder);
            }
        }
    }

    public class NetFieldDeclarationAST : NetMemberDeclarationAST
    {
        internal NetFieldDeclarationAST(FieldInfo field)
            : base(field)
        {
        }

        internal NetFieldDeclarationAST(FieldBuilder field)
            : base(field)
        {
        }

        public static NetFieldDeclarationAST CreateReadonly(FieldInfo field)
        {
            return new NetFieldDeclarationAST(field);
        }

        public static NetFieldDeclarationAST CreateWriteable(FieldBuilder field)
        {
            return new NetFieldDeclarationAST(field);
        }

        public new FieldInfo Member { get { return base.Member as FieldInfo; } }

        public override bool IsReadonly
        {
            get
            {
                return !(Member is FieldBuilder);
            }
        }

        CustomAttributeBuilder FromAttribute(Attribute a)
        {
            FieldInfo[] fields = a.GetType().GetFields();
            object[] fieldValues = fields.Select(f => f.GetValue(a)).ToArray();

            PropertyInfo[] properties = a.GetType().GetProperties();
            object[] propertyValues = properties.Select(p => p.GetValue(a, null)).ToArray();

            return new CustomAttributeBuilder(a.GetType().GetConstructor(Type.EmptyTypes), new object[0], properties, propertyValues, fields, fieldValues);
        }

        public void SetSemantic(params Attribute[] attributes)
        {
            if (IsReadonly)
                throw new InvalidOperationException();

            foreach (var a in attributes)
                ((FieldBuilder)Member).SetCustomAttribute(FromAttribute(a));
        }
    }
}
