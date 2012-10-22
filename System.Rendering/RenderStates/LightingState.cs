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
    public struct LightingState
    {
        public static LightingState Default = new LightingState(false, Vectors.One * 0.2f, MaterialSource.Material, MaterialSource.Material, true);

        bool enable;
        bool localViewer;
        Vector4 ambient;
        
        MaterialSource diffuseMaterialSource;
        MaterialSource specularMaterialSource;

        LightSourceNode firstLight;
        class LightSourceNode
        {
            public LightSourceBase Current;
            public LightSourceNode Next;
        }

        public bool Enable { get { return enable; } }
        public Vector4 Ambient { get { return ambient; } }
        public MaterialSource DiffuseMaterialSource { get { return diffuseMaterialSource; } set { diffuseMaterialSource = value; } }
        public MaterialSource SpecularMaterialSource { get { return specularMaterialSource; } set { specularMaterialSource = value; } }
        public bool LocalViewer { get { return localViewer; } }

        public IEnumerable<LightSourceBase> Lights
        {
            get
            {
                var current = firstLight;
                while (current != null)
                {
                    yield return current.Current;
                    current = current.Next;
                }
            }
        }

        public LightingState(bool enable) : this (enable, new Vector4(0.2f,0.2f,0.2f,1), MaterialSource.Material, MaterialSource.Material, true)
        {
        }

        internal void Add(LightSourceBase light)
        {
            firstLight = new LightSourceNode { Current = light, Next = firstLight };
        }

        public LightingState(bool enable, Vector4 ambient, MaterialSource diffuseMaterialSource, MaterialSource specularMaterialSource, bool localViewer, params LightSourceBase[] lights)
        {
            this.enable = enable;
            this.ambient = ambient;
            this.diffuseMaterialSource = diffuseMaterialSource;
            this.specularMaterialSource = specularMaterialSource;
            this.localViewer = localViewer;
            this.firstLight = null;

            foreach (var l in lights)
                Add(l);
        }

        public static LightingState Enabled { get { return new LightingState(true); } }
        public static LightingState Disabled { get { return new LightingState(false); } }

        public LightingState Blend(LightingState previus, StateBlendMode mode)
        {
            if (mode == StateBlendMode.None)
                return this;
            else
            {
                LightingState l = this;
                l.firstLight = null;
                switch (mode)
                {
                    case StateBlendMode.Add:
                        foreach (LightSourceBase light in previus.Lights)
                            l.Add(light);
                        foreach (LightSourceBase light in this.Lights)
                            l.Add(light);
                        break;
                    case StateBlendMode.Replace:
                        foreach (LightSourceBase light in this.Lights)
                            l.Add(light);
                        break;
                    default:
                        throw new NotSupportedException();
                }
                
                return l;
            }
        }

        public LightingState Setting(bool enable, Vector4 ambient, MaterialSource diffuseMaterialSource, MaterialSource specularMaterialSource, bool localViewer)
        {
            LightingState _return = this;
            _return.enable = enable;
            _return.ambient = ambient;
            _return.diffuseMaterialSource = diffuseMaterialSource;
            _return.specularMaterialSource = specularMaterialSource;
            _return.localViewer = localViewer;
            _return.firstLight = firstLight;
            return _return;
        }

        public LightingState SwitchingOn()
        {
            LightingState ret = this;
            ret.enable = true;
            return ret;
        }
        public LightingState SwitchingOff()
        {
            LightingState ret = this;
            ret.enable = false;
            return ret;
        }
        public LightingState SettingAmbient(Vector4 ambient)
        {
            LightingState ret = this;
            ret.ambient = ambient;
            return ret;
        }
        public LightingState SettingDiffuseMaterialSource(MaterialSource source)
        {
            LightingState ret = this;
            ret.diffuseMaterialSource = source;
            return ret;
        }
    }

    public delegate void LightCollectionChange (ComponentModel.CollectionChangeAction action, LightSourceBase light);

    public enum MaterialSource { Material, VertexDiffuse, VertexSpecular }
}
