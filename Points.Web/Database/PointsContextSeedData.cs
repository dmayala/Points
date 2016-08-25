using Points.Shared.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Points.Web.Database
{
    public class PointsContextSeedData
    {
        private PointsContext _context;

        public PointsContextSeedData(PointsContext context)
        {
            _context = context;
        }

        public async Task EnsureSeedDataAsync()
        {
            if (!_context.Cards.Any())
            {
                // Add new data
                _context.Cards.AddRange(new Card[]
                {
                    new Card() { Name = "Chase Freedom" },
                    new Card() { Name = "Chase Sapphire Preferred" },
                    new Card() { Name = "Starwood Preferred Guest" }
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}