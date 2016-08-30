using Points.Shared.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Points.Web.Persistence
{
    public class PointsContextSeedData
    {
        private readonly PointsContext _context;

        public PointsContextSeedData(PointsContext context)
        {
            _context = context;
        }

        public async Task EnsureSeedDataAsync()
        {
            if (!_context.Cards.Any())
            {
                // Add new data
                const Category cat = Category.All;

                var valuations = new[]
                {
                    new Valuation
                    {
                        Card = new Card() { Name = "Chase Freedom", ImageName = "cf.png" },
                        Category = cat,
                        Points = 1.0
                    },
                    new Valuation
                    {
                        Card = new Card() { Name = "Chase Sapphire Preferred", ImageName = "csp.png" },
                        Category = cat,
                        Points = 2.0
                    },
                    new Valuation
                    {
                        Card = new Card() { Name = "Starwood Preferred Guest", ImageName = "spg.png" },
                        Category = cat,
                        Points = 2.4
                    },
                    new Valuation
                    {
                        Card = new Card() { Name = "Discover It", ImageName = "dit.png" },
                        Category = cat,
                        Points = 1.0
                    }
                };

                _context.Valuations.AddRange(valuations);
                await _context.SaveChangesAsync();
            }
        }
    }
}