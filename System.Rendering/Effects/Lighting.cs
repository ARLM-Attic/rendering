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
using System.ComponentModel.Design.Serialization;
using System.ComponentModel;
using System.Rendering.RenderStates;
using System.Maths;

namespace System.Rendering.Effects
{
    public class Lighting : BlendableEffect<LightingState>
    {
        public static Lighting Enabled { get { return new Lighting(); } }

        public static Lighting Disabled { get { return new Lighting() { Enable = false }; } }

        public Lighting() : base(LightingState.Enabled) {
        }

        public static Lighting PointLight(Vector3 position, Vector3 color)
        {
            Lighting l = new Lighting();
            l.BlendMode = StateBlendMode.Add;
            PointLightSource light = new PointLightSource()
            {
                Range = 1000,
                Position = position,
                Ambient = new Vector3 (0,0,0),
                Diffuse = color,
                Specular = new Vector3 (1, 1, 1) 
            };
            l.Add(light);
            return l;
        }

        public static Lighting DirectionalLight(Vector3 direction, Vector3 color)
        {
            Lighting l = new Lighting();
            l.BlendMode = StateBlendMode.Add;
            DirectionalLightSource light = new DirectionalLightSource()
            {
                Direction = direction,
                Ambient = new Vector3(0.0f, 0.0f, 0.0f),
                Diffuse = color,
                Specular = new Vector3(1.0f, 1.0f, 1.0f),
            };
            l.Add(light);
            return l;
        }

        public static Lighting AmbientLight(Vector3 ambient)
        {
            Lighting l = new Lighting();
            l.BlendMode = StateBlendMode.Add;
            AmbientLightSource light = new AmbientLightSource()
            {
                Ambient = ambient
            };
            l.Add(light);
            return l;
        }

        protected override LightingState Blend(LightingState previus)
        {
            return state.Blend(previus, BlendMode);
        }

        public Vector4 Ambient
        {
            get { return state.Ambient; }
            set { this.state = state.SettingAmbient(value); }
        }

        public void Add(LightSourceBase light)
        {
            state.Add(light);
        }

        public IEnumerable<LightSourceBase> Lights
        {
            get { return state.Lights; }
        }

        public MaterialSource DiffuseMaterialSource
        {
            get
            {
                return state.DiffuseMaterialSource;
            }
            set { state.DiffuseMaterialSource = value; }
        }

        public MaterialSource SpecularMaterialSource
        {
            get
            {
                return state.SpecularMaterialSource;
            }
            set { state.SpecularMaterialSource = value; }
        }

        public bool Enable
        {
            get { return state.Enable; }
            set { state = (value) ? state.SwitchingOn() : state.SwitchingOff(); }
        }
    }
}
