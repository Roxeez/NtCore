using System.Collections;
using System.Collections.Generic;

namespace Moonlight.Core.Collection
{
    public sealed class ThreadSafeDictionary<K, V> : IDictionary<K, V>
    {
        private readonly IDictionary<K, V> _internalDictionary = new Dictionary<K, V>();
        private readonly object _lock = new object();

        public V this[K key]
        {
            get
            {
                lock (_lock)
                {
                    return _internalDictionary[key];
                }
            }
            set
            {
                lock (_lock)
                {
                    _internalDictionary[key] = value;
                }
            }
        }

        public ICollection<K> Keys
        {
            get
            {
                lock (_lock)
                {
                    return _internalDictionary.Keys;
                }
            }
        }

        public ICollection<V> Values
        {
            get
            {
                lock (_lock)
                {
                    return _internalDictionary.Values;
                }
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _internalDictionary.Count;
                }
            }
        }

        public bool IsReadOnly => false;

        public bool ContainsKey(K key)
        {
            lock (_lock)
            {
                return _internalDictionary.ContainsKey(key);
            }
        }

        public void Add(K key, V value)
        {
            lock (_lock)
            {
                _internalDictionary.Add(key, value);
            }
        }

        public bool Remove(K key)
        {
            lock (_lock)
            {
                return _internalDictionary.Remove(key);
            }
        }

        public bool TryGetValue(K key, out V value)
        {
            lock (_lock)
            {
                return _internalDictionary.TryGetValue(key, out value);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _internalDictionary.Clear();
            }
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            lock (_lock)
            {
                return _internalDictionary.Contains(item);
            }
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            lock (_lock)
            {
                _internalDictionary.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            lock (_lock)
            {
                return _internalDictionary.Remove(item);
            }
        }

        public void Add(KeyValuePair<K, V> item)
        {
            lock (_lock)
            {
                _internalDictionary.Add(item);
            }
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            lock (_lock)
            {
                return _internalDictionary.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}