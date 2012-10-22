using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace System.Rendering
{
    /// <summary>
    /// Define functionallities of a Render
    /// </summary>
    public interface IRenderDevice : IDisposable
    {
        #region Services Manager

        IRenderDeviceServices Services { get; }

        #endregion

        #region Global Resources Manager.

        /// <summary>
        /// Allows access to the manager of resources in this render object.
        /// </summary>
        IResourcesManager ResourcesManager { get; }

        #endregion

        #region Info objects

        /// <summary>
        /// Retrieves information about the render state during rendering process.
        /// </summary>
        IRenderStatesInfo RenderStateInfo { get; }

        /// <summary>
        /// Retrieves information about tesselletor object of this render.
        /// </summary>
        ITessellatorInfo TessellatorInfo { get; }

        #endregion

        #region Scene Methods

        /// <summary>
        /// When implemented begin a new rendered scene.
        /// </summary>
        void BeginScene();

        /// <summary>
        /// Performs final actions to present the rendered scene.
        /// </summary>
        void EndScene();

        #endregion

        #region Effects applying

        /// <summary>
        /// Apply a multipass effect. This method binds the effect to current render if it is not binded.
        /// If effect is currently binded to another render, this method throws an InvalidOperationException.
        /// </summary>
        /// <param name="effect">effect being applied</param>
        /// <returns>Enumerator to each pass of effect over render state</returns>
        /// <example>
        /// foreach (Pass p in render.Apply (myEffect))
        /// {
        ///     render.Draw (Models.Cube);
        /// }
        /// render.EndScope (myEffect);
        /// </example>
        [EditorBrowsable(EditorBrowsableState.Never)]
        IEnumerable<Pass> Apply(IEffect effect);

        /// <summary>
        /// Release the effect calling to its End method.
        /// </summary>
        /// <param name="effect">effect to release, note that it will throw an exception if effect is not the last applied effect without release</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void EndScope(IEffect effect);

        #endregion

        #region Drawing

        /// <summary>
        /// Draws a model in a render.
        /// </summary>
        /// <param name="model">An IModel object to be drawn</param>
        void Draw(IModel model);

        /// <summary>
        /// Draws a IDrawable structure.
        /// This method is for extensibility purpouses only and should not be used freely. Use other Draw overloads whenever you can.
        /// </summary>
        /// <param name="drawable">A IDrawable object to be drawn</param>
        void Draw(IDrawable drawable);

        #endregion
    }

    /// <summary>
    /// Defines this class can be drawn using a IRender object.
    /// </summary>
    public interface IDrawable
    {
    }
}