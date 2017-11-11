using S3K.RealTimeOnline.BusinessDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;

namespace S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork
{
    public interface IBusinessUnitOfWork : IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
    }
}