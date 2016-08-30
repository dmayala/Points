using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Points.Shared.Models;
using Points.Web.Core.Repositories;

namespace Points.Web.Persistence.Repositories
{
    public class CardsRepository : ICardsRepository
    {
        private readonly PointsContext _context;
        private readonly ILogger<CardsRepository> _logger;

        public CardsRepository(PointsContext context, ILogger<CardsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Card> GetAllCards()
        {
            try
            {
                return _context.Cards.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get cards from database", ex);
                return null;
            }
        }
    }
}
