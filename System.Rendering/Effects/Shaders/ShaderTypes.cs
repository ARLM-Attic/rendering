using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Rendering.Effects.Shaders
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    public class CompilePrimitiveTypeAsAttribute : Attribute
    {
        public Type ASTNodeType { get; set; }
    }

    public static class ShaderTypes
    {
        public static readonly ShaderBooleanType Boolean = new ShaderBooleanType();
        public static readonly ShaderInt16Type Int16 = new ShaderInt16Type();
        public static readonly ShaderInt32Type Int32 = new ShaderInt32Type();
        public static readonly ShaderInt64Type Int64 = new ShaderInt64Type();
        public static readonly ShaderFloatType Float = new ShaderFloatType();
        public static readonly ShaderDoubleType Double = new ShaderDoubleType();
        public static readonly ShaderByteType Byte = new ShaderByteType();
        public static readonly ShaderVoidType Void = new ShaderVoidType();
        public static readonly ShaderStringType String = new ShaderStringType();
        public static readonly ShaderTextureType Texture = new ShaderTextureType();
        public static readonly ShaderVextexShader VertexShader = new ShaderVextexShader();
        public static readonly ShaderPixelShader PixelShader = new ShaderPixelShader();
        public static readonly ShaderSamplerType Sampler1D = new ShaderSamplerType(SamplerType.Sanpler1D);
        public static readonly ShaderSamplerType Sampler2D = new ShaderSamplerType(SamplerType.Sanpler2D);
        public static readonly ShaderSamplerType Sampler3D = new ShaderSamplerType(SamplerType.Sanpler3D);
        public static readonly ShaderSamplerType SamplerCube = new ShaderSamplerType(SamplerType.SanplerCube);
        public static readonly ShaderSamplerType Sampler1DShadow = new ShaderSamplerType(SamplerType.Sanpler1DShadow);
        public static readonly ShaderSamplerType Sampler2DShadow = new ShaderSamplerType(SamplerType.Sanpler2DShadow);
        public static ShaderMatrixType Matrix<T>(int r, int c)
        {
            if (!matrices.ContainsKey(new Pair(typeof(T), r, c)))
                matrices.Add(new Pair(typeof(T), r, c), new ShaderMatrixType(r, c, ResolveType<T>()));
            return matrices[new Pair(typeof(T), r, c)];
        }
        public static ShaderVectorType Vector<T>(int length)
        {
            if (!vectors.ContainsKey(new Simple(typeof(T), length)))
                vectors.Add(new Simple(typeof(T), length), new ShaderVectorType(length, ResolveType<T>()));
            return vectors[new Simple(typeof(T), length)];
        }

        #region Keys

        struct Pair
        {
            public Type Type;
            public int rows, cols;

            public Pair(Type type, int rows, int cols)
            {
                this.Type = type;
                this.rows = rows;
                this.cols = cols;
            }

            public override bool Equals(object obj)
            {
                return ((Pair)obj).rows == this.rows && ((Pair)obj).cols == this.cols && ((Pair)obj).Type == this.Type;
            }

            public override int GetHashCode()
            {
                return rows * cols + rows + Type.MetadataToken;
            }
        }

        struct Simple
        {
            public Type Type;
            public int components;

            public Simple(Type type, int components)
            {
                this.Type = type;
                this.components = components;
            }

            public override bool Equals(object obj)
            {
                return this.Type == ((Simple)obj).Type && this.components == ((Simple)obj).components;
            }

            public override int GetHashCode()
            {
                return this.components + this.Type.MetadataToken;
            }

        }

        #endregion

        #region Dictionaries

        private static Dictionary<Pair, ShaderMatrixType> matrices = new Dictionary<Pair, ShaderMatrixType>();
        
        private static Dictionary<Simple, ShaderVectorType> vectors = new Dictionary<Simple, ShaderVectorType>();

        #endregion

        private static Dictionary<Type, ShaderType> resolvedTypes = new Dictionary<Type, ShaderType>();

        static ShaderTypes()
        {
            resolvedTypes.Add(typeof(Int16), Int16);
            resolvedTypes.Add(typeof(Int32), Int32);
            resolvedTypes.Add(typeof(Single), Float);
            resolvedTypes.Add(typeof(Double), Double);
            resolvedTypes.Add(typeof(Byte), Byte);
            resolvedTypes.Add(typeof(void), Void);
            resolvedTypes.Add(typeof(Boolean), Boolean);
            resolvedTypes.Add(typeof(Sampler), Sampler2D);
            resolvedTypes.Add(typeof(Vector1<float>), Vector<float>(1));
            resolvedTypes.Add(typeof(Vector2<float>), Vector<float>(2));
            resolvedTypes.Add(typeof(Vector3<float>), Vector<float>(3));
            resolvedTypes.Add(typeof(Vector4<float>), Vector<float>(4));
            resolvedTypes.Add(typeof(Matrix1x1<float>), Matrix<float>(1, 1));
            resolvedTypes.Add(typeof(Matrix2x2<float>), Matrix<float>(2, 2));
            resolvedTypes.Add(typeof(Matrix3x3<float>), Matrix<float>(3, 3));
            resolvedTypes.Add(typeof(Matrix4x4<float>), Matrix<float>(4, 4));
        }

        public static ShaderType ResolveType(Type type)
        {
            if (resolvedTypes.ContainsKey(type))
                return resolvedTypes[type];

            if (type.GetCustomAttributes(typeof(CompilePrimitiveTypeAsAttribute), true).Length > 0)
                try
                {
                    return (ShaderType)Activator.CreateInstance(((CompilePrimitiveTypeAsAttribute)type.GetCustomAttributes(typeof(CompilePrimitiveTypeAsAttribute), true)[0]).ASTNodeType);
                }
                catch
                {
                    throw new InvalidOperationException("The type you are requesting analog ShaderType is wrongly attributed as Compiled");
                }

            return null;
        }

        public static ShaderType ResolveType<T>()
        {
            return ResolveType(typeof(T));
        }       
    }

    public abstract class ShaderType
    {
        protected ShaderType(string name)
        {
            Name = name;
        }

        public abstract ShaderType GetMemberType(string member);

        public abstract int NumberOfComponents { get; }

        public abstract IEnumerable<ShaderType> ExpandedMembers { get; }

        public string Name { get; set; }
    }

    public abstract class PrimitiveType : ShaderType
    {
        public PrimitiveType(string name)
            : base(name)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return this.GetType().Equals(obj.GetType());
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
    }

    public abstract class ShaderObjectType : PrimitiveType
    {
        public ShaderObjectType(string name)
            : base(name)
        {
        }

        public override ShaderType GetMemberType(string member)
        {
            return null;
        }

        public override int NumberOfComponents
        {
            get { return 1; }
        }

        public override IEnumerable<ShaderType> ExpandedMembers
        {
            get { yield return this; }
        }
    }

    public class ShaderStringType : ShaderObjectType
    {
        public ShaderStringType()
            : base("string")
        {
        }
    }

    public class ShaderTextureType : ShaderObjectType
    {
        public ShaderTextureType()
            : base("texture")
        {
        }
    }

    public class ShaderPixelShader : ShaderObjectType
    {
        public ShaderPixelShader()
            : base("pixelshader")
        {
        }
    }

    public class ShaderVextexShader : ShaderObjectType
    {
        public ShaderVextexShader()
            : base("vertexshader")
        {
        }
    }

    public class ShaderSamplerType : ShaderObjectType
    {
        public ShaderSamplerType(SamplerType samplerType)
            : base("sampler")
        {
            SamplerType = samplerType;
        }

        public SamplerType SamplerType { get; set; }
    }

    public enum SamplerType
    {
        Sanpler1D,
        Sanpler2D,
        Sanpler3D,
        SanplerCube,
        Sanpler1DShadow,
        Sanpler2DShadow
    }

    public class ShaderArrayType : PrimitiveType
    {
        public ShaderArrayType(string name, int? length, ShaderType elementType)
            : base(name)
        {
            Length = length;
            ElementType = elementType;
        }

        public override bool Equals(object obj)
        {
            ShaderArrayType arrType = obj as ShaderArrayType;
            if (arrType == null)
                return false;
            return base.Equals(obj) && Length == arrType.Length;
        }

        public override ShaderType GetMemberType(string member)
        {
            return null;
        }

        public override int NumberOfComponents
        {
            get { return Length != null ? Length.Value : -1; }
        }

        public override IEnumerable<ShaderType> ExpandedMembers
        {
            get
            {
                for (int i = 0; i < Length; i++)
                    yield return ElementType;
            }
        }

        public ShaderType ElementType { get; set; }

        public int? Length { get; set; }
    }

    public interface IFields
    {
        IEnumerable<FieldInfo> Fields { get; }
    }

    public class ShaderFixedArrayType : ShaderArrayType
    {
        internal protected ShaderFixedArrayType(string name, int length, ShaderType elementType)
            : base(name, length, elementType)
        {
            Length = length;
        }

        public new int Length { get; set; }
    }

    public class ShaderVectorType : ShaderFixedArrayType , IFields
    {
        FieldInfo[] fields ;

        internal ShaderVectorType(int length, ShaderType elementType)
            : base("Vector", length, elementType)
        {
                string [] names = new string[] { "x", "y", "z", "w" };

                List<FieldInfo> f = new List<FieldInfo> ();
                for (int i = 0; i < length; i++)
                    f.Add(new PrimitiveTypeFieldInfo(names[i], elementType, null));

            fields = f.ToArray ();

        }

        public IEnumerable<FieldInfo> Fields
        {
            get { return fields; }
        }
    }

    public class ShaderMatrixType : ShaderFixedArrayType, IFields
    {
        FieldInfo[] fields;

        internal ShaderMatrixType(int rows, int columns, ShaderType elementType)
            : base("Matrix", rows * columns, elementType)
        {
            Rows = rows;
            Columns = columns;

            List<FieldInfo> f = new List<FieldInfo>();

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < columns; c++)
                    f.Add(new PrimitiveTypeFieldInfo("M" + r + c, elementType, null));

            fields = f.ToArray();

        }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public IEnumerable<FieldInfo> Fields
        {
            get { return fields; }
        }
    }

    public class ShaderScalarType : PrimitiveType
    {
        protected ShaderScalarType(string name)
            : base(name)
        {
        }

        public override ShaderType GetMemberType(string member)
        {
            return null;
        }

        public override int NumberOfComponents
        {
            get { return 1; }
        }

        public override IEnumerable<ShaderType> ExpandedMembers
        {
            get { yield return this; }
        }
    }

    public class ShaderBooleanType : ShaderScalarType
    {
        internal ShaderBooleanType()
            : base("bool")
        {
        }
    }

    public class ShaderByteType : ShaderScalarType
    {
        internal ShaderByteType() : base("byte") { }
    }

    public class ShaderInt16Type : ShaderScalarType
    {
        internal ShaderInt16Type() : base("short") { }
    }

    public class ShaderInt32Type : ShaderScalarType
    {
        internal ShaderInt32Type() : base("int") { }
    }

    public class ShaderInt64Type : ShaderScalarType
    {
        internal ShaderInt64Type() : base("long") { }
    }

    public class ShaderFloatType : ShaderScalarType
    {
        internal ShaderFloatType() : base("float") { }
    }

    public class ShaderDoubleType : ShaderScalarType
    {
        internal ShaderDoubleType() : base("double") { }
    }

    public class ShaderVoidType : ShaderScalarType
    {
        internal ShaderVoidType()
            : base("void")
        {
        }
    }

    public abstract class UserDefinedType : ShaderType
    {
        protected UserDefinedType(string name)
            : base(name)
        {
        }
    }

    public class StructType : UserDefinedType, IFields
    {
        internal StructType(string name, IEnumerable<FieldInfo> fields)
            : base(name)
        {
            this.Fields = fields;
        }

        public override ShaderType GetMemberType(string member)
        {
            FieldInfo fieldMember = Fields.FirstOrDefault(field => field.Name == member);
            return fieldMember != null ? fieldMember.Type : null;
        }

        public override int NumberOfComponents
        {
            get { return Fields.Count(); }
        }

        public override IEnumerable<ShaderType> ExpandedMembers
        {
            get 
            {
                return Fields.SelectMany(field => field.Type.ExpandedMembers);
            }
        }

        public IEnumerable<FieldInfo> Fields { get; private set; }
    }

    
    sealed class PrimitiveTypeFieldInfo : FieldInfo
    {
        internal PrimitiveTypeFieldInfo(string name, ShaderType type, Semantic semantic)
        {
            _Name = name;
            _Type = type;
            _Semantic = semantic;
        }

        Semantic _Semantic;
        public override Semantic Semantic { get { return _Semantic; } }

        string _Name;
        public override string Name { get { return _Name; } }

        ShaderType _Type;
        public override ShaderType Type { get { return _Type; } }

    }
}
