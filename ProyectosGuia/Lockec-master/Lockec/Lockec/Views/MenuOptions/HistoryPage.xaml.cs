namespace Lockec.Views.MenuOptions
{

    using Lockec.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        private bool first;

        public HistoryPage()
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
                MainViewModel.GetInstance().ServiceSumVM.InitServiceDetailsCommand.Execute(null);
            }
            first = false;
        }
    }
}