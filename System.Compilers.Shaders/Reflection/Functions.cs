using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Compilers.Shaders.AST;

namespace System.Compilers.Shaders.Reflection
{
    /// <summary>
    /// Represents a method base in a shader language
    /// </summary>
    public abstract class ShaderMethodBase : ShaderMember
    {
        /// <summary>
        /// Gets the array of parameters for this function.
        /// </summary>
        public abstract ShaderParameter[] Parameters { get; }
    }

    /// <summary>
    /// Represents a constructor function.
    /// </summary>
    public abstract class ShaderConstructor : ShaderMethodBase
    {
        public abstract ShaderType DeclaringType { get; }

        public bool IsDefault
        {
            get { return this is DefaultConstructor; }
        }
    }

    /// <summary>
    /// Represents a named function.
    /// </summary>
    public abstract class ShaderMethod : ShaderMethodBase
    {
        public abstract ShaderType ReturnType { get; }

        public abstract Operators Operator { get; }

        public bool IsOperator { get { return Operator != Operators.None; } }
    }

    /// <summary>
    /// Represents a primitive function at a primitive set.
    /// </summary>
    class PrimitiveShaderMethod : ShaderMethod, IPrimitiveMember<MethodInfo>
    {
        /// <summary>
        /// Creates a function to be invoked with a name
        /// </summary>
        internal PrimitiveShaderMethod(Builtins set, MethodInfo method, string name)
            : base()
        {
            this.NetMember = method;
            this._Name = name;
            this._ReturnType = set.ResolveType(method.ReturnType);
            this._Parameters = method.GetParameters().Select(pi => (ShaderParameter)new PrimitiveShaderParameter(set, pi.Name, pi)).ToArray();
            this._Operator = Operators.None;
        }

        /// <summary>
        /// Creates a function to be invoked with an operator
        /// </summary>
        internal PrimitiveShaderMethod(Builtins set, MethodInfo method, Operators op) :
            this(set, method, NameFor(op))
        {
            this._Operator = op;
        }

        private static string NameFor(Operators op)
        {
            if (op == Operators.Indexer)
                return "[]";
            return "op_" + op.ToString();
        }

        ShaderType _ReturnType;
        public override ShaderType ReturnType { get { return _ReturnType; } }

        ShaderParameter[] _Parameters;
        public override ShaderParameter[] Parameters { get { return _Parameters.ToArray(); } }

        string _Name;
        public override string Name
        {
            get { return _Name; }
        }

        public bool IsOperator
        {
            get { return Operator != Operators.None; }
        }

        Operators _Operator;
        public override Operators Operator { get { return _Operator; } }

        public override bool IsPrimitive
        {
            get { return true; }
        }

        public MethodInfo NetMember
        {
            get;
            private set;
        }
    }

    class UserShaderFunction : ShaderMethod
    {
        FunctionDeclarationAST ast;

        internal UserShaderFunction(FunctionDeclarationAST ast)
        {
            this.ast = ast;
        }

        public override ShaderType ReturnType
        {
            get { return ast.ReturnType; }
        }

        public override ShaderParameter[] Parameters
        {
            get { return ast.Parameters.Select(p => p.Parameter).ToArray(); }
        }

        public override Operators Operator
        {
            get { return Operators.None; }
        }

        public override string Name
        {
            get { return ast.Label; }
        }

        public override bool IsPrimitive
        {
            get { return false; }
        }
    }

    class PrimitiveShaderConstructor : ShaderConstructor
    {
        internal PrimitiveShaderConstructor(Builtins set, ConstructorInfo constructor)
        {
            ShaderType type = set.ResolveType(constructor.DeclaringType);
            ShaderParameter[] parameters = constructor.GetParameters().Select(pi => (ShaderParameter)new PrimitiveShaderParameter(set, pi.Name, pi)).ToArray();
            this._DeclaringType = type;
            this._Parameters = parameters;
        }

        ShaderType _DeclaringType;
        public override ShaderType DeclaringType
        {
            get { return _DeclaringType; }
        }

        ShaderParameter[] _Parameters;
        public override ShaderParameter[] Parameters
        {
            get { return _Parameters.ToArray(); }
        }

        public override string Name
        {
            get { return DeclaringType.Name; }
        }

        public override bool IsPrimitive
        {
            get { return true; }
        }
    }

    class DefaultConstructor : ShaderConstructor
    {
        internal DefaultConstructor(ShaderType type)
            : base()
        {
            _DeclaringType = type;
        }

        ShaderType _DeclaringType;
        public override ShaderType DeclaringType
        {
            get { return _DeclaringType; }
        }

        public override ShaderParameter[] Parameters
        {
            get { return new ShaderParameter[0]; }
        }

        public override string Name
        {
            get { return _DeclaringType.Name; }
        }

        public override bool IsPrimitive
        {
            get { return true; }
        }
    }
}