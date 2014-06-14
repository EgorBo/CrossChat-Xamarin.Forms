using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Crosschat.Client.Seedwork.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static ObservableCollection<NamedObservableCollection<TItem>> ToNamedObservableCollections<TItem>(this IEnumerable<TItem> items, Func<TItem, string> groupSelector)
        {
            //it's not quite efficient implementation, but it's enough for our purposes.
            return items
                .OrderBy(groupSelector)
                .GroupBy(groupSelector)
                .Select(i => new NamedObservableCollection<TItem>(i.Key, i))
                .ToObservableCollection();
        }

        public static ObservableCollection<TItem> ToObservableCollection<TItem>(this IEnumerable<TItem> source)
        {
            return new ObservableCollection<TItem>(source);
        }

        public static void AddRange<TItem>(this ObservableCollection<TItem> source, IEnumerable<TItem> items)
        {
            foreach (var item in items)
            {
                source.Add(item); // TODO: fire CollectionChanged only once
            }
        }

        /// <summary>
        /// Set one-way synchronization between two observale collections (TODO: use Rx)
        /// </summary>
        public static void SynchronizeWith<T, TR, TKey>(this ObservableCollection<T> source,
                                                  ObservableCollection<TR> destination,
                                                  Func<T, TR> creator,
                                                  Func<T, TKey> sourceKey,
                                                  Func<TR, TKey> destKey)
        {
            source.ForEach(item => destination.Add(creator(item)));
            source.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
                {
                    foreach (var item in e.NewItems.OfType<T>())
                    {
                        var projection = creator(item);
                        destination.Insert(0, projection);
                    }
                }
                if (e.Action == NotifyCollectionChangedAction.Remove && sourceKey != null && destKey != null)
                {
                    foreach (var item in e.OldItems.OfType<T>().Join(destination, sourceKey, destKey, (src, dst) => dst))
                    {
                        destination.Remove(item);
                    }
                }
            };
        }

        /// <summary>
        /// Set one-way synchronization between two observale collections
        /// </summary>
        public static void SynchronizeWith<T, TR>(this ObservableCollection<T> source,
                                                  ObservableCollection<TR> destination,
                                                  Func<T, TR> creator)
        {
            SynchronizeWith<T, TR, T>(source, destination, creator, null, null);
        }
    }
}
