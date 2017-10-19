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

        IEnumerable<TEntity> Select(object conditions);

        TEntity SelectById(object id);

        int Insert(object parameters);

        int Update(object parameters);

        int Update(object parameters, object conditions);

        int Delete(object conditions);

        int DeleteById(object id);

        bool IsIdentityInsert();

        bool IsOpenConnection();
    }
}