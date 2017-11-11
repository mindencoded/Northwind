using S3K.RealTimeOnline.BusinessDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWorks;

namespace S3K.RealTimeOnline.BusinessDataAccess.UnitOfWorks
{
    public interface IBusinessUnitOfWork : IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
    }
}