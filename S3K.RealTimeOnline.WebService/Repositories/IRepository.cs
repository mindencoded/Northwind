using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Threading.Tasks;
using S3K.RealTimeOnline.WebService.Tools;

namespace S3K.RealTimeOnline.WebService.Repositories
{
    public interface IRepository
    {
        void SetSqlConnection(SqlConnection sqlConnection);

        void SetSqlTransaction(SqlTransaction sqlTransaction);
    }

    public interface IRepository<T> : IRepository, IDisposable where T : class
    {
        IEnumerable<ExpandoObject> Select(IList<string> columns, string orderBy = null, int? page = null,
            int? pageSize = null);

        Task<IEnumerable<ExpandoObject>> SelectAsync(IList<string> columns, string orderBy = null, int? page = null,
            int? pageSize = null);

        IEnumerable<T> Select(IDictionary<string, object> conditions, string orderBy = null, int? page = null,
            int? pageSize = null);

        Task<IEnumerable<T>> SelectAsync(IDictionary<string, object> conditions, string orderBy = null,
            int? page = null,
            int? pageSize = null);

        IEnumerable<T> Select(IList<ParameterBuilder> conditions, string orderBy = null, int? page = null,
            int? pageSize = null);

        Task<IEnumerable<T>> SelectAsync(IList<ParameterBuilder> conditions, string orderBy = null, int? page = null,
            int? pageSize = null);

        IEnumerable<ExpandoObject> Select(IList<string> columns, IDictionary<string, object> conditions,
            string orderBy = null, int? page = null, int? pageSize = null);

        Task<IEnumerable<ExpandoObject>> SelectAsync(IList<string> columns, IDictionary<string, object> conditions,
            string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<ExpandoObject> Select(IList<string> columns, IList<ParameterBuilder> conditions,
            string orderBy = null,
            int? page = null,
            int? pageSize = null);

        Task<IEnumerable<ExpandoObject>> SelectAsync(IList<string> columns, IList<ParameterBuilder> conditions,
            string orderBy = null,
            int? page = null,
            int? pageSize = null);

        T SelectById(object id);

        Task<T> SelectByIdAsync(object id);

        int Count(IDictionary<string, object> conditions);

        int Count(IList<ParameterBuilder> conditions);

        Task<int> CountAsync(IDictionary<string, object> conditions);

        Task<int> CountAsync(IList<ParameterBuilder> conditions);

        int Insert(object parameters);

        Task<int> InsertAsync(object parameters);

        int Insert(IDictionary<string, object> parameters);

        Task<int> InsertAsync(IDictionary<string, object> parameters);

        int Update(object parameters);

        Task<int> UpdateAsync(object parameters);

        int Update(IDictionary<string, object> parameters);

        Task<int> UpdateAsync(IDictionary<string, object> parameters);

        int Update(object parameters, object conditions);

        Task<int> UpdateAsync(object parameters, object conditions);

        int Update(IDictionary<string, object> parameters, IDictionary<string, object> conditions);

        Task<int> UpdateAsync(IDictionary<string, object> parameters, IDictionary<string, object> conditions);

        int Delete(object conditions);

        Task<int> DeleteAsync(object conditions);

        int Delete(IDictionary<string, object> conditions);

        Task<int> DeleteAsync(IDictionary<string, object> conditions);

        int DeleteById(object id);

        Task<int> DeleteByIdAsync(object id);

        bool IsIdentityInsert();

        Task<bool> IsIdentityInsertAsync();

        bool IsOpenConnection();

        object IdentCurrent();

        Task<object> IdentCurrentAsync();

        string CreateSelectStatement();
    }
}