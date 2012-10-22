using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Rendering.Forms;
using SlimDX.Direct3D9;
using System.Threading;
using System.Threading.Tasks;

namespace System.Rendering.Direct3D9
{
    public partial class Direct3DRender : RenderBase, IControlRenderDevice, ITextureRenderDevice
    {
        Device device;

        bool fullScreen = false;
        DeviceType preferred;

        internal protected Direct3DEffectManager Manager
        {
            get
            {
                var rs = RenderStates as Direct3DRender.RenderStatesManager;
                if (rs == null)
                    return null;

                return rs.PipelineManager;
            }
        }

        protected internal Device Device { get { return device; } }

        public DeviceType DeviceType
        {
            get
            {
                return preferred;
            }
        }

        public Direct3DRender()
            : this(DeviceType.Hardware, false)
        {
        }

        public Direct3DRender(DeviceType preferred, bool fullScreen)
        {
            this.preferred = preferred;
            this.fullScreen = fullScreen;
        }

        protected override ResourcesManagerBase CreateResourcesManager()
        {
            return new Direct3DResourcesManager(this);
        }

        protected override TessellatorBase CreateTessellator()
        {
            return new TessellatorManager(this);
        }

        protected override RenderStatesManagerBase CreateRenderStatesManager()
        {
            return new RenderStatesManager(this);
        }

        protected override RenderBase.ServicesManagerBase CreateServicesManager()
        {
            return new Direct3D9Services(this);
        }

        class RenderTargetsInfo : IDisposable
        {
            public SlimDX.Direct3D9.Surface[] RenderTargets;
            public TextureBuffer[] DestinationTextures;
            public SlimDX.Direct3D9.Surface DepthSurface;

            public int Width { get { return DepthSurface == null ? RenderTargets[0].Description.Width : DepthSurface.Description.Width; } }
            public int Height { get { return DepthSurface == null ? RenderTargets[0].Description.Height : DepthSurface.Description.Height; } }

            public void Dispose()
            {
                if (DepthSurface != null)
                    DepthSurface.Dispose();

                foreach (var s in RenderTargets)
                    if (s != null)
                        s.Dispose();
            }
        }

        RenderTargetsInfo backBufferRenderTarget;
        Stack<RenderTargetsInfo> renderTargets = new Stack<RenderTargetsInfo>();

        int NumberOfRTs { get { return renderTargets.Count == 0 ? 1 : renderTargets.Peek().RenderTargets.Length; } }

        private RenderTargetsInfo CreateTargetsInfo(TextureBuffer[] targets)
        {
            if (targets.Length == 0)
                return backBufferRenderTarget;

            CheckBuffers(targets);

            TextureBuffer[] availableRTs = new TextureBuffer[Math.Min(targets.Length, device.Capabilities.SimultaneousRTCount)];

            int count = 0;
            foreach (var rt in targets)
            {
                availableRTs[count++] = rt;
            }

            Surface[] surfaces = new Surface[availableRTs.Length];

            for (int i = 0; i < availableRTs.Length; i++)
            {
                surfaces[i] = Surface.CreateRenderTarget(device, availableRTs[i].Width, availableRTs[i].Height, Direct3D9Tools.GetPixelFormat(availableRTs[i].InnerElementType), MultisampleType.None, 0, true);
            }

            Surface depthSurface = Surface.CreateDepthStencil(device, targets[0].Width, targets[0].Height, Format.D24S8, MultisampleType.None, 0, false);

            return new RenderTargetsInfo { RenderTargets = surfaces, DestinationTextures = availableRTs, DepthSurface = depthSurface };
        }

        private void CheckBuffers(TextureBuffer[] targets)
        {
            int width = targets[0].Width ;
            int height = targets[0].Height ;

            foreach (var t in targets)
                if (t.Width != width || t.Height != height)
                    throw new ArgumentException("targets");
        }

        private void SetTargetsInfo(RenderTargetsInfo info)
        {
            if (info.DepthSurface != null)
                device.DepthStencilSurface = info.DepthSurface;

            for (int i = 0; i < info.RenderTargets.Length; i++)
                device.SetRenderTarget(i, info.RenderTargets[i]);

            ImageWidth = info.Width;
            ImageHeight = info.Height;

            device.Viewport = new Viewport(0, 0, ImageWidth, ImageHeight);
        }

        private void UpdateTextures(RenderTargetsInfo info)
        {
            for (int i = 0; i < info.RenderTargets.Length; i++)
                CopySurfaceToTexture(info.RenderTargets[i], info.DestinationTextures[i]);
        }

        private void CopySurfaceToTexture(Surface surface, TextureBuffer textureBuffer)
        {
            if (surface == null || textureBuffer == null)
                return;

            if (textureBuffer is CubeTextureBuffer)
            {
                var ct = (CubeTextureBuffer)textureBuffer;

                CubeTexture texture = ((Direct3DResourcesManager.CubeTextureBufferResourceOnDeviceManager)((Direct3DRender.Direct3DResourcesManager)this.Resources).GetManagerFor<CubeTextureBuffer>((CubeTextureBuffer)textureBuffer)).TextureBuffer;

                device.UpdateSurface(surface, texture.GetCubeMapSurface((CubeMapFace)ct.ActiveFace, 0));
            }
            else
            {
                Texture texture = (Texture)((Direct3DResourcesManager.TextureBufferResourceOnDeviceManager)((Direct3DRender.Direct3DResourcesManager)this.Resources).GetManagerFor<TextureBuffer>(textureBuffer)).TextureBuffer;

                Surface offscreen = Surface.CreateOffscreenPlain(device, texture.GetLevelDescription(0).Width, texture.GetLevelDescription(0).Height, Direct3D9Tools.GetPixelFormat(textureBuffer.InnerElementType), Pool.SystemMemory);

                device.GetRenderTargetData(surface, offscreen);

                var data = offscreen.LockRectangle(LockFlags.None);
                var toData = texture.LockRectangle(0, LockFlags.None);

                toData.Data.WriteRange(data.Data.DataPointer, data.Data.Length);

                offscreen.UnlockRectangle();
                texture.UnlockRectangle(0);

                offscreen.Dispose();
            }
        }

        void ITextureRenderDevice.BeginScene(params TextureBuffer[] targets)
        {
            if (renderTargets.Count > 1)
                device.EndScene();

            renderTargets.Push(CreateTargetsInfo(targets));
            SetTargetsInfo(renderTargets.Peek());

            device.BeginScene();

            base.BeginScene();
        }

        Control control;
        SwapChain swapChain;

        public override void BeginScene()
        {
            ((ITextureRenderDevice)this).BeginScene(new TextureBuffer[0]);
        }

        public override void EndScene()
        {
            if (renderTargets.Count == 1)
                throw new InvalidOperationException("Trying to end a non-started scene.");

            base.EndScene();

            device.EndScene();

            var toRemove = renderTargets.Pop();

            if (toRemove != backBufferRenderTarget)
            {
                UpdateTextures(toRemove);
                toRemove.Dispose();
            }

            var toRestore = renderTargets.Peek();

            SetTargetsInfo(toRestore);

            if (renderTargets.Count > 1)
            {
                device.BeginScene();
            }

            if (renderTargets.Count == 1)
            {
                try
                {
                    swapChain.Present(Present.None);
                }
                catch (Direct3D9Exception e)
                {
                    if (e.ResultCode == global::SlimDX.Direct3D9.ResultCode.DeviceLost)
                    {
                        device.Dispose();
                        device = null;
                        return;
                    }
                }
            }
        }

        public bool IsCreated
        {
            get { return device != null; }
        }

        private PresentParameters GetPreferredParameters()
        {
            PresentParameters pp = new PresentParameters();

            pp.AutoDepthStencilFormat = Format.D24S8;
            pp.EnableAutoDepthStencil = true;
            pp.PresentFlags = PresentFlags.LockableBackBuffer;
            pp.Windowed = !fullScreen;
            if (fullScreen)
            {
                pp.DeviceWindowHandle = control.Handle;
                pp.BackBufferCount = 1;
                pp.BackBufferFormat = Format.X8B8G8R8;
                pp.BackBufferWidth = 1024;
                pp.BackBufferHeight = 768;
                pp.SwapEffect = SwapEffect.Discard;
                pp.Multisample = MultisampleType.None;
                pp.MultisampleQuality = 0;
            }
            else
            {
                pp.BackBufferCount = 1;
                pp.BackBufferFormat = Format.A8R8G8B8;
                pp.BackBufferWidth = control.Width;
                pp.BackBufferHeight = control.Height;
                pp.SwapEffect = SwapEffect.Discard;
                pp.DeviceWindowHandle = control.Handle;
            }

            return pp;
        }

        public void CreateDevice(Control control)
        {
            if (this.control != null && this.control != control)
                throw new InvalidOperationException();

            if (this.control == control)
                return;

            this.control = control;

            this.control.Resize += new EventHandler(control_Resize);

            try
            {
                device = new Device(new Direct3D(), 0, preferred, control.Handle, CreateFlags.HardwareVertexProcessing, GetPreferredParameters());

                control_Resize(null, null);

                if (Created != null)
                    Created(this, EventArgs.Empty);
            }
            catch (Direct3D9Exception e)
            {
            }
        }

        void control_Resize(object sender, EventArgs e)
        {
            if (control.ClientSize.Width > 0 && control.ClientSize.Height > 0)
            {
                ImageWidth = control.ClientSize.Width;
                ImageHeight = control.ClientSize.Height;

                foreach (var rt in renderTargets) rt.Dispose();

                renderTargets.Clear();

                swapChain = new SwapChain(device, GetPreferredParameters());

                var bb = swapChain.GetBackBuffer(0);
                var db = Surface.CreateDepthStencil(device, bb.Description.Width, bb.Description.Height, Format.D24S8, MultisampleType.None, 0, false);

                device.SetRenderTarget(0, bb);
                device.DepthStencilSurface = db;

                backBufferRenderTarget = new RenderTargetsInfo { DestinationTextures = new TextureBuffer[] { null }, RenderTargets = new Surface[] { device.GetRenderTarget(0) }, DepthSurface = device.DepthStencilSurface };
                renderTargets.Push(backBufferRenderTarget);
            }
        }

        public event EventHandler Created;

        #region IComponent Members

        public event EventHandler Disposed;

        public System.ComponentModel.ISite Site
        {
            get;
            set;
        }

        #endregion

        public override void Dispose()
        {
            base.Dispose();

            foreach (var rt in renderTargets)
                rt.Dispose();

            renderTargets = new Stack<RenderTargetsInfo>();

            this.Resources.Dispose();
            this.RenderStates.Dispose();

            if (device != null)
                device.Dispose();

            if (Disposed != null)
                Disposed(this, EventArgs.Empty);

            device = null;
        }
    }
}
