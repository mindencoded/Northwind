﻿using System.Threading.Tasks;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Commands
{
    public class GenericDeleteByIdCommandHandler<TUnitOfWork> : IGenericCommandHandler
        where TUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericDeleteByIdCommandHandler(TUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Handle<TEntity>(object command) where TEntity : class
        {
            using (_unitOfWork)
            {
                _unitOfWork.Open();
                _unitOfWork.CommandRepository<TEntity>().DeleteById(command);
            }
        }

        public async Task HandleAsync<TEntity>(object command) where TEntity : class
        {
            using (_unitOfWork)
            {
                await _unitOfWork.OpenAsync();
                await _unitOfWork.CommandRepository<TEntity>().DeleteByIdAsync(command);
            }
        }
    }
}