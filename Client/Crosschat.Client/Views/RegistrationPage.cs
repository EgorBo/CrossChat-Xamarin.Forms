using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Controls;
using Crosschat.Client.Views.ValueConverters;
using Xamarin.Forms;

namespace Crosschat.Client.Views
{
    public class RegistrationPage : MvvmableContentPage
    {
        public RegistrationPage(ViewModelBase viewModel) : base(viewModel)
        {

            var countryPicker = new BindablePicker();
            countryPicker.Title = "Country";
            countryPicker.SetBinding(BindablePicker.ItemsSourceProperty, new Binding("Countries"));
            countryPicker.SetBinding(BindablePicker.SelectedItemProperty, new Binding("SelectedCountry", BindingMode.TwoWay));

            var sexPicker = new BindablePicker();
            sexPicker.Title = "Sex";
            sexPicker.SetBinding(BindablePicker.ItemsSourceProperty, new Binding("Sexes"));
            sexPicker.SetBinding(BindablePicker.SelectedItemProperty, new Binding("SelectedSex", BindingMode.TwoWay));

            var agePicker = new BindablePicker();
            agePicker.Title = "Age";
            agePicker.SetBinding(BindablePicker.ItemsSourceProperty, new Binding("Ages"));
            agePicker.SetBinding(BindablePicker.SelectedItemProperty, new Binding("SelectedAge", BindingMode.TwoWay));

            var header = new Label
                {
                    Text = "Registration",
                    Font = Font.BoldSystemFontOfSize(44),
                    HorizontalOptions = LayoutOptions.Center
                };

            var button = new Button();
            button.Text = "Register";
            button.SetBinding(IsEnabledProperty, new Binding("IsBusy", converter: new InverterConverter()));
            button.SetBinding(Button.CommandProperty, new Binding("RegisterCommand"));
            button.BackgroundColor = Color.Green;
            button.TextColor = Color.White;

            var nameEntry = new Entry
                {
                    Keyboard = Keyboard.Text,
                    Placeholder = "Nickname",
                };
            nameEntry.SetBinding(Entry.TextProperty, new Binding("Name", BindingMode.TwoWay));

            var passwordEntry = new Entry
                {
                    Keyboard = Keyboard.Text,
                    IsPassword = true,
                    Placeholder = "Password",
                };
            passwordEntry.SetBinding(Entry.TextProperty, new Binding("Password", BindingMode.TwoWay));


            // Accomodate iPhone status bar.
            Padding = new Thickness(10, Device.OnPlatform(iOS: 20, Android: 0, WinPhone: 0), 10, 5);

            Content = new StackLayout
                {
                    Children =
                        {
                            header,
                            nameEntry,
                            passwordEntry,
                            countryPicker,
                            sexPicker,
                            agePicker,
                            button
                        }
                };
        }
    }
}
