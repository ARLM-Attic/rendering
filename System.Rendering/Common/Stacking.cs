using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace System.Rendering
{
    public class Stacking
    {
        class __STACK<T>
        {
            public static readonly Dictionary<Stacking, T> current = new Dictionary<Stacking, T>();
            public static readonly Dictionary<Stacking, Stack<T>> stacks = new Dictionary<Stacking, Stack<T>>();
        }

        List<IDictionary> dictionaries = new List<IDictionary>();

        public bool IsCreated<T>()
        {
            lock (__STACK<T>.stacks)
                return __STACK<T>.stacks.ContainsKey(this);
        }

        public void Create<T>(T defaultValue)
        {
            lock (__STACK<T>.stacks)
            {
                if (!__STACK<T>.stacks.ContainsKey(this))
                {
                    __STACK<T>.stacks[this] = new Stack<T>();
                    dictionaries.Add(__STACK<T>.stacks);
                    __STACK<T>.stacks[this].Push(defaultValue);
                    __STACK<T>.current[this] = defaultValue;
                    dictionaries.Add(__STACK<T>.current);
                }
                else
                {
                    __STACK<T>.stacks[this] = new Stack<T>();
                    __STACK<T>.stacks[this].Push(defaultValue);
                    __STACK<T>.current[this] = defaultValue;
                }
            }
        }

        public void Dispose()
        {
            foreach (IDictionary d in dictionaries)
                d.Remove(this);
        }

        public void Push<T>()
        {
            lock (__STACK<T>.stacks)
            {
                __STACK<T>.stacks[this].Push(__STACK<T>.current[this]);
            }
        }

        public void Pop<T>()
        {
            lock (__STACK<T>.stacks)
            {
                __STACK<T>.current[this] = __STACK<T>.stacks[this].Pop();
            }
        }

        public T GetCurrent<T>()
        {
            lock (__STACK<T>.stacks)
            {
                return __STACK<T>.current[this];
            }
        }

        public void SetCurrent<T>(T value)
        {
            lock (__STACK<T>.stacks)
            {
                __STACK<T>.current[this] = value;
            }
        }

        public T Peek<T>()
        {
            lock (__STACK<T>.stacks)
            {
                return __STACK<T>.stacks[this].Peek();
            }
        }
    }
}
