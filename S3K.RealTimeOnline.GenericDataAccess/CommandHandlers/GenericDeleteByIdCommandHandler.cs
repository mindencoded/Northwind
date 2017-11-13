using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;

namespace S3K.RealTimeOnline.GenericDataAccess.CommandHandlers
{
    public class GenericDeleteByIdCommandHandler<TUnitOfWork, TEntity> : ICommandHandler<object>
        where TEntity : class where TUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericDeleteByIdCommandHandler(TUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Handle(object command)
        {
            using (_unitOfWork)
            {
                _unitOfWork.Open();
                _unitOfWork.Repository<TEntity>().DeleteById(command);
            }
        }
    }
}