namespace Lockec.Services.Map
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    class GoogleMapsApiService : IGoogleMapsApiService
    {
        static string _googleMapsKey;
        static string _countryLocation;
        static int _radius;
        static string _language;

        public static void Initialize(string googleMapsKey)
        {
            _googleMapsKey = googleMapsKey;
            _countryLocation = "-2.147630, -79.941750"; //Guayaquil
            _radius = 130;
            _language = "es";
        }

        public async Task<Directions> GetDirections(string originLatitude, string originLongitude, string destinationLatitude, string destinationLongitude)
        {
            Directions googleDirection = new Directions();

            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetAsync($"api/directions/json?mode=driving&transit_routing_preference=less_driving&origin={originLatitude},{originLongitude}&destination={destinationLatitude},{destinationLongitude}&key={_googleMapsKey}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        googleDirection = await Task.Run(() =>
                           JsonConvert.DeserializeObject<Directions>(json)
                        ).ConfigureAwait(false);
                    }
                }
            }

            return googleDirection;
        }

        public async Task<string> GetAddress(string latitude, string longitude)
        {
            string googleAddress = null;
            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetStringAsync($"api/geocode/json?latlng={latitude},{longitude}&location_type=ROOFTOP|APPROXIMATE&key={_googleMapsKey}").ConfigureAwait(false);
                var dataJson = (JObject)JsonConvert.DeserializeObject(response);
                googleAddress = dataJson["results"][0]["formatted_address"].ToString();
            }
            return googleAddress;
        }

        public async Task<GooglePlaceAutoCompleteResult> GetPlaces(string text)
        {
            GooglePlaceAutoCompleteResult results = null;

            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetAsync($"api/place/autocomplete/json?input={Uri.EscapeUriString(text)}&location={_countryLocation}&radius={_radius}&language={_language}&key={_googleMapsKey}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json) && json != "ERROR")
                    {
                        results = await Task.Run(() =>
                           JsonConvert.DeserializeObject<GooglePlaceAutoCompleteResult>(json)
                        ).ConfigureAwait(false);

                    }
                }
            }

            return results;
        }

        public async Task<GooglePlace> GetPlaceDetails(string placeId)
        {
            GooglePlace result = null;
            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetAsync($"api/place/details/json?placeid={Uri.EscapeUriString(placeId)}&key={_googleMapsKey}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json) && json != "ERROR")
                    {
                        result = new GooglePlace(JObject.Parse(json));
                    }
                }
            }

            return result;
        }


        private const string ApiBaseAddress = "https://maps.googleapis.com/maps/";
        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiBaseAddress)
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
    }

}
