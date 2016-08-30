using Newtonsoft.Json;

namespace Points.Shared.Dtos
{
    public class Geometry
    {
        [JsonProperty(PropertyName = "location")]
        public Location Location { get; set; }
    }
}