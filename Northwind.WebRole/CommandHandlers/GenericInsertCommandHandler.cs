using System.Threading.Tasks;
using Northwind.WebRole.Tools;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.CommandHandlers
{
    public class GenericInsertCommandHandler<TUnitOfWork> : IGenericCommandHandler
        where TUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericInsertCommandHandler(TUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Handle<TEntity>(object command) where TEntity : class
        {
            using (_unitOfWork)
            {
                _unitOfWork.Open();
                _unitOfWork.Repository<TEntity>().Insert(command);
            }
        }

        public async Task HandleAsync<TEntity>(object command) where TEntity : class
        {
            using (_unitOfWork)
            {
                await _unitOfWork.OpenAsync();
                await _unitOfWork.Repository<TEntity>().InsertAsync(command);
            }
        }
    }
}