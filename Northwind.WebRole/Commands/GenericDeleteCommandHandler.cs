﻿using System.Threading.Tasks;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Commands
{
    public class GenericDeleteCommandHandler<TUnitOfWork> : IGenericCommandHandler
        where TUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericDeleteCommandHandler(TUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Handle<TEntity>(object command) where TEntity : class
        {
            using (_unitOfWork)
            {
                _unitOfWork.Open();
                _unitOfWork.CommandRepository<TEntity>().Delete(command);
            }
        }

        public async Task HandleAsync<TEntity>(object command) where TEntity : class
        {
            using (_unitOfWork)
            {
                await _unitOfWork.OpenAsync();
                await _unitOfWork.CommandRepository<TEntity>().DeleteAsync(command);
            }
        }
    }
}