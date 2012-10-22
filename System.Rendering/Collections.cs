using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace System.Rendering
{
    /// <summary>
    /// Represents a collection of T. This class can be used to represent lists that notifies collections changes.
    /// This class can be used to wrap native collections in API with the collections in render states. I.e. Lights collection, Textures, etc.
    /// </summary>
    /// <typeparam name="T">Type of each element in collection</typeparam>
    [TypeConverter (typeof (CollectionConverter))]
    [Serializable]
    public class Collection<T> : CollectionBase, IList<T>
    {
        /// <summary>
        /// Initializes a new instance of collection. 
        /// </summary>
        public Collection()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of collection with several items in it.
        /// </summary>
        /// <param name="collection">IEnumerable representing the initial items in collection</param>
        public Collection(IEnumerable<T> collection)
        {
            foreach (T t in collection)
                InnerList.Add(t);
        }
        
        /// <summary>
        /// Initializes a new instance of collection with several items in it.
        /// </summary>
        /// <param name="collection">IList representing the initial items in collection</param>
        public Collection(IList<T> collection)
            : this((IEnumerable<T>)collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of collection with several items in it.
        /// </summary>
        /// <param name="elements">Params array representing the initial items in collection</param>
        public Collection(params T[] elements)
            : this((IEnumerable<T>)elements)
        {
        }

        /// <summary>
        /// Initializes a new instance of collection with several items in it.
        /// </summary>
        /// <param name="elements">IEnumerable representing the initial items in collection</param>
        public Collection(IEnumerable elements)
        {
            foreach (T t in elements)
                InnerList.Add(t);
        }

        /// <summary>
        /// Initializes a collection with initial space for some items
        /// </summary>
        /// <param name="capacity">Initial capacity of the list</param>
        public Collection(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Event that ocurrs when item is inserted, added, removed or changed.
        /// </summary>
        public event CollectionChangeEventHandler Change;

        #region IList<T> Members

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the
        /// first occurrence within the range of elements in the System.Collections.Generic.List<T>
        /// that extends from the specified index to the last element.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.List&lt;T&gt;. The value
        /// can be null for reference types.
        /// </param>
        /// <returns>Index of element found.</returns>
        public int IndexOf(T item)
        {
            return InnerList.IndexOf(item);
        }

        /// <summary>
        /// Inserts an element into the System.Collections.Generic.List&lt;T&gt; at the specified
        /// index. This method rises Change event.
        /// </summary>
        /// <param name="index">The zero-based index at wich item should be inserted.</param>
        /// <param name="item">The object inserted. The value can be null for reference types.</param>
        public void Insert(int index, T item)
        {
            if (Change != null) Change(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
            InnerList.Insert(index, item);
        }

        /// <summary>
        /// Gets or sets the element at the specified index. When set, this method rise the Change event.
        /// </summary>
        /// <param name="index">The zero-based index of element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index]
        {
            get
            {
                return (T)InnerList[index];
            }
            set
            {
                if (Change != null) Change(this, new CollectionChangeEventArgs(CollectionChangeAction.Remove, InnerList[index]));
                InnerList[index] = value;
                if (Change != null) Change(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, InnerList[index]));
            }
        }

        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// Adds an element at the end of this list. This method rises Change event.
        /// </summary>
        /// <param name="item"></param>
        public virtual void Add(T item)
        {
            if (Change != null) Change(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
            InnerList.Add(item);
        }

        /// <summary>
        /// Remove last element of this list. This method rises Change event.
        /// </summary>
        public virtual void RemoveLast()
        {
            RemoveAt(this.Count - 1);
        }
        
        /// <summary>
        /// Determines if any item is in this list.
        /// </summary>
        /// <param name="item">Item to look for.</param>
        /// <returns>True if item is found in this list, false otherwise.</returns>
        public bool Contains(T item)
        {
            return InnerList.Contains(item);
        }

        /// <summary>
        /// Copies all elements in this object into an array starting in a position.
        /// </summary>
        /// <param name="array">Array to copy to.</param>
        /// <param name="arrayIndex">Start index to start copying.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            InnerList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets if this collection is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes some item from this list and returns if item was found.
        /// </summary>
        /// <param name="item">Item to be removed.</param>
        /// <returns>True if the item was found and removed, false otherwise.</returns>
        public bool Remove(T item)
        {
            bool b = InnerList.Contains(item);
            InnerList.Remove(item);
            if (b && Change != null)
                Change(this, new CollectionChangeEventArgs(CollectionChangeAction.Remove, item));
            return b;
        }

        #endregion

        #region IEnumerable<T> Members
        /// <summary>
        /// Returns a iterator of the items in this list.
        /// </summary>
        /// <returns>An IEnumerator object that iterates this list</returns>
        public new IEnumerator<T> GetEnumerator()
        {
            foreach (T t in InnerList)
                yield return t;
        }

        #endregion
    }
}
