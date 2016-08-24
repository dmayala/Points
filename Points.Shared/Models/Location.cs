using Newtonsoft.Json;

namespace Points.Shared.Models
{
    public class Location
    {
        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "lng")]
        public double Longitude { get; set; }
    }
}