using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Queries
{
    public class
        GenericSelectQueryHandler<TUnitOfWork> : IGenericQueryHandler<GenericSelectQuery, IEnumerable<ExpandoObject>>
        where TUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericSelectQueryHandler(TUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ExpandoObject> Handle<TEntity>(GenericSelectQuery query) where TEntity : class
        {
            using (_unitOfWork)
            {
                _unitOfWork.Open();
                return _unitOfWork.QueryRepository<TEntity>().Select(
                    query.Columns,
                    query.Conditions,
                    query.OrderBy,
                    query.Page,
                    query.PageSize
                );
            }
        }

        public async Task<IEnumerable<ExpandoObject>> HandleAsync<TEntity>(GenericSelectQuery query)
            where TEntity : class
        {
            using (_unitOfWork)
            {
                await _unitOfWork.OpenAsync();
                return await _unitOfWork.QueryRepository<TEntity>().SelectAsync(
                    query.Columns,
                    query.Conditions,
                    query.OrderBy,
                    query.Page,
                    query.PageSize
                );
            }
        }
    }
}