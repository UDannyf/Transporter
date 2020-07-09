namespace Lockec.Views.MenuOptions
{
    using Lockec.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationsPage : ContentPage
    {
        private bool first;
        public NotificationsPage()
        {
            MainViewModel.GetInstance().NotVM = new NotificationSummaryViewModel();
            InitializeComponent();
            first = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (first)
            {
                MainViewModel.GetInstance().NotVM.InitNotificationDetailsCommand.Execute(null);
            }
            first = false;
        }
    }
}