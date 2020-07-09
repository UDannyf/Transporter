namespace Lockec.Views.Menu
{
    using Lockec.ViewModels;
    using SlideOverKit;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : MenuContainerPage
    {
        public MenuPage()
        {
            MainViewModel.GetInstance().BarVM = new BarcodeViewModel();
            InitializeComponent();
            this.SlideMenu = new SlideUpMenuView();
        }

        private void SlideUpButton_Clicked(object sender, System.EventArgs e)
        {
            this.ShowMenu();
        }

        /*
        private void SlideDownButton_Clicked(object sender, System.EventArgs e)
        {
            this.HideMenu();
        }
        */
        

    }
}