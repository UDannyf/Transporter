
namespace Lockec.Views.Services
{
    using Lockec.ViewModels;
    using Lockec.Views.Services.SearchPage;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustodyServicePage : ContentPage
    {
        public CustodyServicePage()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new SearchPlacesPage() { BindingContext = this.BindingContext }, false);
        }
    }
}