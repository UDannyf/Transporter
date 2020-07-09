namespace Lockec.ViewModels
{

    using Lockec.Extras;
    using Lockec.Models;
    using Lockec.Services.Api_rest;
    using Lockec.Views.Menu;
    using Lockec.Views.MenuOptions;
    using Lockec.Views.PopUps;
    using Rg.Plugins.Popup.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ServiceSummaryViewModel : BaseViewModel
    {
        private ObservableCollection<ServiceDetails> _services;
        private ServiceDetails _serviceSelected;

        private ObservableCollection<Anomaly> _anomalies;

        private ApiService api;
        private Account AccountInstance;

        private bool _isRunning;
        private string _anomalyText;
        private string _suggestionTitleText;
        private string _suggestionBodyText;
        private string _rateValue;
        private string _commentary;

        public string RateValue
        {
            get { return this._rateValue; }
            set { SetValue(ref this._rateValue, value); }
        }

        public string Commentary
        {
            get { return this._commentary; }
            set { SetValue(ref this._commentary, value); }
        }
        public string SuggestionTitleText
        {
            get { return this._suggestionTitleText; }
            set { SetValue(ref this._suggestionTitleText, value); }
        }

        public string SuggestionBodyText
        {
            get { return this._suggestionBodyText; }
            set { SetValue(ref this._suggestionBodyText, value); }
        }

        public string AnomalyText
        {
            get { return this._anomalyText; }
            set { SetValue(ref this._anomalyText, value); }
        }
        public bool IsRunning
        {
            get { return this._isRunning; }
            set { SetValue(ref this._isRunning, value); }
        }
        public ICommand GetServiceDetailCommand { get; set; }
        public ICommand InitServiceDetailsCommand { get; set; }
        public ICommand InitServiceAnomalyCommand { get; set; }
        public ICommand ShowRateServicePageCommand 
        {
            get 
            { 
                return new Command<ServiceDetails>(async (param) => await ShowRateServicePage(param));
            }
        }

        public ICommand NavigateServiceCommand
        {
            get
            {
                return new Command<Type>(async (page) => await NavigateService(page));
            }
        }

        public ICommand MakeRateCommand
        {
            get
            {
                return new Command(MakeRate);
            }
        }

        public ObservableCollection<ServiceDetails> Services
        {
            get { return this._services; }
            set { SetValue(ref this._services, value); }
        }
        public ServiceDetails ServiceSelected
        {
            get { return _serviceSelected; }
            set
            {
                SetValue(ref this._serviceSelected, value);
                if (_serviceSelected != null)
                {
                    GetServiceDetailCommand.Execute(_serviceSelected);
                }
            }
        }

        public ObservableCollection<Anomaly> Anomalies
        {
            get { return this._anomalies; }
            set { SetValue(ref this._anomalies, value); }
        }

        public ServiceSummaryViewModel()
        {
            GetServiceDetailCommand = new Command<ServiceDetails>(async (param) => await ShowServiceDetailsSummary(param));
            InitServiceDetailsCommand = new Command(InitServiceHistoryDetails);
            InitServiceAnomalyCommand = new Command(InitServiceAnomalyDetails);
            api = new ApiService();
            IsRunning = true;
            AccountInstance = MainViewModel.GetInstance().MenuVM.AccountInstance;
        }

        private async Task NavigateService(Type pageType)
        {
            Page page = (Page)Activator.CreateInstance(pageType);
            await App.Current.MainPage.Navigation.PushModalAsync(page, false);
        }

        private async void InitServiceHistoryDetails()
        {

            await Task.Run(async () =>
            {
                 
                 var historyResponse = await api.GetAllInfo<ServiceDetails>(Constants.SERVICES_DETAILS, "?user_id=" + AccountInstance.PhoneNumber + "&ordering=-date");
                 if(historyResponse.IsSuccess != false)
                 {
                     var history = (List<ServiceDetails>)historyResponse.Result;
                     Services = new ObservableCollection<ServiceDetails>();
                     //Services.OrderBy(s => s.Status)
                     foreach (var h in history)
                     {
                         Services.Add(h);
                     }

                     Device.BeginInvokeOnMainThread(() =>
                     {
                         // UI interaction goes here
                         IsRunning = false;
                     });
                 }
                 else
                 {
                     await Application.Current.MainPage.DisplayAlert(
                   "¡Ups!",
                   "No posees ningún servicio asociado a tu cuenta.",
                   "Ok");
                     return;
                 }
                 
             });


/*            var stringHistory = await api.GetAllServiceInfo(Constants.SERVICES_DETAILS, "user_id", AccountInstance.PhoneNumber);
            var history = JsonConvert.DeserializeObject<List<ServiceDetails>>(stringHistory);
            Services = new ObservableCollection<ServiceDetails>();
            foreach (var h in history)
            {
                Services.Add(h);
            }
            IsRunning = false;*/
        }

        private async void MakeRate() 
        {
            if (string.IsNullOrEmpty(Commentary))
            {
                await Application.Current.MainPage.DisplayAlert(
                  "¡Ups!",
                  "Escribe un comentario.",
                  "Ok");
                return;
            }

            if (string.IsNullOrEmpty(RateValue))
            {
                await Application.Current.MainPage.DisplayAlert(
                  "¡Ups!",
                  "Danos una calificación",
                  "Ok");
                return;
            }

            var serviceProvidedResponse = await api.GetWithParam<ServiceProvided>(Constants.SERVICES_PROVIDED, "service_details", ServiceSelected.Id);
            var serviceProvidedList = (List<ServiceProvided>)serviceProvidedResponse.Result;
            var serviceProvided = serviceProvidedList[0];
            serviceProvided.service_score = getRateNum();
            serviceProvided.comments = Commentary;

            var response = await api.Put<ServiceProvided>(Constants.SERVICES_PROVIDED, serviceProvided, serviceProvided.id);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                 "¡Ups!",
                 "No se pudo calificar, inténtalo más tarde",
                 "Ok");
                return;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                 "¡Yeah!",
                 "Gracias por calificar",
                 "Ok");
                await App.Current.MainPage.Navigation.PushModalAsync(new MenuPage(), false); 
            }
        }


        private int getRateNum() 
        {
            switch (RateValue)
            {   
                case "Mala":
                    return 2;
                case "Regular":
                    return 3;
                case "Buena":
                    return 4;
                case "Excelente":
                    return 5;
                default:
                    return 1;
            }
            
        }
        private async void InitServiceAnomalyDetails()
        {

            await Task.Run(async () =>
            {
                // Run code here
                
                var anomaliesResponse = await api.GetAllInfo<Anomaly>(Constants.ANOMALY, "?ordering=-created&user_anomaly=" + AccountInstance.PhoneNumber);
                var servicesResponse = await api.GetAllInfo<ServiceDetails>(Constants.SERVICES_DETAILS, "?user_id=" + AccountInstance.PhoneNumber);
                if (anomaliesResponse.IsSuccess != false && servicesResponse.IsSuccess != false)
                {
                    var anomalies = (List<Anomaly>)anomaliesResponse.Result;
                    Anomalies = new ObservableCollection<Anomaly>();
                    var services = (List<ServiceDetails>)servicesResponse.Result;
                    Services = new ObservableCollection<ServiceDetails>();
                    foreach (var x in anomalies)
                    {
                        foreach (var y in services)
                        {
                            if (x.service.ToString() == y.Id.ToString())
                            {
                                x.service = y;
                                Anomalies.Add(x);
                            }
                        }
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        // UI interaction goes here
                        IsRunning = false;
                    });
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                  "¡Ups!",
                  "No posees ningún servicio asociado a tu cuenta.",
                  "Ok");
                    return;
                }


            });
        }

        private async Task ShowServiceDetailsSummary(ServiceDetails service)
        {
            await PopupNavigation.Instance.PushAsync(new ServiceHistoryPopUpView());
            /*await Application.Current.MainPage.DisplayAlert(
                    service.ReqService, service.Date,
                    "Ok");
            return;*/
        }

        private async Task ShowRateServicePage(ServiceDetails service) 
        {
            await PopupNavigation.Instance.PopAsync();
            await App.Current.MainPage.Navigation.PushModalAsync(new ServiceRatePage(), false);
        }

        public ICommand ShowAnomalyPopUpViewCommand
        {
            get 
            { 
                return new Command(ShowAnomalyPopUpView); 
            }
        }

        public ICommand SendAnomalyCommand
        {
            get
            {
                return new Command(SendAnomaly);
            }
        }

        public ICommand SendSuggestionCommand
        {
            get
            {
                return new Command(SendSuggestion);
            }
        }

        private async void ShowAnomalyPopUpView()
        {
            await PopupNavigation.Instance.PopAsync();
            await PopupNavigation.Instance.PushAsync(new AnomalyPopUpView());
        }

        private async void SendAnomaly()
        {
            if (string.IsNullOrEmpty(AnomalyText))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa un mensaje.",
                    "Ok");
                    return;
            }

            var anomaly = new Anomaly
            {
                comment_anomaly = AnomalyText,
                user_anomaly = AccountInstance.PhoneNumber,
                service = ServiceSelected.Id
            };

            var response = await api.Post<Anomaly>(Constants.ANOMALY, anomaly);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Algo salió mal. Prueba nuevamente más tarde.",
                    "Ok");
                return;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Yeah!",
                    "Anomalía registrada con éxito.",
                    "Ok");
                await PopupNavigation.Instance.PopAsync();
                return;
            }

        }

        private async void SendSuggestion()
        {
            if (string.IsNullOrEmpty(SuggestionTitleText))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa un asunto.",
                    "Ok");
                return;
            }

            if (string.IsNullOrEmpty(SuggestionBodyText))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Ingresa un mensaje.",
                    "Ok");
                return;
            }

            var suggestion = new Suggestion
            {
                title = SuggestionTitleText,
                comment_suggestion = SuggestionBodyText,
                user_suggestion = AccountInstance.PhoneNumber
            };

            var response = await api.Post<Suggestion>(Constants.SUGGESTION, suggestion);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Ups!",
                    "Algo salió mal. Prueba nuevamente más tarde.",
                    "Ok");
                return;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "¡Yeah!",
                    "Gracias por escribirnos tus sugerencias y comentarios.",
                    "Ok");
                CleanFields();
                return;
            }
            
        }

        private void CleanFields()
        {
            this.SuggestionTitleText = this.SuggestionBodyText = string.Empty;
        }


        ///AQUI ME HE QUEDADO, DEBO MANDAR LA PANTALLA DE CALIGICAR SERVICIOS

    }
}
