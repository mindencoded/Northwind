using S3K.RealTimeOnline.DataAccess.Repositories.Security;

namespace S3K.RealTimeOnline.DataAccess.UnitOfWorks.Security
{
    public interface ISecurityUnitOfWork : IUnitOfWork
    {
        IUserRepository UserRepository { get; }
    }
}
