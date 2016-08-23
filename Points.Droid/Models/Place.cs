using Newtonsoft.Json;

namespace Points.Droid.Models
{
    public class Place
    {
        [JsonProperty(PropertyName = "place_id")]
        public string PlaceId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty(PropertyName = "types")]
        public string[] Types { get; set; }
    }
}