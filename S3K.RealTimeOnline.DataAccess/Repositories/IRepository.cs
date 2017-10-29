using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using S3K.RealTimeOnline.Commons;

namespace S3K.RealTimeOnline.DataAccess.Repositories
{
    public interface IRepository
    {
        void SetSqlConnection(SqlConnection sqlConnection);

        void SetSqlTransaction(SqlTransaction sqlTransaction);
    }

    public interface IRepository<TEntity> : IRepository, IDisposable where TEntity : class
    {
        IEnumerable<dynamic> Select(IList<string> columns);

        IEnumerable<TEntity> Select(object conditions);

        IEnumerable<TEntity> Select(IDictionary<string, object> conditions);

        IEnumerable<dynamic> Select(IList<string> columns, object conditions);

        IEnumerable<dynamic> Select(IList<string> columns, IDictionary<string, object> conditions);

        IEnumerable<dynamic> Select(IList<string> columns, object conditions, IList<string> orderBy);

        IEnumerable<dynamic> Select(IList<string> columns, IDictionary<string, object> conditions, IList<string> orderBy);

        IEnumerable<TEntity> Select(object conditions, IList<string> orderBy);


        IEnumerable<TEntity> Select(IDictionary<string, object> conditions, IList<string> orderBy);


        TEntity SelectById(object id);

        int Insert(object parameters);

        int Update(object parameters);

        int Update(object parameters, object conditions);

        int Delete(object conditions);

        int DeleteById(object id);

        bool IsIdentityInsert();

        bool IsOpenConnection();

        SqlDataAdapter SqlDataAdapter();
    }
}