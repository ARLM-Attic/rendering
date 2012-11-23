using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Rendering;

namespace Tutorials.Pipelines
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            renderedControl1.Render = new System.Rendering.Direct3D9.Direct3DRender();
        }

        IModel m;

        private void renderedControl1_InitializeRender(object sender, System.Rendering.Forms.RenderEventArgs e)
        {
        }

        private void renderedControl1_Rendered(object sender, System.Rendering.Forms.RenderEventArgs e)
        {

        }
    }
}
