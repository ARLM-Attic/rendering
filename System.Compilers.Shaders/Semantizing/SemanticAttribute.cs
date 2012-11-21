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
    /// Base class for semantic purpouses.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Struct, AllowMultiple = true, Inherited = true)]
    public abstract class SemanticAttribute : Attribute
    {
        private int semanticTypeToken;
        private int semanticGetHashCode;

        public SemanticAttribute()
        {
            this.semanticTypeToken = this.GetType().MetadataToken;
            this.semanticGetHashCode = this.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is SemanticAttribute && ((SemanticAttribute)obj).semanticTypeToken == this.semanticTypeToken;
        }

        public override int GetHashCode()
        {
            return semanticGetHashCode;
        }

        public override string ToString()
        {
            return GetType().Name.Substring(0, GetType().Name.Length - "Attribute".Length);
        }
    }

    public enum ComponentMap
    {
        /// <summary>
        /// Component value doesnt need to be mapped.
        /// </summary>
        None,
        /// <summary>
        /// Component value is an integer and maps to range 0..1 using number of bits to know the greater value.
        /// </summary>
        Unsigned,
        /// <summary>
        /// Component value is an integer and maps to range -1..1 using number of bits to know the less value and the greater one.
        /// </summary>
        Signed
    }

    public abstract class DataComponentAttribute : SemanticAttribute
    {
        public ComponentMap Mapping { get; protected set; }

        public int StartBit { get; protected set; }

        public bool MatchField { get; protected set; }
    }

    /// <summary>
    /// Base class that specifies the role of certain field at a struct.
    /// </summary>
    public abstract class VertexComponentAttribute : DataComponentAttribute
    {
        public VertexComponentAttribute()
        {
            Mapping = ComponentMap.None;
            MatchField = true;
            StartBit = 0;
        }
    }

    /// <summary>
    /// Base class for indexed field elements.
    /// </summary>
    public abstract class IndexedComponentAttribute : VertexComponentAttribute
    {
        int index;

        public IndexedComponentAttribute(int index) { this.index = index; }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && ((IndexedComponentAttribute)obj).Index == Index;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() + this.Index;
        }

        public override string ToString()
        {
            return base.ToString()+Index;
        }

    }

    [CompileSemanticAs(SemanticType = typeof(SamplerSemantic))]
    public class SamplerAttribute : SemanticAttribute
    {
        public SamplerAttribute() { }
    }

    #region Vertex elements semantics

    /// <summary>
    /// Indicates this field will be used as Position of some vertex.
    /// </summary>
    [CompileSemanticAs(SemanticType = typeof(PositionSemantic))]
    public class PositionAttribute : IndexedComponentAttribute
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
    public class ProjectedAttribute : IndexedComponentAttribute
    {
        public ProjectedAttribute() : base(0) { }
        public ProjectedAttribute(int index)
            : base(index)
        {
        }
    }

    /// <summary>
    /// Class to specify this field will be used as a Texture Coordinates.
    /// </summary>
    [CompileSemanticAs(SemanticType = typeof(CoordinatesSemantic))]
    public class CoordinatesAttribute : IndexedComponentAttribute
    {
        public CoordinatesAttribute() : this(0) { }

        public CoordinatesAttribute(int index)
            : base(index)
        {
        }
    }

    [CompileSemanticAs(SemanticType = typeof(ColorSemantic))]
    public class ColorAttribute : IndexedComponentAttribute
    {
        public ColorAttribute() : this(0) { }

        public ColorAttribute(int index)
            : base(index)
        {
        }
    }

    /// <summary>
    /// Indicates this field will be used as normal information of vertex.
    /// </summary>
    [CompileSemanticAs(SemanticType = typeof(NormalSemantic))]
    public class NormalAttribute : IndexedComponentAttribute
    {
        public NormalAttribute() : this(0) { }
        public NormalAttribute(int index)
            : base(index)
        {
        }
    }

    [CompileSemanticAs(SemanticType = typeof(TangentSemantic))]
    public class TangentAttribute : IndexedComponentAttribute
    {
        public TangentAttribute() : this(0) { }
        public TangentAttribute(int index) : base(index) { }
    }

    [CompileSemanticAs(SemanticType = typeof(BinormalSemantic))]
    public class BinormalAttribute : IndexedComponentAttribute
    {
        public BinormalAttribute() : this(0) { }
        public BinormalAttribute(int index) : base(index) { }
    }

    [CompileSemanticAs(SemanticType = typeof(WeightSemantic))]
    public class WeightAttribute : IndexedComponentAttribute
    {
        public WeightAttribute(int index)
            : base(index)
        {
        }
    }

    [CompileSemanticAs(SemanticType = typeof(IndexSemantic))]
    public class IndexAttribute : IndexedComponentAttribute
    {
        public IndexAttribute(int index)
            : base(index)
        {
        }
    }

    #endregion

    #region Texel elements semantics.

    public abstract class TexelSemanticAttribute : DataComponentAttribute 
    {
        public abstract int Channel { get; }

        internal TexelSemanticAttribute(int offset, ComponentMap map)
        {
            this.StartBit = offset;
            this.MatchField = false;
            this.Mapping = map;
        }

        internal TexelSemanticAttribute()
        {
            this.MatchField = true;
            this.Mapping = ComponentMap.None;
        }

        public override bool Equals(object obj)
        {
            return obj is TexelSemanticAttribute && ((TexelSemanticAttribute)obj).Channel == this.Channel;
        }
        public override int GetHashCode()
        {
            return this.Channel;
        }
    }

    public class RedAttribute : TexelSemanticAttribute
    {
        public override int Channel
        {
            get { return 1; }
        }

        /// <summary>
        /// Identifies a Red unsigned component.
        /// </summary>
        public RedAttribute() : base() { }

        /// <summary>
        /// Identifies a unsigned component.
        /// </summary>
        /// <param name="startBit">Number of start bit</param>
        public RedAttribute(int startBit) : base(startBit, ComponentMap.Unsigned) { }
    }

    public class GreenAttribute : TexelSemanticAttribute
    {
        public override int Channel
        {
            get { return 2; }
        }

        /// <summary>
        /// Identifies a 8-bit unsigned component.
        /// </summary>
        public GreenAttribute() : base() { }

        /// <summary>
        /// Identifies a unsigned component.
        /// </summary>
        /// <param name="startBit">Number of start bit</param>
        public GreenAttribute(int startBit) : base(startBit, ComponentMap.Unsigned) { }
    }

    public class BlueAttribute : TexelSemanticAttribute
    {
        public override int Channel
        {
            get { return 3; }
        }

        /// <summary>
        /// Identifies a blue unsigned component.
        /// </summary>
        public BlueAttribute() : base() { }

        /// <summary>
        /// Identifies a unsigned component.
        /// </summary>
        /// <param name="bits">Number of start bit</param>
        public BlueAttribute(int startBit) : base(startBit, ComponentMap.Unsigned) { }
    }

    public class AlphaAttribute : TexelSemanticAttribute
    {
        public override int Channel
        {
            get { return 4; }
        }

        /// <summary>
        /// Identifies an alpha unsigned component.
        /// </summary>
        public AlphaAttribute() : base() { }

        /// <summary>
        /// Identifies a unsigned component.
        /// </summary>
        /// <param name="startBit">Number of start bit</param>
        public AlphaAttribute(int startBit) : base(startBit, ComponentMap.Unsigned) { }
    }

    public class XAttribute : TexelSemanticAttribute
    {
        public override int Channel
        {
            get { return 1; }
        }

        public XAttribute() : base() { }

        public XAttribute(int bits)
            : base(bits, ComponentMap.Signed)
        {
        }
    }

    public class YAttribute : TexelSemanticAttribute
    {
        public override int Channel
        {
            get { return 2; }
        }

        public YAttribute() : base() { }

        public YAttribute(int bits)
            : base(bits, ComponentMap.Signed)
        {
        }
    }

    public class ZAttribute : TexelSemanticAttribute
    {
        public override int Channel
        {
            get { return 3; }
        }

        public ZAttribute() : base() { }

        public ZAttribute(int bits)
            : base(bits, ComponentMap.Signed)
        {
        }
    }

    public class WAttribute : TexelSemanticAttribute
    {
        public override int Channel
        {
            get { return 4; }
        }

        public WAttribute() : base() { }

        public WAttribute(int bits)
            : base(bits, ComponentMap.Signed)
        {
        }
    }

    #endregion

    public class ArrayLengthAttribute : Attribute
    {
        public ArrayLengthAttribute(int length)
        {
            this.Length = length;
        }

        public int Length { get; private set; }
    }
}
