namespace Lockec.Views.PopUps
{
    using Rg.Plugins.Popup.Pages;
    using Rg.Plugins.Popup.Services;
    using System;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceRequestPopUpView : PopupPage
    {
        public ServiceRequestPopUpView()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            //await PopupNavigation.Instance.PushAsync(new PaymentPopUpView());

        }
    }
}