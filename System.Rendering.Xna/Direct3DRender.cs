using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Forms;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.Xna.Framework.Graphics;

namespace System.Rendering.Xna
{
  public partial class Direct3DRender : RenderBase, IControlRenderDevice
  {
    private Control control;
    private GraphicsDevice device;
    private bool fullScreen;

    public event EventHandler Created;
    public event EventHandler Disposed;

    public Direct3DRender()
      : this(false)
    {
    }

    public Direct3DRender(bool fullScreen)
    {
      this.fullScreen = fullScreen;
    }

    protected void OnCreated()
    {
      if (Created != null)
        Created(this, EventArgs.Empty);
    }
    
    protected void OnDisposed()
    {
      if (Disposed != null)
        Disposed(this, EventArgs.Empty);
    }

    protected override RenderBase.ResourcesManagerBase CreateResourcesManager()
    {
      return new ResourcesManager(this);
    }

    protected override RenderBase.TessellatorBase CreateTessellator()
    {
      return new Tessellator(this);
    }

    protected override RenderBase.RenderStatesManagerBase CreateRenderStatesManager()
    {
      return new RenderStatesManager(this);
    }

    public override void BeginScene()
    {
      base.BeginScene();
    }

    public override void EndScene()
    {
      device.Present();
    }

    public bool IsCreated
    {
      get { return device != null; }
    }

    public void CreateDevice(Control hWnd)
    {
      control = hWnd;

      var parameters = new PresentationParameters()
      {
        BackBufferFormat = SurfaceFormat.Color,
        BackBufferHeight = control.Height,
        BackBufferWidth = control.Width,
        DeviceWindowHandle = control.Handle,
        IsFullScreen = fullScreen,
        MultiSampleCount = 1,
        DepthStencilFormat = DepthFormat.Depth24Stencil8,
        PresentationInterval = PresentInterval.Default,
        RenderTargetUsage = RenderTargetUsage.DiscardContents
      };

      if (device == null)
      {
        device = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.Reach, parameters);
        device.DeviceReset += (o, e) => { OnCreated(); };
        device.Disposing += (o, e) => { OnDisposed(); };
        OnCreated();
      }
      else
      {
        device.Reset(parameters, GraphicsAdapter.DefaultAdapter);
      }
    }

    public ISite Site { get; set; }
  }
}
