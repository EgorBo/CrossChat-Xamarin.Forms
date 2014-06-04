using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Crosschat.Client.Seedwork.Extensions
{
    public class NamedObservableCollection<TItem> : ObservableCollection<TItem>
    {
        public NamedObservableCollection(string name)
        {
            Name = name;
        }

        public NamedObservableCollection(string name, IEnumerable<TItem> items) : this(name)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public string Name { get; set; }
    }
}
