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

        public BindableListView()
        {
            ItemTapped += OnItemTapped;
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && ItemClickedCommand != null && ItemClickedCommand.CanExecute(e))
            {
                ItemClickedCommand.Execute(e.Item);
            }
        }
    }
}
