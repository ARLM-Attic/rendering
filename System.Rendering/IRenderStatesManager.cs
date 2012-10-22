using System;
using System.Collections.Generic;
using System.Text;

namespace System.Rendering
{
    /// <summary>
    /// Defines functionallities for read or write states in a render.
    /// </summary>
    public interface IRenderStatesManager : IRenderStatesInfo, IDisposable
    {
        /// <summary>
        /// Saves the state of specific type
        /// </summary>
        /// <typeparam name="RS">Render state type</typeparam>
        void Save<RS>() where RS : struct;

        /// <summary>
        /// Restore the last saved state without restore of specific type
        /// </summary>
        /// <typeparam name="RS">Render state type</typeparam>
        void Restore<RS>() where RS : struct;
        
        /// <summary>
        /// Sets the state in the render device pipeline for a specific type
        /// </summary>
        /// <typeparam name="RS">Render state type</typeparam>
        /// <param name="state">Value of the state</param>
        void SetState<RS>(RS state) where RS : struct;

        /// <summary>
        /// Render object this render states manager is acting to.
        /// </summary>
        IRenderDevice Render { get; }
    }

    /// <summary>
    /// Defines functionallities for read-only access to render states.
    /// </summary>
    public interface IRenderStatesInfo
    {
        /// <summary>
        /// Retrieves the current state in render pipeline for a specific type.
        /// </summary>
        /// <typeparam name="RS">Render state type</typeparam>
        /// <returns>Struct with the value of render state</returns>
        RS GetState<RS>() where RS : struct;

        /// <summary>
        /// Indicates if a specific render state type is supportted by the render pipeline.
        /// </summary>
        /// <typeparam name="RS">Render state type</typeparam>
        /// <returns>True if render state type is supportted; false otherwise.</returns>
        bool IsSupported<RS>() where RS : struct;
    }

    /// <summary>
    /// Defines funcionallities for a specific render state setter.
    /// </summary>
    /// <typeparam name="RS"></typeparam>
    public interface IRenderStateSetterOf<RS> where RS : struct
    {
        RS State { set; }
    }

    /// <summary>
    /// Defines funcionallities for a specific render state manager.
    /// </summary>
    /// <typeparam name="RS">Render state type</typeparam>
    public interface IRenderStateManagerOf<RS> where RS:struct
    {
        /// <summary>
        /// Saves the state
        /// </summary>
        void Save ();

        /// <summary>
        /// Restore the previus saved without restore of render pipeline.
        /// </summary>
        void Restore();

        /// <summary>
        /// Gets or sets the state of specific type in the render pipeline.
        /// </summary>
        RS State { get; set;}
    }
}
