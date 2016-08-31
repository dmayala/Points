using Points.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Points.Shared.Services
{
    public interface IPointsService
    {
        Task<IEnumerable<Card>> FetchCardsAsync(bool includeImages = false);
        Task<Card> FetchBestCardForCategoryAsync(Category categoryType, bool includeImages = false);
        Task<Card> FetchBestCardForCategoriesAsync(string[] categories, bool includeImages = false);
    }
}