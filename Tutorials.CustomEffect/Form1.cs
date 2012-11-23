/// Tutorial 4. Custom effects
/// This tutorial shows creating custom effect objects in Rendering.
/// Effect objects are responsable of creating a technique that performs changes in render states.
/// Each effect type should define a Technique derivated type to set this logic.
/// An effect is a Graphic Resource so it is intended to be allocated whenever it can in a render device memory.
/// You need to implement allocation methods (OnClone, OnDispose) as well as technique methods: Save, Restore and Technique enumeration.
/// The technique passes are described in a lazy enumerable that performs changes in render states and yields new passes.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Rendering;
using System.Rendering.RenderStates;
using System.Rendering.Effects;
using System.Maths;

namespace Tutorials.CustomEffect
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.renderedControl1.Render = new System.Rendering.OpenGL.OpenGLRender();
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

            sampleModel = Models.Teapot.Allocate(render);
        }

        private void renderedControl1_Rendered(object sender, System.Rendering.Forms.RenderEventArgs e)
        {
            var render = e.Render;

            render.BeginScene();

            render.Draw(() =>
            {
                    render.Draw(sampleModel, 
                        new TwoMaterialsEffect { Material = Materials.DarkRed.Shinness.Shinness.Glossy.Glossy, BackMaterial = Materials.White },
                        Transforms.Rotate (Environment.TickCount/500f, Axis.Y | Axis.X)
                        );
            },
                /// Set an ambient light to see back faces color.
                Lights.Ambient (new Vector3(0.5f, 0.5f,0.5f)),
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

    /// <summary>
    /// Represents an effect that have two different materials for an object. One to its front faces and other to back faces.
    /// </summary>
    public class TwoMaterialsEffect : Effect
    {
        /// <summary>
        /// Material to front faces
        /// </summary>
        public Material Material;

        /// <summary>
        /// Material to back faces
        /// </summary>
        public Material BackMaterial;

        /// <summary>
        /// Represents the technique of this effect
        /// </summary>
        class TwoMaterialsTechnique : Technique
        {
            /// <summary>
            /// Instance of the effect this technique belongs to.
            /// </summary>
            TwoMaterialsEffect effect;

            /// <summary>
            /// The constructor bind the render states manager object to this technique.
            /// </summary>
            public TwoMaterialsTechnique(TwoMaterialsEffect effect, IRenderStatesManager manager)
                : base(manager)
            {
                this.effect = effect;
            }

            /// <summary>
            /// This method will be called at starting a technique and grants the states of the pipeline will be saved before this effect take place.
            /// </summary>
            protected override void SaveStates()
            {
                /// Our effect will modify these states.
                RenderStates.Save<CullModeState>();
                RenderStates.Save<MaterialState>();
            }

            protected override void RestoreStates()
            {
                /// Restores the render states were modified.
                RenderStates.Restore<MaterialState>();
                RenderStates.Restore<CullModeState>();
            }

            /// <summary>
            /// Our effect has two passes. One to draw the back material and another to draw the front face material.
            /// </summary>
            protected override IEnumerable<Pass> Passes
            {
                get
                {
                    RenderStates.SetState(CullModeState.Front); // occlude front faces
                    RenderStates.SetState(effect.BackMaterial.State);
                    yield return NewPass();

                    RenderStates.SetState(CullModeState.Back); // occlude back faces
                    RenderStates.SetState(effect.Material.State);
                    yield return NewPass();
                }
            }
        }


        /// <summary>
        /// OnClone and OnDispose methods are inherited from AllocateableBase class. This class allows a fast implementation of a graphic resource
        /// and these methods allows to make a depth allocation of this resource in the device or in RAM.
        /// </summary>
        /// <param name="toFill">This is the clone instance of this object that needs to be filled with specific object fields stored in render device.</param>
        /// <param name="render">The render were to store this resource. If null, RAM should be intended.</param>
        /// <returns>The final location of the allocated resource. If it could be stored at device, it should return Location.Device.</returns>
        protected override Location OnClone(System.Rendering.Resourcing.AllocateableBase toFill, IRenderDevice render)
        {
            /// we dont have any particular resource or effect that can be stored so...

            var effect = toFill as TwoMaterialsEffect;
            effect.Material = this.Material;
            effect.BackMaterial = this.BackMaterial;

            return Location.Device;
        }

        protected override void OnDispose()
        {
            /// nothing to dispose.
        }

        /// <summary>
        /// This method will be used by the effect to get a techique valid for a specific render states manager.
        /// This way an effect is independent of a specific device or RenderStatesManager and can be used in several render devices
        /// because allways creates a new technique for differents render devices.
        /// </summary>
        protected override Technique GetTechnique(IRenderStatesManager manager)
        {
            return new TwoMaterialsTechnique(this, manager);
        }
    }
}
