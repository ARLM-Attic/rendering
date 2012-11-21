using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Rendering.Effects;
using System.Diagnostics;
using System.Threading;

namespace System.Rendering.Forms
{
    /// <summary>
    /// Defines a Win32 Control for rendering.
    /// </summary>
    public class RenderedControl : Control
    {
        IControlRenderDevice render;
        bool displayInfo = false;

        public RenderedControl()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
        }

        bool includeRayPickers;
        [Category("Rendering")]
        public bool IncludeRayPicker
        {
            get
            {
                return includeRayPickers;
            }
            set
            {
                if (render != null)
                {
                    render.RayPickers.Clear();

                    if (value)
                        render.RayPickers.Add(new Win32MousePicker(this));
                }

                includeRayPickers = value;
            }
        }

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

                    // no hacer dispose aqui, y dejarle esa responsabilidad al usuario del control.
                    //render.Dispose();
                }

                render = value;

                if (render != null)
                {
                    render.Created += new EventHandler(render_Created);
                    // render.CreateDevice(this.Handle);
                }

                IncludeRayPicker = IncludeRayPicker;
            }
        }

        private void render_Created(object sender, EventArgs e)
        {
            if (InitializeRender != null)
                InitializeRender(this, new RenderEventArgs(render));

        }

        static int co;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (render == null)
            {
                e.Graphics.Clear(Color.SteelBlue);
                e.Graphics.DrawString("No render present.", Font, Brushes.Black, 0, 0);
                return;
            }

            RaiseRender();

            if (!render.IsCreated)
            {
                e.Graphics.Clear(Color.SteelBlue);
                e.Graphics.DrawString("No render created." + (co++), Font, Brushes.Black, 0, 0);
                return;
            }

            if (this.DesignMode)
            {
                e.Graphics.Clear(BackColor);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        public void RaiseRender()
        {
            if (render == null)
                return;

            if (!render.IsCreated)
                render.CreateDevice(this);

            if (render.IsCreated)
                OnRendered(new RenderEventArgs(render));
        }

        public virtual void OnRendered(RenderEventArgs e)
        {
            fpsCounter.Start();

            if (this.Rendered != null)
                this.Rendered(this, e);

            fpsCounter.End();
        }

        FPSCounter fpsCounter = new FPSCounter();

        public int FPS
        {
            get { return (int)fpsCounter.FPS; }
        }

        [Category ("Rendering")]
        public event EventHandler<RenderEventArgs> InitializeRender;

        [Category("Rendering")]
        public event EventHandler<RenderEventArgs> Rendered;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (render != null)
                render.Dispose();
        }
    }

    public class FPSCounter
    {
        int numberOfSamples = 0;
        double totalMilliseconds = 0;

        double lastTime;

        double fpsMedia = 0;

        Stopwatch clock = new Stopwatch();

        public void Start() {
            clock.Reset();
            clock.Start();
        }
        public void End()
        {
            clock.Stop();

            lastTime = clock.Elapsed.TotalMilliseconds;
            fpsMedia = (fpsMedia * numberOfSamples + 1000.0 / lastTime) / (++numberOfSamples);

            totalMilliseconds += clock.Elapsed.TotalMilliseconds;
        }

        public int NumberOfSamples { get { return numberOfSamples; } }

        public double TotalMilliseconds { get { return totalMilliseconds; } }

        public double FPS
        {
            get { return fpsMedia; }
            //get { return 1000.0 * numberOfSamples / Math.Max(0.00001, totalMilliseconds); }
        }
    }

    public class RenderEventArgs : EventArgs
    {
        public readonly IControlRenderDevice Render;

        public RenderEventArgs(IControlRenderDevice render)
        {
            this.Render = render;
        }
    }
}
