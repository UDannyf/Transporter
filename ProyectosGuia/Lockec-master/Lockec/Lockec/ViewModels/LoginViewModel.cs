namespace Lockec.ViewModels
{
    using Lockec.Extras;
    using Lockec.Models;
    using Lockec.Services.Api_rest;
    using Lockec.Services.Sms;
    using Lockec.Views.Account;
    using Lockec.Views.Login;
    using Lockec.Views.Menu;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {

        private readonly ApiService apiService;
        private readonly SmsService smsService;

        private string _username;
        private string _phoneNumber;
        private string _verificationCode;
        private string _code;
        private bool _isRunning;
        private bool _isExistingUser;

        private Account _account;

        public string Username
        {
            get { return this._username; }
            set { SetValue(ref this._username, value); }
        }
        public string PhoneNumber
        {
            get { return this._phoneNumber; }
            set { SetValue(ref this._phoneNumber, value); }
        }
        public string VerificationCode
        {
            get { return this._verificationCode; }
            set { SetValue(ref this._verificationCode, value); }
        }
        public string Code
        {
            get { return this._code; }
            set { SetValue(ref this._code, value); }
        }
        public bool IsRunning
        {
            get { return this._isRunning; }
            set { SetValue(ref this._isRunning, value); }
        }
        public bool IsExistingUser
        {
            get { return this._isExistingUser; }
            set { SetValue(ref this._isExistingUser, value); }
        }
        public Account AccountInstance
        {
            get { return this._account; }
            set { SetValue(ref this._account, value); }
        }

        public ICommand NavigateToPageCommand
        {
            get
            {
                return new Command<Type>(async (page) => await NavigateToPage(page));
            }
        }
        public ICommand GoToCodePageCommand
        {
            get
            {
                return new Command(GoToCodePage);
            }
        }
        public ICommand LoginCommand
        {
            get
            {
                return new Command(Login);
            }
        }
        public ICommand LogoutCommand
        {
            get
            {
                return new Command(Logout);
            }
        }


        public LoginViewModel()
        {
            apiService = new ApiService();
            smsService = new SmsService();
        }


        private async void GoToCodePage()
        {


            if (string.IsNullOrEmpty(PhoneNumber))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa tu número de teléfono",
                    "Ok");
                return;
            }

            if (!EntryLengthValidator())
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa un número de teléfono válido",
                    "Ok");
                return;
            }

            foreach (var number in PhoneNumber)
            {
                if (number < '0' || number > '9')
                {
                    await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa un número de teléfono válido",
                    "Ok");
                    return;
                }
            }

            this.IsRunning = true;
            
            var connection = this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Debes conectarte a internet",
                    "Ok");
                return;
            }

            GenerateRandomCode();

            var response = await apiService.Get<Account>(Constants.ACCOUNT_SERVICE, PhoneNumber);
            if (!response.IsSuccess)
            {
                CreateAccountInstance();
                await apiService.Post<Account>(Constants.ACCOUNT_SERVICE, AccountInstance);
            }
            else
            {
                AccountInstance = ((Account)response.Result);
                AccountInstance.VerificationCode = Code;
                IsExistingUser = true;
                await apiService.Put<Account>(Constants.ACCOUNT_SERVICE, AccountInstance, AccountInstance.PhoneNumber);
            }
            Settings.PhoneSettings = PhoneNumber;
            await smsService.SendSms(AccountInstance.PhoneNumber, AccountInstance.VerificationCode);
            await Application.Current.MainPage.Navigation.PushModalAsync(new CodePage(), false);
            this.IsRunning = false;
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.VerificationCode))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa el código de verificación",
                    "Ok");
                return;
            }

            var connection = this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Debes conectarte a internet",
                    "Ok");
                return;
            }

            if (VerificationCode != Code)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Código de verificación incorrecto",
                   "Ok");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            if (!(IsExistingUser && !string.IsNullOrEmpty(AccountInstance.Email)))
            {
                mainViewModel.AccountVM = new AccountViewModel
                {
                    AccountInstance = AccountInstance,
                    PersonalAccountInstance = new PersonalAccount { Account = PhoneNumber },
                    EnterpriseAccountInstance = new EnterpriseAccount { Account = PhoneNumber }
                };

                await Application.Current.MainPage.Navigation.PushModalAsync(new AccountTypePage(), false);
            }
            else
            {
                var response = await apiService.Get<PersonalAccount>(Constants.PERSONAL_ACCOUNT, AccountInstance.PhoneNumber);
                //Verifico el tipo de cuenta, si es personal o empresarial
                if (!response.IsSuccess)
                {
                    response = await apiService.Get<EnterpriseAccount>(Constants.ENTERPRISE_ACCOUNT, AccountInstance.PhoneNumber);
                    EnterpriseAccount enterprise = ((EnterpriseAccount)response.Result);
                    Username = enterprise.EnterpriseName;
                    Settings.EnterpriseAccountSettings = JsonConvert.SerializeObject(enterprise);
                }
                else
                {
                    PersonalAccount personal = (PersonalAccount)response.Result;
                    Username = personal.Name;
                    Settings.PersonalAccountSettings = JsonConvert.SerializeObject(personal);
                }
                
                mainViewModel.MenuVM = new MenuViewModel
                {
                    AccountInstance = AccountInstance,
                    UserName = Username //Nombre de la cuenta
                };

                Settings.AccountSettings = JsonConvert.SerializeObject(AccountInstance);
                
                Settings.IsLoggedIn = true;
                await Application.Current.MainPage.Navigation.PushModalAsync(new MenuPage(), false);
            }
            
        }

        private async void Logout()
        {
            Settings.ClearAllData();

            await Application.Current.MainPage.Navigation.PushModalAsync(new LoginPage(), false);
            /*MainViewModel.GetInstance().LoginVM = new LoginViewModel();
            MainViewModel.GetInstance().AccountVM = new AccountViewModel();
            MainViewModel.GetInstance().MenuVM = new MenuViewModel();
            MainViewModel.GetInstance().MapVM = null;
            MainViewModel.GetInstance().ServiceVM = null; */
        }

        private async Task NavigateToPage(Type pageType)
        {
            if (!string.IsNullOrEmpty(VerificationCode)) {
                VerificationCode = string.Empty;
            }
            Page page = (Page)Activator.CreateInstance(pageType);
            await App.Current.MainPage.Navigation.PushModalAsync(page, false);
        }



        //METODOS QUE DEBERIAN IR EN ALGUN OTRO LUGAR
        private void CreateAccountInstance()
        {
            AccountInstance = new Account
            {
                PhoneNumber = PhoneNumber,
                Email = "",
                City = "",
                Address = "",
                VerificationCode = Code
            };
        }

        private bool EntryLengthValidator()
        {
            return this.PhoneNumber.Length == Constants.MAX_PHONE_LENGTH;
        }

        private void GenerateRandomCode()
        {
            int min = 1000;
            int max = 9999;
            Random code = new Random();
            Code = code.Next(min, max).ToString();
        }

    }
}
