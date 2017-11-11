using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;
using S3K.RealTimeOnline.SecurityDataAccess.Repositories;

namespace S3K.RealTimeOnline.SecurityDataAccess.UnitOfWork
{
    public interface ISecurityUnitOfWork : IUnitOfWork
    {
        IUserRepository UserRepository { get; }
    }
}