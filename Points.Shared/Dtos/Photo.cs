using Newtonsoft.Json;

namespace Points.Shared.Dtos
{
    public class Photo
    {
        [JsonProperty(PropertyName = "height")]
        public long Height { get; set; }
        [JsonProperty(PropertyName = "width")]
        public long Width { get; set; }
        [JsonProperty(PropertyName = "photo_reference")]
        public string PhotoReference { get; set; }
    }
}
