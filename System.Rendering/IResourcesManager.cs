using System;
using System.Collections.Generic;
using System.Text;
using System.Unsafe;

namespace System.Rendering
{
    /// <summary>
    /// Defines functionallities of a resources manager of a render device.
    /// </summary>
    public interface IResourcesManager : IDisposable
    {
        /// <summary>
        /// Creates an internal resource of type R with elements of type T.
        /// Elements have its default values in its fields.
        /// </summary>
        /// <typeparam name="R">Resource Type</typeparam>
        /// <typeparam name="T">Type of the resource elements</typeparam>
        /// <param name="ranks">An array of int indicating the ranks of the spatial resource in each dimension</param>
        /// <returns>Allocated resource</returns>
        R Create<R, T>(params int[] ranks)
            where R : IGraphicResource
            where T : struct;
        
        /// <summary>
        /// Determines the type of support for certain resource.
        /// </summary>
        /// <typeparam name="R">Resource Type</typeparam>
        /// <returns>A SupportMode object indicating the type of support.</returns>
        SupportMode GetSupport<R>()
            where R : IGraphicResource;
    }

    public static class ResourcesManagerExtensors
    {
        /// <summary>
        /// Loads an array in a device and stored it as a resource.
        /// </summary>
        /// <typeparam name="R">Resource Type to return.</typeparam>
        /// <param name="manager">Manager used to create the resource.</param>
        /// <param name="data">Array of elements.</param>
        /// <returns>A Resource object.</returns>
        public static R Load<R>(this IResourcesManager manager, Array data) where R:IGraphicResource
        {
            int[] sizes = new int[data.Rank];
            for (int i = 0; i < sizes.Length; i++)
                sizes[i] = data.GetLength(i);
            R resource = (R) typeof(IResourcesManager).GetMethod("Create").MakeGenericMethod(typeof(R), data.GetType().GetElementType ()).Invoke(manager, new object[] { sizes });
            if (resource.InnerElementType == data.GetType().GetElementType())
                ((IGraphicResource)resource).SetData(GraphicResourceUpdateMode.Match, data, null);
            else
                ((IGraphicResource)resource).SetData(GraphicResourceUpdateMode.Replace, data, null);

            return resource;
        }
    }

    /// <summary>
    /// Defines different modes of support of a resource in memory.
    /// </summary>
    public enum SupportMode
    {
        /// <summary>
        /// The element is not supported by this render implementation.
        /// </summary>
        None,
        /// <summary>
        /// The element is supported as a UserMemory element in this render.
        /// </summary>
        User,
        /// <summary>
        /// The element is supported as an internal object in this render.
        /// </summary>
        Device
    }

    /// <summary>
    /// Defines functionallities for a specific resource data type manager.
    /// </summary>
    /// <typeparam name="RD">Resource data type</typeparam>
    public interface IResourceManagerOf<R> where R : IGraphicResource
    {
        /// <summary>
        /// Loads a resource in the render device.
        /// </summary>
        /// <param name="elementType">Type of the elements of the resource.</param>
        /// <param name="sizes">Sizes of the resource.</param>
        /// <returns>A Resource structure that identifies the loaded resource</returns>
        R Create(Type elementType, int[] sizes);

        /// <summary>
        /// When implemented, retrieves the specific manager for certain resource.
        /// </summary>
        /// <param name="resource">Resource to wrap with the manager.</param>
        /// <returns>A IResourceOnDeviceManager object that helps for common operations with a resource stored at device.</returns>
        IResourceOnDeviceManager GetManagerFor(R resource);

        /// <summary>
        /// Gets the support mode for this type of resources.
        /// </summary>
        SupportMode SupportMode { get; }
    }

    /// <summary>
    /// Defines functionallities to help with common operations with a resource at device.
    /// </summary>
    public interface IResourceOnDeviceManager
    {
        /// <summary>
        /// Gets the rank of this resource at device.
        /// </summary>
        int Rank { get; }

        /// <summary>
        /// Gets the length for certain dimension.
        /// </summary>
        int GetLength(int dimension);

        /// <summary>
        /// When implemented, gets the total number of elements stored in this resource object.
        /// </summary>
        int Length
        {
            get;
        }

        /// <summary>
        /// Relases some resource from the device.
        /// </summary>
        void Release();

        /// <summary>
        /// Gets the type of this resource in device.
        /// </summary>
        Type ElementType { get; }

        /// <summary>
        /// Gets the render this resource is binded in.
        /// </summary>
        IRenderDevice Render { get; }
    }

    /// <summary>
    /// Defines this resource have remarkable performance locking and unlocking.
    /// </summary>
    public interface ILockableResourceOnDeviceManager : IResourceOnDeviceManager
    {
        /// <summary>
        /// Locks some region of the resource buffer and retrieves an array of elements.
        /// This array can be modified and will be used for update later with unlock.
        /// </summary>
        Array Lock(int[] start, int[] ranks);

        /// <summary>
        /// Unlocks the last array locked.
        /// </summary>
        void Unlock();
    }

    /// <summary>
    /// Defines this resource have remarkable performance getting and setting data.
    /// </summary>
    public interface ISettableResourceOnDeviceManager : IResourceOnDeviceManager
    {
        /// <summary>
        /// Gets an array with all elements at certain area.
        /// </summary>
        Array GetData(int[] start, int[] ranks);

        /// <summary>
        /// Sets an array as elements in the resource starting at certain position.
        /// </summary>
        void SetData(Array data, int[] start, int[] ranks);
    }

    public static class ResourceOnDeviceManagerExtensors
    {
        public static int[] GetRanks(this IResourceOnDeviceManager manager)
        {
            int[] ranks = new int[manager.Rank];
            for (int i = 0; i < ranks.Length; i++)
                ranks[i] = manager.GetLength(i);
            return ranks;
        }

        internal static Array GetDataImplementation(this ILockableResourceOnDeviceManager manager, int[] start, int[] count)
        {
            Array data = manager.Lock(start, count);
            manager.Unlock();
            return data;
        }

        internal static void SetDataImplementation(this ILockableResourceOnDeviceManager manager, Array data, int[] start, int[] ranks)
        {
            Array locked = manager.Lock(start ?? new int[data.Rank], ranks ?? data.GetRanks());
            PointerManager.Copy(data, locked);
            manager.Unlock();
        }
    }
}
