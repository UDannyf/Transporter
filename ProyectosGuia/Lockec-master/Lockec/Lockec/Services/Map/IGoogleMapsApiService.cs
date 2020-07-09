namespace Lockec.Services.Map
{
    using System.Threading.Tasks;

    interface IGoogleMapsApiService
    {
        Task<Directions> GetDirections(string originLatitude, string originLongitude, string destinationLatitude, string destinationLongitude);
        Task<string> GetAddress(string latitude, string longitude);
        Task<GooglePlaceAutoCompleteResult> GetPlaces(string text);
        Task<GooglePlace> GetPlaceDetails(string placeId);
    }
}
