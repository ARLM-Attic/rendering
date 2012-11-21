/// Tutorial 5. Shader effects
/// This tutorial shows creating shader effects objects in Rendering.
/// A shader effect set a program (shader) in some stage of the pipeline (Vertex or Pixel).
/// The program can be expressed as a .NET function that receives a struct type (Input data) and returns another struct type (Output data).
/// To configure a program to work per-vertex you can create a VertexShaderEffect object, and a PixelShaderEffect object is you want a per-pixel processing.
/// Shaders in rendering can be blended. That means, if there are a shader set in the pipeline and you want to set up another... this shader can be set
/// at the end (shader receives transformed data and output the final data), at the begining (shader receives the input data and its output is passed to the existing one)
/// or replace (shader replace the existing behaviour). Those operations conform the new shader to be set. Many times vertex shaders are prepended and pixel shaders are appended.
/// This feature of shaders in Rendering allows programmers to think in shaders by parts. I.e. a transformation to colors, a transformation to positions, a coordinates map, a normal map, 
/// a normal transformation, etc. instead of traditional shader programing using a full code to all aspect to be transformed or calculated.
/// Shaders programming allows abstraction! Shaders can be declared using a virtual method, or an abstract method, even a delegate object. The compiler mechanism and cache do the real
/// work.
/// Several of most used shader logics can be accessed using the Shaders class.
/// There is a lot of customized vertex/pixel data element types, but you create your own struct type with its own semantics.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Rendering;
using System.Maths;
using System.Rendering.Effects;
using System.Compilers.Shaders;

namespace Tutorials.Shading
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            renderedControl1.Render = new System.Rendering.Direct3D9.Direct3DRender();
        }

        IModel sampleModel;
        bool filling = true;

        private void renderedControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F)
                filling = !filling;
        }

        private void renderedControl1_InitializeRender(object sender, System.Rendering.Forms.RenderEventArgs e)
        {
            var render = e.Render;

            /// Tessellated method is valid for meshes and subdivide triangles in smaller triangles performing an approximation for the new positions.
            sampleModel = Models.Teapot.Allocate(render).Tessellated (4);
        }

        private void renderedControl1_Rendered(object sender, System.Rendering.Forms.RenderEventArgs e)
        {
            var render = e.Render;

            render.BeginScene();

            render.Draw(() =>
            {
                /// This teapot sample presents a vertex transformation and a color set up per-pixel.
                render.Draw(sampleModel, Transforms.Translate (3,0,0), 
                    /// Both vertex shader creations are valid. 
                    /// First, using directly the effect for a vertex shader. 
                    /// Second, using a static method that allow fast vertex shader creation.
                    //new VertexShaderEffect<PositionData, PositionData> (In => 
                    //    new PositionData 
                    //    { 
                    //        Position = In.Position + new Vector3(0, 0.1f, 0) * GMath.sin(In.Position.X * 4 * In.Position.Z * 3  + Environment.TickCount/200f) 
                    //    }),
                    Shaders.FreeTransform<PositionData>(In =>
                        new PositionData
                        {
                            Position = In.Position + new Vector3(0, 0.1f, 0) * GMath.sin(In.Position.X * 4 * In.Position.Z * 3 + Environment.TickCount / 200f)
                        }),
                    Shaders.PixelTransform<ColorCoordinatesData, ColorData> (In => 
                        new ColorData 
                        { 
                            Color = new Vector4 (In.Coordinates.X , In.Coordinates.Y, 0, 1) * In.Color
                        })
                    );

                /// This teapot sample presents a custom-semantic that is set up foreach vertex and used as color in a per-pixel processing.
                /// See below the creation of a user-semantic to vertex and pixel transforms.
                render.Draw(sampleModel, Transforms.Translate(-3, 0, 0),
                    /// Vertex shader to define the temperature of a point based in its height.
                    Shaders.VertexTransform<PositionData, TemperatureData>(In => new TemperatureData
                    {
                        Value = (In.Position.Y + GMath.sin (Environment.TickCount/1000f) + 1)/2
                    }),
                    /// Pixel shader to define the color foreach temperature as a lerp between red and blue.
                    Shaders.PixelTransform<TemperatureData, ColorData>(In => new ColorData
                    {
                        Color = new Vector4(GMath.lerp(Vectors.Blue, Vectors.Red, new Vector3(1, 1, 1) * In.Value), 1)
                    }));
            },
                Materials.White.Glossy.Glossy.Shinness,
                Transforms.Rotate (Environment.TickCount/1000f, Axis.Y),
                Lights.Ambient(new Vector3(0.5f, 0.5f, 0.5f)),
                Lights.Point(new Vector3(3, 5, 6), new Vector3(1, 1, 1)),
                filling ? RasterOptions.ViewSolid : RasterOptions.ViewWireframe,
                Cameras.LookAt(new Vector3(3, 2, 5), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
                Cameras.Perspective(render.GetAspectRatio()),
                Buffers.Clear(0.2f, 0.2f, 0.4f, 1),
                Buffers.ClearDepth(),
                Shaders.Phong
                );

            render.EndScene();

            renderedControl1.Invalidate();
        }
    }

    /// Imagine we want to set a temperature value to each vertex to be used to draw later.
    /// First stablish the semantic. Semantic objects have information about the semantic, i.e. default values for its components.
    public class TemperatureSemantic : DataSemantic
    {
        /// <summary>
        /// This method will specify the default value to each component of vectors with this semantic.
        /// </summary>
        public override float GetDefaultValue()
        {
            return 1; // our temperature by default will be 1 en each component.
        }
    }

    /// Second we need to create a SemanticAttribute to mark field with this semantic.
    [CompileSemanticAs(SemanticType = typeof(TemperatureSemantic))] /// this way, when this attribute is found in a field, we know what semantic it corresponds.
    public class TemperatureAttribute : DataComponentAttribute
    {
    }

    /// Now we can create a struct type to define Temperature operations.
    [ShaderType]
    public struct TemperatureData
    {
        [Temperature]
        public float Value;
    }
}
