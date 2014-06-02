using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
    }
}
