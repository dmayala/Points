using Newtonsoft.Json;

namespace Points.Shared.Dtos
{
    public class Place
    {
        [JsonProperty(PropertyName = "place_id")]
        public string PlaceId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty(PropertyName = "types")]
        public string[] Types { get; set; }

        [JsonProperty(PropertyName = "photos")]
        public Photo[] Photos { get; set; }
    }
}