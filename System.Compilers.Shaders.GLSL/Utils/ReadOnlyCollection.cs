using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Utils
{
  public class List<T> : IList<T>
  {
    private List<T> elements;

    public List()
    {
      elements = new List<T>();
    }

    public List(IEnumerable<T> items)
    {
      if (items == null)
        throw new ArgumentNullException("items");
      elements = new List<T>(items);
    }

    #region IList<T> Members

    public int IndexOf(T item)
    {
      for (int i = 0; i < elements.Count; i++)
      {
        if (elements[i].Equals(item))
          return i;
      }
      return -1;
    }

    void IList<T>.Insert(int index, T item)
    {
      throw new InvalidOperationException("This collection is readonly");
    }

    void IList<T>.RemoveAt(int index)
    {
      throw new InvalidOperationException("This collection is readonly");
    }

    public T this[int index]
    {
      get
      {
        if (index < 0 || index >= elements.Count)
          throw new ArgumentOutOfRangeException("index");
        return elements[index];
      }
      set
      {
        throw new InvalidOperationException("This collection is readonly");
      }
    }

    #endregion

    #region ICollection<T> Members

    void ICollection<T>.Add(T item)
    {
      throw new InvalidOperationException("This collection is readonly");
    }

    void ICollection<T>.Clear()
    {
      throw new InvalidOperationException("This collection is readonly");
    }

    public bool Contains(T item)
    {
      return IndexOf(item) >= 0;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      elements.CopyTo(array, arrayIndex);
    }

    public int Count
    {
      get { return elements.Count; }
    }

    public bool IsReadOnly
    {
      get { return true; }
    }

    bool ICollection<T>.Remove(T item)
    {
      throw new InvalidOperationException("This collection is readonly");
    }

    #endregion

    #region IEnumerable<T> Members

    public IEnumerator<T> GetEnumerator()
    {
      return elements.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    #endregion
  }
}
