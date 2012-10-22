using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Rendering.Effects.Shaders
{
    public static class ShaderSemantics
    {
        private static Dictionary<SemanticAttribute, Semantic> resolvedSemantics = new Dictionary<SemanticAttribute, Semantic>();

        static ShaderSemantics()
        {
        }

        public static Semantic Resolve(ICustomAttributeProvider member)
        {
            SemanticAttribute[] semantics = (SemanticAttribute[])member.GetCustomAttributes(typeof(SemanticAttribute), true);
            if (semantics.Length == 0)
                return null;
            SemanticAttribute sa = semantics[0];

            if (resolvedSemantics.ContainsKey(sa))
                return resolvedSemantics[sa];

            object[] objs = sa.GetType().GetCustomAttributes(typeof(CompileSemanticAsAttribute), true);

            if (objs.Length > 0)
            {
                CompileSemanticAsAttribute compiling = objs[0] as CompileSemanticAsAttribute;

                Semantic result = Activating.CreateInstanceOfSubclass<Semantic>(compiling.SemanticType);

                if (result is IndexedSemantic && sa is IndexedSemanticAttribute)
                    ((IndexedSemantic)result).Index = ((IndexedSemanticAttribute)sa).Index;
                if (result is SamplerSemantic && sa is SamplerAttribute)
                    ((SamplerSemantic)result).Register = ((SamplerAttribute)sa).Register;
                if (result is CustomSemantic && sa is CustomAttribute)
                    ((CustomSemantic)result).Label = ((CustomAttribute)sa).Label;

                return result;
            }
            return null;
        }

        public static Semantic Custom(string Label)
        {
            return new CustomSemantic(Label);
        }
        public static PositionSemantic Position(int index)
        {
            return new PositionSemantic() { Index = index };
        }
        public static NormalSemantic Normal(int index)
        {
            return new NormalSemantic() { Index = index };
        }
        public static TextureCoordinatesSemantic TextureCoordinates(int index)
        {
            return new TextureCoordinatesSemantic() { Index = index };
        }
        public static DiffuseSemantic Diffuse()
        {
            return new DiffuseSemantic() ;
        }
        public static SpecularSemantic Specular()
        {
            return new SpecularSemantic();
        }
        public static DepthSemantic Depth()
        {
            return new DepthSemantic();
        }
        public static FogSemantic Fog()
        {
            return new FogSemantic();
        }
    }

    #region Semantics

    [AttributeUsage(AttributeTargets.Class)]
    public class CompileSemanticAsAttribute : Attribute
    {
        public Type SemanticType { get; set; }
    }

    public class Semantic
    {
        public override bool Equals(object obj)
        {
            return this.GetType().Equals(obj.GetType());
        }
        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
    }

    public class IndexedSemantic : Semantic
    {
        public int Index { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && (Index == ((IndexedSemantic)obj).Index);
        }
        public override int GetHashCode()
        {
            return Index;
        }
        public override string ToString()
        {
            return GetType().Name.Replace("Semantic", "");
        }
    }

    public class PositionSemantic : IndexedSemantic
    {
    }

    public class ProjectedSemantic : IndexedSemantic
    {
    }

    public class NormalSemantic : IndexedSemantic
    {
    }

    public class TextureCoordinatesSemantic : IndexedSemantic
    {
    }

    public class DiffuseSemantic : Semantic
    {
    }

    public class SpecularSemantic : Semantic
    {
    }

    public class DepthSemantic : Semantic
    {
    }

    public class SamplerSemantic : GlobalSemantic
    {
        public int Register { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && (Register == ((SamplerSemantic)obj).Register);
        }
        public override int GetHashCode()
        {
            return Register;
        }
    }

    public class FogSemantic : Semantic
    {
    }

    public class GlobalSemantic : Semantic
    {
    }

    public class CustomSemantic : GlobalSemantic
    {
        public CustomSemantic(string Label)
        {
            this.Label = Label;
        }

        public string Label { get; internal set; }

        public override bool Equals(object obj)
        {
            return this.Label.Equals(((CustomSemantic)obj).Label);
        }

        public override int GetHashCode()
        {
            return Label.GetHashCode();
        }

        public override string ToString()
        {
            return Label;
        }
    }

    #endregion
}
