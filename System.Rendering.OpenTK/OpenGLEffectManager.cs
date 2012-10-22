using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Effects.Shaders;
using System.Rendering.OpenGL;
using System.Compilers.Shaders.GLSL;
using System.Compilers.Generators;
using System.Maths;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using System.Compilers.Shaders.ShaderAST;

namespace System.Rendering.OpenTK
{
    public class OpenGLEffectManager : ShadingPipeline<OpenGLEffect>
    {
        public OpenGLEffectManager(OpenGLRender render)
            : base(render, new Dictionary<ShaderStage, ShaderCodeGenerator>
            {
                { ShaderStage.Vertex, new GLSL400CodeGenerator (ShaderStage.Vertex, GLSLBasic.GLSL ) },
                { ShaderStage.Pixel , new GLSL400CodeGenerator (ShaderStage.Pixel, GLSLBasic.GLSL ) }
            })
        {
        }

        protected override OpenGLEffect BuildEffect(IEnumerable<CompiledStage> stages)
        {
            OpenGLEffect effect = new OpenGLEffect();

            foreach (var stage in stages)
                effect.AddShader(stage.Stage, stage.Code);
            
            effect.Link();

            return effect;
        }

        protected override void SetValueOnEffect(string fieldName, object value)
        {
            Effect.SetValue(fieldName, value);
        }

        HashSet<TextureBuffer> toDispose = new HashSet<TextureBuffer>();

        protected override void SetSamplerOnEffect(string fieldName, int index, ISampler sampler)
        {
            if (sampler.Texture == null) // no texture
            {
                GL.ActiveTexture(TextureUnit.Texture0 + index);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                return;
            }

            if (sampler.Texture.Render != this.Render)
            {
                var t = (TextureBuffer)sampler.Texture.Clone(this.Render);
                toDispose.Add(t);
                sampler = sampler.Clone(t);
            }

            var openglRender = (OpenGLRender)Render;
            var resourcesManager = (OpenGLRender.OpenGLResourcesManager)openglRender.ResourcesManager;

            GL.ActiveTexture(TextureUnit.Texture0 + index);

            OpenGLTextureBuffer texture = ((OpenGLRender.OpenGLResourcesManager.TextureBufferResourceOnDeviceManager)resourcesManager.GetManagerFor(sampler.Texture)).TextureBuffer;
            texture.Bind();

            OpenGLTools.SetTextureSampler(sampler, index);

            Effect.SetSampler(fieldName, sampler, index);
        }

        protected override void ClearSamplerOnEffect(string fieldName, int index, ISampler sampler)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + index);
            GL.BindTexture(TextureTarget.Texture1D, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindTexture(TextureTarget.Texture3D, 0);
            GL.BindTexture(TextureTarget.TextureCubeMap, 0);

            if (toDispose.Contains(sampler.Texture))
            {
                toDispose.Remove(sampler.Texture);
                sampler.Texture.Dispose();
            }
        }

        protected override void ApplyEffect()
        {
            Effect.Use();
        }

        protected override void BeginEffect()
        {
        }

        protected override void UnapplyEffect()
        {
            GL.UseProgram(0);
        }
    }

    public sealed class OpenGLEffect
    {
        public int ProgramID;

        public int VertexShaderID;

        public int PixelShaderID;

        private Dictionary<string, int> uniforms = new Dictionary<string, int>();

        public void AddShader(ShaderStage stage, string code)
        {
            var shaderType = (ShaderType)0;

            switch (stage){
                case ShaderStage.Vertex: shaderType = ShaderType.VertexShader; break;
                case ShaderStage.Pixel: shaderType = ShaderType.FragmentShader; break;
                case ShaderStage.Geometry: shaderType = ShaderType.GeometryShader; break;
            }

            var shader = GL.CreateShader(shaderType);
            GL.ShaderSource(shader, code);
            GL.CompileShader(shader);

            GL.AttachShader(ProgramID, shader);

            Console.WriteLine("// Shader for " + stage);
            Console.WriteLine(code);
            Console.WriteLine();

            string errors;
            GL.GetShaderInfoLog(shader, out errors);

            if (!string.IsNullOrEmpty(errors))
                Console.WriteLine("// "+stage+" Error: " + errors);
        }

        public OpenGLEffect()
        {
            ProgramID = GL.CreateProgram();
        }

        public void Link()
        {
            GL.LinkProgram(ProgramID);

            string errors;
            GL.GetProgramInfoLog(ProgramID, out errors);

            if (!string.IsNullOrEmpty(errors))
                Console.WriteLine("// Program Error: " + errors);
        }

        public void SetSampler(string field, ISampler sampler, int index)
        {
            if (!uniforms.ContainsKey(field))
                uniforms[field] = GL.GetUniformLocation(ProgramID, field);

            int location = uniforms[field];

            GL.Uniform1(location, index);
        }

        public void SetValue(string field, object value)
        {
            if (!uniforms.ContainsKey(field))
                uniforms[field] = GL.GetUniformLocation(ProgramID, field);

            int location = uniforms[field];

            if (value is bool)
            {
                GL.Uniform1(location, Convert.ToInt32(value));
                return;
            }

            if (value is int)
            {
                GL.Uniform1(location, (int)value);
                return;
            }

            if (value is float)
            {
                GL.Uniform1(location, (float)value);
                return;
            }

            if (value is Vector1)
            {
                Vector1 v = (Vector1)value;
                GL.Uniform1(location, (float)v.X);
                return;
            }

            if (value is Vector3)
            {
                Vector3 v = (Vector3)value;
                GL.Uniform3(location, 1, ref v.X);
                return;
            }
            if (value is Vector2)
            {
                Vector2 v = (Vector2)value;
                GL.Uniform2(location, 1, ref v.X);
                return;
            }
            if (value is Vector4)
            {
                Vector4 v = (Vector4)value;
                GL.Uniform4(location, 1, ref v.X);
                return;
            }

            if (value is Matrix4x4)
            {
                Matrix4x4 mat = (Matrix4x4)value;
                GL.UniformMatrix4(location, 1, true, ref mat.M00);
                return;
            }

            throw new ArgumentException();
        }

        public void Use()
        {
            GL.UseProgram(ProgramID);
        }
    }
}
