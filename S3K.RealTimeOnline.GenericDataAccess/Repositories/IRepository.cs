using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using S3K.RealTimeOnline.GenericDataAccess.Tools;

namespace S3K.RealTimeOnline.GenericDataAccess.Repositories
{
    public interface IRepository
    {
        void SetSqlConnection(SqlConnection sqlConnection);

        void SetSqlTransaction(SqlTransaction sqlTransaction);
    }

    public interface IRepository<T> : IRepository, IDisposable where T : class
    {
        IEnumerable<dynamic> Select(IList<string> columns, string orderBy = null, int? page = null,
            int? pageSize = null);

        IEnumerable<T> Select(object conditions, string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<T> Select(IDictionary<string, object> conditions, string orderBy = null, int? page = null,
            int? pageSize = null);

        IEnumerable<T> Select(IList<ParameterBuilder> conditions, string orderBy = null, int? page = null,
            int? pageSize = null);

        IEnumerable<dynamic> Select(IList<string> columns, object conditions, string orderBy = null, int? page = null,
            int? pageSize = null);

        IEnumerable<dynamic> Select(IList<string> columns, IDictionary<string, object> conditions,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<dynamic> Select(IList<string> columns, IList<ParameterBuilder> conditions, string orderBy = null,
            int? page = null,
            int? pageSize = null);

        T SelectById(object id);

        int Insert(object parameters);

        int Insert(IDictionary<string, object> parameters);

        int Update(object parameters);

        int Update(IDictionary<string, object> parameters);

        int Update(object parameters, object conditions);

        int Update(IDictionary<string, object> parameters, IDictionary<string, object> conditions);

        int Delete(object conditions);

        int Delete(IDictionary<string, object> conditions);

        int DeleteById(object id);

        bool IsIdentityInsert();

        bool IsOpenConnection();

        object IdentCurrent();

        string CreateSelectStatement();
    }
}