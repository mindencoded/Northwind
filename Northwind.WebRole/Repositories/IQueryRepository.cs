
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Repositories
{
    public interface IQueryRepository<T> : IRepository
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

        int Count(IDictionary<string, object> conditions);

        int Count(IList<ParameterBuilder> conditions);

        Task<int> CountAsync(IDictionary<string, object> conditions);

        Task<int> CountAsync(IList<ParameterBuilder> conditions);
    }
}
