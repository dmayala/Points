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
        private static readonly HttpClient Client = new HttpClient();

        public async Task<IEnumerable<Card>> FetchCardsAsync(bool includeImages = false)
        {
            var jsonString = await Client.GetStringAsync(BaseUrl + "/api/cards");
            var response = JsonConvert.DeserializeObject<IEnumerable<Card>>(jsonString);
            IEnumerable<Card> items = response.ToList();

            if (includeImages)
            {
                var tasks = items.Select(async (i) =>
                {
                    var image = await Client.GetByteArrayAsync(BaseUrl + "/img/cards/" + i.ImageName);
                    i.Image = image;
                    return i;
                }).ToList();

                items = await Task.WhenAll(tasks);
            }

            return items;
        }

        public async Task<Card> FetchBestCardForCategoryAsync(Category categoryType, bool includeImages = false)
        {
            var jsonString = await Client.GetStringAsync($"{BaseUrl}/api/valuations/category/{(long) categoryType}");
            var response = JsonConvert.DeserializeObject<Valuation>(jsonString);

            var card = response.Card;

            if (includeImages)
            {
                var image = await Client.GetByteArrayAsync(BaseUrl + "/img/cards/" + card.ImageName);
                card.Image = image;
            }

            return card;
        }

        public async Task<Card> FetchBestCardForCategoriesAsync(string[] categories, bool includeImages = false)
        {
            var categoriesQuery = String.Join("&", categories.Select(s => $"categories={s}"));
            var jsonString = await Client.GetStringAsync($"{BaseUrl}/api/valuations/categories?{categoriesQuery}");
            var response = JsonConvert.DeserializeObject<Valuation>(jsonString);

            var card = response.Card;

            if (includeImages)
            {
                var image = await Client.GetByteArrayAsync(BaseUrl + "/img/cards/" + card.ImageName);
                card.Image = image;
            }

            return card;
        }

        public async Task FetchCardImagesAsync(IEnumerable<Card> cards)
        {
            var imageDictionary = new Dictionary<string, byte[]>();
            foreach (var card in cards)
            {
                if (!imageDictionary.ContainsKey(card.ImageName))
                {
                    var image = await Client.GetByteArrayAsync(BaseUrl + "/img/cards/" + card.ImageName);
                    imageDictionary.Add(card.ImageName, image);
                    card.Image = image;
                }
                else
                {
                    card.Image = imageDictionary[card.ImageName];
                }
            }
        }
    }
}