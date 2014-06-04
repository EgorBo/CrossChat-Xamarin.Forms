using Xamarin.Forms;

namespace Crosschat.Client.Seedwork.Controls
{
    public class MvvmableContentPage : ContentPage
    {
        private readonly ViewModelBase _viewModel;

        public MvvmableContentPage(ViewModelBase viewModel)
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
