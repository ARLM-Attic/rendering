/// Tutorial 3. Basic effects
/// This tutorial shows effects concepts in Rendering.
/// Effect objects are in charge of setting the rendering pipeline.
/// They will set some states to the RenderStatesManager object and sets that way the processing of a graphic.
/// They are divided in three groups. Those who changes aspects of global renderization, those who sets states and those who sets a pipeline behaviour (shaders).
/// First group set buffers operations, test functions and blending operations.
/// Second group depends of the current pipeline to work. That means, if we set a World transformation it will take effect only
/// if there is a part of the pipeline consuming this information to really transform the model.
/// Global-setter effects can be accessed by RasterOperations and Buffers classes.
/// States-setter effects can be quickly accessed by Materials, Transforms, Cameras and Lights classes.
/// Several Shader-setter effects can be accessed using Shaders class.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Rendering;
using System.Rendering.Services;
using System.Diagnostics;
using System.Maths;

namespace Tutorials.BasicEffects
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            renderedControl1.Render = new System.Rendering.OpenGL.OpenGLRender();
            
            stopwatch.Start();
        }

        /// <summary>
        /// Texture Buffer to use as texture of a model. There are several Buffer objects like VertexBuffer, IndexBuffer and CubeTextureBuffer.
        /// All these resources are treated same way... with options to get and set its data.
        /// </summary>
        TextureBuffer texture;
        TextureBuffer bump;

        IModel sampleModel;

        bool filling = true;

        bool phong = true;

        private void renderedControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F)
                filling = !filling;
            
            if (e.KeyCode == Keys.Enter)
                phong = !phong;

            if (e.KeyCode == Keys.Space)
                if (stopwatch.IsRunning)
                    stopwatch.Stop();
                else
                    stopwatch.Start();
        }

        private void renderedControl1_InitializeRender(object sender, System.Rendering.Forms.RenderEventArgs e)
        {
            var render = e.Render;

            /// Services are used to access the non-drawing functions of an API.
            /// All services are represented by a non-nullable type that manages certain functions. 
            /// i.e. generic type LoaderService allow to load some resource (textures, cubetextures, models, graphics) whenever the API were able to.
            /// Lines below are the same.
            // texture = render.Services.Get<LoaderService<TextureBuffer>>().Load("Penguins.jpg");
            // This is an extensor method.
            texture = render.Services.LoadTexture("Penguins.jpg").Allocate (render);
            bump = render.Services.LoadTexture("bump.png").Allocate (render);

            sampleModel = Models.Teapot.Allocate(render);
        }

        public IEnumerable<IEffect> TestEffects()
        {
            /// Materials class allows to quickly create material effects. This info will be used by pipeline to get diffuse, specular, opacity and surface data.
            /// Shinness, Glossy, Emissive, Plastic and Metallic are a fluent interface to set up a material adding more of those characteristics.
            yield return Materials.Yellow.Shinness.Shinness.Glossy.Glossy;

            /// Setting a texture using a shader. This shader effect will pick on current texture to get a diffuse color.
            /// If you specify SpecularMap, specular color will be pick instead.
            yield return Shaders.DiffuseMap(texture);

            /// Bump mapping uses a map to change the normals at surfaces. This effect will require a per-pixel lighting by using Phong, i.e.
            yield return Shaders.BumpMap(bump, 0.5f);

            /// Psicodelic effect using a function as a map. This function is converted to a shader so it will be resolved as a shader effect.
            // TODO: Casting double to float perform a shr operation not supported.
            float time = (float)stopwatch.Elapsed.TotalMilliseconds/1000f;
            yield return Shaders.DiffuseMap(v => new Vector4(GMath.sin(v.X * v.Y * 10 + time) * GMath.cos(v.Y * 4 + time/2), v.X * v.X, v.Y * v.Y, 1));

            /// Effect collections allows to treat a sequence of effects as just one.
            yield return new EffectCollection(Transforms.Scale(1, 0.5f, 1), Materials.White.Plastic.Shinness.Shinness.Glossy.Glossy);

            /// Some effects, like materials are blendable. That means, they can be blended with the existing one in pipeline.
            yield return new EffectCollection(Materials.Yellow.Shinness,
                new System.Rendering.Effects.Material
                {
                    /// Try other blend modes.
                    //BlendMode = System.Rendering.Effects.StateBlendMode.Replace,
                    BlendMode = System.Rendering.Effects.StateBlendMode.Multiply,
                    Diffuse = new Vector3 (1,0,1)
                });

            /// Other effects, like shaders and transforms are appendable. That means, they can be setted at the end or at the begining of existing effect in pipeline.
            yield return new EffectCollection(
                    Transforms.Translate(1, 0, 0),
                    /// Change AppendMode to Prepend to see what happend.
                    /// Appending a transformation performs a more-local transform and prepending a transformation performs a more-global transform.
                    new System.Rendering.Effects.Transforming(Matrices.RotateY(time)) { AppendMode = System.Rendering.Effects.AppendMode.Append }
                );

            /// AlphaBlends class allows to quickly access to blend operations effects.
            /// Note we are using a effect to draw two passes with the alpha effect. One first pass drawing the back of models, and a second one drawing the front.
            yield return new EffectCollection(RasterOptions.RenderBackToFront, AlphaBlends.Opacity(0.4f));
        }

        /// <summary>
        /// Stopwatch for the animation effect
        /// </summary>
        Stopwatch stopwatch = new Stopwatch();

        private void renderedControl1_Rendered(object sender, System.Rendering.Forms.RenderEventArgs e)
        {
            var render = e.Render;

            render.BeginScene();

            render.Draw(() =>
            {
                int count = 0;
                int N = 8;
                foreach (var effect in TestEffects())
                    render.Draw(sampleModel,
                        effect,
                        Transforms.Translate(4, 0, 0),
                        Transforms.Rotate(2 * GMath.Pi * (count++) / N, Axis.Y));
            },
                filling ? RasterOptions.ViewSolid : RasterOptions.ViewWireframe,
                Transforms.Rotate((float)stopwatch.Elapsed.TotalSeconds / 5, Axis.Y),
                Lights.Point(new Vector3(3, 5, 6), new Vector3(1, 1, 1)),
                Cameras.LookAt(new Vector3(3, 2, 10), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
                Cameras.Perspective(render.GetAspectRatio()),
                Buffers.Clear(0.2f, 0.2f, 0.4f, 1),
                Buffers.ClearDepth(),
                /// The programmable pipeline is set with default shaders to act as vertex process and pixel process that only pass values without no changes.
                /// To perform a vertex process you can use directly Shaders.VertexTransform effect.
                /// To perform lighting calculation you can use VertexLit or PixelLit effects. Anyway, there is two combinations for the pipeline accessible from
                /// Phong and Gouraud properties.
                phong ? Shaders.Phong : Shaders.Gouraud
                );

            render.EndScene();

            renderedControl1.Invalidate();
            
        }


    }
}
