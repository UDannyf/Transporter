namespace Lockec.ViewModels
{
    using Lockec.Extras;
    using Lockec.Models;
    using Lockec.Services.Api_rest;
    using Lockec.Views.Menu;
    using Lockec.Views.PopUps;
    using Lockec.Views.Services;
    using Plugin.DeviceInfo;
    using Rg.Plugins.Popup.Services;
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Xamarin.Forms.GoogleMaps;

    public class ServiceViewModel : BaseViewModel
    {
        private string _totalDistance;
        private string _totalCost;
        private string _units;
        private string _startPlace;
        private string _endPlace;
        private bool _isRunning;
        private bool _isGuardDriver;
        private bool _isRentCar;
        private bool _hasLock;

        private double _serviceRate;

        public double ServiceRate
        {
            get{ return this._serviceRate; }
            set { SetValue(ref this._serviceRate, value); }
        }

        public bool IsRunning
        {
            get { return this._isRunning; }
            set { SetValue(ref this._isRunning, value); }
        }
        public string StartPlace
        {
            get { return this._startPlace; }
            set { SetValue(ref this._startPlace, value); }
        }
        public string EndPlace
        {
            get { return this._endPlace; }
            set { SetValue(ref this._endPlace, value); }
        }

        public bool IsGuardDriver
        {
            get { return this._isGuardDriver; }
            set { SetValue(ref this._isGuardDriver, value); }
        }

        public bool IsRentCar
        {
            get { return this._isRentCar; }
            set { SetValue(ref this._isRentCar, value); }
        }

        public bool HasLock
        {
            get { return this._hasLock; }
            set { SetValue(ref this._hasLock, value); }
        }

        private Map _map;
        private DateTime _date = DateTime.Now;
        private DateTime _endDate = DateTime.Now;
        private TimeSpan _startTime =  DateTime.Now.TimeOfDay;
        private TimeSpan _endTime = DateTime.Now.TimeOfDay;

        private Account _account;
        private ServiceDetails _serviceDetails;

        private ApiService apiService;

        public string TotalDistance
        {
            get { return this._totalDistance; }
            set { SetValue(ref this._totalDistance, value); }
        }
        public string TotalCost
        {
            get { return this._totalCost; }
            set { SetValue(ref this._totalCost, value); }
        }
        public string Units
        {
            get { return this._units; }
            set { SetValue(ref this._units, value); }
        }
        public Map Map
        {
            get { return this._map; }
            set { SetValue(ref this._map, value); }
        }
        public DateTime Date
        {
            get { return this._date; }
            set
            {
                if (value == _date)
                {
                    return;
                }
                SetValue(ref this._date, value);
                //OnPropertyChanged("Date");
            }
        }

        public DateTime EndDate
        {
            get { return this._endDate; }
            set
            {
                if (value == _endDate)
                {
                    return;
                }
                SetValue(ref this._endDate, value);
                //OnPropertyChanged("Date");
            }
        }

        public TimeSpan StartTime
        {
            get { return this._startTime; }
            set
            {
                if (value == _startTime)
                {
                    return;
                }
                SetValue(ref this._startTime, value);
                //OnPropertyChanged("StartTime");
            }
        }
        public TimeSpan EndTime
        {
            get { return this._endTime; }
            set
            {
                if (value == _endTime)
                {
                    return;
                }
                SetValue(ref this._endTime, value);
                //OnPropertyChanged("EndTime");
            }
        }
        
        public Account AccountInstance
        {
            get { return this._account; }
            set { SetValue(ref this._account, value); }
        }
        public ServiceDetails ServiceDetailsInstance
        {
            get { return this._serviceDetails; }
            set { SetValue(ref this._serviceDetails, value); }
        }

        public ServiceViewModel()
        {
            ServiceDetailsInstance = new ServiceDetails();
            apiService = new ApiService();
        }

        public ICommand RequestServiceCommand
        {
            get
            {
                return new Command(RequestService);
            }
        }
        public ICommand NavigateToPageCommand
        {
            get
            {
                return new Command<Type>(async (page) => await NavigateToPage(page));
            }
        }

        private async void RequestService()
        {
            if (string.IsNullOrEmpty(ServiceDetailsInstance.PaymentMethod))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Escoge método de pago",
                    "Ok");
                return;
            }
            ServiceDetailsInstance.Status = Constants.STATUS_PENDIENTE;
            var response = await apiService.Post<ServiceDetails>(Constants.SERVICES_DETAILS, ServiceDetailsInstance);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Algo salió mal, intenta nuevamente solicitar el servicio.",
                    "Ok");
                return;
            }
            await App.Current.MainPage.Navigation.PushModalAsync(new MenuPage());
            await PopupNavigation.Instance.PushAsync(new ServiceRequestPopUpView());
        }
        private async Task NavigateToPage(Type pageType)
        {
            Page page = (Page)Activator.CreateInstance(pageType);
            await App.Current.MainPage.Navigation.PushModalAsync(page, false);
        }

        public ICommand GoToCustodyCommand
        {
            get
            {
                return new Command(GoToCustody);
            }
        }

        public ICommand GoToDriverCommand
        {
            get
            {
                return new Command(GoToDriver);
            }
        }

        public ICommand GoToGuardCommand
        {
            get
            {
                return new Command(GoToGuard);
            }
        }

        public ICommand GoToTransporterCommand
        {
            get
            {
                return new Command(GoToTransporter);
            }
        }

        private void SetServiceData(string serviceType)
        {
            ServiceDetailsInstance.Date = Date.ToString("yyyy-MM-dd");
            ServiceDetailsInstance.StartTime = StartTime.ToString(@"hh\:mm\:ss");
            ServiceDetailsInstance.ReqService = serviceType;
            ServiceDetailsInstance.UserID = AccountInstance.PhoneNumber;
            ServiceDetailsInstance.DeviceID = CrossDeviceInfo.Current.Id;
        }

        private async Task<Service> GetServiceRate(string serviceType)
        {
            var serviceResponse = await apiService.Get<Service>(Constants.SERVICES, serviceType);
            return serviceResponse.IsSuccess ? (Service)serviceResponse.Result : null;
        
        }

        private async void GoToCustody()
        {
            if (string.IsNullOrEmpty(ServiceDetailsInstance.StartPlace))
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Ingresa dirección origen",
                   "Ok");
                return;
            }
            if (string.IsNullOrEmpty(ServiceDetailsInstance.EndPlace))
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Ingresa dirección destino",
                   "Ok");
                return;
            }
            if (Date.Date < DateTime.Now.Date)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Ingresa una fecha válida",
                   "Ok");
                return;
            }

            //var time = DateTime.Now.TimeOfDay.Add(new TimeSpan(0,30,0));

            DateTime myDateTime = new DateTime(Date.Year, Date.Month, Date.Day, StartTime.Hours, StartTime.Minutes, StartTime.Seconds);

            if (myDateTime.CompareTo(DateTime.Now.AddMinutes(30)) < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Pide el servicio al menos con 30 minutos de anticipación",
                   "Ok");
                return;
            }
            if (string.IsNullOrEmpty(Units))
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Ingresa número de unidades",
                   "Ok");
                return;
            }

            IsRunning = true;
            SetServiceData(Constants.CUSTODIA);
            ServiceDetailsInstance.EndTime = null;
            ServiceDetailsInstance.UnitNumber = Int32.Parse(Units);
            ServiceDetailsInstance.ReqLock = HasLock;
            var service = await GetServiceRate(Constants.CUSTODIA);
            if (service == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Parece que hay un problema con el servidor. Inténtalo más tarde.",
                   "Ok");
                IsRunning = false;
                return;
            }
            //var totalCost = ServiceRate * ServiceDetailsInstance.TotalDistance * ServiceDetailsInstance.UnitNumber;
            //ServiceDetailsInstance.TotalCost = Double.Parse(totalCost.ToString("#.##"));
           
            //TotalCost = "$" + ServiceDetailsInstance.TotalCost;
            await App.Current.MainPage.Navigation.PushModalAsync(new CustodyServiceDetailsPage(), false);
            IsRunning = false;
        }

        private async void GoToDriver()
        {
            if (string.IsNullOrEmpty(ServiceDetailsInstance.StartPlace))
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Ingresa dirección",
                   "Ok");
                return;
            }
            /*if (string.IsNullOrEmpty(ServiceDetailsInstance.EndPlace))
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Ingresa dirección destino",
                   "Ok");
                return;
            }*/
            ServiceDetailsInstance.EndPlace = ServiceDetailsInstance.StartPlace;

            if (Date.Date < DateTime.Now.Date || EndDate.Date < Date.Date)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Ingresa una fecha válida",
                   "Ok");
                return;
            }
            DateTime myStartDateTime = new DateTime(Date.Year, Date.Month, Date.Day, StartTime.Hours, StartTime.Minutes, StartTime.Seconds);

            if (myStartDateTime.CompareTo(DateTime.Now.AddMinutes(30)) < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Pide el servicio al menos con 30 minutos de anticipación.",
                   "Ok");
                return;
            }

            DateTime myEndDateTime = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds);
            if (myEndDateTime.CompareTo(myStartDateTime.AddHours(3)) < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "El servicio no puede durar menos de 3 horas.",
                   "Ok");
                return;
            }

            IsRunning = true;
            SetServiceData(Constants.CHOFER);
            ServiceDetailsInstance.UnitNumber = 1;
            ServiceDetailsInstance.EndTime = EndTime.ToString(@"hh\:mm\:ss");

            
            var service = await GetServiceRate(Constants.CHOFER);
            if (service == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Parece que hay un problema con el servidor. Inténtalo más tarde.",
                   "Ok");
                IsRunning = false;
                return;
            }
            //var totalCost = ServiceRate * ServiceDetailsInstance.TotalDistance;
            var duration = (myEndDateTime.Subtract(myStartDateTime)).TotalHours;
            var totalCost = service.ServicePrice(duration, IsGuardDriver, IsRentCar);
            ServiceDetailsInstance.TotalCost = Double.Parse(totalCost.ToString("#.##"));
            ServiceDetailsInstance.ReqGuard = IsGuardDriver;
            ServiceDetailsInstance.ReqVehicle = IsRentCar;
            TotalCost = "$" + ServiceDetailsInstance.TotalCost;
            await App.Current.MainPage.Navigation.PushModalAsync(new DriverServiceDetailsPage(), false);
            IsRunning = false;
        }

        private async void GoToGuard()
        {
            if (string.IsNullOrEmpty(ServiceDetailsInstance.StartPlace))
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Ingresa dirección",
                   "Ok");
                return;
            }
            ServiceDetailsInstance.EndPlace = ServiceDetailsInstance.StartPlace;
            if (Date.Date < DateTime.Now.Date || EndDate.Date < Date.Date)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Ingresa una fecha válida",
                   "Ok");
                return;
            }
            if (string.IsNullOrEmpty(Units))
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Ingresa número de unidades",
                   "Ok");
                return;
            }

            DateTime myStartDateTime = new DateTime(Date.Year, Date.Month, Date.Day, StartTime.Hours, StartTime.Minutes, StartTime.Seconds);
            if (myStartDateTime.CompareTo(DateTime.Now.AddMinutes(30)) < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                  "Pide el servicio al menos con 30 minutos de anticipación.",
                   "Ok");
                return;
            }

            DateTime myEndDateTime = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds);
            if (myEndDateTime.CompareTo(myStartDateTime.AddHours(3)) < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "El servicio no puede durar menos de 3 horas.",
                   "Ok");
                return;
            }

            IsRunning = true;
            SetServiceData(Constants.GUARDIA);
            ServiceDetailsInstance.UnitNumber = Int32.Parse(Units);
            ServiceDetailsInstance.EndTime = EndTime.ToString(@"hh\:mm\:ss");

            var duration = (myEndDateTime.Subtract(myStartDateTime)).TotalHours;

            var service = await GetServiceRate(Constants.GUARDIA);
            if (service == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Parece que hay un problema con el servidor. Inténtalo más tarde.",
                   "Ok");
                IsRunning = false;
                return;
            }
            //var totalCost = ServiceRate * Int32.Parse(Units) + (ServiceRate * duration);
            var totalCost = service.ServicePrice(duration, false, false) * Convert.ToInt32(Units);
            ServiceDetailsInstance.TotalCost = Double.Parse(totalCost.ToString("#.##"));
            
            TotalCost = "$" + ServiceDetailsInstance.TotalCost;
            await App.Current.MainPage.Navigation.PushModalAsync(new GuardServiceDetailsPage(), false);
            IsRunning = false;

        }

        private async void GoToTransporter()
        {
            if (string.IsNullOrEmpty(ServiceDetailsInstance.StartPlace))
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Ingresa dirección origen",
                   "Ok");
                return;
            }
            if (string.IsNullOrEmpty(ServiceDetailsInstance.EndPlace))
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Ingresa dirección destino",
                   "Ok");
                return;
            }
            if (Date.Date < DateTime.Now.Date || EndDate.Date < Date.Date)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Ingresa una fecha válida.",
                   "Ok");
                return;
            }

            DateTime myStartDateTime = new DateTime(Date.Year, Date.Month, Date.Day, StartTime.Hours, StartTime.Minutes, StartTime.Seconds);
            if (myStartDateTime.CompareTo(DateTime.Now.AddMinutes(30)) < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Pide el servicio al menos con 30 minutos de anticipación.",
                   "Ok");
                return;
            }

            DateTime myEndDateTime = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds);
            if (myEndDateTime.CompareTo(myStartDateTime.AddHours(3)) < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "El servicio no puede durar menos de 3 horas.",
                   "Ok");
                return;
            }

            IsRunning = true;
            SetServiceData(Constants.TRANSPORTER);
            ServiceDetailsInstance.UnitNumber = 1;
            ServiceDetailsInstance.EndTime = EndTime.ToString(@"hh\:mm\:ss");

            var service = await GetServiceRate(Constants.TRANSPORTER);
            if (service == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "Parece que hay un problema con el servidor. Inténtalo más tarde.",
                   "Ok");
                IsRunning = false;
                return;
            }
            var duration = (myEndDateTime.Subtract(myStartDateTime)).TotalHours;
            //var totalCost = ServiceRate * ServiceDetailsInstance.TotalDistance;
            var totalCost = service.ServicePrice(duration, false, false);
            ServiceDetailsInstance.TotalCost = Double.Parse(totalCost.ToString("#.##"));
            
            TotalCost = "$" + ServiceDetailsInstance.TotalCost;
            await App.Current.MainPage.Navigation.PushModalAsync(new TransporterServiceDetailsPage(), false);
            IsRunning = false;
        }

    }
}
