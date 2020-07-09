namespace Lockec.ViewModels
{
    using Lockec.Models;
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MenuViewModel : BaseViewModel
    {
        private string _username;
        private Account _account;
        private PersonalAccount _personalAccount;

        public string UserName
        {
            get { return this._username; }
            set { SetValue(ref this._username, value); }
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

        public ICommand NavigateCommand
        {
            get
            {
                return new Command<Type>(async (page) => await Navigate(page));
            }
        }

        public ICommand NavigateServiceCommand
        {
            get
            {
                return new Command<Type>(async (page) => await NavigateService(page));
            }
        }

        private async Task Navigate(Type pageType)
        {
            if (pageType.IsInstanceOfType(new Views.Account.ProfilePage())) {
                MainViewModel.GetInstance().AccountVM.AccountInstance = AccountInstance;
                MainViewModel.GetInstance().AccountVM.PersonalAccountInstance = PersonalAccountInstance;
            }
            Page page = (Page)Activator.CreateInstance(pageType);
            await App.Current.MainPage.Navigation.PushModalAsync(page, false);
        }

        private async Task NavigateService(Type pageType)
        {
            MainViewModel.GetInstance().MapVM = new MapViewModel();
            MainViewModel.GetInstance().ServiceVM = new ServiceViewModel() { AccountInstance = AccountInstance };
            if (pageType.IsInstanceOfType(new Views.Services.GuardServicePage()) ||
                pageType.IsInstanceOfType(new Views.Services.DriverServicePage())) {
                MainViewModel.GetInstance().MapVM.isOneDirection = true;
            }
            Page page = (Page)Activator.CreateInstance(pageType);
            await App.Current.MainPage.Navigation.PushModalAsync(page, false);
        }

    }
}
