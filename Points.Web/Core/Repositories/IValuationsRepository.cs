using Points.Shared.Models;

namespace Points.Web.Core.Repositories
{
    public interface IValuationsRepository
    {
        Valuation GetBestValuationForCategory(Category categoryType);
        Valuation GetBestValuationForCategories(string[] categories);
    }
}