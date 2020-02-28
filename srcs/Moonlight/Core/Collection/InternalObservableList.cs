using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Moonlight.Core.Collection
{
    public class InternalObservableList<T> : IEnumerable<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        protected ThreadSafeList<T> ThreadSafeInternalList { get; } = new ThreadSafeList<T>();

        public int Count => ThreadSafeInternalList.Count;

        public IEnumerator<T> GetEnumerator() => ThreadSafeInternalList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        internal void Add(T item)
        {
            ThreadSafeInternalList.Add(item);

            MoonlightAPI.Context?.Post(x =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            }, null);
        }

        internal void Remove(T item)
        {
            bool removed = ThreadSafeInternalList.Remove(item);
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
            ThreadSafeInternalList.Clear();

            MoonlightAPI.Context?.Post(x =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }, null);
        }

        public bool Contains(T item) => ThreadSafeInternalList.Contains(item);
    }
}