#define FLOAT

#if DECIMAL
using FLOATINGTYPE = System.Decimal;
#endif
#if DOUBLE
using FLOATINGTYPE = System.Double;
#endif
#if FLOAT
using FLOATINGTYPE = System.Single;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Compilers.Shaders;

namespace System
{
    /// <summary>
    /// Indicates this method should be compiled in a shader body.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CompilableAttribute : Attribute
    {
    }

    /// <summary>
    /// Base class for semantic purpouses.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public abstract class SemanticAttribute : Attribute
    {
        public SemanticAttribute()
        {
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == this.GetType();
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name.Substring(0, GetType().Name.Length - "Attribute".Length);
        }
    }

    /// <summary>
    /// Base class that specifies the role of certain field at a struct.
    /// </summary>
    public abstract class ElementSemanticAttribute : SemanticAttribute
    {
        public ElementSemanticAttribute()
        {
        }
    }

    /// <summary>
    /// Identifies variables that should be assumed as globals in a shader body.
    /// </summary>
    [CompileSemanticAs(SemanticType = typeof(GlobalSemantic))]
    public class GlobalAttribute : SemanticAttribute
    {
    }

    /// <summary>
    /// Identifies a field that should be used as sampler in a shader body.
    /// </summary>
    [CompileSemanticAs(SemanticType = typeof(SamplerSemantic))]
    public class SamplerAttribute : GlobalAttribute
    {
        public int Register { get; set; }

        public SamplerAttribute() : this(0) { }

        public SamplerAttribute(int register)
        {
            Register = register;
        }

        public override bool Equals(object obj)
        {
            return obj is SamplerAttribute && (((SamplerAttribute)obj).Register == this.Register);
        }
        public override int GetHashCode()
        {
            return Register;
        }
    }

    /// <summary>
    /// Allows to create global variables with a custom label.
    /// This attribute is for compatibility only.
    /// </summary>
    public class CustomAttribute : GlobalAttribute
    {
        public string Label { get; set; }

        public CustomAttribute(string label)
        {
            this.Label = label;
        }
    }

    /// <summary>
    /// Base class for indexed field elements.
    /// </summary>
    public abstract class IndexedSemanticAttribute : ElementSemanticAttribute
    {
        int index;

        public IndexedSemanticAttribute(int index) { this.index = index; }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == this.GetType() && ((IndexedSemanticAttribute)obj).Index == Index;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() + this.Index;
        }

    }

    #region Vertex Elements Semantics

    /// <summary>
    /// Indicates this field will be used as Position of some vertex.
    /// </summary>
    [CompileSemanticAs(SemanticType = typeof(PositionSemantic))]
    public class PositionAttribute : IndexedSemanticAttribute
    {
        public PositionAttribute() : base(0) { }
        public PositionAttribute(int index)
            : base(index)
        {
        }
    }

    /// <summary>
    /// Indicates this field will be used as Projection info of some vertex.
    /// </summary>
    [CompileSemanticAs(SemanticType = typeof(ProjectedSemantic))]
    public class ProjectedAttribute : IndexedSemanticAttribute
    {
        public ProjectedAttribute() : base(0) { }
        public ProjectedAttribute(int index)
            : base(index)
        {
        }
    }

    /// <summary>
    /// Base class to specify this field will be used as a Texture Coordinates.
    /// </summary>
    [CompileSemanticAs(SemanticType = typeof(TextureCoordinatesSemantic))]
    public abstract class TextureCoordinatesAttribute : IndexedSemanticAttribute
    {
        public TextureCoordinatesAttribute() : this(0) { }

        public TextureCoordinatesAttribute(int index)
            : base(index)
        {
        }
    }


    /// <summary>
    /// Indicates this field will be used as diffuse information of a vertex.
    /// </summary>
    [CompileSemanticAs(SemanticType = typeof(DiffuseSemantic))]
    public class DiffuseAttribute : ElementSemanticAttribute
    {
        public DiffuseAttribute()
        {
        }
    }

    /// <summary>
    /// Indicates this field will be used as specular information of a vertex.
    /// </summary>
    [CompileSemanticAs(SemanticType = typeof(SpecularSemantic))]
    public class SpecularAttribute : ElementSemanticAttribute
    {
    }

    /// <summary>
    /// Indicates this field will be used as normal information of vertex.
    /// </summary>
    [CompileSemanticAs(SemanticType = typeof(NormalSemantic))]
    public class NormalAttribute : IndexedSemanticAttribute
    {
        public NormalAttribute() : this(0) { }
        public NormalAttribute(int index)
            : base(index)
        {
        }
    }

    #endregion

    #region Pixel Elements Semantics

    public class RedAttribute : ElementSemanticAttribute
    {
        public RedAttribute()
        {
        }
    }
    public class GreenAttribute : ElementSemanticAttribute
    {
        public GreenAttribute()
        {
        }
    }
    public class BlueAttribute : ElementSemanticAttribute
    {
        public BlueAttribute()
        {
        }
    }
    public class AlphaAttribute : ElementSemanticAttribute
    {
        public AlphaAttribute()
        {
        }
    }

    public class LuminanceAttribute : ElementSemanticAttribute
    {
        public LuminanceAttribute()
        {
        }
    }


    #endregion
}
