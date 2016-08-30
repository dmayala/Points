using Points.Web.Core.Repositories;

namespace Points.Web.Core
{
    public interface IUnitOfWork
    {
        ICardsRepository Cards { get; }
        IValuationsRepository Valuations { get; }
        void Complete();
    }
}