using Newtonsoft.Json;

namespace Points.Shared.Models
{
    public class Geometry
    {
        [JsonProperty(PropertyName = "location")]
        public Location Location { get; set; }
    }
}