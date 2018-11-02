using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.WebRole.Repositories
{
    public interface ICommandRepository<T> : IQueryRepository<T> where T : class
    {
        T SelectById(object id);

        Task<T> SelectByIdAsync(object id);

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
    }
}
