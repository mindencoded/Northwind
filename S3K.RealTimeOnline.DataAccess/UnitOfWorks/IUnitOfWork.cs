using System;
using System.Collections.Generic;
using S3K.RealTimeOnline.DataAccess.Repositories;

namespace S3K.RealTimeOnline.DataAccess.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        IRepository<T> Repository<T>() where T : class;

        object Repository(Type type);

        void Register(IRepository repository);

        IEnumerable<TEntity> ExecuteQueryText<TEntity>(string statement, object query = null) where TEntity : class;

        IEnumerable<TEntity> ExecuteQueryFunction<TEntity>(string commandText, object query = null) where TEntity : class;
    }
}