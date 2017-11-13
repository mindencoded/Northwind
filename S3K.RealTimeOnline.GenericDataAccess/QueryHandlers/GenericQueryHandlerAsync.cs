using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;

namespace S3K.RealTimeOnline.GenericDataAccess.QueryHandlers
{
    public class
        GenericQueryHandlerAsync<TEntity, TUnitOfWork> : IQueryHandlerAsync<QueryBuilder, IEnumerable<ExpandoObject>>
        where TEntity : class
        where TUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericQueryHandlerAsync(TUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ExpandoObject>> Handle(QueryBuilder query)
        {
            using (_unitOfWork)
            {
                await _unitOfWork.OpenAsync();
                IEnumerable<ExpandoObject> result = await _unitOfWork.Repository<TEntity>().SelectAsync(
                    query.Columns,
                    query.Conditions,
                    query.OrderBy,
                    query.Page,
                    query.PageSize
                ) as IEnumerable<ExpandoObject>;
                return result;
            }
        }
    }
}