using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Rendering.Resourcing;
using System.Unsafe;

namespace System.Rendering
{
    /// <summary>
    /// Represents a base class for graphic resources.
    /// Use factory methods to create resources.
    /// </summary>
    [ComVisible(true)]
    public abstract class GraphicResource : IGraphicResource
    {
        #region Strategies

        /// <summary>
        /// This class visibility is for extensibility porpouses only.
        /// </summary>
        protected abstract class GraphicResourceStrategy : IGraphicResource
        {
            internal GraphicResourceStrategy() { }

            public abstract int Rank { get; }

            public abstract int Length { get; }

            public abstract int GetLength(int dimension);

            public abstract Type InnerElementType { get; }

            protected abstract Array InnerGetData(int[] start, int[] ranks);

            protected abstract void InnerSetData(Array data, int[] start, int[] ranks);

            public Array GetData(int[] start, int[] ranks)
            {
                return InnerGetData(start, ranks);
            }

            public void SetData(Array data, int[] start, int[] ranks)
            {
                InnerSetData(data, start, ranks);
            }

            public abstract Location Location { get; }

            public Resourcing.IAllocateable Clone(IRenderDevice render)
            {
                throw new InvalidOperationException("This method is not supposed to be invoked.");
            }

            public abstract IRenderDevice Render { get; }

            public abstract void Dispose();
        }

        #region User Memory Resource

        class UserGraphicResourceStrategy : GraphicResourceStrategy
        {
            internal Array data;
            IRenderDevice ownerRender;

            public UserGraphicResourceStrategy(Array data, IRenderDevice ownerRender)
            {
                this.data = data;
                this.ownerRender = ownerRender;
            }

            public override int Length
            {
                get {
                    return data.Length;
                }
            }
    
            public override int Rank
            {
                get { return data.Rank; }
            }

            public override int GetLength(int dimension)
            {
                return data.GetLength(dimension);
            }

            public override Type InnerElementType
            {
                get { return data.GetType().GetElementType(); }
            }

            protected override Array InnerGetData(int[] start, int[] ranks)
            {
                Array data;
                data = Array.CreateInstance(InnerElementType, ranks == null ? this.Length : ranks.Aggregate(1, (a, x) => a * x));

                if (start == null || (start.SequenceEqual(new int[Rank]) && ranks.SequenceEqual(this.GetRanks())))
                {   // full copy
                    PointerManager.Copy(this.data, data);
                }
                else
                {
                    Array src = GetFragment(start, ranks);
                    PointerManager.Copy(src, data);
                }
                return data;
            }

            protected override void InnerSetData(Array data, int[] start, int[] ranks)
            {
                if (start == null)
                    PointerManager.Copy(data, this.data);
                else
                {
                    Array toCopy = Array.CreateInstance(InnerElementType, ranks);
                    PointerManager.Copy(data, toCopy);
                    SetFragment(toCopy, start);
                }
            }
            
            #region Private Get and Set Fragments

            Array Get1(int start, int count)
            {
                Array result = Array.CreateInstance(this.data.GetType().GetElementType(), count);
                
                Array.Copy(data, start, result, 0, count);

                return result;
            }

            Array Get2(int r, int c, int nr, int nc)
            {
                return (Array)this.GetType().GetMethod("__Get2", Reflection.BindingFlags.NonPublic | Reflection.BindingFlags.Instance).
                    MakeGenericMethod(InnerElementType).Invoke(this, new object[] { r, c, nr, nc });
            }

            T[,] __Get2<T>(int r, int c, int nr, int nc)
            {
                T[,] currentData = (T[,])data;

                T[,] result = new T[nr, nc];

                for (int i = 0; i < nr; i++)
                    for (int j = 0; j < nc; j++)
                        result[i, j] = currentData[i + r, j + c];

                return result;
            }

            private Array GetFragment(int[] start, int[] size)
            {
                switch (start.Length)
                {
                    case 1: return Get1(start[0], size[0]);
                    case 2: return Get2(start[0], start[1], size[0], size[1]);
                    default:
                        Array result = Array.CreateInstance(this.data.GetType().GetElementType(), size);
                        CopyToByLevel(this.data, start, size, result, (int[])start.Clone(), new int[size.Length], 0);
                        return result;
                }
            }

            private void SetFragment(Array data, int[] start)
            {
                int[] size = new int[data.Rank];
                for (int i = 0; i < size.Length; i++)
                    size[i] = data.GetLength(i);
                CopyFromByLevel(this.data, start, size, data, (int[])start.Clone(), new int[size.Length], 0);
            }

            private static void CopyToByLevel(Array data, int[] start, int[] size, Array result, int[] indicesFrom, int[] indicesTo, int level)
            {
                if (level == size.Length - 1)
                {
                    for (int i = 0; i < size[level]; i++)
                    {
                        indicesTo[level] = i;
                        indicesFrom[level] = start[level] + i;
                        result.SetValue (data.GetValue(indicesFrom), indicesTo);
                    }
                }
                else
                {
                    for (int i = 0; i < size[level]; i++)
                    {
                        indicesTo[level] = i;
                        indicesFrom[level] = start[level] + i;
                        CopyToByLevel(data, start, size, result, indicesFrom, indicesTo, level + 1);
                    }
                }
            }

            private static void CopyFromByLevel(Array data, int[] start, int[] size, Array result, int[] indicesFrom, int[] indicesTo, int level)
            {
                if (level == size.Length - 1)
                {
                    for (int i = 0; i < size[level]; i++)
                    {
                        indicesTo[level] = i;
                        indicesFrom[level] = start[level] + i;
                        data.SetValue(result.GetValue(indicesTo), indicesFrom);
                    }
                }
                else
                {
                    for (int i = 0; i < size[level]; i++)
                    {
                        indicesTo[level] = i;
                        indicesFrom[level] = start[level] + i;
                        CopyFromByLevel(data, start, size, result, indicesFrom, indicesTo, level + 1);
                    }
                }
            }

            #endregion

            public override Location Location
            {
                get { return ownerRender != null ? Location.Render : Location.User; }
            }

            public override IRenderDevice Render
            {
                get { return ownerRender; }
            }

            public override void Dispose()
            {
                // do nothing in user memory, garbage collector will.
            }
        }

        #endregion

        #region Device Resource

        class DeviceGraphicResourceStrategy<R> : GraphicResourceStrategy where R: IGraphicResource
        {
            IResourceOnDeviceManager manager;

            public DeviceGraphicResourceStrategy(IResourceOnDeviceManager manager)
            {
                if (manager == null)
                    throw new ArgumentNullException();

                this.manager = manager;
            }

            public override int Rank
            {
                get { return manager.Rank; }
            }

            public override int GetLength(int dimension)
            {
                return manager.GetLength(dimension);
            }

            public override int Length
            {
                get { return this.GetRanks().Aggregate(1, (a, n) => n * a); }
            }

            public override Type InnerElementType
            {
                get { return manager.ElementType; }
            }

            protected override Array InnerGetData(int[] start, int[] ranks)
            {
                if (manager is ISettableResourceOnDeviceManager)
                    return ((ISettableResourceOnDeviceManager)manager).GetData(start, ranks);

                if (manager is ILockableResourceOnDeviceManager)
                    return ((ILockableResourceOnDeviceManager)manager).GetDataImplementation(start, ranks);

                throw new NotSupportedException("Manager should be implemented as ILockableResourceOnDeviceManager or ISettableResourceOnDeviceManager.");
            }

            protected override void InnerSetData(Array data, int[] start, int[] ranks)
            {
                if (manager is ISettableResourceOnDeviceManager)
                {
                    ((ISettableResourceOnDeviceManager)manager).SetData(data, start, ranks);
                    return;
                }

                if (manager is ILockableResourceOnDeviceManager)
                {
                    ((ILockableResourceOnDeviceManager)manager).SetDataImplementation(data, start, ranks);
                    return;
                }

                throw new NotSupportedException("Manager should be implemented as ILockableResourceOnDeviceManager or ISettableResourceOnDeviceManager.");
            }

            public override Location Location
            {
                get { return Rendering.Location.Device; }
            }

            public override IRenderDevice Render
            {
                get { return manager.Render; }
            }

            public override void Dispose()
            {
                manager.Release();
            }
        }

        #endregion

        #endregion

        GraphicResourceStrategy Strategy;

        protected internal Array DirectData
        {
            get
            {
                if (this.Location == Rendering.Location.User)
                    return ((UserGraphicResourceStrategy)Strategy).data;
                throw new InvalidOperationException("This call is only valid for user data allocated resources.");
            }
        }

        /// <summary>
        /// Initializes this graphic resource using some strategy.
        /// This constructor will be called via Reflection, thats the reason inherited classes must mantain this constructor protected.
        /// </summary>
        /// <param name="Strategy">The strategy pattern used to allow, user, render and devices resources uniformly.</param>
        protected GraphicResource (GraphicResourceStrategy Strategy)
        {
            this.Strategy = Strategy;
            this.Description = DataComponentExtensors.GetDescriptor(Strategy.InnerElementType);
        }

        public DataDescription Description
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the total number of elements in this resource.
        /// </summary>
        public int Length
        {
            get { return Strategy.Length; }
        }

        /// <summary>
        /// Gets the rank of data in this resource.
        /// </summary>
        public int Rank
        {
            get { return Strategy.Rank; }
        }

        /// <summary>
        /// Gets the dimension length of data in this resource.
        /// </summary>
        /// <param name="dimension">Dimension to request length</param>
        /// <returns>An integer with the size in certain dimension.</returns>
        public int GetLength(int dimension)
        {
            return Strategy.GetLength(dimension);
        }

        /// <summary>
        /// Gets the internal stored element type.
        /// </summary>
        public Type InnerElementType
        {
            get { return Strategy.InnerElementType; }
        }

        public static R Create<R>(Array data, IRenderDevice owner) where R : IGraphicResource
        {
            return Activating.CreateInstance<R>(new UserGraphicResourceStrategy(data, owner));
        }

        /// <summary>
        /// Creates a user memory resource from an array.
        /// </summary>
        /// <typeparam name="R">Type of resource to create.</typeparam>
        /// <typeparam name="T">Type of the elements in array.</typeparam>
        /// <returns>A Memory Graphic Resource wrapping current array.</returns>
        protected static R Create<R, T>(T[] data) where R : GraphicResource
        {
            return Create<R>((Array)data, null);
        }

        public Array GetData(int[] start, int[] ranks)
        {
            return Strategy.GetData(start, ranks);
        }

        public void SetData(Array data, int[] start, int[] ranks)
        {
            Strategy.SetData(data, start, ranks);

            Dirty();
        }

        public long Version { get; private set; }

        protected internal void Dirty()
        {
            Version++;
        }

        /// <summary>
        /// Creates a user memory resource from an array.
        /// </summary>
        /// <typeparam name="R">Type of resource to create.</typeparam>
        /// <typeparam name="T">Type of the elements in array.</typeparam>
        /// <returns>A Memory Graphic Resource wrapping current array.</returns>
        protected static R Create<R, T>(T[,] data) where R:GraphicResource
        {
            return Create<R>((Array)data, null);
        }
        /// <summary>
        /// Creates a user memory resource from an array.
        /// </summary>
        /// <typeparam name="R">Type of resource to create.</typeparam>
        /// <typeparam name="T">Type of the elements in array.</typeparam>
        /// <returns>A Memory Graphic Resource wrapping current array.</returns>
        protected static R Create<R, T>(T[, ,] data) where R:GraphicResource
        {
            return Create<R>((Array)data, null);
        }
        /// <summary>
        /// Creates a user memory resource with certain size.
        /// </summary>
        /// <typeparam name="R">Type of resource to create.</typeparam>
        /// <typeparam name="T">Type of the elements in resource.</typeparam>
        /// <param name="size">Sizes of the spatial resource in each dimension.</param>
        /// <returns>A Memory Graphic Resource wrapping an array.</returns>
        protected static R Empty<R, T>(params int[] size) where R : GraphicResource
        {
            return Create<R>(Array.CreateInstance(typeof(T), size), null);
        }

        /// <summary>
        /// Creates a resource in a render and loads the array into it.
        /// </summary>
        /// <typeparam name="R">Type of resource created.</typeparam>
        /// <typeparam name="T">Type of elements of the resource.</typeparam>
        /// <param name="data">Array with the loaded info.</param>
        /// <param name="render">Render where to store the resource.</param>
        /// <returns>A Resource Object. This resource will be allocated in render user memory or device memory.</returns>
        /// <exception cref="System.NotSupportedException">It is thrown if this resource isnt supported by the render.</exception>
        protected static R CreateResourceInRender<R, T>(T[] data, IRenderDevice render) where R : GraphicResource
        {
            return Create<R>((Array)data, render);
        }
        /// <summary>
        /// Creates a resource in a render and loads the array into it.
        /// </summary>
        /// <typeparam name="R">Type of resource created.</typeparam>
        /// <typeparam name="T">Type of elements of the resource.</typeparam>
        /// <param name="data">Array with the loaded info.</param>
        /// <param name="render">Render where to store the resource.</param>
        /// <returns>A Resource Object. This resource will be allocated in render user memory or device memory.</returns>
        /// <exception cref="System.NotSupportedException">It is thrown if this resource isnt supported by the render.</exception>
        protected static R CreateResourceInRender<R, T>(T[,] data, IRenderDevice render) where R : GraphicResource
        {
            return Create<R>((Array)data, render);
        }
        /// <summary>
        /// Creates a resource in a render and loads the array into it.
        /// </summary>
        /// <typeparam name="R">Type of resource created.</typeparam>
        /// <typeparam name="T">Type of elements of the resource.</typeparam>
        /// <param name="data">Array with the loaded info.</param>
        /// <param name="render">Render where to store the resource.</param>
        /// <returns>A Resource Object. This resource will be allocated in render user memory or device memory.</returns>
        /// <exception cref="System.NotSupportedException">It is thrown if this resource isnt supported by the render.</exception>
        protected static R CreateResourceInRender<R, T>(T[, ,] data, IRenderDevice render) where R : GraphicResource
        {
            return Create<R>((Array)data, render);
        }
        /// <summary>
        /// Creates a resource in a render of certain size.
        /// </summary>
        /// <typeparam name="R">Type of resource created.</typeparam>
        /// <typeparam name="T">Type of elements of the resource.</typeparam>
        /// <param name="render">Render where to store the resource.</param>
        /// <param name="size">Size of each dimension of the created resource.</param>
        /// <returns>A Resource Object. This resource will be allocated in render user memory or device memory.</returns>
        /// <exception cref="System.NotSupportedException">It is thrown if this resource isnt supported by the render.</exception>
        protected static R CreateResourceInRender<R, T>(IRenderDevice render, params int[] size) where R : GraphicResource
        {
            return Create<R>(Array.CreateInstance(typeof(T), size), render);
        }

        /// <summary>
        /// Creates an internal resource using some manager as wrapper. This method sould be only used by renders object.
        /// </summary>
        public static R CreateInternalResource<R>(IResourceOnDeviceManager manager) where R: IGraphicResource
        {
            return Activating.CreateInstance<R>(new DeviceGraphicResourceStrategy<R>(manager));
        }

        /// <summary>
        /// Gets the allocation of this resource.
        /// </summary>
        public Location Location
        {
            get { return Strategy.Location; }
        }

        public IRenderDevice Render
        {
            get
            {
                return Strategy.Render;
            }
        }

        internal bool needsToDispose = true;
        public void Dispose()
        {
            if (needsToDispose)
                Strategy.Dispose();
        }

        internal GraphicResource Reference()
        {
            GraphicResource result = this.MemberwiseClone() as GraphicResource;
            result.needsToDispose = false;
            return result;
        }

        public override bool Equals(object obj)
        {
            return obj is GraphicResource && ((GraphicResource)obj).Strategy == this.Strategy;
        }

        public override int GetHashCode()
        {
            return Strategy.GetHashCode();
        }

        public abstract Resourcing.IAllocateable Clone(IRenderDevice render);
    }
}
