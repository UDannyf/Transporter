namespace Lockec.Views.MenuOptions
{

    using Lockec.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentsPage : ContentPage
    {
        public CommentsPage()
        {
            MainViewModel.GetInstance().ServiceSumVM = new ServiceSummaryViewModel();
            InitializeComponent();
        }
    }
}