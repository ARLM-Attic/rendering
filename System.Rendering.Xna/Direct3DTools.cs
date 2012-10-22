using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Maths;
using Vector2 = System.Maths.Vector2;
using Vector3 = System.Maths.Vector3;
using Vector4 = System.Maths.Vector4;
using System.Rendering.RenderStates;
using Microsoft.Xna.Framework.Graphics;
using System.Rendering.Modelling;
using System.Rendering.Resourcing;
using System.Reflection;

namespace System.Rendering.Xna
{
  internal static class Direct3DTools
  {
    public static Color ToXnaColor(Vector4 v)
    {
      return new Color(ToXnaVector(v));
    }

    public static Microsoft.Xna.Framework.Vector2 ToXnaVector(Vector2 v)
    {
      return new Microsoft.Xna.Framework.Vector2(v.X, v.Y);
    }

    public static Microsoft.Xna.Framework.Vector3 ToXnaVector(Vector3 v)
    {
      return new Microsoft.Xna.Framework.Vector3(v.X, v.Y, v.Z);
    }

    public static Microsoft.Xna.Framework.Vector4 ToXnaVector(Vector4 v)
    {
      return new Microsoft.Xna.Framework.Vector4(v.X, v.Y, v.Z, v.W);
    }

    public static Microsoft.Xna.Framework.Graphics.CompareFunction ToCompareFunction(Compare compare)
    {
      switch (compare)
      {
        case Compare.Always: return Microsoft.Xna.Framework.Graphics.CompareFunction.Always;
        case Compare.Never: return Microsoft.Xna.Framework.Graphics.CompareFunction.Never;
        case Compare.GreaterEqual: return Microsoft.Xna.Framework.Graphics.CompareFunction.GreaterEqual;
        case Compare.NotEqual: return Microsoft.Xna.Framework.Graphics.CompareFunction.NotEqual;
        case Compare.Greater: return Microsoft.Xna.Framework.Graphics.CompareFunction.Greater;
        case Compare.LessEqual: return Microsoft.Xna.Framework.Graphics.CompareFunction.LessEqual;
        case Compare.Equal: return Microsoft.Xna.Framework.Graphics.CompareFunction.Equal;
        case Compare.Less: return Microsoft.Xna.Framework.Graphics.CompareFunction.Less;
        default:
          return Microsoft.Xna.Framework.Graphics.CompareFunction.Less;
      }
    }

    public static Microsoft.Xna.Framework.Graphics.StencilOperation ToStencilOperation(StencilOp stencilOp)
    {
      switch (stencilOp)
      {
        case StencilOp.Zero: return Microsoft.Xna.Framework.Graphics.StencilOperation.Zero;
        case StencilOp.Keep: return Microsoft.Xna.Framework.Graphics.StencilOperation.Keep;
        case StencilOp.Replace: return Microsoft.Xna.Framework.Graphics.StencilOperation.Replace;
        case StencilOp.Increment: return Microsoft.Xna.Framework.Graphics.StencilOperation.Increment;
        case StencilOp.Decrement: return Microsoft.Xna.Framework.Graphics.StencilOperation.Decrement;
        case StencilOp.Invert: return Microsoft.Xna.Framework.Graphics.StencilOperation.Invert;
        default:
          return Microsoft.Xna.Framework.Graphics.StencilOperation.Keep;
      }
    }

    public static Microsoft.Xna.Framework.Graphics.CullMode ToXnaCullMode(CullModeState value)
    {
      switch (value)
      {
        case CullModeState.None: return Microsoft.Xna.Framework.Graphics.CullMode.None;
        case CullModeState.Front: return Microsoft.Xna.Framework.Graphics.CullMode.CullCounterClockwiseFace;
        case CullModeState.Back: return Microsoft.Xna.Framework.Graphics.CullMode.CullClockwiseFace;
        case CullModeState.Front_and_Back: return Microsoft.Xna.Framework.Graphics.CullMode.CullClockwiseFace | Microsoft.Xna.Framework.Graphics.CullMode.CullCounterClockwiseFace;
        default:
          return Microsoft.Xna.Framework.Graphics.CullMode.None;
      }
    }

    public static Microsoft.Xna.Framework.Graphics.FillMode ToXnaFillMode(FillModeState value)
    {
      switch (value)
      {
        case FillModeState.Fill: return Microsoft.Xna.Framework.Graphics.FillMode.Solid;
        case FillModeState.Lines: return Microsoft.Xna.Framework.Graphics.FillMode.WireFrame;
        default:
          return Microsoft.Xna.Framework.Graphics.FillMode.Solid;
      }
    }

    public static Matrix ToXnaMatrix(Matrix4x4 m)
    {
      return new Matrix(
        m.M00, m.M01, m.M02, m.M03,
        m.M10, m.M11, m.M12, m.M13,
        m.M20, m.M21, m.M22, m.M23,
        m.M30, m.M31, m.M32, m.M33
      );
    }

    public static Microsoft.Xna.Framework.Graphics.Blend ToXnaBlend(System.Rendering.RenderStates.Blend blend)
    {
      switch (blend)
      {
        case System.Rendering.RenderStates.Blend.Zero: return Microsoft.Xna.Framework.Graphics.Blend.Zero;
        case System.Rendering.RenderStates.Blend.One: return Microsoft.Xna.Framework.Graphics.Blend.One;
        case System.Rendering.RenderStates.Blend.SourceColor: return Microsoft.Xna.Framework.Graphics.Blend.SourceColor;
        case System.Rendering.RenderStates.Blend.InvSourceColor: return Microsoft.Xna.Framework.Graphics.Blend.InverseSourceColor;
        case System.Rendering.RenderStates.Blend.SourceAlpha: return Microsoft.Xna.Framework.Graphics.Blend.SourceAlpha;
        case System.Rendering.RenderStates.Blend.InvSourceAlpha: return Microsoft.Xna.Framework.Graphics.Blend.InverseSourceAlpha;
        case System.Rendering.RenderStates.Blend.DestinationAlpha: return Microsoft.Xna.Framework.Graphics.Blend.DestinationAlpha;
        case System.Rendering.RenderStates.Blend.InvDestinationAlpha: return Microsoft.Xna.Framework.Graphics.Blend.InverseDestinationAlpha;
        case System.Rendering.RenderStates.Blend.DestinationColor: return Microsoft.Xna.Framework.Graphics.Blend.DestinationColor;
        case System.Rendering.RenderStates.Blend.InvDestinationColor: return Microsoft.Xna.Framework.Graphics.Blend.InverseDestinationColor;
        case System.Rendering.RenderStates.Blend.SourceAlphaSat: return Microsoft.Xna.Framework.Graphics.Blend.SourceAlphaSaturation;
        default:
          return Microsoft.Xna.Framework.Graphics.Blend.One;
      }
    }

    public static PrimitiveType ToXnaPrimitiveType(BasicPrimitiveType primitiveType)
    {
      switch (primitiveType)
      {
        case BasicPrimitiveType.Lines: return PrimitiveType.LineList;
        case BasicPrimitiveType.Line_Strip: return PrimitiveType.LineStrip;
        case BasicPrimitiveType.Triangles: return PrimitiveType.TriangleList;
        case BasicPrimitiveType.Triangle_Strip: return PrimitiveType.TriangleStrip;
        default:
          throw new NotSupportedException("This primitive type isn't supported: " + primitiveType);
      }
    }

    private static Dictionary<Type, VertexDeclaration> vertexDecCache = new Dictionary<Type, VertexDeclaration>();
    public static VertexDeclaration GetVertexDeclaration(Type elementType)
    {
      VertexDeclaration dec;
      if (!vertexDecCache.TryGetValue(elementType, out dec))
      {
        var descriptor = ElementExtensors.GetDescriptor(elementType);

        List<VertexElement> elements = new List<VertexElement>();
        foreach (var d in descriptor.Declaration)
        {
          elements.Add(new VertexElement()
          {
            Offset = d.Offset,
            UsageIndex = GetUsageIndex(d.Semantic),
            VertexElementUsage = GetVertexUsage(d.Semantic),
            VertexElementFormat = GetVertexFormat(d)
          });
        }
        dec = new VertexDeclaration(elements.ToArray());
        vertexDecCache.Add(elementType, dec);
      }
      return dec;
    }

    private static int GetUsageIndex(SemanticAttribute semantic)
    {
      return semantic is IndexedSemanticAttribute ? ((IndexedSemanticAttribute)semantic).Index : 0;
    }

    private static VertexElementUsage GetVertexUsage(SemanticAttribute semantic)
    {
      if (semantic is PositionAttribute || semantic is ProjectedAttribute)
        return VertexElementUsage.Position;
      if (semantic is ColorAttribute)
        return VertexElementUsage.Color;
      if (semantic is NormalAttribute)
        return VertexElementUsage.Normal;
      if (semantic is TextureCoordinatesAttribute)
        return VertexElementUsage.TextureCoordinate;

      throw new NotSupportedException("Usage " + semantic + " is not supported");
    }

    private static VertexElementFormat GetVertexFormat(ElementField field)
    {
      switch (field.Components)
      {
        case 1:
          switch (field.BasicType)
          {
            case TypeCode.UInt32: 
            case TypeCode.Int32: 
              return VertexElementFormat.Color;
          }
          break;
        case 2:
          switch (field.BasicType)
          {
            case TypeCode.Int16: return VertexElementFormat.Short2;
            case TypeCode.Single: return VertexElementFormat.Vector2;
          }
          break;
        case 3:
          switch (field.BasicType)
          {
            case TypeCode.Single:
              return VertexElementFormat.Vector3;
            case TypeCode.Int32:
              return VertexElementFormat.NormalizedShort4;
          }
          break;
        case 4:
          switch (field.BasicType)
	        {
            case TypeCode.Byte:
              return VertexElementFormat.Byte4;
            case TypeCode.Int16:
              return VertexElementFormat.Short4;
            case TypeCode.Int32:
              return VertexElementFormat.Color;
            case TypeCode.Single:
              return VertexElementFormat.Vector4;
	        }
          break;
      }
      throw new NotSupportedException("Specific type of vertex element is not supported " + field.BasicType + " " + field.Components);
    }

    public static int PrimitiveCount(int count, BasicPrimitiveType primitiveType)
    {
      switch (primitiveType)
      {
        case BasicPrimitiveType.Lines: return count /2;
        case BasicPrimitiveType.Line_Strip: return count-1;
        case BasicPrimitiveType.Triangles: return count / 3;
        case BasicPrimitiveType.Triangle_Strip: return count - 2;
        default:
          throw new NotSupportedException("This primitive type isn't supported: " + primitiveType);
      }
    }

    public static XnaState Clone<XnaState>(this XnaState state) where XnaState : new()
    {
      var properties = typeof(XnaState).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite);

      var newState = new XnaState();
      foreach (var prop in properties)
        prop.SetValue(newState, prop.GetValue(state, null), null);

      return newState;
    }
  }
}
