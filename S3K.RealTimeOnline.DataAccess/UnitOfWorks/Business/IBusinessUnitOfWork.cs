using S3K.RealTimeOnline.DataAccess.Repositories.Business;

namespace S3K.RealTimeOnline.DataAccess.UnitOfWorks.Business
{
    public interface IBusinessUnitOfWork : IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
    }
}