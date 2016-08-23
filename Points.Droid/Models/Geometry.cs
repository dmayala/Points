using Newtonsoft.Json;

namespace Points.Droid.Models
{
    public class Geometry
    {
        [JsonProperty(PropertyName = "location")]
        public Location Location { get; set; }
    }
}