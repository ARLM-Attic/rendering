using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.Info;

namespace System.Compilers.Shaders.ShaderAST
{
    public class ShaderTypeDeclarationAST : ShaderMemberDeclarationAST
    {
        private List<ShaderMemberDeclarationAST> members = new List<ShaderMemberDeclarationAST>();

        public IEnumerable<ShaderMemberDeclarationAST> Members
        {
            get { return members; }
        }

        internal ShaderTypeDeclarationAST(ShaderProgramAST program, ShaderTypeDeclarationAST declaringType, int numberOfGenericParameters, ShaderTypeType typeType, string name)
            : base(program, declaringType, name)
        {
            this.typeType = typeType;
            GenericParametersCount = numberOfGenericParameters;

            Type = new UserShaderType(this);
        }

        public ShaderType Type { get; private set; }

        public override ShaderMember Member
        {
            get { return Type; }
        }

        private ShaderTypeType typeType;

        public ShaderFieldDeclarationAST DeclareNewField(ShaderType type, string name, Semantic semantic)
        {
            var field = new ShaderFieldDeclarationAST(this.Program, this, type, name, semantic);
            members.Add(field);
            return field;
        }

        private ShaderTypeDeclarationAST DeclareNewType(ShaderTypeType type, string name, int numberOfGenericParameters)
        {
            var t = new ShaderTypeDeclarationAST(this.Program, this, numberOfGenericParameters + this.GenericParametersCount, type, name);
            members.Add(t);
            return t;
        }

        public ShaderTypeDeclarationAST DeclareNewStruct(string name)
        {
            return DeclareNewStruct(name, 0);
        }

        public ShaderTypeDeclarationAST DeclareNewStruct(string name, int numberOfGenericParameters)
        {
            return DeclareNewType(ShaderTypeType.Struct, name, numberOfGenericParameters);
        }

        public ShaderMethodDeclarationAST DeclareNewMethod(string name, int numberOfGenericParameters)
        {
            return DeclareNewMethod(name, false, numberOfGenericParameters);
        }

        public ShaderMethodDeclarationAST DeclareNewMethod(string name, bool isAbstract, int numberOfGenericParameters)
        {
            if (isAbstract && this.IsStruct)
                throw new ArgumentException("Struct types can not define abstract members");

            var m = new ShaderMethodDeclarationAST(this.Program, this, name, isAbstract, numberOfGenericParameters);
            members.Add(m);
            return m;
        }

        public ShaderConstructorDeclarationAST DeclareNewConstructor()
        {
            var c = new ShaderConstructorDeclarationAST(this);
            members.Add(c);
            return c;
        }


        public ShaderType BaseType { get; private set; }

        public IEnumerable<ShaderType> Interfaces { get { return interfaces; } }

        List<ShaderType> interfaces = new List<ShaderType>();

        public int GenericParametersCount { get; private set; }

        public void SetBaseType(ShaderType baseType)
        {
            this.BaseType = baseType;
        }

        public void ImplementInterface(ShaderType interfaceType)
        {
            interfaces.Add(interfaceType);
        }

        public bool IsStruct { get { return typeType == ShaderTypeType.Struct; } }

        public bool IsDelegate { get { return typeType == ShaderTypeType.Delegate; } }

        public bool IsClass { get { return typeType == ShaderTypeType.Class; } }

        public bool IsEnum { get { return typeType == ShaderTypeType.Enum; } }

        public bool IsInterface { get { return typeType == ShaderTypeType.Interface; } }
    }

    enum ShaderTypeType
    {
        Struct,
        Delegate,
        Class,
        Enum,
        Interface
    }
}
