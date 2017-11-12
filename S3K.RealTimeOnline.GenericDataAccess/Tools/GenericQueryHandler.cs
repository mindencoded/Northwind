using System.Collections.Generic;
using System.Dynamic;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.GenericDataAccess.Tools
{
    public class GenericQueryHandler<TEntity, TUnitOfWork> : IQueryHandler<QueryBuilder, IEnumerable<ExpandoObject>>
        where TEntity : Entity
        where TUnitOfWork : IUnitOfWork
    {
        private readonly TUnitOfWork _unitOfWork;

        public GenericQueryHandler(TUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ExpandoObject> Handle(QueryBuilder query)
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
    }
}