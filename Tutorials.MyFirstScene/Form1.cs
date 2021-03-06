﻿/// Tutorial 1. The begining
/// This tutorial shows the use of Rendering to draw a simple scene in few steps.
/// In this code we use some features like allocation of resources, setting effects and drawing basic models.
/// Also it shows the basic effects that are used frequently in applications.

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
using System.Rendering.RenderStates;

namespace Tutorials.MyFirstScene
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            /// Sets the render object to use by this RenderedControl.
            /// RenderDevice objects represents the abstraction of a Render Device, like Device interface in DX or Rendering Contexts in OpenGL.
            renderedControl1.Render = new System.Rendering.Direct3D9.Direct3DRender();
        }

        IModel model;
        
        /// <summary>
        /// This event will be call once the render device were set to the rendered control.
        /// </summary>
        private void renderedControl1_InitializeRender(object sender, System.Rendering.Forms.RenderEventArgs e)
        {
            /// You can load or unload resources to the device whenever you want. 
            /// In fact, if you dont allocate models or effects before they are used, the engine does temporally.
            /// Anyway, this method can be used to allocate resources at device once created the render device.

            var render = e.Render;

            /// Models is a starting class to manage basic models in user memory.
            /// Allocate method (valid for all resources like textures, effect, models and graphics) creates a clone of the object
            /// using the memory of specific device whenever it can be done.
            /// You can query if the resource could be allocated by checking model.Location property.
            model = Models.Teapot.Allocate(render).Tessellated (4);
        }

        /// <summary>
        /// This event will be call each time the control needs to be rendered.
        /// </summary>
        private void renderedControl1_Rendered(object sender, System.Rendering.Forms.RenderEventArgs e)
        {
            /// Not all render devices are a rectangular control region.
            /// In its more basic form, a render device has not limits (imagine a web page, or a console)
            /// To specify renders that result in a image use (implement) IImageRenderDevice.
            /// IControlRenderDevice is an IImageRenderDevice.
            var render = e.Render;

            /// All draws need to be between a begin scene and an end scene calls. That is to allow the most widely used pattern to draw a frame.
            render.BeginScene();

            /// There are 3 basic operations to draw. Set one effect, draw a primitive, release last used effect.
            /// These operations are the functions Apply, Draw, EndScope.
            /// To maintain the consistency of the applied and unapplied effects, functions Apply and EndScope are not intended to be used
            /// by programmers. Instead of, there are few extensor methods to easily draw objects using a secuence of effects.
            render.Draw(() =>
            {
                /// All the draw in this action will be drawn with global effects.
                /// Draw methods receive the object (model, graphic or primitive) to be drawn and a secuence of effects to be used.
                render.Draw(model,
                    Shaders.VertexTransform<PositionData, PositionData>(In => new PositionData { Position = In.Position + GMath.sin(In.Position.Z*10)*new Vector3(0,1,0)*0.3f }),
                    /// these are the local effects to the model
                    /// More close to the model, more a local effect (last being applied before draw).
                    /// Next transformation effects result in a translation of a rotation instead of a rotation of a translation.
                    /// Transforming effects allows world transformations to be set in render states.
                    /// You can access to several transforms using Transforms class or casting a 4x4 matrix to Transforming.
                    Transforms.Rotate(Environment.TickCount / 1000f, Axis.Y),
                    Transforms.Translate(-1, 0, 0), /// Swaps these transformations to see the importance of the order.
                    /// Materials is a class to easily create material effects. A Material effect sets some material info in render states.
                    Materials.Red.Glossy.Glossy.Shinness.Shinness);
            },
                /// Creates an effect that sets a light in render states.
                Lights.Point (new Vector3 (3,5,6), new Vector3 (1,1,1)),
                /// Creates an effect that sets the viewer matrix in render states.
                Cameras.LookAt(new Vector3 (2,3,4), new Vector3 (0,0,0), new Vector3 (0,1,0)),
                /// Creates an effect that sets the projection matrix in render states.
                Cameras.Perspective(render.GetAspectRatio()),
                /// Creates an effect that erases the frame buffer with certain color.
                Buffers.Clear(0.2f, 0.2f, 0.4f, 1),
                /// Creates an effect that erases the depth buffer with 1.
                Buffers.ClearDepth(),
                /// Shading is a class to enable several pipeline configurations using shaders.
                /// By default, fixed pipelines-based render devices will skip all these effects, but in 
                /// programmable pipelines-based render devices, default behaviour doesnt use render states, so this instruction
                /// is required to used a traditional pipeline using shaders.
                Shaders.Phong

                );

            /// This call should present the scene.
            render.EndScene();

            /// Force renderedcontrol to redraw.
            renderedControl1.Invalidate();
        }

    }
}
