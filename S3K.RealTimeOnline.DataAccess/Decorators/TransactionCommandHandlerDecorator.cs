using System.Transactions;
using S3K.RealTimeOnline.DataAccess.Commands;

namespace S3K.RealTimeOnline.DataAccess.Decorators
{
    public class TransactionCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        private readonly ICommandHandler<TCommand> _decorated;

        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> decorated)
        {
            _decorated = decorated;
        }

        public void Handle(TCommand command)
        {
            using (var scope = new TransactionScope())
            {
                _decorated.Handle(command);
                scope.Complete();
            }
        }
    }
}
