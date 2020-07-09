namespace Lockec.Views.PopUps
{
    using Lockec.Views.Menu;
    using Rg.Plugins.Popup.Pages;
    using Rg.Plugins.Popup.Services;
    using System;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EfectivePaymentPopUpView : PopupPage
    {
        public EfectivePaymentPopUpView()
        {
            InitializeComponent();
        }

        private void ImageButton_Exit_Clicked(object sender, EventArgs e)
        {

            PopupNavigation.Instance.PopAsync();
        }

        private void Button_Pay_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
            //PopupNavigation.Instance.PushAsync(new SuccessfullyPopUpView());
        }

    }
}