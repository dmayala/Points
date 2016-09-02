using Points.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Points.Shared.Services
{
    public interface IPointsService
    {
        Task<IEnumerable<Card>> FetchCardsAsync();
        Task<IEnumerable<Valuation>> FetchBestValuationForCategoriesAsync(string[] categories);
        Task FetchCardImagesAsync(IEnumerable<Card> cards);
    }
}