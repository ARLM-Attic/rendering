using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Rendering.Resourcing;
using System.Unsafe;
using System.Runtime.InteropServices;

namespace System.Rendering
{
    /// <summary>
    /// Defines an object that can be used as a graphic resource.
    /// </summary>
    public interface IGraphicResource : IAllocateable
    {
        /// <summary>
        /// Gets the rank of the space this resource is stored.
        /// </summary>
        int Rank { get; }

        /// <summary>
        /// Gets the length for a specific dimension.
        /// </summary>
        int GetLength(int dimension);

        /// <summary>
        /// Type of the elements in this resource.
        /// </summary>
        Type InnerElementType { get; }

        /// <summary>
        /// Gets the total number of elements in this resource.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// When implemented, gets an unidimensional array with all elements at certain area.
        /// Use this method when you want to get a read only portion of the resource.
        /// Array element type is same of inner element type.
        /// </summary>
        Array GetData(int[] start, int[] ranks);

        /// <summary>
        /// When implemented, sets an unidimensional array of elements in the resource starting at certain position.
        /// Use this method when you want to set a portion of them. Array element needs to match the inner element size of this resource.
        /// </summary>
        void SetData(Array data, int[] start, int[] ranks);
    }

    /// <summary>
    /// Defines possible memory locations for a resource.
    /// </summary>
    public enum Location
    {
        /// <summary>
        /// This resource is allocated in user memory and its not binded to any render or device.
        /// </summary>
        User,
        /// <summary>
        /// This resource is binded to a render but it is allocated in user memory.
        /// </summary>
        Render,
        /// <summary>
        /// This resource is binded to a render and allocated in a device memory.
        /// </summary>
        Device
    }

    /// <summary>
    /// Defines updates modes of data.
    /// </summary>
    public enum GraphicResourceUpdateMode
    {
        /// <summary>
        /// All data in destination receive values from copying data.
        /// If some field in destination data isnt belongs to copying fields, the default value is set.
        /// </summary>
        Replace,
        /// <summary>
        /// All data in destination receive values from copying data.
        /// If some field in destination data isnt belongs to copying fields, the value remains intact.
        /// </summary>
        Update,
        /// <summary>
        /// Use this mode for copying exactly the same format that internal data and gain performance.
        /// If format doesnt match, it throws an InvalidOperationException.
        /// </summary>
        Match
    }

    public static class GraphicResourceExtensors
    {
        /// <summary>
        /// Gets a copy of the entire resource data in a Array of elements T.
        /// </summary>
        public static Array GetData<T>(this IGraphicResource resource) where T:struct
        {
            return GetData<T>(resource, null, null);
        }

        /// <summary>
        /// Gets a copy of certain resource data region in a Array of elements T.
        /// </summary>
        public static T[] GetData<T>(this IGraphicResource resource, int[] start, int[] ranks) where T: struct
        {
            Array data = resource.GetData(start, ranks).Extract<T>();
            return (T[])data;
        }

        /// <summary>
        /// Gets a copy of the entire resource data in a Array.
        /// </summary>
        public static Array GetData(this IGraphicResource resource)
        {
            return GetData(resource, null, null).Pack (resource.GetRanks ());
        }

        /// <summary>
        /// Gets a copy of certain resource data region in a Array.
        /// </summary>
        public static Array GetData(this IGraphicResource resource, int[] start, int[] ranks)
        {
            return resource.GetData(start, ranks);
        }

        /// <summary>
        /// Gets an array with the ranks of this array.
        /// </summary>
        public static int[] GetRanks(this Array array)
        {
            int[] ranks = new int[array.Rank];
            for (int i = 0; i < ranks.Length; i++)
                ranks[i] = array.GetLength(i);
            return ranks;
        }

        /// <summary>
        /// Sets data into a resource in several ways, determined by mode parameter.
        /// </summary>
        /// <param name="start">This parameter can be omitted or null, but only if source is of same size than resource.</param>
        public static void SetData(this IGraphicResource resource, GraphicResourceUpdateMode mode, Array source, params int[] start)
        {
            int[] ranks = start == null ? null : source.GetRanks();
            if (start != null && start.Length == 0)
            {
                start = null;
                ranks = null;
            }

            switch (mode)
            {
                case GraphicResourceUpdateMode.Replace:
                    Array toReplace = resource.GetData(start, ranks);
                    toReplace.Initialize();
                    DataComponentExtensors.Copy(source, toReplace);
                    resource.SetData(toReplace, start, ranks);
                    break;
                case GraphicResourceUpdateMode.Update:
                    Array toUpdate = resource.GetData(start, ranks);
                    DataComponentExtensors.Copy(source, toUpdate);
                    resource.SetData(toUpdate, start, ranks);
                    break;
                case GraphicResourceUpdateMode.Match:
                    resource.SetData(source, start, ranks);
                    break;
            }
        }

        public static void Copy (this IGraphicResource resource, Array data, params int[] start)
        {
            SetData (resource, GraphicResourceUpdateMode.Match, data, start);
        }

        /// <summary>
        /// Copies all data in resource
        /// </summary>
        public static void Copy(this IGraphicResource resource, Array data)
        {
            SetData(resource, GraphicResourceUpdateMode.Match, data, null);
        }

        public static void Replace(this IGraphicResource resource, Array data, params int[] start)
        {
            SetData(resource, GraphicResourceUpdateMode.Replace, data, start);
        }

        /// <summary>
        /// Replaces all data in resource
        /// </summary>
        public static void Replace(this IGraphicResource resource, Array data)
        {
            SetData(resource, GraphicResourceUpdateMode.Replace, data, null);
        }

        public static void Update(this IGraphicResource resource, Array data, params int[] start)
        {
            SetData(resource, GraphicResourceUpdateMode.Update, data, start);
        }

        /// <summary>
        /// Updates full data in resource
        /// </summary>
        public static void Update(this IGraphicResource resource, Array data)
        {
            SetData(resource, GraphicResourceUpdateMode.Update, data, null);
        }

        /// <summary>
        /// Gets a clone of this resource stored in some render object.
        /// </summary>
        public static G Clone<G>(this G resource, IRenderDevice render) where G:IGraphicResource
        {
            if (render == null)
                return Clone<G>(resource);

            return render.ResourcesManager.Load<G>(resource.GetData ());
        }

        /// <summary>
        /// Gets a clone of this resource in user memory.
        /// </summary>
        public static G Clone<G>(this G resource) where G : IGraphicResource
        {
            return GraphicResource.Create<G>(resource.GetData(), null);
        }

        public static G Reference<G>(this G resource) where G : GraphicResource
        {
            return resource.Reference() as G;
        }

        /// <summary>
        /// Gets a clone of this resource, changing its data to be of type T and storing it in some render object.
        /// </summary>
        public static G Clone<G, T>(this IGraphicResource resource, IRenderDevice render) where G : IGraphicResource where T:struct
        {
            return render.ResourcesManager.Load<G>(resource.GetData<T>());
        }

        /// <summary>
        /// Gets a clone of this resource, changing its data to be of type T and storing in user memory.
        /// </summary>
        public static G Clone<G, T>(this G resource) where G : IGraphicResource where T:struct
        {
            return GraphicResource.Create<G>(resource.GetData<T>(), null);
        }

        /// <summary>
        /// Gets the ranks of this resource.
        /// </summary>
        public static int[] GetRanks(this IGraphicResource resource)
        {
            int[] size = new int[resource.Rank];
            for (int i = 0; i < size.Length; i++)
                size[i] = resource.GetLength(i);

            return size;
        }

        public static Array Pack(this Array array, params int[] ranks)
        {
            Array packed = Array.CreateInstance(array.GetType().GetElementType(), ranks);
            PointerManager.Copy(array, packed);
            return packed;
        }

        public static T[] Extract<T>(this Array array) where T : struct
        {
            T[] data = new T[array.Length];
            DataComponentExtensors.Copy(array, data);
            return data;
        }

        public static T[,] Extract<T>(this Array array, int width) where T : struct
        {
            if (array.Length % width != 0)
                throw new ArgumentException("width");

            T[,] data = new T[array.Length / width, width];
            DataComponentExtensors.Copy(array, data);
            return data;
        }

        public static T[, ,] Extract<T>(this Array array, int width, int height) where T : struct
        {
            if (array.Length % (width * height) != 0)
                throw new ArgumentException("width or heigt");
            T[,,] data = new T[array.Length / (width * height), height, width];
            DataComponentExtensors.Copy(array, data);
            return data;
        } 

        /// <summary>
        /// Gets a portion of the unidimensional data of this resource.
        /// </summary>
        public static T[] GetData<T>(this IGraphicResource resource, int start, int count) where T:struct
        {
            return GetData(resource, new int[] { start }, new int[] { count }).Extract<T>();
        }

        /// <summary>
        /// Gets a portion of the bidimensional data of this resource.
        /// </summary>
        public static T[,] GetData<T>(this IGraphicResource resource, int x, int y, int width, int height) where T : struct
        {
            return GetData(resource, new int[] { y, x }, new int[] { height, width }).Extract<T>(width);
        }

        /// <summary>
        /// Gets a portion of the tridimensional data of this resource.
        /// </summary>
        public static T[, ,] GetData<T>(this IGraphicResource resource, int x, int y, int front, int width, int height, int depth) where T : struct
        {
            return GetData(resource, new int[] { front, y, x }, new int[] { depth, height, width }).Extract<T>(width, height);
        }
    }
}
