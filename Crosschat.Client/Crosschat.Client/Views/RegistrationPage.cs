using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace Crosschat.Client.Views
{
    public class RegistrationPage : MvvmableContentPage
    {
        public RegistrationPage(ViewModelBase viewModel) : base(viewModel)
        {
            var countryPicker = new BindablePicker { Title = "Country code" };
            countryPicker.SetBinding(BindablePicker.ItemsSourceProperty, new Binding("Countries"));
            countryPicker.SetBinding(BindablePicker.SelectedItemProperty, new Binding("SelectedCountry", BindingMode.TwoWay));

            var header = new Label
                {
                    Text = "Registration",
                    Font = Font.BoldSystemFontOfSize(50),
                    HorizontalOptions = LayoutOptions.Center
                };

            var button = new Button { Text = "Register" };
            button.SetBinding(Button.CommandProperty, new Binding("RegisterCommand"));

            var inputEntry = new Entry
                {
                    Keyboard = Keyboard.Numeric,
                    Placeholder = "Enter number",
                };
            inputEntry.SetBinding(Entry.TextProperty, new Binding("Number", BindingMode.TwoWay));

            // Accomodate iPhone status bar.
            Padding = new Thickness(10, Device.OnPlatform(iOS: 20, Android: 0, WinPhone: 0), 10, 5);

            Content = new StackLayout
                {
                    Padding = new Thickness(30),
                    Children =
                        {
                            header,
                            inputEntry,
                            countryPicker,
                            button
                        }
                };
        }
    }
}
