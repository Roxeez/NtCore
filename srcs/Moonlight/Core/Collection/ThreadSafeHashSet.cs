using System.Collections;
using System.Collections.Generic;

namespace Moonlight.Core.Collection
{
    public sealed class ThreadSafeHashSet<T> : ISet<T>
    {
        private readonly HashSet<T> _internalHashSet = new HashSet<T>();
        private readonly object _lock = new object();

        public IEnumerator<T> GetEnumerator()
        {
            lock (_lock)
            {
                return _internalHashSet.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<T>.Add(T item)
        {
            lock (_lock)
            {
                _internalHashSet.Add(item);
            }
        }

        public void UnionWith(IEnumerable<T> other)
        {
            lock (_lock)
            {
                _internalHashSet.UnionWith(other);
            }
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            lock (_lock)
            {
                _internalHashSet.IntersectWith(other);
            }
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            lock (_lock)
            {
                _internalHashSet.ExceptWith(other);
            }
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            lock (_lock)
            {
                _internalHashSet.SymmetricExceptWith(other);
            }
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            lock (_lock)
            {
                return _internalHashSet.IsSubsetOf(other);
            }
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            lock (_lock)
            {
                return _internalHashSet.IsSupersetOf(other);
            }
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            lock (_lock)
            {
                return _internalHashSet.IsProperSupersetOf(other);
            }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            lock (_lock)
            {
                return _internalHashSet.IsProperSubsetOf(other);
            }
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            lock (_lock)
            {
                return _internalHashSet.Overlaps(other);
            }
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            lock (_lock)
            {
                return _internalHashSet.SetEquals(other);
            }
        }

        bool ISet<T>.Add(T item)
        {
            lock (_lock)
            {
                return _internalHashSet.Add(item);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _internalHashSet.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (_lock)
            {
                return _internalHashSet.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_lock)
            {
                _internalHashSet.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(T item)
        {
            lock (_lock)
            {
                return _internalHashSet.Remove(item);
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _internalHashSet.Count;
                }
            }
        }

        public bool IsReadOnly => false;

        public bool Add(T item) => ((ISet<T>)this).Add(item);
    }
}