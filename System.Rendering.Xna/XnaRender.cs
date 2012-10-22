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
    public partial class XnaRender : RenderBase, IControlRenderDevice, ITextureRenderDevice
    {
        private Control _control;
        private GraphicsDevice _device;
        private bool _fullScreen;

        public event EventHandler Created;
        public event EventHandler Disposed;

        public XnaRender()
            : this(false)
        {
        }

        public XnaRender(bool fullScreen)
        {
            this._fullScreen = fullScreen;
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
            return new XnaResourcesManager(this);
        }

        protected override RenderBase.TessellatorBase CreateTessellator()
        {
            return new XnaTessellator(this);
        }

        protected override RenderBase.RenderStatesManagerBase CreateRenderStatesManager()
        {
            return new XnaRenderStatesManager(this);
        }

        protected override RenderBase.ServicesManagerBase CreateServicesManager()
        {
            return new XnaServices(this);
        }

        class RenderTargetInfo
        {
            public RenderTargetBinding[] Targets;
            public int Width;
            public int Height;
        }

        Stack<RenderTargetInfo> saveTargets = new Stack<RenderTargetInfo> ();

        RenderTargetBinding[] ScreenTarget;

        RenderTarget2D GetTextureFor(TextureBuffer texture)
        {
            var t = ((XnaRender.XnaResourcesManager.TextureBufferResourceOnDeviceManager)Resources.GetManagerFor<TextureBuffer>(texture)).TextureBuffer;
            return (RenderTarget2D)t;
        }
        RenderTargetCube GetTextureFor(CubeTextureBuffer texture)
        {
            var t = ((XnaRender.XnaResourcesManager.CubeTextureResourceOnDeviceManager)Resources.GetManagerFor<CubeTextureBuffer>(texture)).TextureBuffer;
            return (RenderTargetCube)t;
        }

        RenderTargetInfo CreateRenderTarget(TextureBuffer[] targets)
        {
            if (targets.Length > 0)
            {
                RenderTargetBinding[] t = new RenderTargetBinding[targets.Length];
                for (int i = 0; i < targets.Length; i++)
                    if (targets[i] is CubeTextureBuffer)
                        t[i] = new RenderTargetBinding(GetTextureFor((CubeTextureBuffer)targets[i]), (CubeMapFace)((CubeTextureBuffer)targets[i]).ActiveFace);
                    else
                        t[i] = new RenderTargetBinding(GetTextureFor((TextureBuffer)targets[i]));

                return new RenderTargetInfo { Width = targets[0].Width, Height = targets[0].Height, Targets = t };
            }
            else
                return new RenderTargetInfo { Width = _control.ClientSize.Width, Height = _control.ClientSize.Height, Targets = ScreenTarget };
        }

        void SetRenderTarget(RenderTargetInfo Info)
        {
            this.ImageWidth = Info.Width;
            this.ImageHeight = Info.Height;
            this.Device.SetRenderTargets(Info.Targets);
        }

        void ITextureRenderDevice.BeginScene(params TextureBuffer[] targets)
        {
            saveTargets.Push(CreateRenderTarget(targets));

            SetRenderTarget(saveTargets.Peek());
        }

        public override void BeginScene()
        {
            ((ITextureRenderDevice)this).BeginScene(new TextureBuffer[0]);

            base.BeginScene();
        }

        public override void EndScene()
        {
            saveTargets.Pop();

            if (saveTargets.Count == 0)
            {
                Device.SetRenderTargets(ScreenTarget);
                this.ImageWidth = _control.ClientSize.Width;
                this.ImageHeight = _control.ClientSize.Height;
                _device.Present();
            }
            else
            {
                SetRenderTarget(saveTargets.Peek());
            }
        }

        public bool IsCreated
        {
            get { return _device != null; }
        }

        public void CreateDevice(Control hWnd)
        {
            _control = hWnd;

            var parameters = new PresentationParameters()
            {
                BackBufferFormat = SurfaceFormat.Color,
                BackBufferHeight = 2048,
                BackBufferWidth = 2048,
                DeviceWindowHandle = _control.Handle,
                IsFullScreen = _fullScreen,
                MultiSampleCount = 1,
                DepthStencilFormat = DepthFormat.Depth24Stencil8,
                PresentationInterval = PresentInterval.Immediate,
                RenderTargetUsage = RenderTargetUsage.DiscardContents
            };

            if (_device == null)
            {
                _device = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, parameters);

                _device.DeviceReset += (o, e) =>
                {
                    OnCreated();
                    _device.PresentationParameters.BackBufferHeight = _control.Height;
                    _device.PresentationParameters.BackBufferWidth = _control.Width;
                };

                _device.Disposing += (o, e) =>
                {
                    OnDisposed();
                };

                OnCreated();

                ScreenTarget = Device.GetRenderTargets();
            }
            else
            {
                _device.Reset(parameters, GraphicsAdapter.DefaultAdapter);
            }
        }

        public ISite Site { get; set; }

        internal GraphicsDevice Device
        {
            get { return _device; }
        }
    }
}
