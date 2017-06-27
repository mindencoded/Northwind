using System;
using S3K.RealTimeOnline.DataAccess.Repositories;

namespace S3K.RealTimeOnline.DataAccess.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        IRepository<T> Repository<T>() where T : class;

        object Repository(Type type);

        void Register(IRepository repository);
    }
}
