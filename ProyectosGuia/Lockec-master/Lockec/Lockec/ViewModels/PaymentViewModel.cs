namespace Lockec.ViewModels
{
    using Lockec.Extras;
    using Lockec.Models;
    using Lockec.Services.Api_rest;
    using Lockec.Services.Payment;
    using Lockec.Views.Menu;
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class PaymentViewModel : BaseViewModel
    {

        private string _paymentURL;
        private bool _isLoading;
        private ServiceSummary _serviceSummary;
        public string PaymentURL
        {
            get { return _paymentURL; }
            set { SetValue(ref _paymentURL, value); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetValue(ref _isLoading, value); }
        }

        public ServiceSummary ServiceSummaryInstance
        {
            get { return _serviceSummary; }
            set { SetValue(ref _serviceSummary, value); }
        }

        public PaymentViewModel()
        {
            MakeRequestCommand = new Command(MakeRequest);
            //MakeRequestCommand.Execute(null);
        }

        public ICommand MakeRequestCommand { get; set; }

        private async void MakeRequest()
        {
            IsLoading = true;
            PaymentService paymentApi = new PaymentService();
            ApiService api = new ApiService();

            var account = MainViewModel.GetInstance().MenuVM.AccountInstance;
            var personalResponse = await api.Get<PersonalAccount>(Constants.PERSONAL_ACCOUNT, account.PhoneNumber);

            if (!personalResponse.IsSuccess)
            { 
            await Application.Current.MainPage.DisplayAlert(
                 "¡Ups!",
                 "Algo salió intentando recopilar tu información",
                 "Ok");
                await Application.Current.MainPage.Navigation.PushModalAsync(new MenuPage());
                return;
            }

            var personalAccount = (PersonalAccount)personalResponse.Result;

            var authentication = new Authentication(Key.PlaceToPayLogin, Key.PlaceToPaySecretKey);

            PaymentInfo paymentInfo = new PaymentInfo
            {
                buyer = new Buyer
                {
                    document = personalAccount.IdNumber,
                    documentType = "CI",
                    email = account.Email,
                    mobile = account.PhoneNumber,
                    name = personalAccount.Name,
                    surname = personalAccount.Lastname
                },
                payment = new Payment
                {
                    reference = ServiceSummaryInstance.service_name,
                    description = "LOCKEC",
                    amount = new Amount
                    {
                        currency = "USD",
                        total = ServiceSummaryInstance.service_cost
                    },

                },
                expiration = (DateTime.Now.AddMinutes(15)).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                ipAddress = GetLocalAddress(),
                returnUrl = "https://www.google.com/",
                userAgent = "Xamarin Forms Web View",
                paymentMethod = "",
                auth = new Auth
                {
                    login = Key.PlaceToPayLogin,
                    tranKey = authentication.getTranKey(),
                    nonce = authentication.getNonce(),
                    seed = authentication.getSeed()
                }
            };

            var paymentResponse = await paymentApi.Post<PaymentInfo, PaymentStatus>(paymentInfo);

            if (!paymentResponse.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Algo salió mal intentando generar el pago",
                    "Ok");
                await Application.Current.MainPage.Navigation.PushModalAsync(new MenuPage());
                return;
            }

            var payment = (PaymentStatus)paymentResponse.Result;
            PaymentURL = payment.processUrl;
            IsLoading = false;
            return;
        }
        private string GetLocalAddress()
        {
            var IpAddress = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault();
            if (IpAddress != null)
            {
                return IpAddress.ToString();
            }
            return "Could not locate IP Address";
        }

    }
}
