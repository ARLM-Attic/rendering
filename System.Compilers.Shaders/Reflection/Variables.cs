using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Compilers.Shaders.AST;

namespace System.Compilers.Shaders.Reflection
{
    #region ShaderField

    /// <summary>
    /// Represents the access to a field or composition of fields in a shader language.
    /// </summary>
    public abstract class ShaderField : ShaderMember
    {
        public abstract ShaderType Type { get; }

        public abstract Semantic Semantic { get; }
    }

    /// <summary>
    /// Represents a field of a primitive type.
    /// </summary>
    class PrimitiveShaderField : ShaderField, IPrimitiveMember<FieldInfo>
    {
        internal PrimitiveShaderField(Builtins set, string name, FieldInfo netField)
            : base()
        {
            this.NetMember = netField;
            this._Name = name;
            this._Semantic = ShaderSemantics.Resolve(netField);
            this._Type = set.ResolveType(netField.FieldType);
        }

        public override bool IsPrimitive
        {
            get { return true; }
        }

        ShaderType _Type;
        public override ShaderType Type
        {
            get { return _Type; }
        }

        Semantic _Semantic;
        public override Semantic Semantic
        {
            get { return _Semantic; }
        }

        string _Name;
        public override string Name
        {
            get { return _Name; }
        }

        public FieldInfo NetMember
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// Represents a field of a user type.
    /// </summary>
    class UserShaderField : ShaderField, IUserMember<FieldDeclarationAST>
    {
        FieldDeclarationAST ast;

        internal UserShaderField(FieldDeclarationAST ast)
        {
            this.ast = ast;
        }

        public override string Name
        {
            get { return ast.Label; }
        }

        public override bool IsPrimitive
        {
            get { return false; }
        }

        public override Semantic Semantic
        {
            get { return ast.Semantic; }
        }

        public override ShaderType Type
        {
            get { return ast.Type; }
        }

        public FieldDeclarationAST DeclarationAST
        {
            get { return ast; }
        }
    }

    #endregion

    #region ShaderParameter

    [Flags]
    public enum ParameterModifier
    {
        None = 0,
        Const = 1,
        In = 2,
        Out = 4,
        /// <summary>
        /// Not used
        /// </summary>
        ByRef = 8,
        ConstIn = Const | In,
        ConstOut = Const | Out,
        InOut = In | Out,
        ConstInOut = Const | In | Out
    }

    public abstract class ShaderParameter
    {
        internal ShaderParameter()
            : base()
        {
        }

        public abstract bool IsSelf { get; }

        public abstract string Name { get; }

        public abstract ParameterModifier Modifier { get; }
    }

    class PrimitiveShaderParameter : ShaderParameter
    {
        internal PrimitiveShaderParameter(Builtins set, string name, ParameterInfo netParameter)
            : base()
        {
            this.NetMember = netParameter;
            this._Name = name;
            this._Semantic = ShaderSemantics.Resolve(netParameter);
            this._Type = set.ResolveType(netParameter.ParameterType);

            _Modifier = ParameterModifier.None;

            if ((netParameter.Attributes & ParameterAttributes.In) != 0)
                _Modifier |= ParameterModifier.In;
            if ((netParameter.Attributes & ParameterAttributes.Out) != 0)
                _Modifier |= ParameterModifier.Out;
        }

        public override bool IsPrimitive
        {
            get { return true; }
        }

        ShaderType _Type;
        public override ShaderType Type
        {
            get { return _Type; }
        }

        Semantic _Semantic;
        public override Semantic Semantic
        {
            get { return _Semantic; }
        }

        string _Name;
        public override string Name
        {
            get { return _Name; }
        }

        public override bool IsSelf
        {
            get { return false; }
        }

        ParameterModifier _Modifier;
        public override ParameterModifier Modifier
        {
            get { return _Modifier; }
        }

        public ParameterInfo NetMember
        {
            get;
            private set;
        }
    }

    public class UserShaderParameter : ShaderParameter
    {
        ParameterDeclarationAST ast;

        internal UserShaderParameter(ParameterDeclarationAST ast)
        {
            this.ast = ast;
        }

        public override string Name
        {
            get { return ast.Label; }
        }

        public override bool IsPrimitive
        {
            get { return false; }
        }

        public override Semantic Semantic
        {
            get { return ast.Semantic; }
        }

        public override ShaderType Type
        {
            get { return ast.Type; }
        }

        public override bool IsSelf
        {
            get { return false; }
        }

        public override ParameterModifier Modifier
        {
            get
            {
                return (ast.IsIn ? ParameterModifier.In : ParameterModifier.None) | (ast.IsOut ? ParameterModifier.Out : ParameterModifier.None) |
                    (ast.IsConst ? ParameterModifier.Const : ParameterModifier.None);
            }
        }
    }
    #endregion
}