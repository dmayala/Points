using Points.Shared.Models;
using System.Collections.Generic;

namespace Points.Web.Database
{
    public interface IPointsRepository
    {
        IEnumerable<Card> GetAllCards();
        CategoryValue GetBestCardForCategory(string categoryName);
    }
}
