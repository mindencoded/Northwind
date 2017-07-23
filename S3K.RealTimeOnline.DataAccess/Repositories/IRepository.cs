using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace S3K.RealTimeOnline.DataAccess.Repositories
{
    public interface IRepository
    {
        void SetSqlConnection(SqlConnection sqlConnection);

        void SetSqlTransaction(SqlTransaction sqlTransaction);
    }

    public interface IRepository<TEntity> : IRepository, IDisposable where TEntity : class
    {
        IEnumerable<TEntity> Select();

        IEnumerable<TEntity> Select(TEntity entity);

        IEnumerable<TEntity> Select(object instance);

        TEntity SelectById(object id);

        int Insert(TEntity entity);

        int Insert(object instance);

        int Update(TEntity entity);

        int Update(object instance);

        int Update(object instance, object conditions);

        int Delete();

        int Delete(TEntity entity);

        int Delete(object instance);

        int DeleteById(object id);

        bool IsIdentityInsert();

        bool IsOpenConnection();
    }
}