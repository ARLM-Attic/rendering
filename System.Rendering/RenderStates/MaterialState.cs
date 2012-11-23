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
using System.Maths;
using System.Rendering.Effects;

namespace System.Rendering.RenderStates
{
    public struct MaterialState 
    {
        public static readonly MaterialState Default = new MaterialState(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1f, 1f, 1f), new Vector3(0, 0, 0), 1, new Vector3(0, 0, 0), 1);

        public MaterialState(Vector3 ambient, Vector3 diffuse, Vector3 specular, float specularSharpness, Vector3 emissive, float opacity)
        {
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
            this.emission = emissive;
            this.specularSharpness = specularSharpness;
            this.opacity = opacity;
        }

        Vector3 BlendVector(Vector3 v1, Vector3 v2, StateBlendMode mode)
        {
            switch (mode)
            {
                case StateBlendMode.Add: return v1 + v2;
                case StateBlendMode.Modulate: return (v1 + v2) * 0.5f;
                case StateBlendMode.ModulateX4: return (v1 + v2) * 2;
                case StateBlendMode.Multiply: return v1 * v2;
                case StateBlendMode.None: return v1;
                case StateBlendMode.Replace: return v2;
                case StateBlendMode.RevSubtract: return v2 - v1;
                case StateBlendMode.Subtract: return v1 - v2;
                default: return v1;
            }
        }
        float BlendVector(float v1, float v2, StateBlendMode mode)
        {
            switch (mode)
            {
                case StateBlendMode.Add: return v1 + v2;
                case StateBlendMode.Modulate: return (v1 + v2) * 0.5f;
                case StateBlendMode.ModulateX4: return (v1 + v2) * 2;
                case StateBlendMode.Multiply: return v1 * v2;
                case StateBlendMode.None: return v1;
                case StateBlendMode.Replace: return v2;
                case StateBlendMode.RevSubtract: return v2 - v1;
                case StateBlendMode.Subtract: return v1 - v2;
                default: return v1;
            }
        }

        public MaterialState Blend(MaterialState previus, StateBlendMode mode)
        {
            MaterialState m = this;
            if (mode == StateBlendMode.Replace)
                return m;

            m.Ambient = BlendVector(previus.Ambient, this.Ambient, mode);
            m.Diffuse = BlendVector(previus.Diffuse, this.Diffuse, mode);
            m.Emission = BlendVector(previus.Emission, this.Emission, StateBlendMode.Replace);
            m.Specular = BlendVector(previus.Specular, this.Specular, StateBlendMode.Replace);

            m.SpecularSharpness = this.SpecularSharpness;
            m.Opacity = BlendVector(new Vector3(previus.Opacity, 0, 0), new Vector3(this.Opacity, 0, 0), mode).X;
            return m;
        }

        Vector3 ambient, diffuse, specular, emission;
        float specularSharpness;
        float opacity;

        public float SpecularSharpness
        {
            get { return this.specularSharpness; }
            set { this.specularSharpness = value; }
        }

        public float Opacity
        {
            get { return this.opacity; }
            set { this.opacity = value; }
        }

        public Vector3 Ambient
        {
            get { return ambient; }
            set { ambient = value; }
        }

        public Vector3 Diffuse
        {
            get { return diffuse; }
            set { diffuse = value; }
        }

        public Vector3 Specular
        {
            get { return specular; }
            set { specular = value; }
        }

        public Vector3 Emission
        {
            get { return emission; }
            set { emission = value; }
        }
    }
}
