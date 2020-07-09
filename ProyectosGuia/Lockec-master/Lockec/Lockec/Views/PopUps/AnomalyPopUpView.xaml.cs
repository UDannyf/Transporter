namespace Lockec.Views.PopUps
{
    using Rg.Plugins.Popup.Pages;
    using Rg.Plugins.Popup.Services;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnomalyPopUpView : PopupPage
    {
        public AnomalyPopUpView()
        {
            InitializeComponent();
        }

        private void ImageButton_Clicked(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}