using System.Collections.Generic;
using Newtonsoft.Json;

namespace Points.Shared.Dtos
{
    public class PlacesResponse
    {
        [JsonProperty(PropertyName = "results")]
        public ICollection<Place> Places { get; set; }
    }
}