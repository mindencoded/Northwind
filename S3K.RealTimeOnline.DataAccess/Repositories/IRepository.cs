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

    public interface IRepository<T> : IRepository, IDisposable where T : class
    {
        IEnumerable<dynamic> Select(IList<string> columns, IList<string> orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<T> Select(object conditions, IList<string> orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<T> Select(IDictionary<string, object> conditions, IList<string> orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<dynamic> Select(IList<string> columns, object conditions, IList<string> orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<dynamic> Select(IList<string> columns, IDictionary<string, object> conditions, IList<string> orderBy = null, int? page = null, int? pageSize = null);

        T SelectById(object id);

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