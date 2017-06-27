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
        IEnumerable<TEntity> SelectAll();

        IEnumerable<TEntity> SelectAll(TEntity entity);

        bool IsOpenConnection();

        int Insert(TEntity entity);

        int Update(TEntity entity);

        int Delete();

        int Delete(TEntity entity);

        int Delete(object id);

        TEntity SelectById(TEntity entity);

        TEntity SelectById(object id);

        bool IsIdentityInsert();
    }
}