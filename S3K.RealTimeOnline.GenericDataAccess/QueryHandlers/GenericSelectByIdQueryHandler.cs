using System.Threading.Tasks;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.GenericDataAccess.QueryHandlers
{
    public class GenericSelectByIdQueryHandler<TUnitOfWork, TEntity> : IQueryHandler<GenericSelectByIdQuery<TEntity>, TEntity>
        where TEntity : Entity
        where TUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericSelectByIdQueryHandler(TUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TEntity Handle(GenericSelectByIdQuery<TEntity> query)
        {
            using (_unitOfWork)
            {
                _unitOfWork.Open();
                object id = query.Id;
                return _unitOfWork.Repository<TEntity>().SelectById(id);
            }
        }

        public async Task<TEntity> HandleAsync(GenericSelectByIdQuery<TEntity> query)
        {
            using (_unitOfWork)
            {
                await _unitOfWork.OpenAsync();
                return await _unitOfWork.Repository<TEntity>().SelectByIdAsync(query.Id);
            }
        }
    }
}
