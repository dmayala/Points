using Newtonsoft.Json;
using Points.Droid.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Points.Droid.Utils
{
    public class PlacesApi
    {
        const string GoogleBrowserApiKey = "";

        public async Task<IList<Place>> FetchNearbyPlacesAsync(double latitude, double longitude)
        {
            var items = new List<Place>();
            const string Type = "establishment";

            var googlePlacesUrl = new StringBuilder("https://maps.googleapis.com/maps/api/place/nearbysearch/json?");
            googlePlacesUrl.Append("location=").Append(latitude).Append(",").Append(longitude);
            googlePlacesUrl.Append("&radius=").Append(1500);
            googlePlacesUrl.Append("&types=").Append(Type);
            googlePlacesUrl.Append("&sensor=true");
            googlePlacesUrl.Append("&key=" + GoogleBrowserApiKey);

            using (var client = new HttpClient())
            {
                var jsonString = await client.GetStringAsync(googlePlacesUrl.ToString());
                var response = JsonConvert.DeserializeObject<PlacesResponse>(jsonString);
                items = response.Places.ToList();
                return items;
            }
        }
    }
}