namespace Lockec.Services.Map
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class DistanceOp
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }

    public class Duration
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }

    public class EndLocation
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

    public class StartLocation
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

    public class Polyline
    {
        [JsonProperty("points")]
        public string Points { get; set; }
    }

    public class Step
    {
        [JsonProperty("distance")]
        public DistanceOp Distance { get; set; }

        [JsonProperty("duration")]
        public Duration Duration { get; set; }

        [JsonProperty("end_location")]
        public EndLocation EndLocation { get; set; }

        [JsonProperty("html_instructions")]
        public string HtmlInstructions { get; set; }

        [JsonProperty("polyline")]
        public Polyline Polyline { get; set; }

        [JsonProperty("start_location")]
        public StartLocation StartLocation { get; set; }

        [JsonProperty("travel_mode")]
        public string TravelMode { get; set; }

        [JsonProperty("maneuver")]
        public string Maneuver { get; set; }
    }

    public class Leg
    {
        [JsonProperty("distance")]
        public DistanceOp Distance { get; set; }

        [JsonProperty("duration")]
        public Duration Duration { get; set; }

        [JsonProperty("end_address")]
        public string EndAddress { get; set; }

        [JsonProperty("end_location")]
        public EndLocation EndLocation { get; set; }

        [JsonProperty("start_address")]
        public string StartAddress { get; set; }

        [JsonProperty("start_location")]
        public StartLocation StartLocation { get; set; }

        [JsonProperty("steps")]
        public IList<Step> Steps { get; set; }

        [JsonProperty("traffic_speed_entry")]
        public IList<object> TrafficSpeedEntry { get; set; }

        [JsonProperty("via_waypoint")]
        public IList<object> ViaWaypoint { get; set; }
    }

    public class Directions
    {
        public Route[] Routes { get; set; }
        public string Status { get; set; }

        public bool Success
        {
            get { return Status == "OK"; }
        }
    }

    public class Route
    {
        public Bounds Bounds { get; set; }

        public string Copyrights { get; set; }

        public Leg[] Legs { get; set; }

        [JsonProperty("overview_polyline")]
        public EncodedPolyline OverviewPolyline { get; set; }

        [JsonIgnore]
        public IList<Geocode> OverviewPolylineCoords
        {
            get
            {
                return DirectionUtils.DecodePolylinePoints(OverviewPolyline.Points);
            }
        }

        public string Summary { get; set; }
    }

    public class Bounds
    {
        [JsonProperty("northeast")]
        public Northeast Northeast { get; set; }

        [JsonProperty("southwest")]
        public Southwest Southwest { get; set; }
    }

    public class Northeast
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

    public class Southwest
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

    public class EncodedPolyline
    {
        public string Points { get; set; }
    }

    internal static class DirectionUtils
    {
        /// <summary>
        /// Google byte-encodes the coordinates for polylines on the map, this routine will
        /// decode those coordinates back to Android Lat/Lng values.
        /// </summary>
        /// <returns>The polyline points.</returns>
        /// <param name="encodedPoints">Encoded points.</param>
        internal static List<Geocode> DecodePolylinePoints(string encodedPoints)
        {
            List<Geocode> results = new List<Geocode>();
            int index = 0, len = encodedPoints.Length;
            int lat = 0, lng = 0;
            while (index < len)
            {
                int oneByte, shift = 0, result = 0;
                do
                {
                    oneByte = encodedPoints[index++] - 63;
                    result |= (oneByte & 0x1f) << shift;
                    shift += 5;
                } while (oneByte >= 0x20);

                int dlat = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
                lat += dlat;
                shift = 0;
                result = 0;
                do
                {
                    oneByte = encodedPoints[index++] - 63;
                    result |= (oneByte & 0x1f) << shift;
                    shift += 5;
                } while (oneByte >= 0x20);
                int dlng = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
                lng += dlng;

                results.Add(new Geocode((double)lat / 1E5, (double)lng / 1E5));
            }
            return results;
        }
    }
}
