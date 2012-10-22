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
using System.Text;
using System.Rendering.Effects;
using System.Maths;

namespace System.Rendering.Effects
{
    public class SpotLightSource : PointBasedLightSource
    {
        float innerConeAngle, outerConeAngle, falloff;

        Vector3 direction = (Vector3)Vectors.Front;
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public float InnerConeAngle
        {
            get { return innerConeAngle; }
            set { innerConeAngle = value; }
        }
        public float OuterConeAngle
        {
            get { return outerConeAngle; }
            set { outerConeAngle = value; }
        }
        public float Falloff
        {
            get { return falloff; }
            set { falloff = value; }
        }

        public override LightType Type
        {
            get { return LightType.SpotLight; }
        }
    }

    public class PointLightSource : PointBasedLightSource
    {
        public override LightType Type
        {
            get { return LightType.PointSource; }
        }
    }

    public class DirectionalLightSource : LightSourceBase
    {
        public override LightType Type
        {
            get { return LightType.DirectionalLight; }
        }

        Vector3 direction = (Vector3)Vectors.Front;
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        Vector3 diffuse;
        public Vector3 Diffuse
        {
            get { return diffuse; }
            set { diffuse = value; }
        }

        Vector3 specular;
        public Vector3 Specular
        {
            get { return specular; }
            set { specular = value; }
        }        
    }

    public abstract class PointBasedLightSource : LightSourceBase
    {
        Vector3 position = (Vector3)Vectors.O;
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        FLOATINGTYPE range = 100;
        public FLOATINGTYPE Range
        {
            get { return range; }
            set { range = value; }
        }

        Vector3 diffuse;
        public Vector3 Diffuse
        {
            get { return diffuse; }
            set { diffuse = value; }
        }

        Vector3 specular;
        public Vector3 Specular
        {
            get { return specular; }
            set { specular = value; }
        }
        
        float const_at = 1, linear_at = 0, quadratic_at = 0;
        public float Constant_Attenuation
        {
            get { return const_at; }
            set { const_at = value; }
        }
        public float Linear_Attenuation
        {
            get { return linear_at; }
            set { linear_at = value; }
        }
        public float Quadratic_Attenuation
        {
            get { return quadratic_at; }
            set { quadratic_at = value; }
        }
    }

    public class AmbientLightSource : LightSourceBase
    {
        public override LightType Type
        {
            get { return LightType.Ambient; }
        }
    }

    public abstract class LightSourceBase
    {
        public abstract LightType Type { get; }

        Vector3 ambient = new Vector3(0.2f, 0.2f, 0.2f);
        
        public Vector3 Ambient
        {
            get { return ambient; }
            set { ambient = value; }
        }
    }

    /// <summary>
    /// Defines supported values for a light type.
    /// </summary>
    public enum LightType 
    { 
        /// <summary>
        /// Source of light is a point and spread in all directions
        /// </summary>
        PointSource, 
        /// <summary>
        /// Source of light is in some direction from infinity
        /// </summary>
        DirectionalLight, 
        /// <summary>
        /// Source of light is a point and spread in a cone of directions
        /// </summary>
        SpotLight,
        /// <summary>
        /// Source of light is in all places and affects all surface equally
        /// </summary>
        Ambient
    }
}
