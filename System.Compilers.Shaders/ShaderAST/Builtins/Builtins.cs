using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Compilers.Shaders.Info;

namespace System.Compilers.Shaders.ShaderAST
{
    abstract class BuiltinShaderMember : ShaderMember
    {
        public string Name { get; private set; }

        public MemberInfo MemberInfo { get; private set; }

        public Builtins Builtins { get; private set; }

        public BuiltinShaderMember(Builtins builtins, MemberInfo memberInfo, string name, ShaderType declaringType)
        {
            this.MemberInfo = memberInfo;
            this.Name = name;
            this.Builtins = builtins;
            this.DeclaringType = declaringType;
        }

        public virtual bool IsGlobal { get { return false; } }

        public bool IsBuiltin
        {
            get { return true; }
        }

        public ShaderType DeclaringType { get; private set; }

        public override bool Equals(object obj)
        {
            return (obj is BuiltinShaderMember) && ((BuiltinShaderMember)obj).MemberInfo.Equals(this.MemberInfo);
        }

        public override int GetHashCode()
        {
            return MemberInfo.GetHashCode();
        }
    }

    class BuiltinShaderType : BuiltinShaderMember, ShaderType
    {
        public BuiltinShaderType(Builtins builtins, Type type, string name, ShaderType declaringType)
            : base(builtins, type, name, declaringType)
        {
            genericParameters = (type.IsGenericTypeDefinition) ?
                type.GetGenericArguments().Select(t => ShaderTypeExtensors.CreateGenericParameter(this, t.Name)).ToArray() :
                new ShaderType[0];
        }

        ShaderType[] genericParameters;

        public Type TypeInfo { get { return (Type)MemberInfo; } }

        public bool IsArray
        {
            get { return false; }
        }

        public int Rank
        {
            get { return 0; }
        }

        public int[] Ranks
        {
            get { return null; }
        }

        public bool IsGenericType
        {
            get { return false; }
        }

        public bool IsGenericTypeDefinition
        {
            get { return TypeInfo.IsGenericTypeDefinition; }
        }

        public bool IsGenericParameter
        {
            get { return false; }
        }

        public ShaderType[] GetGenericParameters()
        {
            return genericParameters;
        }

        public ShaderType GetGenericDefinition()
        {
            return null;
        }

        public ShaderType BaseType
        {
            get
            {
                if (Builtins.Contains(TypeInfo.BaseType))
                    return Builtins.Resolve(TypeInfo.BaseType);
                else
                    return null;
            }
        }

        public ShaderType ElementType
        {
            get { return null; }
        }

        public IEnumerable<ShaderMember> Members
        {
            get
            {
                return TypeInfo.GetMembers(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public)
                    .Where(m => Builtins.Contains(m))
                    .Select(m => Builtins.Resolve(m));
            }
        }

        public ShaderType MakeGenericType(params ShaderType[] typeArguments)
        {
            if (this.IsGenericTypeDefinition)
                return ShaderTypeExtensors.MakeGenericType(this, typeArguments);

            throw new InvalidOperationException();
        }

        public override string ToString()
        {
            return "Builtin type: " + Name;
        }
    }

    class BuiltinShaderField : BuiltinShaderMember, ShaderField
    {
        public BuiltinShaderField(Builtins builtins, FieldInfo field, string name, ShaderType declaringType)
            : base(builtins, field, name, declaringType)
        {
        }

        public FieldInfo FieldInfo
        {
            get { return base.MemberInfo as FieldInfo; }
        }

        public Semantic Semantic
        {
            get
            {
                return ShaderSemantics.Resolve(FieldInfo);
            }
        }

        public ShaderType Type
        {
            get
            {
                return Builtins.Resolve(FieldInfo.FieldType);
            }
        }

        public override bool IsGlobal
        {
            get
            {
                return FieldInfo.IsStatic;
            }
        }
    }

    class BuiltinShaderMethodBase : BuiltinShaderMember, ShaderMethodBase
    {
        public BuiltinShaderMethodBase(Builtins builtins, MethodBase methodBase, string name, ShaderType declaringType)
            : base(builtins, methodBase, name, declaringType)
        {
            this.parameters = methodBase.GetParameters().Select(p =>
                new ShaderParameter(builtins.Resolve(p.ParameterType),
                    p.Name, ShaderSemantics.Resolve(p),
                    ResolveParameterModifier(p))).ToArray();
        }

        private Info.ParameterModifier ResolveParameterModifier(ParameterInfo p)
        {
            System.Compilers.Shaders.Info.ParameterModifier modifier = Info.ParameterModifier.None;

            if (p.IsIn)
                modifier |= Info.ParameterModifier.In;
            if (p.IsOut)
                modifier |= Info.ParameterModifier.Out;

            return modifier;
        }

        ShaderParameter[] parameters;

        public MethodBase MethodBaseInfo { get { return base.MemberInfo as MethodBase; } }

        public override bool IsGlobal
        {
            get
            {
                return MethodBaseInfo.IsStatic;
            }
        }

        public IEnumerable<ShaderParameter> Parameters
        {
            get { return parameters; }
        }
    }

    class BuiltinShaderMethod : BuiltinShaderMethodBase, ShaderMethod
    {
        public BuiltinShaderMethod(Builtins builtins, MethodInfo method, string name, ShaderType declaringType)
            : base(builtins, method, name, declaringType)
        {
            genericParameters = method.IsGenericMethodDefinition ?
                method.GetGenericArguments().Select(t => ShaderTypeExtensors.CreateGenericParameter(this, t.Name)).ToArray() :
                new ShaderType[0];
        }

        ShaderType[] genericParameters;
        
        public MethodInfo Method { get { return MemberInfo as MethodInfo; } }

        public ShaderType ReturnType
        {
            get { return Builtins.Resolve(Method.ReturnType); }
        }

        public bool IsGenericDefinition
        {
            get { return Method.IsGenericMethodDefinition; }
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

        private static Operators GetOperatorByName(string op)
        {
            switch (op)
            {
                case "op_Addition": return Operators.Addition;
                case "op_Subtraction": return Operators.Subtraction;
                case "op_Multiply": return Operators.Multiply;
                case "op_Division": return Operators.Division;
                case "op_Modulus": return Operators.Modulus;
                case "op_Explicit": return Operators.Cast;
                case "op_Implicit": return Operators.Implicit;
                default:
                    if (op.StartsWith("op_"))
                    {
                        Operators result;
                        if (Enum.TryParse<Operators>(op.Substring(3), out result))
                            return result;
                        return Operators.None;
                    }
                    return Operators.None;
            }
        }

        public Operators Operator
        {
            get
            {
                return GetOperatorByName(Name);
            }
        }

        public bool IsAbstract
        {
            get { return Method.IsAbstract; }
        }
    }

    class BuiltinShaderConstructor : BuiltinShaderMethodBase, ShaderConstructor
    {
        public BuiltinShaderConstructor(Builtins builtins, ConstructorInfo constructor, ShaderType declaringType)
            : base(builtins, constructor, "ctor", declaringType)
        {
        }

        public ConstructorInfo Constructor
        {
            get { return base.MemberInfo as ConstructorInfo; }
        }
    }
}
