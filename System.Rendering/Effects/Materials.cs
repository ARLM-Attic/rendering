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
using System.Rendering.RenderStates;
using System.Maths;

namespace System.Rendering.Effects
{
    public class Material : BlendableEffect<MaterialState>
    {
        public Material() : base(MaterialState.Default) { BlendMode = StateBlendMode.Replace; }

        public Material(MaterialState state) : base(state) { BlendMode = StateBlendMode.Replace; }

        public Material Transparent(float transparency)
        {
            var m = new Material(this.state);
            m.Opacity = 1 - transparency;
            return m;
        }

        public Vector3 Ambient
        {
            get { return state.Ambient;}
            set { state.Ambient = value; }
        }

        public Vector3 Diffuse
        {
            get { return state.Diffuse;}
            set { state.Diffuse = value; }
        }

        public Vector3 Specular
        {
            get { return state.Specular;}
            set { state.Specular = value; }
        }

        public Vector3 Emission
        {
            get { return state.Emission;}
            set { state.Emission = value; }
        }

        public float SpecularSharpness
        {
            get
            {
                return state.SpecularSharpness;
            }
            set { state.SpecularSharpness = value; }
        }

        public float Opacity { get { return state.Opacity; } set { state.Opacity = value; } }

        protected override MaterialState Blend(MaterialState previus)
        {
            return state.Blend (previus, BlendMode);
        }

        public static Material From(Vector3 ambient_and_diffuse)
        {
            Material mat = new Material() { BlendMode = StateBlendMode.Multiply };
            mat.Ambient = mat.Diffuse = ambient_and_diffuse;
            mat.Specular = new Vector3(0, 0, 0);
            mat.SpecularSharpness = 1;
            return mat;
        }

        public static Material From(Vector3 ambient, Vector3 diffuse, Vector3 specular, float specularSharpness)
        {
            return new Material(new MaterialState(
                ambient,
                diffuse,
                specular,
                specularSharpness,
                Vectors.O,
                1));
        }

        public static Material From(Vector3 ambient, Vector3 diffuse, Vector3 specular, float specularSharpness, float opacity, Vector3 emission)
        {
            return new Material(new MaterialState(
                ambient,
                diffuse,
                specular,
                specularSharpness,
                emission,
                opacity));
        }

        

        public Material Glossy
        {
            get
            {
                return Material.From(
                    this.Ambient, this.Diffuse, this.Specular + new Vector3(0.3f,0.3f,0.3f),
                    this.SpecularSharpness+10);
            }
        }

        public Material Shinness
        {
            get
            {
                return Material.From(this.Ambient, this.Diffuse, this.Specular,
                    this.SpecularSharpness + 20);
            }
        }

        public Material Metalic
        {
            get
            {
                return Material.From(
                    new Vector3(0, 0, 0), Diffuse * 0.6f, new Vector3(0.4f, 0.4f, 0.4f) + Specular,
                    SpecularSharpness + 10, Opacity, Emission);
            }
        }

        public Material Emissive
        {
            get
            {
                return new Material()
                {
                    Ambient = Diffuse*0.1f,
                    Diffuse = Diffuse*0.2f,
                    Emission = Diffuse*0.9f,
                    Specular = new Vector3 (1,1,1),
                    SpecularSharpness = 20
                };
            }
        }

        public Material Plastic
        {
            get
            {
                return Material.From(
                    this.Diffuse * 0.3f + Ambient, this.Diffuse, 0.2f * this.Diffuse + Specular,
                    SpecularSharpness + 5, Opacity, Emission);
            }
        }
    }
}
