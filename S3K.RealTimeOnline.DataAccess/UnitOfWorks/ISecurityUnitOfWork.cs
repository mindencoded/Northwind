using S3K.RealTimeOnline.DataAccess.Repositories;

namespace S3K.RealTimeOnline.DataAccess.UnitOfWorks
{
    public interface ISecurityUnitOfWork : IUnitOfWork
    {
        IUserRepository UserRepository { get; }
    }
}
