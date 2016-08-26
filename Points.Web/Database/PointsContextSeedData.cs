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
                var cat = new Category() { Name = "All" };

                var categoryValues = new CategoryValue[]
                {
                    new CategoryValue() {
                        Card =  new Card() { Name = "Chase Freedom", ImageName = "cf.png" },
                        Category = cat,
                        Points = 1.0
                    },
                    new CategoryValue() {
                        Card = new Card() { Name = "Chase Sapphire Preferred", ImageName = "csp.png" },
                        Category = cat,
                        Points = 2.0
                    },
                    new CategoryValue() {
                        Card =  new Card() { Name = "Starwood Preferred Guest", ImageName = "spg.png" },
                        Category = cat,
                        Points = 2.4
                    },
                    new CategoryValue() {
                        Card =  new Card() { Name = "Discover It", ImageName = "dit.png" },
                        Category = cat,
                        Points = 1.0
                    }
                };

                _context.CategoryValues.AddRange(categoryValues);
                await _context.SaveChangesAsync();
            }
        }
    }
}