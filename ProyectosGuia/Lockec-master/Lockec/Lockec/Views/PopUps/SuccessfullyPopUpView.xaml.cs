namespace Lockec.Views.PopUps
{

    using Rg.Plugins.Popup.Pages;
    using Rg.Plugins.Popup.Services;
    using System;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuccessfullyPopUpView : PopupPage
    {
        public SuccessfullyPopUpView()
        {
            InitializeComponent();
        }

        private void ImageButton_Accept_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

    }
}