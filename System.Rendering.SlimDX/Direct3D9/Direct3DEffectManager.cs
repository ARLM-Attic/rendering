using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Effects.Shaders;
using System.Compilers.Shaders.ShaderAST;
using System.Compilers.Generators;
using System.Compilers.Shaders.HLSL;
using System.Rendering.Effects;

using D3DEffect = global::SlimDX.Direct3D9.Effect;
using System.Maths;

namespace System.Rendering.Direct3D9
{
    public class Direct3DEffectManager : ShadingPipeline<D3DEffect>
    {
        public Direct3DEffectManager(Direct3DRender render)
            : base(render, new Dictionary<ShaderStage, ShaderCodeGenerator>
            {
                { ShaderStage.Vertex, new HLSLCodeGenerator(ShaderStage.Vertex, HLSLBasic.HLSL) },
                { ShaderStage.Pixel, new HLSLCodeGenerator(ShaderStage.Pixel, HLSLBasic.HLSL)}
            })
        {
        }

        protected override D3DEffect BuildEffect(IEnumerable<CompiledStage> stages)
        {
            var code = GetEffectCode(stages);
            string errors;
            var effect = D3DEffect.FromString(((Direct3DRender)Render).Device, code, null, null, null, global::SlimDX.Direct3D9.ShaderFlags.OptimizationLevel3, null, out errors);
            return effect;
        }

        private string GetCompilationInstruction(ShaderStage stage, string mainName)
        {
            switch (stage)
            {
                case ShaderStage.Vertex: return "VertexShader = compile vs_3_0 " + mainName + "();";
                case ShaderStage.Geometry: return "GeometryShader = compile gs_3_0 " + mainName + "();";
                case ShaderStage.Pixel: return "PixelShader = compile ps_3_0 " + mainName + "();";
            }
            throw new NotSupportedException();
        }

        private string GetEffectCode(IEnumerable<CompiledStage> stages)
        {
            string code = string.Join("\n", stages.Select(s => s.Code)) +
                          GetTechniqueCode(stages.Select(s => GetCompilationInstruction(s.Stage, s.Main)));
            Console.WriteLine(code);
            return code;
        }

        private string GetTechniqueCode(IEnumerable<string> instructions)
        {
            return string.Format(
@"technique Technique1
{{
  pass Pass1
  {{
{0}
  }}
}}
", string.Join("\n", instructions));
        }

        private void SetValue<T>(string fieldName, T value) where T:struct
        {
            try
            {
                var effectHandle = Effect.GetParameter(null, fieldName);
                Effect.SetValue(effectHandle, (T)value);
            }
            catch
            {
            }
        }

        protected override void SetValueOnEffect(string fieldName, object value)
        {
            if (value is bool)
            {
                SetValue (fieldName, (bool)value);
                return;
            }

            if (value is int)
            {
                SetValue(fieldName,(int)value);
                return;
            }

            if (value is float)
            {
                SetValue(fieldName, (float)value);
                return;
            }

            if (value is Vector2)
            {
                SetValue (fieldName, (Vector2)value);
                return;
            }

            if (value is Vector3)
            {
                SetValue(fieldName, (Vector3)value);
                return;
            }

            if (value is Vector4)
            {
                SetValue (fieldName, (Vector4)value);
                return;
            }

            if (value is Matrix4x4)
            {
                SetValue(fieldName, (Matrix4x4)value);
                return;
            }

            if (value is Matrix3x3)
            {
                SetValue(fieldName, (Matrix3x3)value);
                return;
            }

            GetType()
                .GetMethod("SetValue", Reflection.BindingFlags.NonPublic | Reflection.BindingFlags.Instance)
                .MakeGenericMethod(value.GetType())
                .Invoke(this, new object[] { fieldName, value });
        }

        HashSet<TextureBuffer> toDispose = new HashSet<TextureBuffer>();

        protected override void SetSamplerOnEffect(string fieldName, int index, ISampler sampler)
        {
            var d3dRender = (Direct3DRender)Render;

            if (sampler.Texture == null)
            {
                d3dRender.Device.SetTexture(index, null);
                return;
            }

            if (sampler.Texture.Render != this.Render)
            {
                var t = (TextureBuffer)sampler.Texture.Clone(this.Render);
                sampler = sampler.Clone(t);
                toDispose.Add(t);
            }

            var resourcesManager = (Direct3DRender.Direct3DResourcesManager)d3dRender.ResourcesManager;

            Direct3D9Tools.SetTextureSampler (d3dRender.Device, sampler, index);

            var man = resourcesManager.GetManagerFor<TextureBuffer>(sampler.Texture);
            
            global::SlimDX.Direct3D9.BaseTexture texture = null;

            if (man is Direct3DRender.Direct3DResourcesManager.TextureBufferResourceOnDeviceManager)
                texture = ((Direct3DRender.Direct3DResourcesManager.TextureBufferResourceOnDeviceManager)man).TextureBuffer;
            else
                texture = ((Direct3DRender.Direct3DResourcesManager.CubeTextureBufferResourceOnDeviceManager)man).TextureBuffer;

            d3dRender.Device.SetTexture (index, texture);
        }

        protected override void ClearSamplerOnEffect(string fieldName, int index, ISampler sampler)
        {
            var d3dRender = (Direct3DRender)Render;
            var resourcesManager = (Direct3DRender.Direct3DResourcesManager)d3dRender.ResourcesManager;

            d3dRender.Device.SetTexture(index, null);

            if (toDispose.Contains(sampler.Texture))
            {
                toDispose.Remove(sampler.Texture);
                sampler.Texture.Dispose();
            }
        }

        protected override void ApplyEffect()
        {
            
        }

        protected override void BeginEffect()
        {
            Effect.Begin();
            Effect.BeginPass(0);
        }

        protected override void UnapplyEffect()
        {
            Effect.EndPass();
            Effect.End();
        }
    }
}
