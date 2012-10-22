using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.Info;

namespace System.Compilers.Shaders.ShaderAST
{
    abstract class UserShaderMember : ShaderMember
    {
        public ShaderMemberDeclarationAST AST { get; private set; }

        internal UserShaderMember(ShaderMemberDeclarationAST ast)
        {
            this.AST = ast;
            ID = __ID++;
        }

        public int ID { get; private set; }

        public string Name
        {
            get { return AST.Name; }
        }

        public bool IsBuiltin
        {
            get { return false; }
        }

        public bool IsGlobal { get { return AST.IsGlobal; } }

        public ShaderType DeclaringType
        {
            get
            {
                if (this.AST.DeclaringType == null) return null;
                return AST.DeclaringType.Type;
            }
        }

        static int __ID = 0;
    }

    class UserShaderType : UserShaderMember, ShaderType
    {
        internal UserShaderType(ShaderTypeDeclarationAST ast)
            : base(ast)
        {
            genericParameters = new ShaderType[ast.GenericParametersCount];
            for (int i = 0; i < genericParameters.Length; i++)
                genericParameters[i] = ShaderTypeExtensors.CreateGenericParameter(this, "T" + i);
        }

        new public ShaderTypeDeclarationAST AST { get { return base.AST as ShaderTypeDeclarationAST; } }

        public bool IsArray
        {
            get { return false; }
        }

        public bool IsGenericType
        {
            get { return false; }
        }

        public bool IsGenericTypeDefinition
        {
            get { return AST.GenericParametersCount > 0; }
        }

        public bool IsGenericParameter
        {
            get { return false; }
        }

        public ShaderType BaseType
        {
            get { return AST.BaseType; }
        }

        public IEnumerable<ShaderMember> Members
        {
            get
            {
                return AST.Members.Select(m => m.Member);
            }
        }

        private ShaderType[] genericParameters;

        public ShaderType[] GetGenericParameters()
        {
            return genericParameters.Clone() as ShaderType[];
        }

        public int Rank
        {
            get { return 0; }
        }

        public int[] Ranks
        {
            get { return new int[0]; }
        }

        public ShaderType GetGenericDefinition()
        {
            return null;
        }

        public ShaderType ElementType
        {
            get { return null; }
        }

        public ShaderType MakeGenericType(params ShaderType[] typeArguments)
        {
            if (this.IsGenericTypeDefinition)
                return ShaderTypeExtensors.MakeGenericType(this, typeArguments);

            throw new InvalidOperationException();
        }

        public override string ToString()
        {
            return "UserType: " + Name + " (" + ID + ")";
        }
    }

    class UserShaderMethodBase : UserShaderMember, ShaderMethodBase
    {
        public UserShaderMethodBase(ShaderMethodBaseDeclarationAST ast)
            : base(ast)
        {
        }

        public new ShaderMethodBaseDeclarationAST AST { get { return base.AST as ShaderMethodBaseDeclarationAST; } }

        public IEnumerable<ShaderParameter> Parameters
        {
            get { return AST.Parameters; }
        }
    }

    class UserShaderMethod : UserShaderMethodBase, ShaderMethod
    {
        public UserShaderMethod(ShaderMethodDeclarationAST ast)
            : base(ast)
        {
            genericParameters = new ShaderType[ast.NumberOfGenericParameters];
            for (int i = 0; i < genericParameters.Length; i++)
                genericParameters[i] = ShaderTypeExtensors.CreateGenericParameter(this, "T" + i);
        }

        ShaderType[] genericParameters;

        public new ShaderMethodDeclarationAST AST { get { return base.AST as ShaderMethodDeclarationAST; } }

        public ShaderType ReturnType
        {
            get { return AST.ReturnType; }
        }

        public bool IsGenericDefinition
        {
            get { return AST.NumberOfGenericParameters > 0; }
        }

        public bool IsGenericMethod
        {
            get { return false; }
        }

        public ShaderMethod GetGenericDefinition()
        {
            return null;
        }

        public ShaderType[] GetGenericParameters()
        {
            return genericParameters.Clone() as ShaderType[];
        }

        public Operators Operator
        {
            get
            {
                return Operators.None; // not supported operators declaration.
            }
        }

        public bool IsAbstract
        {
            get { return AST.IsAbstract; }
        }

        public override string ToString()
        {
            return "Method: " + ReturnType.ToString() + " " + Name + "(" + string.Join(",", Parameters.Select(p => p.ParameterType.ToString())) + ")";
        }
    }

    class UserShaderConstructor : UserShaderMethodBase, ShaderConstructor
    {
        public UserShaderConstructor(ShaderConstructorDeclarationAST ast) :base (ast)
        {
        }

        public new ShaderConstructorDeclarationAST AST
        {
            get { return base.AST as ShaderConstructorDeclarationAST; }
        }
    }

    class UserShaderField : UserShaderMember, ShaderField
    {
        public UserShaderField(ShaderFieldDeclarationAST ast)
            : base(ast)
        {
        }

        public new ShaderFieldDeclarationAST AST
        {
            get { return base.AST as ShaderFieldDeclarationAST; }
        }

        public Semantic Semantic
        {
            get
            {
                return AST.Semantic;
            }
        }

        public ShaderType Type
        {
            get { return AST.FieldType; }
        }

        public override string ToString()
        {
            return Type.ToString() + " " + Name;
        }
    }
}
