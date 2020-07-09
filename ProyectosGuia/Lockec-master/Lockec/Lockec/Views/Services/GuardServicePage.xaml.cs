namespace Lockec.Views.Services
{
    using Lockec.Views.Services.SearchPage;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GuardServicePage : ContentPage
    {
        public GuardServicePage()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new SearchPlacePage() { BindingContext = this.BindingContext }, false);
        }
    }
}