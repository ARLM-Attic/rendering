/// Tutorial 2. Modeling
/// This tutorial shows some modeling tools in Rendering.
/// Model objects are in charge of drawing primitive using some tessellation tools provided by a tessellator object.
/// Primitive represents the geometrical structure of a solid. They are mostly represented by vertexes and each vertex represents a set of data
/// foreach surface point. Rendering supports flexible vertex formats using structs and semantics to its fields.

/// In this code we use some features like models creation, transformation, grouping.
/// In this tutorial we show five kind of basic model conceptions...
/// 1) Basic models like cube, sphere, cylinder, teapot.
/// 2) Manifold models created by parametrization of a point, curve or surfaces.
/// 3) Single user primitive as a model.
/// 4) Custom model implementation by grouping several model parts.
/// 5) Csg models.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Rendering.Direct3D9;
using System.Rendering;
using System.Maths;
using System.Rendering.Modeling;
using System.Rendering.OpenGL;
using System.Rendering.Resourcing;
using System.Diagnostics;

namespace Tutorials.Modeling
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.renderedControl1.Render = new Direct3DRender();

            renderedControl1.KeyDown += new KeyEventHandler(renderedControl1_KeyDown);

            stopwatch.Start();
        }

        void renderedControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F)
                filling = !filling;

            if (e.KeyCode == Keys.Space)
                if (stopwatch.IsRunning)
                    stopwatch.Stop();
                else
                    stopwatch.Start();
        }

        IModel basic;
        IModel manifoldModel;
        IModel primitive;
        IModel group;
        IModel csg;

        bool filling = true;

        private void renderedControl1_InitializeRender(object sender, System.Rendering.Forms.RenderEventArgs e)
        {
            var render = e.Render;

            #region Basic Models

            /// Basic models like sphere, cylinder, cube, teapot, etc. are accessible from Models class.
            /// All models can be transformed by a matrix (affine transformations) or by a function to transform each vertex of the model.
            /// Those transformations occur in CPU. To transform using a GPU capability use a shader effect instead.
            
            basic = Models.Sphere.Transformed (Matrices.Scale (1,0.4f,1)).Allocate(render);
            //basic = Models.Cylinder.Allocate(render);
            //basic = Models.Sphere.Allocate(render);

            #endregion

            #region Manifold Models

            /// A Manifold object is a math structure representing a manifold in a space. If the manifold has dimension 1 it is a curve,
            /// dimension 2 is a surface, etc.
            /// Manifold objects (accessible from Manifold class methods) uses a delegate to generate 3D points from canonic parameters.
            /// A Manifold object can be used to map vertexes and draw a discrete representation using a IManifoldModel object.
            /// For this interface we present the PointModel (draws manifolds of dimension 0), a CurveModel (draws manifolds of dimension 1),
            /// and SurfaceModel (for dimension 2). The model is created of certain resolution using slices and stacks numbers.

            //// Using shorcut creations in Models class
            //manifoldModel = Models.Surface((u, v) => new Vector3(u, GMath.sin (u)*GMath.cos(v), v)).Allocate(render);

            //// Other manifolds
            //manifoldModel = Models.Curve(u => new Vector3(GMath.cos(u * 2 * GMath.Pi), u, GMath.sin(u * 2 * GMath.Pi)));

            // Using directly the corresponding manifold model with the manifold object as parameter.
            // The resolution of the model is passed as well. If not slices or stacks are provider, it will be constructed with 32.
            manifoldModel = new SurfaceModel(Manifold.Surface((u, v) => new Vector3(u, GMath.sin(u) * GMath.cos(v), v)), 32, 32, false).Allocate(render);

            #endregion

            #region Single Primitive Models

            /// A Model is an object capable of draw primitives in a RenderDevice. The ability to draw primitives is exposed in
            /// ITessellator object of a RenderDevice.
            /// The SingleModel class can be used to get a single primitive as a model.
            /// Vertexes of a model are represented by a struct type. There is a lot of structs to represent the most used variety of
            /// vertex combination. Anyway you can use your own flexible vertex type defining a struct and setting semantics to its field.
            /// See the MyPositionNormalColorData definition below.

            //// Using Extensor method AsModel for Primitives
            //primitive = Basic.TriangleFan(new PositionNormalColorData[] {
            //    PositionNormalColorData.Create (new Vector3 (0,0,0), new Vector3 (0,0,1), new Vector4 (1,1,1,1)),
            //    PositionNormalColorData.Create (new Vector3 (1,0,0), new Vector3 (0,0,1), new Vector4 (1,0,0,1)),
            //    PositionNormalColorData.Create (new Vector3 (1,1,0), new Vector3 (0,0,1), new Vector4 (1,1,0,1)),
            //    PositionNormalColorData.Create (new Vector3 (0,1,0), new Vector3 (0,0,1), new Vector4 (0,1,0,1))
            //}).AsModel().Allocate(render);

            //// Using Single method in Models class
            //primitive = Models.Single (Basic.TriangleFan(new PositionNormalColorData[] {
            //    PositionNormalColorData.Create (new Vector3 (0,0,0), new Vector3 (0,0,1), new Vector4 (1,1,1,1)),
            //    PositionNormalColorData.Create (new Vector3 (1,0,0), new Vector3 (0,0,1), new Vector4 (1,0,0,1)),
            //    PositionNormalColorData.Create (new Vector3 (1,1,0), new Vector3 (0,0,1), new Vector4 (1,1,0,1)),
            //    PositionNormalColorData.Create (new Vector3 (0,1,0), new Vector3 (0,0,1), new Vector4 (0,1,0,1))
            //}));

            // Using directly the single model class.
            primitive = new SingleModel<Basic>(Basic.TriangleFan(new MyPositionNormalColorData[] {
                MyPositionNormalColorData.Create (new Vector3 (0,0,0), new Vector3 (0,0,-1), new Vector4 (1,1,1,1)),
                MyPositionNormalColorData.Create (new Vector3 (1,0,0), new Vector3 (0,0,-1), new Vector4 (1,0,0,1)),
                MyPositionNormalColorData.Create (new Vector3 (1,1,0), new Vector3 (0,0,-1), new Vector4 (1,1,0,1)),
                MyPositionNormalColorData.Create (new Vector3 (0,1,0), new Vector3 (0,0,-1), new Vector4 (0,1,0,1))
            })).Allocate(render);

            #endregion

            #region Groups of models treated like just one.

            /// A group of models can be treated as a model. To get this functionallity easy to use, we design the ModelGroup class.
            /// This class can be used directly (passing children models as parameters in constructor of ModelGroup type) or by inheritance.
            /// Note: Grouping has not same effect that Union. (Union is a CSG operation that removes internal surfaces).

            // Inherited from Model group...
            group = new Table().Allocate(render);
            //// Using ModelGroup class directly.
            //group = new ModelGroup(Models.Cube, Models.Sphere.Translated(1, 0, 0));
            //// Using Group method in Models class.
            //group = Models.Group(Models.Cube, Models.Sphere.Translated(1, 0, 0));

            #endregion

            #region CSG with models

            /// Models class support csg operations between models. Union receive several models to join, Intersect receives two models to get
            /// the intersection model and Subtract receives the two models to subtract the second solid to the first one.
            /// All these operations returns a Mesh object with the result solid. Those operations works with any model but it should be used with Two-Manifold models.
            var cross = Models.Union(
							Models.Cylinder.Translated(0, -0.5f, 0).Scaled(0.5f, 2, 0.5f),
							Models.Cylinder.Translated(0, -0.5f, 0).Scaled(0.5f, 2, 0.5f).Rotated(GMath.Pi / 2, new Vector3(1, 0, 0)),
							Models.Cylinder.Translated(0, -0.5f, 0).Scaled(0.5f, 2, 0.5f).Rotated(GMath.Pi / 2, new Vector3(0, 0, 1))).Scaled(0.7f, 0.7f, 0.7f);

						var intersection = Models.Intersection(Models.Cube.Translated(-0.5f, -0.5f, -0.5f), Models.Sphere.Scaled(0.7f, 0.7f, 0.7f));

						csg = Models.Subtract(intersection, cross).Allocate(render);
            #endregion
        }

        // This attribute is not needed for vertex declaration only. But probably a vertex definition 
        // is used as a Shader Type to define a vertex to be transformed.
        [System.Compilers.Shaders.ShaderType]
        public struct MyPositionNormalColorData
        {
            [Position] // Attribute to set a Position semantic to this field.
            public Vector3 Position;
            [Normal] // Attribute to set a Normal semantic to this field.
            public Vector3 Normal;
            [Color] // Attribute to set a Color semantic to this field.
            public Vector4 Color;

            public static MyPositionNormalColorData Create(Vector3 position, Vector3 normal, Vector4 color)
            {
                return new MyPositionNormalColorData { Position = position, Normal = normal, Color = color };
            }
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
                /// Drawing five models rotating over the world center.
                /// They are all translated 4 units from the center and they are rotating over its own axis.

                render.Draw(basic,
                    Transforms.Rotate((float)-stopwatch.Elapsed.TotalSeconds, Axis.Y | Axis.X),
                    Transforms.Translate(-4, 0, 0),
                    Transforms.Rotate(0, Axis.Y),
                    Materials.WhiteSmoke.Glossy.Glossy.Shinness.Shinness);

                render.Draw(manifoldModel,
                    Transforms.Rotate((float)-stopwatch.Elapsed.TotalSeconds, Axis.Y | Axis.X),
                    Transforms.Translate(-4, 0, 0),
                    Transforms.Rotate(2 * GMath.Pi / 5, Axis.Y),
                    Materials.LightBlue.Glossy.Glossy.Shinness.Shinness);

                render.Draw(primitive,
                    Transforms.Rotate((float)-stopwatch.Elapsed.TotalSeconds, Axis.Y | Axis.X),
                    Transforms.Translate(-4, 0, 0),
                    Transforms.Rotate(2 * GMath.Pi / 5 * 2, Axis.Y),
                    Materials.White.Glossy.Glossy.Shinness.Shinness);
                render.Draw(group,
                    Transforms.Rotate((float)-stopwatch.Elapsed.TotalSeconds, Axis.Y | Axis.X),
                    Transforms.Translate(-4, 0, 0),
                    Transforms.Rotate(2 * GMath.Pi / 5 * 3, Axis.Y),
                    Materials.Brown.Glossy.Glossy.Shinness.Shinness);

                render.Draw(csg,
                   Transforms.Rotate((float)-stopwatch.Elapsed.TotalSeconds, Axis.Y | Axis.X),
                   Transforms.Translate(-4, 0, 0),
                   Transforms.Rotate(2 * GMath.Pi / 5 * 4, Axis.Y),
                   Materials.Red.Glossy.Glossy.Shinness.Shinness);
            },
                /// RasterOptions class allows to access to Culling and Filling effects. Those are settings of cull modes and fill modes respectively.
                RasterOptions.NoCull,
                filling ? 
                    RasterOptions.ViewSolid : 
                    RasterOptions.ViewWireframe,
                Transforms.Rotate((float)stopwatch.Elapsed.TotalSeconds / 5, Axis.Y),
                Lights.Point(new Vector3(3, 5, 6), new Vector3(1, 1, 1)),
                Cameras.LookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), new Vector3(0, 1, 0)),
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
    /// Custom model designed by extending a model group.
    /// </summary>
    class Table : ModelGroup
    {
        static Matrix4x4 center = Matrices.Translate(-0.5f, 0, -0.5f);
        static Matrix4x4 legScale = Matrices.Scale(0.1f, 1, 0.1f);

        public Table()
            : base(
                /// A model group uses a collection of models to conform a new model.
                /// In this case we are building a Table model using 5 transformed cubes.
                /// Transformed is a method in all models. But Transformed used below is a extensor method to apply 
                /// several matrix transformations as one.
                Models.Cube.Transformed(center, Matrices.Scale(1.3f, 0.1f, 1.3f)),
                Models.Cube.Transformed(center, legScale, Matrices.Translate(-0.5f, -1, -0.5f)),
                Models.Cube.Transformed(center, legScale, Matrices.Translate(0.5f, -1, -0.5f)),
                Models.Cube.Transformed(center, legScale, Matrices.Translate(0.5f, -1, 0.5f)),
                Models.Cube.Transformed(center, legScale, Matrices.Translate(-0.5f, -1, 0.5f)))
        {
        }
    }
}
