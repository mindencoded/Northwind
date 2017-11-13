using System.Threading.Tasks;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;

namespace S3K.RealTimeOnline.GenericDataAccess.CommandHandlers
{
    public class GenericDeleteByIdCommandHandlerAsync<TUnitOfWork, TEntity> : ICommandHandlerAsync<object>
        where TEntity : class where TUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericDeleteByIdCommandHandlerAsync(TUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(object command)
        {
            using (_unitOfWork)
            {
                await _unitOfWork.OpenAsync();
                await _unitOfWork.Repository<TEntity>().DeleteByIdAsync(command);
            }
        }
    }
}