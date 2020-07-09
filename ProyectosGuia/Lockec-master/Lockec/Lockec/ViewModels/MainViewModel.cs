namespace Lockec.ViewModels
{

    public class MainViewModel
    {
        public LoginViewModel LoginVM { get; set; }
        public AccountViewModel AccountVM { get; set; }
        public MenuViewModel MenuVM { get; set; }
        public MapViewModel MapVM { get; set; }
        public ServiceViewModel ServiceVM { get; set; }
        public NotificationSummaryViewModel NotVM { get; set; }
        public ServiceSummaryViewModel ServiceSumVM { get; set; }
        public BarcodeViewModel BarVM { get; set; }
        public PaymentViewModel PayVM { get; set; }

        public MainViewModel()
        {
            instance = this;
            LoginVM = new LoginViewModel();
            AccountVM = new AccountViewModel();
            MenuVM = new MenuViewModel();
        }


        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }


    }
}
