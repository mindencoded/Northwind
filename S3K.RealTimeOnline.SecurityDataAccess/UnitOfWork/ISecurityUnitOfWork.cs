using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;
using S3K.RealTimeOnline.SecurityDataAccess.Repositories;
using S3K.RealTimeOnline.SecurityDataAccess.Repositories.Contracts;

namespace S3K.RealTimeOnline.SecurityDataAccess.UnitOfWork
{
    public interface ISecurityUnitOfWork : IUnitOfWork
    {
        IUserRepository UserRepository { get; }
    }
}