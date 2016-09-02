using System.Collections.Generic;
using Points.Shared.Models;

namespace Points.Web.Core.Repositories
{
    public interface IValuationsRepository
    {
        Valuation GetBestValuationForCategory(Category categoryType);
        IEnumerable<Valuation> GetBestValuationsForCategories(string[] categories);
    }
}