using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Maths;
using System.Rendering.RenderStates;
using System.Threading.Tasks;

namespace System.Rendering
{
    /// <summary>
    /// Defines extra functionallities for renders that support interaction with ray pickers.
    /// </summary>
    public interface IInteractiveRenderDevice : IImageRenderDevice
    {
        /// <summary>
        /// List of ray pickers used by this render.
        /// </summary>
        IList<IRayPicker> RayPickers { get; }

        /// <summary>
        /// Pushes a new ray listener to be notified by ray interactions.
        /// </summary>
        /// <param name="obj">RayListener object to be pushed.</param>
        void PushRayListener(IRayListener obj);

        /// <summary>
        /// Pops a ray listener from the stack.
        /// </summary>
        void PopRayListener();
    }


    /// <summary>
    /// Represents a RenderDevice over an image.
    /// </summary>
    public interface IImageRenderDevice : IRenderDevice
    {
        /// <summary>
        /// Gets the image with in pixels.
        /// </summary>
        int ImageWidth { get; }

        /// <summary>
        /// Gets the image height in pixels.
        /// </summary>
        int ImageHeight { get; }
    }

    /// <summary>
    /// Represents a RenderDevice that is able to render over several TextureBuffer
    /// </summary>
    public interface ITextureRenderDevice : IImageRenderDevice
    {
        /// <summary>
        /// Starts the rendering of a scene over a texture targets.
        /// </summary>
        /// <param name="depthBuffer">Can be null to identify the depth buffer is not needed.</param>
        void BeginScene(params TextureBuffer[] targets);
    }

    public interface IParallelRenderDevice : ITextureRenderDevice
    {
        Task DrawAsync(Action<IParallelRenderDevice> rendering);

        Task<TextureBuffer> DrawAsync(TextureBuffer texture, Action<IParallelRenderDevice> rendering);
    }

    /// <summary>
    /// Represents some extensions for an InteractiveRenders.
    /// </summary>
    public static class InteractiveRenderDeviceExtensors
    {
        class MethodWrapperAsRayListener : IRayListener
        {
            public MethodWrapperAsRayListener(Action<RayInteractionEventArgs> action)
            {
                this.action = action;
            }

            Action<RayInteractionEventArgs> action;

            public void Notify(RayInteractionEventArgs e)
            {
                action(e);
            }
        }

        public static void PushRayListener(this IInteractiveRenderDevice render, Action<RayInteractionEventArgs> action)
        {
            render.PushRayListener(new MethodWrapperAsRayListener(action));
        }

        public static void Hit(this IInteractiveRenderDevice render, Action<IInteractiveRenderDevice> drawing,
            Action<RayInteractionEventArgs> whenHit, Action<RayInteractionEventArgs> whenSkip)
        {
            render.PushRayListener((e) =>
            {
                if (e.IntersectInfo != null)
                    whenHit(e);
                else
                    whenSkip(e);
            });
            render.Draw(() => { drawing(render); },
                new Effect<FrameBufferState>(FrameBufferState.Disable),
                new Effect<DepthBufferState>(DepthBufferState.ReadOnly)
                );
            render.PopRayListener();
        }

        public static void Hit(this IInteractiveRenderDevice render, IModel model,
            Action<RayInteractionEventArgs> whenHit, Action<RayInteractionEventArgs> whenSkip)
        {
            render.Hit((r) => { r.Draw(model); }, whenHit, whenSkip);
        }
  
        public static void PushRayListener(this IInteractiveRenderDevice render, Action whenHit, Action whenSkip)
        {
            render.PushRayListener(new MethodWrapperAsRayListener((e) =>
            {
                if (e.IntersectInfo != null)
                    whenHit();
                else
                    whenSkip();
            }));
        }

        /// <summary>
        /// Gets the position in screen coordinates of a certain point in the world using current view and projection matrices at render.
        /// </summary>
        /// <param name="render"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector2 FromWorldToScreen(this IInteractiveRenderDevice render, Vector3 position)
        {
            var projection = render.RenderStateInfo.GetState<ProjectionState>().Matrix;
            var view = render.RenderStateInfo.GetState<ViewState>().Matrix;

            var p = new Vector4(position.X, position.Y, position.Z, 1);

            p = GMath.mul (p , GMath.mul (view , projection));

            return (Vector2)p.Homogeneous;
        }

        /// <summary>
        /// Gets the ray in world coordinates of a certain point in the screen. 
        /// The begining of the ray represents the position of the viewer in the scene.
        /// </summary>
        /// <param name="render"></param>
        /// <param name="screenCoordinates"></param>
        /// <returns></returns>
        public static Ray FromScreenToWorld(this IInteractiveRenderDevice render, Vector2<float> screenCoordinates)
        {
            int width = render.ImageWidth;
            int height = render.ImageHeight;

            int x = (int)screenCoordinates.X;
            int y = (int)screenCoordinates.Y;

            return Ray.LookingToScreen(x, y, width, height,
                render.RenderStateInfo.GetState<ProjectionState>().Matrix,
                render.RenderStateInfo.GetState<ViewState>().Matrix);
        }

        public static float GetAspectRatio(this IImageRenderDevice render)
        {
            return render.ImageHeight / (float)render.ImageWidth;
        }
    }

    public class RayInteractionEventArgs : EventArgs
    {
        public RayInteractionEventArgs(IRayPicker rayPicker, IntersectInfo info)
        {
            this.rayPicker = rayPicker;
            this.info = info;
        }

        IRayPicker rayPicker;

        public IRayPicker RayPicker
        {
            get { return rayPicker; }
        }

        IntersectInfo info;

        public IntersectInfo IntersectInfo
        {
            get { return info; }
        }
    }

    /// <summary>
    /// Defines an object that can be notified by a ray hit.
    /// This functionallity is meant to be used by render only, so... implements explicitly.
    /// </summary>
    public interface IRayListener
    {
        /// <summary>
        /// Method used to notify this object.
        /// </summary>
        /// <param name="e"></param>
        void Notify(RayInteractionEventArgs e);
    }

    /// <summary>
    /// Defines functionallities for a ray picker. 
    /// Pickers have property to identify the ray.
    /// </summary>
    public interface IRayPicker
    {
        /// <summary>
        /// Gets the actual state of the ray used to pick.
        /// </summary>
        Ray Ray { get; }
    }
}
