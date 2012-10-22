using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Rendering.Effects.Shaders
{
    #region ShaderMember

    public abstract class ShaderMember
    {
        internal ShaderMember()
        {
        }

        public abstract string Name { get; }

        public abstract bool IsPrimitive { get; }
    }

    #endregion

    #region ShaderType

    public abstract class ShaderType : ShaderMember
    {
        internal ShaderType()
            : base()
        {
        }

        protected internal ShaderFieldAccessResolver AccessResolver { get; set; }

        public abstract ShaderFieldAccess Access(string fieldAccess);

        public abstract IEnumerable<ShaderFunction> Functions { get; }

        public abstract IEnumerable<ShaderConstructor> Constructors { get; }

        public abstract bool IsScalar { get; }

        public ShaderArray MakeArrayType()
        {
            return new ShaderArray(this);
        }

        public ShaderArray MakeArrayType(int length)
        {
            return new ShaderArray(this)
            {
                HasFixedLength = true,
                Length = length
            };
        }

        public ShaderConstructor GetDefaultConstructor()
        {
            return new DefaultConstructor(this);
        }
    }

    public abstract class ShaderFieldAccessResolver
    {
        protected ShaderFieldAccessResolver()
        {
        }

        protected internal PrimitiveSet PrimitiveSet { get; set; }

        public abstract ShaderType GetAccessType(string fieldAccess);

        public abstract object GetAccessValue(object target, string fieldAccess);

        public abstract IEnumerable<string> ValidNames { get; }
    }

    public class DefaultFieldAccessResolver : ShaderFieldAccessResolver
    {
        public DefaultFieldAccessResolver(IEnumerable<ShaderField> fields)
        {
            this.fields = fields;
        }

        IEnumerable<ShaderField> fields;

        public override IEnumerable<string> ValidNames
        {
            get
            {
                return fields.Select(f => f.Name);
            }
        }

        public override ShaderType GetAccessType(string fieldAccess)
        {
            ShaderField field = fields.FirstOrDefault(f => f.Name == fieldAccess);
            if (field == null)
                return null;
            return field.Type;
        }

        public override object GetAccessValue(object target, string fieldAccess)
        {
            ShaderField field = fields.FirstOrDefault(f => f.Name == fieldAccess);
            if (field == null)
                return null;
            return PrimitiveSet.Invoke(field, target);
        }
    }

    class PrimitiveShaderType : ShaderType
    {
        internal PrimitiveShaderType(PrimitiveSet set, string name, Type netType, ShaderFieldAccessResolver accessResolver)
            : base()
        {
            this.NetType = netType;
            this.PrimitiveSet = set;
            this._name = name;
            this.AccessResolver = accessResolver;
        }

        internal PrimitiveShaderType(PrimitiveSet set, string name, Type netType)
            : this(set, name, netType, null)
        {
            this.AccessResolver = new DefaultFieldAccessResolver(this.Fields);
        }

        string _name;
        public override string Name
        {
            get { return _name; }
        }

        public override bool IsPrimitive
        {
            get { return true; }
        }

        public override ShaderFieldAccess Access(string fieldAccess)
        {
            ShaderType type = AccessResolver.GetAccessType (fieldAccess);

            return new PrimitiveShaderFieldAccess(type, fieldAccess, o => AccessResolver.GetAccessValue(o, fieldAccess));
        }

        public Type NetType { get; private set; }

        public PrimitiveSet PrimitiveSet { get; private set; }

        public override bool IsScalar
        {
            get { return NetType.IsPrimitive; }
        }

        private IEnumerable<ShaderField> Fields
        {
            get
            {
                return NetType.GetMembers().
                    Where(m => this.PrimitiveSet.IsDefined(m) && m is System.Reflection.FieldInfo).
                    Select(m => this.PrimitiveSet.ResolveField((System.Reflection.FieldInfo)m));
            }
        }

        public override IEnumerable<ShaderConstructor> Constructors
        {
            get
            {
                return NetType.GetMembers().
                    Where(m => this.PrimitiveSet.IsDefined(m) && m is System.Reflection.ConstructorInfo).
                    Select(m => this.PrimitiveSet.ResolveConstructor((System.Reflection.ConstructorInfo)m)).Union(new ShaderConstructor[] { GetDefaultConstructor() });
            }
        }

        public override IEnumerable<ShaderFunction> Functions
        {
            get
            {

                throw new NotSupportedException("O-O is not supported.");
            }
        }
    }

    class UserShaderType : ShaderType
    {
        StructDeclarationAST ast;
        public UserShaderType(StructDeclarationAST ast)
        {
            this.ast = ast;
            this.AccessResolver = new DefaultFieldAccessResolver(this.Fields);
        }

        public override string Name
        {
            get { return ast.Label; }
        }

        private IEnumerable<ShaderField> Fields
        {
            get { return ast.Fields.Select(f => f.Field); }
        }

        public override IEnumerable<ShaderFunction> Functions
        {
            get { throw new NotSupportedException("O-O is not supported"); }
        }

        public override IEnumerable<ShaderConstructor> Constructors
        {
            get { yield return GetDefaultConstructor(); }
        }

        public override ShaderFieldAccess Access(string fieldAccess)
        {
            return new UserShaderFieldAccess(ast.Fields.First(f => f.Label == fieldAccess));
        }

        public override bool IsScalar
        {
            get { return false; }
        }

        public override bool IsPrimitive
        {
            get { return false; }
        }
    }

    #endregion

    #region ShaderArray

    public class ShaderArray : ShaderType
    {
        internal ShaderArray(ShaderType elementType)
            : base()
        {
            this.ElementType = elementType;
        }

        public ShaderType ElementType
        {
            get;
            private set;
        }

        public bool HasFixedLength { get; internal set; }

        public int Length { get; internal set; }

        public override ShaderFieldAccess Access(string fieldAccess)
        {
            throw new NotSupportedException();
        }

        public override IEnumerable<ShaderFunction> Functions
        {
            get { throw new NotSupportedException("O-O is not supported."); }
        }

        public override IEnumerable<ShaderConstructor> Constructors
        {
            get { yield break; }
        }

        public override string Name
        {
            get { return "Array of " + ElementType.Name; }
        }

        public override bool IsScalar
        {
            get { return false; }
        }

        public override bool IsPrimitive
        {
            get { return true; }
        }
    }

    #endregion

    #region ShaderVariable

    public abstract class ShaderVariable : ShaderMember
    {
        internal ShaderVariable()
            : base()
        {
        }

        public abstract ShaderType Type { get; }
    }

    public abstract class ShaderFieldAccess : ShaderVariable
    {
    }

    public abstract class SemantizedShaderVariable : ShaderVariable
    {
        internal SemantizedShaderVariable()
            : base()
        {
        }

        public abstract Semantic Semantic { get; }
    }

    #endregion

    #region ShaderField

    public abstract class ShaderField : SemantizedShaderVariable
    {
        internal ShaderField() :
            base()
        {
        }
    }

    class PrimitiveShaderFieldAccess : ShaderFieldAccess
    {
        public PrimitiveShaderFieldAccess(ShaderType type, string name, Func<object, object> Resolver)
        {
            this.NetResolver = Resolver;
            this._Name = name;
            this._type = type;
        }

        public Func<object, object> NetResolver { get; private set; }

        public override bool IsPrimitive
        {
            get { return true; }
        }

        string _Name;
        public override string Name
        {
            get { return _Name; }
        }

        ShaderType _type;
        public override ShaderType Type
        {
            get { return _type; }
        }
    }

    class UserShaderFieldAccess : ShaderFieldAccess
    {
        FieldDeclarationAST ast;
        
        public UserShaderFieldAccess(FieldDeclarationAST fieldDeclaration)
        {
            this.ast = fieldDeclaration;
        }

        public override string Name
        {
            get { return ast.Label; }
        }

        public override bool IsPrimitive
        {
            get { return false; }
        }

        public override ShaderType Type
        {
            get { return ast.Type; }
        }
    }

    class PrimitiveShaderField : ShaderField
    {
        internal PrimitiveShaderField(PrimitiveSet set, string name, FieldInfo netField)
            : base()
        {
            this.NetField = netField;
            this._Name = name;
            this._Semantic = ShaderSemantics.Resolve(netField);
            this._Type = set.ResolveType(netField.FieldType);
            this.NetField = netField;
        }

        public FieldInfo NetField
        {
            get;
            private set;
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
    }

    class UserShaderField : ShaderField
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
    }

    #endregion

    #region ShaderLocal

    public abstract class ShaderLocal : ShaderVariable
    {
        internal ShaderLocal() :
            base()
        {
        }
    }

    class UserShaderLocal : ShaderLocal
    {
        LocalDeclarationAST ast;

        internal UserShaderLocal(LocalDeclarationAST ast)
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

        public override ShaderType Type
        {
            get { return ast.Type; }
        }
    }

    #endregion

    #region ShaderGlobal

    public abstract class ShaderGlobal : SemantizedShaderVariable
    {
        internal ShaderGlobal() :
            base()
        {
        }
    }

    class PrimitiveShaderGlobal : ShaderGlobal
    {
        internal PrimitiveShaderGlobal(PrimitiveSet set, string name, FieldInfo netField)
            : base()
        {
            this.NetField = netField;
            this._Name = name;
            this._Semantic = ShaderSemantics.Resolve(netField);
            this._Type = set.ResolveType(netField.FieldType);
            this.NetField = netField;
        }

        public FieldInfo NetField
        {
            get;
            private set;
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
    }

    public class UserShaderGlobal : ShaderGlobal
    {
        GlobalDeclarationAST ast;

        internal UserShaderGlobal(GlobalDeclarationAST ast)
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

    public abstract class ShaderParameter : SemantizedShaderVariable
    {
        internal ShaderParameter()
            : base()
        {
        }

        public abstract bool IsSelf { get; }

        public abstract ParameterModifier Modifier { get; }
    }

    class SelfShaderParameter : ShaderParameter
    {
        internal SelfShaderParameter(ShaderType type)
        {
            _Type = type;
        }

        public override bool IsSelf
        {
            get { return false; }
        }

        public override Semantic Semantic
        {
            get { return null; }
        }

        private ShaderType _Type;
        public override ShaderType Type
        {
            get { return _Type; }
        }

        public override string Name
        {
            get { return "this"; }
        }

        public override bool IsPrimitive
        {
            get { return true; }
        }

        public override ParameterModifier Modifier
        {
            get { return ParameterModifier.In; }
        }
    }

    class PrimitiveShaderParameter : ShaderParameter
    {
        internal PrimitiveShaderParameter(PrimitiveSet set, string name, ParameterInfo netParameter)
            : base()
        {
            this.NetParameter = netParameter;
            this._Name = name;
            this._Semantic = ShaderSemantics.Resolve(netParameter);
            this._Type = set.ResolveType(netParameter.ParameterType);

            _Modifier = ParameterModifier.None;

            if ((netParameter.Attributes & ParameterAttributes.In) != 0)
                _Modifier |= ParameterModifier.In;
            if ((netParameter.Attributes & ParameterAttributes.Out) != 0)
                _Modifier |= ParameterModifier.Out;
        }

        public ParameterInfo NetParameter
        {
            get;
            private set;
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

    #region ShaderFunction

    public abstract class ShaderMethodBase : ShaderMember
    {
        public abstract ShaderParameter[] Parameters { get; }
    }

    public abstract class ShaderConstructor : ShaderMethodBase
    {
        public abstract ShaderType DeclaringType { get; }

        public bool IsDefault
        {
            get { return this is DefaultConstructor; }
        }
    }

    public abstract class ShaderFunction : ShaderMethodBase
    {
        public abstract ShaderType ReturnType { get; }

        public abstract Operators Operator { get; }

        public bool IsOperator { get { return Operator != Operators.None; } }
    }

    class PrimitiveShaderFunction : ShaderFunction
    {
        internal PrimitiveShaderFunction(PrimitiveSet set, MethodInfo method, string name)
            : base()
        {
            this.NetFunction = method;
            this._Name = name;
            this._ReturnType = set.ResolveType (method.ReturnType);
            this._Parameters = method.GetParameters().Select(pi => (ShaderParameter)new PrimitiveShaderParameter(set, pi.Name, pi)).ToArray();
            if (!method.IsStatic)
                this._Parameters = new ShaderParameter[] { 
                    new SelfShaderParameter (set.ResolveType (method.DeclaringType))
                }.Union(this._Parameters).ToArray();
            this._Operator = Operators.None;
        }

        internal PrimitiveShaderFunction(PrimitiveSet set, MethodInfo method, Operators op) :
            this(set, method, NameFor(op))
        {
            this._Operator = op;
        }

        private static string NameFor(Operators op)
        {
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
            get { return true;}
        }

        public MethodInfo NetFunction { get; private set; }
    }

    class UserShaderFunction : ShaderFunction
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
            get { return ast.Parameters.Select (p => p.Parameter).ToArray (); }
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
        internal PrimitiveShaderConstructor(PrimitiveSet set, ConstructorInfo constructor)
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
            get { return _Parameters.ToArray (); }
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
        internal DefaultConstructor(ShaderType type) : base ()
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

    #endregion
}