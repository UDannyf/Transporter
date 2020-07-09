namespace Lockec.Views.CreditCard
{
    using Lockec.ViewModels;
    using Lockec.Views.Menu;
    using Lockec.Views.PopUps;
    using Rg.Plugins.Popup.Services;
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MakePaymentPage : ContentPage
    {
        public MakePaymentPage()
        {
            InitializeComponent();
        }

        private async void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.StartsWith("https://www.google.com"))
            {
                e.Cancel = true;
                await Navigation.PushModalAsync(new MenuPage());
            }
        }

    }
}