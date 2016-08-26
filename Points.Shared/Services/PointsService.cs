using Newtonsoft.Json;
using Points.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Points.Shared.Services
{
    public class PointsService : IPointsService
    {
        const string BaseUrl = "http://10.211.55.3:5000/";

        public async Task<IEnumerable<Card>> FetchCardsAsync(bool includeImages = false)
        {
            IEnumerable<Card> items;

            using (var client = new HttpClient())
            {
                var jsonString = await client.GetStringAsync(BaseUrl + "api/cards");
                var response = JsonConvert.DeserializeObject<IEnumerable<Card>>(jsonString);
                items = response.ToList();

                if (includeImages)
                {
                    var tasks = items.Select(async (i) =>
                    {
                        var image = await client.GetByteArrayAsync(BaseUrl + "img/cards/" + i.ImageName);
                        i.Image = image;
                        return i;
                    }).ToList();

                    items = await Task.WhenAll(tasks);
                }

                return items;
            }
        }


    }
}
