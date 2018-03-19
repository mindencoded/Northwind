﻿using System.Threading.Tasks;
using System.Transactions;
using S3K.RealTimeOnline.GenericDataAccess.Tools;

namespace S3K.RealTimeOnline.Core.Decorators
{
    public class TransactionGenericCommandHandlerDecorator : IGenericCommandHandler
    {
        private readonly IGenericCommandHandler _decorated;

        public TransactionGenericCommandHandlerDecorator(IGenericCommandHandler decorated)
        {
            _decorated = decorated;
        }

        public void Handle<TEntity>(object command) where TEntity : class
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _decorated.Handle<TEntity>(command);
                scope.Complete();
            }
        }

        public async Task HandleAsync<TEntity>(object command) where TEntity : class
        {
            using (TransactionScope scope = new TransactionScope())
            {
                await _decorated.HandleAsync<TEntity>(command);
                scope.Complete();
            }
        }
    }
}