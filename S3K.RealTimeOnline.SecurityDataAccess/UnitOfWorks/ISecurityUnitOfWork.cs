using S3K.RealTimeOnline.GenericDataAccess.UnitOfWorks;
using S3K.RealTimeOnline.SecurityDataAccess.Repositories;

namespace S3K.RealTimeOnline.SecurityDataAccess.UnitOfWorks
{
    public interface ISecurityUnitOfWork : IUnitOfWork
    {
        IUserRepository UserRepository { get; }
    }
}