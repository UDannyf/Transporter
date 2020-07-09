namespace Lockec.Views.Menu
{
    using SlideOverKit;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SlideUpMenuView : SlideMenuView
    {
        public SlideUpMenuView()
        {
            InitializeComponent();
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var height = mainDisplayInfo.Height/mainDisplayInfo.Density;

            this.HeightRequest = height - (0.3*height);
            this.IsFullScreen = true;
            this.MenuOrientations = MenuOrientation.BottomToTop;

            this.BackgroundColor = Color.Transparent;
            this.BackgroundViewColor = Color.Transparent;
                
        }

    }
}