using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Rendering.OpenGL;
using System.Rendering.Direct3D9;
using System.Rendering.Xna;
using Testing.Common;
using System.Rendering.Forms;
using System.Rendering;

namespace Testing.Comparisons
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            StartMonitors();

            refreshTimer = new Timer();
            refreshTimer.Tick += new EventHandler(refreshTimer_Tick);
            refreshTimer.Interval = 10;
            refreshTimer.Enabled = true;
        }

        void refreshTimer_Tick(object sender, EventArgs e)
        {
            foreach (var monitor in monitors)
                monitor.RaiseRender();
        }

        Type[] renderTypes = new Type[] { /*typeof(Direct3DRender),*/ typeof(OpenGLRender), /*typeof (XnaRender)*/ };

        Type[] sceneTypes = new Type[] { typeof(BasicShaderScene), typeof (TextureCubeScene), typeof (VolumeTextureScene), typeof (GeneratingOverTextureScene), typeof (ShadowMapScene) };

        List<RenderedControl> monitors = new List<RenderedControl>();

        Timer refreshTimer;

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //tableLayoutPanel1.Width = (int)(1.8 * tableLayoutPanel1.ClientSize.Height / renderTypes.Length * sceneTypes.Length);
        }

        private void StartMonitors()
        {
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();

            tableLayoutPanel1.RowCount = renderTypes.Length;
            tableLayoutPanel1.ColumnCount = sceneTypes.Length;
            for (int i = 0; i < renderTypes.Length; i++)
            {
                RowStyle s = new RowStyle { Height = 100, SizeType = SizeType.Percent };
                tableLayoutPanel1.RowStyles.Add(s);
            }
            for (int i = 0; i < sceneTypes.Length; i++)
            {
                ColumnStyle s = new ColumnStyle { Width = 100, SizeType = SizeType.Percent };
                tableLayoutPanel1.ColumnStyles.Add(s);
            }

            for (int i = 0; i < renderTypes.Length; i++)
            {
                for (int j = 0; j < sceneTypes.Length; j++)
                {
                    var render = Activator.CreateInstance(renderTypes[i]) as IControlRenderDevice;
                    var scene = Activator.CreateInstance(sceneTypes[j]) as Scene;

                    RenderedControl renderControl = new RenderedControl();
                    renderControl.Dock = DockStyle.Fill;
                    renderControl.Render = render;

                    renderControl.InitializeRender += (o, e) =>
                    {
                        scene.Initialize(e.Render as IControlRenderDevice);
                    };

                    renderControl.Rendered += (o, e) =>
                    {
                        try
                        {
                            scene.Draw(e.Render as IControlRenderDevice);
                        }
                        catch
                        {
                        }
                    };

                    tableLayoutPanel1.Controls.Add(renderControl);
                    tableLayoutPanel1.SetRow(renderControl, i);
                    tableLayoutPanel1.SetColumn(renderControl, j);

                    monitors.Add(renderControl);
                }
            }
        }
    }
}
