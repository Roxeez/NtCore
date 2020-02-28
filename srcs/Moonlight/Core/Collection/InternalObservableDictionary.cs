using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Moonlight.Extensions;
using PropertyChanged;

namespace Moonlight.Core.Collection
{
    [SuppressPropertyChangedWarnings]
    public class InternalObservableDictionary<K, V> : IEnumerable<V>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        protected ThreadSafeDictionary<K, V> ThreadSafeInternalDictionary { get; } = new ThreadSafeDictionary<K, V>();

        public int Count => ThreadSafeInternalDictionary.Count;
        internal ICollection<V> Values => ThreadSafeInternalDictionary.Values;

        internal V this[K key]
        {
            get => ThreadSafeInternalDictionary[key];
            set
            {
                ThreadSafeInternalDictionary[key] = value;

                MoonlightAPI.Context?.Post(x =>
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
                }, null);
            }
        }

        public IEnumerator<V> GetEnumerator() => ThreadSafeInternalDictionary.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        internal void Add(K key, V value)
        {
            ThreadSafeInternalDictionary.Add(key, value);

            MoonlightAPI.Context?.Post(x =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
            }, null);
        }

        internal void Remove(K key)
        {
            if (!ThreadSafeInternalDictionary.TryGetValue(key, out V value))
            {
                return;
            }

            ThreadSafeInternalDictionary.Remove(key);
            MoonlightAPI.Context?.Post(x =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value));
            }, null);
        }

        internal void Clear()
        {
            ThreadSafeInternalDictionary.Clear();

            MoonlightAPI.Context?.Post(x =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }, null);
        }

        internal V GetValueOrDefault(K key) => ThreadSafeInternalDictionary.GetValueOrDefault(key);

        public bool ContainsKey(K key) => ThreadSafeInternalDictionary.ContainsKey(key);
    }
}