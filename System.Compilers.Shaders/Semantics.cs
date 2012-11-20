using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Maths;

namespace System.Compilers.Shaders
{
    #region Semantics

    public class OnlyGlobalDeclarationAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CompileSemanticAsAttribute : Attribute
    {
        public Type SemanticType { get; set; }
    }

    public abstract class Semantic
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return this.GetType().Equals(obj.GetType());
        }
        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
    }

    public abstract class DataSemantic : Semantic
    {
        public virtual float GetDefaultValue() { return 0f; }

        public override string ToString()
        {
            return GetType().Name.Replace("Semantic", "");
        }
    }

    public class IndexedSemantic : DataSemantic
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
            return base.ToString()+Index;
        }
    }

    public class PositionSemantic : IndexedSemantic
    {
    }

    public class ProjectedSemantic : DataSemantic
    {
    }

    public class NormalSemantic : IndexedSemantic
    {
        public override float GetDefaultValue()
        {
            return 1;
        }
    }

    public class CoordinatesSemantic : IndexedSemantic
    {
    }

    public class WeightSemantic : IndexedSemantic
    {
    }

    public class IndexSemantic : IndexedSemantic
    {
    }

    public class ColorSemantic : IndexedSemantic
    {
        public override float GetDefaultValue()
        {
            return 1f;
        }
    }

    public class DepthSemantic : DataSemantic
    {
    }

    public class FogSemantic : DataSemantic
    {
    }

    public class GlobalSemantic : Semantic
    {
    }

    public class SamplerSemantic : GlobalSemantic
    {
    }

    #endregion

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

            return Resolve(sa);
        }

        public static Semantic Resolve(SemanticAttribute sa)
        {
            if (resolvedSemantics.ContainsKey(sa))
                return resolvedSemantics[sa];

            object[] objs = sa.GetType().GetCustomAttributes(typeof(CompileSemanticAsAttribute), true);

            if (objs.Length > 0)
            {
                CompileSemanticAsAttribute compiling = objs[0] as CompileSemanticAsAttribute;

                Semantic result = (Semantic)Activator.CreateInstance(compiling.SemanticType);

                if (result is IndexedSemantic && sa is IndexedComponentAttribute)
                    ((IndexedSemantic)result).Index = ((IndexedComponentAttribute)sa).Index;

                return result;
            }
            return null;
        }

        public static PositionSemantic Position(int index)
        {
            return new PositionSemantic() { Index = index };
        }

        public static ProjectedSemantic Projected()
        {
            return new ProjectedSemantic();
        }

        public static NormalSemantic Normal(int index)
        {
            return new NormalSemantic() { Index = index };
        }
        public static CoordinatesSemantic Coordinates(int index)
        {
            return new CoordinatesSemantic() { Index = index };
        }
        public static ColorSemantic Color(int index)
        {
            return new ColorSemantic { Index = index };
        }
        public static DepthSemantic Depth()
        {
            return new DepthSemantic();
        }
        public static FogSemantic Fog()
        {
            return new FogSemantic();
        }

        public static SamplerSemantic Sampler()
        {
            return new SamplerSemantic();
        }
    }
}
