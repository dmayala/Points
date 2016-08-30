using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Points.Shared.Models;
using Points.Web.Core.Repositories;

namespace Points.Web.Persistence.Repositories
{
    public class ValuationsRepository : IValuationsRepository
    {
        private readonly PointsContext _context;
        private readonly ILogger<CardsRepository> _logger;

        public ValuationsRepository(PointsContext context, ILogger<CardsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Valuation GetBestValuationForCategory(Category categoryType)
        {
            var valuation = _context.Valuations.Where(c => c.Category == categoryType)
                .Include(c => c.Card)
                .OrderByDescending(c => c.Points)
                .FirstOrDefault();

            return valuation;
        }
    }
}