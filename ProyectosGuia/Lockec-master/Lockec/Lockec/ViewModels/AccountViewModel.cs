namespace Lockec.ViewModels
{
    using Lockec.Extras;
    using Lockec.Models;
    using Lockec.Services.Api_rest;
    using Lockec.Views.Account;
    using Lockec.Views.Menu;
    using Lockec.Views.PopUps;
    using Newtonsoft.Json;
    using Rg.Plugins.Popup.Services;
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class AccountViewModel : BaseViewModel
    {

        private bool _isChecked;
        private bool _isRunning;
        private Account _account;
        private readonly ApiService apiService;
        private PersonalAccount _personalAccount;
        private EnterpriseAccount _enterpriseAccount;

        private Image _image;

        public DateTime MyDate { get; set; } = DateTime.Now;

        public bool IsChecked
        {
            get { return this._isChecked; }
            set { SetValue(ref this._isChecked, value); }
        }
        public bool IsRunning
        {
            get { return this._isRunning; }
            set { SetValue(ref this._isRunning, value); }
        }

        public Image MyImage
        {
            get { return this._image; }
            set { SetValue(ref this._image, value); }
        }
        public Account AccountInstance
        {
            get { return this._account; }
            set { SetValue(ref this._account, value); }
        }
        public PersonalAccount PersonalAccountInstance
        {
            get { return this._personalAccount; }
            set { SetValue(ref this._personalAccount, value); }
        }
        public EnterpriseAccount EnterpriseAccountInstance
        {
            get { return this._enterpriseAccount; }
            set { SetValue(ref this._enterpriseAccount, value); }
        }


        public AccountViewModel()
        {
            apiService = new ApiService();
        }

        public ICommand NavigateToPageCommand
        {
            get
            {
                return new Command<Type>(async (page) => await NavigateToPage(page));
            }
        }

        public ICommand AccountConfirmationPageCommand
        {
            get
            {
                return new Command(AccountConfirmationPage);
            }
        }

        public ICommand EnterpriseConfirmationPageCommand
        {
            get
            {
                return new Command(EnterpriseConfirmationPage);
            }
        }

        public ICommand CreatePersonalAccountCommand
        {
            get
            {
                return new Command(CreatePersonalAccount);
            }
        }

        public ICommand CreateEnterpriseAccountCommand
        {
            get
            {
                return new Command(CreateEnterpriseAccount);
            }
        }

        public ICommand GoToServicesPageCommand
        {
            get
            {
                return new Command(GoToServicesPage);
            }
        }

        public ICommand ConditionsViewCommand
        {
            get
            {
                return new Command(ConditionsView);
            }
        }

        public ICommand UpdateAccountCommand
        {
            get
            {
                return new Command(UpdateAccount);
            }
        }

        private async void ConditionsView()
        {
            await PopupNavigation.Instance.PushAsync(new ConditionsPopUpView());
        }

        /*public ICommand SelectUserProfileCommand
{
   get
   {
       return new Command(SelectUserProfile);
   }
}*/

        private async Task NavigateToPage(Type pageType)
        {
            Page page = (Page)Activator.CreateInstance(pageType);
            await App.Current.MainPage.Navigation.PushModalAsync(page, false);
        }

        private async void UpdateAccount() 
        {
            var updateAccountResponse = await apiService.Put<Account>(Constants.ACCOUNT_SERVICE, AccountInstance, AccountInstance.PhoneNumber);
            //var updatePersonalResponse = await apiService.Put<PersonalAccount>(Constants.PERSONAL_ACCOUNT, PersonalAccountInstance, PersonalAccountInstance.Account);
            if (!(updateAccountResponse.IsSuccess)) {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Hubo un problema actualizando tu información. Inténtalo más tarde.",
                    "Ok");
                return;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Yeah!",
                    "Datos actualizados exitosamente",
                    "Ok");
                return;
            }
        }

        private async void GoToServicesPage()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.MenuVM = new MenuViewModel
            {
                AccountInstance = AccountInstance
            };
            //Verifico que tipo de cuenta es para obtener su nombre
            if (!string.IsNullOrEmpty(PersonalAccountInstance.Name))
            {
                mainViewModel.MenuVM.UserName = PersonalAccountInstance.Name;
            }
            if (!string.IsNullOrEmpty(EnterpriseAccountInstance.EnterpriseName))
            {
                mainViewModel.MenuVM.UserName = EnterpriseAccountInstance.EnterpriseName;
            }
            await Application.Current.MainPage.Navigation.PushModalAsync(new MenuPage(), false);

        }

        private async void AccountConfirmationPage()
        {

            if (string.IsNullOrEmpty(PersonalAccountInstance.Name))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa tu nombre",
                    "Ok");
                return;
            }

            if (string.IsNullOrEmpty(PersonalAccountInstance.Lastname))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa tu apellido",
                    "Ok");
                return;
            }

            if (string.IsNullOrEmpty(AccountInstance.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa tu correo electrónico",
                    "Ok");
                return;
            }

            if (string.IsNullOrEmpty(MyDate.Date.ToString()))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa tu fecha de nacimiento",
                    "Ok");
                return;
            }

            if (string.IsNullOrEmpty(PersonalAccountInstance.IdNumber))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa tu número de cédula",
                    "Ok");
                return;
            }

            if (ValidateID(PersonalAccountInstance.IdNumber.ToCharArray()))
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Ingresa un número de cédula válido.",
                   "Ok");
                return;
            }

            if (string.IsNullOrEmpty(AccountInstance.Address))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa tu dirección",
                    "Ok");
                return;
            }

            await Application.Current.MainPage.Navigation.PushModalAsync(new PersonalAccountConfirmationPage(), false);
        }

        private async void CreatePersonalAccount()
        {
            if (string.IsNullOrEmpty(AccountInstance.City))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Elige una ciudad",
                    "Ok");
                return;
            }

            if (!IsChecked)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Debes aceptar términos y condiciones",
                    "Ok");
                return;
            }
            PersonalAccountInstance.DoB = MyDate.ToString("yyyy-MM-dd");
            IsRunning = true;
            await apiService.Put<Account>(Constants.ACCOUNT_SERVICE, AccountInstance, AccountInstance.PhoneNumber);
            await apiService.Post<PersonalAccount>(Constants.PERSONAL_ACCOUNT, PersonalAccountInstance);

            //GUARDAMOS EN MEMORIA DEL DISPOSITIVO LA INFORMACION DE LA CUENTA
            Settings.AccountSettings = JsonConvert.SerializeObject(AccountInstance);
            Settings.PersonalAccountSettings = JsonConvert.SerializeObject(PersonalAccountInstance);
            Settings.IsLoggedIn = true;

            //AQUI
            GoToServicesPageCommand.Execute(null);
            //FIN AQUI
            IsRunning = false;
        }

        private async void EnterpriseConfirmationPage()
        {
            if (string.IsNullOrEmpty(EnterpriseAccountInstance.Ruc))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa RUC",
                    "Ok");
                return;
            }

            if (string.IsNullOrEmpty(EnterpriseAccountInstance.EnterpriseName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa el nombre de la empresa",
                    "Ok");
                return;
            }

            if (string.IsNullOrEmpty(EnterpriseAccountInstance.LegalRepresentativeName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa nombre de representante legal",
                    "Ok");
                return;
            }

            if (string.IsNullOrEmpty(AccountInstance.City))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Elige una ciudad",
                    "Ok");
                return;
            }

            if (string.IsNullOrEmpty(AccountInstance.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa tu correo electrónico",
                    "Ok");
                return;
            }

            if (string.IsNullOrEmpty(AccountInstance.Address))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa tu dirección",
                    "Ok");
                return;
            }

            await Application.Current.MainPage.Navigation.PushModalAsync(new EnterpriseAccountConfirmationPage(), false);

        }

        private async void CreateEnterpriseAccount()
        {
            if (string.IsNullOrEmpty(EnterpriseAccountInstance.ContactName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa el nombre de una persona de contacto",
                    "Ok");
                return;
            }

            if (string.IsNullOrEmpty(EnterpriseAccountInstance.ContactWorkPosition))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa el cargo de la persona de contacto",
                    "Ok");
                return;
            }

            if (!IsChecked)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Debes aceptar términos y condiciones",
                    "Ok");
                return;
            }
            IsRunning = true;
            await apiService.Put<Account>(Constants.ACCOUNT_SERVICE, AccountInstance, AccountInstance.PhoneNumber);
            await apiService.Post<EnterpriseAccount>(Constants.ENTERPRISE_ACCOUNT, EnterpriseAccountInstance);

            //GUARDAMOS EN MEMORIA DEL DISPOSITIVO LA INFORMACION DE LA CUENTA
            Settings.AccountSettings = JsonConvert.SerializeObject(AccountInstance);
            Settings.EnterpriseAccountSettings = JsonConvert.SerializeObject(EnterpriseAccountInstance);
            Settings.IsLoggedIn = true;

            CreateEnterpriseAccountCommand.Execute(null);
            IsRunning = false;
        }

        public bool ValidateID(char[] validarCedula)
        {
            int par = 0, impar = 0, verifi;
            int aux;
            for (int i = 0; i < 9; i += 2)
            {
                aux = 2 * int.Parse(validarCedula[i].ToString());
                if (aux > 9)
                    aux -= 9;
                par += aux;
            }
            for (int i = 1; i < 9; i += 2)
            {
                impar += int.Parse(validarCedula[i].ToString());
            }

            aux = par + impar;
            if (aux % 10 != 0)
            {
                verifi = 10 - (aux % 10);
            }
            else
                verifi = 0;
            if (verifi == int.Parse(validarCedula[9].ToString()))
                return true;
            else
                return false;
        }


        /*private async void SelectUserProfile()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {

                await Application.Current.MainPage.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }


            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
            });

            if (file == null)
            {
                return;
            }

            MyImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }*/

    }

}
