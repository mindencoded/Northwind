using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Commands
{
    public class GenericUpdateCommandHandler<TUnitOfWork> : IGenericCommandHandler
        where TUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericUpdateCommandHandler(TUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Handle<TEntity>(object command) where TEntity : class
        {
            using (_unitOfWork)
            {
                _unitOfWork.Open();
                if (command.GetType().IsGenericType &&
                    command.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    _unitOfWork.Repository<TEntity>().Update((IDictionary<string, object>) command);
                }
                else
                {
                    _unitOfWork.Repository<TEntity>().Update(command);
                }
            }
        }

        public async Task HandleAsync<TEntity>(object command) where TEntity : class
        {
            using (_unitOfWork)
            {
                await _unitOfWork.OpenAsync();
                if (command.GetType().IsGenericType &&
                    command.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    await _unitOfWork.Repository<TEntity>().UpdateAsync((IDictionary<string, object>) command);
                }
                else
                {
                    await _unitOfWork.Repository<TEntity>().UpdateAsync(command);
                }
            }
        }
    }
}