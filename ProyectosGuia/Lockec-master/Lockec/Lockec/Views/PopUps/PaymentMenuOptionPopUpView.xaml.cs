namespace Lockec.Views.PopUps
{
    using Lockec.ViewModels;
    using Lockec.Views.CreditCard;
    using Rg.Plugins.Popup.Pages;
    using Rg.Plugins.Popup.Services;
    using System;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentMenuOptionPopUpView : PopupPage
    {
        public PaymentMenuOptionPopUpView()
        {
            InitializeComponent();
            
        }

        private void ImageButton_Exit_Clicked(object sender, EventArgs e)
        {

            PopupNavigation.Instance.PopAsync();
        }

        private void Button_Pay_Clicked(object sender, EventArgs e)
        {
            MainViewModel.GetInstance().PayVM.MakeRequestCommand.Execute(null);
            PopupNavigation.Instance.PopAsync();
            Navigation.PushModalAsync(new MakePaymentPage());
        }
    }
}