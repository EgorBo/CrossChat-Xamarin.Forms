using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Crosschat.Client.Seedwork.Controls
{
    public class BindableListView : ListView
    {
        public static BindableProperty ItemClickedCommandProperty = 
            BindableProperty.Create<BindableListView, ICommand>(o => o.ItemClickedCommand, default(ICommand));

        public ICommand ItemClickedCommand
        {
            get { return (ICommand) GetValue(ItemClickedCommandProperty); }
            set { SetValue(ItemClickedCommandProperty, value); }
        }

        public static BindableProperty ScrollableItemsSourceProperty =
            BindableProperty.Create<BindableListView, IEnumerable>(o => o.ScrollableItemsSource, default(IEnumerable), bindingPropertyChanged: OnItemsSourceChanged);

        private static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
        {
            var listView = bindable as BindableListView;
            if (listView == null)
                return;

            listView.ItemsSource = newvalue;
            if (newvalue is INotifyCollectionChanged)
            {
                ((INotifyCollectionChanged) newvalue).CollectionChanged += async (s, e) =>
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        var item = listView.ItemsSource.OfType<object>().FirstOrDefault();
                        listView.Layout(listView.Bounds);
                    }
                };
            }

        }

        public IEnumerable ScrollableItemsSource
        {
            get { return (IEnumerable)GetValue(ScrollableItemsSourceProperty); }
            set { SetValue(ScrollableItemsSourceProperty, value); }
        }

        public BindableListView()
        {
            ItemTapped += OnItemTapped;
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && ItemClickedCommand != null && ItemClickedCommand.CanExecute(e))
            {
                ItemClickedCommand.Execute(e.Item);
                SelectedItem = null; //we don't need the SelectedItem if we have binding to ItemClickedCommand
            }
        }
    }
}
