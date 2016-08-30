using System.Collections.Generic;
using Points.Shared.Models;

namespace Points.Web.Core.Repositories
{
    public interface ICardsRepository
    {
        IEnumerable<Card> GetAllCards();
    }
}
