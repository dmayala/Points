using System;
using System.Collections.Generic;
using System.Linq;
using Points.Shared.Models;
using Microsoft.Extensions.Logging;

namespace Points.Web.Database
{
    public class PointsRepository : IPointsRepository
    {
        private PointsContext _context;
        private ILogger<PointsRepository> _logger;

        public PointsRepository(PointsContext context, ILogger<PointsRepository> logger)
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
