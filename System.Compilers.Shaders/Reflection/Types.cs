using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Reflection.Emit;

namespace System.Compilers.Shaders.Reflection
{
    /// <summary>
    /// Represents a type in the shader language. It contains access to fields, functions and constructors.
    /// </summary>
    public abstract class ShaderType : ShaderMember
    {
        internal ShaderType()
            : base()
        {
        }

        public bool IsArray { get; private set; }

        public abstract bool IsGenericType { get; }

        public abstract bool IsGenericTypeDefinition { get; }

        public abstract bool IsGenericParameter { get; }

        public abstract ShaderType[] GetGenericArguments();

        public abstract IEnumerable<ShaderMember> Members { get; }

        /// <summary>
        /// Gets an IEnumerable object with all field for this type.
        /// </summary>
        public IEnumerable<ShaderField> Fields
        {
            get { return Members.OfType<ShaderField>(); }
        }

        public virtual ShaderType ElementType
        {
            get { return null; }
        }

        public virtual int Rank
        {
            get { return 0; }
        }

        public virtual bool IsFixedArray
        {
            get { return false; }
        }

        public virtual int[] GetRanks()
        {
            if (!IsFixedArray)
                throw new InvalidOperationException();

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a IEnumerable object of valid functions for this type.
        /// </summary>
        public IEnumerable<ShaderMethod> Methods
        {
            get { return Members.OfType<ShaderMethod>(); }
        }

        /// <summary>
        /// Gets a IEnumerable object of valid constructors for this type.
        /// </summary>
        public IEnumerable<ShaderConstructor> Constructors
        {
            get { return Members.OfType<ShaderConstructor>(); }
        }

        /// <summary>
        /// Gets if this type is a scalar type in the shader language.
        /// </summary>
        public abstract bool IsScalar { get; }

        /// <summary>
        /// Gets when this type have a default constructor.
        /// </summary>
        public abstract bool HasDefaultConstructor { get; }

        class ShaderArrayType : ShaderType
        {
            public ShaderArrayType(ShaderType elementType, int rank, int[] ranks)
            {
                _ElementType = elementType;
                _rank = rank;
                _ranks = ranks;
            }
            
            public override bool IsGenericType
            {
                get { return false; }
            }

            public override bool IsGenericTypeDefinition
            {
                get { return false; }
            }

            public override bool IsGenericParameter
            {
                get { return false; }
            }

            public override ShaderType[] GetGenericArguments()
            {
                return new ShaderType[0];
            }

            public override IEnumerable<ShaderMember> Members
            {
                get { yield break; }
            }

            public override bool IsScalar
            {
                get { return false; }
            }

            public override bool HasDefaultConstructor
            {
                get { return false; }
            }

            public override string Name
            {
                get { return ElementType.Name + "[]"; }
            }

            ShaderType _ElementType;
            public override ShaderType ElementType
            {
                get
                {
                    return _ElementType;
                }
            }

            public override bool IsPrimitive
            {
                get { return false; }
            }

            int[] _ranks;
            int _rank;

            public override bool IsFixedArray
            {
                get
                {
                    return _ranks != null;
                }
            }

            public override int Rank
            {
                get
                {
                    return _rank;
                }
            }

            public override int[] GetRanks()
            {
                return _ranks;
            }
        }

        /// <summary>
        /// Creates an array type with this type as element.
        /// </summary>
        /// <returns></returns>
        public ShaderType MakeArrayType()
        {
            return new (this, 1, null);
        }

        /// <summary>
        /// Creates a array type with this type as element.
        /// </summary>
        public ShaderArrayType MakeArrayType(int rank)
        {
            return new ShaderArrayType(this, 1, null);
        }

        /// <summary>
        /// Creates a fixed array of certain lengths.
        /// </summary>
        public ShaderType MakeArrayType(int[] fixedLengths)
        {
            return new ShaderArrayType(this, fixedLengths.Length, fixedLengths);
        }

        /// <summary>
        /// Gets the default constructor for this type.
        /// </summary>
        /// <returns></returns>
        public virtual ShaderConstructor GetDefaultConstructor()
        {
            if (!HasDefaultConstructor) throw new InvalidOperationException();

            return new DefaultConstructor(this);
        }
    }

    class PrimitiveShaderType : ShaderType
    {
        internal PrimitiveShaderType(Builtins set, string name, Type netType)
            : base()
        {
            this.NetType = netType;
            this.Builtins = set;
            this._name = name;
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

        public Type NetType { get; private set; }

        public Builtins Builtins { get; private set; }

        public override bool IsScalar
        {
            get { return NetType.IsPrimitive; }
        }

        public override IEnumerable<ShaderMember> Members
        {
            get { return NetType.GetMembers().Where(m=>Builtins.con; }
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

        public override IEnumerable<ShaderField> Fields
        {
            get
            {
                foreach (var f in ast.Fields)
                    yield return f.Field;
            }
        }

        public override IEnumerable<ShaderMethod> Functions
        {
            get { throw new NotSupportedException("O-O is not supported"); }
        }

        public override IEnumerable<ShaderConstructor> Constructors
        {
            get { yield return GetDefaultConstructor(); }
        }

        public override ShaderFieldAccess Access(string fieldAccess)
        {
            return new UserShaderFieldAccess(ast.Fields.First(f => f.Label.Equals(fieldAccess)));
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

}