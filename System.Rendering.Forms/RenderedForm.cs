using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace System.Rendering.Forms
{
    public partial class RenderedForm : Form
    {
        public RenderedForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, false);

        }

        public DisplaySettings FullScreenSettings { get; set; }

        IControlRenderDevice render;

        bool displayInfo = false;

        [Category("Rendering")]
        public bool DisplayInfo
        {
            get
            {
                return displayInfo;
            }
            set
            {
                displayInfo = value;
            }
        }

        [Category("Rendering")]
        public virtual IControlRenderDevice Render
        {
            get { return render; }
            set
            {

                if (render != null)
                {
                    render.Created -= new EventHandler(render_Created);
                    render.Dispose();
                }

                render = value;

                if (render != null)
                {
                    render.Created += new EventHandler(render_Created);
                }
            }
        }

        private void render_Created(object sender, EventArgs e)
        {
            if (InitializeRender != null)
                InitializeRender(this, new RenderEventArgs(render));
        }

        protected override void OnResize(EventArgs e)
        {
            lock (this)
            {
                try
                {
                    base.OnResize(e);

                    if (render == null) return;

                    render.CreateDevice(this);

                    if (ResizeRender != null)
                        ResizeRender(this, new RenderEventArgs(render));
                }
                catch { }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (render == null)
            {
                e.Graphics.Clear(Color.SteelBlue);
                e.Graphics.DrawString("No render present.", Font, Brushes.Black, 0, 0);
                return;
            }

            if (!render.IsCreated)
                render.CreateDevice(this);

            if (!render.IsCreated)
            {
                e.Graphics.Clear(Color.SteelBlue);
                e.Graphics.DrawString("No render created.", Font, Brushes.Black, 0, 0);
                return;
            }

            Stopwatch clock = new Stopwatch();

            clock.Start();

            OnRendered(new RenderEventArgs(render));

            clock.Stop();

            if (displayInfo)
            {
                e.Graphics.FillRectangle(Brushes.Black, new Rectangle(0, this.Height - 40, 200, 40));
                e.Graphics.DrawString("" + render.ToString(), Font, Brushes.Yellow, 0, this.Height - 40);
                e.Graphics.DrawString("Frame in " + clock.Elapsed.TotalMilliseconds + " milliseconds", Font, Brushes.Yellow, 0, this.Height - 20);
            }
        }

        public virtual void OnRendered(RenderEventArgs e)
        {
            if (this.Rendered != null)
                this.Rendered(this, e);
        }

        [Category("Rendering")]
        public event EventHandler<RenderEventArgs> InitializeRender;

        [Category("Rendering")]
        public event EventHandler<RenderEventArgs> ResizeRender;

        [Category("Rendering")]
        public event EventHandler<RenderEventArgs> Rendered;

    }
}
