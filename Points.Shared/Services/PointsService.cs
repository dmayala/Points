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

        public async Task<IEnumerable<Card>> FetchCardsAsync()
        {
            var jsonString = await Client.GetStringAsync(BaseUrl + "/api/cards");
            var response = JsonConvert.DeserializeObject<IEnumerable<Card>>(jsonString);
            return response;
        }

        public async Task<IEnumerable<Valuation>> FetchBestValuationForCategoriesAsync(string[] categories)
        {
            var categoriesQuery = String.Join("&", categories.Select(s => $"category={s}"));
            var jsonString = await Client.GetStringAsync($"{BaseUrl}/api/valuations/categories?{categoriesQuery}");
            var response = JsonConvert.DeserializeObject<IEnumerable<Valuation>>(jsonString);
            return response;
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