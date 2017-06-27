using S3K.RealTimeOnline.DataAccess.Repositories;

namespace S3K.RealTimeOnline.DataAccess.UnitOfWorks
{
    public interface IBusinessUnitOfWork : IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
    }
}