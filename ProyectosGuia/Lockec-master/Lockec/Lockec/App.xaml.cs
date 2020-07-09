namespace Lockec
{
    using Lockec.Models;
    using Lockec.ViewModels;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Threading;
    using Xamarin.Forms;
    using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

    public partial class App : Xamarin.Forms.Application
    {

        public App()
        {
            Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize); //Para poder deslizar los layout cuando el teclado aparece
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");                     //Para tener los elementos visuales en español
            Services.Map.GoogleMapsApiService.Initialize(Extras.Key.GoogleMapsApiKey);          //Inicializa los servicios de Google Maps con su api key
        }


        protected override void OnStart()
        {
            //Handle when your app starts
            CheckLogIn();
            //Services.Firebase.MyFirebaseNotification.MyNotificationReceived();
            Services.Firebase.MyFirebaseNotification.MyNotificationOpened();
            

        }

        
        

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            CheckLogIn();
        }


        private void CheckLogIn()
        {
            if (Extras.Settings.IsLoggedIn)
            {
                var accountSettings = Extras.Settings.AccountSettings;
                var personalSettings = Extras.Settings.PersonalAccountSettings;

                var account = JsonConvert.DeserializeObject<Account>(accountSettings);
                var personal = JsonConvert.DeserializeObject<PersonalAccount>(personalSettings);

                MainViewModel.GetInstance().MenuVM.AccountInstance = account;
                MainViewModel.GetInstance().MenuVM.PersonalAccountInstance = personal;
                //MainViewModel.GetInstance().MenuVM.UserName = personal.Name;


                MainPage = new NavigationPage(new Views.Menu.MenuPage());
                //MainPage = new NavigationPage(new Views.CreditCard.MakePaymentPage());
                return;
            }
            else
            {
                MainPage = new NavigationPage(new Views.Login.LoginPage());

            }
        }

    }
}
