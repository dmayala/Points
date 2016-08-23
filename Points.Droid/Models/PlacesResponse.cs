using Newtonsoft.Json;
using System.Collections.Generic;

namespace Points.Droid.Models
{
    public class PlacesResponse
    {
        [JsonProperty(PropertyName = "results")]
        public ICollection<Place> Places { get; set; }
    }
}