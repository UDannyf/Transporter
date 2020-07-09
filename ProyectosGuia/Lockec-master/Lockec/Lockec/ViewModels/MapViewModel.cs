namespace Lockec.ViewModels
{
    using Lockec.Extras;
    using Lockec.Services.Map;
    using Plugin.Permissions;
    using Plugin.Permissions.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Xamarin.Forms.GoogleMaps;
    using Bounds = Xamarin.Forms.GoogleMaps.Bounds;
    using Polyline = Xamarin.Forms.GoogleMaps.Polyline;

    public class MapViewModel : BaseViewModel
    {

        private string _origin;
        private string _destination;
        private string _originLatitud;
        private string _originLongitud;
        private string _destinationLatitud;
        private string _destinationLongitud;

        private bool _isBusy;
        private bool _isBusyOnMap;
        private bool _isOneDirection;
        private bool _isOriginFocused = true;

        private Geocode Start = null;
        private Geocode End = null;
        private Pin StartPin = null;
        private Pin EndPin = null;
        private Map _map;
        private IGoogleMapsApiService googleMapsApi;
        private GooglePlaceAutoCompletePrediction _placeSelected;
        private ObservableCollection<GooglePlaceAutoCompletePrediction> _places;
        private readonly List<Polyline> plottedRoutes = new List<Polyline>();

        public Map Map
        {
            get { return this._map; }
            set { SetValue(ref this._map, value); }
        }
        public ObservableCollection<GooglePlaceAutoCompletePrediction> Places
        {
            get { return this._places; }
            set { SetValue(ref this._places, value); }
        }
        public GooglePlaceAutoCompletePrediction PlaceSelected
        {
            get { return _placeSelected; }
            set
            {
                SetValue(ref this._placeSelected, value);
                if (_placeSelected != null)
                {
                    GetPlaceDetailCommand.Execute(_placeSelected);
                }
            }
        }
        public string Origin
        {
            get { return _origin; }
            set
            {
                SetValue(ref this._origin, value);
                if (!string.IsNullOrEmpty(_origin))
                {
                    MainViewModel.GetInstance().ServiceVM.ServiceDetailsInstance.StartPlace = _origin;
                    _isOriginFocused = true;
                    GetPlacesCommand.Execute(_origin);
                }
            }
        }
        public string Destination
        {
            get { return _destination; }
            set
            {
                SetValue(ref this._destination, value);
                if (!string.IsNullOrEmpty(_destination))
                {
                    MainViewModel.GetInstance().ServiceVM.ServiceDetailsInstance.EndPlace = _destination;
                    _isOriginFocused = false;
                    GetPlacesCommand.Execute(_destination);
                }

            }
        }
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                {
                    return;
                }
                SetValue(ref this._isBusy, value);
                OnPropertyChanged("IsBusy");
            }
        }
        public bool IsBusyOnMap
        {
            get { return _isBusyOnMap; }
            set
            {
                if (_isBusyOnMap == value)
                {
                    return;
                }
                SetValue(ref this._isBusyOnMap, value);
                OnPropertyChanged("IsBusyOnMap");
            }
        }
        public bool isOneDirection
        {
            get { return _isOneDirection; }
            set { SetValue(ref this._isOneDirection, value); }
        }
        public ICommand GetPlacesCommand { get; set; }
        public ICommand SetPinCommand { get; set; }
        public ICommand GetPlaceDetailCommand { get; set; }
        public ICommand LoadRouteCommand { get; set; }
       

        public MapViewModel()
        {
            googleMapsApi = new GoogleMapsApiService();
            Map = new Map
            {
                InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(-2.19d, -79.89d), 12d),
                MapType = MapType.Street,
            };
            Map.UiSettings.ZoomControlsEnabled = false;
            RequestLocationServices();
            Map.MapLongClicked += Map_MapLongClicked;
            Map.PinDragEnd += Map_PinDragEnd;
            GetPlacesCommand = new Command<string>(async (param) => await GetPlacesByName(param));
            GetPlaceDetailCommand = new Command<GooglePlaceAutoCompletePrediction>(async (param) => await SetPlaceSelectedOnMap(param));
            LoadRouteCommand = new Command(() => DrawRoute());
            
        }

        private async void RequestLocationServices()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                status = results[Permission.Location];
            }
            if (status == PermissionStatus.Granted)
            {
                this.Map.MyLocationEnabled = true;
            }
        }

        private async Task SetPlaceSelectedOnMap(GooglePlaceAutoCompletePrediction placeA)
        {
            var place = await googleMapsApi.GetPlaceDetails(placeA.PlaceId);
            if (place != null)
            {
                if (_isOriginFocused)
                {
                    Origin = place.Name;
                    _originLatitud = $"{place.Latitude}";
                    _originLongitud = $"{place.Longitude}";
                    _isOriginFocused = false;
                    if (StartPin != null)
                    {
                        Map.Pins.Remove(StartPin);
                    }
                    StartPin = CreatePin(Constants.ORIGEN_LABEL, place.Latitude, place.Longitude);
                    Start = new Geocode(place.Latitude, place.Longitude);
                    Map.Pins.Add(StartPin);
                    if (!_isOriginFocused && isOneDirection)
                    {
                        await App.Current.MainPage.Navigation.PopModalAsync(false);
                        _isOriginFocused = true;
                        CleanFields();
                        return;
                    }
                        
                }
                else
                {
                    Destination = place.Name;
                    _destinationLatitud = $"{place.Latitude}";
                    _destinationLongitud = $"{place.Longitude}";
                    if (EndPin != null)
                    {
                        Map.Pins.Remove(EndPin);
                    }
                    EndPin = CreatePin(Constants.DESTINO_LABEL, place.Latitude, place.Longitude);
                    End = new Geocode(place.Latitude, place.Longitude);
                    Map.Pins.Add(EndPin);

                    if (_originLatitud == _destinationLatitud && _originLongitud == _destinationLongitud)
                    {
                        await App.Current.MainPage.DisplayAlert("Ups", "El origen y el destino no pueden ser los mismos.", "Ok");
                    }
                    else
                    {
                        LoadRouteCommand.Execute(null);
                        await App.Current.MainPage.Navigation.PopModalAsync(false);
                        CleanFields();
                    }
                }
            }
        }

        void CleanFields()
        {
            PlaceSelected = null;
        }

        public async Task GetPlacesByName(string placeText)
        {
            Device.BeginInvokeOnMainThread(() => IsBusy = true);
            var places = await googleMapsApi.GetPlaces(placeText);
            var placeResult = places.AutoCompletePlaces;
            if (placeResult != null && placeResult.Count > 0)
            {
                Places = new ObservableCollection<GooglePlaceAutoCompletePrediction>(placeResult);
                Device.BeginInvokeOnMainThread(() => IsBusy = false);
            }
        }

        private void Map_MapLongClicked(object sender, MapLongClickedEventArgs e)
        {
            SetPinsOnMap(e);
            DrawRoute();
        }

        private async void Map_PinDragEnd(object sender, PinDragEventArgs e)
        {
            if (e.Pin.Label == Constants.ORIGEN_LABEL)
            {
                Start = new Geocode(e.Pin.Position.Latitude, e.Pin.Position.Longitude);
                Origin = await googleMapsApi.GetAddress(Start.StringLat(), Start.StringLng());
                SetOneDirection();
            }
            else
            {
                End = new Geocode(e.Pin.Position.Latitude, e.Pin.Position.Longitude);
                Destination = await googleMapsApi.GetAddress(End.StringLat(), End.StringLng());
            }
            DrawRoute();
        }

        private async void SetPinsOnMap(MapLongClickedEventArgs e)
        {
            if (Start == null)
            {
                Start = new Geocode(e.Point.Latitude, e.Point.Longitude);
                StartPin = CreatePin(e, Constants.ORIGEN_LABEL);
                Map.Pins.Add(StartPin);
                Origin = await googleMapsApi.GetAddress(Start.StringLat(), Start.StringLng());
                SetOneDirection();
                return;
            }
            if (End == null)
            {
                End = new Geocode(e.Point.Latitude, e.Point.Longitude);
                EndPin = CreatePin(e, Constants.DESTINO_LABEL);
                Map.Pins.Add(EndPin);
                Destination = await googleMapsApi.GetAddress(End.StringLat(), End.StringLng());
                return;
            }
        }

        private Pin CreatePin(MapLongClickedEventArgs e, string label)
        {
            return new Pin
            {
                Type = PinType.Place,
                Position = new Position(e.Point.Latitude, e.Point.Longitude),
                Label = label,
                IsDraggable = true
            };
        }

        private Pin CreatePin(string label, double Lat, double Lng)
        {
            return new Pin
            {
                Label = label,
                Type = PinType.Place,
                Position = new Position(Lat, Lng),
                IsDraggable = true
            };
        }

        private async void DrawRoute()
        {
            SetOneDirection();
            if (Start != null && End != null && Start != End)
            {
                Device.BeginInvokeOnMainThread(() => IsBusyOnMap = true);
                //IsBusy = true;
                var direction = await googleMapsApi.GetDirections(
                    Start.StringLat(), Start.StringLng(), 
                    End.StringLat(), End.StringLng());

                if (direction.Success)
                {
                    plottedRoutes.Clear();
                    Map.Polylines.Clear();
                    ShowMeMyCameraUpdate(direction.Routes[0].Bounds);
                    Leg[] legs = null;
                    for (int index = 0; index < direction.Routes.Length; index++)
                    {
                        var route = direction.Routes[index];
                        legs = direction.Routes[0].Legs;
                        Polyline polyLineData = new Polyline
                        {
                            StrokeWidth = 5,
                            StrokeColor = Color.FromRgb(118, 170, 219)
                        };
                        foreach (var coord in route.OverviewPolylineCoords)
                        {
                            polyLineData.Positions.Add(new Position(coord.Lat, coord.Lng));
                        }
                        Map.Polylines.Add(polyLineData);
                        plottedRoutes.Add(polyLineData);
                    }
                    //FOR DISTANCE
                    ServiceInfo(legs[0]);
                }
                Device.BeginInvokeOnMainThread(() => IsBusyOnMap = false);
                //IsBusy = false;
            };

        }

        private void ServiceInfo(Leg leg)
        {
            var distance = Double.Parse((leg.Distance.Value / 1000.0).ToString("#.##"));
            MainViewModel.GetInstance().ServiceVM.ServiceDetailsInstance.TotalDistance = distance;
            MainViewModel.GetInstance().ServiceVM.TotalDistance = leg.Distance.Text;
        }

        private async void ShowMeMyCameraUpdate(Services.Map.Bounds bounds)
        {
            var Southwest = new Position(bounds.Southwest.Lat, bounds.Southwest.Lng);
            var Northeast = new Position(bounds.Northeast.Lat, bounds.Northeast.Lng);
            Bounds googleBounds = new Bounds(Southwest, Northeast);
            CameraUpdate update = CameraUpdateFactory.NewBounds(googleBounds, 50);
            Map.InitialCameraUpdate = update;
            await Map.AnimateCamera(update);
        }

        private void SetOneDirection()
        {
            if (this.isOneDirection)
            {
                this.End = this.Start;
                this.EndPin = this.StartPin;
            }
        }

        public ICommand NavigateToPageCommand
        {
            get
            {
                return new Command<Type>(async (page) => await NavigateToPage(page));
            }
        }

        private async Task NavigateToPage(Type pageType)
        {
            Page page = (Page)Activator.CreateInstance(pageType);
            await App.Current.MainPage.Navigation.PushModalAsync(page, false);
        }
    }

}
