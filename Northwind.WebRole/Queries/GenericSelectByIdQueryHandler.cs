using System.Threading.Tasks;
using Northwind.WebRole.Domain;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Queries
{
    public class GenericSelectByIdQueryHandler<TUnitOfWork, TEntity> : IQueryHandler<GenericSelectByIdQuery, Entity>
        where TEntity : Entity
        where TUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericSelectByIdQueryHandler(TUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Entity Handle(GenericSelectByIdQuery query)
        {
            using (_unitOfWork)
            {
                _unitOfWork.Open();
                return _unitOfWork.Repository<TEntity>().SelectById(query.Id);
            }
        }

        public async Task<Entity> HandleAsync(GenericSelectByIdQuery query)
        {
            using (_unitOfWork)
            {
                await _unitOfWork.OpenAsync();
                return await _unitOfWork.Repository<TEntity>().SelectByIdAsync(query.Id);
            }
        }
    }
}