namespace Lockec.Services.Map
{
    public class Geocode
    {
        public double Lat { get; private set; }
        public double Lng { get; private set; }

        public Geocode(double lat, double lng)
        {
            this.Lat = lat;
            this.Lng = lng;
        }

        public Geocode()
        {
            this.Lat = 0.0;
            this.Lng = 0.0;
        }

        public string StringLat()
        {
            return Lat.ToString().Replace(',', '.');
        }

        public string StringLng()
        {
            return Lng.ToString().Replace(',', '.');
        }
    }
}
