using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaEffect = Microsoft.Xna.Framework.Graphics.Effect;
using Microsoft.Xna.Framework.Graphics;
using System.Compilers.Shaders;
using System.Maths;
using System.Rendering.Effects;
using System.Rendering.RenderStates;
using System.Rendering.Resourcing;
using System.Compilers.Shaders.ShaderAST;
using System.Compilers.Generators;
using System.Compilers.Shaders.HLSL;
using System.Rendering.Effects.Shaders;
using XnaSamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;

namespace System.Rendering.Xna
{
    public class XnaEffectManager : ShadingPipeline<XnaEffect>
    {
        public XnaEffectManager(XnaRender render)
            : base(render, new Dictionary<ShaderStage, ShaderCodeGenerator>
            {
                { ShaderStage.Vertex, new HLSLCodeGenerator(ShaderStage.Vertex, HLSLBasic.HLSL) },
                { ShaderStage.Pixel, new HLSLCodeGenerator(ShaderStage.Pixel, HLSLBasic.HLSL)}
            })
        {
        }

        protected override XnaEffect BuildEffect(IEnumerable<CompiledStage> stages)
        {
            var effect = new XnaEffect(((XnaRender)Render).Device, XnaTools.GetEffectByteCode(GetEffectCode(stages)));
            return effect;
        }

        private string GetCompilationInstruction(ShaderStage stage, string mainName)
        {
            switch (stage)
            {
                case ShaderStage.Vertex: return "VertexShader = compile vs_2_0 " + mainName + "();";
                case ShaderStage.Geometry: return "GeometryShader = compile gs_2_0 " + mainName + "();";
                case ShaderStage.Pixel: return "PixelShader = compile ps_2_0 " + mainName + "();";
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

        private EffectParameter GetParameter(EffectParameterCollection parameters, string[] names, int index)
        {
            if (string.IsNullOrWhiteSpace(names[index]))
                return GetParameter(parameters, names, index + 1);

            EffectParameter e;
            if (char.IsDigit(names[index][0]))
                e = parameters[int.Parse(names[index])];
            else
                e = parameters[names[index]];
            if (index == names.Length - 1)
                return e;
            else
                return GetParameter(e.StructureMembers, names, index + 1);
        }

        protected override void SetValueOnEffect(string fieldName, object value)
        {
            try
            {
                EffectParameter parameter = GetParameter(Effect.Parameters, fieldName.Split('.', '[', ']'), 0);
                if (value is bool)
                {
                    parameter.SetValue((bool)value);
                    return;
                }

                if (value is int)
                {
                    parameter.SetValue((int)value);
                    return;
                }

                if (value is float)
                {
                    parameter.SetValue((float)value);
                    return;
                }

                if (value is Vector2)
                {
                    parameter.SetValue(XnaTools.ToXnaVector((Vector2)value));
                    return;
                }

                if (value is Vector3)
                {
                    parameter.SetValue(XnaTools.ToXnaVector((Vector3)value));
                    return;
                }

                if (value is Vector4)
                {
                    parameter.SetValue(XnaTools.ToXnaVector((Vector4)value));
                    return;
                }

                if (value is Matrix4x4)
                {
                    parameter.SetValue(XnaTools.ToXnaMatrix((Matrix4x4)value));
                    return;
                }
            }
            catch { }
        }

        protected override void SetSamplerOnEffect(string fieldName, int index, ISampler sampler)
        {
            var xnaRender = (XnaRender)Render;
            var resourcesManager = (XnaRender.XnaResourcesManager)xnaRender.ResourcesManager;

            xnaRender.Device.SamplerStates[index] = XnaTools.ToSamplerState(sampler);
            xnaRender.Device.Textures[index] = XnaTools.ToTexture(resourcesManager, sampler.Texture);
        }

        protected override void ClearSamplerOnEffect(string fieldName, int index, ISampler sampler)
        {
            var xnaRender = (XnaRender)Render;
            var resourcesManager = (XnaRender.XnaResourcesManager)xnaRender.ResourcesManager;

            xnaRender.Device.SamplerStates[index] = XnaSamplerState.LinearWrap;
            xnaRender.Device.Textures[index] = null;
        }

        protected override void ApplyEffect()
        {
        }

        protected override void BeginEffect()
        {
            Effect.CurrentTechnique.Passes[0].Apply();
        }

        protected override void UnapplyEffect()
        {
        }
    }

}
