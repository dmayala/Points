using System;
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
        private const string BaseUrl = "http://10.211.55.3:5000";

        public async Task<IEnumerable<Card>> FetchCardsAsync(bool includeImages = false)
        {
            using (var client = new HttpClient())
            {
                var jsonString = await client.GetStringAsync(BaseUrl + "/api/cards");
                var response = JsonConvert.DeserializeObject<IEnumerable<Card>>(jsonString);
                IEnumerable<Card> items = response.ToList();

                if (includeImages)
                {
                    var tasks = items.Select(async (i) =>
                    {
                        var image = await client.GetByteArrayAsync(BaseUrl + "/img/cards/" + i.ImageName);
                        i.Image = image;
                        return i;
                    }).ToList();

                    items = await Task.WhenAll(tasks);
                }

                return items;
            }
        }

        public async Task<Card> FetchBestCardForCategoryAsync(Category categoryType, bool includeImages = false)
        {
            using (var client = new HttpClient())
            {
                var jsonString = await client.GetStringAsync($"{BaseUrl}/api/valuations/category/{(long) categoryType}");
                var response = JsonConvert.DeserializeObject<Valuation>(jsonString);

                var card = response.Card;

                if (includeImages)
                {
                    var image = await client.GetByteArrayAsync(BaseUrl + "/img/cards/" + card.ImageName);
                    card.Image = image;
                }

                return card;
            }
        }

        public async Task<Card> FetchBestCardForCategoriesAsync(string[] categories, bool includeImages = false)
        {
            var categoriesQuery = String.Join("&", categories.Select(s => $"categories={s}"));

            using (var client = new HttpClient())
            {
                var jsonString = await client.GetStringAsync($"{BaseUrl}/api/valuations/categories?{categoriesQuery}");
                var response = JsonConvert.DeserializeObject<Valuation>(jsonString);

                var card = response.Card;

                if (includeImages)
                {
                    var image = await client.GetByteArrayAsync(BaseUrl + "/img/cards/" + card.ImageName);
                    card.Image = image;
                }

                return card;
            }
        }
    }
}