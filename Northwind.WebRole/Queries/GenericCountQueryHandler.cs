using System.Threading.Tasks;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Queries
{
    public class GenericCountQueryHandler<TUnitOfWork> : IGenericQueryHandler<GenericCountQuery, int>
        where TUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericCountQueryHandler(TUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Handle<TEntity>(GenericCountQuery query) where TEntity : class
        {
            using (_unitOfWork)
            {
                _unitOfWork.Open();
                return _unitOfWork.Repository<TEntity>().Count(query.Conditions);
            }
        }

        public async Task<int> HandleAsync<TEntity>(GenericCountQuery query)
            where TEntity : class
        {
            using (_unitOfWork)
            {
                await _unitOfWork.OpenAsync();
                return await _unitOfWork.Repository<TEntity>().CountAsync(query.Conditions);
            }
        }
    }
}