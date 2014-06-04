using Xamarin.Forms;

namespace Crosschat.Client.Seedwork.Controls
{
    public class MvvmableTabbedPage : TabbedPage
    {
        private readonly ViewModelBase _viewModel;

        public MvvmableTabbedPage(ViewModelBase viewModel)
        {
            _viewModel = viewModel;
            BindingContext = viewModel;
            Title = "Cross chat";
        }

        protected override void OnAppearing()
        {
            _viewModel.OnAppearing(this);
            base.OnAppearing();
        }
    }
}