using System.Collections;
using System.Collections.Generic;

namespace Moonlight.Core.Collection
{
    public sealed class ThreadSafeList<T> : IList<T>
    {
        private readonly List<T> _internalList = new List<T>();
        private readonly object _lock = new object();

        public T this[int index]
        {
            get
            {
                lock (_lock)
                {
                    return _internalList[index];
                }
            }
            set
            {
                lock (_lock)
                {
                    _internalList[index] = value;
                }
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _internalList.Count;
                }
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            lock (_lock)
            {
                _internalList.Add(item);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _internalList.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (_lock)
            {
                return _internalList.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_lock)
            {
                _internalList.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(T item)
        {
            lock (_lock)
            {
                return _internalList.Remove(item);
            }
        }

        public int IndexOf(T item)
        {
            lock (_lock)
            {
                return _internalList.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (_lock)
            {
                _internalList.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (_lock)
            {
                _internalList.RemoveAt(index);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (_lock)
            {
                return _internalList.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}