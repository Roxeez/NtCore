using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Moonlight.Core.Collection
{
    public class InternalObservableHashSet<T> : IEnumerable<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        protected ThreadSafeHashSet<T> ThreadSafeInternalHashSet { get; } = new ThreadSafeHashSet<T>();

        public IEnumerator<T> GetEnumerator() => ThreadSafeInternalHashSet.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        internal void Add(T item)
        {
            bool added = ThreadSafeInternalHashSet.Add(item);

            if (!added)
            {
                return;
            }

            MoonlightAPI.Context?.Post(x =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            }, null);
        }

        internal void Remove(T item)
        {
            bool removed = ThreadSafeInternalHashSet.Remove(item);
            if (!removed)
            {
                return;
            }

            MoonlightAPI.Context?.Post(x =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            }, null);
        }

        internal void Clear()
        {
            ThreadSafeInternalHashSet.Clear();

            MoonlightAPI.Context?.Post(x =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }, null);
        }

        public bool Contains(T item) => ThreadSafeInternalHashSet.Contains(item);
    }
}