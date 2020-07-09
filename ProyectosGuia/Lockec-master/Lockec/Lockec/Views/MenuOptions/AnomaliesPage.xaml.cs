namespace Lockec.Views.MenuOptions
{
    using Lockec.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnomaliesPage : ContentPage
    {
        private bool first;
        public AnomaliesPage()
        {
            MainViewModel.GetInstance().ServiceSumVM = new ServiceSummaryViewModel();
            InitializeComponent();
            first = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (first)
            {
                MainViewModel.GetInstance().ServiceSumVM.InitServiceAnomalyCommand.Execute(null);
            }
            first = false;
        }
    }
}