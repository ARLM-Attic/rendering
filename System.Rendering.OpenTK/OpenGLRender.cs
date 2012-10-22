using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using OpenTK.Graphics.OpenGL;
using System.Rendering.Forms;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using OpenTK.Graphics;
using OpenTK.Platform;

namespace System.Rendering.OpenGL
{
    public partial class OpenGLRender : RenderBase, IControlRenderDevice, ITextureRenderDevice
    {
        public OpenGLRender()
            : base()
        {

        }

        static object SYNC_OBJECT = new object();

        static GraphicsContext currentRenderingContext =  null;

        public void SyncExecute(Action a)
        {
            lock (SYNC_OBJECT)
            {
                if (this.renderingContext != currentRenderingContext)
                    MakeCurrent();
                a();
            }
        }

        #region Private Fields
        private GraphicsContext renderingContext = null;                      // Rendering context

        IntPtr control;
        Control __control;

        private bool fullScreen = false;
        IWindowInfo Info;
        #endregion Private Fields

        #region Public Properties

        public bool FullScreen
        {
            get { return fullScreen; }
            set { fullScreen = value; }
        }


        #endregion Public Properties

        #region DestroyContexts()
        public void DestroyContexts()
        {
            if (renderingContext != null)
            {
                renderingContext.Dispose();
                renderingContext = null;
            }
        }
        #endregion DestroyContexts()

        #region CreateDevice(IntPtr hWnd)
        /// <summary>
        ///     Creates the OpenGL contexts.
        /// </summary>
        public void CreateDevice(Control __control)
        {
            if (this.__control == __control)
                return;
            this.control = __control.Handle;
            this.__control = __control;
            this.Info = Utilities.CreateWindowsWindowInfo(__control.Handle);
            renderingContext = new GraphicsContext(GraphicsMode.Default, Info);

            renderingContext.VSync = false;

            MakeCurrent();
            
            ((IGraphicsContextInternal)renderingContext).LoadAll();

            __control.Resize += new EventHandler(__control_Resize);

            __control_Resize(null, null);
            
            if (renderingContext != null)
            {
                if (Created != null)
                    Created(this, EventArgs.Empty);
            }
        }

        void __control_Resize(object sender, EventArgs e)
        {
            if (__control.ClientSize.Width > 0 && __control.ClientSize.Height > 0)
            {
                ImageWidth = __control.ClientSize.Width;
                ImageHeight = __control.ClientSize.Height;
                GL.Viewport(0, 0, ImageWidth, ImageHeight);

                foreach (var rt in renderTargets) rt.Dispose();

                renderTargets.Clear();

                backBufferRenderTarget = new RenderTargetsInfo { Width = ImageWidth, Height = ImageHeight, FB = 0, DepthSurface = 0, DestinationTextures = new TextureBuffer[] { null } };
                renderTargets.Push(backBufferRenderTarget);
            }
        }
        #endregion CreateDevice()

        #region MakeCurrent()
        public void MakeCurrent()
        {
            this.renderingContext.MakeCurrent(Info);
            currentRenderingContext = this.renderingContext;
        }
        #endregion MakeCurrent()

        #region SwapBuffers()
        public void SwapBuffers()
        {
            renderingContext.SwapBuffers();
        }
        #endregion SwapBuffers()

        protected override ResourcesManagerBase CreateResourcesManager()
        {
            return new OpenGLResourcesManager(this);
        }

        protected override TessellatorBase CreateTessellator()
        {
            return new OpenGLModelling(this);
        }

        protected override RenderStatesManagerBase CreateRenderStatesManager()
        {
            return new OpenGLRenderStates(this);
        }

        protected override RenderBase.ServicesManagerBase CreateServicesManager()
        {
            return new OpenGLServices(this);
        }

        class RenderTargetsInfo
        {
            public int FB;
            public TextureBuffer[] DestinationTextures;

            public int DepthSurface;

            public int Width; 
            public int Height;

            public void Dispose()
            {
                if (DepthSurface != 0)
                    GL.DeleteRenderbuffers(1, ref DepthSurface);

                if (FB != 0)
                {
                    GL.DeleteFramebuffers(1, ref FB);
                }
            }
        }

        RenderTargetsInfo backBufferRenderTarget;
        Stack<RenderTargetsInfo> renderTargets = new Stack<RenderTargetsInfo>();

        int NumberOfRTs { get { return renderTargets.Count == 0 ? 1 : renderTargets.Peek().DestinationTextures.Length; } }

        private void CheckBuffers(TextureBuffer[] targets)
        {
            int width = targets[0].Width ;
            int height = targets[0].Height;

            foreach (var t in targets)
                if (t.Width != width || t.Height != height)
                    throw new ArgumentException("targets");
        }

        private RenderTargetsInfo CreateTargetsInfo(TextureBuffer[] targets)
        {
            if (targets.Length == 0)
                return backBufferRenderTarget;

            CheckBuffers(targets);

            TextureBuffer[] availableRTs = new TextureBuffer[Math.Min(targets.Length, 4)];

            int count = 0;
            foreach (var rt in targets)
            {
                availableRTs[count++] = rt;
            }

            int[] surfaces = new int[availableRTs.Length];

            int fb = 0;
            GL.GenFramebuffers(1, out fb);

            int depthSurface = 0;
            GL.GenRenderbuffers(1, out depthSurface);

            return new RenderTargetsInfo
            {
                FB = fb,
                DestinationTextures = availableRTs,
                DepthSurface = depthSurface,
                Width = targets[0].Width,
                Height = targets[0].Height
            };
        }

        private void SetTargetsInfo(RenderTargetsInfo info)
        {
            var availableRTs = info.DestinationTextures;
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, info.FB);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, info.DepthSurface);

            for (int i = 0; i < availableRTs.Length; i++)
                if (availableRTs[i] != null)
                {
                    var man = ((OpenGLRender.OpenGLResourcesManager)Resources).GetManagerFor<TextureBuffer>(availableRTs[i]);
                    var manager = (OpenGLRender.OpenGLResourcesManager.TextureBufferResourceOnDeviceManager)man;

                    GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + i, TextureTarget.Texture2D, manager.TextureBuffer.ID, 0);
                }

            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent32, info.Width, info.Height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, info.DepthSurface);

            ImageWidth = info.Width;
            ImageHeight = info.Height;

            DrawBuffersEnum[] buffers = new DrawBuffersEnum[info.DestinationTextures.Length];
            for (int i = 0; i < buffers.Length; i++)
                buffers[i] = DrawBuffersEnum.ColorAttachment0 + i;
            GL.DrawBuffers(info.DestinationTextures.Length, buffers);

            GL.Viewport(0, 0, ImageWidth, ImageHeight);
        }

        void ITextureRenderDevice.BeginScene(params TextureBuffer[] targets)
        {
            SyncExecute(() =>
            {
                //if (renderTargets.Count > 1)
                //    GL.Flush();

                renderTargets.Push(CreateTargetsInfo(targets));
                SetTargetsInfo(renderTargets.Peek());
            });

            base.BeginScene();
        }

        public override void BeginScene()
        {
            ((ITextureRenderDevice)this).BeginScene(new TextureBuffer[0]);
        }

        public override void EndScene()
        {
            SyncExecute(() =>
            {
                if (renderTargets.Count == 1)
                    throw new InvalidOperationException("Trying to end a non-started scene.");

                base.EndScene();

                //GL.Flush();

                var toRemove = renderTargets.Pop();

                if (toRemove != backBufferRenderTarget)
                {
                    //UpdateTextures(toRemove);
                    toRemove.Dispose();
                }

                var toRestore = renderTargets.Peek();

                if (renderTargets.Count > 1)
                {
                }

                SetTargetsInfo(toRestore);

                if (renderTargets.Count == 1)
                {
                    SwapBuffers();
                }
            });
        }

        #region IHandledRenderBase Members

        public bool IsCreated
        {
            get { return renderingContext != null; }
        }

        public event EventHandler Created;

        #endregion

        public override void Dispose()
        {
            DestroyContexts();

            if (Disposed != null)
                Disposed(this, EventArgs.Empty);
        }

        #region IComponent Members

        public event EventHandler Disposed;

        ISite site;

        public ISite Site
        {
            get
            {
                return site;
            }
            set
            {
                site = value;
            }
        }

        #endregion
    }
}
