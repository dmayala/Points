using Points.Web.Core;
using Points.Web.Core.Repositories;

namespace Points.Web.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PointsContext _context;
        public ICardsRepository Cards { get; }
        public IValuationsRepository Valuations { get; }

        public UnitOfWork(PointsContext context, ICardsRepository cards, IValuationsRepository valuations)
        {
            _context = context;
            Cards = cards;
            Valuations = valuations;
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}