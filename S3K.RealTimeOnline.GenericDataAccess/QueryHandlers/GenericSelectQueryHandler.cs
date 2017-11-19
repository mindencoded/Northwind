using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;

namespace S3K.RealTimeOnline.GenericDataAccess.QueryHandlers
{
    public class
        GenericSelectQueryHandler<TEntity, TUnitOfWork> : IQueryHandler<GenericSelectQuery, IEnumerable<ExpandoObject>>
        where TEntity : class
        where TUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericSelectQueryHandler(TUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ExpandoObject> Handle(GenericSelectQuery query)
        {
            using (_unitOfWork)
            {
                _unitOfWork.Open();
                IEnumerable<ExpandoObject> result = _unitOfWork.Repository<TEntity>().Select(
                    query.Columns,
                    query.Conditions,
                    query.OrderBy,
                    query.Page,
                    query.PageSize
                ) as IEnumerable<ExpandoObject>;
                return result;
            }
        }

        public async Task<IEnumerable<ExpandoObject>> HandleAsync(GenericSelectQuery query)
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